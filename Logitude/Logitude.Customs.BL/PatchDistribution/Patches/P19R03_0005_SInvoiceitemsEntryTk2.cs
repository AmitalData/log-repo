using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.PatchDistribution.Patches
{
    class P19R03_0005_SInvoiceitemsEntryTk2 : PatchDistributionBase
    {
        public P19R03_0005_SInvoiceitemsEntryTk2()
            : base("פריסה מלאה מהצהרת FIX כנמ", new DateTime(2019, 12, 02))
        {

        }

        public override void CreateDownScripts()
        {
            throw new NotImplementedException();
        }

        public override void CreateUpScripts()
        {
            this.AddUpSqlScript("ALTER TABLE supplierinvoiceitems ADD (zmarksandnumbers VARCHAR2(30 CHAR),zweight NUMBER(15,3)) ");
            this.AddUpSqlScript("update supplierinvoiceitems  set zmarksandnumbers = substr(marksandnumbers,1,30) ,zweight =weight   where marksandnumbers is not null ");
            this.AddUpSqlScript("ALTER TABLE supplierinvoiceitems drop column marksandnumbers  ");
            this.AddUpSqlScript("ALTER TABLE supplierinvoiceitems drop column weight ");
            this.AddUpSqlScript("ALTER TABLE supplierinvoiceitems rename COLUMN  zmarksandnumbers to marksandnumbers  ");
            this.AddUpSqlScript("ALTER TABLE supplierinvoiceitems rename COLUMN  zweight to weight ");


        }
    }
}
