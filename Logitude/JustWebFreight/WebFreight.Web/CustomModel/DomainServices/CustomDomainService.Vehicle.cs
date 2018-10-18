using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Xml.Serialization;
using Logitude.Customs.Data.EntityLists;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data.EntityListQueryServices;
using Simplog.Server.Infrastructure;
using System.ServiceModel.DomainServices.Server;
using Logitude.Server.Tools.Helpers;

namespace WebFreight.Web.CustomModel.DomainServices
{
    public partial class CustomDomainService
    {

        public VehiclePM GetSingleVehiclePM(string id, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            vehicleQuery = new VehicleQueryService(customContext);
            VehiclePM Vehicle = vehicleQuery.GetSingle(id, true, false);
            return Vehicle;
        }

        public VehiclePM GetVehicleByVehicleChassisNumberOrRichbitFileNumber(string vehicleChassisNumber, string richbitFileNumber, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            vehicleQuery = new VehicleQueryService(customContext);
            VehiclePM Vehicle = vehicleQuery.GetVehicleByVehicleChassisNumberOrRichbitFileNumber(vehicleChassisNumber, richbitFileNumber, tenant);
            return Vehicle;
        }

        public List<VehicleList> GetVehiclesForSelection(int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            vehicleQuery = new VehicleQueryService(customContext);
            List<VehicleList> vehicles = vehicleQuery.GetVehiclesForSelection(tenant);
            return vehicles;
        }

        public VehicleList GetSingleVehicleList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Customs.Vehicle", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            VehicleListQueryService listService = new VehicleListQueryService(customContext);
            return listService.GetSingle(id);
        }


        public bool DoesChassisNumberExist(string number, int tenant)
        {
            vehicleRepository = new VehicleRepository(tenant);
            return (vehicleRepository.GetAll(tenant).Where(d => d.VehicleChassisNumber == number && d.Tenant == tenant)).Any();
        }

        public List<VehicleList> GetVehicleLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Customs.Vehicle", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            VehicleListQueryService listService = new VehicleListQueryService(customContext);
            return listService.GetList(tenant);

        }


        public List<VehicleList> GetVehicleFilters(byte[] xmlFilters, int tenant)
        {


            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Customs.Vehicle", "READ", tenant);

            customContext = CustomContext.GetContext(tenant);
            VehicleListQueryService listService = new VehicleListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }



        public int GetVehicleFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Customs.Vehicle", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            VehicleListQueryService queryService = new VehicleListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations, tenant);

        }

        public void InsertVehicle(VehiclePM entityPm)
        {
            SecurityUtility.CheckContactFeature("Customs.Vehicle", "NEW", entityPm.Tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(entityPm.Tenant);
            }

            VehicleUpdateService service = new VehicleUpdateService(customContext, new Dictionary<string, IContext>(), entityPm.Tenant);
            entityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
            foreach (VehicleOwnerPM VehicleOwner in entityPm.VehicleOwners)
            {
                VehicleOwner.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
            }

            foreach (VehicleSafetyAccessoryPM VehicleSafetyAccessory in entityPm.VehicleSafetyAccessories)
            {
                VehicleSafetyAccessory.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
            }

            service.Update(entityPm, true);



        }



        public void UpdateVehicle(VehiclePM currententityPm)
        {
            SecurityUtility.CheckContactFeature("Customs.Vehicle", "UPDATE", currententityPm.Tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(currententityPm.Tenant);
            }

            VehicleUpdateService service = new VehicleUpdateService(customContext, new Dictionary<string, IContext>(), currententityPm.Tenant);
            currententityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
            SetVehicleSafetyAccessoriesChangeSet(currententityPm);
            SetVehicleOwnersChangeSet(currententityPm);
            service.Update(currententityPm, true);

        }

        private void SetVehicleSafetyAccessoriesChangeSet(VehiclePM currententityPm)
        {
            List<VehicleSafetyAccessoryPM> VehicleSafetyAccessorchangeset = ChangeSet.GetAssociatedChanges(currententityPm, d => d.VehicleSafetyAccessories).Cast<VehicleSafetyAccessoryPM>().ToList();
            foreach (VehicleSafetyAccessoryPM itemPM in VehicleSafetyAccessorchangeset)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert:
                        {
                            VehicleSafetyAccessoryPM currentItemPM = currententityPm.VehicleSafetyAccessories.Where(d => d.VehicleId == itemPM.VehicleId && d.LineNumber == itemPM.LineNumber).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Insert;
                            break;
                        }
                    case ChangeOperation.Update:
                        {
                            VehicleSafetyAccessoryPM currentItemPM = currententityPm.VehicleSafetyAccessories.Where(d => d.VehicleId == itemPM.VehicleId && d.LineNumber == itemPM.LineNumber).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Update;


                            break;
                        }
                    case ChangeOperation.Delete:
                        {
                            VehicleSafetyAccessoryPM currentItemPM = new VehicleSafetyAccessoryPM() { ChangeSetOp = ChangeSetOperation.Delete, VehicleId = itemPM.VehicleId, LineNumber = itemPM.LineNumber };

                            currententityPm.DeletedVehicleSafetyAccessories.Add(currentItemPM);

                            break;
                        }
                    default:
                        {
                            VehicleSafetyAccessoryPM currentItemPM = currententityPm.VehicleSafetyAccessories.Where(d => d.VehicleId == itemPM.VehicleId && d.LineNumber == itemPM.LineNumber).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.None;
                            break;
                        }
                }
            }
        }

        private void SetVehicleOwnersChangeSet(VehiclePM currententityPm)
        {
            List<VehicleOwnerPM> VehicleOwnerchangeset = ChangeSet.GetAssociatedChanges(currententityPm, d => d.VehicleOwners).Cast<VehicleOwnerPM>().ToList();
            foreach (VehicleOwnerPM itemPM in VehicleOwnerchangeset)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert:
                        {
                            VehicleOwnerPM currentItemPM = currententityPm.VehicleOwners.Where(d => d.VehicleId == itemPM.VehicleId && d.LineNumber == itemPM.LineNumber).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Insert;
                            break;
                        }
                    case ChangeOperation.Update:
                        {
                            VehicleOwnerPM currentItemPM = currententityPm.VehicleOwners.Where(d => d.VehicleId == itemPM.VehicleId && d.LineNumber == itemPM.LineNumber).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Update;


                            break;
                        }
                    case ChangeOperation.Delete:
                        {
                            VehicleOwnerPM currentItemPM = new VehicleOwnerPM() { ChangeSetOp = ChangeSetOperation.Delete, VehicleId = itemPM.VehicleId, LineNumber = itemPM.LineNumber };

                            currententityPm.DeletedVehicleOwners.Add(currentItemPM);

                            break;
                        }
                    default:
                        {
                            VehicleOwnerPM currentItemPM = currententityPm.VehicleOwners.Where(d => d.VehicleId == itemPM.VehicleId && d.LineNumber == itemPM.LineNumber).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.None;
                            break;
                        }
                }
            }
        }


        public void UpdateVehicleList(VehicleList list)
        {

        }

        [Invoke]
        public void DeleteVehicleDeclarationId(string vehicleChassisNumber, string richbitFileNumber, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            vehicleQuery = new VehicleQueryService(customContext);
            VehiclePM Vehicle = vehicleQuery.GetVehicleByVehicleChassisNumberOrRichbitFileNumber(vehicleChassisNumber, richbitFileNumber, tenant);
            if (Vehicle != null)
            {
                Vehicle.DeclarationId = null;
                VehicleUpdateService service = new VehicleUpdateService(customContext, new Dictionary<string, IContext>(), Vehicle.Tenant);
                Vehicle.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                service.Update(Vehicle, true);
            }
        }

    }



}
