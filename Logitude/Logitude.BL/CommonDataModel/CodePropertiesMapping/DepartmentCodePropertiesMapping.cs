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
    public class DepartmentCodePropertiesMapping
    {
        public static string GetDepartmenthIdFromDepartmentProperties(int ImporterTenant, CodeProperties departmentProperties)
        {

            if (!string.IsNullOrEmpty(departmentProperties.Id))
            {
                return departmentProperties.Id;
            }
            else if (!string.IsNullOrEmpty(departmentProperties.Code))
            {
                ICommonDataContext commoncontext = CommonDataContext.GetContext(ImporterTenant);
                DepartmentRepository departmentRepository = new DepartmentRepository(commoncontext);
                Department Department = departmentRepository.GetSingleDepartmentByCode(departmentProperties.Code, ImporterTenant);
                if (Department != null)
                {
                    return Department.Id;
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
