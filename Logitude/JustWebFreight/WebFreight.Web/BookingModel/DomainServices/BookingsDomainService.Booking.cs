using Logitude.BookingLib.BL.EntityPMs;
using Logitude.BookingLib.BL.EntityQueryServices;
using Logitude.BookingLib.BL.EntityUpdateServices;
using Logitude.BookingLib.Data;
using Logitude.BookingLib.Data.EntityListQueryServices;
using Logitude.BookingLib.Data.EntityLists;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.DomainServices.Server;
using System.Web;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Logitude.Server.Tools.Helpers;
using Logitude.BookingLib.Data.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.Helpers;
using Logitude.BookingLib.Data.Repositories;
using Logitude.BookingLib.Data.EntityKeys;
using Logitude.BL.DataContracts;
using Logitude.BookingLib.BL.DataContracts;
using System.ComponentModel.DataAnnotations;
using System.Transactions;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using System.Text.RegularExpressions;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel;

namespace WebFreight.Web.BookingModel.DomainServices
{
    public partial class BookingsDomainService
    {
        private BookingQueryService bookingQueryService;

        public BookingPM GetSingleBookingPM(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Booking", "READ", tenant);

            if (objectContext == null)
            {
                objectContext = BookingContext.GetContext(tenant);
            }

            bookingQueryService = new BookingQueryService(objectContext);

            BookingPM entityPM = bookingQueryService.GetSingle(id, true, false);

            return entityPM;
        }

        public BookingList GetSingleBookingList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Booking", "READ", tenant);

            if (objectContext == null)
            {
                objectContext = BookingContext.GetContext(tenant);
            }

            BookingListQueryService listService = new BookingListQueryService(objectContext);
            BookingList list = listService.GetSingle(id);

            return list;
        }

        public List<BookingList> GetBookingLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Booking", "READ", tenant);

            if (objectContext == null)
            {
                objectContext = BookingContext.GetContext(tenant);
            }

            BookingListQueryService listService = new BookingListQueryService(objectContext);
            return listService.GetList(tenant);
        }

        public void UpdateBookingList(BookingList list)
        {

        }

        [System.ServiceModel.DomainServices.Server.Query(HasSideEffects = true)]
        public List<BookingList> GetBookingFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Booking", "READ", tenant);

            if (objectContext == null)
            {
                objectContext = BookingContext.GetContext(tenant);
            }

            BookingListQueryService listService = new BookingListQueryService(objectContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);

            List<BookingList> listQuery = listService.GetList(queryOperations, tenant);

            return listQuery;
        }

        public int GetBookingFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Booking", "READ", tenant);

            if (objectContext == null)
            {
                objectContext = BookingContext.GetContext(tenant);
            }

            BookingListQueryService queryService = new BookingListQueryService(objectContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations, tenant);
        }

        public void InsertBooking(BookingPM entityPM)
        {
            int tenant = entityPM.Tenant;
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Booking", "NEW", tenant);

            if (objectContext == null)
            {
                objectContext = BookingContext.GetContext(tenant);
            }

            DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant);
            entityPM.CreateDate = todayDate;
            entityPM.UpdateDate = todayDate;

            SetBookingPackagesChangeSet(entityPM);

            entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
            BookingUpdateService service = new BookingUpdateService(objectContext, new Dictionary<string, IContext>(), tenant);
            service.Update(entityPM, true);
        }

        public void UpdateBooking(BookingPM entityPM)
        {
            int tenant = entityPM.Tenant;
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Booking", "UPDATE", tenant);

            if (objectContext == null)
            {
                objectContext = BookingContext.GetContext(tenant);
            }

            DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant);
            entityPM.UpdateDate = todayDate;

            SetBookingPackagesChangeSet(entityPM);
            SetBookingAnswersChangeSet(entityPM);

            entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
            BookingUpdateService service = new BookingUpdateService(objectContext, new Dictionary<string, IContext>(), tenant);
            service.Update(entityPM, true);
        }

        private void SetBookingPackagesChangeSet(BookingPM entityPM)
        {
            List<BookingPackagePM> entityChangeSet = ChangeSet.GetAssociatedChanges(entityPM, d => d.BookingPackages).Cast<BookingPackagePM>().ToList();

            foreach (BookingPackagePM itemPM in entityChangeSet)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert:
                        {
                            BookingPackagePM currentItemPM = entityPM.BookingPackages.Where(d => d.BookingId == itemPM.BookingId && d.ChangeSetOp != ChangeSetOperation.Insert && d.Id == null).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Insert;
                            break;
                        }

                    case ChangeOperation.Update:
                        {
                            BookingPackagePM currentItemPM = entityPM.BookingPackages.Where(d => d.BookingId == itemPM.BookingId && d.Id == itemPM.Id).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Update;
                            break;
                        }

                    case ChangeOperation.Delete:
                        {
                            BookingPackagePM currentItemPM = new BookingPackagePM() { ChangeSetOp = ChangeSetOperation.Delete, BookingId = itemPM.BookingId, Id = itemPM.Id };
                            entityPM.DeletedBookingPackages.Add(currentItemPM);
                            break;
                        }

                    default:
                        {
                            BookingPackagePM currentItemPM = entityPM.BookingPackages.Where(d => d.BookingId == itemPM.BookingId && d.Id == itemPM.Id).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.None;
                            break;
                        }
                }
            }              
        }

        private void SetBookingAnswersChangeSet(BookingPM entityPM)
        {
            List<BookingAnswerPM> entityChangeSet = ChangeSet.GetAssociatedChanges(entityPM, d => d.BookingAnswers).Cast<BookingAnswerPM>().ToList();

            foreach (BookingAnswerPM itemPM in entityChangeSet)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert:
                        {
                            BookingAnswerPM currentItemPM = entityPM.BookingAnswers.Where(d => d.BookingId == itemPM.BookingId && d.ChangeSetOp != ChangeSetOperation.Insert && d.Id == null).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Insert;
                            break;
                        }

                    case ChangeOperation.Update:
                        {
                            BookingAnswerPM currentItemPM = entityPM.BookingAnswers.Where(d => d.BookingId == itemPM.BookingId && d.Id == itemPM.Id).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Update;
                            break;
                        }

                    case ChangeOperation.Delete:
                        {
                            BookingAnswerPM currentItemPM = new BookingAnswerPM() { ChangeSetOp = ChangeSetOperation.Delete, BookingId = itemPM.BookingId, Id = itemPM.Id };
                            entityPM.DeletedBookingAnswers.Add(currentItemPM);
                            break;
                        }

                    default:
                        {
                            BookingAnswerPM currentItemPM = entityPM.BookingAnswers.Where(d => d.BookingId == itemPM.BookingId && d.Id == itemPM.Id).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.None;
                            break;
                        }
                }
            } 
        }

        public List<EntityPartner> GetBookingEntityPartners(string entityId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Booking", "READ", tenant);

            if (objectContext == null)
            {
                objectContext = BookingContext.GetContext(tenant);
            }

            List<EntityPartner> list = new List<EntityPartner>();

            return list;
        }

        public List<BookingList> GetRecentActivityBookings(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Booking", "READ", tenant);

            if (objectContext == null)
            {
                objectContext = BookingContext.GetContext(tenant);
            }

            string mail = SecurityUtility.GetAuthenticatedUser();
            ContactQuery contactQuery = new ContactQuery(tenant);
            ContactPM contact = contactQuery.GetContactByEmailOnly(mail, tenant);

            ObjectTableRepository objectTabelRepository = new ObjectTableRepository(tenant);
            ObjectTable objectTable = objectTabelRepository.GetObjectTableByName("Booking", 0, true);

            BookingListQueryService queryService = new BookingListQueryService(objectContext);
            IQueryable<BookingList> myResult = queryService.GetRecentEntityLists(tenant, contact.Id, objectTable.Id).AsQueryable();

            return myResult.ToList();
        }

        [Invoke]
        public string ValidateBookingMasterFieldExistance(string entityId, string myMasterField, string myAirlinePrefixField, string myDirectionCode, string myTransportModeCode, bool isCancelled, int myTenant)
        {
            string myResult = null;

            if (myTenant != 343 && myTenant != 528)
            {
                if (!string.IsNullOrEmpty(myMasterField) && !string.IsNullOrEmpty(myAirlinePrefixField) && !isCancelled)
                {
                    BookingRepository myBookingRepository = new BookingRepository(myTenant);
                    bool isFieldExists = myBookingRepository.IsMasterFieldUsed(myMasterField, myAirlinePrefixField, entityId, myTenant, myDirectionCode, myTransportModeCode);
                    if (isFieldExists)
                    {
                        myResult = "Master field already used in another Booking";
                    }
                }
            }

            return myResult;
        }

        public List<BookingAnswerPM> GetBookingAnswerPMs(string myBookingId, int tenant)
        {
            List<BookingAnswerPM> myResult = new List<BookingAnswerPM>();
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Booking", "READ", tenant);

            if (objectContext == null)
            {
                objectContext = BookingContext.GetContext(tenant);
            }

            BookingKeys bookingKeys = new BookingKeys()
            {
                Id = myBookingId
            };

            BookingAnswerQueryService mBookingAnswerQueryService = new BookingAnswerQueryService(objectContext);
            myResult = mBookingAnswerQueryService.GetMulti(bookingKeys, true);

            return myResult;
        }

        public void UpdateChartingDataClass(ChartingDataClass entity)
        {

        }

        public List<ChartingDataClass> GetBookingsDashBoard(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Booking", "READ", tenant);

            objectContext = BookingContext.GetContext(tenant);
            BookingQueryService bookingQuery = new BookingQueryService(objectContext);

            List<BookingChartingClass> data = bookingQuery.GetBookingsDashBoard(tenant);

            List<ChartingDataClass> myResult = new List<ChartingDataClass>();

            foreach (BookingChartingClass item in data)
            {
                string myShortLabelProperty = "";
                if (item.StringProperty != null)
                {
                    string[] nameString = item.StringProperty.Split(' ');
                    myShortLabelProperty = nameString[0];
                }

                myResult.Add(new ChartingDataClass()
                {
                    Id = item.Id,
                    DataTypeCode = item.DataTypeCode,
                    StringProperty = item.StringProperty,
                    IntegerProperty = item.IntegerProperty,
                    OwnerId = item.MainCarriageCarrierId,
                    ShortLabelProperty = myShortLabelProperty,
                });
            }

            return myResult;
        }

        [Invoke]
        public bool CheckTotalFields(string bookingId, int tenant)
        {
            bool IsAnyTotalMissing = false;

            bookingRepository = new BookingRepository(tenant);
            Booking booking = bookingRepository.GetSingle(bookingId, tenant);

            if (booking != null)
            {
                if (booking.NumberOfPackages == 0 || booking.Volume == 0 || booking.VolumetricWeight == 0)
                {
                    IsAnyTotalMissing = true;
                }
            }

            return IsAnyTotalMissing;
        }

        #region Validation
        private Booking myEntityPOCO;
        private BookingValidatorResultClass myResultClass;
        public BookingValidatorResultClass ValidateBookingForSending(string bookingId, int tenant, bool isCancellationSent)
        {
            myResultClass = new BookingValidatorResultClass()
            {
                Id = tenant,
                Tenant = tenant,
                BookingId = bookingId,
                IsValid = true,
                IsDemoTenant = false,
                IsAWBStockPrepaid = false,
                StockCode = "FFR",
            };

            this.GetGlobalVariables();
            this.GetBookingObject();

            if (!isCancellationSent)
            {
                this.CheckStockValidity();
            }

            this.Validate_BKD();
            this.Validate_PAC();

            if (!myResultClass.IsEAWBOnlyDemo)
            {
                if (myResultClass.AWBMessagesCCSTypeCode == "GLSHK")
                {
                    if (string.IsNullOrEmpty(myResultClass.PIMA))
                    {
                        myResultClass.IsValid = false;
                        myResultClass.HasMainErrors = true;
                        myResultClass.ErrorsList.Add("PIMA field is required");
                    }
                }

                else
                {
                    if (string.IsNullOrEmpty(myResultClass.TTY))
                    {
                        myResultClass.IsValid = false;
                        myResultClass.HasMainErrors = true;
                        myResultClass.ErrorsList.Add("TTY field is required");
                    }
                }
            }

            return myResultClass;
        }
        private void GetGlobalVariables()
        {
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                TenantManagementRepository tenantManagementRepository = new TenantManagementRepository();
                TenantManagement tenantManagement = tenantManagementRepository.GetSingleTenantManagement(myResultClass.Tenant);

                if (tenantManagement != null)
                {
                    myResultClass.TTY = tenantManagement.TTY;
                    myResultClass.PIMA = tenantManagement.PIMA;
                    myResultClass.AWBMessagesCCSTypeCode = tenantManagement.AWBMessagesCCSTypeCode;
                    myResultClass.IsEAWBOnlyDemo = tenantManagement.IsEAWBOnlyDemo;
                    myResultClass.IsAWBStockPrepaid = tenantManagement.IsAWBStockPrepaid;
                }

                if (myResultClass.Tenant == 65 || myResultClass.IsEAWBOnlyDemo)
                {
                    myResultClass.IsDemoTenant = true;
                }

                scope.Complete();
            }
        }
        private void GetBookingObject()
        {
            if (objectContext == null)
            {
                objectContext = BookingContext.GetContext(myResultClass.Tenant);
            }

            bookingRepository = new BookingRepository(objectContext);
            myEntityPOCO = bookingRepository.GetSingle(myResultClass.BookingId, myResultClass.Tenant);
        }        
        private void CheckStockValidity()
        {
            if (!myResultClass.IsDemoTenant)
            {
                if (myResultClass.IsAWBStockPrepaid)
                {
                    IShipmentsContext myShipmentContext = ShipmentsContext.GetContext(myResultClass.Tenant);
                    DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(myResultClass.Tenant).Date;
                    MessagingStockRepository stockRepository = new MessagingStockRepository(myShipmentContext);
                    MessagingStockUsageHistoryRepository usageHistoryRepository = new MessagingStockUsageHistoryRepository(myShipmentContext);
                    IQueryable<MessagingStock> myStocksData = stockRepository.GetMessagingStocksByTenant(myResultClass.Tenant, "Champ");
                    IQueryable<MessagingStockUsageHistory> myUsageHistoryData = usageHistoryRepository.GetTenantMessagingStockUsageHistory(myResultClass.Tenant);

                    myStocksData = myStocksData.Where(d => d.StartDate <= todayDate && d.EndDate > todayDate && d.Remaining > 0 && !d.IsCancelled);

                    int sendingCount = 0;
                    int? myStocksRemaining = 0;
                    if (myStocksData.Count() > 0)
                    {
                        myStocksRemaining = myStocksData.Sum(s => s.Remaining);
                    }

                    if (!myUsageHistoryData.Where(d => d.EntityId == myResultClass.BookingId && d.MessageType == myResultClass.StockCode).Any())
                    {
                        sendingCount = 1;
                    }

                    myResultClass.SendingCount = sendingCount;
                    myResultClass.StockRemainingBefore = myStocksRemaining == null ? 0 : myStocksRemaining.Value;

                    if (sendingCount > 0)
                    {
                        if (myStocksRemaining < sendingCount)
                        {
                            myResultClass.IsValid = false;
                            myResultClass.HasStockErrors = true;
                        }
                    }
                }
            }
        }


        private void Validate_BKD()
        {
            if (myEntityPOCO != null)
            {
                if (string.IsNullOrEmpty(myEntityPOCO.Master))
                {
                    myResultClass.IsValid = false;
                    myResultClass.ErrorsList.Add("Master field is required");
                }

                if (string.IsNullOrEmpty(myEntityPOCO.MainCarriageFromPortId))
                {
                    myResultClass.IsValid = false;
                    myResultClass.ErrorsList.Add("Departure field is required");
                }

                if (string.IsNullOrEmpty(myEntityPOCO.MainCarriageFinalDestinationPortId))
                {
                    myResultClass.IsValid = false;
                    myResultClass.ErrorsList.Add("Destination field is required");
                }

                #region Main Carriage
                if (string.IsNullOrEmpty(myEntityPOCO.MainCarriageCarrierPrefix))
                {
                    myResultClass.IsValid = false;
                    myResultClass.ErrorsList.Add("Main Carriage Carrier Prefix field is required");
                }
                else if (myEntityPOCO.MainCarriageCarrierPrefix.Length != 2)
                {
                    myResultClass.IsValid = false;
                    myResultClass.ErrorsList.Add("Main Carriage Carrier Prefix length must be 2");
                }

                if (string.IsNullOrEmpty(myEntityPOCO.MainCarriageCarrierNumber))
                {
                    myResultClass.IsValid = false;
                    myResultClass.ErrorsList.Add("Flight Number field is required");
                }
                else
                {
                    if (!this.ValidateFlightNumberFormat(myEntityPOCO.MainCarriageCarrierNumber))
                    {
                        myResultClass.IsValid = false;
                        myResultClass.ErrorsList.Add("Flight No. wrong format: must be [3-4 numerics] Or [4 numerics plus 1 Alpha]");
                    }
                }

                if (myEntityPOCO.MainCarriageETD == null)
                {
                    myResultClass.IsValid = false;
                    myResultClass.ErrorsList.Add("Main Carriage ETD field is required");
                }

                if (string.IsNullOrEmpty(myEntityPOCO.MainCarriageSpaceAllocationCode))
                {
                     myResultClass.IsValid = false;
                     myResultClass.ErrorsList.Add("Main Carriage Space Allocation field is required");
                }
                else
                {
                    if (myEntityPOCO.MainCarriageSpaceAllocationCode == "CA")
                    {
                        if (string.IsNullOrEmpty(myEntityPOCO.MainCarriageAllotmentIdentification))
                        {
                            myResultClass.IsValid = false;
                            myResultClass.ErrorsList.Add("Main Carriage Allotment Identification field is required");
                        }
                    }
                }
                #endregion

                #region Transshipment1
                if (!string.IsNullOrEmpty(myEntityPOCO.Transshipment1FromPortId))
                {
                    if (string.IsNullOrEmpty(myEntityPOCO.Transshipment1CarrierId))
                    {
                        myResultClass.IsValid = false;
                        myResultClass.ErrorsList.Add("Transshipment1 Airline field is required");
                    }

                    if (string.IsNullOrEmpty(myEntityPOCO.Transshipment1CarrierPrefix))
                    {
                        myResultClass.IsValid = false;
                        myResultClass.ErrorsList.Add("Transshipment1 Carrier Prefix field is required");
                    }

                    else if (myEntityPOCO.Transshipment1CarrierPrefix.Length != 2)
                    {
                        myResultClass.IsValid = false;
                        myResultClass.ErrorsList.Add("Transshipment1 Carrier Prefix length must be 2");
                    }

                    if (string.IsNullOrEmpty(myEntityPOCO.Transshipment1CarrierNumber))
                    {
                        myResultClass.IsValid = false;
                        myResultClass.ErrorsList.Add("Transshipment1 Flight No. field is required");
                    }
                    else
                    {
                        if (!this.ValidateFlightNumberFormat(myEntityPOCO.Transshipment1CarrierNumber))
                        {
                            myResultClass.IsValid = false;
                            myResultClass.ErrorsList.Add("Transshipment1 Flight No. wrong format: must be [3-4 numerics] Or [4 numerics plus 1 Alpha]");
                        }
                    }

                    if (myEntityPOCO.Transshipment1ETD == null)
                    {
                        myResultClass.IsValid = false;
                        myResultClass.ErrorsList.Add("Transshipment1 ETD field is required");
                    }

                    if (string.IsNullOrEmpty(myEntityPOCO.Transshipment1SpaceAllocationCode))
                    {
                        myResultClass.IsValid = false;
                        myResultClass.ErrorsList.Add("Transshipment1 Space Allocation field is required");
                    }
                    else
                    {
                        if (myEntityPOCO.Transshipment1SpaceAllocationCode == "CA")
                        {
                            if (string.IsNullOrEmpty(myEntityPOCO.Transshipment1AllotmentIdentification))
                            {
                                myResultClass.IsValid = false;
                                myResultClass.ErrorsList.Add("Transshipment1 Allotment Identification field is required");
                            }
                        }
                    }
                }
                #endregion

                #region Transshipment2
                if (!string.IsNullOrEmpty(myEntityPOCO.Transshipment2FromPortId))
                {
                    if (string.IsNullOrEmpty(myEntityPOCO.Transshipment2CarrierId))
                    {
                        myResultClass.IsValid = false;
                        myResultClass.ErrorsList.Add("Transshipment2 Airline field is required");
                    }

                    if (string.IsNullOrEmpty(myEntityPOCO.Transshipment2CarrierPrefix))
                    {
                        myResultClass.IsValid = false;
                        myResultClass.ErrorsList.Add("Transshipment2 Carrier Prefix field is required");
                    }

                    else if (myEntityPOCO.Transshipment2CarrierPrefix.Length != 2)
                    {
                        myResultClass.IsValid = false;
                        myResultClass.ErrorsList.Add("Transshipment2 Carrier Prefix length must be 2");
                    }

                    if (string.IsNullOrEmpty(myEntityPOCO.Transshipment2CarrierNumber))
                    {
                        myResultClass.IsValid = false;
                        myResultClass.ErrorsList.Add("Transshipment2 Flight No. field is required");
                    }
                    else
                    {
                        if (!this.ValidateFlightNumberFormat(myEntityPOCO.Transshipment2CarrierNumber))
                        {
                            myResultClass.IsValid = false;
                            myResultClass.ErrorsList.Add("Transshipment2 Flight No. wrong format: must be [3-4 numerics] Or [4 numerics plus 1 Alpha]");
                        }
                    }

                    if (myEntityPOCO.Transshipment2ETD == null)
                    {
                        myResultClass.IsValid = false;
                        myResultClass.ErrorsList.Add("Transshipment2 ETD field is required");
                    }

                    if (string.IsNullOrEmpty(myEntityPOCO.Transshipment2SpaceAllocationCode))
                    {
                        myResultClass.IsValid = false;
                        myResultClass.ErrorsList.Add("Transshipment2 Space Allocation field is required");
                    }
                    else
                    {
                        if (myEntityPOCO.Transshipment2SpaceAllocationCode == "CA")
                        {
                            if (string.IsNullOrEmpty(myEntityPOCO.Transshipment2AllotmentIdentification))
                            {
                                myResultClass.IsValid = false;
                                myResultClass.ErrorsList.Add("Transshipment2 Allotment Identification field is required");
                            }
                        }
                    }
                }
                #endregion
            }
        }
        private void Validate_PAC()
        {
            if (myEntityPOCO.GrossWeight == null || myEntityPOCO.GrossWeight == 0)
            {
                myResultClass.IsValid = false;
                myResultClass.ErrorsList.Add("Gross Weight cannot be zero");
            }

            if (myEntityPOCO.ChargeableWeight == null || myEntityPOCO.ChargeableWeight == 0)
            {
                myResultClass.IsValid = false;
                myResultClass.ErrorsList.Add("Chargeable Weight cannot be zero");
            }

            if (myEntityPOCO.NumberOfPackages == null || myEntityPOCO.NumberOfPackages == 0)
            {
                myResultClass.IsValid = false;
                myResultClass.ErrorsList.Add("Number of Packages cannot be zero");
            }

            if (myEntityPOCO.Volume == null || myEntityPOCO.Volume == 0)
            {
                myResultClass.IsValid = false;
                myResultClass.ErrorsList.Add("Volume cannot be zero");
            }

            if (myEntityPOCO.VolumetricWeight == null || myEntityPOCO.VolumetricWeight == 0)
            {
                myResultClass.IsValid = false;
                myResultClass.ErrorsList.Add("Volumetric Weight cannot be zero");
            }
        }
        private bool ValidateFlightNumberFormat(string input)
        {
            bool result = false;

            if (!string.IsNullOrEmpty(input))
            {
                Regex isMatch1 = new Regex("^[0-9]{3,4}$");
                Regex isMatch2 = new Regex("^[0-9]{4}[A-Z]{1}$");

                if (isMatch1.IsMatch(input))
                {
                    result = true;
                }

                else if (isMatch2.IsMatch(input.ToUpper()))
                {
                    result = true;
                }
            }

            return result;
        }
        #endregion
    }

    public class BookingValidatorResultClass
    {
        [Key]
        public int Id { get; set; }
        public int Tenant { get; set; }
        public string TTY { get; set; }
        public string PIMA { get; set; }
        public string BookingId { get; set; }
        public bool IsDemoTenant { get; set; }
        public bool IsValid { get; set; }
        public bool HasMainErrors { get; set; }
        public bool IsEAWBOnlyDemo { get; set; }
        public string AWBMessagesCCSTypeCode { get; set; }
        public List<string> ErrorsList { get; set; }

        public string StockCode { get; set; }
        public int SendingCount { get; set; }
        public bool IsAWBStockPrepaid { get; set; }
        public int StockRemainingBefore { get; set; }
        public int StockRemainingAfter { get; set; }
        public bool HasStockErrors { get; set; }
        public BookingValidatorResultClass()
        {
            this.ErrorsList = new List<string>();
        }
    }
}