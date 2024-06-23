 
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
   public partial class NUnicloudRepository:IRepository<NUnicloud>
   {
   
        private ICustomContext currentContext;
        public NUnicloudRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public NUnicloudRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  NUnicloud GetSingle(string code)
        {
            return (from a in context.NUniclouds
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<NUnicloud> GetAll()
        {
            return from a in context.NUniclouds  
                   select a;
        }
				 
        public NUnicloud GetSingle(EntityKeyFields entityKeys)
        {
            NUnicloudKeys keys = entityKeys as NUnicloudKeys;
            return (from a in context.NUniclouds
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(NUnicloud entity)
        {
            onAdd();
            context.NUniclouds.Add(entity);
        }

        public void Remove(NUnicloud entity)
        {
            context.NUniclouds.Attach(entity);
            context.NUniclouds.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(NUnicloud entity)
        {
            onUpdate();
            context.NUniclouds.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<NUnicloud> All()
        {
            return context.NUniclouds.ToList();
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
	 