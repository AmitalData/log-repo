using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.SystemLogs;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using WebFreight.Web.WcfApi;
using WebFreight.Web.WebServices;

namespace WebFreight.Web.TrainingResourcesHTML
{
    public partial class TrainingResourcesMainPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int? tenant = null;
            string token = Request["Token"] ?? "";
            SecurityDocumentResult securityDocumentResult = SecurityDocumentHelper.ValidationDocumentToken(token);
            bool isValid = securityDocumentResult.IsValid;
            string email = securityDocumentResult.Email;
            string exceptionMessage = securityDocumentResult.ExceptionResult;
            tenant = securityDocumentResult.Tenant;
            
            if (!string.IsNullOrEmpty(email))
            {
                int tenant1 = tenant == null ? 0 : tenant.Value;

                ContactRepository contactRepository = new ContactRepository(tenant1);
                Contact contact = contactRepository.GetSingleContactByEmail(email, tenant1);
                if (contact == null)
                {
                    this.Context.Response.Redirect("../Login.aspx");
                }
            }
            else
            {
                this.Context.Response.Redirect("../Login.aspx");
            }

            this.TokenForResources.Value = token;

            if (isValid)
            {
                isValid = false;
                if (CheckAvailablityTenantsForEmail(email, (int)tenant))
                {
                    isValid = true;
                }
            }
            else
            {
                this.Context.Response.Redirect("../Login.aspx");
            }
        }

        public bool CheckAvailablityTenantsForEmail(string email, int tenant)
        {
            UserRepository userRep = new UserRepository(tenant);
            Simplog.Data.CommonDataModel.EntityPOCOs.User user = userRep.GetSingleUserByEmail(email, 0, false);

            bool available = true;
            if (user != null)
            {
                TenantManagementRepository tenantManagementRep = new TenantManagementRepository();
                bool isDistributorToCurrentTenant = tenantManagementRep.CheckDistributor(user.DistributorCode, tenant);
                if (user.IsDistributor)
                {
                    if (isDistributorToCurrentTenant)
                    {
                        available = true;
                    }
                    else
                    {
                        available = false;
                    }
                }
                else
                {
                    available = true;
                }
            }
            else
            {
                ContactRepository contactRep = new ContactRepository(tenant);
                available = contactRep.CheckEmailAvailabilityForTenant(email, tenant);
            }

            return available;
        }
    }
}