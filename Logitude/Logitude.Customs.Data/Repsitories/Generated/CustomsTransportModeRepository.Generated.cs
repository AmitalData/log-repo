 
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
   public partial class CustomsTransportModeRepository:IRepository<CustomsTransportMode>
   {
   
        private ICustomContext currentContext;
        public CustomsTransportModeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CustomsTransportModeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CustomsTransportMode GetSingle(string code)
        {
            return (from a in context.CustomsTransportModes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<CustomsTransportMode> GetAll()
        {
            return from a in context.CustomsTransportModes  
                   select a;
        }
				 
        public CustomsTransportMode GetSingle(EntityKeyFields entityKeys)
        {
            CustomsTransportModeKeys keys = entityKeys as CustomsTransportModeKeys;
            return (from a in context.CustomsTransportModes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CustomsTransportMode entity)
        {
            onAdd();
            context.CustomsTransportModes.Add(entity);
        }

        public void Remove(CustomsTransportMode entity)
        {
            context.CustomsTransportModes.Attach(entity);
            context.CustomsTransportModes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CustomsTransportMode entity)
        {
            onUpdate();
            context.CustomsTransportModes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CustomsTransportMode> All()
        {
            return context.CustomsTransportModes.ToList();
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
	 