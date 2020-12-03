 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools;
using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.BL.EntityDataMappings;
using Logitude.Infrastructure.Data.Repsitories;
using Logitude.Infrastructure.Data.EntityKeys;
using Logitude.Infrastructure.Data;
using Logitude.Server.Tools.StorageService;
using Microsoft.Practices.Unity;
using System.IO;

namespace Logitude.Infrastructure.BL.EntityQueryServices
{ 
   public partial class SharedLogisticsSettingQueryService
   {
		public string GetTenantLogoUri(int companyId, bool isSmalLogo)
        {
            string fileName = "sharedLogtsitcslogo";//smalllogo

            BlobFileInfo fileInfo = new BlobFileInfo()
            {
                FileName = fileName + companyId,
                FolderName = "logos",
                Extension = "png",
                Tenant = companyId,
            };

            IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
            byte[] datainByte = storageservice.Read(fileInfo);

            if (datainByte == null)
            {
                fileName = isSmalLogo ? "smalllogo" : "logo";
                fileInfo.FileName = fileName + companyId;
                fileInfo.Extension = "jpg";
                datainByte = storageservice.Read(fileInfo);
            }
            string uri = GetUriAfterConvertDataToBase64(datainByte);

            return uri;
        }

        private string GetUriAfterConvertDataToBase64(byte[] datainByte)
        {
            if (datainByte != null)
            {
                using (MemoryStream memstream = new MemoryStream())
                {
                    //blobfile.DownloadToStream(memstream);
                    //memstream.ToArray();
                    // <img src="data:image/gif;base64,xxxxxxxxxxxxx...">
                    //data:image/gif;base64,xxxxxxxxxxxxx...
                    string base64String = System.Convert.ToBase64String(datainByte, 0, datainByte.Length);

                    string uri = "data:image/jpg;base64," + base64String;
                    return uri;//blobfile.Uri.AbsoluteUri;
                }
            }
            else
                return null;
        }
   }
}