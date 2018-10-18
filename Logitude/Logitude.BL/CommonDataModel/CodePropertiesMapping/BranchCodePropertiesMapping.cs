using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.CodePropertiesMapping
{
    public class BranchCodePropertiesMapping
    {
        public static string GetBranchIdFromBranchProperties(int ImporterTenant, CodeProperties branchProperties)
        {

            if (!string.IsNullOrEmpty(branchProperties.Id))
            {
                return branchProperties.Id;
            }
            else if (!string.IsNullOrEmpty(branchProperties.Code))
            {
                ICommonDataContext commoncontext = CommonDataContext.GetContext(ImporterTenant);
                BranchRepository branchRepository = new BranchRepository(commoncontext);
                Branch Branch = branchRepository.GetSingleBranchByCode(branchProperties.Code, ImporterTenant);
                if (Branch != null)
                {
                    return Branch.Id;
                }
                else
                {
                    return "";
                }
            }
            else// if (!string.IsNullOrEmpty(CardProperties.Code))
            {
                throw new NotImplementedException();
            }
        }
    }
}
