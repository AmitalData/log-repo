using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Logitude.BL.CommonDataModel.Tools.Validating
{
    public class CustomerSizeValidating
    {
        public static void Validate(CustomerSizePM entityPM)
        {
            CustomerSizeRepository entityRepository = new CustomerSizeRepository(entityPM.Tenant);
            CustomerSize CustomerSize = entityRepository.GetSingleCustomerSizeByCode(entityPM.Code, entityPM.Tenant);

            if (CustomerSize == null) return;

            if (CustomerSize.Id != entityPM.Id)
                throw new ApplicationException("Customer size with same code already exists");
        }
    }
}