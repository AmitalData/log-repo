using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.Tools.DataMapping
{
    public class FreelancerGroupTypeMapping
    {
        public static void MapEntity(FreelancerGroupTypePM itemPM, FreelancerGroupType itemPoco, bool isNewEntity)
        {
            if (isNewEntity)
            {
                itemPoco.Id = itemPM.Id;
                itemPoco.Tenant = itemPM.Tenant;
                itemPoco.Code = itemPM.Code;
            }

            itemPoco.InActive = itemPM.InActive;
            itemPoco.EnglishName = itemPM.EnglishName;


        }
    }
}
