using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.Validators;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Logitude.Server.Tools.Helpers;
using System.Transactions;
using Logitude.Server.Tools.Counters;
using System.Threading;
using System.Reflection;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Simplog.Data.Helpers;
using Logitude.Server.Tools;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.Tools.EntityService;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.ShipmentsModel.Tools.TraceEvents;
using Logitude.BL.Validators;
using Logitude.Server.Tools;
using Logitude.BL.ShipmentsModel.Tools.DataMapping;
using Simplog.Data.QuoteModel.Repositories;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Logitude.BL.DataContracts;
using System.Data.Entity;
using Simplog.Server.Infrastructure;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.ComponentModel;

namespace WebFreight.Web.WcfApi
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ShipmentHypredService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select ShipmentHypredService.svc or ShipmentHypredService.svc.cs at the Solution Explorer and start debugging.
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ShipmentWcfService : IShipmentWcfService
    {
        public Response Upsert(ShipmentPM entityPM, bool batch)
        {
            if (CacheManager.CacheWrapper == null)
            {
                CacheManager.CacheWrapper = new MockCacheWrapper();
            }

            Response response = new Response();
            try
            {
                SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
                SecurityUtility.CheckContactFeature("Shipment", "UPDATE", entityPM.Tenant);//UPDATE//READ
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    //IdCounter.GetNumber("Shipment", 1);
                    //Thread.Sleep(6000);


                    ICommonDataContext commoncontext = CommonDataContext.GetContext(entityPM.Tenant);
                    IWebFreightContext webFreightContext = WebFreightContext.GetContext(entityPM.Tenant);
                    PortRepository portRepository = new PortRepository(commoncontext);
                    CardRepository cardsReporistory = new CardRepository(commoncontext);
                    IncotermRepository incotermRepository = new IncotermRepository(commoncontext);
                    DepartmentRepository departmentRepository = new DepartmentRepository(commoncontext);
                    BranchRepository branchRepository = new BranchRepository(commoncontext);
                    UserRepository userrepository = new UserRepository(commoncontext);
                    EntityStatusRepository entityStatusRepository = new EntityStatusRepository(webFreightContext);
                    CurrencyRepository currencyRepository = new CurrencyRepository(commoncontext);
                    ContactRepository contactRepository = new ContactRepository(commoncontext);
                    VesselRepository vesselRepository = new VesselRepository(commoncontext);
                    AgentRepository agentRepository = new AgentRepository(commoncontext);
                    MoveTypeRepository moveTypeRepository = new MoveTypeRepository(webFreightContext);
                    IShipmentsContext objectContext = ShipmentsContext.GetContext(entityPM.Tenant);
                    ShipmentRepository shipmentRepository = new ShipmentRepository(objectContext);
                    SpecialServicesTypeRepository specialServicesTypeRepository = new SpecialServicesTypeRepository(objectContext);
                    TenantRepository tenantRepository = new TenantRepository(entityPM.Tenant);
                    TenantQuery tenantQuery = new TenantQuery(tenantRepository);
                    TenantPM tenantPM = tenantQuery.GetSinglePM(entityPM.Tenant);
                    CountryRepository countryRepository = new CountryRepository(commoncontext);
                    AddressRepository addressRepository = new AddressRepository(commoncontext);
                    TruckerRepository truckerRepository = new TruckerRepository(commoncontext); 


                    // ???????????
                    //"system@tenant1.com"


                    entityPM.IsHybrid = true;

                    entityPM.NewConcurrencyGUID = Guid.NewGuid().ToString();

                    if (String.IsNullOrEmpty(entityPM.DimensionsUnitCode))
                        entityPM.DimensionsUnitCode = tenantPM.DimensionsUnitCode;

                    if (String.IsNullOrEmpty(entityPM.VolumeUnitCode))
                        entityPM.VolumeUnitCode = tenantPM.VolumeUnitCode;

                    if (String.IsNullOrEmpty(entityPM.GrossWeightUnitCode))
                        entityPM.GrossWeightUnitCode = tenantPM.GrossWeightUnitCode;

                    if (String.IsNullOrEmpty(entityPM.ChargeableWeightUnitCode))
                        entityPM.ChargeableWeightUnitCode = tenantPM.ChargeableWeightUnitCode;

                    if (string.IsNullOrEmpty(entityPM.StatusId))
                    {
                        EntityStatus status = EntityStatusRepository.GetSingleEntityStatusByCode("OPOP", entityPM.Tenant, true);
                        if (status == null)
                        {
                            status = EntityStatusRepository.GetSingleEntityStatusByCode("SHOR", entityPM.Tenant, true);
                        }

                        entityPM.StatusId = status.Id;
                        entityPM.StatusDate = entityPM.CreateDateTime;
                        entityPM.LastStatusLogDate = entityPM.CreateDateTime;
                    }
                    else
                    {
                        EntityStatus status = EntityStatusRepository.GetSingleEntityStatusByCode(entityPM.StatusId, entityPM.Tenant, true);
                        if (status != null)
                        {
                            entityPM.StatusId = status.Id;
                        }
                        else
                        {
                            response.HasError = true;
                            response.ErrorMessage = "StatusId field doesn't exist in the database,Upsert this entity before using it.";
                            return response;
                        }

                    }


                    ClassLevelValidator validationClass = new ClassLevelValidator("Shipment", entityPM.Tenant) { IsHybrid = true };
                    if (!validationClass.IsValid(entityPM, entityPM, null))
                    {
                        response.HasError = true;
                        response.ErrorMessage = validationClass.GetErrorMessage(entityPM, null);
                        return response;
                    }

                    User user = userrepository.GetSingleUserByCode(entityPM.CreatedByUserId, entityPM.Tenant, true);
                    if (user != null)
                    {
                        entityPM.CreatedByUserId = user.Id;
                    }
                    else
                    {
                        response.HasError = true;
                        response.ErrorMessage = "CreatedByUserId field doesn’t  exist!";
                        return response;
                    }
                    if (string.IsNullOrWhiteSpace(entityPM.MasterShipmentDataId))
                    {
                        entityPM.MasterShipmentDataId = null;
                    }

                    #region Resolving Keys

                    WcfServicesHelper.SetPortId(entityPM.FromPortId, entityPM.Tenant, "FromPortId", entityPM, portRepository, response);
                    WcfServicesHelper.SetPortId(entityPM.ToPortId, entityPM.Tenant, "ToPortId", entityPM, portRepository, response);
                    WcfServicesHelper.SetPortId(entityPM.MainCarriageFromPortId, entityPM.Tenant, "MainCarriageFromPortId", entityPM, portRepository, response);
                    WcfServicesHelper.SetPortId(entityPM.MainCarriageToPortId, entityPM.Tenant, "MainCarriageToPortId", entityPM, portRepository, response);


                    if (entityPM.TransportModeId == "O" || entityPM.TransportModeId == "I")
                    {
                        if (string.IsNullOrEmpty(entityPM.ShipmentTypeId))
                        {
                            response.HasError = true;
                            response.ErrorMessage = "ShipmentTypeId field is required for ocean and inlad shipments.";
                            return response;
                        }
                    }

                    if (entityPM.ShipperId != null)
                    {
                        string cardId = cardsReporistory.GetCardIdByCode(entityPM.ShipperId, entityPM.Tenant);
                        if (!string.IsNullOrEmpty(cardId))
                        {
                            entityPM.ShipperId = cardId;
                        }
                        else
                        {
                            response.HasError = true;
                            response.ErrorMessage = "ShipperId field doesn't exist in the database,Upsert this entity before using it.";
                            return response;
                        }
                    }


                    if (entityPM.Notify1Id != null)
                    {
                        string cardId = cardsReporistory.GetCardIdByCode(entityPM.Notify1Id, entityPM.Tenant);
                        if (!string.IsNullOrEmpty(cardId))
                        {
                            entityPM.Notify1Id = cardId;
                        }
                        else
                        {
                            response.HasError = true;
                            response.ErrorMessage = "Notify1Id field doesn't exist in the database,Upsert this entity before using it.";
                            return response;
                        }
                    }

                    if (entityPM.Notify2Id != null)
                    {
                        string cardId = cardsReporistory.GetCardIdByCode(entityPM.Notify2Id, entityPM.Tenant);
                        if (!string.IsNullOrEmpty(cardId))
                        {
                            entityPM.Notify2Id = cardId;
                        }
                        else
                        {
                            response.HasError = true;
                            response.ErrorMessage = "Notify2Id field doesn't exist in the database,Upsert this entity before using it.";
                            return response;
                        }
                    }

                    if (entityPM.FreelancerId != null)
                    {
                        string cardId = cardsReporistory.GetCardIdByCode(entityPM.FreelancerId, entityPM.Tenant);
                        if (!string.IsNullOrEmpty(cardId))
                        {
                            entityPM.FreelancerId = cardId;
                        }
                        else
                        {
                            response.HasError = true;
                            response.ErrorMessage = "FreelancerId field doesn't exist in the database,Upsert this entity before using it.";
                            return response;
                        }
                    }


                    if (entityPM.CustomerId != null)
                    {
                        string cardId = cardsReporistory.GetCardIdByCode(entityPM.CustomerId, entityPM.Tenant);
                        if (!string.IsNullOrEmpty(cardId))
                        {
                            entityPM.CustomerId = cardId;
                        }
                        else
                        {
                            response.HasError = true;
                            response.ErrorMessage = "CustomerId field doesn't exist in the database,Upsert this entity before using it.";
                            return response;
                        }
                    }

                    if (entityPM.ConsigneeId != null)
                    {
                        string cardId = cardsReporistory.GetCardIdByCode(entityPM.ConsigneeId, entityPM.Tenant);
                        if (!string.IsNullOrEmpty(cardId))
                        {
                            entityPM.ConsigneeId = cardId;
                        }
                        else
                        {
                            response.HasError = true;
                            response.ErrorMessage = "ConsigneeId field doesn't exist in the database,Upsert this entity before using it.";
                            return response;
                        }
                    }




                    if (entityPM.DepartmentId != null)
                    {
                        Department department = departmentRepository.GetSingleDepartmentByCode(entityPM.DepartmentId, entityPM.Tenant);
                        if (department != null)
                        {
                            entityPM.DepartmentId = department.Id;
                        }
                        else
                        {
                            response.HasError = true;
                            response.ErrorMessage = "DepartmentId field doesn't exist in the database,Upsert this entity before using it.";
                            return response;
                        }
                    }

                    if (entityPM.BranchId != null)
                    {
                        Branch branch = branchRepository.GetSingleBranchByCode(entityPM.BranchId, entityPM.Tenant);
                        if (branch != null)
                        {
                            entityPM.BranchId = branch.Id;
                        }
                        else
                        {
                            response.HasError = true;
                            response.ErrorMessage = "BranchId field doesn't exist in the database,Upsert this entity before using it.";
                            return response;
                        }
                    }

                    if (entityPM.MainCarriageVesselId != null)
                    {
                        Vessel vessel = vesselRepository.GetSingleVesselByCode(entityPM.MainCarriageVesselId, entityPM.Tenant);
                        if (vessel != null)
                        {
                            entityPM.MainCarriageVesselId = vessel.Id;
                        }
                        else
                        {
                            response.HasError = true;
                            response.ErrorMessage = "MainCarriageVesselId field doesn't exist in the database,Upsert this entity before using it.";
                            return response;
                        }
                    }

                    if (entityPM.AgentId != null)
                    {
                        Agent agent = agentRepository.GetSingleAgentByCode(entityPM.AgentId, entityPM.Tenant);
                        if (agent != null)
                        {
                            entityPM.AgentId = agent.Id;
                        }
                        else
                        {
                            response.HasError = true;
                            response.ErrorMessage = "AgentId field doesn't exist in the database,Upsert this entity before using it.";
                            return response;
                        }
                    }

                    if (entityPM.TruckerId != null)
                    {
                        Trucker trucker = truckerRepository.GetSingleTruckerByCode(entityPM.TruckerId, entityPM.Tenant);
                        if (trucker != null)
                        {
                            entityPM.TruckerId = trucker.Id;
                        }
                        else
                        {
                            response.HasError = true;
                            response.ErrorMessage = "TruckerId field doesn't exist in the database, Upsert this entity before using it.";
                            return response;
                        }
                    }
                     

                    if (entityPM.MainCarriageCarrierId != null)
                    {
                        string cardId = cardsReporistory.GetCardIdByCode(entityPM.MainCarriageCarrierId, entityPM.Tenant);
                        if (!string.IsNullOrEmpty(cardId))
                        {
                            entityPM.MainCarriageCarrierId = cardId;
                        }
                        else
                        {
                            response.HasError = true;
                            response.ErrorMessage = "MainCarriageCarrierId field doesn't exist in the database,Upsert this entity before using it.";
                            return response;
                        }
                    }

                    if (entityPM.MoveTypeId != null)
                    {
                        MoveType moveType = moveTypeRepository.GetSingleMoveTypesByCode(entityPM.MoveTypeId, entityPM.Tenant);
                        if (moveType != null)
                        {
                            entityPM.MoveTypeId = moveType.Id;
                        }
                        else
                        {
                            response.HasError = true;
                            response.ErrorMessage = "MoveTypeId field doesn't exist in the database,Upsert this entity before using it.";
                            return response;
                        }
                    }


                    if (entityPM.SalesmanUserId != null)
                    {
                        User userentity = userrepository.GetSingleUserByCode(entityPM.SalesmanUserId, entityPM.Tenant, true);
                        if (userentity != null)
                        {
                            entityPM.SalesmanUserId = userentity.Id;
                        }
                        else
                        {
                            response.HasError = true;
                            response.ErrorMessage = "SalesmanUserId field doesn't exist in the database,Upsert this entity before using it.";
                            return response;
                        }
                    }

                    if (entityPM.AWBCurrencyId != null)
                    {
                        Currency currency = CurrencyRepository.GetSingleCurrencyByCode(entityPM.AWBCurrencyId, entityPM.Tenant, true);
                        if (currency != null)
                        {
                            entityPM.AWBCurrencyId = currency.Id;
                        }
                        else
                        {
                            response.HasError = true;
                            response.ErrorMessage = "AWBCurrencyId field doesn't exist in the database,Upsert this entity before using it.";
                            return response;
                        }
                    }

                    if (entityPM.ProfitCurrencyId != null)
                    {
                        Currency currency = CurrencyRepository.GetSingleCurrencyByCode(entityPM.ProfitCurrencyId, entityPM.Tenant, true);
                        if (currency != null)
                        {
                            entityPM.ProfitCurrencyId = currency.Id;
                        }
                        else
                        {
                            response.HasError = true;
                            response.ErrorMessage = "ProfitCurrencyId field doesn't exist in the database,Upsert this entity before using it.";
                            return response;
                        }
                    }

                    if (entityPM.IncotermId != null)
                    {
                        Incoterm incoterm = incotermRepository.GetIncotermByCode(entityPM.IncotermId, entityPM.Tenant);
                        if (incoterm != null)
                        {
                            entityPM.IncotermId = incoterm.Id;
                        }
                        else
                        {
                            response.HasError = true;
                            response.ErrorMessage = "IncotermId field doesn't exist in the database,Upsert this entity before using it.";
                            return response;
                        }
                    }

                    if (entityPM.SpecialServicesTypeId != null)
                    {
                        SpecialServicesType specialServicesType = specialServicesTypeRepository.GetSingleSpecialServicesTypeByCode(entityPM.SpecialServicesTypeId, entityPM.Tenant);
                        if (specialServicesType != null)
                        {
                            entityPM.SpecialServicesTypeId = specialServicesType.Id;
                        }
                        else
                        {
                            response.HasError = true;
                            response.ErrorMessage = "SpecialServicesTypeId field doesn't exist in the database,Upsert this entity before using it.";
                            return response;
                        }
                    }

                    if (entityPM.AccountManagerUserId != null)
                    {
                        User userentity = userrepository.GetSingleUserByCode(entityPM.AccountManagerUserId, entityPM.Tenant, true);
                        if (userentity != null)
                        {
                            entityPM.AccountManagerUserId = userentity.Id;
                        }
                        else
                        {
                            response.HasError = true;
                            response.ErrorMessage = "AccountManagerUserId field doesn't exist in the database,Upsert this entity before using it.";
                            return response;
                        }
                    }


                    if (!string.IsNullOrEmpty(entityPM.QuoteNumber))
                    {
                        QuoteRepository quoteRepository = new QuoteRepository(entityPM.Tenant);
                        string quoteentityId = quoteRepository.GetQuoteId(entityPM.QuoteNumber, entityPM.Tenant);
                        if (quoteentityId != null)
                        {
                            entityPM.QuoteId = quoteentityId;
                        }
                        else
                        {
                            response.HasError = true;
                            response.ErrorMessage = "QuoteNumber field doesn't exist in the database,Upsert this entity before using it.";
                            return response;
                        }
                    }


                    WcfServicesHelper.SetPortId(entityPM.PreCarriageFromPortId, entityPM.Tenant, "PreCarriageFromPortId", entityPM, portRepository, response);
                    WcfServicesHelper.SetPortId(entityPM.PreCarriageToPortId, entityPM.Tenant, "PreCarriageToPortId", entityPM, portRepository, response);
                    WcfServicesHelper.SetPortId(entityPM.OnCarriageFromPortId, entityPM.Tenant, "OnCarriageFromPortId", entityPM, portRepository, response);
                    WcfServicesHelper.SetPortId(entityPM.OnCarriageToPortId, entityPM.Tenant, "OnCarriageToPortId", entityPM, portRepository, response);

                    #endregion

                    #region Transshipments


                    WcfServicesHelper.SetPortId(entityPM.Transshipment1FromPortId, entityPM.Tenant, "Transshipment1FromPortId", entityPM, portRepository, response);
                    WcfServicesHelper.SetPortId(entityPM.Transshipment2FromPortId, entityPM.Tenant, "Transshipment2FromPortId", entityPM, portRepository, response);
                    WcfServicesHelper.SetPortId(entityPM.Transshipment3FromPortId, entityPM.Tenant, "Transshipment3FromPortId", entityPM, portRepository, response);

                    WcfServicesHelper.SetPortId(entityPM.Transshipment1ToPortId, entityPM.Tenant, "Transshipment1ToPortId", entityPM, portRepository, response);
                    WcfServicesHelper.SetPortId(entityPM.Transshipment2ToPortId, entityPM.Tenant, "Transshipment2ToPortId", entityPM, portRepository, response);
                    WcfServicesHelper.SetPortId(entityPM.Transshipment3ToPortId, entityPM.Tenant, "Transshipment3ToPortId", entityPM, portRepository, response);


                    if (entityPM.Transshipment1CarrierId != null)
                    {
                        string cardId = cardsReporistory.GetCardIdByCode(entityPM.Transshipment1CarrierId, entityPM.Tenant);
                        if (!string.IsNullOrEmpty(cardId))
                        {
                            entityPM.Transshipment1CarrierId = cardId;
                        }
                        else
                        {
                            response.HasError = true;
                            response.ErrorMessage = "Transshipment1CarrierId field doesn't exist in the database,Upsert this entity before using it.";
                            return response;
                        }
                    }

                    if (entityPM.Transshipment2CarrierId != null)
                    {
                        string cardId = cardsReporistory.GetCardIdByCode(entityPM.Transshipment2CarrierId, entityPM.Tenant);
                        if (!string.IsNullOrEmpty(cardId))
                        {
                            entityPM.Transshipment2CarrierId = cardId;
                        }
                        else
                        {
                            response.HasError = true;
                            response.ErrorMessage = "Transshipment2CarrierId field doesn't exist in the database,Upsert this entity before using it.";
                            return response;
                        }
                    }

                    if (entityPM.Transshipment3CarrierId != null)
                    {
                        string cardId = cardsReporistory.GetCardIdByCode(entityPM.Transshipment3CarrierId, entityPM.Tenant);
                        if (!string.IsNullOrEmpty(cardId))
                        {
                            entityPM.Transshipment3CarrierId = cardId;
                        }
                        else
                        {
                            response.HasError = true;
                            response.ErrorMessage = "Transshipment3CarrierId field doesn't exist in the database,Upsert this entity before using it.";
                            return response;
                        }
                    }

                    if (entityPM.Transshipment1VesselId != null)
                    {
                        Vessel vessel = vesselRepository.GetSingleVesselByCode(entityPM.Transshipment1VesselId, entityPM.Tenant);
                        if (vessel != null)
                        {
                            entityPM.Transshipment1VesselId = vessel.Id;
                        }
                        else
                        {
                            response.HasError = true;
                            response.ErrorMessage = "Transshipment1VesselId field doesn't exist in the database,Upsert this entity before using it.";
                            return response;
                        }
                    }

                    if (entityPM.Transshipment2VesselId != null)
                    {
                        Vessel vessel = vesselRepository.GetSingleVesselByCode(entityPM.Transshipment2VesselId, entityPM.Tenant);
                        if (vessel != null)
                        {
                            entityPM.Transshipment2VesselId = vessel.Id;
                        }
                        else
                        {
                            response.HasError = true;
                            response.ErrorMessage = "Transshipment2VesselId field doesn't exist in the database,Upsert this entity before using it.";
                            return response;
                        }
                    }

                    if (entityPM.Transshipment3VesselId != null)
                    {
                        Vessel vessel = vesselRepository.GetSingleVesselByCode(entityPM.Transshipment3VesselId, entityPM.Tenant);
                        if (vessel != null)
                        {
                            entityPM.Transshipment3VesselId = vessel.Id;
                        }
                        else
                        {
                            response.HasError = true;
                            response.ErrorMessage = "Transshipment3VesselId field doesn't exist in the database,Upsert this entity before using it.";
                            return response;
                        }
                    }


                    #endregion

                    #region ShipmentPackages

                    foreach (ShipmentPackagePM package in entityPM.ShipmentPackages)
                    {
                        if (!string.IsNullOrEmpty(package.PackageTypeCode))
                        {
                            package.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
                            PackageTypeRepository packageTypeRep = new PackageTypeRepository(commoncontext);
                            PackageType type = packageTypeRep.GetSinglePackageTypeByCode(package.PackageTypeCode, entityPM.Tenant, true);

                            if (type != null)
                            {
                                package.PackageTypeId = type.Id;
                            }
                            else
                            {
                                response.HasError = true;
                                response.ErrorMessage = "PackageTypeCode field doesn't exist in the database,Upsert this entity before using it.";
                                return response;
                            }
                        }
                        else
                        {
                            response.HasError = true;
                            response.ErrorMessage = "PackageTypeCode field is required";
                            return response;
                        }

                        foreach (InsideShipmentPackagePM insidePackage in package.InsideShipmentPackages)
                        {
                            if (!string.IsNullOrEmpty(insidePackage.PackageTypeId))
                            {
                                PackageTypeRepository packageTypeRep = new PackageTypeRepository(commoncontext);
                                PackageType type = packageTypeRep.GetSinglePackageTypeByCode(insidePackage.PackageTypeId, entityPM.Tenant, true);

                                if (type != null)
                                {
                                    insidePackage.PackageTypeId = type.Id;
                                }
                                else
                                {
                                    response.HasError = true;
                                    response.ErrorMessage = "Inside Shipment Package PackageTypeId field doesn't exist in the database,Upsert this entity before using it.";
                                    return response;
                                }
                            }
                            else
                            {
                                response.HasError = true;
                                response.ErrorMessage = "PackageTypeId field is required";
                                return response;
                            }
                        }

                    }

                    MapShipmentPickUps(entityPM, cardsReporistory, countryRepository);
                    MapShipmentDeliveries(entityPM, cardsReporistory, countryRepository);

                    #endregion

                    #region WarehouseLeg
                    MapWarehouseLeg(entityPM, cardsReporistory, addressRepository);
                    #endregion

                    #region CustomAgent
                    MapCustomAgent(entityPM, cardsReporistory);
                    #endregion


                    if (response.HasError)
                    {
                        return response;
                    }

                    Contact contact = ContactRepository.GetSingleContact(user.Id, entityPM.Tenant, true);
                    ShipmentService service = null;

                    Shipment entity = shipmentRepository.GetSingleShipmentOnlyByNumber(entityPM.ShipmentNumber, entityPM.Tenant);
                    if (entity == null)
                    {

                        entityPM.StatusDate = entityPM.CreateDateTime;

                        service = new ShipmentService(objectContext, entityPM, contact.Email);
                        service.SetChangeSet(entityPM.ShipmentPackages, new List<ShipmentOrderPackagePM>(), entityPM.ShipmentPickUps, entityPM.ShipmentDeliveries, new List<ShipmentReceivablePM>(), new List<ShipmentPayablePM>(), new List<ShipmentFollowUpPM>(), new List<ShipmentAWBPrintOnlyPM>(), new List<ConsoleShipmentPM>(), new List<ShipmentCarrierStatusPM>(), new List<AWBOCIPM>(), new List<ShipmentCommodityPM>(), new List<ShipmentAssemblyPM>(), new List<ShipmentStoragePricingPM>());

                        service.Create();


                        if (entityPM.ShipmentLevelCode == "A")
                        {
                            entity = service.entityPoco;
                            List<Shipment> notconnectedShipments = shipmentRepository.GetNotConnectedCustomShipments(entityPM.Tenant, entityPM.ShipmentNumber);

                            foreach (Shipment shipment in notconnectedShipments)
                            {
                                shipment.CustomFileId = entityPM.Id;
                                shipmentRepository.Update(shipment);
                                entity.CustomConnectToShipment = true;
                            }

                            bool hasConnectedShipments = shipmentRepository.HasConnectedCustomShipments(entityPM.Tenant, entityPM.Id);
                            entity.NoFreightFile = !hasConnectedShipments;

                            shipmentRepository.Update(entity);
                            shipmentRepository.SubmitChanges();
                        }

                        // get connected shipment number if zero mark is as nofrieghtfile true
                    }
                    else
                    {

                        if (entity.IsCancelled && entityPM.IsCancelled)
                        {
                            //response.HasError = true;
                            //response.ErrorMessage = "This shipment is cancelled,reactivate this entity before using it.";
                            response.Result = entity.Id;
                            return response;
                        }

                        entityPM.Id = entity.Id;
                        entityPM.ConcurrencyGUID = entity.ConcurrencyGUID;

                        entityPM.StatusId = entity.StatusId;
                        entityPM.StatusDate = entity.StatusDate;
                        entityPM.LastStatusLogDate = entity.LastStatusLogDate;

                        if (entity.ShipmentLevelCode == "H" && entityPM.ShipmentLevelCode == "D")
                        {
                            entityPM.ConvertFromHouseToDirect = true;
                        }

                        if (entity.ShipmentLevelCode == "D" && entityPM.ShipmentLevelCode == "H")
                        {
                            entityPM.ConvertFromDirectToHouse = true;
                        }

                        service = new ShipmentService(objectContext, entityPM, contact.Email);//Prob... Refactore on ShipmentService.

                        ShipmentPackageQuery shipPackageQuery = new ShipmentPackageQuery(new ShipmentPackageRepository(objectContext));
                        List<ShipmentPackagePM> shipmentPackages = shipPackageQuery.GetShipmentPackages(entity.Id, entity.ShipmentNumber, entity.Tenant);
                        foreach (ShipmentPackagePM package in shipmentPackages)
                        {
                            package.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Delete;
                            entityPM.ShipmentPackages.Add(package);
                        }


                        var shipmentPickUpQuery = new ShipmentPickUpQuery(new ShipmentPickUpDeliveryRepository(objectContext));
                        List<ShipmentPickUpPM> shipmentPickUps = shipmentPickUpQuery.GetShipmentPickUpPMsByTenantAndShipment(entity.Id, entity.Tenant);
                        foreach (var pickUpPM in shipmentPickUps)
                        {
                            pickUpPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Delete;
                            entityPM.ShipmentPickUps.Add(pickUpPM);
                        }

                        var shipmentDeliveryQuery = new ShipmentDeliveryQuery(new ShipmentPickUpDeliveryRepository(objectContext));
                        List<ShipmentDeliveryPM> shipmentDeliveries = shipmentDeliveryQuery.GetShipmentDeliveryPMsByTenantAndShipment(entity.Id, entity.Tenant);
                        foreach (var deliveryPM in shipmentDeliveries)
                        {
                            deliveryPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Delete;
                            entityPM.ShipmentDeliveries.Add(deliveryPM);
                        }

                        service.SetChangeSet(entityPM.ShipmentPackages, new List<ShipmentOrderPackagePM>(), entityPM.ShipmentPickUps, entityPM.ShipmentDeliveries, new List<ShipmentReceivablePM>(), new List<ShipmentPayablePM>(), new List<ShipmentFollowUpPM>(), new List<ShipmentAWBPrintOnlyPM>(), new List<ConsoleShipmentPM>(), new List<ShipmentCarrierStatusPM>(), new List<AWBOCIPM>(), new List<ShipmentCommodityPM>(), new List<ShipmentAssemblyPM>(), new List<ShipmentStoragePricingPM>());


                        

                        if (entityPM.ShipmentLevelCode == "A")
                        {
                            List<Shipment> notconnectedShipments = shipmentRepository.GetNotConnectedCustomShipments(entityPM.Tenant, entityPM.ShipmentNumber);

                            SharedFollowedShipmentRepository sharedFollowedShipmentRepository = new SharedFollowedShipmentRepository(entityPM.Tenant);
                            IQueryable<SharedFollowedShipment> SharedFollowedShipmentLists = sharedFollowedShipmentRepository.GetSharedFollowedShipmentByShipmentId(entityPM.Id, entityPM.Tenant);

                            foreach (Shipment shipment in notconnectedShipments)
                            {
                                shipment.CustomFileId = entityPM.Id;
                                shipmentRepository.Update(shipment);

                                if (entity.HasException)
                                {
                                    shipment.HasException = entity.HasException;
                                    shipment.ExceptionDate = entity.ExceptionDate;
                                    shipment.ExceptionDescription = entity.ExceptionDescription;
                                    shipment.LastExceptionDescription = entity.LastExceptionDescription;

                                    shipmentRepository.Update(shipment);
                                }

                                foreach (SharedFollowedShipment item in SharedFollowedShipmentLists)
                                {
                                    SharedFollowedShipment sharedFollowedShipment = new SharedFollowedShipment() { Id = Guid.NewGuid().ToString(), ContactId = item.ContactId, Tenant = item.Tenant, TrackDate = item.TrackDate, ShipmentId = shipment.Id };
                                    sharedFollowedShipmentRepository.Add(sharedFollowedShipment);

                                }
                                entityPM.CustomConnectToShipment = true;
                            }



                            shipmentRepository.SubmitChanges();
                            sharedFollowedShipmentRepository.SubmitChanges();
                            bool hasConnectedShipments = shipmentRepository.HasConnectedCustomShipments(entityPM.Tenant, entityPM.Id);
                            entityPM.NoFreightFile = !hasConnectedShipments;
                        }

                        if (entityPM.DirectionId != entity.DirectionId)
                        {
                            var vResponse = ValidateDirectionConversion(entityPM);
                            if (vResponse.HasError)
                            {
                                return vResponse;
                            }
                            else
                            {
                                DirectionRepository directionRepository = new DirectionRepository(0);
                                var directionsList = directionRepository.GetDirections();
                                var oldDirectionName = directionsList.FirstOrDefault(d => d.Id == entity.DirectionId);
                                var currentDirectionName = directionsList.FirstOrDefault(d => d.Id == entityPM.DirectionId);

                                entityPM.ShipmentDirectionConverted = true;
                                entityPM.EventNote = "Converted from [" + oldDirectionName + "] to [" + currentDirectionName + "]";
                            }

                        }


                        service.Update();

                        entityPM.SecurityKey = entity.SecurityKey;

                    }

                    if (entityPM != null)
                    {
                        RunStoredProcedureClass.UpdateShipmentStatus(entityPM.Id, entityPM.Tenant);
                    }

                    response.Result = entityPM.Id;
                    response.Result2 = entityPM.SecurityKey;

                    scope.Complete();
                    return response;
                }
            }
            catch (ApplicationException ex)
            {
                response.HasError = true;
                response.ErrorMessage = ex.Message;
                response.InnerErrorMessage = (ex.InnerException != null ? (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : ex.InnerException.Message) : null);
                return response;
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException e)
            {
                string Error = "";
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);

                        Error += "- Property:" + ve.PropertyName + ", Error:" + ve.ErrorMessage + Environment.NewLine;
                    }
                }
                response.HasError = true;
                response.ErrorMessage = Error;

                return response;

            }

            catch (Exception ex)
            {
                response.IsAuthenticationError = ex.GetType() == typeof(AutenticationException);
                response.HasError = true;
                response.ErrorMessage = ex.Message;
                response.InnerErrorMessage = (ex.InnerException != null ? (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : ex.InnerException.Message) : null);

                if (!string.IsNullOrEmpty(ex.StackTrace))
                {
                    response.ErrorMessage += Environment.NewLine + ex.StackTrace;
                }
                return response;
            }
             

        }

        private void MapCustomAgent(ShipmentPM entityPM, CardRepository cardsReporistory)
        {

            if (entityPM.AssginedtoCustomsAgentId == "--")
            {
                entityPM.AssginedtoCustomsAgentId = null;
            }

            if (entityPM.AssginedtoCustomsAgentId != null)
            {
                Card customAgent = cardsReporistory.GetSingleCardByCode(entityPM.AssginedtoCustomsAgentId, entityPM.Tenant, false);

                if (customAgent != null && !string.IsNullOrEmpty(customAgent.Id))
                {
                    entityPM.AssginedtoCustomsAgentId = customAgent.Id;
                }
                else
                {
                    throw new ApplicationException("AssginedtoCustomsAgentId field doesn't exist in the database, Upsert this entity before using it.");
                }
            }
         
        }

        private  void MapShipmentPickUps(ShipmentPM entityPM, CardRepository cardsReporistory, CountryRepository countryRepository)
        {
            foreach (var pickUp in entityPM.ShipmentPickUps)
            {
                pickUp.ChangeSetOp = ChangeSetOperation.Insert;
                pickUp.PickUpDeliveryTypeCode = "PICK";
                if (pickUp.CarrierId != null)
                {
                    string cardId = cardsReporistory.GetCardIdByCode(pickUp.CarrierId, entityPM.Tenant);
                    if (!string.IsNullOrEmpty(cardId))
                    {
                        pickUp.CarrierId = cardId;
                    }
                    else
                    {
                        throw new ApplicationException($"Pickup CarrierId field ({pickUp.CarrierId}) doesn't exist in the database,Upsert this entity before using it.");
                    }
                }

                if (pickUp.FromAddressCountryId != null)
                {
                    var entityId = countryRepository.GetCountryIdByCode(pickUp.FromAddressCountryId, entityPM.Tenant);
                    if (!string.IsNullOrEmpty(entityId))
                    {
                        pickUp.FromAddressCountryId = entityId;
                    }
                    else
                    {
                        throw new ApplicationException($"Pickup FromAddressCountryId field ({pickUp.FromAddressCountryId}) doesn't exist in the database,Upsert this entity before using it.");
                    }
                }

                if (pickUp.ToAddressCountryId != null)
                {
                    var entityId = countryRepository.GetCountryIdByCode(pickUp.ToAddressCountryId, entityPM.Tenant);
                    if (!string.IsNullOrEmpty(entityId))
                    {
                        pickUp.ToAddressCountryId = entityId;
                    }
                    else
                    {
                        throw new ApplicationException($"Pickup ToAddressCountryId field ({pickUp.ToAddressCountryId}) doesn't exist in the database,Upsert this entity before using it.");
                    }
                }

                if (string.IsNullOrEmpty(pickUp.PickUpDeliveryFromTypeCode))
                {
                    pickUp.PickUpDeliveryFromTypeCode = "CASL";
                }
                if (string.IsNullOrEmpty(pickUp.PickUpDeliveryToTypeCode))
                {
                    pickUp.PickUpDeliveryToTypeCode = "CASL";
                }

                if (string.IsNullOrEmpty(pickUp.TransportModeCode))
                {
                    pickUp.TransportModeCode = "BYTR";
                }

                if(pickUp.PickUpDeliveryFromTypeCode == "CASL")
                {
                    var errorsStrBuilder = new StringBuilder();
                    if (pickUp.FromAddressCountryId == null)
                        errorsStrBuilder.Append("FromAddressCountryId field is required");
                    if (pickUp.FromAddressCity == null)
                        errorsStrBuilder.Append("FromAddressCity field is required");

                    if (pickUp.ToAddressCountryId == null)
                        errorsStrBuilder.Append("ToAddressCountryId field is required");
                    if (pickUp.ToAddressCity == null)
                        errorsStrBuilder.Append("ToAddressCity field is required");
                }

            }
        }

        private void MapShipmentDeliveries(ShipmentPM entityPM, CardRepository cardsReporistory, CountryRepository countryRepository)
        {
            foreach (var deliveryPM in entityPM.ShipmentDeliveries)
            {
                deliveryPM.ChangeSetOp = ChangeSetOperation.Insert;
                deliveryPM.PickUpDeliveryTypeCode = "DELV";
                if (deliveryPM.CarrierId != null)
                {
                    string cardId = cardsReporistory.GetCardIdByCode(deliveryPM.CarrierId, entityPM.Tenant);
                    if (!string.IsNullOrEmpty(cardId))
                    {
                        deliveryPM.CarrierId = cardId;
                    }
                    else
                    {
                        throw new ApplicationException($"Pickup CarrierId field ({deliveryPM.CarrierId}) doesn't exist in the database,Upsert this entity before using it.");
                    }
                }

                if (deliveryPM.FromAddressCountryId != null)
                {
                    var entityId = countryRepository.GetCountryIdByCode(deliveryPM.FromAddressCountryId, entityPM.Tenant);
                    if (!string.IsNullOrEmpty(entityId))
                    {
                        deliveryPM.FromAddressCountryId = entityId;
                    }
                    else
                    {
                        throw new ApplicationException($"Pickup FromAddressCountryId field ({deliveryPM.FromAddressCountryId}) doesn't exist in the database,Upsert this entity before using it.");
                    }
                }

                if (deliveryPM.ToAddressCountryId != null)
                {
                    var entityId = countryRepository.GetCountryIdByCode(deliveryPM.ToAddressCountryId, entityPM.Tenant);
                    if (!string.IsNullOrEmpty(entityId))
                    {
                        deliveryPM.ToAddressCountryId = entityId;
                    }
                    else
                    {
                        throw new ApplicationException($"Pickup ToAddressCountryId field ({deliveryPM.ToAddressCountryId}) doesn't exist in the database,Upsert this entity before using it.");
                    }
                }

                if (string.IsNullOrEmpty(deliveryPM.PickUpDeliveryFromTypeCode))
                {
                    deliveryPM.PickUpDeliveryFromTypeCode = "CASL";
                }
                if (string.IsNullOrEmpty(deliveryPM.PickUpDeliveryToTypeCode))
                {
                    deliveryPM.PickUpDeliveryToTypeCode = "CASL";
                }

                if (string.IsNullOrEmpty(deliveryPM.TransportModeCode))
                {
                    deliveryPM.TransportModeCode = "BYTR";
                }

                if (deliveryPM.PickUpDeliveryFromTypeCode == "CASL")
                {
                    var errorsStrBuilder = new StringBuilder();
                    if (deliveryPM.FromAddressCountryId == null)
                        errorsStrBuilder.Append("FromAddressCountryId field is required");
                    if (deliveryPM.FromAddressCity == null)
                        errorsStrBuilder.Append("FromAddressCity field is required");

                    if (deliveryPM.ToAddressCountryId == null)
                        errorsStrBuilder.Append("ToAddressCountryId field is required");
                    if (deliveryPM.ToAddressCity == null)
                        errorsStrBuilder.Append("ToAddressCity field is required");
                }

            }
        }

        private void MapWarehouseLeg(ShipmentPM entityPM, CardRepository cardsReporistory, AddressRepository addressRepository)
        {
            if (entityPM.WarehouseLegWarehouseId == "--")
                entityPM.WarehouseLegWarehouseId = null;
            
            if (entityPM.WarehouseLegWarehouseId != null)
            {
                Card warehouseCard = cardsReporistory.GetSingleCardByCode(entityPM.WarehouseLegWarehouseId, entityPM.Tenant, false);
                if (warehouseCard != null && !string.IsNullOrEmpty(warehouseCard.Id))
                {
                    entityPM.WarehouseLegWarehouseId = warehouseCard.Id;
                    entityPM.WarehouseLegTerminalName = warehouseCard.EnglishName;
                    //MapWarehouseLegAddressId(entityPM, addressRepository, warehouseCard.Id);
                }
                else
                {
                    throw new ApplicationException("WarehouseLegWarehouseId field doesn't exist in the database,Upsert this entity before using it.");
                }
            }
            else if(entityPM.WarehouseLegActualEntryDate != null)
            {
                throw new ApplicationException("WarehouseLegWarehouseId field doesn't exist in the database,Upsert WarehouseLegWarehouseId before using WarehouseLegActualEntryDate");
            }
        }

        private static void MapWarehouseLegAddressId(ShipmentPM entityPM, AddressRepository addressRepository, string warehouseCardId)
        {
            Address warehouseAddress = addressRepository.GetMainAddressByCardId(warehouseCardId, entityPM.Tenant);
            if (warehouseAddress != null)
            {
                entityPM.WarehouseLegAddressId = warehouseAddress.Id;
            }
        }

        private static Response ValidateDirectionConversion(ShipmentPM entityPM)
        {
            Response response = new Response(); 
            StringBuilder errors = new StringBuilder();
            var isInlandDomestic = entityPM.TransportModeId == "I" && entityPM.DirectionId == "D";
            if (isInlandDomestic)
            {
                if (string.IsNullOrEmpty(entityPM.ConsigneeId))
                {
                    errors.AppendLine("ConsigneeId field is required.");
                }

                if (string.IsNullOrEmpty(entityPM.ShipperId))
                {
                    errors.AppendLine("ShipperId field is required.");
                }

                if (entityPM.ShipmentLevelCode == "C")
                {

                    errors.AppendLine("Master inland domestic are not allowed");
                }

                else if (entityPM.ShipmentLevelCode == "H")
                {
                    errors.AppendLine("House inland domestic shipments are not allowed");
                }

                if (entityPM.ShipmentLevelCode != "C")
                {
                    if (!string.IsNullOrEmpty(entityPM.ShipperId) && !string.IsNullOrEmpty(entityPM.ConsigneeId))
                    {
                        if (entityPM.FromCountryId != entityPM.ToCountryId)
                        {
                            if (entityPM.FromCountryIsEC == false || entityPM.ToCountryIsEC == false)
                            {
                                errors.AppendLine("Both Addresses must be in the same country since the direction is Domestic");
                            }
                        }
                    }
                }
            }

            else
            {
                if (string.IsNullOrEmpty(entityPM.MainCarriageFromPortId))
                {
                    errors.AppendLine("MainCarriageFromPortId field is required.");
                }
                if (entityPM.DirectionId == "D")
                {
                    if (!string.IsNullOrEmpty(entityPM.MainCarriageFromPortId) && !string.IsNullOrEmpty(entityPM.MainCarriageToPortId))
                    {
                        if (entityPM.FromCountryId != entityPM.ToCountryId)
                        {
                            if (entityPM.FromCountryIsEC == false || entityPM.ToCountryIsEC == false)
                            {
                                errors.AppendLine("Both Ports must be in the same country since the direction is Domestic");
                            }
                        }
                    }
                }
            }

            //if (entityPM.ShipmentLevelCode != "C")
            //{
            //    if (string.IsNullOrEmpty(entityPM.CustomerId))
            //    {
            //        errors.AppendLine("CustomerId field is required");
            //    }
            //}

            response.HasError = errors.Length > 0;
            response.ErrorMessage = errors.ToString();

            return response;
        }


        public Response Cancel(string shipmentNumber, int tenant)
        {
            if (CacheManager.CacheWrapper == null)
            {
                CacheManager.CacheWrapper = new MockCacheWrapper();
            }

            Response response = new Response();
            try
            {
                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Shipment", "UPDATE", tenant);//UPDATE//READ
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {

                    IShipmentsContext objectContext = ShipmentsContext.GetContext(tenant);
                    ShipmentRepository shipmentRepository = new ShipmentRepository(objectContext);
                    ICommonDataContext commoncontext = CommonDataContext.GetContext(tenant);


                    UserRepository userrepository = new UserRepository(commoncontext);
                    ContactRepository contactRepository = new ContactRepository(commoncontext);

                    ShipmentQuery shipmentQuery = new ShipmentQuery(shipmentRepository);

                    ShipmentPM entityPM = shipmentQuery.GetSingleShipmentPMByNumber(shipmentNumber, tenant);
                    if (entityPM != null)
                    {
                        entityPM.IsHybrid = true;
                        User user = userrepository.GetSingleUser(entityPM.CreatedByUserId, entityPM.Tenant, true);

                        Contact contact = contactRepository.GetSingleContact(user.Id, entityPM.Tenant);

                        ShipmentService service = new ShipmentService(objectContext, entityPM, contact.Email);
                        entityPM.IsCancelled = true;

                        service.Update();

                        response.Result = shipmentNumber;
                    }
                    else
                    {
                        response.HasError = true;
                        response.ErrorMessage = "Shipment not found!";
                    }
                    scope.Complete();
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.IsAuthenticationError = ex.GetType() == typeof(AutenticationException);
                response.HasError = true;
                response.ErrorMessage = ex.Message;
                response.InnerErrorMessage = ex.InnerException != null ? ex.InnerException.Message : null;

                if (!string.IsNullOrEmpty(ex.StackTrace))
                {
                    response.ErrorMessage += Environment.NewLine + ex.StackTrace;
                }
                return response;
            }


        }

        public Response Delete(string shipmentNumber, int tenant)
        {
            if (CacheManager.CacheWrapper == null)
            {
                CacheManager.CacheWrapper = new MockCacheWrapper();
            }

            Response response = new Response();
            try
            {
                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Shipment", "UPDATE", tenant);//UPDATE//READ
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {

                    IShipmentsContext objectContext = ShipmentsContext.GetContext(tenant);
                    ShipmentRepository shipmentRepository = new ShipmentRepository(objectContext);
                    ICommonDataContext commoncontext = CommonDataContext.GetContext(tenant);


                    UserRepository userrepository = new UserRepository(commoncontext);
                    ContactRepository contactRepository = new ContactRepository(commoncontext);

                    ShipmentQuery shipmentQuery = new ShipmentQuery(shipmentRepository);
                    ShipmentPM entityPM = shipmentQuery.GetSingleShipmentPMByNumber(shipmentNumber, tenant);

                    if (entityPM != null)
                    {
                        entityPM.IsHybrid = true;
                        User user = userrepository.GetSingleUser(entityPM.CreatedByUserId, entityPM.Tenant, true);

                        Contact contact = contactRepository.GetSingleContact(user.Id, entityPM.Tenant);

                        ShipmentService service = new ShipmentService(objectContext, entityPM, contact.Email);


                        service.Delete();

                        response.Result = shipmentNumber;
                    }


                    scope.Complete();
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.IsAuthenticationError = ex.GetType() == typeof(AutenticationException);
                response.HasError = true;
                response.ErrorMessage = ex.Message;
                response.InnerErrorMessage = ex.InnerException != null ? ex.InnerException.Message : null;

                if (!string.IsNullOrEmpty(ex.StackTrace))
                {
                    response.ErrorMessage += Environment.NewLine + ex.StackTrace;
                }
                return response;
            }


        }

        public Response CreateEvent(int tenant, string externalId, string shipmentNumber, string userId, string eventTypeCode, DateTime logDate, DateTime eventDate, string notes)
        {
            if (CacheManager.CacheWrapper == null)
            {
                CacheManager.CacheWrapper = new MockCacheWrapper();
            }

            Response response = new Response();
            try
            {
                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Shipment", "UPDATE", tenant);//UPDATE//READ
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {

                    ICommonDataContext commoncontext = CommonDataContext.GetContext(tenant);
                    IShipmentsContext shipmentsContext = ShipmentsContext.GetContext(tenant);
                    IWebFreightContext webfreightContext = WebFreightContext.GetContext(tenant);

                    EntityStatusRepository entityStatusRepository = new EntityStatusRepository(webfreightContext);
                    UserRepository userrepository = new UserRepository(commoncontext);
                    ShipmentRepository shipmentRepository = new ShipmentRepository(shipmentsContext);
                    ShipmentMasterDataRepository shipmentMasterDataRepository = new ShipmentMasterDataRepository(shipmentsContext);
                    User user = userrepository.GetSingleUserByCode(userId, tenant, true);
                    TraceEventRepository traceEventRepository = new TraceEventRepository(tenant);



                    Shipment entityPoco = shipmentRepository.GetSingleShipmentByShipmentNumber(shipmentNumber, tenant);
                    ShipmentQuery shipmentQuery = new ShipmentQuery(shipmentRepository);
                    if (entityPoco != null)
                    {
                        ShipmentMasterData masterData = null;
                        ShipmentPM entityPM = shipmentQuery.GetSinglePM(entityPoco.Id, tenant);

                        if (entityPoco.ShipmentLevelCode != "H")
                        {
                            masterData = shipmentMasterDataRepository.GetSingleMasterData(entityPoco.Id);

                        }

                        bool exists = false;
                        if (!string.IsNullOrEmpty(externalId))
                        {
                            exists = traceEventRepository.GetSingleTraceEventByExternalId(externalId, tenant) != null;
                        }

                        if (!exists)
                        {
                            EventTracerArgs parameters = new EventTracerArgs()
                            {
                                ExternalId = externalId,
                                Tenant = tenant,
                                CurrentStatusId = entityPoco.StatusId,
                                EventTypeCode = eventTypeCode,
                                EventDateTime = eventDate,
                                LogDateTime = logDate,
                                UserId = user.Id,
                                EntityId = entityPoco.Id,
                                Notes = notes,
                                ObjectTableName = "Shipment",
                            };

                            EventTracer.CreateTraceEvent(parameters);
                            string newStatus = parameters.NewStatusId;
                            //string newStatus = EventTracer.CreateTraceEvent(parameters);

                            if (newStatus != entityPoco.StatusId)
                            {
                                entityPoco.StatusId = newStatus;
                                entityPoco.StatusDate = eventDate;//TenantServerConfigration.GetCurrentDateTime(tenant);
                                entityPoco.LastStatusLogDate = TenantServerConfigration.GetCurrentDateTime(tenant);

                                entityPM.StatusId = newStatus;
                                entityPM.StatusDate = eventDate;//TenantServerConfigration.GetCurrentDateTime(tenant);
                                entityPM.LastStatusLogDate = TenantServerConfigration.GetCurrentDateTime(tenant);


                                ShipmentMapping.BuildSearchField(entityPM, entityPoco, masterData, entityPM.ShipmentPackages);

                                if (masterData != null && masterData.StatusId != newStatus)
                                {
                                    masterData.StatusId = entityPoco.StatusId;
                                    masterData.StatusDate = entityPoco.StatusDate;
                                    shipmentMasterDataRepository.Update(masterData);

                                }
                                shipmentRepository.Update(entityPoco);
                                shipmentsContext.SaveChanges();

                                RunStoredProcedureClass.CreateShipmentQueue(entityPM.Id, entityPM.Tenant);
                            }

                            if (eventTypeCode.ToUpper() == "EXCE")
                            {
                                TraceEvent newTraceEvent = traceEventRepository.GetSingleTraceEventByExternalIdandEventTypeCode(externalId,eventTypeCode, tenant);
                                ContactsUnseenEntitiesHelper.AddUnseenEntityRecord(newTraceEvent.Id, tenant);
                                entityPoco.ExceptionDate = newTraceEvent.EventDateTime;

                                if (!string.IsNullOrEmpty(newTraceEvent.Notes) && newTraceEvent.Notes.Length > 500) entityPoco.ExceptionDescription = newTraceEvent.Notes.Substring(0, 499);
                                else entityPoco.ExceptionDescription = newTraceEvent.Notes;
                          
                                entityPoco.LastExceptionDescription = newTraceEvent.Notes;
                                entityPoco.HasException = true;

                                if (entityPoco.ShipmentLevelCode == "A")
                                {
                                    List<Shipment> connectedShipments = shipmentRepository.GetConnectedCustomShipments(entityPM.Tenant, entityPM.Id).ToList();
                                    foreach (Shipment shipment in connectedShipments)
                                    {
                                        shipment.HasException = entityPoco.HasException;
                                        shipment.ExceptionDate = entityPoco.ExceptionDate;
                                        shipment.ExceptionDescription = entityPoco.ExceptionDescription;
                                        shipment.LastExceptionDescription = entityPoco.LastExceptionDescription;
                                        shipmentRepository.Update(shipment);
                                    }

                                }

                                shipmentRepository.Update(entityPoco);
                                shipmentsContext.SaveChanges();
                            }



                        }

                        RunStoredProcedureClass.UpdateShipmentStatus(entityPM.Id, entityPM.Tenant);

                        response.Result = entityPoco.StatusId;
                    }
                    else
                    {
                        response.HasError = true;
                        response.ErrorMessage = "Shipment doesn't exist!";

                    }

                    scope.Complete();
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.IsAuthenticationError = ex.GetType() == typeof(AutenticationException);
                response.HasError = true;
                response.ErrorMessage = ex.Message;
                response.InnerErrorMessage = (ex.InnerException != null ? (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : ex.InnerException.Message) : null);

                if (!string.IsNullOrEmpty(ex.StackTrace))
                {
                    response.ErrorMessage += Environment.NewLine + ex.StackTrace;
                }

                return response;
            }


        }

        public Response BuildEventsList(int tenant, string shipmentNumber, List<TraceEventPM> eventsList)
        {
            if (CacheManager.CacheWrapper == null)
            {
                CacheManager.CacheWrapper = new MockCacheWrapper();
            }

            Response response = new Response();
            try
            {
                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Shipment", "UPDATE", tenant);//UPDATE//READ
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {

                    eventsList = eventsList.OrderBy(e => e.EventDateTime).ToList();

                    ICommonDataContext commoncontext = CommonDataContext.GetContext(tenant);
                    IShipmentsContext shipmentsContext = ShipmentsContext.GetContext(tenant);
                    IWebFreightContext webfreightContext = WebFreightContext.GetContext(tenant);

                    EntityStatusRepository entityStatusRepository = new EntityStatusRepository(webfreightContext);
                    UserRepository userrepository = new UserRepository(commoncontext);
                    ShipmentRepository shipmentRepository = new ShipmentRepository(shipmentsContext);
                    ShipmentMasterDataRepository shipmentMasterDataRepository = new ShipmentMasterDataRepository(shipmentsContext);
                    ShipmentQuery shipmentQuery = new ShipmentQuery(shipmentRepository);
                    TraceEventRepository traceEventRepository = new TraceEventRepository(tenant);

                    Shipment entityPoco = shipmentRepository.GetSingleShipmentByShipmentNumber(shipmentNumber, tenant);

                    if (entityPoco != null)
                    {
                        ShipmentMasterData masterData = null;
                        ShipmentPM entityPM = shipmentQuery.GetSinglePM(entityPoco.Id, tenant);
                        if (entityPoco.ShipmentLevelCode != "H")
                        {
                            masterData = shipmentMasterDataRepository.GetSingleMasterData(entityPoco.Id);
                        }

                        ObjectTableRepository objectTabelRepository = new ObjectTableRepository(webfreightContext);
                        ObjectTable objectTable = objectTabelRepository.GetObjectTableByName("Shipment", 0, true);
                        string objectTableId = objectTable.Id;

                        List<TraceEvent> shipmentEvents = traceEventRepository.GetTraceEvents(tenant, entityPoco.Id, objectTableId).ToList();
                        List<TraceEventParams> toBuildEvents = new List<TraceEventParams>();
                        foreach (TraceEventPM traceEvent in eventsList)
                        {
                            bool exists = false;
                            if (!string.IsNullOrEmpty(traceEvent.ExternalId))
                            {
                                exists = shipmentEvents.Where(e => e.ExternalId != null && e.ExternalId.Trim() == traceEvent.ExternalId.Trim()).Any();
                            }

                            if (!exists)
                            {
                                string currentUserId = null;
                                if (!string.IsNullOrEmpty(traceEvent.UserId))
                                {
                                    User user = userrepository.GetSingleUserByCode(traceEvent.UserId, tenant, true);
                                    currentUserId = user != null ? user.Id : null;
                                }
                                TraceEventParams parameters = new TraceEventParams()
                                {
                                    ExternalId = traceEvent.ExternalId,
                                    Tenant = tenant,
                                    CurrentStatusId = entityPoco.StatusId,
                                    EventTypeCode = traceEvent.EventTypeCode,
                                    EventDate = traceEvent.EventDateTime,
                                    LogDate = traceEvent.LogDateTime,
                                    UserId = currentUserId,
                                    EntityId = entityPoco.Id,
                                    Notes = traceEvent.Notes,
                                    ObjectTableName = "Shipment",

                                };

                                toBuildEvents.Add(parameters);
                            }
                        }

                        if (toBuildEvents.Count() > 0)
                        {
                            Logitude.Server.Tools.Response eventResponse = EventTracer.CreateTraceEventsList(toBuildEvents, tenant, "Shipment", entityPoco.StatusId);
                            if (!eventResponse.HasError)
                            {
                                string newStatus = eventResponse.Result;
                                if (newStatus != entityPoco.StatusId)
                                {
                                    IWebFreightContext objectContext = WebFreightContext.GetContext(tenant);

                                    EventTypeRepository eventTypesRepository = new EventTypeRepository(objectContext);
                                    ObjectTableRepository objectTableRepository = new ObjectTableRepository(objectContext);

                                    Tenant tenantEntity = TenantRepository.GetSingleTenant(tenant, true);

                                    List<string> toBuildEventTypeCodes = new List<string>();
                                    foreach (TraceEventParams ev in toBuildEvents)
                                    {
                                        toBuildEventTypeCodes.Add(ev.EventTypeCode);
                                    }

                                    List<EventType> eventTypes = eventTypesRepository.GetEventTypesByCodes(tenant, objectTable.Id, toBuildEventTypeCodes).ToList();
                                    List<EventType> newStatusEventTypes = eventTypes.Where(e => e.EntityStatusId == newStatus).ToList();

                                    List<string> typeCodes = new List<string>();
                                    foreach (EventType evt in newStatusEventTypes)
                                    {
                                        typeCodes.Add(evt.Code);
                                    }

                                    TraceEventParams builtEvent = (from a in toBuildEvents
                                                                   where typeCodes.Contains(a.EventTypeCode)
                                                                   select a).OrderBy(s => s.LogDate).Last();

                                    entityPoco.StatusId = newStatus;
                                    entityPoco.StatusDate = builtEvent.EventDate;//TenantServerConfigration.GetCurrentDateTime(tenant);
                                    entityPoco.LastStatusLogDate = TenantServerConfigration.GetCurrentDateTime(tenant);

                                    entityPM.StatusId = newStatus;
                                    entityPM.StatusDate = builtEvent.EventDate;//TenantServerConfigration.GetCurrentDateTime(tenant);
                                    entityPM.LastStatusLogDate = TenantServerConfigration.GetCurrentDateTime(tenant);

                                    ShipmentMapping.BuildSearchField(entityPM, entityPoco, masterData, entityPM.ShipmentPackages);

                                    if (masterData != null && masterData.StatusId != newStatus)
                                    {
                                        masterData.StatusId = entityPoco.StatusId;
                                        masterData.StatusDate = entityPoco.StatusDate;

                                        shipmentMasterDataRepository.Update(masterData);
                                    }

                                    shipmentRepository.Update(entityPoco);
                                    shipmentsContext.SaveChanges();

                                    RunStoredProcedureClass.CreateShipmentQueue(entityPM.Id, entityPM.Tenant);
                                }

                                TraceEventParams exceptionbuiltEvent = (from a in toBuildEvents
                                                                        where a.EventTypeCode.ToUpper() == "EXCE"
                                                                        select a).OrderByDescending(s => s.LogDate).FirstOrDefault();

                                TraceEventParams exceptionResolvedEvent = (from a in toBuildEvents
                                                                           where a.EventTypeCode.ToUpper() == "EXRE"
                                                                           select a).OrderByDescending(s => s.LogDate).FirstOrDefault();

                                if (exceptionbuiltEvent != null)
                                {
                                    TraceEvent newTraceEvent = traceEventRepository.GetSingleTraceEventByExternalIdandEventTypeCode(exceptionbuiltEvent.ExternalId, exceptionbuiltEvent.EventTypeCode, tenant);
                                    ContactsUnseenEntitiesHelper.AddUnseenEntityRecord(newTraceEvent.Id, tenant);
                                    entityPoco.ExceptionDate = newTraceEvent.EventDateTime;

                                    if (newTraceEvent.Notes != null)
                                    {
                                        if (!string.IsNullOrEmpty(newTraceEvent.Notes) && newTraceEvent.Notes.Length > 500) entityPoco.ExceptionDescription = newTraceEvent.Notes.Substring(0, 499);
                                        else entityPoco.ExceptionDescription = newTraceEvent.Notes;
                                    }

                                    entityPoco.LastExceptionDescription = newTraceEvent.Notes;
                                    entityPoco.HasException = true;
                                    entityPoco.ExceptionResolvedDescription = null;

                                    if (entityPoco.ShipmentLevelCode == "A")
                                    {
                                        List<Shipment> connectedShipments = shipmentRepository.GetConnectedCustomShipments(entityPM.Tenant, entityPM.Id).ToList();
                                        foreach (Shipment shipment in connectedShipments)
                                        {
                                            shipment.HasException = entityPoco.HasException;
                                            shipment.ExceptionDate = entityPoco.ExceptionDate;
                                            shipment.ExceptionDescription = entityPoco.ExceptionDescription;
                                            shipment.LastExceptionDescription = entityPoco.LastExceptionDescription;
                                            shipment.ExceptionResolvedDescription = entityPoco.ExceptionResolvedDescription;
                                            shipmentRepository.Update(shipment);
                                        }
                                    }

                                    shipmentRepository.Update(entityPoco);
                                    shipmentsContext.SaveChanges();
                                }

                                if (exceptionResolvedEvent != null)
                                {
                                    TraceEvent newTraceEvent = traceEventRepository.GetSingleTraceEventByExternalIdandEventTypeCode(exceptionResolvedEvent.ExternalId, exceptionbuiltEvent.EventTypeCode, tenant);
                                    entityPoco.ExceptionResolvedDescription = newTraceEvent.Notes;
                                    entityPoco.HasException = false;

                                    if (entityPoco.ShipmentLevelCode == "A")
                                    {
                                        List<Shipment> connectedShipments = shipmentRepository.GetConnectedCustomShipments(entityPM.Tenant, entityPM.Id).ToList();
                                        foreach (Shipment shipment in connectedShipments)
                                        {
                                            shipment.ExceptionResolvedDescription = entityPoco.ExceptionResolvedDescription;
                                            shipment.HasException = entityPoco.HasException;
                                            shipmentRepository.Update(shipment);
                                        }
                                    }

                                    shipmentRepository.Update(entityPoco);
                                    shipmentsContext.SaveChanges();
                                }

                            }
                            else
                            {
                                response.HasError = true;
                                response.ErrorMessage = eventResponse.ErrorMessage;
                            }
                        }

                        RunStoredProcedureClass.UpdateShipmentStatus(entityPM.Id, entityPM.Tenant);


                        if (!response.HasError)
                        {
                            response.Result = entityPoco.StatusId;
                        }

                    }
                    else
                    {
                        response.HasError = true;
                        response.ErrorMessage = "Shipment doesn't exist!";
                    }

                    scope.Complete();
                    return response;
                }
            }

            catch (Exception ex)
            {
                response.IsAuthenticationError = ex.GetType() == typeof(AutenticationException);
                response.HasError = true;
                response.ErrorMessage = ex.Message;
                response.InnerErrorMessage = (ex.InnerException != null ? (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : ex.InnerException.Message) : null);

                if (!string.IsNullOrEmpty(ex.StackTrace))
                {
                    response.ErrorMessage += Environment.NewLine + ex.StackTrace;
                }

                return response;
            }


        }



        public List<ShipmentList> GetShipmentList(DataContracts.ShipmentApiFilters filters, int tenant, ref Response response)
        {
            if (CacheManager.CacheWrapper == null)
            {
                CacheManager.CacheWrapper = new MockCacheWrapper();
            }

            try
            {
                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Shipment", "READ", tenant);

                TenantQuery tenantQuery = new TenantQuery(tenant);
                TenantPM currentTenant = tenantQuery.GetSinglePM(tenant);

                List<ShipmentList> result = new List<ShipmentList>();
                ShipmentRepository shipmentRepository = new ShipmentRepository(tenant);
                ContactRepository contactRepository = new ContactRepository(tenant);

                IShipmentsContext context = ShipmentsContext.GetContext(tenant);
                ShipmentsContext activeContext = context.GetActiveDbContext() as ShipmentsContext;
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@Tenant", tenant));
                parameters.Add(new SqlParameter("@IsOperationalClosed", (filters.OperationallyOpen ? "0" : "1")));
                parameters.Add(new SqlParameter("@IsCancelled","0"));
                string Where = " Where Shipments.Tenant = @Tenant and Shipments.IsOperationalClosed = @IsOperationalClosed and Shipments.IsCancelled = @IsCancelled";
                if (!string.IsNullOrEmpty(filters.SearchFields))
                { 
                    if (LogitudeSettings.DeploymentStage == "Simplog")
                    {
                        parameters.Add(new SqlParameter("@SearchFields", "%" + filters.SearchFields + "%"));
                        Where += " and Shipments.SearchFields like @SearchFields";
                    }
                    else
                    {
                        parameters.Add(new SqlParameter("@SearchFields", "\"" + filters.SearchFields + "*\""));
                        Where += " and Contains(Shipments.SearchFields,@SearchFields)";
                    }

                }
                if (filters.MyShipments && !string.IsNullOrEmpty(filters.Email))
                {
                    Contact contact = contactRepository.GetSingleContactByEmail(filters.Email, tenant);
                    if (contact != null)
                    {
                        parameters.Add(new SqlParameter("@ContactId", contact.Id));
                        Where += " and Shipments.AccountManagerUserId = @ContactId";
                        //Where += " and (Shipments.CustomerId in (select CardId from CardContacts where ContactId = '@ContactId') or Shipments.AgentId in (select CardId from CardContacts where ContactId = '@ContactId'))";
                    }
                }
                if (!string.IsNullOrEmpty(filters.TransPortMod))
                {
                    parameters.Add(new SqlParameter("@TransportModeId", filters.TransPortMod));
                    Where += " and Shipments.TransportModeId = @TransportModeId";
                }
                if (!string.IsNullOrEmpty(filters.Direction))
                {
                    parameters.Add(new SqlParameter("@DirectionId", filters.Direction));
                    Where += " and Shipments.DirectionId = @DirectionId";
                }
                if (!string.IsNullOrEmpty(filters.ShipmentLevel))
                {
                    parameters.Add(new SqlParameter("@ShipmentLevelCode", filters.ShipmentLevel));
                    Where += " and Shipments.ShipmentLevelCode = @ShipmentLevelCode";
                }
                //Where += " order by Shipments.CreateDateTime desc"; 
                IQueryable<OutlookShipmentView> shipments = activeContext.Database.SqlQuery<OutlookShipmentView>("SELECT top 51 Shipments.Id,Shipments.CreateDateTime,Shipments.StatusDate, Shipments.SearchFields,CustomerCards.SearchFields as CardsSearchFields, Shipments.Tenant, Shipments.ShipmentNumber,REPLACE(Shipments.Routing,',','>') as Routing,ShipmentMasterDatas.AirlinePrefix, Shipments.AccountManagerUserId,Shipments.IsOperationalClosed,ShipmentMasterDatas.Master,Shipments.House,Shipments.DirectionId, Shipments.TransportModeId ,CustomerCards.EnglishName AS CustomerName,AgentsCards.EnglishName AS AgentName,ShipmentLevels.Name as Type,ShipmentLevels.Code AS ShipmentLevelCode FROM Shipments LEFT OUTER JOIN ShipmentMasterDatas ON ShipmentMasterDatas.Id = Shipments.MasterShipmentDataId LEFT OUTER JOIN  ShipmentLevels AS ShipmentLevels ON Shipments.ShipmentLevelCode = ShipmentLevels.Code LEFT OUTER JOIN  Cards AS CustomerCards ON Shipments.CustomerId = CustomerCards.Id LEFT OUTER JOIN  Cards AS AgentsCards ON Shipments.AgentId = AgentsCards.Id LEFT OUTER JOIN  ShipmentTypes AS ShipmentTypes ON Shipments.ShipmentTypeId = ShipmentTypes.Id " + Where, parameters.ToArray()).AsQueryable();

                //IQueryable<Shipment> shipments = shipmentRepository.GetShipments(tenant);


                result = (from f in shipments
                          select new ShipmentList()
                          {
                              Id = f.Id,
                              DirectionId = f.DirectionId,
                              DirectionName = f.DirectionId == "I" ? "Import" : f.DirectionId == "E" ? "Export" : f.DirectionId == "D" ? "Domestic" : "Customs Import",
                              TransportModeName = f.TransportModeId == "I" ? "Inland" : f.TransportModeId == "A" ? "Air" : "Ocean",
                              ShipmentNumber = f.ShipmentNumber,
                              TransportModeId = f.TransportModeId,
                              Routing = f.Routing,
                              CustomerName = f.CustomerName,
                              LongMaster = f.TransportModeId == "A" ? (!string.IsNullOrEmpty(f.AirlinePrefix) && !string.IsNullOrEmpty(f.Master) ? f.AirlinePrefix + "-" + f.Master : "") : f.Master,
                              SearchFields = f.SearchFields + "," + f.CardsSearchFields,
                              ShipmentType = f.Type,
                              AgentName = f.AgentName,
                              ShipmentLevelCode = f.ShipmentLevelCode,
                              House = f.House,
                              Master = f.Master,
                              SearchFieldsText = !string.IsNullOrEmpty(filters.SearchFields) ? SearchFieldsFinder.HybridFind(f.SearchFields, filters.SearchFields) : "",
                              StatusDate = f.StatusDate
                          }).ToList();

                if (!string.IsNullOrEmpty(filters.SearchFields))
                {
                    result = result.Where(a => a.SearchFields.ToLower().Contains(filters.SearchFields.ToLower())).ToList();
                }
                //if (!string.IsNullOrEmpty(filters.SearchFields))
                //{
                //    foreach (var list in result)
                //    {
                //        list.SearchFieldsText = SearchFieldsFinder.HybridFind(list.SearchFields, filters.SearchFields);
                //    }
                //}

                return result;
            }
            catch (Exception ex)
            {
                response = new Response();
                response.IsAuthenticationError = ex.GetType() == typeof(AutenticationException);
                response.HasError = true;
                response.ErrorMessage = ex.Message;
                response.InnerErrorMessage = ex.InnerException != null ? ex.InnerException.Message : null;
                if (!string.IsNullOrEmpty(ex.StackTrace))
                {
                    response.ErrorMessage += Environment.NewLine + ex.StackTrace;
                }

                return null;

            }
        }

        public Response DeleteShipmentEvent(string shipmentNumber, string traceEventId, int tenant)
        {
            if (CacheManager.CacheWrapper == null)
            {
                CacheManager.CacheWrapper = new MockCacheWrapper();
            }

            Response response = new Response();
            try
            {
                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Shipment", "UPDATE", tenant);
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {

                    ICommonDataContext commoncontext = CommonDataContext.GetContext(tenant);
                    IShipmentsContext shipmentsContext = ShipmentsContext.GetContext(tenant);


                    ShipmentRepository shipmentRepository = new ShipmentRepository(shipmentsContext);
                    ShipmentMasterDataRepository shipmentMasterDataRepository = new ShipmentMasterDataRepository(shipmentsContext);

                    TraceEventRepository traceEventRepository = new TraceEventRepository(tenant);

                    ShipmentQuery shipmentQuery = new ShipmentQuery(shipmentRepository);
                    ShipmentPM entityPM = shipmentQuery.GetSingleShipmentPMByNumber(shipmentNumber, tenant);

                    if (entityPM != null)
                    {

                        TraceEvent traceEvent = traceEventRepository.GetSingleTraceEventByExternalId(traceEventId, tenant);
                        if (traceEvent != null && !traceEvent.Deleted)
                        {
                            //ShipmentTracing.DeleteShipmentTraceEvent(entityPM, traceEvent.Id, tenant, true);
                            this.DeleteShipmentTraceEvent(entityPM, traceEvent.Id, tenant, true);

                            RunStoredProcedureClass.CreateShipmentQueue(entityPM.Id, entityPM.Tenant);
                            RunStoredProcedureClass.UpdateShipmentStatus(entityPM.Id, entityPM.Tenant);

                            response.Result = entityPM.StatusId;
                        }
                        else
                        {
                            response.HasError = true;
                            response.ErrorMessage = "Event doesn't exist!";
                        }
                    }
                    else
                    {
                        response.HasError = true;
                        response.ErrorMessage = "Shipment doesn't exist!";
                    }

                    scope.Complete();
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.IsAuthenticationError = ex.GetType() == typeof(AutenticationException);
                response.HasError = true;
                response.ErrorMessage = ex.Message;
                response.InnerErrorMessage = (ex.InnerException != null ? (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : ex.InnerException.Message) : null);

                if (!string.IsNullOrEmpty(ex.StackTrace))
                {
                    response.ErrorMessage += Environment.NewLine + ex.StackTrace;
                }

                return response;
            }
        }

        private void DeleteShipmentTraceEvent(ShipmentPM entityPM, string traceEventId, int tenant, bool external)
        {
            ShipmentTracing.DeleteShipmentTraceEventForHybrid(entityPM, traceEventId, tenant);
        }

    }
    class OutlookShipmentView
    {

        public string Id { get; set; }
        public string DirectionId { get; set; }
        public string ShipmentNumber { get; set; }
        public string TransportModeId { get; set; }
        public string Routing { get; set; }
        public string CustomerName { get; set; }
        public string AirlinePrefix { get; set; }
        public string Master { get; set; }
        public string SearchFields { get; set; }
        public string CardsSearchFields { get; set; }
        public string AccountManagerUserId { get; set; }
        public int Tenant { get; set; }
        public bool IsOperationalClosed { get; set; }
        public DateTime CreateDateTime { get; set; }
        public string CustomerId { get; set; }
        public string Type { get; set; }
        public string AgentName { get; set; }
        public string ShipmentLevelCode { get; set; }
        public string House { get; set; }
        public DateTime? StatusDate { get; set; }
    }

}