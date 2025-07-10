
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

        public List<ContainerizationList> GetContainerizationsByIds(string ids , int tenant)
        {


            return new List<ContainerizationList>(repository.GetContainerizationsByIds(ids, tenant).Select
                (a => new ContainerizationList()
                {
                    Id = a.Id,

                    Tenant = a.Tenant,

                    SearchFields = a.SearchFields,

                    AgentDeclaration = a.AgentDeclaration,

                    ContainerizationDate = a.ContainerizationDate,

                    ContainerizationNumber = a.ContainerizationNumber,

                    ContainerizationStatus = a.ContainerizationStatusCode.Code,

                    HataraStatus = a.HataraStatus,
                    OpenContainerization = a.ContainerizationStatusCode.Code != "3",
                    OperationMode = a.OperationMode,

                    ContainerizationStatusName = a.ContainerizationStatusCode != null ? a.ContainerizationStatusCode.Name : null,
                    ExportFile = a.ExportFile,// ed.dec.FirstOrDefault().ExportFile,
                    HataraStatusName = a.ContainerizationHataraStatus != null ? a.ContainerizationHataraStatus.Name : null,
                    ImporterName = a.IsMultiCustomers,
                    TransportModeForExport = a.TransportModeId,// ed.dec.FirstOrDefault().TransportModeId ,
                    HataraStatusIsNull = a.HataraStatus != null ? false : true,
                    CargoTypeCode = a.CargoTypeCode,
                    ManifestNumber = a.ManifestNumber,
                    SecondCargoID = a.SecondCargoID,
                    ThirdCargoID = a.ThirdCargoID,

                }));
            
        }

    }
}
