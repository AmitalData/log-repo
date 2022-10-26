using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Server.Tools;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class ContainerizationUpdateService : EntityUpdateService<Containerization, ContainerizationPM, EntityPM>
    {

        protected override void UpdateComposition(ContainerizationPM entityPM)
        {
            var declarationQueryService = new DeclarationQueryService(entityPM.Tenant);
            var consignmentQueryService = new ConsignmentQueryService(entityPM.Tenant);
            var ContainerizationUpdateService = new ContainerizationUpdateService(this.MainContext, new Dictionary<string, IContext>(), entityPM.Tenant);
            var declarationUpdateService = new DeclarationUpdateService(this.MainContext, new Dictionary<string, IContext>(), entityPM.Tenant);
            List<DeclarationPM> AllPms=new List<DeclarationPM>();
            if (!String.IsNullOrWhiteSpace(entityPM.ConnectedDeclarations) && entityPM.ContainerizationStatus != "3")
            {
                var connectedDeclarations = entityPM.ConnectedDeclarations.Split(',').ToList();
                var pms = declarationQueryService.GetDeclarationsByIds(connectedDeclarations, entityPM.Tenant);
                AllPms.AddRange(pms);
                if (pms.Count == 0)
                {
                    //throw new Exception("Why ??"); 
                }
                else
                {
                    foreach (var declaration in pms)
                    {
                        foreach(var cons in declaration.Consignments)
                        {
                            if(cons.CargoTypeCode==EntityPM.CargoTypeCode && cons.ManifestNumber == EntityPM.ManifestNumber && cons.SecondCargoID == EntityPM.SecondCargoID && cons.ThirdCargoID == EntityPM.ThirdCargoID)
                            {
                                if (cons.ExportContainerizationID != entityPM.Id)
                                {
                                    cons.ExportContainerizationID = entityPM.Id;
                                    cons.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                                    declaration.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;

                                }

                            }
                        }
                    }
                }

            }
            if (!String.IsNullOrWhiteSpace(entityPM.NotConnectedDeclarations) && entityPM.ContainerizationStatus != "3")
            {
                var disConnectedDeclarations = entityPM.NotConnectedDeclarations.Split(',').ToList();
                var pms=declarationQueryService.GetDeclarationsByIds(disConnectedDeclarations, entityPM.Tenant);
                AllPms.AddRange(pms);
                if (pms.Count == 0)
                {
                    //throw new Exception("Why ??"); 
                }
                else
                {
                    foreach (var declaration in pms)
                    {
                        foreach (var cons in declaration.Consignments)
                        {
                            if (cons.CargoTypeCode == EntityPM.CargoTypeCode && cons.ManifestNumber == EntityPM.ManifestNumber && cons.SecondCargoID == EntityPM.SecondCargoID && cons.ThirdCargoID == EntityPM.ThirdCargoID)
                            {
                                if (cons.ExportContainerizationID == entityPM.Id)
                                {
                                    cons.ExportContainerizationID = null;
                                    cons.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                                    declaration.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;

                                }

                            }
                        }
                    }

                   

                }

            }
            if (!String.IsNullOrWhiteSpace(entityPM.NotConnectedDeclarations) || !String.IsNullOrWhiteSpace(entityPM.ConnectedDeclarations)) {
                declarationUpdateService.UpdateMulti(AllPms, new List<DeclarationPM>(), entityPM, false); 
            }
           
            if (String.IsNullOrEmpty(entityPM.ConnectedDeclarations)&& entityPM.ContainerizationStatus != "3")
                {
                    entityPM.CargoTypeCode = null;
                    entityPM.ManifestNumber = null;
                    entityPM.SecondCargoID = null;
                    entityPM.ThirdCargoID = null;
                    entityPM.ContainerizationStatus = "3";
                    entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                    ContainerizationUpdateService.Update(entityPM, true);
                }
            
            base.UpdateComposition(entityPM);
        }

        protected override void OnCreating(ContainerizationPM entityPM, EntityPM entityParentPM)
        {
            entityPM.ContainerizationDate = DateTime.Now;
            ContainerizationQueryService containerizationQueryService = new ContainerizationQueryService(entityPM.Tenant);
            if(containerizationQueryService != null)
            {
                entityPM.ContainerizationNumber = containerizationQueryService.GetContainerizationNumber(entityPM.Tenant).ToString();
            }
            entityPM.OperationMode = "1";

        }
        protected override void OnUpdating(ContainerizationPM entityPM, Containerization entityPOCO)
        {
            entityPOCO.IsMultiExportFiles = false;
            entityPOCO.IsMultiCustomers = null;
            if (String.IsNullOrWhiteSpace(entityPM.ConnectedDeclarations))
            {
                entityPM.IsChange = false;
                entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
            }
            else
            {
                ContainerizationRepository containerizationRepository = new ContainerizationRepository(entityPM.Tenant);
                var containerizationExportFiles = containerizationRepository.GetContainerizationExportFiles(entityPM.Tenant, entityPM.ConnectedDeclarations);
                if (containerizationExportFiles.Count > 1)
                {
                    entityPOCO.IsMultiExportFiles = true;
                    entityPM.ExportFile = "List";
                }
                else
                {
                    entityPM.ExportFile = containerizationExportFiles[0];
                }
                var containerizationImporters = containerizationRepository.GetContainerizationImporters(entityPM.Tenant, entityPM.ConnectedDeclarations);
                if (containerizationImporters.Count > 1)
                {
                    entityPM.IsMultiCustomers = "List";
                }
                else
                {
                    entityPM.IsMultiCustomers = containerizationImporters[0].LocalName;
                }
                 //entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
 
            }
            
            base.OnUpdating(entityPM, entityPOCO);
        }
    }
}
