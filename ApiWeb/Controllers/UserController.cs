using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ApiWeb.Models;
using ApiWeb.Service.TokenService;
using Newtonsoft.Json;
using ApiWeb.Respositories.UserRepository;
using ApiWeb.Repositories.TokenRepository;
using ApiWeb.Service.oAuthService;
using System.IdentityModel.Tokens.Jwt;
using GoogleOAuth;
using Logic.Extentions;
using EfficacySend.Utilities;
using EfficacySend.Models;
using static ApiWeb.Constants.AppConsts;
using Logic.Efficacy;
using AppContext.Interface;
using AppContext.Extentions;
using ApiWeb.Service.EnvironmentService;



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
        private readonly GoogleApi googleApi;
        private readonly IConfiguration configuration;
        private readonly IApplicationContext applicationContext;
        private readonly IEnvironmentService environmentService;
       
        //private readonly ILogger<UserController> logger;


        public UserController(IApplicationContext applicationContext, IEnvironmentService environmentService, IToken token, IUserService userService, ITokenService tokenService, GoogleApi googleApi, IConfiguration configuration)
        {
            this.token = token;
            this.userService = userService;
            this.tokenService = tokenService;
            this.googleApi = googleApi;
            this.configuration = configuration;
            this.applicationContext = applicationContext;
            this.environmentService = environmentService;
        }

        //Postman > Body > JSON
        [AllowAnonymous]
        [HttpPost("RegisterAuthEmail")]
        public async Task<IActionResult> RegisterAuthEmail(User u)
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

                await userService.Register(u);

                var user = await userService.AuthenticateViaEmail(u);

                string Token = token.GenerateToken(user.UserId, user.Roles, TokenType.Refresh.ToString());
                await tokenService.SaveToken(user.UserId, Token, TokenType.Refresh.ToString());

                var tokens = new Tokens
                {
                    AccessToken = token.GenerateToken(user.UserId, user.Roles, TokenType.Access.ToString()),
                    RefreshToken = Token,
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
                //logger.LogDebug()
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

        [AllowAnonymous]
        [HttpPost("RegisterAuthUserName")]
        public async Task<IActionResult> RegisterAuthUserName(User u)
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

                await userService.Register(u);

                var user = await userService.AuthenticateViaUserName(u);

                string Token = token.GenerateToken(user.UserId, user.Roles, TokenType.Refresh.ToString());
                await tokenService.SaveToken(user.UserId, Token, TokenType.Refresh.ToString());

                var tokens = new Tokens
                {
                    AccessToken = token.GenerateToken(user.UserId, user.Roles, TokenType.Access.ToString()),
                    RefreshToken = Token,
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
                //logger.LogDebug()
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



        [HttpPost("LoginViaUserName")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginViaUserName(User u)
        {
            environmentService.GetConfigurationValue("");
            try
            {
                var user = await userService.AuthenticateViaUserName(u);
                if (user != null)
                {
                    string RefreshToken = token.GenerateToken(user.UserId, user.Roles, TokenType.Refresh.ToString());
                    await tokenService.SaveToken(user.UserId, RefreshToken, TokenType.Refresh.ToString());

                    var User = new User
                    {
                        EmailAddress = u.EmailAddress,
                        UserId = user.UserId,
                        Roles = user.Roles
                    };

                    var Tokens = new Tokens
                    {
                        AccessToken = token.GenerateToken(user.UserId, user.Roles, TokenType.Access.ToString()),
                        RefreshToken = RefreshToken,
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

        [HttpPost("LoginViaEmailAddress")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginViaEmailAddress(User u)
        {
            try
            {
                var user = await userService.AuthenticateViaEmail(u);
                if (user != null)
                {
                    string RefreshToken = token.GenerateToken(user.UserId, user.Roles, TokenType.Refresh.ToString());
                    await tokenService.SaveToken(user.UserId, RefreshToken, TokenType.Refresh.ToString());

                    var User = new User
                    {
                        EmailAddress = u.EmailAddress,
                        UserId = user.UserId,
                        Roles = user.Roles
                    };

                    var Tokens = new Tokens
                    {
                        AccessToken = token.GenerateToken(user.UserId, user.Roles, TokenType.Access.ToString()),
                        RefreshToken = RefreshToken,
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
            string Token = token.GenerateToken(loginResponse.User.UserId, loginResponse.User.Roles, TokenType.Refresh.ToString());
            try
            {
                var IsTokenValid = await tokenService.IsTokenValid(loginResponse.Tokens.RefreshToken);

                if (IsTokenValid)
                {
                    var IsTokenExpired = token.IsTokenExpired(loginResponse.Tokens.RefreshToken);
                    if (IsTokenExpired)
                    {
                        await tokenService.SaveToken(loginResponse.User.UserId, Token, TokenType.Refresh.ToString());
                    }
                    var Tokens = new Tokens
                    {
                        IsRefreshTokenExpired = IsTokenExpired,
                        AccessToken = token.GenerateToken(loginResponse.User.UserId, loginResponse.User.Roles, TokenType.Access.ToString()),
                        RefreshToken = Token,
                    };
                    var response = new LoginResponse
                    {
                        Tokens = Tokens,
                    };
                    return response;
                }
            }
            catch (Exception)
            {

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

        [HttpPost("Forgot")]
        [AllowAnonymous]
        public async Task<IActionResult> Forgot(User u)
        {
            var GoogleOAuthConfig = configuration.GetSection("GoogleOAuth").Get<GoogleOAuthConfig>();
            try
            {
                if (await userService.IsUserExists(u.EmailAddress))
                {
                    Guid UserId = await userService.GetUserIdOnEmail(u.EmailAddress);
                    var payloadString = token.GenerateToken(UserId, "User", "Reset");
                    await tokenService.SaveToken(UserId, payloadString, TokenType.Reset.ToString());

                    var sendEmail = new Email
                    {
                        FromEmail = "hans.profession@gmail.com",
                        FromName = "Hans",
                        ToEmail = u.EmailAddress,
                        Subject = "Subject",
                        PlainEmail = "Hi",
                        HtmlEmail = $"<p>{$"{GoogleOAuthConfig.WebAppUri}/resetpassword?payload={payloadString}"}</p>"
                    };
                    await userService.SendForgetPasswordEmail(sendEmail);
                    
                }
            }
            catch (Exception) { }

            return Ok(new ApiResponse
            {
                Payload = null,
                Notify = new Notify
                {
                    Message = "If your email address is registered with us, we will send a link in your email to reset password",
                    Success = true
                },
                Success = true
            });
        }

        [HttpPost("ResetTokenCheck")]
        [AllowAnonymous]
        public IActionResult ResetTokenCheck(TheToken intoken)
        {
            try { 
            if (token.IsTokenExpired(intoken.Token))
            {
                var apiResponse = new ApiResponse
                {
                    Payload = null,
                    Success = false,
                    Notify = new Notify
                    {
                        Success = false,
                        Message = $"Token has expired, please try again."
                    }

                };
                return Ok(apiResponse);
            }
            }
            catch(Exception)
            {
                var apiResponse = new ApiResponse
                {
                    Payload = null,
                    Success = false,
                    Notify = new Notify
                    {
                        Success = false,
                        Message = $"Token has been tampered with."
                    }

                };
                return Ok(apiResponse);
            }

            return Ok();
        }

        [HttpPost("ResetPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(LoginResponse lr)
        {
            try { 
            if (token.IsTokenExpired(lr.Tokens.AccessToken))
            {
                var apiResponse = new ApiResponse
                {
                    Payload = null,
                    Success = false,
                    Notify = new Notify
                    {
                        Success = false,
                        Message = $"Token has expired, please try again."
                    }

                };
                return Ok(apiResponse);
            }
                else
                {
                    try
                    {
                        lr.User.UserId = token.GetUserId();
                        await userService.UpdatePassword(lr.User);
                    }
                    catch (Exception )
                    {

                    }
                    var apiResponse = new ApiResponse
                    {
                        Payload = null,
                        Success = true,
                        Notify = new Notify
                        {
                            Success = true,
                            Message = $"Your password has been changed, please login again to continue"
                        }

                    };
                    return Ok(apiResponse);
                }
            }
           catch (Exception)
            {
                var apiResponse = new ApiResponse
                {
                    Payload = null,
                    Success = false,
                    Notify = new Notify
                    {
                        Success = false,
                        Message = $"Token has been tampered with."
                    }

                };
                return Ok(apiResponse);
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

            string RefreshToken = token.GenerateRefreshToken(34);

            //await tokenService.SaveRefreshToken(user.UserId, RefreshToken);

            var result = await googleApi.HttpPostCall<GoogleToken>(query, "application/x-www-form-urlencoded");

            var handler = new JwtSecurityTokenHandler();

            var googletoken = handler.ReadJwtToken(result.Id_Token);

            var tokens = new Tokens
            {
                AccessToken = token.GenerateToken(Guid.NewGuid(), "User", TokenType.Access.ToString()),
                RefreshToken = token.GenerateToken(Guid.NewGuid(), "User", TokenType.Refresh.ToString()),
            };

            var user = new User
            {
                EmailAddress = googletoken.Claims.ElementAt(4).Value,
                FirstName = googletoken.Claims.ElementAt(9).Value,
                LastName = googletoken.Claims.ElementAt(10).Value,
                PhotoString = googletoken.Claims.ElementAt(8).Value,
            };

            var Payload = new LoginResponse
            {
                User = user,
                Tokens = tokens,
            };

            string payloadString = JsonConvert.SerializeObject(Payload);
            return Redirect($"{GoogleOAuthConfig.WebAppUri}/login?payload={payloadString}");
        }

        [HttpGet("GetUserInfoId")]
        [AllowAnonymous]
        public IActionResult GetUserInfo(string id)
        {
            string strName = "authorised Button Result";
            string result = strName.ChangeFirstLetterCase();
            //string result = Extentions.StringHelper.ChangeFirstLetterCase(strName);
            return Ok(result);
        }

        [HttpGet("GetUserInfo")]
        //[ApiAuthoriseSecurity(10)]
        //[Authorize]
        public IActionResult GetUserInfo()
        {
            string strName = "authorised Button Result";
            string result = strName.ChangeFirstLetterCase();
            //string result = Extentions.StringHelper.ChangeFirstLetterCase(strName);
            return Ok(result);
        }
        
        [HttpGet("Auth")]
        public async Task<IActionResult> Auth()
        {
            var userService = applicationContext.Create<IUserService>();
            var hashingService = applicationContext.HashingService;
            var hashed = hashingService.PasswordHash("asdfsd");
            var exists = await userService.IsUserExists("hansdeep.singh@hotmail.com");
            string strName = $"authorised Button Result : {exists} hashed {hashed}";
            string result = strName.ChangeFirstLetterCase();
            //string result = Extentions.StringHelper.ChangeFirstLetterCase(strName);
            return Ok(result);
        }

        [HttpPost("LogOut")]
        public async Task<IActionResult> LogOut()
        {
            try
            {
                await token.DeactivateCurrentAsync();
                return Ok(new ApiResponse
                {
                    Payload = null,
                    Success = true,
                    Notify = new Notify
                    {
                        Success = true,
                        Message = "**Congratulations you have sucessfully logged out. (>> Token Cancelled <<)**"
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
    }
}
