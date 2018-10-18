using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unifreight.Data.AmitalModel;
using Unifreight.Data.AmitalModel.Repsitories;


namespace Unifreight.BL.EntityQueryServices
{
    public class DualQueryService
    {
        private DbContext MainContext;
        private DualRepository Repository;
        public DualQueryService(DbContext context)
        {
            MainContext = context;
            Repository = new DualRepository(context);
        }
        public DateTime? GetServerDateTime()
        {
            return Repository.GetServerDateTime();
            
        }
    }
}
