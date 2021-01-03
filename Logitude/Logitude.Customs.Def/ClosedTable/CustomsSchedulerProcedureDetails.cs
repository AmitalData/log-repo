using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.Def.ClosedTable
{
    public class CustomsSchedulerProcedureDetails : SchedulerProcedure, ICloseTable<SchedulerProcedure, CustomsSchedulerProcedureDetails> 
    {
        public List<CustomsSchedulerProcedureDetails> GetAll()
        {
            var all = new List<CustomsSchedulerProcedureDetails>();
            all.Add(new CustomsSchedulerProcedureDetails()
            {
                Code = "CustomsCloseCourierMasterTask",
                Name = "CustomsCloseCourierMasterTask",
                SearchFields = "CustomsCloseCourierMasterTask,CustomsCloseCourierMasterTask",
                Description = "Close Courier Master",
            });

            all.Add(new CustomsSchedulerProcedureDetails()
            {
                Code = "CustomsSendManifestTask",
                Name = "CustomsSendManifestTask",
                SearchFields = "CustomsSendManifestTask,CustomsSendManifestTask",
                Description = "Send Manifest",

            });
            all.Add(new CustomsSchedulerProcedureDetails()
            {
                Code= "CustomsSendDeclarationStatus",
                Name= "CustomsSendDeclarationStatus",
                SearchFields= "CustomsSendDeclarationStatus,CustomsSendDeclarationStatus",
                Description="Send DeclarationStatus",
            });
            return all;
        }
            public void MapPoco(SchedulerProcedure newPoco)
        {
            newPoco.Code = this.Code;
            newPoco.Name = this.Name;
            newPoco.SearchFields = GetSearchFields(this);
        }

        public string GetSearchFields(SchedulerProcedure rec)
        {
            return String.Concat(rec.Code, ",", rec.Name, ",");
        }
    }
}
