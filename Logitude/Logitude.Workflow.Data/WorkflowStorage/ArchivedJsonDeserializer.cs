using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Logitude.Workflow.Data.WorkflowStorage
{
    public class ArchivedJsonDeserializer
    {
        private readonly string ArchivedJson;
        private readonly JsonSerializerSettings Settings;

        public ArchivedJsonDeserializer(byte[] zipFileBytes, string entryFileName, JsonSerializerSettings settings)
        {
            ArchivedJson = GetArchivedJson(zipFileBytes, entryFileName);
            Settings = settings;
        }
        
        public T Deserialize<T>(string jsonPath = null)
        {
            string json = GetJson(jsonPath);
            return Settings == null ? JsonConvert.DeserializeObject<T>(json) : JsonConvert.DeserializeObject<T>(json, Settings);


            //try
            //{

            //    if (!string.IsNullOrEmpty(json))
            //    {
            //        return Settings == null ? JsonConvert.DeserializeObject<T>(json) : JsonConvert.DeserializeObject<T>(json, Settings);

            //    }
            //    return default ;
            //}
            //catch (Exception)
            //{
            //    return default ;
            //}
        }

        private string GetArchivedJson(byte[] zipFileBytes, string entryFileName)
        {
            try
            {
                if (zipFileBytes != null && zipFileBytes.Length > 0 && !string.IsNullOrEmpty(entryFileName))
                {
                    using (MemoryStream zipFileStream = new MemoryStream(zipFileBytes))
                    using (ZipArchive zipArchive = new ZipArchive(zipFileStream, ZipArchiveMode.Read))
                    {
                        ZipArchiveEntry zipArchiveEntry = zipArchive.Entries.FirstOrDefault(e => e.Name.ToLower() == entryFileName.ToLower());
                        if (zipArchiveEntry != null)
                        {
                            using (Stream unzippedEntryStream = zipArchiveEntry.Open())
                            using (MemoryStream memoryStream = new MemoryStream())
                            {
                                unzippedEntryStream.CopyTo(memoryStream);
                                return Encoding.ASCII.GetString(memoryStream.ToArray());
                            }
                        }
                    }
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private string GetJson(string jsonPath)
        {
            try
            {
                if (string.IsNullOrEmpty(ArchivedJson))
                {
                    return null;
                }

                if (!string.IsNullOrEmpty(jsonPath))
                {
                    return JObject.Parse(ArchivedJson)?.SelectToken(jsonPath)?.ToString();
                }

                return ArchivedJson;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}