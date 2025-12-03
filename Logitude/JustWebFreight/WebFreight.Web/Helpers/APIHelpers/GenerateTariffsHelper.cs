using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.BL.ExtendedServices;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.TariffModule.Data;
using Logitude.TariffModule.Data.EntityPOCOs;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs; 
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Transactions;
using System.Web;
using System.Xml.Serialization;

namespace WebFreight.Web.Helpers.APIHelpers
{
    public class GenerateTariffsHelper : BatchTaskExecutionsService
    {
        public GenerateTariffsHelper(BatchTaskExecutionPM batchTaskExecution) : base(batchTaskExecution)
        {

        }

        public override void RunCode()
        {
            using (TransactionScope scope = TransactionFactory.GetTransaction(new TimeSpan(3, 0, 0)))
            {
                this.RunPartnersGenerator();
                //this.RunTariffGenerator();
                scope.Complete();
            }
        }

        private void RunPartnersGenerator()
        {
            string xmlParameters = BatchTaskExecution.PrametersXml;
            System.IO.StringReader stringReader = new System.IO.StringReader(xmlParameters);
            XmlSerializer serializer = new XmlSerializer(typeof(GenerateTariffsArgs));
            GenerateTariffsArgs parameterArgs = serializer.Deserialize(stringReader) as GenerateTariffsArgs;
            int tenant = parameterArgs.Tenant;
            var LoggedUserEmail = parameterArgs.LoggedUserEmail;
            ContactRepository contactRep = new ContactRepository(tenant);

            Contact systemContact = contactRep.GetSingleContactByEmail(LoggedUserEmail, tenant);
            ICommonDataContext commonDataContext = CommonDataContext.GetContext(tenant);
            CountryRepository countryRep = new CountryRepository(commonDataContext);
            StateRepository stateRep = new StateRepository(commonDataContext);
            Dictionary<string, State> statesDictionary = stateRep.GetStates(tenant).ToDictionary(d => d.Code + ',' + d.CountryId, o => o);
            Dictionary<string, Country> countrieysDictionary = countryRep.GetCountries(tenant).ToDictionary(d => d.Code, o => o);

            Country country = null;
            if (countrieysDictionary.Keys.Contains("MX"))
            {
                country = countrieysDictionary["MX"];
            }

            State state = null;
            if (country != null)
            {

                if (statesDictionary.Keys.Contains("JAL" + ',' + country.Id))
                {
                    state = statesDictionary["JAL" + ',' + country.Id];
                }
            }

            AddressPM address = new AddressPM()
            {
                Name = "Main Address",
                Description = "Main Address",
                Address1 = "address1",
                Address2 = "address2",
                ZipCode = "zip",
                FaxNumber = "fax",
                AddressTypeId = "M",
                Tenant = tenant,
                StateId = state != null ? state.Id : null,
                CountryId = country != null ? country.Id : null,
                City = "city",
                PhoneNumber = "phoneNumber",
                ContactFax = "fax",

            };
            ContactPM contactPM = new ContactPM()
            {
                Email = "contact@email.com",
                EnglishName = "test contact",
                Tenant = tenant,
                CardId = "newCard",
                IsHybrid = true,
                IsCreatedWithPartner = true,
            };

            for (int i = 1; i <= 200; i++)
            {
                CustomerPM customer = new CustomerPM()
                {
                    EnglishName = "customer " + i,
                    VatNumber = "customervat " + i,
                    Tenant = tenant,
                    IsHybrid = true,
                    Code = TableCounter.DoesCounterDefinitionExist("CADC", tenant, "CS") ? CodeCounter.GetNumber("Customer", tenant).ToString() :TableCounter.GetNumber(tenant, "CADC", "CS", null, null, true),
                    PartnerTypeId = "CS",
                    CustomerStatusCode = "ACT",
                    IsCustomer = true,
                };
                customer.Addresses.Add(address);
                customer.Contacts.Add(contactPM);
                CustomerService service = new CustomerService(commonDataContext, customer, systemContact.Id);
                service.Create();
            }

            for (int i = 1; i <= 100; i++)
            {
                AgentPM agent = new AgentPM()
                {
                    EnglishName = "agent " + i,
                    VatNumber = "agentvat " + i,
                    Tenant = tenant,
                    IsHybrid = true,
                    Code = TableCounter.DoesCounterDefinitionExist("CADC", tenant, "AG") ?  TableCounter.GetNumber(tenant, "CADC", "AG", null, null, true)  : CodeCounter.GetNumber("Agent", tenant).ToString(),
                    PartnerTypeId = "AG",
                };
                agent.Addresses.Add(address);
                agent.Contacts.Add(contactPM);
                AgentService service = new AgentService(commonDataContext, agent, systemContact.Id);
                service.Create(agent);
            }

            for (int i = 1; i <= 200; i++)
            {
                TruckerPM trucker = new TruckerPM()
                {
                    EnglishName = "Trucker " + i,
                    VatNumber = "truckvat " + i,
                    Tenant = tenant,
                    IsHybrid = true,
                    Code = "198" + i,
                    CarrierTypeId = "TR",
                };
                trucker.Addresses.Add(address);
                trucker.Contacts.Add(contactPM);

                TruckerService service = new TruckerService(commonDataContext, trucker, systemContact.Id);
                service.Create(trucker);
            }

            for (int i = 1; i <= 200; i++)
            {
                WarehousePM warehouse = new WarehousePM()
                {
                    EnglishName = "warehouse " + i,
                    VatNumber = "warehousevat " + i,
                    Tenant = tenant,
                    IsHybrid = true,
                    Code = "199" + i,
                    PartnerTypeId = "WH",
                };
                warehouse.Addresses.Add(address);
                warehouse.Contacts.Add(contactPM);
                WarehouseService service = new WarehouseService(commonDataContext, warehouse, systemContact.Id);
                service.Create(warehouse);
            }

            for (int i = 1; i <= 200; i++)
            {
                VendorPM vendor = new VendorPM()
                {
                    EnglishName = "vendor " + i,
                    VatNumber = "vendorvat " + i,
                    Tenant = tenant,
                    IsHybrid = true,
                    Code = TableCounter.DoesCounterDefinitionExist("CADC", tenant, "VD") ?TableCounter.GetNumber(tenant, "CADC", "VD", null, null, true) : CodeCounter.GetNumber("Vendor", tenant).ToString(),
                    PartnerTypeId = "VD",
                };
                vendor.Addresses.Add(address);
                vendor.Contacts.Add(contactPM);
                VendorService service = new VendorService(commonDataContext, vendor, systemContact.Id);
                service.Create(vendor);
            }

            for (int i = 1; i <= 100; i++)
            {
                CustomAgentPM customAgent = new CustomAgentPM()
                {
                    EnglishName = "customAgent " + i,
                    VatNumber = "customAgentvat " + i,
                    Tenant = tenant,
                    IsHybrid = true,
                    Code = TableCounter.DoesCounterDefinitionExist("CADC", tenant, "AG") ? TableCounter.GetNumber(tenant, "CADC", "AG", null, null, true) : CodeCounter.GetNumber("Agent", tenant).ToString(),
                    PartnerTypeId = "CG",
                };
                customAgent.Addresses.Add(address);
                customAgent.Contacts.Add(contactPM);
                CustomAgentService service = new CustomAgentService(commonDataContext, tenant, systemContact);
                service.Create(customAgent);
            }
        }

        private void RunTariffGenerator()
        {
            string xmlParameters = BatchTaskExecution.PrametersXml;
            System.IO.StringReader stringReader = new System.IO.StringReader(xmlParameters);
            XmlSerializer serializer = new XmlSerializer(typeof(GenerateTariffsArgs));
            GenerateTariffsArgs parameterArgs = serializer.Deserialize(stringReader) as GenerateTariffsArgs;

            ITariffModuleContext iContext = TariffModuleContext.GetContext(parameterArgs.Tenant);
            TariffSetting iTariffSetting = (from d in iContext.TariffSettings where d.Tenant == parameterArgs.Tenant select d).FirstOrDefault();

            if (iTariffSetting != null)
            {
                Random random = new Random();
                DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(parameterArgs.Tenant);

                ICommonDataContext context = CommonDataContext.GetContext(parameterArgs.Tenant);

                UserRepository userRepository = new UserRepository(context);
                User loggedUser = userRepository.GetSingleUserByEmail(parameterArgs.LoggedUserEmail, parameterArgs.Tenant, false);

                List<Card> airlines = context.Cards.Where(d => d.Tenant == parameterArgs.Tenant && d.PartnerTypeId == "AL").Take(20).ToList();
                List<string> currencyIds = context.Currencies.Where(d => d.Tenant == parameterArgs.Tenant).Select(s => s.Id).ToList();
                List<Port> ports = context.Ports.Where(d => d.Tenant == parameterArgs.Tenant).ToList();

                var myCount = 0;
                for (int i = 0; i < 20; i++)
                {
                    List<TariffVersion> versions = new List<TariffVersion>();

                    Card airline = airlines[random.Next(airlines.Count)];

                    Tariff tariff = new Tariff()
                    {
                        Id = IdCounter.GetNumber("Tariff", parameterArgs.Tenant),
                        TariffNumber = CodeCounter.GetNumber("Tariff", parameterArgs.Tenant).ToString(),
                        PriceSteps = iTariffSetting.DefaultPriceSteps,
                        Tenant = parameterArgs.Tenant,
                        CreateDate = todayDate,
                        UpdateDate = todayDate,
                        CreatedByUserId = loggedUser.Id,
                        UpdatedByUserId = loggedUser.Id,
                        SellerId = airline.Id,
                        Name = "Test Tariff " + i,
                        StartDate = todayDate.AddMonths(i),
                        ExpirationDate = todayDate.AddYears(1),
                        CurrencyId = currencyIds[random.Next(currencyIds.Count)],
                        LastVersion = 50,
                        TypeCode = "AFC",
                        ConcurrencyGUID = Guid.NewGuid().ToString(),
                        SearchFields = "Test Tariff " + i + "," + airline.EnglishName,
                    };
                    myCount++;

                    TariffVersion draftVersion = new TariffVersion()
                    {
                        TariffId = tariff.Id,
                        Version = 1,
                        IsDraft = true,
                        CreateDate = todayDate,
                        CreatedByUserId = loggedUser.Id,
                        StartDate = todayDate,
                        ExpirationDate = todayDate.AddYears(1),
                        Tenant = parameterArgs.Tenant,
                    };
                    iContext.TariffVersions.Add(draftVersion);
                    myCount++;

                    TariffVersion activeVersion = new TariffVersion()
                    {
                        TariffId = tariff.Id,
                        Version = 2,
                        IsDraft = false,
                        CreateDate = todayDate,
                        CreatedByUserId = loggedUser.Id,
                        StartDate = todayDate.AddMonths(-1),
                        ExpirationDate = todayDate.AddMonths(2),
                        Tenant = parameterArgs.Tenant,
                        ApproveDate = todayDate,
                        ApprovedByUserId = loggedUser.Id,
                        ParentVersionNumber = 1,
                    };
                    iContext.TariffVersions.Add(activeVersion);
                    myCount++;

                    versions.Add(draftVersion);
                    versions.Add(activeVersion);

                    for (int j = 3; j <= 50; j++)
                    {
                        TariffVersion version = new TariffVersion()
                        {
                            TariffId = tariff.Id,
                            Version = j,
                            IsDraft = false,
                            CreateDate = todayDate,
                            CreatedByUserId = loggedUser.Id,
                            StartDate = todayDate.AddMonths(-(j - 1)),
                            ExpirationDate = todayDate.AddMonths(-j),
                            Tenant = parameterArgs.Tenant,
                            ApproveDate = todayDate,
                            ApprovedByUserId = loggedUser.Id,
                            ParentVersionNumber = j - 1,
                        };

                        iContext.TariffVersions.Add(version);
                        myCount++;

                        versions.Add(version);
                    }

                    foreach (TariffVersion item in versions)
                    {
                        for (int k = 0; k < 50; k++)
                        {
                            Port fromPort = ports[random.Next(ports.Count)];
                            Port toPort = ports[random.Next(ports.Count)];
                            string[] steps = iTariffSetting.DefaultPriceSteps.Split(',');

                            TariffLine line = new TariffLine()
                            {
                                Id = IdCounter.GetNumber("TariffLine", parameterArgs.Tenant),
                                Tenant = parameterArgs.Tenant,
                                TariffId = tariff.Id,
                                Version = item.Version,
                                StartDate = item.StartDate,
                                ExpirationDate = item.ExpirationDate,
                                OriginPortId = fromPort.Id,
                                DestinationPortId = toPort.Id,
                                Index = k,
                                LineUniqueKey = fromPort.Code + "," + toPort.Code,
                                LineUniqueKeyText = fromPort.Code + "," + toPort.Code + k,
                                MinPrice = RandomDecimal(random),
                            };

                            for (var s = 1; s <= steps.Length; s++)
                            {
                                PropertyInfo propInfo = typeof(TariffLine).GetProperty("Step" + s + "Price");
                                propInfo.SetValue(line, RandomDecimal(random));
                            }

                            myCount++;
                            iContext.TariffLines.Add(line);
                        }
                    }

                    TariffVersion lastVersion = versions.Where(d => d.Version == 50).FirstOrDefault();
                    tariff.LastStartDate = lastVersion.StartDate;
                    tariff.LastExpirationDate = lastVersion.ExpirationDate;

                    iContext.Tariffs.Add(tariff);
                    airlines.Remove(airline);

                    iContext.SaveChanges();
                }
            }
        }


        private decimal RandomDecimal(Random random)
        {
            int precision = random.Next(1, 3);
            int scale = random.Next(0, precision);

            Decimal d = 0m;
            for (int i = 0; i < precision; i++)
            {
                int r = random.Next(0, 10);
                d = d * 10m + r;
            }

            for (int s = 0; s < scale; s++)
            {
                d /= 10m;
            }

            return d;
        }
    }

    public class GenerateTariffsArgs
    {
        public int Tenant { get; set; }
        public string LoggedUserEmail { get; set; }
    }
}