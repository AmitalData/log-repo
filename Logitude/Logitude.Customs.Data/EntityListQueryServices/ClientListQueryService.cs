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
using Logitude.Customs.Data.CustomFilters;

namespace Logitude.Customs.Data.EntityListQueryServices
{ 

    public partial class ClientListQueryService
    {
	    private IQueryable<ClientList> GetIqueryableList(IQueryable<Client> iQueryable)
        {
            IQueryable<ClientList> query = (from a in iQueryable
                                            select new ClientList()
                                                       {
                                                         Id= a.Id,
                                                         Code = a.Code,
                                                         SearchFields = a.SearchFields,
                                                         Tenant = a.Tenant,
                                                         ClientTypeSpecificCode = a.ClientTypeSpecificCode,
                                                         IsActive = a.IsActive,
                                                         DunsNumber = a.DunsNumber,
                                                         EnglishBirthPlace =a.EnglishBirthPlace,
                                                         EnglishCorporationName =a.EnglishCorporationName,
                                                         EnglishFatherName = a.EnglishFatherName,
                                                         EnglishFirstName = a.EnglishFirstName,
                                                         EnglishLastName = a.EnglishLastName,
                                                         FullName = a.FullName,
                                                         GenderCode = a.GenderCode,
                                                         LocalCorporationName = a.LocalCorporationName,
                                                         LocalFirstName= a.LocalFirstName,
                                                         LocalLastName = a.LocalLastName,
                                                         PassportCountryCode = a.PassportCountryCode,
                                                         PassportExpirationDate = a.PassportExpirationDate,
                                                         PassportFirstName = a.PassportFirstName,
                                                         PassportIssueDate = a.PassportIssueDate,
                                                         PassportLastName = a.PassportLastName,
                                                         PassportNumber = a.PassportNumber,
                                                         PassportTypeCode = a.PassportTypeCode,
                                                         ClientTypeSpecificName = a.CustomerTypeGeneral == null ? null : a.CustomerTypeGeneral.LocalName,
                                                         GenderName = a.Gender == null ? null : a.Gender.LocalName,
                                                         PassportCountryName = a.Country == null ? null : a.Country.LocalName,
                                                         PassportTypeName = a.PassportType == null ? null : a.PassportType.LocalName,
                                                         BirthDate = a.BirthDate,
                                                         IsImporter = a.IsImporter,
                                                         IsExporter = a.IsExporter,
                                                         FacilitationTypeCode = a.FacilitationTypeCode,
                                                         NationalIdentificationNumber = a.NationalIdentificationNumber,
                                                         IsExportPoaActive = a.IsExportPoaActive,

                                                       });
            return query;
		}

        private IQueryable<Client> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<Client> iQueryable, int tenant)
        {
            ClientCustomFilters filters = new ClientCustomFilters();
            iQueryable = filters.GetFilteredQuery(queryOperations, iQueryable);

            return iQueryable;
        }

        public ClientList GetSingleClientListByCode(string code, int tenant)
        {
            ClientList client = (from a in context.Clients
                                 where a.Code == code && a.Tenant == tenant
                                 select new ClientList()
                                 {
                                     Id = a.Id,
                                     Code = a.Code,
                                    // SearchFields = a.SearchFields,
                                     Tenant = a.Tenant,
                                     ClientTypeSpecificCode = a.ClientTypeSpecificCode,
                                     IsActive = a.IsActive,
                                     DunsNumber = a.DunsNumber,
                                     EnglishBirthPlace = a.EnglishBirthPlace,
                                     EnglishCorporationName = a.EnglishCorporationName,
                                     EnglishFatherName = a.EnglishFatherName,
                                     EnglishFirstName = a.EnglishFirstName,
                                     EnglishLastName = a.EnglishLastName,
                                     FullName = a.FullName,
                                     GenderCode = a.GenderCode,
                                     LocalCorporationName = a.LocalCorporationName,
                                     LocalFirstName = a.LocalFirstName,
                                     LocalLastName = a.LocalLastName,
                                     PassportCountryCode = a.PassportCountryCode,
                                     PassportExpirationDate = a.PassportExpirationDate,
                                     PassportFirstName = a.PassportFirstName,
                                     PassportIssueDate = a.PassportIssueDate,
                                     PassportLastName = a.PassportLastName,
                                     PassportNumber = a.PassportNumber,
                                     PassportTypeCode = a.PassportTypeCode,


                                 }).FirstOrDefault();
            return client;
        }

        public ClientList GetSingleClientListByPassportNumber(string passportNumber, int tenant)
        {
            ClientList client = (from a in context.Clients
                                 where a.PassportNumber == passportNumber && a.Tenant == tenant
                                 select new ClientList()
                                 {
                                     Id = a.Id,
                                     Code = a.Code,
                                     // SearchFields = a.SearchFields,
                                     Tenant = a.Tenant,
                                     ClientTypeSpecificCode = a.ClientTypeSpecificCode,
                                     IsActive = a.IsActive,
                                     DunsNumber = a.DunsNumber,
                                     EnglishBirthPlace = a.EnglishBirthPlace,
                                     EnglishCorporationName = a.EnglishCorporationName,
                                     EnglishFatherName = a.EnglishFatherName,
                                     EnglishFirstName = a.EnglishFirstName,
                                     EnglishLastName = a.EnglishLastName,
                                     FullName = a.FullName,
                                     GenderCode = a.GenderCode,
                                     LocalCorporationName = a.LocalCorporationName,
                                     LocalFirstName = a.LocalFirstName,
                                     LocalLastName = a.LocalLastName,
                                     PassportCountryCode = a.PassportCountryCode,
                                     PassportExpirationDate = a.PassportExpirationDate,
                                     PassportFirstName = a.PassportFirstName,
                                     PassportIssueDate = a.PassportIssueDate,
                                     PassportLastName = a.PassportLastName,
                                     PassportNumber = a.PassportNumber,
                                     PassportTypeCode = a.PassportTypeCode,


                                 }).FirstOrDefault();
            return client;
        }
	}


}
	