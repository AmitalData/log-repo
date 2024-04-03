 
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
   public partial class PartyRelationshipTypeRepository:IRepository<PartyRelationshipType>
   {
   
        private ICustomContext currentContext;
        public PartyRelationshipTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public PartyRelationshipTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  PartyRelationshipType GetSingle(string code)
        {
            return (from a in context.PartyRelationshipTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<PartyRelationshipType> GetAll()
        {
            return from a in context.PartyRelationshipTypes  
                   select a;
        }
				 
        public PartyRelationshipType GetSingle(EntityKeyFields entityKeys)
        {
            PartyRelationshipTypeKeys keys = entityKeys as PartyRelationshipTypeKeys;
            return (from a in context.PartyRelationshipTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(PartyRelationshipType entity)
        {
            onAdd();
            context.PartyRelationshipTypes.Add(entity);
        }

        public void Remove(PartyRelationshipType entity)
        {
            context.PartyRelationshipTypes.Attach(entity);
            context.PartyRelationshipTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(PartyRelationshipType entity)
        {
            onUpdate();
            context.PartyRelationshipTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<PartyRelationshipType> All()
        {
            return context.PartyRelationshipTypes.ToList();
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
	 