using Logitude.FullAccounting.Test.Models;
using Logitude.Test.Base.Models.Api;
using Logitude.Test.Base.Models.BillingsPreparation;
using Logitude.Test.Base.Models.PartnersPreparation;
using Logitude.Test.Base.Models.Shared;
using Logitude.Test.Base.Models.UserTenantPreparation;
using Logitude.Test.Base.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.FullAccounting.Test.Services.Preparation
{
    public class VendorPreparation
    {

        public void Prepare()
        {
            var partnerParameters = GetPartnerParameters();
            FullAccountingData.VendorId = DataPreparation.GetPartnerId(partnerParameters);
            CheckConnectWithGLAccount(FullAccountingData.VendorId);
        }
        
        private PartnerParameters GetPartnerParameters()
        {
            return new PartnerParameters()
            {
                Code = "FVDSpecFlowT12",
                Name = "FAC SpecFlowTest",
                TypeCode = "VD"
            };
        }
        private void CheckConnectWithGLAccount(string VendorId)
        {
            var vendor = APICaller.CallGet<VendorPM>(Urls.VendorssGetSingle(VendorId), UserTenant.Token)?.Data;
            if (string.IsNullOrEmpty(vendor.GLAccountId))
            {
                ConnectWithGLAccount(vendor);
            }
        }

        private void ConnectWithGLAccount(VendorPM vendor)
        {
            var account = CreateGLAccountInstance(vendor);
            var response = APICaller.CallPost<GLAccountPM>(account, Urls.GLAccountsController, UserTenant.Token);

        }
        private GLAccountPM CreateGLAccountInstance(VendorPM vendor)
        {
            return new GLAccountPM()
            {
                DisplayNumber = vendor.Id,
                NewGLAccountCardId = vendor.Card.Id,
                IsMultiCurrency = false,
                AccountTypeCode = (int)GLAccountTypeEnum.Client + "",
                CurrencyId = BillingData.CurrencyNISId,
                Tenant = UserTenant.Tenant,
                AutomaticReconcileId = FullAccountingData.AutomaticReconcile_A,
                ChartOfAccountsTypeCode = (int)ChartOfAccountsTypeEnum.Vendors + "",
                ReconcileMethodCode = (int)ReconcileMethodEnum.Local + "",
                RevenueExpenseType = (int)RevenueExpenseTypeEnum.Other + "",
                ChartOfAccountsId = FullAccountingData.VendorChartOfAccountId,
                LocalName = vendor.LocalName,
                EnglishName = vendor.EnglishName,
            };
        }
    }
}
