using Logitude.BL.CommonDataModel.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.IntegrationTest.Core
{
    public class CorePreparationVariables
    {
        public static TenantPM TenantPM { get; set; }
        public static string UserId { get; set; }
        public static string BranchId { get; set; }
        public static string DepartmentId { get; set; }
        public static string BusinessUnitId { get; set; }
    }
}
