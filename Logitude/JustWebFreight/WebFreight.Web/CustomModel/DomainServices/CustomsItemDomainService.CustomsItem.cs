using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.DomainServices.Hosting;
using System.Web;
using WebFreight.Web.Helpers;

namespace WebFreight.Web.CustomModel.DomainServices
{

   
    public partial class CustomsItemDomainService
    {

     
        public CustomsItemPM GetCustomsItemByClassificationCode(string classificationCode, int tenant)
        {
            if (MyContext == null)
            {
                MyContext = CustomContext.GetContext(tenant);
            }

            CustomsItemQueryService customsItemQuery = new CustomsItemQueryService(MyContext);
            CustomsItemPM customsItemPM = customsItemQuery.GetCustomsItemByClassificationCode(classificationCode);
            return customsItemPM;

        }
    }
}