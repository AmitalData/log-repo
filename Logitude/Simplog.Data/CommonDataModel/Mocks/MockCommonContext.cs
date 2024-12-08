using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;

namespace Simplog.Data.CommonDataModel.Mocks
{
    public class MockCommonContext : ICommonDataContext
    {
        List<Country> countries;
        MockObjectSet<Country> countryObjectSet;
        public IDbSet<Country> Countries
        {
            get
            {
                if (countries == null)
                {
                    countries = new List<Country>() {
                        new Country() { Id = "1-1", Code = "GB", EnglishName = "Britin", Tenant = 1  },
                        new Country() { Id = "1-2", Code = "US", EnglishName = "USA", Tenant = 2} };
                    countryObjectSet = new MockObjectSet<Country>(countries);
                }
                return countryObjectSet;
            }
        }

        List<State> states;
        MockObjectSet<State> stateObjectSet;
        public IDbSet<State> States
        {
            get
            {
                if (states == null)
                {
                    states = new List<State>() {
                        new State() { Id ="1-1", Code = "GB", EnglishName = "Britin", Tenant = 1 , CountryId="1-1", LocalName="Britin",  Country=this.Countries.Where(d => d.Id == "1-1").FirstOrDefault()  },
                        new State() { Id = "1-2", Code = "US", EnglishName = "USA", Tenant = 2 } };
                    stateObjectSet = new MockObjectSet<State>(states);
                }
                return stateObjectSet;
            }
        }

        List<GlobalZone> globalZones;
        MockObjectSet<GlobalZone> globalZoneObjectSet;
        public IDbSet<GlobalZone> GlobalZones
        {
            get
            {
                if (globalZones == null)
                {
                    globalZones = new List<GlobalZone>() {
                        new GlobalZone() { Id = "1-1", Code = "GB", EnglishName = "Britin", Tenant =1 },
                        new GlobalZone() { Id = "1-2", Code = "US", EnglishName = "USA", Tenant = 2 } };
                    globalZoneObjectSet = new MockObjectSet<GlobalZone>(globalZones);

                }
                return globalZoneObjectSet;
            }
        }

        List<Address> addresses;
        MockObjectSet<Address> addressObjectSet;
        public IDbSet<Address> Addresses
        {
            get
            {
                if (addresses == null)
                {
                    addresses = new List<Address>() {
                        new Address() { Id = "1-1" ,  Tenant =1 , Country=Countries.Where(d=>d.Id=="1-1").FirstOrDefault()  , CountryId="1-1" },
                        new Address() { Id = "1-2" ,  Tenant = 2, Country=Countries.Where(d=>d.Id=="1-2").FirstOrDefault() } };
                    addressObjectSet = new MockObjectSet<Address>(addresses);
                }
                return addressObjectSet;
            }
        }
        
        MockObjectSet<Port> portObjectSet;
        List<Port> ports;
        public IDbSet<Port> Ports
        {
            get
            {
                if (ports == null)
                {
                    ports = new List<Port>() 
                { new Port() { Id = "1-1", Code = "HT", EnglishName = "Hethrow", Country = this.Countries.Where(d => d.Code == "GB").FirstOrDefault(), Tenant = 1 , CountryId="1-1" },
                 new Port() { Id = "1-2", Code = "JFK", EnglishName = "jfk", Country = this.Countries.Where(d => d.Code == "US").FirstOrDefault(), Tenant = 2  , CountryId="1-2"} };

                    portObjectSet = new MockObjectSet<Port>(ports);
                }
                return portObjectSet;
            }

        }

        MockObjectSet<Card> cardObjectSet;
        List<Card> cards;
        public IDbSet<Card> Cards
        {
            get
            {
                if (cards == null)
                {
                    cards = new List<Card>() 
                { new Card() { Id = "1-1", Code = " CRTR", EnglishName = "Hethrow" ,Tenant = 1    },
                 new Card() { Id = "1-2", Code = "JFK", EnglishName = "jfk",Tenant = 2 } };

                    cardObjectSet = new MockObjectSet<Card>(cards);
                }
                return cardObjectSet;
            }

        }

        MockObjectSet<Customer> customerObjectSet;
        List<Customer> customers;
        public IDbSet<Customer> Customers
        {
            get
            {
                if (customers == null)
                {
                    customers = new List<Customer>() 
                { new Customer() { Id = "1-1" ,Tenant = 1 , Card=Cards.Where(d=>d.Id=="1-1").FirstOrDefault()},
                 new Customer() { Id = "1-2",Tenant = 2  ,  Card=Cards.Where(d=>d.Id=="1-2").FirstOrDefault()} };

                    customerObjectSet = new MockObjectSet<Customer>(customers);
                }
                return customerObjectSet;
            }

        }

        public IDbSet<Agent> Agents
        {
            get { throw new NotImplementedException(); }
        }

        MockObjectSet<CustomAgent> customAgentObjectSet;
        List<CustomAgent> customAgents;
        public IDbSet<CustomAgent> CustomAgents
        {
            get
            {
                if (customAgents == null)
                {
                    customAgents = new List<CustomAgent>() 
                { new CustomAgent() { Id = "1-1",Tenant = 1 , Card=Cards.Where(d=>d.Id=="1-1").FirstOrDefault() },
                 new CustomAgent() { Id = "1-2",Tenant = 2  , Card=Cards.Where(d=>d.Id=="1-2").FirstOrDefault()} };

                    customAgentObjectSet = new MockObjectSet<CustomAgent>(customAgents);
                }
                return customAgentObjectSet;
            }
        }

        MockObjectSet<ShippingAgent> shippingAgentObjectSet;
        List<ShippingAgent> shippingAgents;
        public IDbSet<ShippingAgent> ShippingAgents
        {
            get
            {
                if (shippingAgents == null)
                {
                    shippingAgents = new List<ShippingAgent>() 
                { new ShippingAgent() { Id = "1-1",Tenant = 1 , Card=Cards.Where(d=>d.Id=="1-1").FirstOrDefault() },
                 new ShippingAgent() { Id = "1-2",Tenant = 2  , Card=Cards.Where(d=>d.Id=="1-2").FirstOrDefault()} };

                    shippingAgentObjectSet = new MockObjectSet<ShippingAgent>(shippingAgents);
                }
                return shippingAgentObjectSet;
            }
        }

        List<Contact> contatcs;
        MockObjectSet<Contact> contactObjectSet;
        public IDbSet<Contact> Contacts
        {
            get
            {
                if (contatcs == null)
                {
                    contatcs = new List<Contact>() 
                {
                new Contact() { Id = "1-1", EnglishName = "user1", Email = "user1@amital.co.il", Tenant = 1, },
                new Contact() { Id = "1-2", EnglishName = "user2", Email = "user2@amital.co.il", Tenant = 2, },
                new Contact() { Id = "1-5", EnglishName = "user7", Email = "user7@amital.co.il", Tenant = 1, },
                new Contact() { Id = "1-3", EnglishName = "user3", Email = "user3@amital.co.il", Tenant =3, }, };

                    contactObjectSet = new MockObjectSet<Contact>(contatcs);
                }

                return contactObjectSet;
            }
        }

        public IDbSet<CardContact> CardContacts
        {
            get { throw new NotImplementedException(); }
        }

        List<PartnerType> partnerTypes;
        MockObjectSet<PartnerType> partnerTypeObjectSet;
        public IDbSet<PartnerType> PartnerTypes
        {
            get
            {
                if (partnerTypes == null)
                {
                    partnerTypes = new List<PartnerType>() {
                        new PartnerType() { Id = "1-1"},
                        new PartnerType() { Id = "1-2" } };
                    partnerTypeObjectSet = new MockObjectSet<PartnerType>(partnerTypes);
                }
                return partnerTypeObjectSet;
            }
        }

        List<Rank> ranks;
        MockObjectSet<Rank> rankObjectSet;
        public IDbSet<Rank> Ranks
        {
            get
            {
                if (ranks == null)
                {
                    ranks = new List<Rank>() {
                        new Rank() { Id = "1-1", Code = "GB", Tenant = 1},
                        new Rank() { Id = "1-2", Code = "US",  Tenant = 2 } };
                    rankObjectSet = new MockObjectSet<Rank>(ranks);
                }
                return rankObjectSet;
            }
        }

        List<CustomerTeam> customerTeams;
        MockObjectSet<CustomerTeam> customerTeamObjectSet;
        public IDbSet<CustomerTeam> CustomerTeams
        {
            get
            {
                if (customerTeams == null)
                {
                    customerTeams = new List<CustomerTeam>() {
                        new CustomerTeam() { Id = "1-1", Name = "CT1", InActive = false, Tenant = 1},
                        new CustomerTeam() { Id = "1-2", Name = "CT2", InActive = false, Tenant = 2 } };
                    rankObjectSet = new MockObjectSet<Rank>(ranks);
                }
                return customerTeamObjectSet;
            }
        }

        List<CustomerGroup> customerGroups;
        MockObjectSet<CustomerGroup> customerGroupObjectSet;
        public IDbSet<CustomerGroup> CustomerGroups
        {
            get
            {
                if (customerGroups == null)
                {
                    customerGroups = new List<CustomerGroup>() {
                        new CustomerGroup() { Id = "1-1", Name = "General", InActive = false, Tenant = 1}};
                    customerGroupObjectSet = new MockObjectSet<CustomerGroup>(customerGroups);
                }
                return customerGroupObjectSet;
            }
        }


        public IDbSet<User> Users
        {
            get { return new MockObjectSet<User>(new List<User>() { new User() { BranchId = "1-1", DepartmentId = "1-1", Id = "1-3", Tenant = 1,Contact=Contacts.Where(d=>d.Id=="1-3").FirstOrDefault()  },
              
                new User() { BranchId = "1-2", DepartmentId = "1-2", Id = "1-2", Tenant = 2,Contact=Contacts.Where(d=>d.Id=="1-2").FirstOrDefault() ,  },
                
                new User() { BranchId = "1-1", DepartmentId = "1-1", Id = "1-1", Tenant = 1,Contact=Contacts.Where(d=>d.Id=="1-1").FirstOrDefault() } }); }
        }

        List<Department> departments;
        MockObjectSet<Department> departmentObjectSet;
        public IDbSet<Department> Departments
        {
            get
            {
                if (departments == null)
                {
                    departments = new List<Department>() {
                        new Department() { Id = "1-1", Tenant = 1},
                        new Department() { Id = "1-2",  Tenant = 2 , EnglishName="Department" , LocalName="Department" , InActive=false, Notes="Nothing"} };

                    departmentObjectSet = new MockObjectSet<Department>(departments);
                }
                return departmentObjectSet;
            }
        }

        List<Branch> branches;
        MockObjectSet<Branch> branchObjectSet;
        public IDbSet<Branch> Branches
        {
            get
            {
                if (branches == null)
                {
                    branches = new List<Branch>() {
                        new Branch() { Id = "1-1", Tenant = 1 , EnglishName="New Branch"},
                        new Branch() { Id = "1-2",  Tenant = 2  , EnglishName="New Branch"} };

                    branchObjectSet = new MockObjectSet<Branch>(branches);
                }
                return branchObjectSet;
            }
        }

        List<Airline> airlines;
        MockObjectSet<Airline> airlineObjectSet;
        public IDbSet<Airline> Airlines
        {
            get
            {
                if (airlines == null)
                {
                    airlines = new List<Airline>() {
                        new Airline() { Id = "1-1", Tenant = 1 , Card=Cards.Where(d=>d.Id=="1-1").FirstOrDefault() },
                        new Airline() { Id = "1-2",  Tenant = 2 , Card=Cards.Where(d=>d.Id=="1-2").FirstOrDefault()} };

                    airlineObjectSet = new MockObjectSet<Airline>(airlines);
                }
                return airlineObjectSet;
            }
        }

        List<ShippingLine> shippingLines;
        MockObjectSet<ShippingLine> shippingLineObjectSet;
        public IDbSet<ShippingLine> ShippingLines
        {
            get
            {
                if (shippingLines == null)
                {
                    shippingLines = new List<ShippingLine>() {
                        new ShippingLine() { Id = "1-1", Tenant = 1 , Card=Cards.Where(d=>d.Id=="1-1").FirstOrDefault()},
                        new ShippingLine() { Id = "1-2",  Tenant = 2 , Card=Cards.Where(d=>d.Id=="1-2").FirstOrDefault()} };

                    shippingLineObjectSet = new MockObjectSet<ShippingLine>(shippingLines);
                }
                return shippingLineObjectSet;
            }
        }

        List<Trucker> truckers;
        MockObjectSet<Trucker> truckerObjectSet;
        public IDbSet<Trucker> Truckers
        {
            get
            {
                if (truckers == null)
                {
                    truckers = new List<Trucker>() {
                        new Trucker() { Id = "1-1", Tenant = 1 , Card=Cards.Where(d=>d.Id=="1-1").FirstOrDefault() },
                        new Trucker() { Id = "1-2",  Tenant = 2 , Card=Cards.Where(d=>d.Id=="1-2").FirstOrDefault()} };

                    truckerObjectSet = new MockObjectSet<Trucker>(truckers);
                }
                return truckerObjectSet;
            }
        }

        List<Tenant> tenants;
        public IDbSet<Tenant> Tenants
        {
            get
            {
                if (tenants == null)
                {
                    tenants = new List<Tenant>() {
                        new Tenant() { Id = 1 , PasswordPolicy=PasswordPolicies.Where(d=>d.Code=="MEDU").FirstOrDefault(), PasswordPolicyCode="MEDU" , Company="OMG",AccountingSetting=new AccountingSetting(){ Id = 1,}  },
                        new Tenant() { Id = 2 , AddressId="1-2" , Company="AMA",AccountingSetting=new AccountingSetting(){ Id = 2,IsVatNumberMandatoryInAP = true,IsARInvoiceChronologicalDates = true, IsVatNumberMandatoryInAR = true,}} };
                }
                return new MockObjectSet<Tenant>(tenants);
            }
        }

        List<Currency> currencies;
        MockObjectSet<Currency> currencyObjctSet;
        public IDbSet<Currency> Currencies
        {
            get
            {
                if (currencies == null)
                {
                    currencies = new List<Currency>() {
                new Currency() { Id = "1-1", Code = "USD", EnglishName = "Us Dollar", Tenant = 1, AddedManually=true },
                new Currency() { Id = "1-2", Code = "YEN", EnglishName = "", Tenant = 2, },
                new Currency() { Id = "1-3", Code = "NIS", EnglishName = "", Tenant = 3, },
                    };

                    currencyObjctSet = new MockObjectSet<Currency>(currencies);
                }
                return currencyObjctSet;
            }
        }

        List<ContactTenant> contactTenants;
        MockObjectSet<ContactTenant> contactTenantObjctSet;
        public IDbSet<ContactTenant> ContactTenants
        {
            get
            {
                if (contactTenants == null)
                {
                    contactTenants = new List<ContactTenant>() {
                new ContactTenant() { ContactId = "1-1" ,TenantId = 1 },
                new ContactTenant() { ContactId = "1-2", TenantId = 2 },
                new ContactTenant() { ContactId = "1-3",  TenantId = 3 },
                    };

                    contactTenantObjctSet = new MockObjectSet<ContactTenant>(contactTenants);
                }
                return contactTenantObjctSet;
            }
        }

        List<ContactTenantRole> contactTenantRoles;
        MockObjectSet<ContactTenantRole> contactTenantRoleObjctSet;
        public IDbSet<ContactTenantRole> ContactTenantRoles
        {
            get
            {
                if (contactTenantRoles == null)
                {
                    contactTenantRoles = new List<ContactTenantRole>() {
                new ContactTenantRole() {Id="1-1" , Tenant=1 , ContactTenant=ContactTenants.Where(d=>d.Id=="1-1").FirstOrDefault() , ContactTenantId="1-1" },
              
                    };

                    contactTenantRoleObjctSet = new MockObjectSet<ContactTenantRole>(contactTenantRoles);
                }
                return contactTenantRoleObjctSet;
            }
        }

        public IDbSet<Role> Roles
        {
            get { throw new NotImplementedException(); }
        }

        public IDbSet<PaymentTerm> PaymentTerms
        {
            get { throw new NotImplementedException(); }
        }

        List<VatType> vatTypes;
        MockObjectSet<VatType> vatTypesObjectSet;
        public IDbSet<VatType> VatTypes
        {
            get
            {
                if (vatTypes == null)
                {
                    vatTypes = new List<VatType>() {
                        new VatType() { Id = "1-1", Tenant = 1 },
                        new VatType() { Id = "1-2", Tenant = 2 } };
                    vatTypesObjectSet = new MockObjectSet<VatType>(vatTypes);
                }
                return vatTypesObjectSet;
            }
        }
        List<Incoterm> incoterms;
        MockObjectSet<Incoterm> incotermObjectSet;
        public IDbSet<Incoterm> Incoterms
        {
            get
            {
                if (incoterms == null)
                {
                    incoterms = new List<Incoterm>() {
                        new Incoterm() { Id = "1-1", Tenant = 1 },
                        new Incoterm() { Id = "1-2", Tenant = 2 } };
                    incotermObjectSet = new MockObjectSet<Incoterm>(incoterms);
                }
                return incotermObjectSet;
            }
        }

        List<ChargesType> chargesTypes;
        MockObjectSet<ChargesType> chargesTypeObjectSet;
        public IDbSet<ChargesType> ChargesTypes
        {
            get
            {
                if (chargesTypes == null)
                {
                    chargesTypes = new List<ChargesType>() {
                        new ChargesType() { Id = "1-1", Tenant = 1, MeasurementId= "1-1" , Code="CT", ContainerMeasurementId= "1-2" , Measurement=Measurements.Where(d=>d.Id=="1-1").FirstOrDefault() , EnglishName= "AirFreight" , LocalName="AirFreight" , VatTypeId="1-1" , ReceivableAccountId="1-1" , PayableAccountId="1-1" }};
                        //new ChargesType() { Id = "1-2", Tenant = 2 , MeasurementId= "1-1" , ContainerMeasurementId= "1-2" , Measurement=this.measurments.Where(d=>d.Id=="1-1").FirstOrDefault() , EnglishName= "AirFreight" , LocalName="AirFreight" , VatTypeId="1-1" , ReceivableAccountId="1-1" , PayableAccountId="1-1" } };
                    chargesTypeObjectSet = new MockObjectSet<ChargesType>(chargesTypes);
                }
                return chargesTypeObjectSet;
            }
        }
                
        List<Measurement> measurments;
        MockObjectSet<Measurement> measurmentObjectSet;
        public IDbSet<Measurement> Measurements
        {
            get
            {
                if (measurments == null)
                {
                    measurments = new List<Measurement>() {
                        new Measurement() { Id = "1-1", Tenant = 1, Code="AC" , ShortName="Meas" },
                        new Measurement() { Id = "1-2", Tenant = 2, Code="AS" , ShortName="NOTH" }
                    };
                    measurmentObjectSet = new MockObjectSet<Measurement>(measurments);
                }
                return measurmentObjectSet;
            }
        }

        public IDbSet<PrepaidCollect> PrepaidCollects
        {
            get { throw new NotImplementedException(); }
        }

        public IDbSet<EntityDate> EntityDates
        {
            get { throw new NotImplementedException(); }
        }

        List<Document> documents;
        MockObjectSet<Document> documentObjectSet;
        public IDbSet<Document> Documents
        {
            get
            {
                if (documents == null)
                {
                    documents = new List<Document>() {
                        new Document() { Id = "1-1", Tenant = 1  },
                        new Document() { Id = "1-2", Tenant = 2 } };
                    documentObjectSet = new MockObjectSet<Document>(documents);
                }
                return documentObjectSet;
            }
        }

        List<DocumentType> documentTypes;
        public IDbSet<DocumentType> DocumentTypes
        {
            get
            {
                if (documentTypes == null)
                {
                    documentTypes = new List<DocumentType>() {
                        new DocumentType() { Id = "1-1", Tenant = 1 , Name= "docs" , DocumentTypeDefaultReportTemplateId= "1-1" , DocumentTypeDefaultHTMLTemplateId= "1-1"   , DocumentTypeCategoryCode = "O" },
                        new DocumentType() { Id = "1-2", Tenant = 2 } };
                }
                return new MockObjectSet<DocumentType>(documentTypes);
            }
        }

        //List<DocumentIn> documentIns;
        //MockObjectSet<DocumentIn> documentInObjectSet;
        //public IDbSet<DocumentIn> DocumentIns
        //{
        //    get
        //    {
        //        if (documentIns == null)
        //        {
        //            documentIns = new List<DocumentIn>() {
        //                new DocumentIn() { Id = "1-1", Tenant = 1 , DocumentTypeId= "1-1" },
        //                new DocumentIn() { Id = "1-2", Tenant =2 } };
        //            documentInObjectSet = new MockObjectSet<DocumentIn>(documentIns);
        //        }
        //        return documentInObjectSet;
        //    }
        //}

        List<DocumentOut> documentsOut;
        MockObjectSet<DocumentOut> documentOutObjectSet;
        public IDbSet<DocumentOut> DocumentOuts
        {
            get
            {
                if (documentsOut == null)
                {
                    documentsOut = new List<DocumentOut>() {
               //         new DocumentOut() { Id = "1-1", Tenant = 1  , IssuedByUserId = "1-1" , DocumentTypeId = "1-1" , DocumentTemplateId ="1-1", 
                          
               //ChildEntityId = "1-1",
               //ChildEntityReference = "1-1",
               //Issued = true,
               //ObjectTableId ="1-2",
               //EntityId = "1-3",
               //Note = "nothing",
               //EmailTemplateId = "1-3",
               //XamlDocumentId = "1-3",
               //NeedsRebuild = false,
               //IsDuplex=true,},
                };
                    documentOutObjectSet = new MockObjectSet<DocumentOut>(documentsOut);

                }
                return documentOutObjectSet;
            }
        }

        List<PackageType> packageTypes;
        MockObjectSet<PackageType> packageTypeObjectSet;
        public IDbSet<PackageType> PackageTypes
        {
            get
            {
                if (packageTypes == null)
                {
                    packageTypes = new List<PackageType>() {
                        new PackageType() { Id = "1-1", Tenant = 1  },
                        new PackageType() { Id = "1-2", Tenant = 2 } };
                    packageTypeObjectSet = new MockObjectSet<PackageType>(packageTypes);
                }
                return packageTypeObjectSet;
            }
        }

        List<DocumentTypeCustomField> documentTypeCustomFields;
        MockObjectSet<DocumentTypeCustomField> documentTypeCustomFieldObjectSet;
        public IDbSet<DocumentTypeCustomField> DocumentTypeCustomFields
        {
            get
            {
                if (documentTypeCustomFields == null)
                {
                    documentTypeCustomFields = new List<DocumentTypeCustomField>() {
                        new DocumentTypeCustomField() { Id = "1-1", Tenant = 1  },
                        new DocumentTypeCustomField() { Id = "1-2", Tenant = 2 } };

                    documentTypeCustomFieldObjectSet = new MockObjectSet<DocumentTypeCustomField>(documentTypeCustomFields);
                }
                return documentTypeCustomFieldObjectSet;
            }
        }

        List<FormCustomField> formCustomFields;
        MockObjectSet<FormCustomField> formCustomFieldObjectSet;
        public IDbSet<FormCustomField> FormCustomFields
        {
            get
            {
                if (formCustomFields == null)
                {
                    formCustomFields = new List<FormCustomField>() {
                        new FormCustomField() { Id = "1-1", Tenant = 1  },
                        new FormCustomField() { Id = "1-2", Tenant = 2 } };
                    formCustomFieldObjectSet = new MockObjectSet<FormCustomField>(formCustomFields);
                }
                return formCustomFieldObjectSet;
            }
        }

        public IDbSet<FieldDataType> FieldDataTypes
        {
            get { throw new NotImplementedException(); }
        }

        List<WeightUnit> weightUnits;
        public IDbSet<WeightUnit> WeightUnits
        {
            get
            {
                if (weightUnits == null)
                {
                    weightUnits = new List<WeightUnit>() {
                        new WeightUnit() { Code= "1-1" },
                        new WeightUnit() { Code = "1-2"} };
                }
                return new MockObjectSet<WeightUnit>(weightUnits);
            }
        }

        List<DimensionsUnit> dimensionsUnits;
        public IDbSet<DimensionsUnit> DimensionsUnits
        {
            get
            {
                if (dimensionsUnits == null)
                {
                    //dimensionsUnits = new List<DimensionsUnit>() {
                    //    new DimensionsUnit() { Code= "RD" , Tenants=Tenants.Where(d=>d.Id==1).ToList()},
                    //    new DimensionsUnit() { Code = "TY"} };
                }
                return new MockObjectSet<DimensionsUnit>(dimensionsUnits);
            }
        }

        List<RateClass> rateClasses;
        public IDbSet<RateClass> RateClasses
        {
            get
            {
                if (rateClasses == null)
                {
                    rateClasses = new List<RateClass>() {
                        new RateClass() { Code= "QW" },
                        new RateClass() { Code = "TY"} };
                }
                return new MockObjectSet<RateClass>(rateClasses);
            }
        }

        List<CommunicationLog> communicationlogs;
        MockObjectSet<CommunicationLog> communicationlogObjectSet;
        public IDbSet<CommunicationLog> CommunicationLogs
        {
            get
            {
                if (communicationlogs == null)
                {
                   MockWebFreightContext MockWebFreightContext = new MockWebFreightContext();
                    communicationlogs = new List<CommunicationLog>() {
                        new CommunicationLog() { Id="1-1" , Tenant= 1 , CommunicationStatusTypeCode = "AB",CommunicationStatusType= CommunicationStatusTypes.Where(d=>d.Code=="AB").FirstOrDefault() , CommunicationLogTypeCode="BB" , 
                            CommunicationLogType=CommunicationLogTypes.Where(d=>d.Code=="BB").FirstOrDefault() , 
                            CreatedByUser=Users.Where(d=>d.Id=="1-1").FirstOrDefault() , 
                            ObjectTableId="1-1",ObjectTable=MockWebFreightContext.ObjectTables.Where(d=>d.Id=="1-1").FirstOrDefault() , Subject="S" },


                        new CommunicationLog() {Id="1-2", Tenant=2} };
                    communicationlogObjectSet = new MockObjectSet<CommunicationLog>(communicationlogs);
                }
                return communicationlogObjectSet;
            }
        }

        List<CommunicationAttachment> communicationAttachments;
        MockObjectSet<CommunicationAttachment> communicationAttachmentObjectSet;
        public IDbSet<CommunicationAttachment> CommunicationAttachments
        {
            get
            {
                if (communicationAttachments == null)
                {
                    communicationAttachments = new List<CommunicationAttachment>() {
                        new CommunicationAttachment() { Id= "1-1" , Tenant=1},
                        new CommunicationAttachment() { Id = "1-2" , Tenant=2} };
                    communicationAttachmentObjectSet = new MockObjectSet<CommunicationAttachment>(communicationAttachments);
                }
                return communicationAttachmentObjectSet;
            }
        }

        List<DueType> dueTypes;
        public IDbSet<DueType> DueTypes
        {
            get
            {
                if (dueTypes == null)
                {
                    dueTypes = new List<DueType>() {
                        new DueType() { Code= "AC" },
                        new DueType() { Code = "AB" } };
                }
                return new MockObjectSet<DueType>(dueTypes);
            }
        }

        List<QuoteGroupSection> quoteGroupSections;
        public IDbSet<QuoteGroupSection> QuoteGroupSections
        {
            get
            {
                if (quoteGroupSections == null)
                {
                    quoteGroupSections = new List<QuoteGroupSection>() {
                        new QuoteGroupSection() { Code= "O" },
                        new QuoteGroupSection() { Code = "F" },
                        new QuoteGroupSection() { Code= "D" },
                    };
                }
                return new MockObjectSet<QuoteGroupSection>(quoteGroupSections);
            }
        }

        List<CommunicationStatusType> communicationStatusTypes;
        public IDbSet<CommunicationStatusType> CommunicationStatusTypes
        {
            get
            {
                if (communicationStatusTypes == null)
                {
                    communicationStatusTypes = new List<CommunicationStatusType>() {
                        new CommunicationStatusType() { Code= "CS"  },
                        new CommunicationStatusType() { Code = "AB" } };
                }
                return new MockObjectSet<CommunicationStatusType>(communicationStatusTypes);
            }
        }

        List<CommunicationLogType> communicationLogTypes;
        public IDbSet<CommunicationLogType> CommunicationLogTypes
        {
            get
            {
                if (communicationLogTypes == null)
                {
                    communicationLogTypes = new List<CommunicationLogType>() {
                        new CommunicationLogType() { Code= "BB"},
                        new CommunicationLogType() { Code = "CB" } };
                }
                return new MockObjectSet<CommunicationLogType>(communicationLogTypes);
            }
        }

        List<WarehouseType> warehouseTypes;
        public IDbSet<WarehouseType> WarehouseTypes
        {
            get
            {
                if (warehouseTypes == null)
                {
                    warehouseTypes = new List<WarehouseType>() {
                        new WarehouseType() { Code= "TM"},
                        new WarehouseType() { Code = "BO" } };
                }
                return new MockObjectSet<WarehouseType>(warehouseTypes);
            }
        }


        List<DocumentTypeTemplate> documentTypeTaplates;
        MockObjectSet<DocumentTypeTemplate> documentTypeTeplampteObjectSet;
        public IDbSet<DocumentTypeTemplate> DocumentTypeTemplates
        {
            get
            {
                if (documentTypeTaplates == null)
                {
                    documentTypeTaplates = new List<DocumentTypeTemplate>() {
                        new DocumentTypeTemplate() { Id ="1-1" , Tenant= 1 , EditorTool= "5" , DocumentTypeId= "1-1" , LastUpdatedByUserId="1-2" , DocumentType=DocumentTypes.Where(d=>d.Id=="1-1").FirstOrDefault() , LastUpdatedByUser=Users.Where(d=>d.Id=="1-2").FirstOrDefault()}};
                    documentTypeTeplampteObjectSet = new MockObjectSet<DocumentTypeTemplate>(documentTypeTaplates);
                }
                return documentTypeTeplampteObjectSet;
            }
        }

        List<TemplateFormat> templateFormats;
        public IDbSet<TemplateFormat> TemplateFormats
        {
            get
            {
                if (templateFormats == null)
                {
                    templateFormats = new List<TemplateFormat>() 
                { new TemplateFormat() { Code = "HT" },
                 new TemplateFormat() {  Code = "JFK" } };
                }
                return new MockObjectSet<TemplateFormat>(templateFormats);
            }

        }

        List<Warehouse> warehouses;
        MockObjectSet<Warehouse> warehouseObjectSet;
        public IDbSet<Warehouse> Warehouses
        {
            get
            {
                if (warehouses == null)
                {
                    warehouses = new List<Warehouse>() 
                { new Warehouse() { Id = "1-1",Tenant = 1 , Card=Cards.Where(d=>d.Id=="1-1").FirstOrDefault() },
                 new Warehouse() { Id = "1-2", Tenant = 2 ,  Card=Cards.Where(d=>d.Id=="1-2").FirstOrDefault() } };


                    warehouseObjectSet = new MockObjectSet<Warehouse>(warehouses);
                }
                return warehouseObjectSet;
            }
        }
        List<Vessel> vessels;
        MockObjectSet<Vessel> vesselObjectSet;
        public IDbSet<Vessel> Vessels
        {
            get
            {
                if (vessels == null)
                {
                    vessels = new List<Vessel>() 
                { new Vessel() { Id = "1-1", Code = "HT", EnglishName = "Hethrow", Tenant = 1 },
                 new Vessel() { Id = "1-2", Code = "JFK", EnglishName = "jfk", Tenant = 2 } };
              

                  vesselObjectSet = new MockObjectSet<Vessel>(vessels);
                }
                return vesselObjectSet;
        }
        }
        



        List<MAWBStack> mAWBStacks;
        MockObjectSet<MAWBStack> mAWBStackObjectSet;
        public IDbSet<MAWBStack> MAWBStacks
        {
            get
            {
                if (mAWBStacks == null)
                {
                    mAWBStacks = new List<MAWBStack>() 
                { new MAWBStack() { Id = "1-1", Tenant = 1 },
                 new MAWBStack() { Id = "1-2",Tenant = 2 } };


                    mAWBStackObjectSet = new MockObjectSet<MAWBStack>(mAWBStacks);
                }
                return mAWBStackObjectSet;
            }
        }

        public IDbSet<DocumentTypeCopy> DocumentTypeCopies
        {
            get { throw new NotImplementedException(); }
        }

        public IDbSet<DocumentOutCopy> DocumentOutCopies
        {
            get { return new MockObjectSet<DocumentOutCopy>(new List<DocumentOutCopy>()); }
        }

        public IDbSet<Feature> Features
        {
            get { throw new NotImplementedException(); }
        }

        public IDbSet<RoleFeature> RoleFeatures
        {
            get { throw new NotImplementedException(); }
        }

        public IDbSet<Restriction> Restrictions
        {
            get { throw new NotImplementedException(); }
        }
        List<VatTypePercentage> vatTypePercentages;
        public IDbSet<VatTypePercentage> VatTypePercentages
        {
            get
            {
                if (vatTypePercentages == null)
                {
                    vatTypePercentages = new List<VatTypePercentage>() {
                        new VatTypePercentage() { Id = "1-1", Tenant = 1 },
                        new VatTypePercentage() { Id = "1-2", Tenant = 2 } };
                }
                return new MockObjectSet<VatTypePercentage>(vatTypePercentages);
            }
        }

        List<TarrifHeader> tarrifHeaders;
        MockObjectSet<TarrifHeader> tarrifHeaderObjectSet;
        public IDbSet<TarrifHeader> TarrifHeaders
        {
            get
            {
                if (tarrifHeaders == null)
                {
                    tarrifHeaders = new List<TarrifHeader>() {
                        new TarrifHeader() { Id = "1-1", Tenant = 1 },
                        new TarrifHeader() { Id = "1-2", Tenant = 2 } };
                    tarrifHeaderObjectSet = new MockObjectSet<TarrifHeader>(tarrifHeaders);
                }
                return tarrifHeaderObjectSet;
            }
        }

        List<TarrifType> tarrifTypes;
        MockObjectSet<TarrifType> tarrifTypeObjectSet;
        public IDbSet<TarrifType> TarrifTypes
        {
            get
            {

                if (tarrifTypes == null)
                {
                    tarrifTypes = new List<TarrifType>() {
                        new TarrifType() { Code="1-1" },
                        new TarrifType() {Code="1-1"} };

                    tarrifTypeObjectSet = new MockObjectSet<TarrifType>(tarrifTypes);
                }

                return tarrifTypeObjectSet;
            }
        }

        List<TarrifCharge> tarrifCharges;
        MockObjectSet<TarrifCharge> tarrifChargeObjectSet;
        public IDbSet<TarrifCharge> TarrifCharges
        {
            get
            {
                if (tarrifCharges == null)
                {
                    tarrifCharges = new List<TarrifCharge>() {
                        new TarrifCharge() { Id = "1-1", Tenant = 1 , ChargesType=ChargesTypes.Where(d=>d.Id=="1-1").FirstOrDefault(), Currency=Currencies.Where(d=>d.Id=="1-1").FirstOrDefault() , Measurement=Measurements.Where(d=>d.Id=="1-1").FirstOrDefault(), MeasurementId="1-1", CurrencyId="1-1", ChargesTypeId="1-1" },
                        new TarrifCharge() { Id = "1-2", Tenant = 2 } };
                    tarrifChargeObjectSet = new MockObjectSet<TarrifCharge>(tarrifCharges);
                }
                return tarrifChargeObjectSet;
            }
        }

        List<TarrifFromTo> tarrifFromTo;
        //MockObjectSet<TarrifFromTo> tarrifFromToObjectSet;
        public IDbSet<TarrifFromTo> TarrifFromToes
        {
            get {
                if (tarrifFromTo == null)
                {
                    tarrifFromTo = new List<TarrifFromTo>(){
                        new TarrifFromTo() {Id="1-1" , Tenant=1, Country=Countries.Where(d=>d.Id=="1-1").FirstOrDefault(), Port=Ports.Where(d=>d.Id=="1-1").FirstOrDefault(), CountryId="1-1", PortId="1-1"}
                    };
                }
                
                return new MockObjectSet<TarrifFromTo>(tarrifFromTo); }
        }

        List<TarrifFromToType> tarrifFromToType;
        MockObjectSet<TarrifFromToType> tarrifFromToTypeObjectSet;
        public IDbSet<TarrifFromToType> TarrifFromToTypes
        {
            get
            {
                if (tarrifFromToType == null)
                {
                    tarrifFromToType = new List<TarrifFromToType>() {
                        new TarrifFromToType() {Code="TF" },
                        new TarrifFromToType() { Code="TR"} };
                    tarrifFromToTypeObjectSet = new MockObjectSet<TarrifFromToType>(tarrifFromToType);
                }
                return tarrifFromToTypeObjectSet;
            }
        }

        List<TarrifStep> tarrifSteps;
        MockObjectSet<TarrifStep> tarrifStepObjectSet;
        public IDbSet<TarrifStep> TarrifSteps
        {
            get {

                if (tarrifSteps == null)
                {
                    tarrifSteps = new List<TarrifStep>(){
                        new TarrifStep() {Id="1-1" , Tenant=1 }
                    };
                    tarrifStepObjectSet = new MockObjectSet<TarrifStep>(tarrifSteps); 
                }
                
                return tarrifStepObjectSet ; }
        }

        public IDbSet<UserLoginLog> UserLoginLogs
        {
            get { throw new NotImplementedException(); }
        }

        List<PasswordPolicy> passwordPolicies;
        public IDbSet<PasswordPolicy> PasswordPolicies
        {
            get
            {
                if (passwordPolicies == null)
                {
                    passwordPolicies = new List<PasswordPolicy>() {
                        new PasswordPolicy() { Code = "MEDU" },
                        new PasswordPolicy() { Code = "AF" } };
                }
                return new MockObjectSet<PasswordPolicy>(passwordPolicies);
            }
        }
         
        List<Vendor> vendors;
        MockObjectSet<Vendor> vendorObjectSet;
        public IDbSet<Vendor> Vendors
        {
            get
            {
                if (vendors == null)
                {
                    vendors = new List<Vendor>() {
                        new Vendor() {  Id="1-1" , Tenant=1, Card=Cards.Where(d=>d.Id=="1-1").FirstOrDefault() , },
                        new Vendor() {Id="1-2" , Tenant=2, Card=Cards.Where(d=>d.Id=="1-2").FirstOrDefault() } };
                    vendorObjectSet= new MockObjectSet<Vendor>(vendors);
                }
                return vendorObjectSet;
            }
        }

        List<AccountingSetting> accountingSetting;
        public IDbSet<AccountingSetting> AccountingSettings
        {
            get
            {
                if (accountingSetting == null)
                {
                    accountingSetting = new List<AccountingSetting>() {
                        new AccountingSetting() { Id =1 , Tenant=Tenants.Where(d=>d.Id==1).FirstOrDefault() },
                        new AccountingSetting() { Id =2 , Tenant=Tenants.Where(d=>d.Id==2).FirstOrDefault() } };
                }
                return new MockObjectSet<AccountingSetting>(accountingSetting);
            }
        }

        List<AccountingSystem> accountingSystems;
        public IDbSet<AccountingSystem> AccountingSystems
        {
            get
            {
                if (accountingSystems == null)
                {
                    accountingSystems = new List<AccountingSystem>() {
                        new AccountingSystem() { Code = "DF" },
                        new AccountingSystem() { Code = "AF"} };
                }
                return new MockObjectSet<AccountingSystem>(accountingSystems);
            }
        }

        public IDbSet<FeatureType> FeatureTypes
        {
            get { throw new NotImplementedException(); }
        }

        public IDbSet<RoleType> RoleTypes
        {
            get { throw new NotImplementedException(); }
        }
        
        public List<Port> GetPorts()
        {
            return this.Ports.ToList();
        }

        public void SetAsModified(object entity)
        {
            //throw new NotImplementedException();
        }

        public void DetectChanges()
        {
            throw new NotImplementedException();
        }

        public int SaveChanges()
        {
            return 1;
        }

        List<Package> packages;
        public IDbSet<Package> Packages
        {
            get
            {
                if (packages == null)
                {
                    packages = new List<Package>() {
                        new Package() { Code = "DF",/*Tenants=Tenants.Where(d=>d.Id==1).ToList() */ },
                        new Package() { Code = "AF", /*Tenants=Tenants.Where(d=>d.Id==2).ToList()*/ } };
                }
                return new MockObjectSet<Package>(packages);
            }
        }

        public IDbSet<PackageFeature> PackageFeatures
        {
            get { throw new NotImplementedException(); }
        }
        
        public IDbSet<UserLastLogin> UserLastLogins
        {
            get { throw new NotImplementedException(); }
        }


        List<TermsofUse> termOfUses;
        MockObjectSet<TermsofUse> termOfUsesObjectSet;
        public IDbSet<TermsofUse> TermsofUses
        {
            get
            {
                if (termOfUses == null)
                {
                    termOfUses = new List<TermsofUse>() {
                        new TermsofUse() {VersionNumber=1,Date=DateTime.Now.Date, },
                        new TermsofUse() { VersionNumber=2} };
                    termOfUsesObjectSet = new MockObjectSet<TermsofUse>(termOfUses);
                }
                return termOfUsesObjectSet;
            }
        }

        List<TermsofUseSignature> termsofUseSignatures;
        MockObjectSet<TermsofUseSignature> termsofUseSignatureObjectSet;
        public IDbSet<TermsofUseSignature> TermsofUseSignatures
        {
            get
            {
                if (termsofUseSignatures == null)
                {
                    termsofUseSignatures = new List<TermsofUseSignature>() {
                        new TermsofUseSignature() {Id="1-1",Tenant = 1},
                        new TermsofUseSignature() { Id="1-2",Tenant = 1} };
                    termsofUseSignatureObjectSet = new MockObjectSet<TermsofUseSignature>(termsofUseSignatures);
                }
                return termsofUseSignatureObjectSet;
            }
        }
        
        List<ChargeTypeAccounting> chargeTypeAccountings;
        MockObjectSet<ChargeTypeAccounting> chargeTypeAccountingsObjectSet;
        public IDbSet<ChargeTypeAccounting> ChargeTypeAccountings
        {
            get
            {
                if (chargeTypeAccountings == null)
                {
                    chargeTypeAccountings = new List<ChargeTypeAccounting>() {
                        new ChargeTypeAccounting() {Id="1-1",Tenant = 1},
                        new ChargeTypeAccounting() { Id="1-2",Tenant = 1} };
                    chargeTypeAccountingsObjectSet = new MockObjectSet<ChargeTypeAccounting>(chargeTypeAccountings);
                }
                return chargeTypeAccountingsObjectSet;
            }
        }


        public IDbSet<Report> Reports
        {
            get { throw new NotImplementedException(); }
        }
        
        public IDbSet<ContactLastLogin> ContactLastLogins
        {
            get { throw new NotImplementedException(); }
        }

        public IDbSet<ContactLoginLog> ContactLoginLogs
        {
            get { throw new NotImplementedException(); }
        }
        
        public IDbSet<SmallDocument> SmallDocuments
        {
            get { throw new NotImplementedException(); }
        }

        public IDbSet<CommunicationLogStep> CommunicationLogSteps
        {
            get { throw new NotImplementedException(); }
        }
        
        public IDbSet<UserPermittedBranch> UserPermittedBranches
        {
            get { throw new NotImplementedException(); }
        }
        
        public IDbSet<ReportGroup> ReportGroups
        {
            get;
            set;
        }

        public IDbSet<AddressType> AddressTypes
        {
            get { throw new NotImplementedException(); }
        }

        public IDbSet<LeadSource> LeadSources
        {
            get { throw new NotImplementedException(); }
        }

        public IDbSet<Industry> Industries
        {
            get { throw new NotImplementedException(); }
        }
        
        IDbSet<ProductType> ICommonDataContext.ProductTypes
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

        IDbSet<ProductPeriod> ICommonDataContext.ProductPeriods
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

        IDbSet<CustomerProduct> ICommonDataContext.CustomerProducts
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

        IDbSet<ColorIndex> ICommonDataContext.ColorIndexs
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

        IDbSet<DataProvider> ICommonDataContext.DataProviders
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

        /// <summary>
        /// /////
        /// </summary>
        public IDbSet<HybridTenantState> HybridTenantStates
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

        public IDbSet<HybridTenantThreshold> HybridTenantThresholds
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



        public IDbSet<EntityChange> EntityChanges
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

        //public IDbSet<EntityChangesAutomation> EntityChangesAutomations
        //{
        //    get
        //    {
        //        throw new NotImplementedException();
        //    }
        //    set
        //    {
        //        throw new NotImplementedException();
        //    }
        //}



        public IDbSet<Automation> Automations
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






        public IDbSet<AutomationHistory> AutomationHistorys
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




        public IDbSet<AutomationResultEmailRecipient> AutomationResultEmailRecipients
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

        public IDbSet<AutomationLastUpdate> AutomationLastUpdates
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


        




        public IDbSet<CustomMetaDataTypesAddtional> CustomMetaDataTypesAddtionals
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

        public IDbSet<CustomerProductActualData> CustomerProductActualDatas
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

        public IDbSet<CustomerProductLocation> CustomerProductLocations
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

        public IDbSet<CustomerProductLocationActualData> CustomerProductLocationActualDatas
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

        public IDbSet<Competitor> Competitors
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

        public IDbSet<CustomerCompetitor> CustomerCompetitors
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

        public IDbSet<CustomerCompetitorProduct> CustomerCompetitorProducts
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

        public IDbSet<CustomerAdditionalService> CustomerAdditionalServices
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

        public IDbSet<Commodity> Commodities { get; set; }

        public IDbSet<ContactDoneMethod> ContactDoneMethods
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

        IDbSet<AdditionalService> ICommonDataContext.AdditionalServices
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

        public IDbSet<VatUniqueType> VatUniqueTypes
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

        public IDbSet<VatMandatoryType> VatMandatoryTypes
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

        public IDbSet<CustomerSalesNote> CustomerSalesNotes
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

        public IDbSet<AuthenticationToken> AuthenticationTokens
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
        public IDbSet<CustomerStatus> CustomerStatus
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
        public IDbSet<ProductTypeModification> ProductTypeModifications
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
        public IDbSet<CustomerSalesmanByProduct> CustomerSalesmanByProducts
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
        public IDbSet<CustomerAccountManagerByProduct> CustomerAccountManagerByProducts
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
        public IDbSet<CustomerFreelancerByProduct> CustomerFreelancerByProducts
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
        public IDbSet<CustomerCustomsAgentByProduct> CustomerCustomsAgentByProducts
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
        public IDbSet<CustomerForwarderByProduct> CustomerForwarderByProducts
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
        public IDbSet<CustomerMediatorByProduct> CustomerMediatorByProducts
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
        public IDbSet<CardExternalCodeByCurrency> CardExternalCodeByCurrencies
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
        public IDbSet<BusinessUnit> BusinessUnits
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
        public IDbSet<FeatureAccessLevel> FeatureAccessLevels
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
        public IDbSet<EmailProvider> EmailProviders
        {
            get { throw new NotImplementedException(); }
        }
        public IDbSet<Region> Regions
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
        public IDbSet<CustomerSize> CustomerSizes
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
        public IDbSet<CountryCity> CountryCities
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
        public IDbSet<ReportModification> ReportModifications
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
        public IDbSet<ContactsUnseenEntitie> ContactsUnseenEntities
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
        public IDbSet<SharedFollowedShipment> SharedFollowedShipments
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
        public IDbSet<UserPermittedProduct> UserPermittedProducts
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
        public IDbSet<Distributor> Distributors
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
        public IDbSet<DocumentsFiling> DocumentsFilings
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
        public IDbSet<DocumentStatus> DocumentStatus
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
        public IDbSet<DocumentsFilingMetaDataValue> DocumentsFilingMetaDataValues
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
        public IDbSet<DocumentTypeMetaData> DocumentTypeMetaDatas
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
        public IDbSet<DocumentsDataProvider> DocumentsDataProviders
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
        public IDbSet<DocumentsMetaDataType> DocumentsMetaDataTypes
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
        public IDbSet<ComputingPartner> ComputingPartners
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
        public IDbSet<ComputingPartnerCode> ComputingPartnerCodes
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
        public IDbSet<ComputingPartnerTable> ComputingPartnerTables
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
        public IDbSet<ComputingPartnerTranslation> ComputingPartnerTranslations
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
        public IDbSet<DocumentFolder> DocumentFolders
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
        public IDbSet<DocumentTypeCategory> DocumentTypeCategories
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
        public IDbSet<CustomerTenantAccessStatusType> CustomerTenantAccessStatusTypes
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
        public IDbSet<CustomerTenantAccess> CustomerTenantAccesses
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
        public IDbSet<CustomerTenantAccessCard> CustomerTenantAccessCards
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
        public IDbSet<CustomerTenantAccessRequest> CustomerTenantAccessRequests
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
        public IDbSet<HybridPartner> HybridPartners
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
        public IDbSet<CustomerTenantAccessCardsBatch> CustomerTenantAccessCardsBatches
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
        public IDbSet<AirlineStatistics> AirlineStatistics
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
        IDbSet<Participant> ICommonDataContext.Participants
        {
            get { throw new NotImplementedException(); }
        }
        public IDbSet<AWBDescriptionOfGoods> AWBDescriptionOfGoods
        {
            get { throw new NotImplementedException(); }
        }
        public IDbSet<VatFormatType> VatFormatTypes
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
        public IDbSet<LogitudeMessagesTransmissionLog> LogitudeMessagesTransmissionLogs
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
        public IDbSet<FeaturePackageType> FeaturePackageTypes
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
        public IDbSet<PackageConnectedPackage> PackageConnectedPackages
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
        public IDbSet<UserLicense> UserLicenses
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
        public IDbSet<BlobFile> BlobFiles
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
        public IDbSet<AgentSharedManifest> AgentSharedManifests
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
        public IDbSet<SharedManifestsStatus> SharedManifestsStatuses
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
        public IDbSet<EntityCasualData> EntityCasualDatas
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
        public IDbSet<AirlineMessagingRule> AirlineMessagingRules
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
        public IDbSet<PaymentTermDateType> PaymentTermDateTypes
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
        public IDbSet<CustomerFieldsUpdateSetting> CustomerFieldsUpdateSettings
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
        public IDbSet<CreditLimitSetting> CreditLimitSettings
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
        public IDbSet<SharedManifestTranslation> SharedManifestTranslations
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
        public IDbSet<TenantAdditionalData> TenantAdditionalDatas
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
        public IDbSet<CardExternalAccountsByProduct> CardExternalAccountsByProducts
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
        public IDbSet<VATTypesGroup> VATTypesGroups
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
        public IDbSet<CustomsInterface> CustomsInterfaces
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
        public IDbSet<CustomsInterfaceSetting> CustomsInterfaceSettings
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
        public IDbSet<TwoFactorAuthenticationDevice> TwoFactorAuthenticationDevices
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
        public IDbSet<TenantLoginPolicy> TenantLoginPolicies
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
        public IDbSet<LoginPolicy> LoginPolicies
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
        public IDbSet<FTPDetail> FTPDetails
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
        public IDbSet<AgentSharedDocument> AgentSharedDocuments
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
        public IDbSet<MetodoPago> MetodoPagos
        {
            get
            {
                throw new NotImplementedException();
            }
        }
        public IDbSet<ChargesExternalAccountsByProduct> ChargesExternalAccountsByProducts
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
        public IDbSet<RegistryDateType> RegistryDateTypes
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
        public IDbSet<UsoCFDI> UsoCFDIs
        {
            get
            {
                throw new NotImplementedException();
            }
        }
        public IDbSet<RegimenFiscal> RegimenFiscals
        {
            get
            {
                throw new NotImplementedException();
            }
        }
        public IDbSet<PostalCode> PostalCodes
        {
            get
            {
                throw new NotImplementedException();
            }
        }
        public IDbSet<ReportsTemplate> ReportsTemplates
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
        public IDbSet<ReportsTemplatesVersion> ReportsTemplatesVersions
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
        public IDbSet<FeatureChange> FeatureChanges
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
        public IDbSet<FilingInbox> FilingInboxes
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
        public IDbSet<FilingInboxAttachment> FilingInboxAttachments
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
        public IDbSet<FilingInboxAttachmentLog> FilingInboxAttachmentLogs
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
        public IDbSet<ReportExecutionLog> ReportExecutionLogs
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
        public IDbSet<INTTRASetting> INTTRASettings
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
        public IDbSet<INTTRASettingMode> INTTRASettingModes
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
        public IDbSet<INTTRABranchRegisteredCarrier> INTTRABranchRegisteredCarriers
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
        public IDbSet<TemperatureUnit> TemperatureUnits
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

        public IDbSet<DocumentFilingBackupBatch> DocumentFilingBackupBatches
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

        public IDbSet<DocumentFilingBackupSetting> DocumentFilingBackupSettings
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

        public IDbSet<HybridPartnersPermission> HybridPartnersPermissions
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

        public IDbSet<DWHSetting> DWHSettings
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


        public IDbSet<DWHBuildStatus> DWHBuildStatus
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



  
        public IDbSet<PaymentGatewayPartner> PaymentGatewayPartners
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

        public IDbSet<CustomsShipper> CustomsShippers
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

        public IDbSet<CustomerDeposition> CustomerDepositions
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


        public IDbSet<NumberFormat> NumberFormats
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

        public IDbSet<UsersReleaseNotesDisplay> UsersReleaseNotesDisplays
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

        public IDbSet<CheckDigitControlAlgorithm> CheckDigitControlAlgorithms
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

        public IDbSet<CarrierArea> CarrierAreas
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

        public IDbSet<CarrierAreasPort> CarrierAreasPorts
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

        public IDbSet<CardContactProduct> CardContactProducts
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

        public IDbSet<LogBoxTenantSetting> LogBoxTenantSettings
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
        public IDbSet<DocumentsExecutionLog> DocumentsExecutionLogs
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
        public IDbSet<AccountingPartner> AccountingPartners
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
        public IDbSet<CardContactAdditionalService> CardContactAdditionalServices
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

        public IDbSet<SharedLogisticsContactLastLogin> SharedLogisticsContactLastLogins
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

        public IDbSet<UserLastSettings> UserLastSettings
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

        public IDbSet<CustomerOpenFilesAmount> CustomerOpenFilesAmounts
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

        public IDbSet<ProductItem> ProductItems
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
        public IDbSet<HTSCode> HTSCodes
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
        public IDbSet<CargoTenantMilestoneDefinition> CargoTenantMilestoneDefinitions
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

        public IDbSet<TariffCarrierTranslation> TariffCarrierTranslations => throw new NotImplementedException();

        public IDbSet<VatUniquePartnerType> VatUniquePartnerTypes { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IDbSet<WarehouseWeightMeasurement> WarehouseWeightMeasurements { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IDbSet<WarehouseWeightRounding> WarehouseWeightRoundings { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IDbSet<WarehouseStoragePricing> WarehouseStoragePricings { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IDbSet<CardSearch> CardSearches { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IDbSet<Horse> Horses { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IDbSet<DWHEnvironmentSetting> DWHEnvironmentSettings { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IDbSet<PortTimeZone> PortTimeZones => throw new NotImplementedException();
        public IDbSet<UnassignedEntity> UnassignedEntitys => throw new NotImplementedException();

        IDbSet<UnassignedEntity> ICommonDataContext.UnassignedEntitys { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IDbSet<Mention> Mentions { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IDbSet<CarrierServiceLine> CarrierServiceLines { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IDbSet<HorseGender> HorseGenders { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IDbSet<DigitalContactLastSetting> DigitalContactLastSettings { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IDbSet<PortGroup> PortGroups { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IDbSet<CustomFieldsMainObject> CustomFieldsMainObjects { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IDbSet<AllActiveGLAccountsView> AllActiveGLAccountsViews
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

        public IDbSet<FreelancerGroupType> FreelancerGroupTypes
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

        public IDbSet<UserFreelancerGroup> UserFreelancerGroups
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

        public IDbSet<TruckerSetting> TruckerSettings
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
        public IDbSet<Responsibility> Responsibilities
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



        //public IDbSet<CardContactAdditionalService> CardContactAdditionalServices { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        //public IDbSet<UsersReleaseNotesDisplay>  { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}