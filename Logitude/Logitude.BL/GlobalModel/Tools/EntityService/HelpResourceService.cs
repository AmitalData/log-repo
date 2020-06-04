using Logitude.BL.GlobalModel.EntityPMs;
using Logitude.BL.GlobalModel.Tools.DataMapping;
using Logitude.BL.GlobalModel.Tools.TraceEvents;
using Simplog.Data.Helpers;
using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            entityPM.CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
            entityPM.UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant);

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
            entityPM.UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant);

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
    }
}
