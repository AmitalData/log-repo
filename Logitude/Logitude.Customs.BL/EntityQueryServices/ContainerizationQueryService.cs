
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



    }
}
