using Logitude.Accounting.BL.APIDataContract.ApiV1;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.Def.EntityQueryServicesExt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.EntityQueryServiceExt
{
    public class GLAccountQueryServiceExt : IGLAccountQueryServiceExt
    {
        public GLAccountQueryServiceExt()
        {

        }

        public GLAccountPM GetSingleGLAccountPM(string id, int tenant)
        {
            EntityQueryServices.GLAccountQueryService query = new EntityQueryServices.GLAccountQueryService(tenant);
            return query.GetSinglePM(id, tenant);
        }
        public GLAccountPM GetSingleGLAccountWithComposition(string id, int tenant)
        {
            EntityQueryServices.GLAccountQueryService query = new EntityQueryServices.GLAccountQueryService(tenant);
            return query.GetSingle(id, true,false);
        }


        public GLAccountPM GetGLAccountByDisplayNumber(string number, int tenant)
        {
            EntityQueryServices.GLAccountQueryService query = new EntityQueryServices.GLAccountQueryService(tenant);
           
     
            GLAccountPM gLAccount = query.GetSinglePMByDisplayNumber(number, tenant);

            return gLAccount;
              
           
           
        }

        public GLAccountPM GetGLAccountByInternalNumber(string number, int tenant)
        {
            EntityQueryServices.GLAccountQueryService query = new EntityQueryServices.GLAccountQueryService(tenant);


            GLAccountPM gLAccount = query.GetSinglePMByInternalNumber(number, tenant);

            return gLAccount;



        }

        public string GetGLAccountDisplayNoAndLocalName(string id, int tenant)
        {
            EntityQueryServices.GLAccountQueryService query = new EntityQueryServices.GLAccountQueryService(tenant);
            return query.GetGLAccountDisplayNoAndLocalName(id, tenant);
        }

        public string GetDisplayNumberByGLAccountId(string id, int tenant)
        {
            EntityQueryServices.GLAccountQueryService query = new EntityQueryServices.GLAccountQueryService(tenant);
            return query.GetDisplayNumberByGLAccountId(id, tenant);
        }

        public APIDataContract.ApiV1.GLAccount GLAccountDataMappingAndValidatin(GLAccountPM MyEntity, int Tenant, string ComputingPartnerName = "")
        {
            APIDataContract.ApiV1.GLAccountQueryService query = new APIDataContract.ApiV1.GLAccountQueryService(Tenant);
            return query.GLAccountDataMappingAndValidatin(MyEntity, Tenant);
        }


        public GLAccountPM  GetSplittedByCurrencyGLAccount(string accountId, int tenant, string currency)
        {
            EntityQueryServices.GLAccountQueryService query = new EntityQueryServices.GLAccountQueryService(tenant);


            GLAccountPM gLAccount = query.GetSplittedByCurrencyGLAccount(accountId, tenant, currency);

            return gLAccount;


        }

        public GLAccountPM GetSplittedGLAccount(string accountId, int tenant, string currency)
        {
            EntityQueryServices.GLAccountQueryService query = new EntityQueryServices.GLAccountQueryService(tenant);


            GLAccountPM gLAccount = query.GetSplittedGLAccount(accountId, tenant, currency);

            return gLAccount;


        }

        public IQueryable<GLAccountPM> GetSplittedByCurrencyGLAccounts(string accountId, int tenant)
        {
            EntityQueryServices.GLAccountQueryService query = new EntityQueryServices.GLAccountQueryService(tenant);
            IQueryable<GLAccountPM> gLAccounts = query.GetSplittedByCurrencyGLAccounts(accountId, tenant);

            return gLAccounts;
        }

        public bool CheckInactiveGLAccounts(List<string> glaccountIds, int tenant)
        {
            EntityQueryServices.GLAccountQueryService query = new EntityQueryServices.GLAccountQueryService(tenant);
            return query.CheckInactiveGLAccounts(glaccountIds, tenant);
        }

        public List<string> GetChildrenByCurrencyGLAccountIds (string accountId, int tenant)
        {
            EntityQueryServices.GLAccountQueryService query = new EntityQueryServices.GLAccountQueryService(tenant);
            return query.GetChildrenByCurrencyGLAccountIds(accountId, tenant);
        }
    }
}
