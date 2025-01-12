
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityDataMappings;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data;
using Simplog.Server.Infrastructure;
using static Logitude.Customs.Data.Repsitories.ContainerizationRepository;
using Logitude.Customs.Data.EntityLists;

namespace Logitude.Customs.BL.EntityQueryServices
{
    public partial class ContainerizationQueryService : EntityQueryService<Containerization, ContainerizationKeys, ContainerizationPM, object, ContainerizationKeys>
    {
        public int GetContainerizationNumber(int tenant)
        {
            ContainerizationRepository containerizationRepository = new ContainerizationRepository(context);
            return containerizationRepository.GetContainerizationNumber(tenant);
        }
        public List<ContainerizationDetails> GetContainerizationByKeys(int tenent,List<string> declarationList)
        {
            return this.repository.GetContainerizationByKeys(tenent,declarationList);
        }
        public ConKeys GetcontainerizationById(string exportContainerizationID, int tenant)
        {
            return this.repository.GetcontainerizationById(exportContainerizationID, tenant);
        }

        public List<ContainerizationPM> GetContainerizationsByIds(string ids , int tenant)
        {
            List<Containerization> containerizationPM = repository.GetContainerizationsByIds(ids, tenant);
            List<ContainerizationPM> containerizationList = new List<ContainerizationPM>();

            foreach (Containerization item in containerizationPM)
            {
                ContainerizationPM containerization = GetEntityPM(item);
                containerizationList.Add(containerization);
            }

            return containerizationList;

        }

    }
}
