
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
    public partial class ExceptionReasonRepository : IRepository<ExceptionReason>
    {

        public List<ExceptionReason> GetMulti(EntityKeyFields entityKeys)
        {

            throw new NotImplementedException();
        }
        public List<ExceptionReason> GetExceptionReasonByUnifreightStatus(string unifreightStatusCode)
        {
            List<ExceptionReason> selectedexceptionReasons = (from exceptionReasons in context.ExceptionReasons
                                                              where exceptionReasons.UnifreightStatusCode == unifreightStatusCode && exceptionReasons.IsActive
                                                              select exceptionReasons).ToList();
            return selectedexceptionReasons;
        }

    }

}
