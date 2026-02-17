
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
using Logitude.Customs.Data.DataContracts;

namespace Logitude.Customs.Data.Repsitories
{
   public partial class CustomsVendorRepository:IRepository<CustomsVendor>
   {
        
		public List<CustomsVendor> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }

        public bool DoesVendorExist(string vendorNumber,int tenant)
        {
            return (from a in context.CustomsVendors
                    where a.VendorNumber == vendorNumber && a.Tenant==tenant
                    select a).Any();
        }

        public CustomsVendor GetVendorByNumber(string vendorNumber, int tenant)
        {
            return (from a in context.CustomsVendors
                    where a.VendorNumber == vendorNumber && a.Tenant == tenant
                    select a).FirstOrDefault();
        }
        public List<CustomsVendor> GetVendorByNumberList(List<string> vendorNumberList, int tenant)
        {
            return (from a in context.CustomsVendors
                    where vendorNumberList.Contains(a.VendorNumber) && a.Tenant == tenant
                    select a).ToList();
        }

        public string GetIdByVendorNumber(string vendorNumber, int tenant)
        {
            return
                (
                from rec in context.CustomsVendors
                where rec.VendorNumber == vendorNumber && rec.Tenant == tenant
                select rec.Id
                )
                .FirstOrDefault();
        }

        public IQueryable<ImporterDespositionClass> GetVendorsWithImporterDespositions(string vendorId, string importerId, bool ShowOnlyValid, bool useImporterFilter, string searchText, int tenant)
        {
            IQueryable<ImporterDespositionClass> despositions = null;
            IQueryable<ImporterDesposition> importerDespositions = null;


            if (!useImporterFilter && !ShowOnlyValid)
            {
                importerDespositions = (from a in context.ImporterDespositions
                                        where a.Tenant == tenant
                                        select a);

                if (searchText == "null")
                {
                    //despositions = from a in context.CustomsVendors
                    //               join m in importerDespositions on a.Id equals m.VendorID
                    //               //  where a.SearchFields.Contains(searchText)
                    //               select new ImporterDespositionClass()
                    //               {
                    //                   VendorId = a.Id,
                    //                   VendorName = a.VendorName,
                    //                   VendorNumber = a.VendorNumber,
                    //                   CountryCode = a.CountryCode,
                    //                   EndDate = (m.ImporterlId == importerId) ? m.EndDate : null,
                    //                   ImporterDespositionNumber = (m.ImporterlId == importerId) ? m.DepositionNumber : null,
                    //                   SearchFields = a.VendorName + "," + a.VendorNumber + "," + m.DepositionNumber,
                    //               };

                    despositions = (from left in context.CustomsVendors
                                 join right in importerDespositions on left.Id equals right.VendorID into joinedList
                                 from m in joinedList.DefaultIfEmpty()
                                 select new ImporterDespositionClass()
                                 {
                                     VendorId = left.Id,
                                     VendorName = left.VendorName,
                                     VendorNumber = left.VendorNumber,
                                     CountryCode = left.CountryCode,
                                     EndDate = (m.ImporterlId == importerId) ? m.EndDate : null,
                                     ImporterDespositionNumber = (m.ImporterlId == importerId) ? m.DepositionNumber : null,
                                     SearchFields = left.VendorName + "," + left.VendorNumber + "," + m.DepositionNumber,
                                 });
                }

                else
                {
                    //despositions = from a in context.CustomsVendors
                    //               join m in importerDespositions on a.Id equals m.VendorID
                    //               where a.SearchFields.Contains(searchText.ToLower())
                    //               select new ImporterDespositionClass()
                    //               {
                    //                   //   Id = Guid.NewGuid().ToString(),
                    //                   VendorId = a.Id,
                    //                   VendorName = a.VendorName,
                    //                   VendorNumber = a.VendorNumber,
                    //                   CountryCode = a.CountryCode,
                    //                   EndDate = (m.ImporterlId == importerId) ? m.EndDate : null,
                    //                   ImporterDespositionNumber = (m.ImporterlId == importerId) ? m.DepositionNumber : null,
                    //                   SearchFields = a.VendorName + "," + a.VendorNumber + "," + m.DepositionNumber,

                    //               };

                    despositions = (from left in context.CustomsVendors
                                    join right in importerDespositions on left.Id equals right.VendorID into joinedList
                                    where left.SearchFields.Contains(searchText.ToLower())
                                    from m in joinedList.DefaultIfEmpty()
                                    select new ImporterDespositionClass()
                                    {
                                        VendorId = left.Id,
                                        VendorName = left.VendorName,
                                        VendorNumber = left.VendorNumber,
                                        CountryCode = left.CountryCode,
                                        EndDate = (m.ImporterlId == importerId) ? m.EndDate : null,
                                        ImporterDespositionNumber = (m.ImporterlId == importerId) ? m.DepositionNumber : null,
                                        SearchFields = left.VendorName + "," + left.VendorNumber + "," + m.DepositionNumber,
                                    });

                }
            }

            else
            {
                if (useImporterFilter && ShowOnlyValid)
                {
                    if (string.IsNullOrEmpty(importerId) || importerId == "undefined")
                    {

                        importerDespositions = (from a in context.ImporterDespositions
                                                where a.Tenant == tenant && a.ImporterlId == "1"
                                                select a);
                    }
                    else
                    {
                        importerDespositions = (from a in context.ImporterDespositions
                                                where a.Tenant == tenant && a.ImporterlId == importerId && a.EndDate >= DateTime.Today
                                                select a);
                    }
                }
                else if (useImporterFilter && !ShowOnlyValid)
                {
                    if (string.IsNullOrEmpty(importerId) || importerId == "undefined")
                    {

                        importerDespositions = (from a in context.ImporterDespositions
                                                where a.Tenant == tenant && a.ImporterlId == "1"
                                                select a);
                    }
                    else
                    {
                        importerDespositions = (from a in context.ImporterDespositions
                                                where a.Tenant == tenant && a.ImporterlId == importerId
                                                select a);
                    }
                }
                else if (!useImporterFilter && ShowOnlyValid)
                {

                    importerDespositions = (from a in context.ImporterDespositions
                                            where a.Tenant == tenant && a.EndDate >= DateTime.Today
                                            select a);
                }


                if (searchText == "null")
                {
                    despositions = from a in context.CustomsVendors
                                   join m in importerDespositions on a.Id equals m.VendorID
                                   select new ImporterDespositionClass()
                                   {
                                       VendorId = a.Id,
                                       VendorName = a.VendorName,
                                       VendorNumber = a.VendorNumber,
                                       CountryCode = a.CountryCode,
                                       EndDate = (m.ImporterlId == importerId) ? m.EndDate : null,
                                       ImporterDespositionNumber = (m.ImporterlId == importerId) ? m.DepositionNumber : null,
                                       SearchFields = a.VendorName + "," + a.VendorNumber + "," + m.DepositionNumber,
                                   };

                }

                else
                {
                    despositions = from a in context.CustomsVendors
                                   join m in importerDespositions on a.Id equals m.VendorID
                                   where a.SearchFields.Contains(searchText.ToLower())
                                   select new ImporterDespositionClass()
                                   {
                                       VendorId = a.Id,
                                       VendorName = a.VendorName,
                                       VendorNumber = a.VendorNumber,
                                       CountryCode = a.CountryCode,
                                       EndDate = (m.ImporterlId == importerId) ? m.EndDate : null,
                                       ImporterDespositionNumber = (m.ImporterlId == importerId) ? m.DepositionNumber : null,
                                       SearchFields = a.VendorName + "," + a.VendorNumber + "," + m.DepositionNumber,
                                   };
                }
            }

            return despositions;
        }


     }

}
   