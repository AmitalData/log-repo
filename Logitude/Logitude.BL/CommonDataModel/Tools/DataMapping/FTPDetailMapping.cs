using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs; 
using Simplog.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Identity;
using System.IO;
using System.Security.Cryptography;
using System.Configuration;
using Azure.Security.KeyVault.Secrets;
using System.Net;
using Azure.Core;

namespace Logitude.BL.CommonDataModel.Tools.DataMapping
{
    public class FTPDetailMapping
    {
        public static void MapEntity(FTPDetailPM itemPM, FTPDetail itemPoco, bool isNewEntity, string loggedContactId)
        {
            if (isNewEntity)
            {
                itemPoco.Id = itemPM.Id;
                itemPoco.Tenant = itemPM.Tenant;
                itemPoco.CreatedByUserId = loggedContactId;
                itemPoco.CreateDate = TenantServerConfigration.GetCurrentDateTime(itemPM.Tenant);
            }

            itemPoco.UpdateDate = TenantServerConfigration.GetCurrentDateTime(itemPM.Tenant);
            itemPoco.UpdatedByUserId = loggedContactId;
            itemPoco.UserName = itemPM.UserName;
            itemPoco.Password = itemPM.Password;
            itemPoco.Host = itemPM.Host;
            itemPoco.Folder = itemPM.Folder;
            itemPoco.InActive = itemPM.InActive;
            itemPoco.UseSFTP = itemPM.UseSFTP;
            itemPoco.PrivateKey = EncryptPrivateKeyAsync(itemPM.PrivateKey).GetAwaiter().GetResult();
            itemPoco.Port = itemPM.Port;

		}

        public async static Task<byte[]> EncryptPrivateKeyAsync(string privateKey)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            var keyVaultUrl = ConfigurationManager.AppSettings["keyVaultUrl"];
            var secretName = ConfigurationManager.AppSettings["secretName"];
            var clientId = ConfigurationManager.AppSettings["clientIdVault"];
            var tenantId = ConfigurationManager.AppSettings["tenantIdVault"];
            var clientSecret = ConfigurationManager.AppSettings["clientSecretVault"];
            var credential = new ClientSecretCredential(tenantId, clientId, clientSecret);
            var options = new SecretClientOptions
            {
                Retry =
                {
                    MaxRetries = 5,
                    Delay = TimeSpan.FromSeconds(2),
                    Mode = RetryMode.Exponential
                }
            };
            var secretClient = new SecretClient(new Uri(keyVaultUrl), credential, options);
            KeyVaultSecret secret = await secretClient.GetSecretAsync(secretName).ConfigureAwait(false);
            byte[] aesKey = Convert.FromBase64String(secret.Value);
            Aes aesAlg = Aes.Create();
            aesAlg.Key = aesKey;
            aesAlg.GenerateIV();
            byte[] plainBytes = Encoding.UTF8.GetBytes(privateKey);
            var ms = new MemoryStream();
            ms.Write(aesAlg.IV, 0, aesAlg.IV.Length);
            using (var cs = new CryptoStream(ms, aesAlg.CreateEncryptor(), CryptoStreamMode.Write))
            {
                cs.Write(plainBytes, 0, plainBytes.Length);
                cs.FlushFinalBlock();
            }
            return ms.ToArray();
        }
    }
}
