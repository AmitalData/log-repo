using Logitude.BL.GlobalModel.EntityPMs;
using Logitude.BL.GlobalModel.Tools.DataMapping;
using Logitude.BL.GlobalModel.Tools.TraceEvents;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.StorageService;
using Microsoft.Practices.Unity;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Logitude.BL.GlobalModel.Tools.EntityService
{
    public class HelpResourceService
    {
        private int tenant;
        private bool isNewEntity;
        public HelpResource entityPoco { get; set; }
        private HelpResourcePM entityPM;
        private IGlobalContext objectContext;
        private HelpResourceRepository entityRepository;
        public HelpResourceService(IGlobalContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.objectContext = objectContext;
            this.entityRepository = new HelpResourceRepository(objectContext);
        }

        public void Create(HelpResourcePM entity)
        {
            this.isNewEntity = true;
            this.entityPM = entity;
            this.entityPM.Code = this.GenerateCode();

            this.entityPoco = new HelpResource()
            {
                Code = entityPM.Code,
                Tenant = 0
            };

            entityPM.CreateDate = DateTime.Now;
            entityPM.UpdateDate = DateTime.Now;

            if (entityPM.File != null && entityPM.FileExtension != null)
            {
                this.UploadFile();
            }

            HelpResourceTracing.Trace(entityPM, entityPoco, isNewEntity);
            HelpResourceMapping.MapEntity(entityPM, entityPoco, isNewEntity);
            entityRepository.Add(entityPoco);
            entityRepository.SubmitChanges();
        }
        
        public void Update(HelpResourcePM entity)
        {
            this.isNewEntity = false;
            this.entityPM = entity;
            this.entityPoco = entityRepository.GetSingleHelpResource(entityPM.Code, entityPM.Tenant);
            entityPM.UpdateDate = DateTime.Now;

            if (entityPM.File != null && entityPM.FileExtension != null)
            {
                this.UploadFile();
            }

            HelpResourceTracing.Trace(entityPM, entityPoco, isNewEntity);
            HelpResourceMapping.MapEntity(entityPM, entityPoco, isNewEntity);
            entityRepository.Update(entityPoco);
            entityRepository.SubmitChanges();
        }

        private string GenerateCode()
        {
            string code = null;

            IQueryable<HelpResource> helpResources = entityRepository.GetHelpResources(0);
            List<string> codes = new List<string>();

            if (entityPM.Type == "REL" || entityPM.Type == "VID")
            {
                codes = helpResources.Where(d => d.Type == entityPM.Type).Select(s => s.Code).ToList();

                List<string> codes_split = new List<string>();
                foreach (string myCode in codes)
                {
                    codes_split.Add(myCode.Substring(3));
                }

                List<int> codes_int = new List<int>();
                foreach (string myCode in codes_split)
                {
                    codes_int.Add(Convert.ToInt32(myCode));
                }

                int maxCode = codes_int.Max();

                StringBuilder str = new StringBuilder();
                str.Append(entityPM.Type);
                str.Append((maxCode + 1).ToString().PadLeft(3, '0'));
                code = str.ToString();
            }
            
            else
            {
                codes = helpResources.Where(d => d.Type != "REL" && d.Type != "VID").Select(s => s.Code).ToList();

                List<int> codes_int = new List<int>();
                foreach(string myCode in codes)
                {
                    codes_int.Add(Convert.ToInt32(myCode));
                }

                int maxCode = codes_int.Max();
                code = (maxCode + 1).ToString();
            }

            return code;
        }

        private void UploadFile()
        {
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                byte[] fileData = Convert.FromBase64String(entityPM.File);
                byte[] buffer = fileData;
                int sentSize = fileData.Length;
                int fileSize = fileData.Length;
                string[] blockIdlist = { Convert.ToBase64String(Guid.NewGuid().ToByteArray()) };
                string filelocation = "how-to";
                string fileName = this.entityPM.FileName.Split('.')[0];

                IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;

                BlobFileInfo fileInfo = new BlobFileInfo()
                {
                    FileName = fileName,
                    FolderName = filelocation,
                    Extension = entityPM.FileExtension,
                    Tenant = tenant,
                    FileSize = fileSize,
                };

                //fileInfo.ContainerName = null;

                storageservice.WriteBlock(buffer, sentSize, blockIdlist, 0, fileInfo);
                scope.Complete();
            }
        }
    }
}
