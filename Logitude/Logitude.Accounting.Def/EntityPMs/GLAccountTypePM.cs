using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.Def.EntityPMs
{
    public partial class GLAccountTypePM
    {
        //GLAccountTypeRepository glAccountTypeRepository = new GLAccountTypeRepository(accountingContext);
        //    AddClosedTables.AddGLAccountType(new GLAccountTypeDetails() { Code = "1", EnglishName = "Card", LocalName = "כרטיס" }, glAccountTypeRepository);
        //    AddClosedTables.AddGLAccountType(new GLAccountTypeDetails() { Code = "2", EnglishName = "Client", LocalName = "לקוח" }, glAccountTypeRepository);
        //    AddClosedTables.AddGLAccountType(new GLAccountTypeDetails() { Code = "3", EnglishName = "Vendor", LocalName = "ספק" }, glAccountTypeRepository);
        //    AddClosedTables.AddGLAccountType(new GLAccountTypeDetails() { Code = "4", EnglishName = "Job", LocalName = "ג'וב" }, glAccountTypeRepository);
        //    AddClosedTables.AddGLAccountType(new GLAccountTypeDetails() { Code = "5", EnglishName = "File", LocalName = "תיק" }, glAccountTypeRepository);
        //    glAccountTypeRepository.SubmitChanges();
        public enum GLAccountTypeEnum
        {
            Card=1,
            Client=2,
            Vendor=3,
            Job=4,
            File=5
        }
    }
}
