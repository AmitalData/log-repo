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
    public class IncotermCodePropertiesMapping
    {
        public static string GetIncotermIdFromIncotermProperties(int ImporterTenant, CodeProperties incotermProperties)
        {

            if (!string.IsNullOrEmpty(incotermProperties.Id))
            {
                return incotermProperties.Id;
            }
            else if (!string.IsNullOrEmpty(incotermProperties.Code))
            {
                ICommonDataContext commoncontext = CommonDataContext.GetContext(ImporterTenant);
                IncotermRepository incotermRepository = new IncotermRepository(commoncontext);
                Incoterm Incoterm = incotermRepository.GetIncotermByCode(incotermProperties.Code, ImporterTenant);
                if (Incoterm != null)
                {
                    return Incoterm.Id;
                }
                else
                {
                    return "";
                }
            }
            else// if (!string.IsNullOrEmpty(CardProperties.Code))
            {
                return null;
            }
        }
    }
}
