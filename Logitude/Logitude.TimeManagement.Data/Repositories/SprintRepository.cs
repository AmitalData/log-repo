
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.TimeManagement.Data.EntityPOCOs;
using Logitude.TimeManagement.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.TimeManagement.Data.Repositories
{
    public partial class SprintRepository : IRepository<Sprint>
    {

        public List<Sprint> GetMulti(EntityKeyFields entityKeys)
        {

            throw new NotImplementedException();
        }

        public Sprint GetSprintByName(string name, int tenant)
        {
            Sprint sprint = (from d in context.Sprints
                             where d.Tenant == tenant && d.Name == name
                             select d).FirstOrDefault();
            return sprint;

        }

    }

}
