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

    public partial class SupplierInvioceItemCertificatListQueryService
    {
	    private IQueryable<SupplierInvioceItemCertificatList> GetIqueryableList(IQueryable<SupplierInvioceItemCertificat> iQueryable)
        {
            IQueryable<SupplierInvioceItemCertificatList> query = (from a in iQueryable
                                                                     select new SupplierInvioceItemCertificatList()
                                                {
                                                   DeclarationId = a.DeclarationId,
                                                   AttachmentTypeCode = a.AttachmentTypeCode,
                                                   CertificateExemptionTypeCode =a.CertificateExemptionTypeCode,
                                                   CertificateNumber =a.CertificateNumber,
                                                   ReqConfirmationTypeCode = a.ReqConfirmationTypeCode,
                                                   InvoiceCounterKey = a.InvoiceCounterKey,
                                                   ItemCertificateCounterKey = a.ItemCertificateCounterKey,
                                                   ResConfirmationTypeCode = a.ResConfirmationTypeCode,
                                                   LineNumber = a.LineNumber,
                                                   Tenant = a.Tenant,
                                                   CustomsAttachmentID = a.CustomsAttachmentID,
                                                   SequenceNumeric = a.SequenceNumeric,
                                                   ExternalCertificatCode = a.ExternalCertificatCode,
                                                });
            return query;
		}

        private IQueryable<SupplierInvioceItemCertificat> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<SupplierInvioceItemCertificat> iQueryable, int tenant)
        {
            return iQueryable;
        }
	}


}
	