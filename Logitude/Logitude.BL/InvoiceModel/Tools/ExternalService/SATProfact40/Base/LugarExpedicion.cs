using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InvoiceModel.Tools.ExternalService.SATProfact40.Base
{
    internal class LugarExpedicion
    {
        private static ICommonDataContext commonContext;
        public static string Get(LugarExpedicionArgs lugarExpedicionArgs)
        {
            string branchId = lugarExpedicionArgs.BranchId;
            string currentTenantZipCode = lugarExpedicionArgs.CurrentTenantZipCode;
            int tenant = lugarExpedicionArgs.Tenant;
            commonContext = lugarExpedicionArgs.CommonContext;

            Address branchAddress = null;
            if (!string.IsNullOrEmpty(branchId))
            {
                branchAddress = GetBranchAddress(branchId, tenant);
            }

            if (branchAddress != null && !string.IsNullOrEmpty(branchAddress.ZipCode))
            {
                return branchAddress.ZipCode;
            }
            else
                return currentTenantZipCode;
        }

        private static Address GetBranchAddress(string branchId, int tenant)
        {
            BranchRepository branchRepository = new BranchRepository(commonContext);
            Branch branch = branchRepository.GetSingleBranch(branchId, tenant);
            if (!string.IsNullOrEmpty(branch.AddressId))
            {
                return new AddressRepository(commonContext).GetSingleAddress(branch.AddressId, branch.Tenant);
            }

            return null;
        }

    }
    public class LugarExpedicionArgs
    {
        public string BranchId { get; set; }
        public string CurrentTenantZipCode { get; set; }
        public int Tenant { get; set; }
        public ICommonDataContext CommonContext { get; set; }

    }
}
