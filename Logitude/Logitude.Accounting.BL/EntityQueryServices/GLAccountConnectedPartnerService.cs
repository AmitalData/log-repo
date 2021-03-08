using Logitude.BL.CommonDataModel.EntityQueries;
using System;
using System.Collections.Generic;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;


namespace Logitude.Accounting.BL.EntityQueryServices
{
    public class GLAccountConnectedPartnerService
    {
        int tenant;

        public GLAccountConnectedPartnerService(int tenant)
        {
            this.tenant = tenant;
        }

        public List<ShortPartnersDetails> GetAllConnectedPartnersByGLAccountId(string glAccountId)
        {
            GLAccountCurrency GLAccountCurrency = GetGLAccountCurrencyByIdAndTenant(glAccountId, tenant);
            CardQuery cardQuery = new CardQuery(tenant);
            List<ShortPartnersDetails> connectedPartners;

            if (GLAccountCurrency != null)
            {
                connectedPartners = cardQuery.GetConnectedPartnerIdsByGLAccountId(GLAccountCurrency.MainGLAccountId, tenant);
            }
            else
            {
                connectedPartners = cardQuery.GetConnectedPartnerIdsByGLAccountId(glAccountId, tenant);
            }

            return connectedPartners;
        }

        private GLAccountCurrency GetGLAccountCurrencyByIdAndTenant(string glAccountId, int tenant)
        {
            GLAccountCurrencyQueryService gLAccountCurrencyQueryService = new GLAccountCurrencyQueryService(tenant);
            GLAccountCurrency GLAccountCurrency = gLAccountCurrencyQueryService.GetGLAccountCurrencyByGLAccountId(glAccountId, tenant);
            return GLAccountCurrency;
        }
    }
}
