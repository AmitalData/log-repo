 
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
   public partial class TransportMeansTypeRepository:IRepository<TransportMeansType>
   {
   
        private ICustomContext currentContext;
        public TransportMeansTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public TransportMeansTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  TransportMeansType GetSingle(string code)
        {
            return (from a in context.TransportMeansTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<TransportMeansType> GetAll()
        {
            return from a in context.TransportMeansTypes  
                   select a;
        }
				 
        public TransportMeansType GetSingle(EntityKeyFields entityKeys)
        {
            TransportMeansTypeKeys keys = entityKeys as TransportMeansTypeKeys;
            return (from a in context.TransportMeansTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(TransportMeansType entity)
        {
            onAdd();
            context.TransportMeansTypes.Add(entity);
        }

        public void Remove(TransportMeansType entity)
        {
            context.TransportMeansTypes.Attach(entity);
            context.TransportMeansTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(TransportMeansType entity)
        {
            onUpdate();
            context.TransportMeansTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<TransportMeansType> All()
        {
            return context.TransportMeansTypes.ToList();
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
	 