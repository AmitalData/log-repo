using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.SystemLogs
{
   public interface IErrorLogRepository<TEntity>
    {
        void Add(TEntity entity);

        void Remove(TEntity entity);

        void Update(TEntity entity);

        List<TEntity> All();
   
        void SubmitChanges();
    }
}
