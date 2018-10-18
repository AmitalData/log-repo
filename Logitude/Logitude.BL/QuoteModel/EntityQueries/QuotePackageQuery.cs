using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Data.QuoteModel.Repositories;
using Logitude.BL.QuoteModel.EntityPMs;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.BL.QuoteModel.EntityQueries
{
    public class QuotePackageQuery
    {
        QuotePackageRepository repository;        
        public QuotePackageQuery(int tenant)
        {
            repository = new QuotePackageRepository(tenant);
        }
        public QuotePackageQuery(QuotePackageRepository myRepository)
        {
            this.repository = myRepository;
        }

        public QuotePackagePM GetSinglePM(string id, int tenant)
        {
            QuotePackagePM quotePackages = (from a in repository.context.QuotePackages.Include("PackageType")
                                                  where a.Tenant == tenant && a.Id == id
                                            select new QuotePackagePM()
                                                  {
                                                      Height = a.Height,
                                                      Id = a.Id,
                                                      Length = a.Length,
                                                      PackageTypeId = a.PackageTypeId,
                                                      PackageTypeName = a.PackageType != null ? a.PackageType.EnglishName : null,
                                                      Quantity = a.Quantity,                                                      
                                                      QuoteId = a.QuoteId,
                                                      Tenant = a.Tenant,
                                                      Volume = a.Volume,
                                                      GrossWeight = a.GrossWeight,
                                                      Width = a.Width,
                                                      VolumetricWeight = a.VolumetricWeight,
                                                  }).FirstOrDefault();

            return quotePackages;
        }

        public IQueryable<QuotePackagePM> GetQuotePackagePMsByTenant_00(int tenant)
        {
            IQueryable<QuotePackagePM> quotePackages = from a in repository.context.QuotePackages.Include("PackageType")
                                                             where a.Tenant == tenant
                                                          select new QuotePackagePM()
                                                             {
                                                                 Height = a.Height,
                                                                 Id = a.Id,
                                                                 Length = a.Length,                                                                 
                                                                 PackageTypeId = a.PackageTypeId,
                                                                 PackageTypeName = a.PackageType != null ? a.PackageType.EnglishName : null,
                                                                 Quantity = a.Quantity,
                                                                 QuoteId = a.QuoteId,
                                                                 Tenant = a.Tenant,
                                                                 Volume = a.Volume,
                                                                 GrossWeight = a.GrossWeight,
                                                                 Width = a.Width,
                                                                 VolumetricWeight = a.VolumetricWeight,
                                                             };
            return quotePackages;
        }

        public List<QuotePackagePM> GetQuotePackagesForQuotePMIDTenant(string quoteId, int tenant)
        {
            List<QuotePackagePM> myResult
                = (from a in repository.context.QuotePackages.Include("PackageType")
                   where a.QuoteId == quoteId && a.Tenant == tenant
                   select new QuotePackagePM()
                   {
                       Height = a.Height,
                       Id = a.Id,
                       Length = a.Length,
                       PackageTypeId = a.PackageTypeId,
                       PackageTypeName = a.PackageType != null ? a.PackageType.EnglishName : null,
                       Quantity = a.Quantity,
                       QuoteId = a.QuoteId,
                       Tenant = a.Tenant,
                       Volume = a.Volume,
                       GrossWeight = a.GrossWeight,
                       Width = a.Width,
                       VolumetricWeight = a.VolumetricWeight,
                   }).ToList();

            foreach(QuotePackagePM item in myResult)
            {
                string myDimensions = null;

                if (item.Length == null && item.Width == null && item.Height == null)
                {
                    myDimensions = " - - ";
                }

                else
                {
                    double myLength = 0;
                    double myWidth = 0;
                    double myHeight = 0;

                    if (item.Length != null)
                    {
                        myLength = item.Length.Value;
                    }

                    if (item.Width != null)
                    {
                        myWidth = item.Width.Value;
                    }

                    if (item.Height != null)
                    {
                        myHeight = item.Height.Value;
                    }

                    myDimensions = myLength + "-" + myWidth + "-" + myHeight;
                }

                item.Dimensions = myDimensions;
            }

            return myResult;
        }
    }
}