using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.BL.ExtendedServices;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.StorageService;
using Logitude.TariffModule.Data;
using Logitude.TariffModule.Data.EntityPOCOs;
using Microsoft.Practices.Unity;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs; 
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Xml.Serialization;
using WebFreight.Web.Controllers.WebDomainControllers;

namespace WebFreight.Web.Helpers.APIHelpers
{
    public class TariffGeneratorFromExcel : BatchTaskExecutionsService
    {
        public TariffGeneratorFromExcel(BatchTaskExecutionPM batchTaskExecution) : base(batchTaskExecution)
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

            for (int i = 1; i <= 1000; i++)
            {
                CustomerPM customer = new CustomerPM()
                {
                    EnglishName = "customer " + i,
                    VatNumber = "customervat " + i,
                    Tenant = tenant,
                    IsHybrid = true,
                    Code = TableCounter.DoesCounterDefinitionExist("CADC", tenant, "CS") ? TableCounter.GetNumber(tenant, "CADC", "CS", null, null, true) :  CodeCounter.GetNumber("Customer", tenant).ToString(),
                    PartnerTypeId = "CS",
                    CustomerStatusCode = "ACT",
                    IsCustomer = true,
                };
                customer.Addresses.Add(address);
                customer.Contacts.Add(contactPM);
                CustomerService service = new CustomerService(commonDataContext, customer, systemContact.Id);
                service.Create();
            }
        }

        private void RunTariffGenerator()
        {
            string xmlParameters = BatchTaskExecution.PrametersXml;
            System.IO.StringReader stringReader = new System.IO.StringReader(xmlParameters);
            XmlSerializer serializer = new XmlSerializer(typeof(TariffsExcelGeneratorArgs));
            TariffsExcelGeneratorArgs parameterArgs = serializer.Deserialize(stringReader) as TariffsExcelGeneratorArgs;
            ITariffModuleContext iContext = TariffModuleContext.GetContext(parameterArgs.Tenant);
            TariffSetting iTariffSetting = (from d in iContext.TariffSettings where d.Tenant == parameterArgs.Tenant select d).FirstOrDefault();
            TariffDomainController tariffDomainController = new TariffDomainController();
            if (iTariffSetting != null)
            {
                List<ExcelTariffLines> tariffLinesResult = new List<ExcelTariffLines>();
                byte[] fileData = null;
                DocumentRepository documentRepository = new DocumentRepository(parameterArgs.Tenant);
                Document document = documentRepository.GetSingleDocument(parameterArgs.Tenant, parameterArgs.DocumentId);
                if (document != null)
                {
                    IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
                    BlobFileInfo fileInfo = new BlobFileInfo()
                    {
                        FileName = document.Id,
                        FolderName = document.Folder,
                        Extension = document.Extension,
                        Tenant = parameterArgs.Tenant,
                        FileSize = document.FileSize,
                    };
                    fileData = storageservice.Read(fileInfo);
                }

                System.IO.MemoryStream stream = new System.IO.MemoryStream(fileData);
                ExcelEngine excelEngine = new ExcelEngine();
                IApplication application = excelEngine.Excel;
                IWorkbook workbook = excelEngine.Excel.Workbooks.Open(stream);
                IWorksheet sheet = workbook.Worksheets[0];
                tariffLinesResult = tariffDomainController.BuildOceanAirFreightCostExcelLines(sheet, parameterArgs.Tenant);
                this.GenerateExcel(tariffLinesResult, parameterArgs.LoggedUserEmail, parameterArgs.Tenant);
            }
        }

        private void GenerateExcel(List<ExcelTariffLines> tariffLines, string loggedUserEmail, int tenant)
        {
            ITariffModuleContext iTariffModuleContext = TariffModuleContext.GetContext(tenant);
            ICommonDataContext iCommonDataContext = CommonDataContext.GetContext(tenant);
            UserRepository userRepository = new UserRepository(iCommonDataContext);
            TariffSetting tariffSetting = (from d in iTariffModuleContext.TariffSettings where d.Tenant == tenant select d).FirstOrDefault();

            if (tariffSetting != null)
            {
                Random random = new Random();
                DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                User loggedUser = userRepository.GetSingleUserByEmail(loggedUserEmail, tenant, false);
                List<Card> airlines = iCommonDataContext.Cards.Where(d => d.Tenant == tenant && d.PartnerTypeId == "AL").Take(20).ToList();
                List<string> currencyIds = iCommonDataContext.Currencies.Where(d => d.Tenant == tenant).Select(s => s.Id).ToList();
              
                for (int i = 0; i < 20; i++)
                {
                    List<TariffVersion> versions = new List<TariffVersion>();
                    List<ExcelTariffLines> randomTariffList = this.UniqueRandomList(tariffLines, tariffLines.Count(), 70);
                    Card airline = airlines[random.Next(airlines.Count)];
                    Tariff tariff = new Tariff()
                    {
                        Id = IdCounter.GetNumber("Tariff", tenant),
                        TariffNumber = CodeCounter.GetNumber("Tariff", tenant).ToString(),
                        PriceSteps = tariffSetting.DefaultPriceSteps,
                        Tenant = tenant,
                        CreateDate = todayDate,
                        UpdateDate = todayDate,
                        CreatedByUserId = loggedUser.Id,
                        UpdatedByUserId = loggedUser.Id,
                        SellerId = airline.Id,
                        Name = "Test Tariff From Excel" + i,
                        StartDate = todayDate.AddMonths(i),
                        ExpirationDate = todayDate.AddYears(1),
                        CurrencyId = currencyIds[random.Next(currencyIds.Count)],
                        LastVersion = 50,
                        TypeCode = "AFC",
                        ConcurrencyGUID = Guid.NewGuid().ToString(),
                        SearchFields = "Test Tariff From Excel " + i + "," + airline.EnglishName,
                    };
                    TariffVersion draftVersion = new TariffVersion()
                    {
                        TariffId = tariff.Id,
                        Version = 1,
                        IsDraft = true,
                        CreateDate = todayDate,
                        CreatedByUserId = loggedUser.Id,
                        StartDate = todayDate,
                        ExpirationDate = todayDate.AddYears(1),
                        Tenant = tenant,
                    };
                    TariffVersion activeVersion = new TariffVersion()
                    {
                        TariffId = tariff.Id,
                        Version = 2,
                        IsDraft = false,
                        CreateDate = todayDate,
                        CreatedByUserId = loggedUser.Id,
                        StartDate = todayDate.AddMonths(-1),
                        ExpirationDate = todayDate.AddMonths(2),
                        Tenant = tenant,
                        ApproveDate = todayDate,
                        ApprovedByUserId = loggedUser.Id,
                        ParentVersionNumber = 1,
                    };
                    iTariffModuleContext.TariffVersions.Add(draftVersion);
                    iTariffModuleContext.TariffVersions.Add(activeVersion);
                    versions.Add(draftVersion);
                    versions.Add(activeVersion);

                    var twentyPercentOftariffLines = 0;
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
                            Tenant = tenant,
                            ApproveDate = todayDate,
                            ApprovedByUserId = loggedUser.Id,
                            ParentVersionNumber = j - 1,
                        };
                        iTariffModuleContext.TariffVersions.Add(version);
                        versions.Add(version);
                    }
                    foreach (TariffVersion item in versions)
                    {
                        twentyPercentOftariffLines = 0;
                        for (int k = 0; k < randomTariffList.Count(); k++)
                        {
                            twentyPercentOftariffLines = twentyPercentOftariffLines + 1; 
                            TariffLine line = new TariffLine()
                            {
                                Id = IdCounter.GetNumber("TariffLine", tenant),
                                Tenant = tenant,
                                TariffId = tariff.Id,
                                Version = item.Version,
                                StartDate = item.StartDate,
                                ExpirationDate = item.ExpirationDate,
                                OriginPortId = randomTariffList.ElementAt(k).FromPortId,
                                DestinationPortId = randomTariffList.ElementAt(k).ToPortId,
                                Index = k,
                                LineUniqueKey = randomTariffList.ElementAt(k).FromPortCode + "," + randomTariffList.ElementAt(k).ToPortCode,
                                LineUniqueKeyText = randomTariffList.ElementAt(k).FromPortCode + "," + randomTariffList.ElementAt(k).ToPortCode + k,
                            };

                            if(twentyPercentOftariffLines <= 14)
                            {
                                // Generate Random Lines
                                line.MinPrice = randomTariffList.ElementAt(k).MinPrice.Value;
                                line.Step1Price = decimal.Round(RandomStepPrice(randomTariffList.ElementAt(k).Step1Price.Value), 3, MidpointRounding.AwayFromZero);
                                line.Step2Price = decimal.Round(RandomStepPrice(randomTariffList.ElementAt(k).Step2Price.Value), 3, MidpointRounding.AwayFromZero);
                                line.Step3Price = decimal.Round(RandomStepPrice(randomTariffList.ElementAt(k).Step3Price.Value), 3, MidpointRounding.AwayFromZero);
                                line.Step4Price = decimal.Round(RandomStepPrice(randomTariffList.ElementAt(k).Step4Price.Value), 3, MidpointRounding.AwayFromZero);
                                line.Step5Price = decimal.Round(RandomStepPrice(randomTariffList.ElementAt(k).Step5Price.Value), 3, MidpointRounding.AwayFromZero);
                                line.Step6Price = decimal.Round(RandomStepPrice(randomTariffList.ElementAt(k).Step6Price.Value), 3, MidpointRounding.AwayFromZero);
                                line.Step7Price = decimal.Round(RandomStepPrice(randomTariffList.ElementAt(k).Step7Price.Value), 3, MidpointRounding.AwayFromZero);
                                line.Step8Price = decimal.Round(RandomStepPrice(randomTariffList.ElementAt(k).Step8Price.Value), 3, MidpointRounding.AwayFromZero);
                                iTariffModuleContext.TariffLines.Add(line);
                            }
                            else
                            {
                                // Excel Lines
                                line.MinPrice = randomTariffList.ElementAt(k).MinPrice.Value;
                                line.Step1Price = decimal.Round(randomTariffList.ElementAt(k).Step1Price.Value, 3, MidpointRounding.AwayFromZero);
                                line.Step2Price = decimal.Round(randomTariffList.ElementAt(k).Step2Price.Value, 3, MidpointRounding.AwayFromZero);
                                line.Step3Price = decimal.Round(randomTariffList.ElementAt(k).Step3Price.Value, 3, MidpointRounding.AwayFromZero);
                                line.Step4Price = decimal.Round(randomTariffList.ElementAt(k).Step4Price.Value, 3, MidpointRounding.AwayFromZero);
                                line.Step5Price = decimal.Round(randomTariffList.ElementAt(k).Step5Price.Value, 3, MidpointRounding.AwayFromZero);
                                line.Step6Price = decimal.Round(randomTariffList.ElementAt(k).Step6Price.Value, 3, MidpointRounding.AwayFromZero);
                                line.Step7Price = decimal.Round(randomTariffList.ElementAt(k).Step7Price.Value, 3, MidpointRounding.AwayFromZero);
                                line.Step8Price = decimal.Round(randomTariffList.ElementAt(k).Step8Price.Value, 3, MidpointRounding.AwayFromZero);
                                iTariffModuleContext.TariffLines.Add(line);
                            }
                        }
                    }

                    TariffVersion lastVersion = versions.Where(d => d.Version == 50).FirstOrDefault();
                    tariff.LastStartDate = lastVersion.StartDate;
                    tariff.LastExpirationDate = lastVersion.ExpirationDate;
                    iTariffModuleContext.Tariffs.Add(tariff);
                    airlines.Remove(airline);
                    iTariffModuleContext.SaveChanges();
                }
            }
        }
        private static readonly Random random = new Random();
        private decimal RandomStepPrice(decimal value)
        {
            var minValue = -6;
            var maxValue = 6;
            var next = random.Next(minValue, maxValue);
            return (value + ((decimal)next)) / 1m;
        }
        private List<ExcelTariffLines> UniqueRandomList(List<ExcelTariffLines> tariffLines, int maxRange, int totalRandomnoCount)
        {
            List<ExcelTariffLines> tariffList_random = new List<ExcelTariffLines>();
            int count = 0;
            Random random = new Random();
            List<ExcelTariffLines> listRange = new List<ExcelTariffLines>();
            for (int i = 0; i < totalRandomnoCount; i++)
            {
                listRange.Add(tariffLines.ElementAt(i));
            }
            while (listRange.Count > 0)
            {
                int item = random.Next(maxRange);
                if (!tariffList_random.Contains(tariffLines.ElementAt(item)) && listRange.Count > 0)
                {
                    tariffList_random.Add(tariffLines.ElementAt(item));
                    listRange.Remove(tariffLines.ElementAt(count));
                    count++;
                }
            }
            return tariffList_random;
        }
    }

    public class TariffsExcelGeneratorArgs
    {
        public int Tenant { get; set; }
        public string LoggedUserEmail { get; set; }
        public string DocumentId { get; set; }
    }
}