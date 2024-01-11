 
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
   public partial class RegularitySourceRepository:IRepository<RegularitySource>
   {
   
        private ICustomContext currentContext;
        public RegularitySourceRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public RegularitySourceRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  RegularitySource GetSingle(string code)
        {
            return (from a in context.RegularitySources
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<RegularitySource> GetAll()
        {
            return from a in context.RegularitySources  
                   select a;
        }
				 
        public RegularitySource GetSingle(EntityKeyFields entityKeys)
        {
            RegularitySourceKeys keys = entityKeys as RegularitySourceKeys;
            return (from a in context.RegularitySources
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(RegularitySource entity)
        {
            onAdd();
            context.RegularitySources.Add(entity);
        }

        public void Remove(RegularitySource entity)
        {
            context.RegularitySources.Attach(entity);
            context.RegularitySources.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(RegularitySource entity)
        {
            onUpdate();
            context.RegularitySources.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<RegularitySource> All()
        {
            return context.RegularitySources.ToList();
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
	 