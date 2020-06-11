 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.Customs.Data.Repsitories
{
    public partial class ReferantExceptionRepository : IRepository<ReferantException>
    {

        public List<ReferantException> GetMulti(EntityKeyFields entityKeys)
        {

            throw new NotImplementedException();
        }
        public List<ReferantException> GetByDecId(string decId)
        {

            List<ReferantException> selectedReferantException = (from ReferantException in context.ReferantExceptions
                                                                 where ReferantException.DeclarationId == decId
                                                                 select ReferantException).ToList();
            return selectedReferantException;

        }
    }

}
   