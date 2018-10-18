 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Infrastructure.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.Infrastructure.Data.Repsitories
{
   public partial class LBPTeamMemberRepository:IRepository<LBPTeamMember>
   {
		public List<LBPTeamMember> GetMulti(EntityKeyFields entityKeys)
        {
            TeamKeys myEntityKeys = entityKeys as TeamKeys;
            return (from a in context.LBPTeamMembers where a.TeamId == myEntityKeys.Id select a).ToList();
        }
   }
}
   