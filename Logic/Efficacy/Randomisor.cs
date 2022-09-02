using System.Security.Cryptography;


namespace Logic.Efficacy
{
    public static class Randomisor
    {
        public static string GenerateRandomCryptoString(int RandomStringLength)
        {
            var randomNumber = new byte[RandomStringLength];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return (Convert.ToBase64String(randomNumber));
        }
    }
}
