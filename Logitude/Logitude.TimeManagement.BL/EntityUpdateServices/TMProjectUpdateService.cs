using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.TimeManagement.BL.EntityPMs;
using Logitude.TimeManagement.Data.Repositories;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.TimeManagement.BL.EntityUpdateServices
{
    public partial class TMProjectUpdateService
    {

        protected override void OnCreating(TMProjectPM entityPM, Server.Tools.EntityPM entityParentPM)
        {
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
              
                    entityPM.Id = IdCounter.GetNumber("TMProject", entityPM.Tenant);
                
              
                if (string.IsNullOrEmpty(entityPM.ProjectNumber))
                {
                    entityPM.ProjectNumber = CodeCounter.GetNumber("TMProject", entityPM.Tenant).ToString();
                }

                else
                {
                    entityPM.ProjectNumber += "-" + GenerateNewId( entityPM);
                }
            }
        }


        public string GenerateNewId(TMProjectPM entityPM)
        {

            TMProjectRepository entityRepository = new TMProjectRepository(entityPM.Tenant);
            List<string> ProjectNumbers = entityRepository.GetInnerTMProjectByNumber(entityPM.ProjectNumber, entityPM.Tenant);
            List<int> EditedProjectNumbers = new List<int>();
            int ProjectId ;
            if (ProjectNumbers.Count==0)
                ProjectId = 1;
            else
            {
                foreach(string item in ProjectNumbers)
                {
                    string newitem = item.Replace(entityPM.ProjectNumber+"-", "");
                    EditedProjectNumbers.Add(int.Parse(newitem.Split('-')[0]));
                }

                int max = EditedProjectNumbers.Max();
               // string[] MaxArr = max.Split('-');
                 ProjectId = max + 1;
            }
           
            return ProjectId+"";
        }

        protected override void OnUpdating(TMProjectPM entityPM)
        {
            DateTime myDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);

            ICommonDataContext commonContext = CommonDataContext.GetContext(entityPM.Tenant);
            ContactRepository contactRep = new ContactRepository(commonContext);
            string email = "";
            if (AuthenticationUtil.IsAuthenticatedUserExists())
            {
                email = AuthenticationUtil.GetAuthenticatedUser();
            }

            else
            {
                email = "system@tenant" + entityPM.Tenant + ".com";
            }

            string myLoggedUserId = null;
            Contact contact = contactRep.GetSingleContactByEmail(email, entityPM.Tenant);
            if (contact != null)
            {
                myLoggedUserId = contact.Id;
            }

            entityPM.UpdateDate = myDate;
            entityPM.UpdatedByUserId = myLoggedUserId;

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                entityPM.CreateDate = myDate;
                if (entityPM.CreatedByUserId == null)
                {
                    entityPM.CreatedByUserId = myLoggedUserId;
                }
            }
        }
    }
}
