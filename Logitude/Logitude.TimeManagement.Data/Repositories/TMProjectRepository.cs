 
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
    public partial class TMProjectRepository : IRepository<TMProject>
    {

        public List<TMProject> GetMulti(EntityKeyFields entityKeys)
        {

            throw new NotImplementedException();
        }

        public string GetTMProjectByNumber(string number, int tenant)
        {
            string projectId = null;
            var project = (from d in context.TMProjects
                          where d.Tenant == tenant
                          && d.ProjectNumber == number
                          select d).FirstOrDefault();

            if (project != null)
            {
                projectId = project.Id;

            }
            return projectId;

        }


        public List<string> GetInnerTMProjectByNumber(string number, int tenant)
        {
            List<string> list = (from d in context.TMProjects
                           where d.Tenant == tenant
                           && d.ProjectNumber.StartsWith(number+"-")
                           select d.ProjectNumber).ToList();            
            return list;

        }    
    }

}
   