using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.StorageService;
using Microsoft.Practices.Unity;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.QuoteModel;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Data.QuoteModel.Repositories;
using Simplog.Server.Infrastructure.Azure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;
using System.Transactions;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Logitude.BL.Validators;
using WebFreight.Web.WebServices;
using Logitude.BL.QuoteModel.EntityPMs;
using Logitude.BL.QuoteModel.Tools.EntityService;
using Logitude.BL.QuoteModel.EntityQueries;
using Logitude.BL.QuoteModel.EntityLists;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.QuoteModel.Tools.TraceEvents;

namespace WebFreight.Web.WcfApi
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "QuoteWcfService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select QuoteWcfService.svc or QuoteWcfService.svc.cs at the Solution Explorer and start debugging.
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class QuoteWcfService : IQuoteWcfService
    {
        public Response Upsert(QuotePM entityPM, bool batch)
        {
            if (CacheManager.CacheWrapper == null)
            {
                CacheManager.CacheWrapper = new MockCacheWrapper();
            }


            Response response = new Response();
            try
            {
                SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
                SecurityUtility.CheckContactFeature("Quote", "UPDATE", entityPM.Tenant);//UPDATE//READ
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
                    CurrencyRepository currencyRepository = new CurrencyRepository(commoncontext);
                    ContactRepository contactRepository = new ContactRepository(commoncontext);
                    IQuotesContext objectContext = QuotesContext.GetContext(entityPM.Tenant);
                    QuoteRepository quoteRepository = new QuoteRepository(objectContext); // ???????????
                    AgentRepository agentRepository = new AgentRepository(commoncontext);

                    //"system@tenant1.com"
                    entityPM.IsQuoteDataExternal = true;
                    QuoteStageRepository quoteStageRepository = new QuoteStageRepository(objectContext);
                    entityPM.IsHybrid = true;

                    if (string.IsNullOrEmpty(entityPM.StageId))
                    {

                        entityPM.StageId = quoteStageRepository.GetSingleQuoteStageByCode("QTDR", entityPM.Tenant).Id;
                    }

                    else
                    {
                        QuoteStage stage = quoteStageRepository.GetSingleQuoteStageByCode(entityPM.StageId, entityPM.Tenant);
                        if (stage != null)
                        {
                            entityPM.StageId = stage.Id;
                        }
                        else
                        {
                            response.HasError = true;
                            response.ErrorMessage = "StageId field doesn't exist in the database,Upsert this entity before using it.";
                            return response;
                        }
                    }

                    ClassLevelValidator validationClass = new ClassLevelValidator("Quote", entityPM.Tenant) { IsHybrid = true };
                    if (!validationClass.IsValid(entityPM, entityPM, null))
                    {
                        response.HasError = true;
                        response.ErrorMessage = validationClass.GetErrorMessage(entityPM, null);
                        return response;
                    }

                    User user = userrepository.GetSingleUserByCode(entityPM.CreatedByUserId, entityPM.Tenant, true);
                    if (user == null)
                    {
                        response.HasError = true;
                        response.ErrorMessage = "CreatedByUserId field doesn’t  exist!";
                        return response;
                    }

                    //QTCR
                    #region Resolving Keys




                    WcfServicesHelper.SetPortId(entityPM.FromPortId, entityPM.Tenant, "FromPortId", entityPM, portRepository, response);
                    WcfServicesHelper.SetPortId(entityPM.ToPortId, entityPM.Tenant, "ToPortId", entityPM, portRepository, response);

                    if (entityPM.ShipperId != null)
                    {
                        Card card = cardsReporistory.GetSingleCardByCode(entityPM.ShipperId, entityPM.Tenant, false);
                        if (card != null)
                        {
                            entityPM.ShipperId = card.Id;
                            entityPM.ShipperName = card.EnglishName;
                        }
                        else
                        {
                            response.HasError = true;
                            response.ErrorMessage = "ShipperId field doesn't exist in the database,Upsert this entity before using it.";
                            return response;
                        }
                    }

                    if (entityPM.FreelancerId != null)
                    {
                        Card card = cardsReporistory.GetSingleCardByCode(entityPM.FreelancerId, entityPM.Tenant, false);
                        if (card != null)
                        {
                            entityPM.FreelancerId = card.Id;
                            entityPM.FreelancerName = card.EnglishName;
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
                        Card card = cardsReporistory.GetSingleCardByCode(entityPM.CustomerId, entityPM.Tenant, false);
                        if (card != null)
                        {
                            entityPM.CustomerId = card.Id;
                            entityPM.CustomerName = card.EnglishName;
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
                        Card card = cardsReporistory.GetSingleCardByCode(entityPM.ConsigneeId, entityPM.Tenant, false);
                        if (card != null)
                        {
                            entityPM.ConsigneeId = card.Id;
                            entityPM.ConsigneeName = card.EnglishName;
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



                    if (entityPM.MainCarriageCarrierId != null)
                    {
                        Card card = cardsReporistory.GetSingleCardByCode(entityPM.MainCarriageCarrierId, entityPM.Tenant, false);
                        if (card != null)
                        {
                            entityPM.MainCarriageCarrierId = card.Id;
                             
                        }
                        else
                        {
                            response.HasError = true;
                            response.ErrorMessage = "MainCarriageCarrierId field doesn't exist in the database,Upsert this entity before using it.";
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


                    if (entityPM.CreatedByUserId != null)
                    {
                        User userentity = userrepository.GetSingleUserByCode(entityPM.CreatedByUserId, entityPM.Tenant, true);
                        if (userentity != null)
                        {
                            entityPM.CreatedByUserId = userentity.Id;
                        }
                        else
                        {
                            response.HasError = true;
                            response.ErrorMessage = "CreatedByUserId field doesn't exist in the database,Upsert this entity before using it.";
                            return response;
                        }
                    }

                    if (entityPM.SaleCurrencyId != null)
                    {
                        Currency currency = CurrencyRepository.GetSingleCurrencyByCode(entityPM.SaleCurrencyId, entityPM.Tenant, true);
                        if (currency != null)
                        {
                            entityPM.SaleCurrencyId = currency.Id;
                        }
                        else
                        {
                            response.HasError = true;
                            response.ErrorMessage = "SaleCurrencyId field doesn't exist in the database,Upsert this entity before using it.";
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


                    if (entityPM.CustomerContactId != null)
                    {
                        Contact customercontact = contactRepository.GetSingleContactByExternalId(entityPM.CustomerContactId, entityPM.Tenant);
                        if (customercontact != null)
                        {
                            entityPM.CustomerContactId = customercontact.Id;
                        }
                        else
                        {
                            response.HasError = true;
                            response.ErrorMessage = "CustomerContactId field doesn't exist in the database,Upsert this entity before using it.";
                            return response;
                        }
                    }

                    if (entityPM.ShipperContactId != null)
                    {
                        Contact shippercontact = contactRepository.GetSingleContactByExternalId(entityPM.ShipperContactId, entityPM.Tenant);
                        if (shippercontact != null)
                        {
                            entityPM.CustomerContactId = shippercontact.Id;
                        }
                        else
                        {
                            response.HasError = true;
                            response.ErrorMessage = "ShipperContactId field doesn't exist in the database,Upsert this entity before using it.";
                            return response;
                        }
                    }

                    if (entityPM.ConsigneeContactId != null)
                    {
                        Contact consigneecontact = contactRepository.GetSingleContactByExternalId(entityPM.ConsigneeContactId, entityPM.Tenant);
                        if (consigneecontact != null)
                        {
                            entityPM.CustomerContactId = consigneecontact.Id;
                        }
                        else
                        {
                            response.HasError = true;
                            response.ErrorMessage = "ConsigneeContactId field doesn't exist in the database,Upsert this entity before using it.";
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

                    #endregion


                    #region QuotePackages

                    foreach (QuotePackagePM package in entityPM.QuotePackages)
                    {
                        package.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;

                        if (!string.IsNullOrEmpty(package.PackageTypeId))
                        {
                            PackageTypeRepository packageTypeRep = new PackageTypeRepository(commoncontext);
                            PackageType type = packageTypeRep.GetSinglePackageTypeByCode(package.PackageTypeId, entityPM.Tenant, true);

                            if (type != null)
                            {
                                package.PackageTypeId = type.Id;
                            }
                            else
                            {
                                response.HasError = true;
                                response.ErrorMessage = "PackageTypeId field doesn't exist in the database,Upsert this entity before using it.";
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

                    if (!string.IsNullOrEmpty(entityPM.PackageType1Id))
                    {
                        PackageTypeRepository packageTypeRep = new PackageTypeRepository(commoncontext);
                        PackageType type = packageTypeRep.GetSinglePackageTypeByCode(entityPM.PackageType1Id, entityPM.Tenant, true);

                        if (type != null)
                        {
                            entityPM.PackageType1Id = type.Id;
                    }
                        else
                        {
                            response.HasError = true;
                            response.ErrorMessage = "PackageType1Id field doesn't exist in the database,Upsert this entity before using it.";
                            return response;
                        }
                    }


                    if (!string.IsNullOrEmpty(entityPM.PackageType2Id))
                    {
                        PackageTypeRepository packageTypeRep = new PackageTypeRepository(commoncontext);
                        PackageType type = packageTypeRep.GetSinglePackageTypeByCode(entityPM.PackageType2Id, entityPM.Tenant, true);

                        if (type != null)
                        {
                            entityPM.PackageType2Id = type.Id;
                        }
                        else
                        {
                            response.HasError = true;
                            response.ErrorMessage = "PackageType2Id field doesn't exist in the database,Upsert this entity before using it.";
                            return response;
                        }
                    }



                    if (!string.IsNullOrEmpty(entityPM.PackageType3Id))
                    {
                        PackageTypeRepository packageTypeRep = new PackageTypeRepository(commoncontext);
                        PackageType type = packageTypeRep.GetSinglePackageTypeByCode(entityPM.PackageType3Id, entityPM.Tenant, true);

                        if (type != null)
                        {
                            entityPM.PackageType3Id = type.Id;
                        }
                        else
                        {
                            response.HasError = true;
                            response.ErrorMessage = "PackageType3Id field doesn't exist in the database,Upsert this entity before using it.";
                            return response;
                        }
                    }


                    if (!string.IsNullOrEmpty(entityPM.PackageType4Id))
                    {
                        PackageTypeRepository packageTypeRep = new PackageTypeRepository(commoncontext);
                        PackageType type = packageTypeRep.GetSinglePackageTypeByCode(entityPM.PackageType4Id, entityPM.Tenant, true);

                        if (type != null)
                        {
                            entityPM.PackageType4Id = type.Id;
                        }
                        else
                        {
                            response.HasError = true;
                            response.ErrorMessage = "PackageType4Id field doesn't exist in the database,Upsert this entity before using it.";
                            return response;
                        }
                    }



                    if (!string.IsNullOrEmpty(entityPM.PackageType5Id))
                    {
                        PackageTypeRepository packageTypeRep = new PackageTypeRepository(commoncontext);
                        PackageType type = packageTypeRep.GetSinglePackageTypeByCode(entityPM.PackageType5Id, entityPM.Tenant, true);

                        if (type != null)
                        {
                            entityPM.PackageType5Id = type.Id;
                        }
                        else
                        {
                            response.HasError = true;
                            response.ErrorMessage = "PackageType5Id field doesn't exist in the database,Upsert this entity before using it.";
                            return response;
                        }
                    }
                    
                    #endregion

                    #region QuoteCharges

                    foreach (QuoteChargePM charge in entityPM.QuoteCharges)
                    {
                        #region QuotePriceSteps
                        foreach (QuotePriceStepsPM price in charge.QuoteChargePriceSteps)
                        {
                        price.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
                    }
                    #endregion

                        charge.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;

                        if (!string.IsNullOrEmpty(charge.ChargesTypeId))
                        {

                            ChargesTypeRepository chargeTypeRep = new ChargesTypeRepository(commoncontext);
                            ChargesType chargetype = chargeTypeRep.GetSingleChargesTypeByCode(charge.ChargesTypeId, entityPM.Tenant);
                            if (chargetype != null)
                            {
                                charge.ChargesTypeId = chargetype.Id;
                            }
                            else
                            {
                                response.HasError = true;
                                response.ErrorMessage = "ChargesTypeId field doesn't exist in the database,Upsert this entity before using it.";
                                return response;
                            }
                        }
                        else
                        {
                            response.HasError = true;
                            response.ErrorMessage = "ChargesTypeId field is required";
                            return response;
                        }

                        if (!string.IsNullOrEmpty(charge.SaleCurrencyId))
                        {
                            Currency salecurrency = currencyRepository.GetSingleCurrencyByCode(charge.SaleCurrencyId, entityPM.Tenant);
                            if (salecurrency != null)
                            {
                                charge.SaleCurrencyId = salecurrency.Id;
                            }
                            else
                            {
                                response.HasError = true;
                                response.ErrorMessage = "SaleCurrencyId field doesn't exist in the database,Upsert this entity before using it.";
                                return response;
                            }
                        }
                        else
                        {
                            response.HasError = true;
                            response.ErrorMessage = "SaleCurrencyId field is required";
                            return response;
                        }

                        if (!string.IsNullOrEmpty(charge.UpdatedByUserId))
                        {
                            User updatedbyuser = userrepository.GetSingleUserByCode(charge.UpdatedByUserId, entityPM.Tenant, true);
                            if (updatedbyuser != null)
                            {
                                charge.UpdatedByUserId = updatedbyuser.Id;
                            }
                            else
                            {
                                response.HasError = true;
                                response.ErrorMessage = "UpdatedByUserId field doesn't exist in the database,Upsert this entity before using it.";
                                return response;
                            }
                        }
                        else
                        {
                            response.HasError = true;
                            response.ErrorMessage = "UpdatedByUserId field is required";
                            return response;
                        }

                        if (!string.IsNullOrEmpty(charge.CostMeasurementId))
                        {
                            MeasurementRepository meaurementRepository = new MeasurementRepository(commoncontext);
                            Measurement measurement = meaurementRepository.GetMeasurementbyCode(charge.CostMeasurementId, entityPM.Tenant);
                            if (measurement != null)
                            {
                                charge.CostMeasurementId = measurement.Id;
                            }
                            else
                            {
                                response.HasError = true;
                                response.ErrorMessage = "CostMeasurementId field doesn't exist in the database,Upsert this entity before using it.";
                                return response;
                            }
                        }
                        else
                        {
                            response.HasError = true;
                            response.ErrorMessage = "CostMeasurementId field is required";
                            return response;
                        }

                        if (!string.IsNullOrEmpty(charge.SaleMeasurementId))
                        {
                            MeasurementRepository meaurementRepository = new MeasurementRepository(commoncontext);
                            Measurement salemeasurement = meaurementRepository.GetMeasurementbyCode(charge.SaleMeasurementId, entityPM.Tenant);
                            if (salemeasurement != null)
                            {
                                charge.SaleMeasurementId = salemeasurement.Id;
                            }
                            else
                            {
                                response.HasError = true;
                                response.ErrorMessage = "SaleMeasurementId field doesn't exist in the database,Upsert this entity before using it.";
                                return response;
                            }
                        }
                        else
                        {
                            response.HasError = true;
                            response.ErrorMessage = "SaleMeasurementId field is required";
                            return response;
                        }


                        if (!string.IsNullOrEmpty(charge.VendorId))
                        {
                            VendorRepository vendorRepository = new VendorRepository(commoncontext);
                            Vendor vendor = vendorRepository.GetSingleVendorByCode(charge.VendorId, entityPM.Tenant);
                            if (vendor != null)
                            {
                                charge.VendorId = vendor.Id;
                            }
                            else
                            {
                                response.HasError = true;
                                response.ErrorMessage = "VendorId field doesn't exist in the database,Upsert this entity before using it.";
                                return response;
                            }
                        }
                        else
                        {
                            response.HasError = true;
                            response.ErrorMessage = "VendorId field is required";
                            return response;
                        }


                        if (!string.IsNullOrEmpty(charge.CostCurrencyId))
                        {
                            Currency costcurrency = currencyRepository.GetSingleCurrencyByCode(charge.CostCurrencyId, entityPM.Tenant);
                            if (costcurrency != null)
                            {
                                charge.CostCurrencyId = costcurrency.Id;
                            }
                            else
                            {
                                response.HasError = true;
                                response.ErrorMessage = "CostCurrencyId field doesn't exist in the database,Upsert this entity before using it.";
                                return response;
                            }
                        }
                        else
                        {
                            response.HasError = true;
                            response.ErrorMessage = "CostCurrencyId field is required";
                            return response;
                        }
                    }

                    #endregion


                    if (response.HasError)
                    {
                        return response;
                    }

                    Contact contact = ContactRepository.GetSingleContact(user.Id, entityPM.Tenant, true);
                    QuoteService service = new QuoteService(objectContext, entityPM.Tenant, contact.Email);

                    Quote entity = quoteRepository.GetSingleQuoteByNumber(entityPM.QuoteNumber, entityPM.Tenant);
                    if (entity == null)
                    {
                        service.SetChangeSet(entityPM.QuoteCharges, new List<QuoteFollowUpPM>(), entityPM.QuotePackages, new List<QuoteDocumentVersionPM>());
                        service.Create(entityPM);
                    }

                    else
                    {
                        if (entity.IsCancelled && entityPM.IsCancelled)
                        {
                            response.HasError = true;
                            response.ErrorMessage = "This quote is cancelled,reactivate this entity before using it.";
                            return response;
                        }

                        entityPM.Id = entity.Id;
                        entityPM.ConcurrencyGUID = entity.ConcurrencyGUID;

                        if (entity.IsClosed)
                        {
                            entityPM.StageId = entity.StageId;
                        }

                        QuotePackageQuery quotePackageQuery = new QuotePackageQuery(new QuotePackageRepository(objectContext));
                        List<QuotePackagePM> quotePackages = quotePackageQuery.GetQuotePackagesForQuotePMIDTenant(entity.Id, entity.Tenant);
                        foreach (QuotePackagePM package in quotePackages)
                        {
                            package.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Delete;
                            entityPM.QuotePackages.Add(package);
                        }

                        QuoteChargeQuery quoteChargeQuery = new QuoteChargeQuery(new QuoteChargeRepository(objectContext));
                        List<QuoteChargePM> quoteCharges = quoteChargeQuery.GetQuoteChargesForDeleting(entity.Id, entity.Tenant);
                        foreach (QuoteChargePM charge in quoteCharges)
                        {
                            charge.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Delete;
                            entityPM.QuoteCharges.Add(charge);
                        }

                        service.SetChangeSet(entityPM.QuoteCharges, new List<QuoteFollowUpPM>(), entityPM.QuotePackages, new List<QuoteDocumentVersionPM>());
                        service.Update(entityPM);
                    }

                    response.Result = entityPM.Id;

                    scope.Complete();
                    return response;
                }
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


        public Response UploadQuotationDocument(string quoteNumber, byte[] fileData, string fileExtension, string userId, int tenant)
        {
            if (CacheManager.CacheWrapper == null)
            {
                CacheManager.CacheWrapper = new MockCacheWrapper();
            }


            Response response = new Response();
            try
            {
                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Quote", "UPDATE", tenant);//UPDATE//READ
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    IQuotesContext objectContext = QuotesContext.GetContext(tenant);
                    QuoteDocumentVersionRepository entityRepository = new QuoteDocumentVersionRepository(objectContext);
                    QuoteRepository quoteRepository = new QuoteRepository(objectContext);
                    UserRepository userrepository = new UserRepository(tenant);
                    ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);

                    User user = userrepository.GetSingleUserByCode(userId, tenant, true);
                    if (user == null)
                    {
                        response.HasError = true;
                        response.ErrorMessage = "CreatedByUserId field doesn’t  exist!";
                        return response;
                    }

                    Quote quote = quoteRepository.GetSingleQuoteByNumber(quoteNumber, tenant);

                    if (quote == null)
                    {
                        response.HasError = true;
                        response.ErrorMessage = "Quote doesn’t  exist!";
                        return response;
                    }

                    quote.IsQuoteDocumentExternal = true;
                    QuoteDocumentVersion lastVersion = entityRepository.GetQuoteDocumentVersionsByQuoteId(quote.Id, tenant).OrderBy(s => s.VersionNumber).ToList().LastOrDefault();

                    QuoteDocumentVersion newVersion = new QuoteDocumentVersion()
                    {
                        VersionNumber = (lastVersion != null ? lastVersion.VersionNumber + 1 : 1),
                        CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                        UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                        CreatedByUserId = user.Id,
                        UpdatedByUserId = user.Id,
                        Tenant = tenant,
                        QuoteId = quote.Id,
                        VersionType = "U",

                    };


                    ObjectTableRepository tableRepository = new ObjectTableRepository(tenant);
                    DocumentTypeRepository documentTypeRepository = new DocumentTypeRepository(commonContext);
                    DocumentOutRepository documentOutRepository = new DocumentOutRepository(commonContext);
                    DocumentTypeCopyRepository documentTypeCopyRepository = new DocumentTypeCopyRepository(commonContext);
                    DocumentOutCopyRepository documentOutCopyRepository = new DocumentOutCopyRepository(commonContext);
                    DocumentsFilingRepository documentsFilingRepository = new DocumentsFilingRepository(commonContext);
                    DocumentRepository documentRep = new DocumentRepository(commonContext);
                    DocumentType documentType = documentTypeRepository.GetSingleDocumentTypeByCode("QUOTE", tenant);
                    if (documentType != null)
                    {
                        Document document = null;
                        DocumentOut documentout = documentOutRepository.GetDocumentOutByDocumentTypeAndEntity(quote.Id, documentType.Id, tenant);
                        if (documentout == null)
                        {
                          
                            //documentRep.SubmitChanges();

                            ObjectTable table = tableRepository.GetObjectTableByName("Quote", 0, true);

                            DocumentsFiling newDocumentFiling = new DocumentsFiling() { DocumentTypeId = documentType.Id, EntityId = quote.Id, Tenant = tenant, ObjectTableId = table.Id, ChildEntityId = null, ChildEntityReference = null, DirectionCode = "O" };
                            newDocumentFiling.Id = IdCounter.GetNumber("Document", tenant).ToString();
                            newDocumentFiling.Code = CodeCounter.GetNumber("DocumentsFiling", tenant).ToString();
                            newDocumentFiling.CreatedByUserId = user.Id;
                            newDocumentFiling.OwnerId = user.Id;
                            newDocumentFiling.CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                            newDocumentFiling.UpdatedByUserId = user.Id;
                            newDocumentFiling.UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                            newDocumentFiling.HasCopies = true;
                            newDocumentFiling.SearchFields = newDocumentFiling.Code + "," + newDocumentFiling.DirectionCode;
                            
                            newDocumentFiling.SecurityId = newDocumentFiling.Id + StringHelper.GetRandomString(10);

                            documentsFilingRepository.Add(newDocumentFiling);

                            document = new Document()
                            {
                                Id =  IdCounter.GetNumber("Document", tenant).ToString(),
                                CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                                Extension = fileExtension,
                                FileSize = Convert.ToInt32(fileData.Length),
                                Tenant = Convert.ToInt32(tenant),
                                Folder = "quotetemplatesectionfiles",
                                HasFile = true,
                                FileName = documentType.Name,
                            };

                            documentRep.Add(document);

                            newDocumentFiling.DocumentId = document.Id;
                            documentout = new DocumentOut()
                            {
                                Id = newDocumentFiling.Id,
                                EmailTemplateId = documentType.DocumentTypeDefaultHTMLTemplateId,
                                DocumentTemplateId = documentType.DocumentTypeDefaultReportTemplateId,
                                Tenant = tenant,
                                Issued = true,
                                IsBlobExist = true,
                                IssuedDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                                IssuedByUserId = user.Id,
                                  
                            };


                             newDocumentFiling.DocumentId = document.Id;
                            documentOutRepository.Add(documentout);

                            DocumentTypeCopy documentTypeCopy = documentTypeCopyRepository.GetSingleDocumentTypeCopyByDocumentTypeId(documentType.Id, tenant);
                            if (documentTypeCopy == null)
                            {
                                documentTypeCopy = new DocumentTypeCopy()
                                {
                                    Id = IdCounter.GetNumber("DocumentTypeCopy", tenant).ToString(),
                                    DocumentTypeId = documentType.Id,
                                    Code = documentType.Code,
                                    Name = documentType.Name,
                                    Tenant = tenant,

                                };

                                documentTypeCopyRepository.Add(documentTypeCopy);
                            }

                            DocumentOutCopy docoutCopy = new DocumentOutCopy()
                            {
                                Id = document.Id,
                                DocumentId = document.Id,
                                DocumentOutId = documentout.Id,
                                DocumentTypeCopyId = documentTypeCopy.Id,
                                Tenant = tenant,
                                 
                            };

                            documentOutCopyRepository.Add(docoutCopy);
                        }
                        else
                        {
                            //DocumentTypeCopy documentTypeCopy = documentTypeCopyRepository.context.DocumentTypeCopies.Where(d => d.DocumentTypeId == documentType.Id).FirstOrDefault();
                            DocumentOutCopy docoutCopy = (from a in commonContext.DocumentOutCopies
                                                          where a.DocumentOutId == documentout.Id && a.Tenant == tenant
                                                          select a).FirstOrDefault();//documentOutCopyRepository.GetDocumentOutCopyByDocumentOutAndType(documentout.Id, documentTypeCopy.Id, tenant);
                            if (docoutCopy != null)
                            {
                                if(!string.IsNullOrEmpty(docoutCopy.DocumentId))
                                {
                                    document = documentRep.GetSingleDocument(tenant, docoutCopy.DocumentId);
                                    document.FileName = documentType.Name;
                                    document.Extension = fileExtension;
                                    document.FileSize = Convert.ToInt32(fileData.Length);
                                    document.Tenant = Convert.ToInt32(tenant);
                                    document.Folder = "quotetemplatesectionfiles";
                                    document.HasFile = true;
                                 
                                    documentRep.Update(document);
                                     
                                }
                                else
                                {
                                    document = new Document()
                                    {
                                        Id = IdCounter.GetNumber("Document", tenant).ToString(),
                                        CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                                        Extension = fileExtension,
                                        FileSize = Convert.ToInt32(fileData.Length),
                                        Tenant = Convert.ToInt32(tenant),
                                        Folder = "quotetemplatesectionfiles",
                                        HasFile = true,
                                        FileName = documentType.Name,

                                    };

                                    documentRep.Add(document);
                                }

                                docoutCopy.DocumentId = document.Id;
                            }
                            else
                            {
                                document = new Document()
                                {
                                    Id = IdCounter.GetNumber("Document", tenant).ToString(),
                                    CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                                    Extension = fileExtension,
                                    FileSize = Convert.ToInt32(fileData.Length),
                                    Tenant = Convert.ToInt32(tenant),
                                    Folder = "quotetemplatesectionfiles",
                                    HasFile = true,
                                    FileName = documentType.Name,

                                };
                                documentRep.Add(document);
                                //documentRep.SubmitChanges();

                                DocumentTypeCopy documentTypeCopy = documentTypeCopyRepository.GetSingleDocumentTypeCopyByDocumentTypeId(documentType.Id, tenant);
                                if (documentTypeCopy == null)
                                {
                                    documentTypeCopy = new DocumentTypeCopy()
                                    {
                                        Id = IdCounter.GetNumber("DocumentTypeCopy", tenant).ToString(),
                                        DocumentTypeId = documentType.Id,
                                        Code = documentType.Code,
                                        Name = documentType.Name,
                                        Tenant = tenant,

                                    };

                                    documentTypeCopyRepository.Add(documentTypeCopy);
                                }

                                docoutCopy = new DocumentOutCopy()
                                {
                                    Id = document.Id,
                                    DocumentId = document.Id,
                                    DocumentOutId = documentout.Id,
                                    DocumentTypeCopyId = documentTypeCopy.Id,
                                    Tenant = tenant,

                                };

                                documentOutCopyRepository.Add(docoutCopy);
                            }

                        }

                        commonContext.SaveChanges();

                        quote.LastVersionNumber = newVersion.VersionNumber;

                        newVersion.DocumentId = document.Id;

                        string filename = document.Id + "." + document.Extension;
                        //string filePath = "tenant" + tenant.ToString() + "/" + StorageAcountDetails.GetBlobNameByLocation(filename.ToLower(), document.Folder);
                        BlobFileInfo fileInfo = new BlobFileInfo()
                        {
                            FileName = document.Id,
                            FolderName = document.Folder,
                            Extension = document.Extension,
                            Tenant = document.Tenant,
                            FileSize = fileData.Length,
                            IsEncrypted = true,

                        };
                        IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
                        storageservice.Write(fileData, fileInfo);
                        //if (response2.HasError)
                        //{
                        //    response.HasError = true;
                        //    response.ErrorMessage = response2.ErrorMessage;
                        //    response.InnerErrorMessage = response2.InnerErrorMessage;
                        //    return response;
                        //}


                        entityRepository.Add(newVersion);
                        entityRepository.SubmitChanges();
                        commonContext.SaveChanges();

                        response.Result = documentout.Id;
                    }
                    else
                    {
                        response.HasError = true;
                        response.ErrorMessage = "Document Type with code 'QUOTE' is not found!";
                        return response;
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


        public List<QuoteList> GetQuoteList(DataContracts.QuoteApiFilters filters, int tenant, ref Response response)
        {

            if (CacheManager.CacheWrapper == null)
            {
                CacheManager.CacheWrapper = new MockCacheWrapper();
            }

            try
            {
                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Quote", "READ", tenant);

                List<QuoteList> result = new List<QuoteList>();
                QuoteRepository quoteRepository = new QuoteRepository(tenant);
                ContactRepository contactRepository = new ContactRepository(tenant);
                IQueryable<Quote> quotes = quoteRepository.GetQuoteByTenant(tenant, !filters.OperationallyOpen, null);

                if (filters.MyQuotes && !string.IsNullOrEmpty(filters.Email))
                {
                    Contact contact = contactRepository.GetSingleContactByEmail(filters.Email, tenant);
                    if (contact != null)
                    {
                        quotes = quotes.Where(q => q.SalesmanUserId == contact.Id);
                    }
                }

                if (!string.IsNullOrEmpty(filters.SearchFields))
                {
                    quotes = quotes.Where(s => s.SearchFields.Contains(filters.SearchFields));
                }

                quotes = quotes.OrderByDescending(d => d.OpenDate).Skip(filters.Skip).Take(filters.Take);
                QuoteQuery quoteQuery = new QuoteQuery(quoteRepository);
                result = quoteQuery.GetIQueryableEntityList(quotes).ToList();

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
        public Response CreateEvent(int tenant, string externalId, string quoteNumber, string userId, string eventTypeCode, DateTime logDate, DateTime eventDate, string notes)
        {
            if (CacheManager.CacheWrapper == null)
            {
                CacheManager.CacheWrapper = new MockCacheWrapper();
            }

            Response response = new Response();
            try
            {
                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Quote", "UPDATE", tenant);//UPDATE//READ
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {

                    ICommonDataContext commoncontext = CommonDataContext.GetContext(tenant);
                    IQuotesContext quotesContext = QuotesContext.GetContext(tenant);

                    UserRepository userrepository = new UserRepository(commoncontext);
                    QuoteRepository quoteRepository = new QuoteRepository(quotesContext);

                    User user = userrepository.GetSingleUserByCode(userId, tenant, true);
                    TraceEventRepository traceEventRepository = new TraceEventRepository(tenant);

                    Quote entityPoco = quoteRepository.GetSingleQuoteByNumber(quoteNumber, tenant);
                    QuoteQuery quoteQuery = new QuoteQuery(quoteRepository);
                    if (entityPoco != null)
                    {

                        bool exists = false;
                        if (!string.IsNullOrEmpty(externalId))
                        {
                            exists = traceEventRepository.GetSingleTraceEventByExternalId(externalId, tenant) != null;
                        }

                        if (!exists)
                        {
                            EventTracer.CreateTraceEvent(new EventTracerArgs()
                            {
                                Tenant = tenant,
                                EventTypeCode = eventTypeCode,
                                UserId = user.Id,
                                EntityId = entityPoco.Id,
                                ObjectTableName = "Quote",
                                ExternalId = externalId,
                                Notes = notes,
                                EventDateTime = eventDate,
                                LogDateTime = logDate,
                            });
                        }


                    }
                    else
                    {
                        response.HasError = true;
                        response.ErrorMessage = "Quote doesn't exist!";

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

        public Response BuildEventsList(int tenant, string quoteNumber, List<TraceEventPM> eventsList)
        {

            if (CacheManager.CacheWrapper == null)
            {
                CacheManager.CacheWrapper = new MockCacheWrapper();
            }

            Response response = new Response();
            try
            {
                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Quote", "UPDATE", tenant);//UPDATE//READ
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {

                    eventsList = eventsList.OrderBy(e => e.EventDateTime).ToList();

                    ICommonDataContext commoncontext = CommonDataContext.GetContext(tenant);
                    IQuotesContext quotesContext = QuotesContext.GetContext(tenant);

                    UserRepository userrepository = new UserRepository(commoncontext);
                    QuoteRepository quoteRepository = new QuoteRepository(quotesContext);
                    TraceEventRepository traceEventRepository = new TraceEventRepository(tenant);
                    Quote entityPoco = quoteRepository.GetSingleQuoteByNumber(quoteNumber, tenant);
                    QuoteQuery quoteQuery = new QuoteQuery(quoteRepository);
                    if (entityPoco != null)
                    {
                        ObjectTableRepository objectTabelRepository = new ObjectTableRepository(tenant);
                        ObjectTable objectTable = objectTabelRepository.GetObjectTableByName("Quote", 0, true);
                        string objectTableId = objectTable.Id;

                        var quoteEvents = traceEventRepository.GetTraceEvents(tenant, entityPoco.Id, objectTableId);
                        List<TraceEventParams> toBuildEvents = new List<TraceEventParams>();
                        foreach (TraceEventPM traceEvent in eventsList)
                        {
                            bool exists = false;
                            if (!string.IsNullOrEmpty(traceEvent.ExternalId))
                            {
                                exists = quoteEvents.Where(e => e.ExternalId == traceEvent.ExternalId).Any();
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
                                    EventTypeCode = traceEvent.EventTypeCode,
                                    EventDate = traceEvent.EventDateTime,
                                    LogDate = traceEvent.LogDateTime,
                                    UserId = currentUserId,
                                    EntityId = entityPoco.Id,
                                    Notes = traceEvent.Notes,
                                    ObjectTableName = "Quote",

                                };

                                toBuildEvents.Add(parameters);
                            }
                        }

                        if (toBuildEvents.Count() > 0)
                        {
                            Logitude.Server.Tools.Response eventResponse = EventTracer.CreateTraceEventsList(toBuildEvents, tenant, "Quote", null);
                            if (!eventResponse.HasError)
                            {
                                //
                            }
                            else
                            {
                                response.HasError = true;
                                response.ErrorMessage = eventResponse.ErrorMessage;


                            }
                        }


                    }
                    else
                    {
                        response.HasError = true;
                        response.ErrorMessage = "Quote doesn't exist!";

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


        public Response DeleteQuoteEvent(string quoteNumber, string traceEventId, int tenant)
        {
            if (CacheManager.CacheWrapper == null)
            {
                CacheManager.CacheWrapper = new MockCacheWrapper();
            }

            Response response = new Response();
            try
            {
                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Quote", "UPDATE", tenant);//UPDATE//READ
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {

                    ICommonDataContext commoncontext = CommonDataContext.GetContext(tenant);
                    IQuotesContext quotesContext = QuotesContext.GetContext(tenant);
                    QuoteRepository quoteRepository = new QuoteRepository(quotesContext);
                    TraceEventRepository traceEventRepository = new TraceEventRepository(tenant);
                    QuoteQuery quoteQuery = new QuoteQuery(quoteRepository);
                    QuotePM entityPM = quoteQuery.GetSinglePMByQuoteNumber(quoteNumber, tenant);

                    if (entityPM != null)
                    {

                        TraceEvent traceEvent = traceEventRepository.GetSingleTraceEventByExternalId(traceEventId, tenant);
                        if (traceEvent != null)
                        {
                            QuoteTracing.DeleteQuoteTraceEvent(entityPM, traceEvent.Id, tenant, true);
                            response.Result = entityPM.StageId;
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
                        response.ErrorMessage = "Quote doesn't exist!";

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
    }
}
