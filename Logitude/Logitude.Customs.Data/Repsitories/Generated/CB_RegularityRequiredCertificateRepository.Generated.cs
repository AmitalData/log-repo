 
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
   public partial class CB_RegularityRequiredCertificateRepository:IRepository<CB_RegularityRequiredCertificate>
   {
   
        private ICustomContext currentContext;
        public CB_RegularityRequiredCertificateRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CB_RegularityRequiredCertificateRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CB_RegularityRequiredCertificate GetSingle(string cb_id)
        {
            return (from a in context.CB_RegularityRequiredCertificates
                    where a.CB_ID == cb_id 
                    select a).FirstOrDefault();
        }

        public IQueryable<CB_RegularityRequiredCertificate> GetAll()
        {
            return from a in context.CB_RegularityRequiredCertificates  
                   select a;
        }
				 
        public CB_RegularityRequiredCertificate GetSingle(EntityKeyFields entityKeys)
        {
            CB_RegularityRequiredCertificateKeys keys = entityKeys as CB_RegularityRequiredCertificateKeys;
            return (from a in context.CB_RegularityRequiredCertificates
                    where a.CB_ID == keys.CB_ID
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CB_RegularityRequiredCertificate entity)
        {
            onAdd();
            context.CB_RegularityRequiredCertificates.Add(entity);
        }

        public void Remove(CB_RegularityRequiredCertificate entity)
        {
            context.CB_RegularityRequiredCertificates.Attach(entity);
            context.CB_RegularityRequiredCertificates.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CB_RegularityRequiredCertificate entity)
        {
            onUpdate();
            context.CB_RegularityRequiredCertificates.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CB_RegularityRequiredCertificate> All()
        {
            return context.CB_RegularityRequiredCertificates.ToList();
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
	 