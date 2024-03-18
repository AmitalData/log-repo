 
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
   public partial class CB_RegularityInceptionRepository:IRepository<CB_RegularityInception>
   {
   
        private ICustomContext currentContext;
        public CB_RegularityInceptionRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CB_RegularityInceptionRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CB_RegularityInception GetSingle(int id)
        {
            return (from a in context.CB_RegularityInceptions
                    where a.ID == id 
                    select a).FirstOrDefault();
        }

        public IQueryable<CB_RegularityInception> GetAll()
        {
            return from a in context.CB_RegularityInceptions  
                   select a;
        }
				 
        public CB_RegularityInception GetSingle(EntityKeyFields entityKeys)
        {
            CB_RegularityInceptionKeys keys = entityKeys as CB_RegularityInceptionKeys;
            return (from a in context.CB_RegularityInceptions
                    where a.ID == keys.ID
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CB_RegularityInception entity)
        {
            onAdd();
            context.CB_RegularityInceptions.Add(entity);
        }

        public void Remove(CB_RegularityInception entity)
        {
            context.CB_RegularityInceptions.Attach(entity);
            context.CB_RegularityInceptions.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CB_RegularityInception entity)
        {
            onUpdate();
            context.CB_RegularityInceptions.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CB_RegularityInception> All()
        {
            return context.CB_RegularityInceptions.ToList();
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
	 