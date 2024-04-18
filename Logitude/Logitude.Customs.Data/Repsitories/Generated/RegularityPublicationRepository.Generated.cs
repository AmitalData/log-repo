 
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
   public partial class RegularityPublicationRepository:IRepository<RegularityPublication>
   {
   
        private ICustomContext currentContext;
        public RegularityPublicationRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public RegularityPublicationRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  RegularityPublication GetSingle(string code)
        {
            return (from a in context.RegularityPublications
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<RegularityPublication> GetAll()
        {
            return from a in context.RegularityPublications  
                   select a;
        }
				 
        public RegularityPublication GetSingle(EntityKeyFields entityKeys)
        {
            RegularityPublicationKeys keys = entityKeys as RegularityPublicationKeys;
            return (from a in context.RegularityPublications
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(RegularityPublication entity)
        {
            onAdd();
            context.RegularityPublications.Add(entity);
        }

        public void Remove(RegularityPublication entity)
        {
            context.RegularityPublications.Attach(entity);
            context.RegularityPublications.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(RegularityPublication entity)
        {
            onUpdate();
            context.RegularityPublications.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<RegularityPublication> All()
        {
            return context.RegularityPublications.ToList();
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
	 