using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.BL.ExtendedServices;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Xml.Serialization;
using Logitude.Server.Tools;
using Logitude.Server.Tools.StorageService;
using Syncfusion.XlsIO;
using Microsoft.Practices.Unity;

namespace WebFreight.Web.Helpers.APIHelpers
{
    public class PartnersUploadHelper : BatchTaskExecutionsService
    {
        private IWorksheet partnersUploadExcelSheet;
        private int tenant;
        private string loggedUserEmail;
        private PartnerTypeRepository partnerTypeRepository;
        private PartnersUploadExcelParameter parameterArgs;
        private DocumentRepository documentRepository;
        private CountryCityRepository countryCityRepository;
        private CountryRepository countryRepository;
        private  StateRepository stateRepository;
        public PartnersUploadHelper(BatchTaskExecutionPM batchTaskExecution) : base(batchTaskExecution)
        {

        }

        public override void RunCode()
        {
            using (TransactionScope scope = TransactionFactory.GetTransaction(new TimeSpan(3, 0, 0)))
            {
                //this.RunPartnersGenerator(); 
                this.Initizlization();
                this.ReadExcelFile_Sheet();
                this.BuildPartnersFromExcelSheet();
                scope.Complete();
            }

        }

        private void Initizlization()
        {
            string xmlParameters = BatchTaskExecution.PrametersXml;
            System.IO.StringReader stringReader = new System.IO.StringReader(xmlParameters);
            XmlSerializer serializer = new XmlSerializer(typeof(PartnersUploadExcelParameter));
            parameterArgs = serializer.Deserialize(stringReader) as PartnersUploadExcelParameter;
            tenant = parameterArgs.Tenant;
            loggedUserEmail = parameterArgs.LoggedUserEmail;
            partnerTypeRepository = new PartnerTypeRepository(tenant);
            documentRepository = new DocumentRepository(tenant);
            countryCityRepository = new CountryCityRepository(tenant);
            countryRepository = new CountryRepository(tenant);
            stateRepository = new StateRepository(tenant);
        }

        private void ReadExcelFile_Sheet()
        {
            byte[] fileData = null;
            Document document = documentRepository.GetSingleDocument(tenant, parameterArgs.DocumentId);
            if (document != null)
            {
                IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
                BlobFileInfo fileInfo = new BlobFileInfo()
                {
                    FileName = document.Id,
                    FolderName = document.Folder,
                    Extension = document.Extension,
                    Tenant = tenant,
                    FileSize = document.FileSize,
                };
                fileData = storageservice.Read(fileInfo);
            }

            System.IO.MemoryStream stream = new System.IO.MemoryStream(fileData);
            ExcelEngine excelEngine = new ExcelEngine();
            IApplication application = excelEngine.Excel;
            IWorkbook workbook = excelEngine.Excel.Workbooks.Open(stream);
            partnersUploadExcelSheet = workbook.Worksheets[0];
        }
        private void BuildPartnersFromExcelSheet()
        {
            List<PartnerExcel> myResult = new List<PartnerExcel>();

            if (partnersUploadExcelSheet != null)
            {
                foreach (IRange row in partnersUploadExcelSheet.UsedRange.Rows.Skip(1))
                {
                    PartnerExcel partnerExcel = new PartnerExcel();
                    String[] rowData = new String[partnersUploadExcelSheet.Columns.Count()];
                    for (int i = 0; i < partnersUploadExcelSheet.Columns.Count(); i++)
                    {
                        rowData[i] = row.Cells[i].Value2.ToString();
                    }

                    // Full Column 
                    partnerExcel.RowIndex = row.Row;
                    partnerExcel.errorMsg = new List<string>();

                    if (rowData.Length > 0)
                    {
                        if (!string.IsNullOrEmpty(rowData[0]))
                        {
                            string partnerTypeCode = rowData[0].Trim();

                            PartnerType partnerType = partnerTypeRepository.GetSinglePartnerType(partnerTypeCode);
                            if (partnerType != null)
                            {
                                partnerExcel.Type = partnerType.Id;
                            }
                            else
                            {
                                partnerExcel.errorMsg.Add("Partner Type is invalid");
                            }
                        }

                        else
                        {
                            partnerExcel.errorMsg.Add("Partner Type is missing");
                        }
                    }

                    if (rowData.Length > 1)
                    {
                        if (!string.IsNullOrEmpty(rowData[1]))
                        {
                            partnerExcel.UniqueCode = rowData[1].Trim(); 
                        }
                        else
                        {
                            partnerExcel.errorMsg.Add("Unique Code is missing");
                        }
                    }

                    if (rowData.Length > 2)
                    {
                        if (!string.IsNullOrEmpty(rowData[2]))
                        {
                            partnerExcel.UniqueCode = rowData[2].Trim();
                        }
                        else
                        {
                            partnerExcel.errorMsg.Add("Name is missing");
                        }
                    }

                    if (rowData.Length > 3)
                    {
                        if (!string.IsNullOrEmpty(rowData[3]))
                        {
                            partnerExcel.VatNO = rowData[3].Trim();
                        }
                    }
                    if (rowData.Length > 4)
                    {
                        if (!string.IsNullOrEmpty(rowData[4]))
                        {
                            partnerExcel.Address1 = rowData[4].Trim();
                        }
                    }
                    if (rowData.Length > 5)
                    {
                        if (!string.IsNullOrEmpty(rowData[5]))
                        {
                            partnerExcel.Address2 = rowData[5].Trim();
                        }
                    }
                    if (rowData.Length > 6)
                    {
                        if (!string.IsNullOrEmpty(rowData[6]))
                        {
                            partnerExcel.ZipCode = rowData[6].Trim();
                        }
                    }
                    if (rowData.Length > 7)
                    {
                        if (string.IsNullOrEmpty(rowData[7]))
                        {
                            partnerExcel.errorMsg.Add("City is missing");
                        }
                    }
                    if (rowData.Length > 8)
                    {
                        if (!string.IsNullOrEmpty(rowData[8]))
                        {
                            partnerExcel.State = rowData[8].Trim();
                        }
                    }
                    if (rowData.Length > 9)
                    {
                        if (!string.IsNullOrEmpty(rowData[9]))
                        {
                            string countryCode = rowData[9].Trim();
                            Country country = countryRepository.GetSingleCountryByCode(countryCode, tenant);
                            if (country != null)
                            {
                                partnerExcel.CountryCode = countryCode;
                                partnerExcel.CountryId = country.Id;

                                string cityCode = rowData[7].Trim();
                                CountryCity countryCity = countryCityRepository.GetSingleCountryCityByCodeAndCountry(cityCode, country.Id,tenant);
                                if (countryCity != null)
                                {
                                    partnerExcel.City = countryCity.Id;
                                }
                                else
                                {
                                    partnerExcel.errorMsg.Add("City is invalid");
                                }

                                if (partnerExcel.State != null)
                                {
                                    State state = stateRepository.GetSingleStateByCodeAndCountry(partnerExcel.State, country.Id, tenant);
                                    if (state != null)
                                    {
                                        partnerExcel.StateId = state.Id;
                                    }
                                }
                            }
                            else
                            {
                                partnerExcel.errorMsg.Add("Country is invalid");
                            }
                        }
                        else
                        {
                            partnerExcel.errorMsg.Add("Country is missing");
                        }
                    }
                    if (rowData.Length > 10)
                    {
                        if (!string.IsNullOrEmpty(rowData[10]))
                        {
                            partnerExcel.PhoneNumber = rowData[10].Trim();
                        }
                    }
                    if (rowData.Length > 11)
                    {
                        if (!string.IsNullOrEmpty(rowData[11]))
                        {
                            partnerExcel.FaxNumber = rowData[11].Trim();
                        }
                    }
                    if (rowData.Length > 12)
                    {
                        if (!string.IsNullOrEmpty(rowData[12]))
                        {
                            partnerExcel.EMail = rowData[12].Trim();
                        }
                    }
                    if (rowData.Length > 13)
                    {
                        if (!string.IsNullOrEmpty(rowData[13]))
                        {
                            partnerExcel.ContactName = rowData[13].Trim();
                        }
                    }
                    if (rowData.Length > 14)
                    {
                        if (!string.IsNullOrEmpty(rowData[14]))
                        {
                            partnerExcel.ExternalID = rowData[14].Trim();
                        }
                    }
                    if (rowData.Length > 15)
                    {
                        if (!string.IsNullOrEmpty(rowData[15]))
                        {
                            partnerExcel.Code = rowData[15].Trim();
                        }
                    }
                }
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
                    Code = CodeCounter.GetNumber("Customer", tenant).ToString(),
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
    }

    public class PartnerExcel
    {
        public int RowIndex { get; set; }
        public List <string> errorMsg { get; set; }
        public string Type { get; set; }
        public string UniqueCode { get; set; }
        public string Name { get; set; }
        public string VatNO { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string CityId { get; set; }
        public string State { get; set; }
        public string StateId { get; set; }
        public string CountryCode { get; set; }
        public string CountryId { get; set; }
        public string PhoneNumber { get; set; }
        public string FaxNumber { get; set; }
        public string EMail { get; set; }
        public string ContactName { get; set; }
        public string ExternalID { get; set; }
        public string Code { get; set; }
    }
}