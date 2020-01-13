using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data.EntityLists;
using Logitude.Customs.Data.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Logitude.Customs.BL.EntityQueryServices
{
    public partial class PhysicalCheckQueryService
    {
        //UQ_Tenant_CheckId_PhysicalChecks
        public string GetIdByCheckId(string CheckId, int tenant)
        {
            if (String.IsNullOrWhiteSpace(CheckId)) return "";
            return repository.GetIdByCheckId(CheckId, tenant);
        }

        public List<PhysicalCheckList> GetPhysicalChecksByDeclarationId(string declarationId, int tenant)
        {
            List<PhysicalCheck>  checks=  repository.GetPhysicalChecksByDeclarationId(declarationId, tenant);
            List<PhysicalCheckList> checkLists = new List<PhysicalCheckList>();
            foreach (PhysicalCheck a in checks)
            {
                PhysicalCheckList check = new PhysicalCheckList()
                {
                    Id = a.Id,
                    Tenant = a.Tenant,
                   // DeclarationId = a.DeclarationId,
                    CheckSiteCode = a.CheckSiteCode,
                   DeclarationNo = a.Declaration != null ? a.Declaration.DeclarationNumber : null,
                   CustomerName = a.Declaration.CustomerCard != null? a.Declaration.CustomerCard.LocalName : null,
                   StorageSiteName = a.StorageSite != null ? a.StorageSite.LocalName : null,
                   CheckSiteName = a.CheckSite != null? a.CheckSite.LocalName : null,
                   QueueTypeName  = a.CheckQueueType != null ? a.CheckQueueType.LocalName : null,
                   LimitDate = a.LimitDate,
                   CheckId = a.CheckId,
                   ContainerNubmer = a.ContainerNubmer,
                   OperationName = a.Operation != null ? a.Operation.LocalName : null,
                };
                checkLists.Add(check);
            }

            return checkLists;
        }

        //<--- Yuval Chalup 17.11.2014 TASK-9089
        public List<PhysicalCheck> GethysicalCheckByDeclarationIdOnly(string declarationId, int tenant)
        {
            List<PhysicalCheck> physicalCheckList = repository.GethysicalCheckByDeclarationIdOnly(declarationId, tenant);

            return physicalCheckList;
        }

        //Yuval Chalup 17.11.2014 TASK-9089 --->

        public Card GetCustomerNameByDeclartionNo(string declartionNumber, int tenant)
        {
            Card card = new Card();
            var declaration = context.Declarations.FirstOrDefault(x => x.DeclarationNumber == declartionNumber);
            if (declaration != null)
            {
                card = context.Cards.FirstOrDefault(x => x.Id == declaration.CustomerId);
                if (card != null)
                    return card;
            }
            return card;
        }
        public string GetCheckTypByCode(string code)
        {
            var type= context.CheckTypeLookups.FirstOrDefault(x => x.Code == code);
            if (type != null)
            {
                return type.LocalName;
            }
            return "";
        }
        public string GetCustomFileNoByCheckId(string Id,int tenant)
        {
            DeclarationQueryService declarationQuery = new DeclarationQueryService(tenant);
            var declaration = context.Declarations.FirstOrDefault(x => x.DeclarationNumber == Id);
            if (declaration != null)
            {
                return declaration.CustomFileNo;
            }
            return "";
            
        }


    }
}
