 
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
   public partial class TransactionNatureTypeRepository:IRepository<TransactionNatureType>
   {
   
        private ICustomContext currentContext;
        public TransactionNatureTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public TransactionNatureTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  TransactionNatureType GetSingle(string code)
        {
            return (from a in context.TransactionNatureTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<TransactionNatureType> GetAll()
        {
            return from a in context.TransactionNatureTypes  
                   select a;
        }
				 
        public TransactionNatureType GetSingle(EntityKeyFields entityKeys)
        {
            TransactionNatureTypeKeys keys = entityKeys as TransactionNatureTypeKeys;
            return (from a in context.TransactionNatureTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(TransactionNatureType entity)
        {
            onAdd();
            context.TransactionNatureTypes.Add(entity);
        }

        public void Remove(TransactionNatureType entity)
        {
            context.TransactionNatureTypes.Attach(entity);
            context.TransactionNatureTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(TransactionNatureType entity)
        {
            onUpdate();
            context.TransactionNatureTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<TransactionNatureType> All()
        {
            return context.TransactionNatureTypes.ToList();
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
	 