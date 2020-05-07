using System;
using System.Reflection;
using System.Security.Cryptography;
using Xunit;
using CoviIDApiCore.V1.Services;
using Microsoft.Extensions.Configuration;

namespace test
{
    public class Crypto
    {
        private IConfiguration configuration;
        private CryptoService crypto;
        private MethodInfo encrypt;
        private MethodInfo decrypt;

        private RNGCryptoServiceProvider rng;

        public Crypto()
        {
            configuration = new ConfigurationBuilder().Build();
            ConfigurationBinder.Bind(configuration, new { SecretKey = "server_secret_key", });
            crypto = new CryptoService(configuration);
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
    }
}
