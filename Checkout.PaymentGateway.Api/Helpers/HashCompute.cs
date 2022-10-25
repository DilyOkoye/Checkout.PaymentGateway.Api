using System.Security.Cryptography;
using System.Text;

namespace Checkout.PaymentGateway.Api.Helpers
{
    /// <summary>Provides helper functions regarding the use of hashes.</summary>
    public static class HashCompute
    {
        /// <summary>Used to generate a SHA256 hash with the given parameters.</summary>
        /// <param name="message">The message to be used in the generation of the hash.</param>
        /// <param name="dateTimeStamp">The date time stamp to be used in the generation of the hash.</param>
        /// <param name="secret">The secret to be used in the generation of the hash.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public static string GenerateSha256Hash(string message, string dateTimeStamp, string secret)
        {
            byte[] hash;

            using (var sha256 = SHA256.Create())
            {
                hash = sha256.ComputeHash(Encoding.UTF8.GetBytes($"{message}{dateTimeStamp}{secret}"));
            }

            var stringBuilder = new StringBuilder();

            foreach (var integer in hash)
            {
                stringBuilder.Append(integer.ToString("x2"));
            }

            return stringBuilder.ToString();
        }
    }
}
