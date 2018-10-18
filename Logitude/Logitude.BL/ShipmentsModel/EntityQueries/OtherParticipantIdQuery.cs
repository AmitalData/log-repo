using Logitude.BL.ShipmentsModel.EntityLists;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.EntityQueries
{
    public class OtherParticipantIdQuery
    {
        OtherParticipantIdRepository repository;

        public OtherParticipantIdQuery(int tenant)
        {
            repository = new OtherParticipantIdRepository(tenant);
        }

        public OtherParticipantIdQuery(OtherParticipantIdRepository entityRepository)
        {
            repository = entityRepository;
        }

        public OtherParticipantIdPM GetSinglePM(string code)
        {
            return (from a in repository.context.OtherParticipantIds
                    where a.Code == code
                    select new OtherParticipantIdPM()
                    {
                        Code = a.Code,
                        Name = a.Name,
                        SearchFields = a.SearchFields
                    }).FirstOrDefault();
        }

        public IQueryable<OtherParticipantIdList> GetIQueryableEntityList(IQueryable<OtherParticipantId> pocos)
        {
            return (from a in pocos
                    select new OtherParticipantIdList()
                    {
                        Code = a.Code,
                        Name = a.Name,
                        SearchFields = a.SearchFields
                    });
        }
    }
}
