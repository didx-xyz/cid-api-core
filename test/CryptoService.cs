using System;
using System.Reflection;
using System.Security.Cryptography;
using Xunit;
using CoviIDApiCore.V1.Services;
using CoviIDApiCore.V1.Attributes;
using Microsoft.Extensions.Configuration;
using Moq;

namespace test
{
    public class Example : IEquatable<Example>
    {
        public string NotEncrypted { get; set; }
        [Encrypted]
        public string Encrypted { get; set; }
        [Encrypted(true)]
        public string EncryptedServerManaged { get; set; }

        public bool Equals (Example other)
        {
            return this.NotEncrypted == other.NotEncrypted
                && this.Encrypted == other.Encrypted
                && this.EncryptedServerManaged == other.EncryptedServerManaged;
        }

        public Example Clone ()
        {
            return new Example
            {
                NotEncrypted = this.NotEncrypted,
                Encrypted = this.Encrypted,
                EncryptedServerManaged = this.EncryptedServerManaged,
            };
        }
    }

    public class Crypto
    {
        private CryptoService crypto;
        private MethodInfo encrypt;
        private MethodInfo decrypt;

        private RNGCryptoServiceProvider rng;

        public Crypto()
        {
            var mock = new Mock<IConfiguration>();
            mock.Setup(c => c[It.IsAny<string>()]).Returns("00000000000000000000000000000000");
            crypto = new CryptoService(mock.Object);

            encrypt = typeof(CryptoService).GetMethod("Encrypt", BindingFlags.NonPublic | BindingFlags.Instance);
            decrypt = typeof(CryptoService).GetMethod("Decrypt", BindingFlags.NonPublic | BindingFlags.Instance);

            rng = new RNGCryptoServiceProvider();
        }

        [Fact]
        public void EncryptDecrypt_PrivateMethods()
        {
            string originalPlainText = "Foo bar baz";

            byte[] key = new byte[32]; // 256 bits
            rng.GetBytes(key);
            string keyString = Convert.ToBase64String(key);

            var cipherText = (string)encrypt.Invoke(crypto, new object[]{originalPlainText, keyString});
            var plainText = (string)decrypt.Invoke(crypto, new object[]{cipherText, keyString});

            Assert.Equal(originalPlainText, plainText);
        }

        [Fact]
        public void EncryptDecrypt_AsServer()
        {
            Example original = new Example
            {
                NotEncrypted = "not encrypted",
                Encrypted = "encrypted",
                EncryptedServerManaged = "encrypted server managed",
            };
            Example example = original.Clone();

            crypto.EncryptAsServer(example);
            Assert.Equal(example.NotEncrypted, original.NotEncrypted);
            Assert.Equal(example.Encrypted, original.Encrypted);
            Assert.NotEqual(example.EncryptedServerManaged, original.EncryptedServerManaged);

            crypto.DecryptAsServer(example);
            Assert.Equal(example, original);
        }

        [Fact]
        public void EncryptDecrypt_AsUser()
        {
            byte[] key = new byte[32]; // 256 bits
            rng.GetBytes(key);
            string keyString = Convert.ToBase64String(key);

            Example original = new Example
            {
                NotEncrypted = "not encrypted",
                Encrypted = "encrypted",
                EncryptedServerManaged = "encrypted server managed",
            };
            Example example = original.Clone();

            crypto.EncryptAsUser(example, keyString);
            Assert.Equal(example.NotEncrypted, original.NotEncrypted);
            Assert.NotEqual(example.Encrypted, original.Encrypted);
            Assert.Equal(example.EncryptedServerManaged, original.EncryptedServerManaged);

            crypto.DecryptAsUser(example, keyString);
            Assert.Equal(example, original);
        }
    }
}
