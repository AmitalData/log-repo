using Logitude.Accounting.Def.EntityPMs;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.EntityUpdateServices
{
  public partial  class GLAccountWithholdingTaxUpdateService
    {


        protected override void OnCreating(GLAccountWithholdingTaxPM entityPM, GLAccountPM entityParentPM)
        {
            entityPM.GLAccountId = entityParentPM.Id;
            entityPM.Id= IdCounter.GetNumber("GLAccountWithholdingTax", entityPM.Tenant);
            entityParentPM.TaxWithholdingLastLine += 1;
            entityPM.LineNumber = entityParentPM.TaxWithholdingLastLine;
         
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
            Contact contact = contactRep.GetSingleContactByEmail(email, entityPM.Tenant);
            entityPM.CreatedByUserId = contact.Id;
        }
    }
}
