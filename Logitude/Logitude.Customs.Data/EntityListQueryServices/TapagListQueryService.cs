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

    public partial class TapagListQueryService
    {
	    private IQueryable<TapagList> GetIqueryableList(IQueryable<Tapag> iQueryable)
        {
            IQueryable<TapagList> query = (from a in iQueryable.Include("CustomerCard").Include("CustomsBranch").Include("Importer").Include("ProfessionUnitType").Include("SpecializationType").Include("TapagType").Include("ReferantUser")
                                           select new TapagList()
                                                                 {
                                                                     CreateDate = a.CreateDate,
                                                                     CustomerId = a.CustomerId,
                                                                     CustomerName = a.CustomerCard.LocalName != null ? a.CustomerCard.LocalName : a.CustomerCard.EnglishName,
                                                                     CustomsBranchCode = a.CustomsBranchCode,
                                                                     CustomsBranchName = a.CustomsBranch.LocalName != null ? a.CustomsBranch.LocalName : a.CustomsBranch.EnglishName,
                                                                     FollowDate = a.FollowDate,
                                                                     Id = a.Id,
                                                                     ImporterId = a.ImporterId,
                                                                     ImporterName = a.Importer.LocalFirstName != null ? a.Importer.LocalFirstName : a.Importer.EnglishFirstName,
                                                                     IsClosed = a.IsClosed,
                                                                     LeadingFileNumber = a.LeadingFileNumber,
                                                                     ProfessionUnitTypeCode = a.ProfessionUnitTypeCode,
                                                                     ProfessionUnitTypeName = a.ProfessionUnitType.LocalName != null ? a.ProfessionUnitType.LocalName : a.ProfessionUnitType.EnglishName,
                                                                     SpecializationTypeCode = a.SpecializationTypeCode,
                                                                     SpecializationTypeName = a.SpecializationType.LocalName != null ? a.SpecializationType.LocalName : a.SpecializationType.EnglishName,
                                                                     TapagNumber = a.TapagNumber,
                                                                     TapagTypeCode = a.TapagTypeCode,
                                                                     TapagTypeName = a.TapagType.LocalName != null ? a.TapagType.LocalName : a.TapagType.EnglishName,
                                                                     Tenant = a.Tenant,
                                                                     ValidityDate = a.ValidityDate,
                                                                     ReferantId = a.ReferantId,
                                                                     ReferantName = a.ReferantUser.Contact.LocalName != null ? a.ReferantUser.Contact.LocalName : a.ReferantUser.Contact.EnglishName,
                                                                 });
            return query;
		}

        private IQueryable<Tapag> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<Tapag> iQueryable, int tenant)
        {
            return iQueryable;
        }

       
	}


}
	