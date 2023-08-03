using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Def.EntityQueryServicesExt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityQueryServiceExt
{
    public class CustomsDocumentQueryServiceExt: ICustomsDocumentQueryServiceExt
    {
        public CustomsDocumentQueryServiceExt()
        {

        }


        public CustomsDocumentPM GetSingle(string id, bool getComposition, bool getFromCache,int tenant)
        {
            CustomsDocumentQueryService query = new CustomsDocumentQueryService(tenant);
            return query.GetSingle(id, getComposition, getFromCache);
        }

        public CustomsDocumentPM GetSingleByDocFileId(string id,  int tenant)
        {
            CustomsDocumentQueryService query = new CustomsDocumentQueryService(tenant);
            return query.GetSingleByDocFileId(id, tenant);
        }
    }
}
