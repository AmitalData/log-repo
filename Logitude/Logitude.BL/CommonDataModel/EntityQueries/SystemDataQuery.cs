using System;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using Logitude.BL.Helpers;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityLists;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System.Transactions;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Logitude.Server.Tools.Helpers;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class SystemDataQuery
    {
        public SystemDataQuery()
        {
        }
        public SystemDataQuery(int tenant)
        {
        }

        public SystemDataPM GetSinglePM(string id, int tenantNum)
        {
            UserRepository userRepository = new UserRepository(tenantNum);
            User user = userRepository.GetSingleUser(id, tenantNum, false);
            if (user == null)
            {
                user = userRepository.GetSingleUser(id, 0, false);
            }

            Tenant tenant = TenantRepository.GetSingleTenant(tenantNum, true);
            string supportEmail = GetSupportEmail(tenantNum);
            string userSignatureImage = new ImageDetailsHtmlRenderingService().Render(user.SignatureImageId, user.Tenant);

            return new SystemDataPM()
            {
                UserId = user != null ? user.Id : "",
                Date = DateTime.Now,
                Signature = user != null ? user.Contact.Signature: null,
                SignatureHtml = user != null ? user.Contact.SignatureHtml: null,
                UserName = user!=null ? user.Contact.EnglishName: "",
                LocalCurrencyId = tenant.CurrencyId,
                AddressId = tenant.AddressId,
                Company = tenant.Company,
                Email = tenant.Email,
                IATA = tenant.IATA,
                VatNumber = tenant.VatNumber,
                Website = tenant.Website,
                ContactId = user != null ? user.Id:"",
                Supportemail = supportEmail,
                UserSignatureImage = userSignatureImage,
            };
        }

        private string GetSupportEmail(int tenant)
        {
            string email = "";
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                TenantManagementRepository tenantManagementRepository = new TenantManagementRepository();
                TenantManagement myTenant = tenantManagementRepository.GetSingleTenantManagement(tenant);
                if (myTenant != null)
                {
                    email = myTenant.SupportEmail;
                }
            }
            return email;
        }
    }
}
