using System.Collections.Generic;
using System.Linq;
using Simplog.Server.Infrastructure;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Simplog.Data.InfrastructureModel.Repositories
{
    public class TextCodeTypesRepository:IRepository<TextCodeType>
    {

        IWebFreightContext webFreightContext;
        public TextCodeTypesRepository()
        {
            webFreightContext = new WebFreightContext();

        }
        public TextCodeTypesRepository(IWebFreightContext context)
        {
            webFreightContext = context;

        }
        public TextCodeTypesRepository(int tenant)
        {
            webFreightContext = WebFreightContext.GetContext(tenant);
        }
        public IQueryable<TextCodeType> GetTextCodeTypes()
        {
            return webFreightContext.TextCodeTypes.OrderBy(d => d.Name);
        }

        public TextCodeType GetSingleTextCodeType(string code)
        {
            return (from a in context.TextCodeTypes
                    where a.Code == code
                    select a).FirstOrDefault();
        }

        public void Add(TextCodeType entity)
        {
            webFreightContext.TextCodeTypes.Add(entity);
        }

        public void Remove(TextCodeType entity)
        {
            webFreightContext.TextCodeTypes.Remove(entity);
        }

        public void Update(TextCodeType entity)
        {
            webFreightContext.TextCodeTypes.Attach(entity);
            webFreightContext.SetAsModified(entity);
        }

        public List<TextCodeType> All()
        {
            return webFreightContext.TextCodeTypes.ToList();
        }

        public IWebFreightContext context
        {
            get { return webFreightContext; }
        }

        public void SubmitChanges()
        {
            webFreightContext.SaveChanges();
        }


        public List<TextCodeType> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public TextCodeType GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}