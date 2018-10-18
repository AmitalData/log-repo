 
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
   public partial class ProceduralFaultsConnEntityRepository:IRepository<ProceduralFaultsConnEntity>
   {

       public List<ProceduralFaultsConnEntity> GetMulti(EntityKeyFields entityKeys)
       {

           ProceduralFaultKeys proceduralFaultKeys = entityKeys as ProceduralFaultKeys;

           return (from a in context.ProceduralFaultsConnEntities
                   where a.ProceduralFaultId == proceduralFaultKeys.Id 
                   select a).ToList();
       }


   }

}
   