using System.Security.Cryptography;
using System.Text;

namespace MarcoPortefolioServer.Functions.v1.lib.server
{
    public class TokenValidator
    {
        private readonly Dictionary<string, string> _tokens;

        public TokenValidator(IConfiguration config)
        {
            _tokens = config.GetSection("Tokens").Get<Dictionary<string, string>>()
                      ?? new Dictionary<string, string>();
        }

        public bool IsValid(string token)
        {
            return _tokens.Values.Contains(token);
        }

        public bool IsValid(string key, string token)
        {
            return _tokens.ContainsKey(key) && _tokens[key] == token;
        }
        public static string GenerateRandomString(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            StringBuilder sb = new StringBuilder(length);
            byte[] buffer = new byte[length];

            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(buffer);

            for (int i = 0; i < length; i++)
            {
                int index = buffer[i] % chars.Length;
                sb.Append(chars[index]);
            }

            return sb.ToString();
        }
    }
}