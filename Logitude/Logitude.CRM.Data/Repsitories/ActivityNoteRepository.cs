 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.CRM.Data.Repsitories
{
   public partial class ActivityNoteRepository:IRepository<ActivityNote>
   {        
		public List<ActivityNote> GetMulti(EntityKeyFields entityKeys)
        {
            ActivityKeys myEntityKeys = entityKeys as ActivityKeys;
            return (from a in context.ActivityNotes where a.ActivityId == myEntityKeys.Id select a).ToList();
        }

   }

}
   