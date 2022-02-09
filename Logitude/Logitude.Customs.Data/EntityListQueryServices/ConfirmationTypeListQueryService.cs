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

namespace Logitude.Customs.Data.EntityListQueryServices
{

    public partial class ConfirmationTypeListQueryService
    {
        private IQueryable<ConfirmationTypeList> GetIqueryableList(IQueryable<ConfirmationType> iQueryable)
        {
            IQueryable<ConfirmationTypeList> query = (from a in iQueryable
                                                      select new ConfirmationTypeList()
                                                      {
                                                          Code = a.Code,
                                                          EnglishName = a.EnglishName,
                                                          LocalName = a.LocalName,
                                                          SearchFields = a.SearchFields,
                                                          Inactive = a.Inactive,
                                                          MalamID = a.MalamID,
                                                          State = a.State,
                                                          Exempt_CertificateDocument = a.Exempt_CertificateDocument,
                                                          IsImport = a.IsImport,
                                                          IsExemptOtherAuthority = a.IsExemptOtherAuthority,
                                                          ConfirmationComputerization = a.ConfirmationComputerization,
                                                          IsCEO = a.IsCEO,
                                                          IsNeedDeclaration = a.IsNeedDeclaration,
                                                          CertificateDocumentCategory = a.CertificateDocumentCategory,
                                                          AuthorityID = a.AuthorityID,
                                                          IsQuotaCheckNeeded = a.IsQuotaCheckNeeded,
                                                          ExternalIDNumPerAuthority = a.ExternalIDNumPerAuthority,
                                                          IsForCustomsItem = a.IsForCustomsItem,
                                                          IsPharmacy = a.IsPharmacy,
                                                          IsVeterinarian = a.IsVeterinarian,
                                                          IsVehicleStandardization = a.IsVehicleStandardization,
                                                          IsQuantityMandatory = a.IsQuantityMandatory,
                                                          IsForCE = a.IsForCE,
                                                      });
            return query;
        }

        private IQueryable<ConfirmationType> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<ConfirmationType> iQueryable)
        {
            return iQueryable;
        }
    }


}
