using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Logitude.Customs.Data
{
    public partial class CustomContext : DbContextBase, ICustomContext
    {
        

        public IDbSet<Card> Cards { get; set; }

        public IDbSet<DecCourierStatusesView> DecCourierStatusesViews { get; set; }

        public IDbSet<RequestSheetInQueueMesView> RequestSheetInQueueMesViews { get; set; }

        public IDbSet<DecTaxesByTaxTypeCodeView> DecTaxesByTaxTypeCodeViews { get; set; }
        public IDbSet<CustomsBookMainView> CustomsBookMainViews { get; set; }

         

    }
    public partial interface ICustomContext : IContext
    {
        
        IDbSet<Card> Cards { get; set; }
        IDbSet<DecCourierStatusesView> DecCourierStatusesViews { get; set; }

        IDbSet<RequestSheetInQueueMesView> RequestSheetInQueueMesViews { get; set; }

        IDbSet<DecTaxesByTaxTypeCodeView> DecTaxesByTaxTypeCodeViews { get; set; }
        IDbSet<CustomsBookMainView> CustomsBookMainViews { get; set; }



    }
}
