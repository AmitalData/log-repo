using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.Repositories;

namespace Logitude.BL.CommonDataModel.Tools.Validating
{
    public class ReportValidating
    {
        public static void Validate(EntityPMs.ReportPM entityPM)
        {
            if (string.IsNullOrEmpty(entityPM.Code))
            {
                throw new ApplicationException("Code field is required");
            }
        }      
    }
}