 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Social.Data.EntityPOCOs;
using Logitude.Social.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.Social.Data.Repsitories
{
   public partial class PostLikeRepository:IRepository<PostLike>
   {

       public List<PostLike> GetMulti(EntityKeyFields entityKeys)
       {

           PostKeys postKeys = entityKeys as PostKeys;

           return (from a in context.PostLikes
                   where a.PostId == postKeys.Id && a.IsCancelled == false
                   select a).ToList();
       }

   }

}
