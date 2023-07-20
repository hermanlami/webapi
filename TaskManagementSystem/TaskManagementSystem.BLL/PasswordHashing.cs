using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementSystem.BLL
{
    public static class PasswordHashing
    {
        /// <summary>
        /// Ben hash-imin e passwordit te perdoruesit.
        /// </summary>
        /// <param name="password">Password-i i perdoruesit.</param>
        /// <param name="salt">Array me byte qe sherben per ta bere password-in e hash-uar unik.</param>
        /// <returns>Password-in e hash-uar.</returns>
        public static string HashPasword(string password, out byte[] salt)
        {
            const int keySize = 64;
            const int iterations = 350000;
            HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;

            salt = RandomNumberGenerator.GetBytes(keySize);
            var hash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(password),
                salt,
                iterations,
                hashAlgorithm,
                keySize);
            return Convert.ToHexString(hash);
        }
        /// <summary>
        /// Kontrollon nese passwordi i dhene eshte i sakte duke krahasuar password-in e hash-uar me rezultatin
        /// qe jep hash-imi i password-it te dhene duke perdorur te nejtin salt.
        /// </summary>
        /// <param name="password">Password-i i dhene.</param>
        /// <param name="hash">Password-i hashuar i perdoruesit.</param>
        /// <param name="salt">Salt i perdorur per passwordi e perdoruesit.</param>
        /// <returns>True nese rezulatet perputhen, perndryshe false.</returns>
        public static bool VerifyPassword(string password, string hash, byte[] salt)
        {
            const int keySize = 64;
            const int iterations = 350000;
            HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;

            var hashToCompare = Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, hashAlgorithm, keySize);

            return CryptographicOperations.FixedTimeEquals(hashToCompare, Convert.FromHexString(hash));
        }
    }
}
