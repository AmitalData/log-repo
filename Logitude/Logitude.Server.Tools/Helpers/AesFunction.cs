using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Server.Tools.Helpers
{
  public  class AesFunction
    {
        public byte[] DecryptData(byte[] data, int tenant, string aesKey = null)
        {
            if (data != null && data.Length>0)
            {

                try
                {

                    string plaintext = null;
                    byte[] result;

                    using (Aes aesAlg = Aes.Create())
                    {
                        aesAlg.Key = GetAesKey(tenant, aesKey);

                        // Validate key size
                        if (aesAlg.Key.Length != 32)
                            throw new Exception("Invalid AES key size. Key must be 32 bytes.");

                        // Validate data length
                        if (data.Length < aesAlg.BlockSize / 8)
                            throw new Exception("Invalid data length. Data too short to contain IV.");

                        // Separate IV and ciphertext
                        byte[] IV = new byte[aesAlg.BlockSize / 8]; // 16 bytes for AES
                        byte[] cipherText = new byte[data.Length - IV.Length];

                        Array.Copy(data, IV, IV.Length);
                        Array.Copy(data, IV.Length, cipherText, 0, cipherText.Length);

                        // Validate ciphertext length
                        //if ((cipherText.Length % aesAlg.BlockSize) != 0)
                        //    throw new Exception("Invalid ciphertext length. Not a multiple of block size.");

                        aesAlg.IV = IV;

                        aesAlg.Mode = CipherMode.CBC;


                        ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);


                        using (var msDecrypt = new MemoryStream(cipherText))
                        {
                            using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                            {
                                using (var srDecrypt = new StreamReader(csDecrypt))
                                {
                                    plaintext = srDecrypt.ReadToEnd();
                                }
                            }
                        } 

                    }

                    result = System.Convert.FromBase64String(plaintext);
                    return result;
                }
                catch (CryptographicException ex)
                {
                    throw new CryptographicException("Cryptographic error: " + ex.Message);
                }
                catch (Exception ex)
                {

                    throw new Exception("Decryption error: " + ex.Message);
                }

            }
            else return data;
        }

        public byte[] EncryptData(byte[] data, int tenant,string aesKey = null)
        {
            if (data != null && data.Length>0)
            {
                try
                {
                    string dataAsString = Convert.ToBase64String(data);
                    byte[] encrypted;
                    byte[] IV;

                    using (Aes aesAlg = Aes.Create())
                    {
                        aesAlg.Key = GetAesKey(tenant, aesKey);
                        aesAlg.GenerateIV();
                        IV = aesAlg.IV;
                        aesAlg.Mode = CipherMode.CBC;
                        var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                        using (var msEncrypt = new MemoryStream())
                        {
                            using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                            {
                                using (var swEncrypt = new StreamWriter(csEncrypt))
                                {
                                    swEncrypt.Write(dataAsString);
                                }
                                encrypted = msEncrypt.ToArray();
                            }
                        }
                    }

                    var combinedIvCt = new byte[IV.Length + encrypted.Length];
                    Array.Copy(IV, 0, combinedIvCt, 0, IV.Length);
                    Array.Copy(encrypted, 0, combinedIvCt, IV.Length, encrypted.Length);


                    return combinedIvCt;
                }
                catch (Exception ex)
                {
                    //string message = "In Valid Aes key";
                    //if (ex.Message == "Aes key is Empty")
                    //{
                    //    message = ex.Message;
                    //}

                    throw new Exception(ex.Message);
                }
            }
            else
            {
                return data;
            }
        }


        private  byte[] GetAesKey(int tenant, string aesKey = null)
        {
            if (string.IsNullOrEmpty(aesKey))
            {
                TenantRepository tenantRepository = new TenantRepository(tenant);
                Tenant currentTenant = tenantRepository.GetSingleTenantByIdAndTenant(tenant, true);
                if (currentTenant != null) aesKey = currentTenant.StorageEncryptionKey;
            }

            if (!string.IsNullOrEmpty(aesKey))
            {
                if (aesKey.Length < 32) throw new Exception("In Valid Aes key");
                else return Encoding.UTF8.GetBytes(aesKey);
            }
            else
            {
                throw new Exception("Aes key is Empty");
            }
        }

        private  Random random = new Random();
        public  string GenerateAesKey()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 32)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

    }
}
