using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.EntityLists;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.Customs.Data.Utils;

namespace Logitude.Customs.Data.EntityListQueryServices
{

    public partial class CustomsCollateralListQueryService
    {
        private IQueryable<CustomsCollateralList> GetIqueryableList(IQueryable<CustomsCollateral> iQueryable)
        {
            CustomsCollateralsAnswerListQueryService customsCollateralsAnswerListQueryService = new CustomsCollateralsAnswerListQueryService(context);


            IQueryable<CustomsCollateralList> query = (from a in iQueryable.Include("CollateralRequestStatus").Include("EntityTypeLookup").Include("RequestedCollateralType")
                                                       select new CustomsCollateralList()
                                                       {
                                                           DeclarationId = a.DeclarationId,
                                                           CollateralRequestNumber = a.CollateralRequestNumber,
                                                           CollateralRequestStatusCode = a.CollateralRequestStatusCode,
                                                           CollateralValidityDate = a.CollateralValidityDate,
                                                           CustomsEntityTypeCode = a.CustomsEntityTypeCode,
                                                           CustomsHouseTypeCode = a.CustomsHouseTypeCode,
                                                           EntityIdKey1 = a.EntityIdKey1,
                                                           EntityIdKey2 = a.EntityIdKey2,
                                                           EntityIdKey3 = a.EntityIdKey3,
                                                           FileNo = a.FileNo,
                                                           Id = a.Id,
                                                           OrganizationUnitTypeCode = a.OrganizationUnitTypeCode,
                                                           Remarks = a.Remarks,
                                                           RequestedCollateralTypeCode = a.RequestedCollateralTypeCode,
                                                           RequestValidityDate = a.RequestValidityDate,
                                                           WorkerName = a.WorkerName,
                                                           Tenant = a.Tenant,
                                                           IncludingThirdPartyGuarantee = a.IncludingThirdPartyGuarantee,
                                                           SearchFields = a.SearchFields,
                                                           CollateralRequestStatusName = a.CollateralRequestStatus.LocalName,
                                                           CustomsEntityTypeName = a.EntityTypeLookup.LocalName,
                                                           RequestedCollateralTypeName = a.RequestedCollateralType.LocalName,
                                                           OrganizationUnitTypeName = a.OrganizationUnitType.LocalName,
                                                           CustomsHouseTypeName = a.CustomsHouseType.LocalName,
                                                           IsClosed = a.IsClosed,
                                                           CreateDateTime = a.CreateDateTime,
                                                           CustomerName = a.Customer != null ? a.Customer.Card.LocalName : null,
                                                           //IsAnswer= test.Where(x=>x.CustomsCollateralId== a.CollateralRequestNumber).Any()
                                                       });

            if (query.Count() > 0)
            {
                var query2 = query.ToList();

                var customsCollateralsAnswerList = customsCollateralsAnswerListQueryService.GetList(query.First().Tenant);

                foreach (var item in query2)
                {
                    item.IsAnswer = customsCollateralsAnswerList.Where(x => x.CustomsCollateralId == item.Id).Any();
                }
                query = query2.AsQueryable();
            }
            return query;
        }

        private IQueryable<CustomsCollateral> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<CustomsCollateral> iQueryable, int tenant)
        {
            var filterDeclaration = queryOperations.QueryFilterItems.FirstOrDefault(x => x.FieldName == "DeclarationId");
            if(filterDeclaration!=null)
            {
             string declarationId = filterDeclaration.FieldValue.ToString();
                var declaration = context.Declarations.FirstOrDefault(x => x.Id == declarationId);
            var declarations = context.Declarations.Where(x => x.AmendmentOriginalDeclartation == declarationId || x.Id== declaration.AmendmentOriginalDeclartation).ToList();

 
                List<string> DeclarationsIds = new List<string>();
            declarations.ForEach(x => DeclarationsIds.Add(x.Id));
            DeclarationsIds.Add(declarationId);

            iQueryable = iQueryable.Where(x => DeclarationsIds.Contains( x.DeclarationId));
            }
        

            // Freelancer filterting
            FreelancerCustomersUtil frlUtil = new FreelancerCustomersUtil(tenant);
            if (frlUtil.user.IsFreelancer)
            {
                List<string> customersIds = frlUtil.GetConnectedCustomersIds(tenant);
                if (customersIds.Count > 0)
                {
                    iQueryable = iQueryable.Where(d => customersIds.Contains(d.CustomerId));
                }
            }

            return iQueryable;
        }
    }


}
