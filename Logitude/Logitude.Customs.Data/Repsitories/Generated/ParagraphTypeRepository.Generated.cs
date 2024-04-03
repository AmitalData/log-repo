 
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
   public partial class ParagraphTypeRepository:IRepository<ParagraphType>
   {
   
        private ICustomContext currentContext;
        public ParagraphTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public ParagraphTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  ParagraphType GetSingle(string code)
        {
            return (from a in context.ParagraphTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<ParagraphType> GetAll()
        {
            return from a in context.ParagraphTypes  
                   select a;
        }
				 
        public ParagraphType GetSingle(EntityKeyFields entityKeys)
        {
            ParagraphTypeKeys keys = entityKeys as ParagraphTypeKeys;
            return (from a in context.ParagraphTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ParagraphType entity)
        {
            onAdd();
            context.ParagraphTypes.Add(entity);
        }

        public void Remove(ParagraphType entity)
        {
            context.ParagraphTypes.Attach(entity);
            context.ParagraphTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ParagraphType entity)
        {
            onUpdate();
            context.ParagraphTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ParagraphType> All()
        {
            return context.ParagraphTypes.ToList();
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
	 