 
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
using System.Diagnostics;

namespace Logitude.Customs.Data.Repsitories
{
   public partial class CourierDeclarationRepository:IRepository<CourierDeclaration>
   {
        
		public List<CourierDeclaration> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }

        public int? GetCourierMasterMaxSequenceNumeric(string courierMasterId, int tenant)
        {
            //List<CourierDeclaration> courierDeclarationList = (from a in context.CourierDeclarations
            //        where a.CourierMasterId == courierMasterId && a.Tenant == tenant
            //        select a).ToList();
            //int? courierDeclaration = courierDeclarationList.Max(rec => rec.SequenceNumeric);

            return (from a in context.CourierDeclarations
                    where a.CourierMasterId == courierMasterId && a.Tenant == tenant
                    select a).Max(rec => rec.SequenceNumeric);
        }


        public void FastDeleteMulti(List<string> declarationIds, int tenant, string courierMasterId, out List<string> deletedDeclarationIds)
        {
            var q = (from a in context.CourierDeclarations
                     where declarationIds.Contains(a.DeclarationId)
                     where a.Tenant == tenant && a.CourierMasterId != courierMasterId
                     select a.DeclarationId);
            deletedDeclarationIds = q.ToList();
            List<string> declarationIdsDel = q.ToList();
            (context as DbContextBase).DeleteWhere<CourierDeclaration>(rec => declarationIdsDel.Contains(rec.DeclarationId));
        }

        public IQueryable<CourierDeclaration> GetByCourierMasterId(int tenant, string courierMasterId)
        {
            return (from a in context.CourierDeclarations
                    where a.CourierMasterId == courierMasterId && a.Tenant == tenant
                    select a);
        }
        public string GetFirstDeclarationCustomFileByCourierMasterId(int tenant, string courierMasterId)
        {
            var declaration = (from a in context.Declarations
                               join c in context.CourierDeclarations on a.Id equals c.DeclarationId
                    where c.CourierMasterId == courierMasterId && a.Tenant == tenant
                    select a).FirstOrDefault();
            if(declaration != null)
            {
                return declaration.CustomFileNo;
            }
            return null;
        }

        public CourierDeclaration GetCourierDeclarationByDeclarationId(string declarationId, int tenant)
        {
            CourierDeclaration courierDeclaration = (from a in context.CourierDeclarations
                                                     where a.DeclarationId == declarationId && a.Tenant == tenant
                                                     select a).FirstOrDefault();

            return courierDeclaration;
        }

        public string GetMAWBCourierMasterByDeclarationId(string declarationId, int tenant)
        {
            string MAWBCourierMaster = null;
            CourierDeclaration courierDeclaration = (from a in context.CourierDeclarations
                                                     where a.DeclarationId == declarationId && a.Tenant == tenant
                                                     select a).FirstOrDefault();

            if(courierDeclaration != null)
            {
                MAWBCourierMaster = (from a in context.CourierMasters
                                    where a.Id == courierDeclaration.CourierMasterId && a.Tenant == courierDeclaration.Tenant
                                    select a).FirstOrDefault().MAWB;
            }
            return MAWBCourierMaster;
        }

        public List<string> GetCourierConnectedDeclaratinsList(string CourierMasterId, int tenant)
        {
            List<string> courierDeclarations = (from a in context.CourierDeclarations
                                                where a.CourierMasterId == CourierMasterId && a.Tenant == tenant
                                                select a.DeclarationId).ToList();

            return courierDeclarations;
        }
        public string GetCourierMasterIdByDeclarationId(string declarationId, int tenant)
        {
            string courierMasterId = (from a in context.CourierDeclarations
                                   where a.DeclarationId == declarationId && a.Tenant == tenant
                                   select a).FirstOrDefault()?.CourierMasterId;
            return courierMasterId;
        }
        public List<string> GetDeclarationIdsByCourierMasterID(string courierMasterId, int tenant)
        {
            var decList = (from a in context.CourierDeclarations
                           where a.CourierMasterId == courierMasterId && a.Tenant == tenant
                           select a.DeclarationId).ToList();
            return decList;
        }
        public List<string> GetDeclarationIdsByCourierMasterIDWithNoCourierCustomStatus(string courierMasterId, int tenant)
        {
            var decList = (from a in context.CourierDeclarations 
                           where a.CourierMasterId == courierMasterId && a.Tenant == tenant && a.Declaration.CourierCustomStatus== null
                           && a.Declaration.PaymentDate != null 
                           select a.DeclarationId).ToList();
            return decList;

        }
        public int CounNoOfCourierHawbwWithoutHatara(string couriermasterid, int tenant)
        {
            return (from a in context.CourierDeclarations
                           where a.CourierMasterId == couriermasterid && a.Tenant == tenant && a.Declaration.HatraDate == null
                           select a).Count();
        }
        public int CountNoOfCourierHawbWithoutDelivery(string couriermasterid, int tenant)
        {
            return (from a in context.CourierDeclarations
                    join s in context.DeclarationCourierStatuses on a.DeclarationId equals s.DeclarationId 
                    where a.CourierMasterId == couriermasterid && a.Tenant == tenant && a.Declaration.HatraDate != null && !s.Delivered 
                    select a).Count();
        }

        public List<string> GetCourierDeclarationToInsert(List<string> declarationIds, string couriermasterid, int tenant)
        {
            var q = (from a in context.CourierDeclarations
                     where declarationIds.Contains(a.DeclarationId)
                     where a.Tenant == tenant && a.CourierMasterId == couriermasterid
                     select a.DeclarationId);

            var containsId= q.ToList();
            var res = declarationIds.Where(r => !containsId.Contains(r)).ToList();
            return res;
        }
    }

}
   