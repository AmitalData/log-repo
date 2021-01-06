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
using Logitude.Infrastructure.Data.Repsitories;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Infrastructure.BL.EntityUpdateServices;
using Logitude.Infrastructure.BL.EntityQueryServices;
using Logitude.Infrastructure.Data;
using Logitude.Infrastructure.Data.EntityPOCOs;

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
        private CountryRepository countryRepository;
        private StateRepository stateRepository;
        private string errorMsg;
        private BatchTaskExecutionRepository batchTaskExecutionRepository;
        private BatchTaskExecutionPM batchTaskExecutionPM;
        private BatchTaskExecution batchTaskExecution;
        private List<PartnerExcel> PartnerExcelList;
        private ICommonDataContext commonDataContext;
        private IInfrastructureContext infrastructureContext;
        private ContactRepository contactRep;
        private CustomerRepository customerRepository;
        private Contact systemContact;
        private List<ContactPM> contacts;
        private ContactQuery contactQuery;
        private int duplicateLinesCount;
        private CardRepository cardRepository;
        private AddressQuery addressQuery;
        private bool IsConfirmationByUser;

        public PartnersUploadHelper(BatchTaskExecutionPM batchTaskExecution) : base(batchTaskExecution)
        {
            this.batchTaskExecutionPM = batchTaskExecution;
        }

        public override void RunCode()
        {

            this.Initizlization();
            this.FillDefaultValues();
            this.ReadExcelFile_Sheet();
            this.BuildPartnersFromExcelSheet();
        }

        private void Initizlization()
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
            infrastructureContext = InfrastructureContext.GetContext(tenant);
            string xmlParameters = batchTaskExecutionPM.PrametersXml;
            System.IO.StringReader stringReader = new System.IO.StringReader(xmlParameters);
            XmlSerializer serializer = new XmlSerializer(typeof(PartnersUploadExcelParameter));
            parameterArgs = serializer.Deserialize(stringReader) as PartnersUploadExcelParameter;
            tenant = parameterArgs.Tenant;
            loggedUserEmail = parameterArgs.LoggedUserEmail;
            IsConfirmationByUser = parameterArgs.IsConfirmationByUser;
            partnerTypeRepository = new PartnerTypeRepository(commonDataContext);
            documentRepository = new DocumentRepository(commonDataContext);
            countryRepository = new CountryRepository(commonDataContext);
            stateRepository = new StateRepository(commonDataContext);
            contactRep = new ContactRepository(commonDataContext);
            customerRepository = new CustomerRepository(commonDataContext);
            contactQuery = new ContactQuery(contactRep);
            cardRepository = new CardRepository(commonDataContext);
            addressQuery = new AddressQuery(tenant);
            batchTaskExecutionRepository = new BatchTaskExecutionRepository(infrastructureContext);
        }

        private void FillDefaultValues()
        {
            contacts = contactQuery.GetContactPMsWithoutPassWordsByTenant(tenant);
            systemContact = contactRep.GetSingleContactByEmail(loggedUserEmail, tenant);
            this.FillDefaultValues_Partner();
        }

        private List<string> partnersUniqueKeys;

        private void FillDefaultValues_Partner()
        {
            partnersUniqueKeys = new List<string>();
            var query = (from card in commonDataContext.Cards
                         where card.Tenant == tenant
                         select new
                         {
                             UploadingUniqueKey = card.UploadingUniqueKey,
                         }
                         );

            foreach (var rec in query)
            {
                partnersUniqueKeys.Add(rec.UploadingUniqueKey);
            }
        }
        ExcelEngine excelEngine;
        IWorkbook workbook;
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


                System.IO.MemoryStream stream = new System.IO.MemoryStream(fileData);
                excelEngine = new ExcelEngine();
                IApplication application = excelEngine.Excel;
                workbook = excelEngine.Excel.Workbooks.Open(stream);
                partnersUploadExcelSheet = workbook.Worksheets[0];

            }
        }

        private void BuildPartnersFromExcelSheet()
        {
            PartnerExcelList = new List<PartnerExcel>();
            this.errorMsg = "";

            if (partnersUploadExcelSheet != null && (partnersUploadExcelSheet.Rows != null && partnersUploadExcelSheet.Rows.Count() < 1002))
            {
                foreach (IRange row in partnersUploadExcelSheet.UsedRange.Rows.Skip(1))
                {

                    PartnerExcel partnerExcel = new PartnerExcel();
                    // Full Column 
                    partnerExcel.RowIndex = row.Row;

                    String[] rowData = new String[partnersUploadExcelSheet.Columns.Count()];
                    for (int i = 0; i < partnersUploadExcelSheet.Columns.Count(); i++)
                    {
                        if (row.Cells[i].HasFormula)
                        {
                            rowData[i] = row.Cells[i].FormulaNumberValue.ToString();
                        }
                        else
                        {
                            rowData[i] = row.Cells[i].Value2.ToString();
                        }
                    }

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
                                this.errorMsg = this.errorMsg + "Line " + partnerExcel.RowIndex + ": " + "Partner Type is invalid" + ",";
                            }
                        }

                        else
                        {
                            this.errorMsg = this.errorMsg + "Line " + partnerExcel.RowIndex + ": " + "Partner Type is missing" + ",";
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
                            this.errorMsg = this.errorMsg + "Line " + partnerExcel.RowIndex + ": " + "Unique Code is missing" + ",";
                        }
                    }

                    if (rowData.Length > 2)
                    {
                        if (!string.IsNullOrEmpty(rowData[2]))
                        {
                            if (rowData[2].Length > 70)
                            {
                                this.errorMsg = this.errorMsg + "Line " + partnerExcel.RowIndex + ": " + "Name Field max length must be 70" + ",";
                            }
                            else
                            {
                                partnerExcel.Name = rowData[2].Trim();
                            }
                        }
                        else
                        {
                            this.errorMsg = this.errorMsg + "Line " + partnerExcel.RowIndex + ": " + "Name is missing" + ",";
                        }
                    }

                    if (rowData.Length > 3)
                    {
                        if (!string.IsNullOrEmpty(rowData[3]))
                        {
                            if (rowData[3].Length > 20)
                            {
                                this.errorMsg = this.errorMsg + "Line " + partnerExcel.RowIndex + ": " + "Vat NO Field max length must be 20" + ",";
                            }
                            else
                            {
                                partnerExcel.VatNO = rowData[3].Trim();
                            }
                        }
                    }
                    if (rowData.Length > 4)
                    {
                        if (!string.IsNullOrEmpty(rowData[4]))
                        {
                            if (rowData[4].Length > 65)
                            {
                                this.errorMsg = this.errorMsg + "Line " + partnerExcel.RowIndex + ": " + "Address1 Field max length must be 65" + ",";
                            }
                            else
                            {
                                partnerExcel.Address1 = rowData[4].Trim();
                            }
                        }
                    }
                    if (rowData.Length > 5)
                    {
                        if (!string.IsNullOrEmpty(rowData[5]))
                        {
                            if (rowData[5].Length > 65)
                            {
                                this.errorMsg = this.errorMsg + "Line " + partnerExcel.RowIndex + ": " + "Address2 Field max length must be 65" + ",";
                            }
                            else
                            {
                                partnerExcel.Address2 = rowData[5].Trim();
                            }
                        }
                    }
                    if (rowData.Length > 6)
                    {
                        if (!string.IsNullOrEmpty(rowData[6]))
                        {
                            if (rowData[6].Length > 15)
                            {
                                this.errorMsg = this.errorMsg + "Line " + partnerExcel.RowIndex + ": " + "Zip/Postal Code Field max length must be 15" + ",";
                            }
                            else
                            {
                                partnerExcel.ZipCode = rowData[6].Trim();
                            }
                        }
                    }
                    if (rowData.Length > 7)
                    {
                        if (string.IsNullOrEmpty(rowData[7]))
                        {
                            this.errorMsg = this.errorMsg + "Line " + partnerExcel.RowIndex + ": " + "City is missing" + ",";
                        }
                    }
                    if (rowData.Length > 8)
                    {
                        if (!string.IsNullOrEmpty(rowData[8]))
                        {
                            if (rowData[8].Length > 40)
                            {
                                this.errorMsg = this.errorMsg + "Line " + partnerExcel.RowIndex + ": " + "State Field max length must be 40" + ",";
                            }
                            else
                            {
                                partnerExcel.State = rowData[8].Trim();
                            }
                        }
                    }
                    if (rowData.Length > 9)
                    {
                        if (!string.IsNullOrEmpty(rowData[9]))
                        {
                            if (rowData[9].Length > 2)
                            {
                                this.errorMsg = this.errorMsg + "Line " + partnerExcel.RowIndex + ": " + "Country Code Field max length must be 2" + ",";
                            }
                            else
                            {
                                string countryCode = rowData[9].Trim();
                                Country country = countryRepository.GetSingleCountryByCode(countryCode, tenant);
                                if (country != null)
                                {
                                    partnerExcel.CountryCode = countryCode;
                                    partnerExcel.CountryId = country.Id;
                                    partnerExcel.City = rowData[7].Trim().Length > 25 ? rowData[7].Trim().Substring(0, 25) : rowData[7].Trim();

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
                                    this.errorMsg = this.errorMsg + "Line " + partnerExcel.RowIndex + ": " + "Country is invalid" + ",";
                                }
                            }
                        }
                        else
                        {
                            this.errorMsg = this.errorMsg + "Line " + partnerExcel.RowIndex + ": " + "Country is missing" + ",";
                        }
                    }
                    if (rowData.Length > 10)
                    {
                        if (!string.IsNullOrEmpty(rowData[10]))
                        {
                            if (rowData[10].Length > 40)
                            {
                                this.errorMsg = this.errorMsg + "Line " + partnerExcel.RowIndex + ": " + "Phone Number Field max length must be 40" + ",";
                            }
                            else
                            {
                                partnerExcel.PhoneNumber = rowData[10].Trim();
                            }
                        }
                    }
                    if (rowData.Length > 11)
                    {
                        if (!string.IsNullOrEmpty(rowData[11]))
                        {
                            if (rowData[11].Length > 25)
                            {
                                this.errorMsg = this.errorMsg + "Line " + partnerExcel.RowIndex + ": " + "Fax Number Field max length must be 25" + ",";
                            }
                            else
                            {
                                partnerExcel.FaxNumber = rowData[11].Trim();
                            }
                        }
                    }
                    if (rowData.Length > 12)
                    {
                        if (!string.IsNullOrEmpty(rowData[12]))
                        {
                            if (rowData[12].Length > 70)
                            {
                                this.errorMsg = this.errorMsg + "Line " + partnerExcel.RowIndex + ": " + "Email Field max length must be 70" + ",";
                            }
                            else
                            {
                                partnerExcel.EMail = rowData[12].Trim();
                            }
                        }
                    }
                    if (rowData.Length > 13)
                    {
                        if (!string.IsNullOrEmpty(rowData[13]))
                        {
                            if (rowData[13].Length > 60)
                            {
                                this.errorMsg = this.errorMsg + "Line " + partnerExcel.RowIndex + ": " + "Contact Name Field max length must be 60" + ",";
                            }
                            else
                            {
                                partnerExcel.ContactName = rowData[13].Trim();
                            }
                        }
                    }
                    if (rowData.Length > 14)
                    {
                        if (!string.IsNullOrEmpty(rowData[14]))
                        {
                            if (rowData[14].Length > 25)
                            {
                                this.errorMsg = this.errorMsg + "Line " + partnerExcel.RowIndex + ": " + "Receivables External ID Field max length must be 25" + ",";
                            }
                            else
                            {
                                partnerExcel.ReceivablesExternalID = rowData[14].Trim();
                            }
                        }
                    }
                    if (rowData.Length > 15)
                    {
                        if (!string.IsNullOrEmpty(rowData[15]))
                        {
                            if (rowData[15].Length > 25)
                            {
                                this.errorMsg = this.errorMsg + "Line " + partnerExcel.RowIndex + ": " + "Payables External ID Field max length must be 25" + ",";
                            }
                            else
                            {
                                partnerExcel.PayablesExternalID = rowData[15].Trim();
                            }
                        }
                    }
                    if (rowData.Length > 16)
                    {
                        if (!string.IsNullOrEmpty(rowData[16]))
                        {
                            if (partnerExcel.Type == "WH" && rowData[16].Length > 5)
                            {
                                this.errorMsg = this.errorMsg + "Line " + partnerExcel.RowIndex + ": " + "Code Field max length must be 5" + ",";
                            }
                            else if (partnerExcel.Type == "TR" && rowData[16].Length > 7)
                            {
                                this.errorMsg = this.errorMsg + "Line " + partnerExcel.RowIndex + ": " + "Code Field max length must be 7" + ",";
                            }
                            else
                            {
                                partnerExcel.Code = rowData[16].Trim();
                            }
                        }
                        else
                        {
                            if (partnerExcel.Type == "WH" || partnerExcel.Type == "TR")
                            {
                                this.errorMsg = this.errorMsg + "Line " + partnerExcel.RowIndex + ": " + "Code field is required " + ",";
                            }
                        }
                    }

                    if (rowData.Length > 17)
                    {
                        if (!string.IsNullOrEmpty(rowData[17]))
                        {
                            if (partnerExcel.Type == "CS" || partnerExcel.Type == "PO")
                            {
                                partnerExcel.SalesmanEmail = rowData[17].Trim();
                            }
                        }
                    }

                    this.PartnerExcelList.Add(partnerExcel);
                }


                var checkDuplicates = from x in PartnerExcelList.Where(a => a.Code != null && (a.Type == "WH" || a.Type == "TR"))
                                      group x by (x.Code, x.Type) into g
                                      let count = g.Count()
                                      orderby count descending
                                      select new { Value = g.Key, Count = count };

                int checkCodeDuplicates_Count = checkDuplicates.Where(a => a.Count > 1).Count();
                if (checkCodeDuplicates_Count > 0)
                {
                    errorMsg = "Warehouse/Trucker Codes are duplicated,";
                }
            }
            else
            {
                errorMsg = "You can't upload more than 1000 Partners,";
            }

            if (!string.IsNullOrEmpty(errorMsg))
            {
                HandelErrorMsg();
            }
            else
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction(new TimeSpan(3, 0, 0)))
                {
                    RunPartnersGenerator_Validation();
                    scope.Complete();
                }
            }
        }

        private void HandelErrorMsg()
        {
            this.errorMsg = this.errorMsg.Length > 4000 ? errorMsg.Substring(0, 4000) : errorMsg;
            this.batchTaskExecutionPM.StatusCode = "F";
            this.batchTaskExecutionPM.ProgressMessage = errorMsg;
        }

        Dictionary<string, State> statesDictionary;
        Dictionary<string, Country> countriesDictionary;
        private void RunPartnersGenerator_Validation()
        {
            var checkDuplicates = from x in PartnerExcelList
                                  group x by x.UniqueCode into g
                                  let count = g.Count()
                                  orderby count descending
                                  select new { Value = g.Key, Count = count };

            if (!IsConfirmationByUser)
            {
                int checkDuplicates_Count = checkDuplicates.Where(a => a.Count > 1).Count();
                var excelIds = PartnerExcelList.Select(a => a.UniqueCode).ToList();
                int intersectionBetweenExcelAndDB = this.partnersUniqueKeys.Intersect(excelIds).ToList().Count();
                this.duplicateLinesCount = duplicateLinesCount + checkDuplicates.Where(a => a.Count > 1).Sum(a => a.Count);
                this.duplicateLinesCount = duplicateLinesCount + intersectionBetweenExcelAndDB;
                var message = "";
                if (checkDuplicates_Count > 0 && intersectionBetweenExcelAndDB > 0)
                {
                    message = checkDuplicates_Count + " duplicate lines were found in Excel and DB. Do you want to continue?";
                }
                else if (checkDuplicates_Count > 0 || intersectionBetweenExcelAndDB > 0)
                {
                    if (checkDuplicates_Count > 0)
                    {
                        message = checkDuplicates_Count + " duplicate lines were found in Excel. Do you want to continue?";
                    }
                    else
                    {
                        message = intersectionBetweenExcelAndDB + " duplicate lines were found in DB. Do you want to continue?";
                    }
                }
                else
                {
                    message = "Do you want to continue?";
                }

                this.SendConfirmationMessage(message);
            }
            else
            {
                this.FillPartnersFromExcelToDB();
            }
        }

        private void FillPartnersFromExcelToDB()
        {
            this.errorMsg = "";
            var errorsCount = 0;
            statesDictionary = stateRepository.GetStates(tenant).ToDictionary(d => d.Id, o => o);
            countriesDictionary = countryRepository.GetCountries(tenant).ToDictionary(d => d.Id, o => o);
            var currentItem = 0;

            foreach (var item in PartnerExcelList)
            {
                try
                {
                    currentItem = currentItem + 1;
                    var checkIfCardExist = this.partnersUniqueKeys.Where(a => a == item.UniqueCode).FirstOrDefault();
                    if (checkIfCardExist == null)
                    {
                        switch (item.Type)
                        {
                            case "AG":
                                {
                                    this.CreateAgentPartner(item);
                                    break;
                                }

                            case "CS":
                            case "PO":
                                {
                                    this.CreateCustomerPartner(item);
                                    break;
                                }

                            case "CG":
                                {
                                    this.CreateCustomAgentPartner(item);
                                    break;
                                }

                            case "SG":
                                {
                                    this.CreateShippingAgentPartner(item);
                                    break;
                                }

                            case "VD":
                                {
                                    this.CreateVendorPartner(item);
                                    break;
                                }
                            case "WH":
                                {
                                    this.CreateWarehousePartner(item);
                                    break;
                                }
                            case "AL":
                                {
                                    this.CreateAirlinePartner(item);
                                    break;
                                }
                            case "SL":
                                {
                                    this.CreateShippingLinePartner(item);
                                    break;
                                }
                            case "TR":
                                {
                                    this.CreateTruckerPartner(item);
                                    break;
                                }
                            case "AC":
                                {
                                    this.CreateAccountingPartnerPartner(item);
                                    break;
                                }
                        }
                        this.partnersUniqueKeys.Add(item.UniqueCode);
                    }
                    else
                    {
                        this.duplicateLinesCount = duplicateLinesCount + 1;
                    }

                    this.UpdateProcessPercentage(PartnerExcelList.Count(), currentItem);
                }

                catch (Exception e)
                {
                    this.errorMsg += "Line " + item.RowIndex + ": " + e.Message + ",";
                    errorsCount = errorsCount + 1;
                }
            }

            this.HandelBatchTask();
        }

        private void HandelBatchTask()
        {
            string msg = "";
            if (!string.IsNullOrEmpty(this.errorMsg))
            {
                errorMsg = errorMsg.Length > 4000 ? errorMsg.Substring(0, 4000) : errorMsg;
                this.batchTaskExecutionPM.StatusCode = "F";
                this.batchTaskExecutionPM.ProgressMessage = errorMsg;
                throw new ApplicationException(errorMsg);

            }

            else
            {
                try
                {
                    this.UpdateCustomersSalesmen();

                    this.batchTaskExecutionPM.StatusCode = "D";
                    msg = "Successfully Uploaded " + (PartnerExcelList.Count() - duplicateLinesCount) + " out of " + PartnerExcelList.Count() + " Partners. " +
                                                               duplicateLinesCount + " duplicate lines were found.";
                    msg = msg.Length > 4000 ? msg.Substring(0, 4000) : msg;
                    this.batchTaskExecutionPM.ProgressMessage = msg;
                }

                catch (Exception e)
                {
                    msg = e.Message;
                    msg = msg.Length > 4000 ? msg.Substring(0, 4000) : msg;
                    this.batchTaskExecutionPM.StatusCode = "F";
                    this.batchTaskExecutionPM.ProgressMessage = msg;
                    throw new ApplicationException(msg);
                }
            }

            workbook.Close();
            excelEngine.Dispose();
        }

        private void SendConfirmationMessage(string msg)
        {
            this.batchTaskExecutionPM.StatusCode = "D";
            this.batchTaskExecutionPM.ProgressMessage = msg;
        }

        private void UpdateProcessPercentage(decimal maximum, decimal current)
        {
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                batchTaskExecution = batchTaskExecutionRepository.GetSingle(batchTaskExecutionPM.Id, tenant);
                decimal percentage = (current / maximum) * 100; ;
                this.batchTaskExecution.ProgressPercentage = Convert.ToInt32(percentage);
                this.batchTaskExecutionPM.ProgressPercentage = Convert.ToInt32(percentage);
                batchTaskExecutionRepository.Update(batchTaskExecution);
                batchTaskExecutionRepository.SubmitChanges();
                scope.Complete();
            }
        }

        private void CreateVendorPartner(PartnerExcel item)
        {
            VendorPM vendor = new VendorPM()
            {
                Id = IdCounter.GetNumber("Card", tenant).ToString(),
                EnglishName = item.Name,
                VatNumber = item.VatNO,
                Tenant = tenant,
                IsHybrid = true,
                Code = CodeCounter.GetNumber("Vendor", tenant).ToString(),
                PartnerTypeId = item.Type,
                UploadingUniqueKey = item.UniqueCode,
                ReceivablesAccountingCard = item.ReceivablesExternalID,
                PayablesAccountingCard = item.PayablesExternalID,

            };

            var address = CreateAddress(item, vendor.Id);
            vendor.Addresses.Add(address);
            var contactPM = CreatContact(item);
            if (contactPM != null)
            {
                vendor.Contacts.Add(contactPM);
            }
            VendorService service = new VendorService(commonDataContext, vendor, systemContact.Id);
            service.Create(vendor);
        }

        private void CreateAccountingPartnerPartner(PartnerExcel item)
        {
            AccountingPartnerPM accountingPartner = new AccountingPartnerPM()
            {
                Id = IdCounter.GetNumber("Card", tenant).ToString(),
                EnglishName = item.Name,
                VatNumber = item.VatNO,
                Tenant = tenant,
                IsHybrid = true,
                Code = CodeCounter.GetNumber("AccountingPartner", tenant).ToString(),
                PartnerTypeId = item.Type,
                UploadingUniqueKey = item.UniqueCode,
                ReceivablesAccountingCard = item.ReceivablesExternalID,
                PayablesAccountingCard = item.PayablesExternalID,
            };

            var address = CreateAddress(item, accountingPartner.Id);
            accountingPartner.Addresses.Add(address);
            var contactPM = CreatContact(item);
            if (contactPM != null)
            {
                accountingPartner.Contacts.Add(contactPM);
            }

            AccountingPartnerService service = new AccountingPartnerService(commonDataContext, accountingPartner, systemContact.Id);
            service.Create(accountingPartner);
        }

        private void CreateTruckerPartner(PartnerExcel item)
        {
            TruckerPM trucker = new TruckerPM()
            {
                Id = IdCounter.GetNumber("Card", tenant).ToString(),
                EnglishName = item.Name,
                VatNumber = item.VatNO,
                Tenant = tenant,
                IsHybrid = true,
                Code = item.Code,
                CarrierTypeId = item.Type,
                UploadingUniqueKey = item.UniqueCode,
                ReceivablesAccountingCard = item.ReceivablesExternalID,
                PayablesAccountingCard = item.PayablesExternalID,
            };

            var address = CreateAddress(item, trucker.Id);
            trucker.Addresses.Add(address);
            var contactPM = CreatContact(item);
            if (contactPM != null)
            {
                trucker.Contacts.Add(contactPM);
            }

            TruckerService service = new TruckerService(commonDataContext, trucker, systemContact.Id);
            service.Create(trucker);
        }

        private void CreateShippingLinePartner(PartnerExcel item)
        {
            ShippingLinePM shippingLine = new ShippingLinePM()
            {
                EnglishName = item.Name,
                VatNumber = item.VatNO,
                Tenant = tenant,
                IsHybrid = true,
                Code = CodeCounter.GetNumber("ShippingLine", tenant).ToString(),
                CarrierTypeId = item.Type,
                UploadingUniqueKey = item.UniqueCode,
                ReceivablesAccountingCard = item.ReceivablesExternalID,
                PayablesAccountingCard = item.PayablesExternalID,
            };

            ShippingLineService service = new ShippingLineService(commonDataContext, shippingLine, systemContact.Id);
            service.Create(shippingLine);
        }

        private void CreateAirlinePartner(PartnerExcel item)
        {
            AirlinePM airline = new AirlinePM()
            {
                EnglishName = item.Name,
                VatNumber = item.VatNO,
                Tenant = tenant,
                IsHybrid = true,
                Code = CodeCounter.GetNumber("Airline", tenant).ToString(),
                CarrierTypeId = item.Type,
                UploadingUniqueKey = item.UniqueCode,
                ReceivablesAccountingCard = item.ReceivablesExternalID,
                PayablesAccountingCard = item.PayablesExternalID,
            };

            AirlineService service = new AirlineService(commonDataContext, airline, systemContact.Id);
            service.Create(airline);
        }

        private void CreateWarehousePartner(PartnerExcel item)
        {
            WarehousePM warehouse = new WarehousePM()
            {
                Id = IdCounter.GetNumber("Card", tenant).ToString(),
                EnglishName = item.Name,
                VatNumber = item.VatNO,
                Tenant = tenant,
                IsHybrid = true,
                Code = item.Code,
                PartnerTypeId = item.Type,
                UploadingUniqueKey = item.UniqueCode,
                ReceivablesAccountingCard = item.ReceivablesExternalID,
                PayablesAccountingCard = item.PayablesExternalID,
            };

            var address = CreateAddress(item, warehouse.Id);
            warehouse.Addresses.Add(address);
            var contactPM = CreatContact(item);
            if (contactPM != null)
            {
                warehouse.Contacts.Add(contactPM);
            }

            WarehouseService service = new WarehouseService(commonDataContext, warehouse, systemContact.Id);
            service.Create(warehouse);
        }

        private void CreateShippingAgentPartner(PartnerExcel item)
        {
            ShippingAgentPM shippingAgent = new ShippingAgentPM()
            {
                Id = IdCounter.GetNumber("Card", tenant).ToString(),
                EnglishName = item.Name,
                VatNumber = item.VatNO,
                Tenant = tenant,
                IsHybrid = true,
                Code = CodeCounter.GetNumber("ShippingAgent", tenant).ToString(),
                PartnerTypeId = item.Type,
                UploadingUniqueKey = item.UniqueCode,
                ReceivablesAccountingCard = item.ReceivablesExternalID,
                PayablesAccountingCard = item.PayablesExternalID,
            };

            var address = CreateAddress(item, shippingAgent.Id);
            shippingAgent.Addresses.Add(address);
            var contactPM = CreatContact(item);
            if (contactPM != null)
            {
                shippingAgent.Contacts.Add(contactPM);
            }
            ShippingAgentService service = new ShippingAgentService(commonDataContext, shippingAgent, systemContact.Id);
            service.Create(shippingAgent);
        }

        private void CreateCustomAgentPartner(PartnerExcel item)
        {
            CustomAgentPM customAgent = new CustomAgentPM()
            {
                Id = IdCounter.GetNumber("Card", tenant).ToString(),
                EnglishName = item.Name,
                VatNumber = item.VatNO,
                Tenant = tenant,
                IsHybrid = true,
                Code = CodeCounter.GetNumber("CustomAgent", tenant).ToString(),
                PartnerTypeId = item.Type,
                UploadingUniqueKey = item.UniqueCode,
                ReceivablesAccountingCard = item.ReceivablesExternalID,
                PayablesAccountingCard = item.PayablesExternalID,
            };

            var address = CreateAddress(item, customAgent.Id);
            customAgent.Addresses.Add(address);
            var contactPM = CreatContact(item);
            if (contactPM != null)
            {
                customAgent.Contacts.Add(contactPM);
            }

            CustomAgentService service = new CustomAgentService(commonDataContext, customAgent, systemContact.Id);
            service.Create(customAgent);
        }

        private void CreateCustomerPartner(PartnerExcel item)
        {
            CustomerPM customer = new CustomerPM()
            {
                Id = IdCounter.GetNumber("Card", tenant).ToString(),
                EnglishName = item.Name,
                VatNumber = item.VatNO,
                Tenant = tenant,
                IsHybrid = true,
                Code = CodeCounter.GetNumber("Customer", tenant).ToString(),
                PartnerTypeId = item.Type,
                CustomerStatusCode = "ACT",
                IsCustomer = true,
                UploadingUniqueKey = item.UniqueCode,
                ReceivablesAccountingCard = item.ReceivablesExternalID,
                PayablesAccountingCard = item.PayablesExternalID,
            };
            var address = CreateAddress(item, customer.Id);
            customer.Addresses.Add(address);
            var contactPM = CreatContact(item);
            if (contactPM != null)
            {
                customer.Contacts.Add(contactPM);
            }

            CustomerService service = new CustomerService(commonDataContext, customer, systemContact.Id);
            service.Create();
        }

        private void CreateAgentPartner(PartnerExcel item)
        {
            AgentPM agent = new AgentPM()
            {
                Id = IdCounter.GetNumber("Card", tenant).ToString(),
                EnglishName = item.Name,
                VatNumber = item.VatNO,
                Tenant = tenant,
                IsHybrid = true,
                Code = CodeCounter.GetNumber("Agent", tenant).ToString(),
                PartnerTypeId = item.Type,
                UploadingUniqueKey = item.UniqueCode,
                ReceivablesAccountingCard = item.ReceivablesExternalID,
                PayablesAccountingCard = item.PayablesExternalID,
            };

            var address = CreateAddress(item, agent.Id);
            agent.Addresses.Add(address);
            var contactPM = CreatContact(item);
            if (contactPM != null)
            {
                agent.Contacts.Add(contactPM);
            }

            AgentService service = new AgentService(commonDataContext, agent, systemContact.Id);
            service.Create(agent);
        }

        private AddressPM CreateAddress(PartnerExcel item, string partnerId)
        {
            Country country = null;
            State state = null;
            if (item.CountryId != null && countriesDictionary.Keys.Contains(item.CountryId))
            {
                country = countriesDictionary[item.CountryId];
            }
            if (item.StateId != null && statesDictionary.Keys.Contains(item.StateId))
            {
                state = statesDictionary[item.StateId];
            }
            AddressPM address = new AddressPM()
            {
                Name = item.Name,
                Description = "Main Address",
                Address1 = item.Address1,
                Address2 = item.Address2,
                ZipCode = item.ZipCode,
                StateId = state != null ? state.Id : null,
                CountryId = country != null ? country.Id : null,
                City = item.City,
                PhoneNumber = item.PhoneNumber != null ? (item.PhoneNumber.Length > 39 ? item.PhoneNumber.Substring(0, 39) : item.PhoneNumber) : null,
                FaxNumber = item.FaxNumber,
                AddressTypeId = "M",
                Tenant = tenant,
            };
            return address;
        }
        private ContactPM CreatContact(PartnerExcel item)
        {
            if (!string.IsNullOrEmpty(item.EMail) || !string.IsNullOrEmpty(item.ContactName))
            {
                ContactPM contactPM = null;
                string contactEnglishName = item.ContactName;
                if (string.IsNullOrEmpty(item.ContactName))
                {
                    contactEnglishName = item.EMail.Split('@')[0];
                }

                if (!string.IsNullOrEmpty(item.EMail))
                {
                    contactPM = contacts.Where(d => d.Email == item.EMail).FirstOrDefault();
                }
                else if (!string.IsNullOrEmpty(item.ContactName))
                {
                    contactPM = contacts.Where(d => d.EnglishName == item.ContactName).FirstOrDefault();
                }
                if (contactPM == null)
                {
                    contactPM = new ContactPM()
                    {
                        Email = item.EMail,
                        EnglishName = contactEnglishName,
                        Tenant = tenant,
                        CardId = "newCard",
                        IsHybrid = true,
                        IsCreatedWithPartner = true,
                    };
                    contacts.Add(contactPM);
                    commonDataContext.SaveChanges();
                }
                return contactPM;
            }
            else
                return null;
        }

        private void UpdateCustomersSalesmen()
        {
            var items = (from b in PartnerExcelList where (b.Type == "CS" || b.Type == "PO") && !string.IsNullOrEmpty(b.SalesmanEmail) select b).ToList();

            if (items.Count > 0)
            {
                List<string> emails = (from b in items group b by b.SalesmanEmail into g select g.Key).ToList();

                var systemUsers = (from user in commonDataContext.Users
                                      join db_Contacts in commonDataContext.Contacts
                                      on user.Id equals db_Contacts.Id
                                      into db_UsersContacts
                                      from contact in db_UsersContacts.DefaultIfEmpty()
                                      where emails.Contains(contact.Email)
                                      select new
                                      {
                                          Id = contact.Id,
                                          Email = contact.Email,
                                      }).ToList();

                int count = 0;
                foreach (PartnerExcel item in items)
                {
                    var systemUser = systemUsers.Where(d => d.Email == item.SalesmanEmail).FirstOrDefault();

                    if (systemUser == null)
                    {
                        if (count > 0)
                        {
                            cardRepository.SubmitChanges();
                            customerRepository.SubmitChanges();
                        }
                        throw new ApplicationException("Salesman " + item.SalesmanEmail + " is not a system user,");
                    }

                    else
                    {
                        Card card = cardRepository.GetSingleCardByUniqueCode(item.UniqueCode, tenant, false);
                      
                        if (card != null)
                        {
                            if (card.SalesmanUserId == null)
                            {
                                card.SalesmanUserId = systemUsers.Where(a=>a.Email == item.SalesmanEmail).Select(a=>a.Id).FirstOrDefault();
                                Customer customer = customerRepository.GetSingleCustomer(card.Id, tenant);
                                customer.SalesmanUserId = card.SalesmanUserId;
                                cardRepository.Update(card);
                                customerRepository.Update(customer);
                                count++;
                            }
                        }
                    }

                    if(count >= 100)
                    {
                        cardRepository.SubmitChanges();
                        customerRepository.SubmitChanges();
                    }
                }

                if (count > 0)
                {
                    cardRepository.SubmitChanges();
                    customerRepository.SubmitChanges();
                }
            }
        }

    }
    public class PartnerExcel
    {
        public int RowIndex { get; set; }
        public string Type { get; set; }
        public string UniqueCode { get; set; }
        public string Name { get; set; }
        public string VatNO { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string StateId { get; set; }
        public string CountryCode { get; set; }
        public string CountryId { get; set; }
        public string PhoneNumber { get; set; }
        public string FaxNumber { get; set; }
        public string EMail { get; set; }
        public string ContactName { get; set; }
        public string PayablesExternalID { get; set; }
        public string ReceivablesExternalID { get; set; }
        public string Code { get; set; }
        public string SalesmanEmail { get; set; }
    }
}