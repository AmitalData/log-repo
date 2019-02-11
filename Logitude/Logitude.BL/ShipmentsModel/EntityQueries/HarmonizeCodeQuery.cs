using Logitude.BL.ShipmentsModel.EntityLists;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.EntityQueries
{
    public class HarmonizeCodeQuery
    {
        HarmonizeCodeRepository repository;
        public HarmonizeCodeQuery(int tenant)
        {
            repository = new HarmonizeCodeRepository(tenant);
        }
        public HarmonizeCodeQuery(HarmonizeCodeRepository repository)
        {
            this.repository = repository;
        }

        public HarmonizeCodeList GetSingleHarmonizeCodeList(HarmonizeCode entity)
        {
            HarmonizeCodeList myResult = null;

            if (entity != null)
            {
                myResult = new HarmonizeCodeList()
                {
                    Code = entity.Code,
                    ChapterCode = entity.ChapterCode,
                    ChapterDescription = entity.ChapterDescription,
                    Description = entity.Description,
                    SubChapterCode = entity.SubChapterCode,
                    SubChapterDescription = entity.SubChapterDescription,
                    SearchFields = entity.SearchFields,
                };
            }

            return myResult;
        }

        public IQueryable<HarmonizeCodeList> GetIQueryableEntityList(IQueryable<HarmonizeCode> entities)
        {
            var myResult = (from entity in entities
                            select new HarmonizeCodeList()
                            {
                                Code = entity.Code,
                                ChapterCode = entity.ChapterCode,
                                ChapterDescription = entity.ChapterDescription,
                                Description = entity.Description,
                                SubChapterCode = entity.SubChapterCode,
                                SubChapterDescription = entity.SubChapterDescription,
                                SearchFields = entity.SearchFields,
                            });

            return myResult;
        }
    }
}
