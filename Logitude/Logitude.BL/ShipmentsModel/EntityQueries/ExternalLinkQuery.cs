using Logitude.BL.ShipmentsModel.EntityPMs;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Logitude.BL.ShipmentsModel.EntityQueries
{
    public class ExternalLinkQuery
    {
        ExternalLinkRepository repository;

        public ExternalLinkQuery(int tenant)
        {
            repository = new ExternalLinkRepository(tenant);
        }

        public ExternalLinkQuery(ExternalLinkRepository repository)
        {
            this.repository = repository;
        }

        public ExternalLinkPM GetSinglePM(string Ref, bool fromCache = true)
        {
            if(!fromCache)
            {
                ExternalLink externalLink = repository.GetSingleExternalLink(Ref);
                return new ExternalLinkPM()
                {
                    Id = externalLink.Id,
                    Ref = externalLink.Ref,
                    Link = externalLink.Link
                };                
            }

            string cacheKey = "ExternalLink_" + Ref;

            if (HttpContext.Current != null && CacheManager.CacheWrapper.Get(cacheKey) != null)
                return (ExternalLinkPM)CacheManager.CacheWrapper.Get(cacheKey);

            ExternalLinkPM externalLinkPM = GetSinglePM(Ref, false);
            CacheManager.CacheWrapper.Insert(cacheKey, externalLinkPM, null, System.DateTime.UtcNow.AddHours(8), TimeSpan.Zero);

            return externalLinkPM;
        }
    }
}