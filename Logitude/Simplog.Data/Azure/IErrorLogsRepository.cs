using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Simplog.Server.Infrastructure.Azure;

namespace Simplog.Data.Azure
{
  public  interface IErrorLogsRepository<TEntity>
    {
        void Add(TEntity entity);

        void Remove(TEntity entity);

        void Update(TEntity entity);

        List<TEntity> All();

        //IErrorLogContext context { get; }

        void SubmitChanges();
    }
}
