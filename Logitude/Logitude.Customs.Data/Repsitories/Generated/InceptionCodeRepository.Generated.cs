 
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
   public partial class InceptionCodeRepository:IRepository<InceptionCode>
   {
   
        private ICustomContext currentContext;
        public InceptionCodeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public InceptionCodeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  InceptionCode GetSingle(string code)
        {
            return (from a in context.InceptionCodes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<InceptionCode> GetAll()
        {
            return from a in context.InceptionCodes  
                   select a;
        }
				 
        public InceptionCode GetSingle(EntityKeyFields entityKeys)
        {
            InceptionCodeKeys keys = entityKeys as InceptionCodeKeys;
            return (from a in context.InceptionCodes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(InceptionCode entity)
        {
            onAdd();
            context.InceptionCodes.Add(entity);
        }

        public void Remove(InceptionCode entity)
        {
            context.InceptionCodes.Attach(entity);
            context.InceptionCodes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(InceptionCode entity)
        {
            onUpdate();
            context.InceptionCodes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<InceptionCode> All()
        {
            return context.InceptionCodes.ToList();
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
	 