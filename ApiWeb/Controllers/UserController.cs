using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ApiWeb.Models;
using ApiWeb.Extentions;
using ApiWeb.Service.TokenService;
using Newtonsoft.Json;
using ApiWeb.Respositories.UserRepository;
using ApiWeb.Repositories.TokenRepository;
using ApiWeb.Service.oAuthService;
using System.IdentityModel.Tokens.Jwt;
using GoogleOAuth;



//https://www.youtube.com/watch?v=7tgLuJ__ZKU
//https://www.youtube.com/watch?v=rPDvBrlTt2Q&ab_channel=AristotelisPitaridis - Custom Authorise Attribute
namespace ApiWeb.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]")]
    //When making a call make sure to use "api" in the URL because it is defined in the Route

    [Authorize]
    public class UserController : Controller
    {
        private readonly IToken token;
        private readonly IUserService userService;
        private readonly ITokenService tokenService;
        private readonly AccessToken accessToken;
        private readonly RefreshToken refreshToken;
        private readonly GoogleApi googleApi;
        private readonly IConfiguration configuration;



        //https://cmatskas.com/-net-password-hashing-using-pbkdf2/
        public UserController(IToken token, IUserService userService, ITokenService tokenService, AccessToken accessToken, RefreshToken refreshToken, GoogleApi googleApi, IConfiguration configuration)
        {
            this.token = token;
            this.userService = userService;
            this.accessToken = accessToken;
            this.refreshToken = refreshToken;
            this.tokenService = tokenService;
            this.googleApi = googleApi;
            this.configuration = configuration;
        }

        //Postman > Body > JSON
        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> Register(User u)
        {
            try
            {
                if (await userService.IsUserExists(u.EmailAddress))
                {
                    return Ok(new ApiResponse
                    {
                        Payload = null,
                        Success = false,
                        Notify = new Notify
                        {
                            Success = false,
                            Message = "This email address is already registered"
                        }
                    });
                }
                u.Roles = "User";
                string RefreshToken = refreshToken.GenerateSimpleRefreshToken();
                await userService.Register(u);

                var user = await userService.Authenticate(u);
                await tokenService.SaveRefreshToken(user.UserId, RefreshToken);

                var tokens = new Tokens
                {
                    AccessToken = accessToken.GenerateAccessToken(user.EmailAddress, user.UserId, user.Roles),
                    RefreshToken = refreshToken.GenerateRefreshToken(user.EmailAddress, user.UserId, user.Roles),
                    SimpleRefreshToken = RefreshToken,
                };

                var userResp = new User
                {
                    UserId = user.UserId,
                    EmailAddress = u.EmailAddress,
                    Roles = user.Roles
                };

                var Payload = new LoginResponse
                {
                    User = userResp,
                    Tokens = tokens
                };
                var notify = new ApiResponse
                {
                    Payload = JsonConvert.SerializeObject(Payload),
                    Success = true,
                    Notify = new Notify
                    {
                        Success = true,
                        Message = "**Congratulations you have sucessfully registered**"
                    }

                };
                return Ok(notify);
            }
            catch (Exception ex)
            {
                var notify = new ApiResponse
                {
                    Payload = null,
                    Success = true,
                    Notify = new Notify
                    {
                        Success = false,
                        Message = $"**Something bad happened, Exception{ex.Message}**"
                    }

                };
                return Ok(notify);
            }
        }


        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(User u)
        {
            try
            {
                var user = await userService.Authenticate(u);
                if (user != null)
                {
                    string RefreshToken = refreshToken.GenerateSimpleRefreshToken();
                    await tokenService.SaveRefreshToken(user.UserId, RefreshToken);

                    var User = new User
                    {
                        EmailAddress = u.EmailAddress,
                        UserId = user.UserId,
                        Roles = user.Roles
                    };

                    var Tokens = new Tokens
                    {
                        AccessToken = accessToken.GenerateAccessToken(user.EmailAddress, user.UserId, user.Roles),
                        RefreshToken = refreshToken.GenerateRefreshToken(user.EmailAddress, user.UserId, user.Roles),
                        SimpleRefreshToken = RefreshToken,
                    };

                    var Payload = new LoginResponse
                    {
                        User = User,
                        Tokens = Tokens
                    };
                    var apiResponse = new ApiResponse
                    {
                        Success = true,
                        Payload = JsonConvert.SerializeObject(Payload),
                        Notify = new Notify
                        {
                            Success = true,
                            Message = "**Congratulations**, you have successfully logged in"
                        }

                    };
                    return Ok(apiResponse);
                }
                else
                {
                    var apiResponse = new ApiResponse
                    {
                        Payload = null,
                        Success = false,
                        Notify = new Notify
                        {
                            Success = false,
                            Message = "Login failed, please check your username or password"
                        }

                    };
                    return Ok(apiResponse);
                }
            }
            catch (Exception ex)
            {
                var apiResponse = new ApiResponse
                {
                    Payload = null,
                    Success = false,
                    Notify = new Notify
                    {
                        Success = false,
                        Message = $"**Something bad happened, Exception{ex.Message}**"
                    }

                };
                return Ok(apiResponse);
            }

        }
        [HttpPost("AccessToken")]
        [AllowAnonymous]
        public async Task<LoginResponse> AccessToken(LoginResponse loginResponse)
        {
            string newToken = refreshToken.GenerateSimpleRefreshToken();
            var IsTokenValid = await tokenService.IsTokenValid(loginResponse.Tokens.SimpleRefreshToken);
          
            if (IsTokenValid)
            {
                var IsTokenExpired = await tokenService.IsTokenExpired(loginResponse.Tokens.SimpleRefreshToken);
                if (IsTokenExpired)
                {
                    await tokenService.SaveRefreshToken(loginResponse.User.UserId, newToken);
                }
                var tokens = new Tokens
                {
                    IsRefreshTokenExpired = IsTokenExpired,
                    AccessToken = accessToken.GenerateAccessToken(loginResponse.User.EmailAddress, loginResponse.User.UserId, loginResponse.User.Roles),
                    SimpleRefreshToken = newToken
                };
                var response = new LoginResponse
                {
                    Tokens = tokens,
                };
                return response;
            }
            return new LoginResponse
            {
                Tokens = null
            };
        }

        [HttpGet("IsEmailRegistered/{email}")]
        [AllowAnonymous]
        public ActionResult IsEmailRegistered(string email)
        {
            return Ok(true);
        }


        [HttpPost("LogOut")]
        [AllowAnonymous]
        public async Task<IActionResult> LogOut(string accesstoken)
        {
            try
            {
                // For middle ware.
                // await token.DeactivateCurrentAsync();

                return Ok(new ApiResponse
                {
                    Payload = null,
                    Success = true,
                    Notify = new Notify
                    {
                        Success = true,
                        Message = "**Congratulations you have sucessfully logged out**"
                    }
                });
            }
            catch (Exception e)
            {
                var apiResponse = new ApiResponse
                {
                    Payload = null,
                    Success = false,
                    Notify = new Notify
                    {
                        Success = false,
                        Message = $"Exception occured {e.Message}"
                    }

                };
                return Ok(apiResponse);
            }
            finally
            {
                Redirect("https://www.google.com/accounts/Logout");
            }
        }

        [HttpGet("GetGoogleOAuthUri")]
        [AllowAnonymous]
        public async Task<IActionResult> GetGoogleOAuthUri()
        {

            var GoogleOAuthConfig = configuration.GetSection("GoogleOAuth").Get<GoogleOAuthConfig>();
            string content = "application/json";
            var DocuConfig = await googleApi.HttpGetCall<OpenIdConnectConfig>(GoogleOAuthConfig.Discovery, content);
            string query = DocuConfig.Authorization_Endpoint + UriService.GetAuthUri(GoogleOAuthConfig.ClientId, GoogleOAuthConfig.RedirectUri);
            return Ok(query);
        }



        [HttpGet("GoogleRedirect")]
        [AllowAnonymous]
        public async Task<IActionResult> GoogleRedirect(string code)
        {
            var GoogleOAuthConfig = configuration.GetSection("GoogleOAuth").Get<GoogleOAuthConfig>();

            string query = UriService.GetAccessUri(GoogleOAuthConfig.ClientId, code, GoogleOAuthConfig.Secret, GoogleOAuthConfig.RedirectUri);

            string RefreshToken = refreshToken.GenerateSimpleRefreshToken();

            //await tokenService.SaveRefreshToken(user.UserId, RefreshToken);

            var result = await googleApi.HttpPostCall<GoogleToken>(query, "application/x-www-form-urlencoded");

            var handler = new JwtSecurityTokenHandler();

            var token = handler.ReadJwtToken(result.Id_Token);

            var tokens = new Tokens
            {
                AccessToken = accessToken.GenerateAccessToken(token.Claims.ElementAt(7).Value, Guid.NewGuid(), "User"),
                SimpleRefreshToken = RefreshToken,
            };

            var user = new User
            {
                EmailAddress = token.Claims.ElementAt(4).Value,
                FirstName = token.Claims.ElementAt(9).Value,
                LastName = token.Claims.ElementAt(10).Value,
                PhotoString = token.Claims.ElementAt(8).Value,
            };

            var Payload = new LoginResponse
            {
                User = user,
                Tokens = tokens,
            };

            string payloadString = JsonConvert.SerializeObject(Payload);


            return Redirect($"{GoogleOAuthConfig.WebAppUri}/login?payload={payloadString}");
        }

        [HttpGet("GetUserInfo")]
        [AllowAnonymous]
        public IActionResult GetUserInfo(string id)
        {
            string strName = "authorised Button Result";
            string result = strName.ChangeFirstLetterCase();
            //string result = Extentions.StringHelper.ChangeFirstLetterCase(strName);
            return Ok(result);
        }

        [HttpGet("GetUserInfo")]
        [AllowAnonymous]
        public IActionResult GetUserInfo()
        {
            string strName = "authorised Button Result";
            string result = strName.ChangeFirstLetterCase();
            //string result = Extentions.StringHelper.ChangeFirstLetterCase(strName);
            return Ok(result);
        }

        [HttpGet("Auth")]
        public IActionResult Auth()
        {
            string strName = "authorised Button Result";
            string result = strName.ChangeFirstLetterCase();
            //string result = Extentions.StringHelper.ChangeFirstLetterCase(strName);
            Console.WriteLine(result);
            return Ok(result);
        }
    }
}
