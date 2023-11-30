using System;
using System.Collections.Generic;
using System.Linq;

using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
namespace Simplog.Data.InfrastructureModel.Repositories
{
    public class DataTypeRepository:IRepository<FieldDataType>
    {
        IWebFreightContext webFreightContext;
        public DataTypeRepository(IWebFreightContext context)
        {
            webFreightContext = context;

        }
        public DataTypeRepository(int tenant)
        {
            webFreightContext = WebFreightContext.GetContext(tenant);
        }
        public DataTypeRepository()
        {
               webFreightContext=new WebFreightContext(); 
        }
        public void Add(FieldDataType entity)
        {
            context.FieldDataTypes.Add(entity);
        }

        public List<FieldDataType> GetDataTypes()
        {
            var excludedFieldDataTypes = GetExcludedFieldDataTypes();
            List<FieldDataType> fieldDataTypes = context.FieldDataTypes
                .Where(d => !excludedFieldDataTypes.Contains(d.Code)).ToList();

            return fieldDataTypes;
        }

        private List<string> GetExcludedFieldDataTypes()
        {
            List<string> excludedFieldDataTypes = new List<string>();
            excludedFieldDataTypes.Add("Byte[]");
            excludedFieldDataTypes.Add("Emails");
            excludedFieldDataTypes.Add("Constant");
            excludedFieldDataTypes.Add("List");
            excludedFieldDataTypes.Add("SigDouble");
            excludedFieldDataTypes.Add("UnsDecimal");
            excludedFieldDataTypes.Add("UnsInteger");
            excludedFieldDataTypes.Add("Raw");
            excludedFieldDataTypes.Add("Binary");
            excludedFieldDataTypes.Add("Text");
            excludedFieldDataTypes.Add("Integer");
            excludedFieldDataTypes.Add("Double");
            excludedFieldDataTypes.Add("BigInteger");
            return excludedFieldDataTypes;
        }

        public void Remove(FieldDataType entity)
        {
            context.FieldDataTypes.Attach(entity);
            context.FieldDataTypes.Remove(entity);
        }

        public void Update(FieldDataType entity)
        {
            context.FieldDataTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<FieldDataType> All()
        {
            return context.FieldDataTypes.ToList();
        }

        public IWebFreightContext context
        {
            get { return webFreightContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<FieldDataType> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public FieldDataType GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}