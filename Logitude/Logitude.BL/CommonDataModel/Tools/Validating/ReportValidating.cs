using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel;

namespace Logitude.BL.CommonDataModel.Tools.Validating
{
    public class ReportValidating
    {
        public static void Validate(EntityPMs.ReportPM entityPM, ICommonDataContext iContext, bool isNewEntity)
        {
            if (string.IsNullOrEmpty(entityPM.Code))
            {
                throw new ApplicationException("Code field is required");
            }

            else
            {
                bool exist = false;

                if (isNewEntity)
                {
                    exist = (from a in iContext.Reports
                             where a.Code.ToLower() == entityPM.Code.ToLower() && a.Tenant == entityPM.Tenant
                             select a).Any();
                }

                else
                {
                    exist = (from a in iContext.Reports
                                  where a.Code.ToLower() == entityPM.Code.ToLower()
                                  && a.Id != entityPM.Id
                                  && a.Tenant == entityPM.Tenant
                                  select a).Any();
                }

                if (exist)
                {
                    throw new ApplicationException("Report Code already exists");
                }
            }
        }      
    }
}