using System;
using System.Security.Cryptography;
using System.Text;
using Konscious.Security.Cryptography;

namespace Alcatraz.DTO.Helpers
{
#if NET5_0_OR_GREATER
        public static class SecurePasswordHasher
        {
            private const int SaltSize = 16;
            private const int HashSize = 32;

            private const int MemorySize = 65536;
            private const int Iterations = 4;
            private const int DegreeOfParallelism = 2;

            private const int FormatVersion = 1;

            private static byte[] HashPassword(string password, byte[] salt, int memory, int iterations,
                int parallelism)
            {
                var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password));
                argon2.Salt = salt;
                argon2.MemorySize = memory;
                argon2.Iterations = iterations;
                argon2.DegreeOfParallelism = parallelism;

                return argon2.GetBytes(HashSize);
            }

            public static string Hash(string password)
            {
                byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
                byte[] hash = HashPassword(password, salt, MemorySize, Iterations, DegreeOfParallelism);

                string saltB64 = Convert.ToBase64String(salt);
                string hashB64 = Convert.ToBase64String(hash);

                return
                    $"$argon2id$v={FormatVersion}$m={MemorySize},t={Iterations},p={DegreeOfParallelism}${saltB64}${hashB64}";
            }

            public static bool Verify(string password, string stored)
            {
                if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(stored))
                    return false;

                string[] parts = stored.Split('$', StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length != 5)
                    return false;

                if (parts[0] != "argon2id")
                    return false;

                string versionPart = parts[1];
                int version = int.Parse(versionPart.Split('=')[1]);
                if (version != FormatVersion)
                    return false;

                string[] paramParts = parts[2].Split(',');
                int memory = int.Parse(paramParts[0].Split('=')[1]);
                int iterations = int.Parse(paramParts[1].Split('=')[1]);
                int parallelism = int.Parse(paramParts[2].Split('=')[1]);

                byte[] salt = Convert.FromBase64String(parts[3]);
                byte[] expectedHash = Convert.FromBase64String(parts[4]);

                byte[] actualHash = HashPassword(password, salt, memory, iterations, parallelism);

                return CryptographicOperations.FixedTimeEquals(actualHash, expectedHash);
            }
        }
#endif
}