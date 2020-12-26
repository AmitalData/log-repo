using Logitude.Server.Tools;
using Logitude.Server.Tools.StorageService;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Practices.Unity;
using System.Text;
using System.Net;

namespace WebFreight.Web.Helpers
{
    public static class BluesnapHelper
    {
        public static Dictionary<string, string> DeserializeDocumentBody(string documentId, int tenant)
        {
            IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
            Dictionary<string, string> queryParameters = null;
            DocumentRepository documentRepository = new DocumentRepository(tenant);
            Document document = documentRepository.GetSingleDocument(tenant, documentId);
            if (document != null)
            {
                BlobFileInfo fileInfo = new BlobFileInfo()
                {
                    FileName = document.Id,
                    FolderName = document.Folder,
                    Extension = document.Extension,
                    Tenant = tenant,
                    FileSize = document.FileSize,
                };

                byte[] fileData = storageservice.Read(fileInfo);

                if (fileData != null)
                {
                    string Stringdetails = Encoding.UTF8.GetString(fileData);
                    queryParameters = new Dictionary<string, string>();
                    string[] querySegments = Stringdetails.Split('&');
                    foreach (string segment in querySegments)
                    {
                        string[] parts = segment.Split('=');
                        if (parts.Length > 0)
                        {
                            string key = parts[0].Trim(new char[] { '?', ' ' });
                            string val = parts[1].Trim();
                            if (!queryParameters.ContainsKey(key))
                            {
                                queryParameters.Add(WebUtility.UrlDecode(key), WebUtility.UrlDecode(val));
                            }
                        }
                    }
                }
            }
            return queryParameters;
        }
        public static Tuple<Dictionary<string, string>, string> DeserializeAnalyzeQueueMessageBody(byte[] messageBody)
        {
            string stringdetails = Encoding.UTF8.GetString(messageBody);
            var queryParameters = new Dictionary<string, string>();
            string[] querySegments = stringdetails.Split('&');
            foreach (string segment in querySegments)
            {
                string[] parts = segment.Split('=');
                if (parts.Length > 0)
                {
                    string key = parts[0].Trim(new char[] { '?', ' ' });
                    string val = parts[1].Trim();
                    if (!queryParameters.ContainsKey(key))
                    {
                        queryParameters.Add(WebUtility.UrlDecode(key), WebUtility.UrlDecode(val));
                    }
                }
            }

            return Tuple.Create(queryParameters, stringdetails);
        }
    }
}