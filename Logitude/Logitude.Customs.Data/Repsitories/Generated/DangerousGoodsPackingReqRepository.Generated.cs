 
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
   public partial class DangerousGoodsPackingReqRepository:IRepository<DangerousGoodsPackingReq>
   {
   
        private ICustomContext currentContext;
        public DangerousGoodsPackingReqRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public DangerousGoodsPackingReqRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  DangerousGoodsPackingReq GetSingle(string code)
        {
            return (from a in context.DangerousGoodsPackingReqs
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<DangerousGoodsPackingReq> GetAll()
        {
            return from a in context.DangerousGoodsPackingReqs  
                   select a;
        }
				 
        public DangerousGoodsPackingReq GetSingle(EntityKeyFields entityKeys)
        {
            DangerousGoodsPackingReqKeys keys = entityKeys as DangerousGoodsPackingReqKeys;
            return (from a in context.DangerousGoodsPackingReqs
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(DangerousGoodsPackingReq entity)
        {
            onAdd();
            context.DangerousGoodsPackingReqs.Add(entity);
        }

        public void Remove(DangerousGoodsPackingReq entity)
        {
            context.DangerousGoodsPackingReqs.Attach(entity);
            context.DangerousGoodsPackingReqs.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(DangerousGoodsPackingReq entity)
        {
            onUpdate();
            context.DangerousGoodsPackingReqs.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<DangerousGoodsPackingReq> All()
        {
            return context.DangerousGoodsPackingReqs.ToList();
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
	 