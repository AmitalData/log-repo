using System;
using System.Collections.Generic;

using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using System.Linq;
using System.Data.Entity;
using Simplog.Server.Infrastructure.Helpers;

//using WebFreight.Web.QuoteModel.EntityPOCOs;


namespace Simplog.Data.InfrastructureModel
{
    public class MockWebFreightContext : IWebFreightContext
    {

        public MockObjectSet<Port> PortsList;
        public MockObjectSet<Country> CountriesList;
        public MockObjectSet<GlobalZone> GlobalZonesList;
        private MockObjectSet<User> usersList;
        //private MockObjectSet<Contact> contactsList;
        //private MockObjectSet<ContactTenant> contactTenantsList;
        //private MockObjectSet<Tenant> tenantList;
        //private MockObjectSet<ContactTenantRole> contactTenantRolesList;
        //private MockObjectSet<Role> rolesList;
        private MockObjectSet<TransportMode> transportModeList;
        private MockObjectSet<EntityStatus> entityStatusList;
        private MockObjectSet<FollowUp> followUpList;
        private MockObjectSet<Counter> counterList;
        //private MockObjectSet<EventType> eventTypeList;
        //private MockObjectSet<ObjectTable> objectTableList;

        #region IWebFreightContext Members

        public IDbSet<Port> Ports
        {
            get
            {
                PortsList = new MockObjectSet<Port>();
                Port p1 = new Port() { Code = "LY", Country = new Country() { Code = "IL", EnglishName = "Israel", GlobalZone = new GlobalZone() { Code = "ME", EnglishName = "MiddleEast", Id = "1-ME", InActive = false, LocalName = "Midddd", Notes = "nothing", Tenant = 1 }, AddedManually = false, Id = "1-IL", InActive = false, LocalName = "iiiii", Notes = "wala eshi", Tenant = 1 }, EnglishName = "El-Al", Id = "1-LY", InActive = false, IsAir = true, IsInland = false, IsOcean = false, LocalName = "elal", Notes = "nonono", Tenant = 1 };
                Port p2 = new Port() { Code = "JFK", Country = new Country() { Code = "US", EnglishName = "USA", GlobalZone = new GlobalZone() { Code = "NA", EnglishName = "NorthAmirica", Id = "1-NA", InActive = false, LocalName = "aaaaa", Notes = "adsdf", Tenant = 1 }, AddedManually = false, Id = "1-US", InActive = false, LocalName = "iiiii", Notes = "wala eshi", Tenant = 1 }, EnglishName = "JFKK", Id = "1-jfk", InActive = false, IsAir = true, IsInland = false, IsOcean = false, LocalName = "edasdlal", Notes = "noasdnono", Tenant = 1 };

                PortsList.AddObject(p1);
                PortsList.AddObject(p2);
                return PortsList;

            }
        }

        public IDbSet<Country> Countries
        {
            get { throw new NotImplementedException(); }
        }

        public IDbSet<GlobalZone> GlobalZones
        {
            get { throw new NotImplementedException(); }
        }

        public void DetectChanges()
        {

        }

        public int SaveChanges()
        {
            return 1;
        }

        #endregion

        #region IWebFreightContext Members


        public void SetAsModified(object entity)
        {

            //throw new NotImplementedException();
        }

        #endregion

        #region IWebFreightContext Members


        public IDbSet<Customer> Clients
        {
            get { throw new NotImplementedException(); }
        }

        public IDbSet<Address> Addresses
        {
            get { throw new NotImplementedException(); }
        }

        #endregion


        public IDbSet<User> Users
        {
            get
            {
                usersList = new MockObjectSet<User>();
                User user1 = new User() { Id = "111", BranchId = "1", DepartmentId = "2", Tenant = 1, Notes = "user1 remarks" };
                User user2 = new User() { Id = "222", BranchId = "5", DepartmentId = "4", Tenant = 3, Notes = "user2 remarks" };

                usersList.AddObject(user1);
                usersList.AddObject(user2);
                return usersList;
            }
        }

        public IDbSet<Contact> Contacts
        {
            get { throw new NotImplementedException(); }
        }

        public IDbSet<ContactTenant> ContactTenants
        {
            get { throw new NotImplementedException(); }
        }

        public IDbSet<Tenant> Tenants
        {
            get { throw new NotImplementedException(); }
        }

        public IDbSet<ContactTenantRole> ContactTenantRoles
        {
            get { throw new NotImplementedException(); }
        }

        public IDbSet<Role> Roles
        {
            get { throw new NotImplementedException(); }
        }

        #region IWebFreightContext Members


        public IDbSet<Department> Department
        {
            get { throw new NotImplementedException(); }
        }

        public IDbSet<Branch> Branch
        {
            get { throw new NotImplementedException(); }
        }

        #endregion


        public IDbSet<Department> Departments
        {
            get { throw new NotImplementedException(); }
        }

        public IDbSet<Branch> Branches
        {
            get { throw new NotImplementedException(); }
        }

        public IDbSet<Card> Cards
        {
            get { throw new NotImplementedException(); }
        }

        public IDbSet<ShippingAgent> ShippingAgents
        {
            get { throw new NotImplementedException(); }
        }

        public IDbSet<CustomAgent> CustomAgents
        {
            get { throw new NotImplementedException(); }
        }

        //public IDbSet<ClientAbroad> ClientAbroads
        //{
        //    get { throw new NotImplementedException(); }
        //}

        #region IWebFreightContext Members




        public IDbSet<Trucker> Truckers
        {
            get { throw new NotImplementedException(); }
        }

        public IDbSet<Airline> Airlines
        {
            get { throw new NotImplementedException(); }
        }

        public IDbSet<ShippingLine> ShippingLines
        {
            get { throw new NotImplementedException(); }
        }

        #endregion


        public IDbSet<Agent> Agents
        {
            get { throw new NotImplementedException(); }
        }

        #region IWebFreightContext Members


        public IDbSet<PaymentTerm> PaymentTerms
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        #region IWebFreightContext Members


        public IDbSet<Incoterm> Incoterms
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        #region IWebFreightContext Members


        public IDbSet<AddressType> AddressTypes
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        #region IWebFreightContext Members

        private MockObjectSet<Currency> currencyObjectSet;
        public IDbSet<Currency> Currencies
        {
            get
            {
                currencyObjectSet = new MockObjectSet<Currency>();
                Currency currency1 = new Currency() { Id = "1-1", Tenant=1 , Code="EUR"};
                Currency currency2 = new Currency() { Id = "1-2", Tenant=1 , Code="YEN" };

                currencyObjectSet.AddObject(currency1);
                currencyObjectSet.AddObject(currency2);
                return currencyObjectSet;
            }
        }

        public IDbSet<State> States
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        #region IWebFreightContext Members


        public IDbSet<CardContact> CardContacts
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        #region IWebFreightContext Members


        public IDbSet<CounterLastNumber> CounterLastNumbers
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        #region IWebFreightContext Members


        public IDbSet<Shipment> Shipments
        {
            get { throw new NotImplementedException(); }
        }

        public IDbSet<ShipmentType> ShipmentTypes
        {
            get { throw new NotImplementedException(); }
        }

        #endregion



        #region IWebFreightContext Members


        public IDbSet<TransportMode> TransportModes
        {
            get
            {
                transportModeList = new MockObjectSet<TransportMode>();
                TransportMode transportMode1 = new TransportMode() { Id = "111", Name = "TM1" };
                TransportMode transportMode2 = new TransportMode() { Id = "222", Name = "TM2" };

                transportModeList.AddObject(transportMode1);
                transportModeList.AddObject(transportMode2);
                return transportModeList;
            }
        }

        private MockObjectSet<Direction> directionsList;
        public IDbSet<Direction> Directions
        {
            get
            {
                directionsList = new MockObjectSet<Direction>();
                Direction direction1 = new Direction() { Id = "111", Name = "dd"};
                Direction direction2 = new Direction() { Id = "222", Name = "DD" };

                directionsList.AddObject(direction1);
                directionsList.AddObject(direction2);
                return directionsList;
            }
        }
        public IDbSet<PartnerType> PartnerTypes
        {
            get { throw new NotImplementedException(); }
        }


        private MockObjectSet<PrepaidCollect> prepaidCollectList;
        public IDbSet<PrepaidCollect> PrepaidCollects
        {
            get
            {
                prepaidCollectList = new MockObjectSet<PrepaidCollect>();
                PrepaidCollect prepaidCollect1 = new PrepaidCollect() { Id = "1-1"};
                PrepaidCollect prepaidCollect2 = new PrepaidCollect() { Id = "1-2", Name = "DD" };

                prepaidCollectList.AddObject(prepaidCollect1);
                prepaidCollectList.AddObject(prepaidCollect2);
                return prepaidCollectList;
            }
        }

        #endregion

        #region IWebFreightContext Members




        #endregion


        public IDbSet<MoveType> MoveTypes
        {
            get { throw new NotImplementedException(); }
        }



        public IDbSet<SpecialService> SpecialServices
        {
            get { throw new NotImplementedException(); }
        }


        public IDbSet<EmailAlertSetting> EmailAlertSettings
        {
            get { throw new NotImplementedException(); }
        }

        

        public IDbSet<FollowUp> FollowUps
        {
            get
            {
                followUpList = new MockObjectSet<FollowUp>();

                return followUpList;
            }
        }

        //public IDbSet<FollowUpType> FollowUpTypes
        //{
        //    get { throw new NotImplementedException(); }
        //}

        //public IDbSet<EntityType> EntityTypes
        //{
        //    get { throw new NotImplementedException(); }
        //}


        public IDbSet<EntityDate> EntityDates
        {
            get { throw new NotImplementedException(); }
        }

        public IDbSet<DocumentType> DocumentTypes
        {
            get { throw new NotImplementedException(); }
        }

        public IDbSet<DocumentOut> InternalDocuments
        {
            get { throw new NotImplementedException(); }
        }

        //public IDbSet<DocumentIn> ExternalDocuments
        //{
        //    get { throw new NotImplementedException(); }
        //}


        public IDbSet<ValidationType> ValidationTypes
        {
            get { throw new NotImplementedException(); }
        }


        //public IDbSet<CustomField> CustomFields
        //{
        //    get { throw new NotImplementedException(); }
        //}


        public IDbSet<FieldDataType> FieldDataTypes
        {
            get { throw new NotImplementedException(); }
        }

        public IDbSet<Translation> Translations
        {
            get { return new MockObjectSet<Translation>(new List<Translation>()); }
        }

        public IDbSet<TranslationHeader> TranslationHeaders
        {
            get { return new MockObjectSet<TranslationHeader>(new List<TranslationHeader>()); }
        }

        List<TextCode> textCodes;
        MockObjectSet<TextCode> textCodesObjectSet;
        public IDbSet<TextCode> TextCodes
        {
            get
            {
                if (textCodes == null)
                {
                    textCodes = new List<TextCode>(){
                   new TextCode() { Id = "1-1", Tenant = 1, Code = "TC" }};
                    textCodesObjectSet = new MockObjectSet<TextCode>(textCodes);
                }
               
                return textCodesObjectSet;
            }
        }

        List<ObjectTable> objectTables;
        MockObjectSet<ObjectTable> objectTablesObjectSet;
        public IDbSet<ObjectTable> ObjectTables
        {
            get
            {
                if (objectTables == null)
                {
                    objectTables = new List<ObjectTable>()
                    {
                     new ObjectTable()
                    {
                        Id = "1-1",
                        Name = "OT",
                        Tenant = 1,
                        NewWizardControlName = "new Wizard",
                        
                        HeaderScreenId = "1-1",
                        HeaderScreen = Screens.Where(d => d.Id == "1-1").FirstOrDefault(),
                        DescriptionTextCode = TextCodes.Where(d => d.Id == "1-1").FirstOrDefault(),
                        DescriptionTextCodeId = "1-1",
                        IsComposition = false,
                        ObjectTableTypeCode = "OT",
                        EnableSecurity = false,
                        MainTipCode = "MT",
                        IsSaveButtonVisible = false,
                        CustomFieldsCount = 2,
                        HasCustomFields = true,
                        SortingByObjectField = "nothing",
                        IsAutoComplete = false,
                        IsMain = true,
                        IsRestrictable = false,
                    },
                     new ObjectTable()
                    {
                        Id = "1-5",
                        Name = "Trucker",
                        Tenant = 1,
                        NewWizardControlName = "new Wizard",
                        HeaderScreenId = "1-1",
                        HeaderScreen = Screens.Where(d => d.Id == "1-1").FirstOrDefault(),
                        DescriptionTextCode = TextCodes.Where(d => d.Id == "1-1").FirstOrDefault(),
                        DescriptionTextCodeId = "1-1",
                        IsComposition = false,
                        ObjectTableTypeCode = "OT",
                        EnableSecurity = false,
                        MainTipCode = "MT",
                        IsSaveButtonVisible = false,
                        CustomFieldsCount = 2,
                        HasCustomFields = true,
                        SortingByObjectField = "nothing",
                        IsAutoComplete = false,
                        IsMain = true,
                        IsRestrictable = false,
                    },
                    new ObjectTable()
                     {
                        Id = "1-3",
                        Name = "Vendor",
                        Tenant = 1,
                        NewWizardControlName = "new Wizard",
                        HeaderScreenId = "1-1",
                        HeaderScreen = Screens.Where(d => d.Id == "1-1").FirstOrDefault(),
                        DescriptionTextCode = TextCodes.Where(d => d.Id == "1-1").FirstOrDefault(),
                        DescriptionTextCodeId = "1-1",
                        IsComposition = false,
                        ObjectTableTypeCode = "OT",
                        EnableSecurity = false,
                        MainTipCode = "MT",
                        IsSaveButtonVisible = false,
                        CustomFieldsCount = 2,
                        HasCustomFields = true,
                        SortingByObjectField = "nothing",
                        IsAutoComplete = false,
                        IsMain = true,
                        IsRestrictable = false,
                    },
                     new ObjectTable()
                    {
                        Id = "1-2",
                        Name = "EventType",
                        Tenant = 1,
                        NewWizardControlName = "new Wizard",
                        HeaderScreenId = "1-1",
                        HeaderScreen = Screens.Where(d => d.Id == "1-1").FirstOrDefault(),
                        DescriptionTextCode = TextCodes.Where(d => d.Id == "1-1").FirstOrDefault(),
                        DescriptionTextCodeId = "1-1",
                        IsComposition = false,
                        ObjectTableTypeCode = "OT",
                        EnableSecurity = false,
                        MainTipCode = "MT",
                        IsSaveButtonVisible = false,
                        CustomFieldsCount = 2,
                        HasCustomFields = true,
                        SortingByObjectField = "nothing",
                        IsAutoComplete = false,
                        IsMain = true,
                        IsRestrictable = false,
                    },
                     new ObjectTable()
                    {
                        Id = "1-4",
                        Name = "Warehouse",
                        Tenant = 1,
                        NewWizardControlName = "new Wizard",
                        HeaderScreenId = "1-1",
                        HeaderScreen = Screens.Where(d => d.Id == "1-1").FirstOrDefault(),
                        DescriptionTextCode = TextCodes.Where(d => d.Id == "1-1").FirstOrDefault(),
                        DescriptionTextCodeId = "1-1",
                        IsComposition = false,
                        ObjectTableTypeCode = "OT",
                        EnableSecurity = false,
                        MainTipCode = "MT",
                        IsSaveButtonVisible = false,
                        CustomFieldsCount = 2,
                        HasCustomFields = true,
                        SortingByObjectField = "nothing",
                        IsAutoComplete = false,
                        IsMain = true,
                        IsRestrictable = false,
                    },
                    new ObjectTable()
                    {
                        Id = "1-6",
                        Name = "VatType",
                        Tenant = 1,
                        NewWizardControlName = "new Wizard",
                        
                        HeaderScreenId = "1-1",
                        HeaderScreen = Screens.Where(d => d.Id == "1-1").FirstOrDefault(),
                        DescriptionTextCode = TextCodes.Where(d => d.Id == "1-1").FirstOrDefault(),
                        DescriptionTextCodeId = "1-1",
                        IsComposition = false,
                        ObjectTableTypeCode = "OT",
                        EnableSecurity = false,
                        MainTipCode = "MT",
                        IsSaveButtonVisible = false,
                        CustomFieldsCount = 2,
                        HasCustomFields = true,
                        SortingByObjectField = "nothing",
                        IsAutoComplete = false,
                        IsMain = true,
                        IsRestrictable = false,
                    },
                    new ObjectTable()
                    {
                        Id = "1-7",
                        Name = "State",
                        Tenant = 1,
                        NewWizardControlName = "new Wizard",
                        
                        HeaderScreenId = "1-1",
                        HeaderScreen = Screens.Where(d => d.Id == "1-1").FirstOrDefault(),
                        DescriptionTextCode = TextCodes.Where(d => d.Id == "1-1").FirstOrDefault(),
                        DescriptionTextCodeId = "1-1",
                        IsComposition = false,
                        ObjectTableTypeCode = "OT",
                        EnableSecurity = false,
                        MainTipCode = "MT",
                        IsSaveButtonVisible = false,
                        CustomFieldsCount = 2,
                        HasCustomFields = true,
                        SortingByObjectField = "nothing",
                        IsAutoComplete = false,
                        IsMain = true,
                        IsRestrictable = false,
                    },
                     new ObjectTable()
                    {
                        Id = "1-8",
                        Name = "ShippingLine",
                        Tenant = 1,
                        NewWizardControlName = "new Wizard",
                        
                        HeaderScreenId = "1-1",
                        HeaderScreen = Screens.Where(d => d.Id == "1-1").FirstOrDefault(),
                        DescriptionTextCode = TextCodes.Where(d => d.Id == "1-1").FirstOrDefault(),
                        DescriptionTextCodeId = "1-1",
                        IsComposition = false,
                        ObjectTableTypeCode = "OT",
                        EnableSecurity = false,
                        MainTipCode = "MT",
                        IsSaveButtonVisible = false,
                        CustomFieldsCount = 2,
                        HasCustomFields = true,
                        SortingByObjectField = "nothing",
                        IsAutoComplete = false,
                        IsMain = true,
                        IsRestrictable = false,
                    },
                     new ObjectTable()
                    {
                        Id = "1-9",
                        Name = "ShippingAgent",
                        Tenant = 1,
                        NewWizardControlName = "new Wizard",
                        
                        HeaderScreenId = "1-1",
                        HeaderScreen = Screens.Where(d => d.Id == "1-1").FirstOrDefault(),
                        DescriptionTextCode = TextCodes.Where(d => d.Id == "1-1").FirstOrDefault(),
                        DescriptionTextCodeId = "1-1",
                        IsComposition = false,
                        ObjectTableTypeCode = "OT",
                        EnableSecurity = false,
                        MainTipCode = "MT",
                        IsSaveButtonVisible = false,
                        CustomFieldsCount = 2,
                        HasCustomFields = true,
                        SortingByObjectField = "nothing",
                        IsAutoComplete = false,
                        IsMain = true,
                        IsRestrictable = false,
                    },
                      new ObjectTable()
                    {
                        Id = "1-10",
                        Name = "PotentialCustomer",
                        Tenant = 1,
                        NewWizardControlName = "new Wizard",
                        
                        HeaderScreenId = "1-1",
                        HeaderScreen = Screens.Where(d => d.Id == "1-1").FirstOrDefault(),
                        DescriptionTextCode = TextCodes.Where(d => d.Id == "1-1").FirstOrDefault(),
                        DescriptionTextCodeId = "1-1",
                        IsComposition = false,
                        ObjectTableTypeCode = "OT",
                        EnableSecurity = false,
                        MainTipCode = "MT",
                        IsSaveButtonVisible = false,
                        CustomFieldsCount = 2,
                        HasCustomFields = true,
                        SortingByObjectField = "nothing",
                        IsAutoComplete = false,
                        IsMain = true,
                        IsRestrictable = false,
                    },
                      new ObjectTable()
                    {
                        Id = "1-11",
                        Name = "Port",
                        Tenant = 1,
                        NewWizardControlName = "new Wizard",
                        
                        HeaderScreenId = "1-1",
                        HeaderScreen = Screens.Where(d => d.Id == "1-1").FirstOrDefault(),
                        DescriptionTextCode = TextCodes.Where(d => d.Id == "1-1").FirstOrDefault(),
                        DescriptionTextCodeId = "1-1",
                        IsComposition = false,
                        ObjectTableTypeCode = "OT",
                        EnableSecurity = false,
                        MainTipCode = "MT",
                        IsSaveButtonVisible = false,
                        CustomFieldsCount = 2,
                        HasCustomFields = true,
                        SortingByObjectField = "nothing",
                        IsAutoComplete = false,
                        IsMain = true,
                        IsRestrictable = false,
                    },
                     new ObjectTable()
                    {
                        Id = "1-12",
                        Name = "PackageType",
                        Tenant = 1,
                        NewWizardControlName = "new Wizard",
                        HeaderScreenId = "1-1",
                        HeaderScreen = Screens.Where(d => d.Id == "1-1").FirstOrDefault(),
                        DescriptionTextCode = TextCodes.Where(d => d.Id == "1-1").FirstOrDefault(),
                        DescriptionTextCodeId = "1-1",
                        IsComposition = false,
                        ObjectTableTypeCode = "OT",
                        EnableSecurity = false,
                        MainTipCode = "MT",
                        IsSaveButtonVisible = false,
                        CustomFieldsCount = 2,
                        HasCustomFields = true,
                        SortingByObjectField = "nothing",
                        IsAutoComplete = false,
                        IsMain = true,
                        IsRestrictable = false,
                    },
                     new ObjectTable()
                    {
                        Id = "1-13",
                        Name = "Incoterm",
                        Tenant = 1,
                        NewWizardControlName = "new Wizard",
                        HeaderScreenId = "1-1",
                        HeaderScreen = Screens.Where(d => d.Id == "1-1").FirstOrDefault(),
                        DescriptionTextCode = TextCodes.Where(d => d.Id == "1-1").FirstOrDefault(),
                        DescriptionTextCodeId = "1-1",
                        IsComposition = false,
                        ObjectTableTypeCode = "OT",
                        EnableSecurity = false,
                        MainTipCode = "MT",
                        IsSaveButtonVisible = false,
                        CustomFieldsCount = 2,
                        HasCustomFields = true,
                        SortingByObjectField = "nothing",
                        IsAutoComplete = false,
                        IsMain = true,
                        IsRestrictable = false,
                    },
                    new ObjectTable()
                    {
                        Id = "1-14",
                        Name = "GlobalZone",
                        Tenant = 1,
                        NewWizardControlName = "new Wizard",
                        HeaderScreenId = "1-1",
                        HeaderScreen = Screens.Where(d => d.Id == "1-1").FirstOrDefault(),
                        DescriptionTextCode = TextCodes.Where(d => d.Id == "1-1").FirstOrDefault(),
                        DescriptionTextCodeId = "1-1",
                        IsComposition = false,
                        ObjectTableTypeCode = "OT",
                        EnableSecurity = false,
                        MainTipCode = "MT",
                        IsSaveButtonVisible = false,
                        CustomFieldsCount = 2,
                        HasCustomFields = true,
                        SortingByObjectField = "nothing",
                        IsAutoComplete = false,
                        IsMain = true,
                        IsRestrictable = false,
                    },
                     new ObjectTable()
                    {
                        Id = "1-15",
                        Name = "Department",
                        Tenant = 1,
                        NewWizardControlName = "new Wizard",
                        HeaderScreenId = "1-1",
                        HeaderScreen = Screens.Where(d => d.Id == "1-1").FirstOrDefault(),
                        DescriptionTextCode = TextCodes.Where(d => d.Id == "1-1").FirstOrDefault(),
                        DescriptionTextCodeId = "1-1",
                        IsComposition = false,
                        ObjectTableTypeCode = "OT",
                        EnableSecurity = false,
                        MainTipCode = "MT",
                        IsSaveButtonVisible = false,
                        CustomFieldsCount = 2,
                        HasCustomFields = true,
                        SortingByObjectField = "nothing",
                        IsAutoComplete = false,
                        IsMain = true,
                        IsRestrictable = false,
                    },
                     new ObjectTable()
                    {
                        Id = "1-17",
                        Name = "Airline",
                        Tenant = 1,
                        NewWizardControlName = "new Wizard",
                        HeaderScreenId = "1-1",
                        HeaderScreen = Screens.Where(d => d.Id == "1-1").FirstOrDefault(),
                        DescriptionTextCode = TextCodes.Where(d => d.Id == "1-1").FirstOrDefault(),
                        DescriptionTextCodeId = "1-1",
                        IsComposition = false,
                        ObjectTableTypeCode = "OT",
                        EnableSecurity = false,
                        MainTipCode = "MT",
                        IsSaveButtonVisible = false,
                        CustomFieldsCount = 2,
                        HasCustomFields = true,
                        SortingByObjectField = "nothing",
                        IsAutoComplete = false,
                        IsMain = true,
                        IsRestrictable = false,
                    },
                     new ObjectTable()
                    {
                        Id = "1-18",
                        Name = "Currency",
                        Tenant = 1,
                        NewWizardControlName = "new Wizard",
                        HeaderScreenId = "1-1",
                        HeaderScreen = Screens.Where(d => d.Id == "1-1").FirstOrDefault(),
                        DescriptionTextCode = TextCodes.Where(d => d.Id == "1-1").FirstOrDefault(),
                        DescriptionTextCodeId = "1-1",
                        IsComposition = false,
                        ObjectTableTypeCode = "OT",
                        EnableSecurity = false,
                        MainTipCode = "MT",
                        IsSaveButtonVisible = false,
                        CustomFieldsCount = 2,
                        HasCustomFields = true,
                        SortingByObjectField = "nothing",
                        IsAutoComplete = false,
                        IsMain = true,
                        IsRestrictable = false,
                    },
                    new ObjectTable()
                    {
                        Id = "1-19",
                        Name = "Country",
                        Tenant = 1,
                        NewWizardControlName = "new Wizard",
                        HeaderScreenId = "1-1",
                        HeaderScreen = Screens.Where(d => d.Id == "1-1").FirstOrDefault(),
                        DescriptionTextCode = TextCodes.Where(d => d.Id == "1-1").FirstOrDefault(),
                        DescriptionTextCodeId = "1-1",
                        IsComposition = false,
                        ObjectTableTypeCode = "OT",
                        EnableSecurity = false,
                        MainTipCode = "MT",
                        IsSaveButtonVisible = false,
                        CustomFieldsCount = 2,
                        HasCustomFields = true,
                        SortingByObjectField = "nothing",
                        IsAutoComplete = false,
                        IsMain = true,
                        IsRestrictable = false,
                    },
                      new ObjectTable()
                    {
                        Id = "1-20",
                        Name = "ChargesType",
                        Tenant = 1,
                        NewWizardControlName = "new Wizard",
                        HeaderScreenId = "1-1",
                        HeaderScreen = Screens.Where(d => d.Id == "1-1").FirstOrDefault(),
                        DescriptionTextCode = TextCodes.Where(d => d.Id == "1-1").FirstOrDefault(),
                        DescriptionTextCodeId = "1-1",
                        IsComposition = false,
                        ObjectTableTypeCode = "OT",
                        EnableSecurity = false,
                        MainTipCode = "MT",
                        IsSaveButtonVisible = false,
                        CustomFieldsCount = 2,
                        HasCustomFields = true,
                        SortingByObjectField = "nothing",
                        IsAutoComplete = false,
                        IsMain = true,
                        IsRestrictable = false,
                    },
                    new ObjectTable()
                    {
                        Id = "1-21",
                        Name = "Branch",
                        Tenant = 1,
                        NewWizardControlName = "new Wizard",
                        HeaderScreenId = "1-1",
                        HeaderScreen = Screens.Where(d => d.Id == "1-1").FirstOrDefault(),
                        DescriptionTextCode = TextCodes.Where(d => d.Id == "1-1").FirstOrDefault(),
                        DescriptionTextCodeId = "1-1",
                        IsComposition = false,
                        ObjectTableTypeCode = "OT",
                        EnableSecurity = false,
                        MainTipCode = "MT",
                        IsSaveButtonVisible = false,
                        CustomFieldsCount = 2,
                        HasCustomFields = true,
                        SortingByObjectField = "nothing",
                        IsAutoComplete = false,
                        IsMain = true,
                        IsRestrictable = false,
                    },

                    new ObjectTable()
                    {
                        Id = "1-22",
                        Name = "CustomAgent",
                        Tenant = 1,
                        NewWizardControlName = "new Wizard",
                        HeaderScreenId = "1-1",
                        HeaderScreen = Screens.Where(d => d.Id == "1-1").FirstOrDefault(),
                        DescriptionTextCode = TextCodes.Where(d => d.Id == "1-1").FirstOrDefault(),
                        DescriptionTextCodeId = "1-1",
                        IsComposition = false,
                        ObjectTableTypeCode = "OT",
                        EnableSecurity = false,
                        MainTipCode = "MT",
                        IsSaveButtonVisible = false,
                        CustomFieldsCount = 2,
                        HasCustomFields = true,
                        SortingByObjectField = "nothing",
                        IsAutoComplete = false,
                        IsMain = true,
                        IsRestrictable = false,
                    },
                    new ObjectTable()
                    {
                        Id = "1-23",
                        Name = "Vendor",
                        Tenant = 1,
                        NewWizardControlName = "new Wizard",
                        HeaderScreenId = "1-1",
                        HeaderScreen = Screens.Where(d => d.Id == "1-1").FirstOrDefault(),
                        DescriptionTextCode = TextCodes.Where(d => d.Id == "1-1").FirstOrDefault(),
                        DescriptionTextCodeId = "1-1",
                        IsComposition = false,
                        ObjectTableTypeCode = "OT",
                        EnableSecurity = false,
                        MainTipCode = "MT",
                        IsSaveButtonVisible = false,
                        CustomFieldsCount = 2,
                        HasCustomFields = true,
                        SortingByObjectField = "nothing",
                        IsAutoComplete = false,
                        IsMain = true,
                        IsRestrictable = false,
                    },
                      new ObjectTable()
                    {
                        Id = "1-24",
                        Name = "Contact",
                        Tenant = 1,
                        NewWizardControlName = "new Wizard",
                        HeaderScreenId = "1-1",
                        HeaderScreen = Screens.Where(d => d.Id == "1-1").FirstOrDefault(),
                        DescriptionTextCode = TextCodes.Where(d => d.Id == "1-1").FirstOrDefault(),
                        DescriptionTextCodeId = "1-1",
                        IsComposition = false,
                        ObjectTableTypeCode = "OT",
                        EnableSecurity = false,
                        MainTipCode = "MT",
                        IsSaveButtonVisible = false,
                        CustomFieldsCount = 2,
                        HasCustomFields = true,
                        SortingByObjectField = "nothing",
                        IsAutoComplete = false,
                        IsMain = true,
                        IsRestrictable = false,
                    },

                      new ObjectTable()
                    {
                        Id = "1-25",
                        Name = "Quote",
                        Tenant = 1,
                        NewWizardControlName = "new Wizard",
                        HeaderScreenId = "1-1",
                        HeaderScreen = Screens.Where(d => d.Id == "1-1").FirstOrDefault(),
                        DescriptionTextCode = TextCodes.Where(d => d.Id == "1-1").FirstOrDefault(),
                        DescriptionTextCodeId = "1-1",
                        IsComposition = false,
                        ObjectTableTypeCode = "OT",
                        EnableSecurity = false,
                        MainTipCode = "MT",
                        IsSaveButtonVisible = false,
                        CustomFieldsCount = 2,
                        HasCustomFields = true,
                        SortingByObjectField = "nothing",
                        IsAutoComplete = false,
                        IsMain = true,
                        IsRestrictable = false,
                    },
                       new ObjectTable()
                    {
                        Id = "1-26",
                        Name = "Customer",
                        Tenant = 1,
                        NewWizardControlName = "new Wizard",
                        HeaderScreenId = "1-1",
                        HeaderScreen = Screens.Where(d => d.Id == "1-1").FirstOrDefault(),
                        DescriptionTextCode = TextCodes.Where(d => d.Id == "1-1").FirstOrDefault(),
                        DescriptionTextCodeId = "1-1",
                        IsComposition = false,
                        ObjectTableTypeCode = "OT",
                        EnableSecurity = false,
                        MainTipCode = "MT",
                        IsSaveButtonVisible = false,
                        CustomFieldsCount = 2,
                        HasCustomFields = true,
                        SortingByObjectField = "nothing",
                        IsAutoComplete = false,
                        IsMain = true,
                        IsRestrictable = false,
                    },

                     new ObjectTable()
                    {
                        Id = "1-27",
                        Name = "PackageType",
                        Tenant = 1,
                        NewWizardControlName = "new Wizard",
                        HeaderScreenId = "1-1",
                        HeaderScreen = Screens.Where(d => d.Id == "1-1").FirstOrDefault(),
                        DescriptionTextCode = TextCodes.Where(d => d.Id == "1-1").FirstOrDefault(),
                        DescriptionTextCodeId = "1-1",
                        IsComposition = false,
                        ObjectTableTypeCode = "OT",
                        EnableSecurity = false,
                        MainTipCode = "MT",
                        IsSaveButtonVisible = false,
                        CustomFieldsCount = 2,
                        HasCustomFields = true,
                        SortingByObjectField = "nothing",
                        IsAutoComplete = false,
                        IsMain = true,
                        IsRestrictable = false,
                    },
                    };

                     objectTablesObjectSet = new MockObjectSet<ObjectTable>(objectTables);
                 
                }  
              
                return objectTablesObjectSet;
            }
        }

        List<ObjectField> objectFields;
        MockObjectSet<ObjectField> objectFieldsObjectSet;
        public IDbSet<ObjectField> ObjectFields
        {
            get
            {
                if (objectFields == null)
                {
                    objectFields = new List<ObjectField>(){
                    new ObjectField() { Id = "1-1", Tenant = 1, InActive = false, ObjectTable_LookUpTable = ObjectTables.Where(d => d.Id == "1-1").FirstOrDefault(), ObjectTableId = "1-1", FullNameTextCode = TextCodes.Where(d => d.Id == "1-1").FirstOrDefault(), FullNameTextCodeId = "1-1", ShortNameTextCode = TextCodes.Where(d => d.Id == "1-1").FirstOrDefault(), ShortNameTextCodeId = "1-1", ListTextCode = TextCodes.Where(d => d.Id == "1-1").FirstOrDefault(), ListTextCodeId = "1-1", HelpTextCode = TextCodes.Where(d => d.Id == "1-1").FirstOrDefault(), HelpTextCodeId = "1-1", ObjectTable = ObjectTables.Where(d => d.Id == "1-1").FirstOrDefault(), ObjectTable_MultiTable = ObjectTables.Where(d => d.Id == "1-1").FirstOrDefault() }};
                    objectFieldsObjectSet = new MockObjectSet<ObjectField>(objectFields);

                }
              
                return objectFieldsObjectSet;
            }
        }

        List<Screen> screens;
        MockObjectSet<Screen> screenObjectSet;
        public IDbSet<Screen> Screens
        {
            get
            {
                if (screens == null)
                {
                    screens = new List<Screen>(){
                    new Screen() { Id = "1-1", Tenant = 1, Code = "HS" }};
                    screenObjectSet = new MockObjectSet<Screen>(screens);

                }
              
                return screenObjectSet;
            }
        }

        public IDbSet<ScreenField> ScreenFields
        {
            get { throw new NotImplementedException(); }
        }


        public IDbSet<TextCodeType> TextCodeTypes
        {
            get { throw new NotImplementedException(); }
        }


        public IDbSet<ChargesType> ChargesTypes
        {
            get { throw new NotImplementedException(); }
        }

        public IDbSet<ShipmentReceivableLineStatus> StatusTypes
        {
            get { throw new NotImplementedException(); }
        }

        public IDbSet<Measurement> UnitOfMeasurements
        {
            get { throw new NotImplementedException(); }
        }

        public IDbSet<ShipmentReceivable> ShipmentReceivables
        {
            get { throw new NotImplementedException(); }
        }


        //public IDbSet<Quote> Quotes
        //{
        //    get { throw new NotImplementedException(); }
        //}

        //public IDbSet<QuoteLine> QuoteLines
        //{
        //    get { throw new NotImplementedException(); }
        //}

        //public IDbSet<QuoteStatusType> QuoteStatusTypes
        //{
        //    get { throw new NotImplementedException(); }
        //}


        public IDbSet<ARInvoice> Invoices
        {
            get { throw new NotImplementedException(); }
        }

        public IDbSet<ARInvoiceLine> InvoiceLines
        {
            get { throw new NotImplementedException(); }
        }

        public IDbSet<ARInvoiceStatus> InvoiceStatuses
        {
            get { throw new NotImplementedException(); }
        }

        public IDbSet<ARInvoiceType> InvoiceTypes
        {
            get { throw new NotImplementedException(); }
        }

        public IDbSet<VatType> VatTypes
        {
            get { throw new NotImplementedException(); }
        }

        List<RatesTable> ratesTables;
        MockObjectSet<RatesTable> ratesTableObjectSet;
        public IDbSet<RatesTable> RatesTable
        {
            get
            {
                if (ratesTables == null)
                {
                    ratesTables = new List<RatesTable>(){
                   new RatesTable() { Id = "1-1", Tenant = 1, ForeignCurrencyId = "1-2", ForeignCurrency = Currencies.Where(d => d.Id == "1-2").FirstOrDefault(), BaseCurrencyId = "1-1", BaseCurrency = Currencies.Where(d => d.Id == "1-1").FirstOrDefault()  }};

                   
                    ratesTableObjectSet = new MockObjectSet<RatesTable>(ratesTables);
                }
                return ratesTableObjectSet;
            }
        }

        List<EventType> eventTypes;
        MockObjectSet<EventType> eventTypesObjectSet;
        public IDbSet<EventType> EventType
        {
            get
            {
                if (eventTypes == null)
                {
                    eventTypes = new List<EventType>(){
                     new EventType() { Id = "1-1", Tenant = 1, EntityStatusId = "111", ObjectTableId = "1-1", AddedManually = true, InActive = true, IsManualEntry = false, Code="CRTR" },
                     new EventType() { Id = "1-2", Tenant = 1, EntityStatusId = "222", ObjectTableId = "1-2", AddedManually = true, InActive = true, IsManualEntry = true, Code="CRET"  },
                     new EventType() { Id = "1-3", Tenant = 1, EntityStatusId = "222", ObjectTableId = "1-2", AddedManually = true, InActive = true, IsManualEntry = true,Code="UPET" },
                     new EventType() { Id = "1-4", Tenant = 1, EntityStatusId = "222", ObjectTableId = "1-4", AddedManually = true, InActive = true, IsManualEntry = true,Code="CRWH" },
                     new EventType() { Id = "1-5", Tenant = 1, EntityStatusId = "222", ObjectTableId = "1-6", AddedManually = true, InActive = true, IsManualEntry = true,Code="CRVT" },
                     new EventType() { Id = "1-6", Tenant = 1, EntityStatusId = "222", ObjectTableId = "1-7", AddedManually = true, InActive = true, IsManualEntry = true,Code="CRST" },
                     new EventType() { Id = "1-7", Tenant = 1, EntityStatusId = "222", ObjectTableId = "1-8", AddedManually = true, InActive = true, IsManualEntry = true,Code="CRSL" },
                     new EventType() { Id = "1-8", Tenant = 1, EntityStatusId = "222", ObjectTableId = "1-9", AddedManually = true, InActive = true, IsManualEntry = true,Code="CRSA" },
                     new EventType() { Id = "1-9", Tenant = 1, EntityStatusId = "222", ObjectTableId = "1-10", AddedManually = true, InActive = true, IsManualEntry = true,Code="CRPC" },
                     new EventType() { Id = "1-10", Tenant = 1, EntityStatusId = "222", ObjectTableId = "1-11", AddedManually = true, InActive = true, IsManualEntry = true,Code="CRPO" },
                     new EventType() { Id = "1-11", Tenant = 1, EntityStatusId = "222", ObjectTableId = "1-12", AddedManually = true, InActive = true, IsManualEntry = true,Code="CRPK" },
                     new EventType() { Id = "1-12", Tenant = 1, EntityStatusId = "222", ObjectTableId = "1-13", AddedManually = true, InActive = true, IsManualEntry = true,Code="CRIT" },
                     new EventType() { Id = "1-13", Tenant = 1, EntityStatusId = "222", ObjectTableId = "1-14", AddedManually = true, InActive = true, IsManualEntry = true,Code="CRGZ" },
                     new EventType() { Id = "1-14", Tenant = 1, EntityStatusId = "222", ObjectTableId = "1-15", AddedManually = true, InActive = true, IsManualEntry = true,Code="CRDP" },
                     new EventType() { Id = "1-15", Tenant = 1, EntityStatusId = "222", ObjectTableId = "1-17", AddedManually = true, InActive = true, IsManualEntry = true,Code="CRAL" },
                     new EventType() { Id = "1-16", Tenant = 1, EntityStatusId = "222", ObjectTableId = "1-18", AddedManually = true, InActive = true, IsManualEntry = true,Code="CRCR" },
                     new EventType() { Id = "1-17", Tenant = 1, EntityStatusId = "222", ObjectTableId = "1-19", AddedManually = true, InActive = true, IsManualEntry = true,Code="CRCN" },
                     new EventType() { Id = "1-18", Tenant = 1, EntityStatusId = "222", ObjectTableId = "1-20", AddedManually = true, InActive = true, IsManualEntry = true,Code="CRCT" },
                     new EventType() { Id = "1-19", Tenant = 1, EntityStatusId = "222", ObjectTableId = "1-21", AddedManually = true, InActive = true, IsManualEntry = true,Code="CRBR" },
                     new EventType() { Id = "1-20", Tenant = 1, EntityStatusId = "222", ObjectTableId = "1-22", AddedManually = true, InActive = true, IsManualEntry = true,Code="CRCA" },
                     new EventType() { Id = "1-21", Tenant = 1, EntityStatusId = "222", ObjectTableId = "1-23", AddedManually = true, InActive = true, IsManualEntry = true,Code="CRVD" },
                     new EventType() { Id = "1-22", Tenant = 1, EntityStatusId = "222", ObjectTableId = "1-24", AddedManually = true, InActive = true, IsManualEntry = true,Code="CRCO" },
                     new EventType() { Id = "1-23", Tenant = 1, EntityStatusId = "222", ObjectTableId = "1-23", AddedManually = true, InActive = true, IsManualEntry = true,Code="UPVD" },
                     new EventType() { Id = "1-24", Tenant = 1, EntityStatusId = "222", ObjectTableId = "1-25", AddedManually = true, InActive = true, IsManualEntry = true,Code="QTIN" },
                     new EventType() { Id = "1-25", Tenant = 1, EntityStatusId = "222", ObjectTableId = "1-26", AddedManually = true, InActive = true, IsManualEntry = true,Code="CRCU" },
                     new EventType() { Id = "1-26", Tenant = 1, EntityStatusId = "222", ObjectTableId = "1-27", AddedManually = true, InActive = true, IsManualEntry = true,Code="UPPK" },};
                 
                   

                    
                    eventTypesObjectSet = new MockObjectSet<EventType>(eventTypes);
                }
                return eventTypesObjectSet;
            }
        }

        List<TraceEvent> traceEvents;
        MockObjectSet<TraceEvent> traceEventsObjectSet;
        public IDbSet<TraceEvent> TraceEvent
        {
            get
            {
                if (traceEvents == null)
                {
                    traceEvents = new List<TraceEvent>() {
                    new TraceEvent(){ Id="1-1" , EventType=eventTypes.Where(d=>d.Id=="1-2").FirstOrDefault() , EventTypeId="1-2" , Tenant=1}};
                   

                    traceEventsObjectSet = new MockObjectSet<TraceEvent>(traceEvents);
                }
                return traceEventsObjectSet;
            }
        }


        //public IDbSet<Basket> Baskets
        //{
        //    get { throw new NotImplementedException(); }
        //}

        //public IDbSet<BasketType> BasketTypes
        //{
        //    get { throw new NotImplementedException(); }
        //}


        public IDbSet<Rank> Ranks
        {
            get { throw new NotImplementedException(); }
        }

        //public IDbSet<VerifyQueue> VerifyQueues
        //{
        //    get { throw new NotImplementedException(); }
        //}


        private MockObjectSet<Document> documentList;
        public IDbSet<Document> Documents
        {
            get
            {
                documentList = new MockObjectSet<Document>();
                Document document1 = new Document() { Id = "1-1", Tenant = 1 };
             
                documentList.AddObject(document1);
            
                return documentList;
            }
        }

        List<Query> queries;
        private MockObjectSet<Query> queryList;
        public IDbSet<Query> Queries
        {
            get
            {
                if (queries == null)
                {
                    queries = new List<Query>(){
           new Query() { Id = "1-1", Code = "IA", Tenant = 1, NameTextCode = TextCodes.Where(d => d.Id == "1-1").FirstOrDefault(), NameTextCodeId = "1-1", ObjectTableId = "1-1", ObjectTable = ObjectTables.Where(d => d.Id == "1-1").FirstOrDefault(), QueryGroup = QueryGroups.Where(d => d.Code == "QG").FirstOrDefault(), QueryGroupCode = "QG" }};

          
                    queryList = new MockObjectSet<Query>(queries);
                }
              
                return queryList;
            }
        }



        public IDbSet<QueryColumn> QueryColumns
        {
            get { throw new NotImplementedException(); }
        }

     
  


        //public IDbSet<NewMessage> NewMessages
        //{
        //    get { throw new NotImplementedException(); }
        //}

        //public IDbSet<Conversation> Conversations
        //{
        //    get { throw new NotImplementedException(); }
        //}


        public IDbSet<DBIdCounter> DBIdCounters
        {
            get { throw new NotImplementedException(); }
        }


        List<MenusTable> menusTables;
        private MockObjectSet<MenusTable> menusTablesList;
        public IDbSet<MenusTable> MenusTables
        {
           get
            {
                if (menusTables == null)
                {
                    menusTables = new List<MenusTable>()

                {
              new MenusTable() { Id = "1-1", Tenant = 1 , MenuTypeCode="40" , MenuType=MenusTypes.Where(d=>d.Code=="40").FirstOrDefault()}};

                    menusTablesList = new MockObjectSet<MenusTable>(menusTables);
                }
               return menusTablesList; 
            }
           
        }

        List<MenuType> menuTypes;
        private MockObjectSet<MenuType> menuTypeList;
        public IDbSet<MenuType> MenusTypes
        {
            get
            {
                if (menusTables == null)
                {
                    menuTypes = new List<MenuType>()

                {
              new MenuType() {Code="40" }};

                    menuTypeList = new MockObjectSet<MenuType>(menuTypes);
                }
                return menuTypeList;
            }
        }

        public IDbSet<CategoryType> CategoryTypes
        {
            get { throw new NotImplementedException(); }
        }

        public IDbSet<CustomTable> CustomTables
        {
            get { throw new NotImplementedException(); }
        }


        public IDbSet<PackageType> PackageTypes
        {
            get { throw new NotImplementedException(); }
        }

        //public IDbSet<ContainerType> ContainerTypes
        //{
        //    get { throw new NotImplementedException(); }
        //}



        public IDbSet<ShipmentPackage> ShipmentPackages
        {
            get { throw new NotImplementedException(); }
        }


        public IDbSet<AdvancedQueryFilter> AdvancedQueryFilters
        {
            get { throw new NotImplementedException(); }
        }


        public IDbSet<ObjectTableTab> ObjectTableTabs
        {
            get { throw new NotImplementedException(); }
        }


        public IDbSet<ObjectTableHelperControl> ObjectTableHelperControls
        {
            get { throw new NotImplementedException(); }
        }


        public IDbSet<InsideShipmentPackage> InsideShipmentPackages
        {
            get { throw new NotImplementedException(); }
        }
        public IDbSet<GeneralLock> GeneralLocks
        {
            get { throw new NotImplementedException(); }
        }
        private MockObjectSet<IATACode> iATACodeList;
        public IDbSet<IATACode> IATACodes
        {
            get
            {
                iATACodeList = new MockObjectSet<IATACode>();
                IATACode iATACode1 = new IATACode() { Code="IA"};
                IATACode iATACode2 = new IATACode() { Code="IA" };

                iATACodeList.AddObject(iATACode1);
                iATACodeList.AddObject(iATACode1);
                return iATACodeList;
            }
        }

        private MockObjectSet<ChargesGroup> chargesGroupList;
        public IDbSet<ChargesGroup> ChargesGroups
        {
            get
            {
                chargesGroupList = new MockObjectSet<ChargesGroup>();
                ChargesGroup chargesGroup1 = new ChargesGroup() { Code = "CG" };


                chargesGroupList.AddObject(chargesGroup1);
          
                return chargesGroupList;
            }
        }

        private MockObjectSet<VolumeUnit> volumeUnitList;
        public IDbSet<VolumeUnit> VolumeUnits
        {
            get
            {
                volumeUnitList = new MockObjectSet<VolumeUnit>();
                VolumeUnit volumeUnit1 = new VolumeUnit() { Code = "VU" };
                VolumeUnit volumeUnit2 = new VolumeUnit() { Code = "YU" };

                volumeUnitList.AddObject(volumeUnit1);
                volumeUnitList.AddObject(volumeUnit2);
                return volumeUnitList;
            }
        }



        List<EntityStatus> entityStatuses;
        public IDbSet<EntityStatus> EntityStatus
        {
            get
            {
                if (entityStatuses == null)
                {
                    entityStatuses = new List<EntityStatus>()
                    {
                   new EntityStatus() { Id = "111", Name = "ES1", Tenant = 1, InActive = false, ObjectTableId = "1-1", ObjectTable = ObjectTables.Where(d => d.Id == "1-1").FirstOrDefault() },
                    new EntityStatus() { Id = "222", Name = "ES2", Tenant = 2 }
                };
                    entityStatusList = new MockObjectSet<EntityStatus>(entityStatuses);
                }
               
                return entityStatusList;
            }
        }


        public IDbSet<CommunicationStatusType> CommunicationStatusTypes
        {
            get { throw new NotImplementedException(); }
        }

        public IDbSet<CommunicationLogType> CommunicationLogTypes
        {
            get { throw new NotImplementedException(); }
        }


        public IDbSet<WarehouseType> WarehouseTypes
        {
            get { throw new NotImplementedException(); }
        }

        public IDbSet<NumberFormat> NumberFormats
        {
            get { throw new NotImplementedException(); }
        }

        public IDbSet<Vessel> Vessels
        {
            get { throw new NotImplementedException(); }
        }

        List<DescriptionOfGoods> descriptionOfGoods;
        private MockObjectSet<DescriptionOfGoods> descriptionOfGoodsList;
        public IDbSet<DescriptionOfGoods> DescriptionOfGoods
        {
            get
            {
                if (descriptionOfGoods == null)
                {
                    descriptionOfGoods = new List<DescriptionOfGoods>(){
                    new DescriptionOfGoods() { Id = "1-1", Name = "ES1", Tenant = 1 }};

                    descriptionOfGoodsList = new MockObjectSet<DescriptionOfGoods>(descriptionOfGoods);
                }
                
              
                return descriptionOfGoodsList;
            }
        }


        public IDbSet<Warehouse> Warehouses
        {
            get { throw new NotImplementedException(); }
        }


        //public IDbSet<LastUpdate> LastUpdates
        //{
        //    get { throw new NotImplementedException(); }
        //}


        private MockObjectSet<MenuButton> menuButtonList;
        public IDbSet<MenuButton> MenuButtons
        {
            get
            {
                menuButtonList = new MockObjectSet<MenuButton>();
               
                return menuButtonList;
            }
        }

        List<MenuButtonGroup> menuButtonGroups;
        private MockObjectSet<MenuButtonGroup> menuButtonGroupList;
        public IDbSet<MenuButtonGroup> MenuButtonGroups
        {
            get
            {
                if (menuButtonGroups == null)
                {
                    menuButtonGroups = new List<MenuButtonGroup>(){
                    new MenuButtonGroup() { Id = "111", Name = "ES1", ObjectTableId = "1-1", MenuButtonGroupType = "new", Tenant = 1, ObjectTable = ObjectTables.Where(d => d.Id == "1-1").FirstOrDefault() }};

                    menuButtonGroupList = new MockObjectSet<MenuButtonGroup>(menuButtonGroups);
                }
                return menuButtonGroupList;
            }
        }


        public IDbSet<EntityLastActivity> EntityLastActivities
        {
            get { throw new NotImplementedException(); }
        }

        List<CounterDefinition> counterDefinitions;
        private MockObjectSet<CounterDefinition> counterDefinitionList;
        public IDbSet<CounterDefinition> CounterDefinitions
        {
            get
            {

                if (counterDefinitions == null)
                {
                    counterDefinitions = new List<CounterDefinition>()
                    {
                    new CounterDefinition() { Id = "111", Tenant = 1, CounterId = "1", Counter = Counters.Where(d => d.Id == "1").FirstOrDefault()} };

                    counterDefinitionList = new MockObjectSet<CounterDefinition>(counterDefinitions);
                }
                return counterDefinitionList;
            }
        }

            private MockObjectSet<ObjectFieldValidation> objectFieldValidationList;
        public IDbSet<ObjectFieldValidation> ObjectFieldValidations
        {
            get
             {

                 objectFieldValidationList = new MockObjectSet<ObjectFieldValidation>();
                 ObjectFieldValidation objectFieldValidation1 = new ObjectFieldValidation() { Id = "1-1", Tenant = 1, ObjectField=ObjectFields.Where(d=>d.Id=="1-1").FirstOrDefault() , ObjectFieldId="1-1"};

                 objectFieldValidationList.AddObject(objectFieldValidation1);
                 return objectFieldValidationList;
            }
        }


        public IDbSet<RuleType> RuleTypes
        {
            get { throw new NotImplementedException(); }
        }

        public IDbSet<ObjectTableRule> ObjectTableRules
        {
            get { throw new NotImplementedException(); }
        }

        public IDbSet<ObjectTableRuleField> ObjectTableRuleFields
        {
            get { throw new NotImplementedException(); }
        }


        private MockObjectSet<QueryGroup> queryGroupList;
        public IDbSet<QueryGroup> QueryGroups
        {
            get
            {
                queryGroupList = new MockObjectSet<QueryGroup>();
                QueryGroup queryGroup1 = new QueryGroup() { Code = "QG", IndexOrder = 1 };


                queryGroupList.AddObject(queryGroup1);
              
                return queryGroupList;
            }
        }


        public IDbSet<TriggerType> TriggerTypes
        {
            get { throw new NotImplementedException(); }
        }


        public IDbSet<RuleNotificationType> RuleNotificationTypes
        {
            get { throw new NotImplementedException(); }
        }


        //public IDbSet<FixedAmount> FixedAmounts
        //{
        //    get { throw new NotImplementedException(); }
        //}

        List<Counter> counters;
        public IDbSet<Counter> Counters
        {
            get
            {
                if (counters == null)
                {
                    counters = new List<Counter>()
                    {
                    new    Counter() { Id = "1", ObjectTableId = "1-1", ObjectTable = ObjectTables.Where(d => d.Id == "1-1").FirstOrDefault(), Tenant = 1 },
                    new   Counter() { Id = "2", ObjectTableId = "1-1", ObjectTable = ObjectTables.Where(d => d.Id == "1-1").FirstOrDefault(), Tenant = 1 }};

                    counterList = new MockObjectSet<Counter>(counters);
                }

                return counterList;
            }
        }

        List<TenantSetting> tenantSettings;
        private MockObjectSet<TenantSetting> tenantSettingList;
        public IDbSet<TenantSetting> TenantSettings
        {
            get
            {
                if (tenantSettings == null)
                {
                    tenantSettings = new List<TenantSetting>(){
                 new TenantSetting() { Id = "1-1", Tenant = 1, ObjectTable = ObjectTables.Where(d => d.Id == "1-1").FirstOrDefault(), ObjectTableId = "1-1", SettingCode = "CC", SettingValue = "12" }};
                    tenantSettingList = new MockObjectSet<TenantSetting>(tenantSettings);
                }

                return tenantSettingList;
            }
        }

        List<CounterStat> counterStats;
        private MockObjectSet<CounterStat> counterStatList;
        public IDbSet<CounterStat> CounterStats
        {
            get
            {
                if (counterStats == null)
                {
                    counterStats = new List<CounterStat>(){
                 new CounterStat() { Id = 1, Tenant = 1 }};
                    counterStatList = new MockObjectSet<CounterStat>(counterStats);
                }

                return counterStatList;
            }
        }


        public IDbSet<Tip> Tips
        {
            get { return new MockObjectSet<Tip>(new List<Tip>()); }
        }

        public IDbSet<TipsVisibility> TipsVisibilities
        {
            get { throw new NotImplementedException(); }
        }


        private MockObjectSet<ObjectFieldModification> objectFieldModificationList;
        public IDbSet<ObjectFieldModification> ObjectFieldModifications
        {
            get
            {
                objectFieldModificationList = new MockObjectSet<ObjectFieldModification>();
                ObjectFieldModification objectFieldModification1 = new ObjectFieldModification() { ObjectFieldId = "1-1", Tenant = 1, Id = "1-1" };

                 objectFieldModificationList.AddObject(objectFieldModification1);
                return objectFieldModificationList;
               
            }
        }


        public IDbSet<ScreenModification> ScreenModifications
        {
            get { throw new NotImplementedException(); }
        }


        public IDbSet<ImageDetail> ImageDetails
        {
            get { throw new NotImplementedException(); }
        }

        public IDbSet<ImageLibrary> ImageLibrarys
        {
            get { throw new NotImplementedException(); }
        }



        public IDbSet<PermissionType> PermissionTypes
        {
            get { throw new NotImplementedException(); }
        }


        public IDbSet<ObjectTableType> ObjectTableTypes
        {
            get { throw new NotImplementedException(); }
        }

        public IDbSet<FeatureType> FeatureTypes
        {
            get { throw new NotImplementedException(); }
        }

        public IDbSet<RoleType> RoleTypes
        {
            get { throw new NotImplementedException(); }
        }


        public IDbSet<Package> Packages
        {
            get { throw new NotImplementedException(); }
        }


        public IDbSet<PackageFeature> PackageFeatures
        {
            get { throw new NotImplementedException(); }
        }


        public IDbSet<RuleConditionField> RuleConditionFields
        {
            get { throw new NotImplementedException(); }
        }

        List<CustomPickList> customPickLists;
        private MockObjectSet<CustomPickList> customPickListObjectSet;
        public IDbSet<CustomPickList> CustomPickLists
        {
            get
            {
                if (customPickLists == null)
                {
                    customPickLists = new List<CustomPickList>(){
                   new CustomPickList() { Id = "1-1", Tenant = 1 }};
                    customPickListObjectSet = new MockObjectSet<CustomPickList>(customPickLists);
                }


                return customPickListObjectSet;
            }
        }

        List<SharedLogisticsUpdate> sharedLogisticsUpdates;
        private MockObjectSet<SharedLogisticsUpdate> sharedLogisticsUpdateObjectSet;
        public IDbSet<SharedLogisticsUpdate> SharedLogisticsUpdates
        {


             get
            {
                if (sharedLogisticsUpdates == null)
                {
                    sharedLogisticsUpdates = new List<SharedLogisticsUpdate>(){
                    new SharedLogisticsUpdate() { Id = "1-1", Tenant = 1, DocumentId = "1-1", EntityId = "1-1", HandledByUserId = "1-1", ObjectTableId = "1-1", Document = Documents.Where(d => d.Id == "1-1").FirstOrDefault(), ObjectTable = ObjectTables.Where(d => d.Id == "1-1").FirstOrDefault(), HandledByUser = Users.Where(d => d.Id == "111").FirstOrDefault() }};
                    sharedLogisticsUpdateObjectSet = new MockObjectSet<SharedLogisticsUpdate>(sharedLogisticsUpdates);

                }
                return sharedLogisticsUpdateObjectSet;
            }
           
        }

        public IDbSet<SharedLogisticsUpdateStatus> SharedLogisticsUpdateStatus
        {
            get { throw new NotImplementedException(); }
        }


        public IDbSet<EntityLastUpdate> EntityLastUpdates
        {
            get { throw new NotImplementedException(); }
        }


        public IDbSet<EntityLastActivityType> EntityLastActivityTypes
        {
            get { throw new NotImplementedException(); }
        }


        public IDbSet<EventTypeCategory> EventTypeCategories
        {
            get { throw new NotImplementedException(); }
        }


        IDbSet<SharedLogisticsInvitationStatus> IWebFreightContext.SharedLogisticsInvitationStatus
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }


        public System.Data.Common.DbConnection GetConnection()
        {
            throw new NotImplementedException();
        }


        public DbContext GetActiveDbContext()
        {
            throw new NotImplementedException();
        }


        public IDbSet<ObjectTableLastUpdate> ObjectTableLastUpdates
        {
            get { throw new NotImplementedException(); }
        }

        #region Inbound Emails
        private MockObjectSet<InboundEmail> inboundEmailsList;
        public IDbSet<InboundEmail> InboundEmails
        {
            get
            {
                inboundEmailsList = new MockObjectSet<InboundEmail>();
                InboundEmail inboundEmails1 = new InboundEmail() { Id = "1-1", Tenant = 0 };
                InboundEmail inboundEmails2 = new InboundEmail() { Id = "1-2", Tenant = 0 };

                inboundEmailsList.AddObject(inboundEmails1);
                inboundEmailsList.AddObject(inboundEmails2);
                return inboundEmailsList;
            }
        }

        private MockObjectSet<InboundEmailLine> inboundEmailLinesList;
        public IDbSet<InboundEmailLine> InboundEmailLines
        {
            get
            {
                inboundEmailLinesList = new MockObjectSet<InboundEmailLine>();
                InboundEmailLine inboundEmailLines1 = new InboundEmailLine() { Id = "1-1", Tenant = 0 };
                InboundEmailLine inboundEmailLines2 = new InboundEmailLine() { Id = "1-2", Tenant = 0 };

                inboundEmailLinesList.AddObject(inboundEmailLines1);
                inboundEmailLinesList.AddObject(inboundEmailLines2);
                return inboundEmailLinesList;
            }
        }
        #endregion 
    

        public IDbSet<QueueDefinition> QueueDefinitions
        {
            get { throw new NotImplementedException(); }
        }

        public IDbSet<QueueMessage> QueueMessages
        {
            get { throw new NotImplementedException(); }
        }

        #region Business Hour 
        private MockObjectSet<BusinessHour> businessHoursList;
        public IDbSet<BusinessHour> BusinessHours
        {
            get
            {
                businessHoursList = new MockObjectSet<BusinessHour>();
                BusinessHour businessHours1 = new BusinessHour() { Id = "1-1", Tenant = 0 };
                BusinessHour businessHours2 = new BusinessHour() { Id = "1-2", Tenant = 0 };

                businessHoursList.AddObject(businessHours1);
                businessHoursList.AddObject(businessHours2);
                return businessHoursList;
            }
        }

        private MockObjectSet<BusinessHoursHoliday> businessHoursHolidaysList;
        public IDbSet<BusinessHoursHoliday> BusinessHoursHolidays
        {
            get
            {
                businessHoursHolidaysList = new MockObjectSet<BusinessHoursHoliday>();
                BusinessHoursHoliday businessHourHolidays1 = new BusinessHoursHoliday() { Id = "1-1", Tenant = 0 };
                BusinessHoursHoliday businessHourHolidays2 = new BusinessHoursHoliday() { Id = "1-2", Tenant = 0 };

                businessHoursHolidaysList.AddObject(businessHourHolidays1);
                businessHoursHolidaysList.AddObject(businessHourHolidays2);
                return businessHoursHolidaysList;
            }
        }
        #endregion 
    

        public IDbSet<APILogs> APILogs
        {
            get { throw new NotImplementedException(); }
        }

        public IDbSet<APILogsData> APILogsData
        {
            get { throw new NotImplementedException(); }
        }


        public IDbSet<QueueMessageMoreDetails> QueueMessageMoreDetails
        {
            get { throw new NotImplementedException(); }
        }


        public IDbSet<TasksScheduler> TasksSchedulers
        {
            get { throw new NotImplementedException(); }
        }

        public IDbSet<TaskSchedulerHistory> TaskSchedulerHistories
        {
            get { throw new NotImplementedException(); }
        }

        public IDbSet<DWObjectTable> DWObjectTables
        {
            get { throw new NotImplementedException(); }
        }
        public IDbSet<DWObjectField> DWObjectFields
        {
            get { throw new NotImplementedException(); }
        }



        public IDbSet<DWQuery> DWQueries
        {
            get { throw new NotImplementedException(); }
        }
        public IDbSet<DWSubQuery> DWSubQueries
        {
            get { throw new NotImplementedException(); }
        }
        public IDbSet<DWQueryColumn> DWQueryColumns
        {
            get { throw new NotImplementedException(); }
        }
        public IDbSet<DWQueryFilter> DWQueryFilters
        {
            get { throw new NotImplementedException(); }
        }

        public IDbSet<SharedUserQuery> SharedUserQueries
        {
            get { throw new NotImplementedException(); }
        }

        public IDbSet<DWCategories> DWCategories
        {
            get { throw new NotImplementedException(); }
        }

        public IDbSet<DWObjectFieldCategories> DWObjectFieldCategories
        {
            get { throw new NotImplementedException(); }
        }
    }
}
