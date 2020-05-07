using System;
using System.Reflection;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoviIDApiCore.V1.Interfaces.Services;
using CoviIDApiCore.V1.Attributes;
using Microsoft.Extensions.Configuration;

namespace CoviIDApiCore.V1.Services
{
    enum TransformDirection { ToCipher, FromCipher };

    public class CryptoService : ICryptoService
    {
        private readonly string serverKey;

        public CryptoService(IConfiguration configuration)
        {
            serverKey = configuration.GetValue<string>("ServerKey");
        }


        public Task<string> GenerateEncryptedSecretKey()
        {
            // TODO
            return Task.FromResult<string>("totally_encrypted_secret_key");
        }

        public void EncryptAsServer<T>(T obj)
        {
            TransformObj(TransformDirection.ToCipher, obj, serverKey, true);
        }

        public void DecryptAsServer<T>(T obj)
        {
            TransformObj(TransformDirection.FromCipher, obj, serverKey, true);
        }

        public void EncryptAsUser<T>(T obj, string encryptedSecretKey)
        {
            TransformObj(TransformDirection.ToCipher, obj, encryptedSecretKey, false);
        }

        public void DecryptAsUser<T>(T obj, string encryptedSecretKey)
        {
            TransformObj(TransformDirection.FromCipher, obj, encryptedSecretKey, false);
        }

        private void TransformObj<T>(TransformDirection direction, T obj, string key, bool serverManaged = false)
        {
            foreach (PropertyInfo prop in GetEncryptedProperties(obj, serverManaged))
            {
                // Weird things would happen here if prop.PropertyType is not a
                // string. We should probably enforce that.
                var before = prop.GetValue(obj) as string;
                string after;
                if (direction == TransformDirection.ToCipher) {
                    after = Encrypt(before, key);
                } else {
                    after = Decrypt(before, key);
                }
                prop.SetValue(obj, after);
            }
        }

        private List<PropertyInfo> GetEncryptedProperties<T>(T obj, bool serverManaged = false)
        {
            List<PropertyInfo> encryptedProps = new List<PropertyInfo>();

            PropertyInfo[] allProps = obj.GetType().GetProperties();
            foreach (PropertyInfo prop in allProps)
            {
                object[] attrs = prop.GetCustomAttributes(true);
                foreach (object attr in attrs)
                {
                    Encrypted encrypted = attr as Encrypted;
                    if (encrypted != null && encrypted.serverManaged == serverManaged)
                    {
                        encryptedProps.Add(prop);
                    }
                }
            }

            return encryptedProps;
        }

        private string Encrypt (string plainText, string key)
        {
            // TODO
            return plainText;
        }

        private string Decrypt (string cipherText, string key)
        {
            // TODO
            return cipherText;
        }
    }
}
