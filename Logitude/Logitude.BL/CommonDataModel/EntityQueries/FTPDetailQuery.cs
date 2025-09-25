using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs; 
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using System.IO;
using System.Security.Cryptography;
using System.Configuration;
using System.Data.Entity;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class FTPDetailQuery
    {
        FTPDetailRepository repository;



        public FTPDetailQuery(int tenant)
        {
            repository = new FTPDetailRepository(tenant);
        }

        public FTPDetailQuery(FTPDetailRepository rep)
        {
            repository = rep;
        }

        public FTPDetailPM GetSinglePM(string id, int tenant)
        {
            var entity = repository.context.FTPDetails
                .FirstOrDefault(d => d.Id == id && d.Tenant == tenant);

            if (entity == null)
                return null;

            var detail = new FTPDetailPM
            {
                Id = entity.Id,
                Tenant = entity.Tenant,
                UserName = entity.UserName,
                Password = entity.Password,
                Host = entity.Host,
                Folder = entity.Folder,
                CreateDate = entity.CreateDate,
                UpdateDate = entity.UpdateDate,
                CreatedByUserId = entity.CreatedByUserId,
                UpdatedByUserId = entity.UpdatedByUserId,
                InActive = entity.InActive,
                UseSFTP = entity.UseSFTP,
                Port = entity.Port
            };

            if (entity.PrivateKey != null && entity.PrivateKey.Length > 0)
            {
                detail.PrivateKey = DecryptFromKeyVaultAsync(entity.PrivateKey).GetAwaiter().GetResult();
            }
            return detail;
        }


        public async Task<List<FTPDetailPM>> GetFTPDetailsForSetting(int settingId, int tenant)
        {
            var data = await repository.context.FTPDetails
                .Where(d => d.Tenant == settingId)
                .ToListAsync();

            var result = new List<FTPDetailPM>();

            foreach (var d in data)
            {
                result.Add(new FTPDetailPM
                {
                    Id = d.Id,
                    Tenant = d.Tenant,
                    UserName = d.UserName,
                    Password = d.Password,
                    Host = d.Host,
                    Folder = d.Folder,
                    CreateDate = d.CreateDate,
                    UpdateDate = d.UpdateDate,
                    CreatedByUserId = d.CreatedByUserId,
                    UpdatedByUserId = d.UpdatedByUserId,
                    InActive = d.InActive,
                    UseSFTP = d.UseSFTP,
                    PrivateKey = await DecryptFromKeyVaultAsync(d.PrivateKey),
                    Port = d.Port
                });
            }
            return result;
        }

        public IQueryable<FTPDetailList> GetIQueryableEntityList(IQueryable<FTPDetail> iQueryable)
        {
            IQueryable<FTPDetailList> result = from d in iQueryable
                                               select new FTPDetailList()
                                               {
                                                   Id = d.Id,
                                                   Tenant = d.Tenant,
                                                   UserName = d.UserName,
                                                   Password = d.Password,
                                                   Host = d.Host,
                                                   Folder = d.Folder,
                                                   CreateDate = d.CreateDate,
                                                   UpdateDate = d.UpdateDate,
                                                   CreatedByUserId = d.CreatedByUserId,
                                                   UpdatedByUserId = d.UpdatedByUserId,
                                                   InActive = d.InActive,
                                                   UseSFTP = d.UseSFTP,
                                                   PrivateKey = d.PrivateKey,
                                                   Port = d.Port,
											   };
            return result;
        }

        public async Task<string> DecryptFromKeyVaultAsync(byte[] encryptedBytes)
        {
            if (encryptedBytes == null || encryptedBytes.Length == 0)
                return null;
            var keyVaultUrl = ConfigurationManager.AppSettings["keyVaultUrl"];
            var secretName = ConfigurationManager.AppSettings["secretName"];
            var clientId = ConfigurationManager.AppSettings["clientIdVault"];
            var tenantId = ConfigurationManager.AppSettings["tenantIdVault"];
            var clientSecret = ConfigurationManager.AppSettings["clientSecretVault"];

            var credential = new ClientSecretCredential(tenantId, clientId, clientSecret);
            var secretClient = new SecretClient(new Uri(keyVaultUrl), credential);

            KeyVaultSecret secret = await secretClient.GetSecretAsync(secretName).ConfigureAwait(false);
            byte[] aesKey = Convert.FromBase64String(secret.Value);

            string decryptedText = "";

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = aesKey;

                using (var ms = new MemoryStream(encryptedBytes))
                {
                    byte[] iv = new byte[aesAlg.BlockSize / 8];
                    ms.Read(iv, 0, iv.Length);

                    aesAlg.IV = iv;

                    ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                    using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    using (var reader = new StreamReader(cs, Encoding.UTF8))
                    {
                        decryptedText = reader.ReadToEnd();
                    }
                }
            }
            return decryptedText;
        }
    }
}
