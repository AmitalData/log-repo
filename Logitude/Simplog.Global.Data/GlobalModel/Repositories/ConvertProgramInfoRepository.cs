using System.Collections.Generic;
using System.Linq;
using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Global.Data.GlobalModel.Repositories
{
    public class ConvertProgramInfoRepository : IRepository<ConvertProgramInfo>
    {
        IGlobalContext globalContext;
        public ConvertProgramInfoRepository(IGlobalContext context)
        {
            globalContext = context;
        }
        public ConvertProgramInfoRepository()
        {

            globalContext = GlobalContext.GetContext();
        }

        public IQueryable<ConvertProgramInfo> GetConvertProgramInfoes()
        {
            return context.ConvertProgramInfoes;
        }

        public bool IsMethodApplied(string methodName,string globalDbId)
        {
            ConvertProgramInfo info = (from a in context.ConvertProgramInfoes
                                       where a.MethodName == methodName && a.GlobalDBId == globalDbId
                                       select a).FirstOrDefault();
            if (info != null)
            {
                return info.IsApplied;
            }
            else
            {
                return false;
            }
        }

        public void Add(ConvertProgramInfo entity)
        {
            context.ConvertProgramInfoes.Add(entity);
        }

        public void Remove(ConvertProgramInfo entity)
        {
            context.ConvertProgramInfoes.Attach(entity);
            context.ConvertProgramInfoes.Remove(entity);
        }

        public void Update(ConvertProgramInfo entity)
        {
            context.ConvertProgramInfoes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ConvertProgramInfo> All()
        {
            return context.ConvertProgramInfoes.ToList();
        }

        public IGlobalContext context
        {
            get { return globalContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<ConvertProgramInfo> GetMulti(EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public ConvertProgramInfo GetSingle(EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}
