using Logitude.Accounting.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.EntityQueryServices
{
   public partial class GLAccountTypeQueryService
    {


        public GLAccountTypePM GetSinglePM(string code, int tenant)
        {
            GLAccountTypePM entityPM = null;

            entityPM =
                (from a in repository.All()
                 where a.Code == code 
                 select new GLAccountTypePM()
                 {
                     Code = a.Code,
                     LocalName = a.LocalName,
                     EnglishName = a.EnglishName,
                     Inactive = a.Inactive,
                     

                    
                 }).FirstOrDefault();

            return entityPM;
        }

        public GLAccountTypePM GetSinglePM(string code )
        {
            GLAccountTypePM entityPM = null;

            entityPM =
                (from a in repository.All()
                 where a.Code == code
                 select new GLAccountTypePM()
                 {
                     Code = a.Code,
                     LocalName = a.LocalName,
                     EnglishName = a.EnglishName,
                     Inactive = a.Inactive,



                 }).FirstOrDefault();

            return entityPM;
        }





    }
}
