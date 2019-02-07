 
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
   public partial class TransferCargoMethodTypeRepository:IRepository<TransferCargoMethodType>
   {
   
        private ICustomContext currentContext;
        public TransferCargoMethodTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public TransferCargoMethodTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  TransferCargoMethodType GetSingle(string code)
        {
            return (from a in context.TransferCargoMethodTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<TransferCargoMethodType> GetAll()
        {
            return from a in context.TransferCargoMethodTypes  
                   select a;
        }
				 
        public TransferCargoMethodType GetSingle(EntityKeyFields entityKeys)
        {
            TransferCargoMethodTypeKeys keys = entityKeys as TransferCargoMethodTypeKeys;
            return (from a in context.TransferCargoMethodTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(TransferCargoMethodType entity)
        {
            onAdd();
            context.TransferCargoMethodTypes.Add(entity);
        }

        public void Remove(TransferCargoMethodType entity)
        {
            context.TransferCargoMethodTypes.Attach(entity);
            context.TransferCargoMethodTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(TransferCargoMethodType entity)
        {
            onUpdate();
            context.TransferCargoMethodTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<TransferCargoMethodType> All()
        {
            return context.TransferCargoMethodTypes.ToList();
        }

        private ICustomContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }
	 
   }
   }
	 