using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Common.Gen;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.Server.Tools.ExternalServices;
using Simplog.Data.CommonDataModel;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Unifreight.BL.EntityQueryServices;

namespace Logitude.Customs.BL.Messaging.Customs
{
    public class SupplierInvoiceByOcr
    {
        public class SupplierInvoiceOcr
        {

            public Page[] pages { get; set; }
        }

        public class Page
        {

            public Prediction[] prediction { get; set; }

        }



        public class Prediction
        {
            public string label { get; set; }
            public string ocr_text { get; set; }
            public Cell[] cells { get; set; }
            public int page_no { get; set; }
        }

        public class Cell
        {
            public int row { get; set; }
            public string label { get; set; }
            public string text { get; set; }
            public int ymin { get; set; }
            public int ymax { get; set; }

        }
    }


}
