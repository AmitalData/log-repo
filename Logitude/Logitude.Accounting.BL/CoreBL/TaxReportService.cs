using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.DataContract;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.APIDataContract.ApiV1;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.EntityQueries;
using Logitude.CustomsMessaging.Common.Gen;
using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.BL.EntityUpdateServices;
using Logitude.Infrastructure.Data;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.QueueService;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using System.Xml.Serialization;
using Contact = Simplog.Data.CommonDataModel.EntityPOCOs.Contact;
using DocumentType = Simplog.Data.CommonDataModel.EntityPOCOs.DocumentType;
using User = Simplog.Data.CommonDataModel.EntityPOCOs.User;

namespace Logitude.Accounting.BL.CoreBL
{
	public class TaxReportService
	{
        public TaxReportService()
        {

        }

		public static string FilePath = @"E:\PCN874.txt";

		public static void CreateTaxReportLines(TaxReportPM taxReport, int tenant)
		{
			JournalRepository journalRepository = new JournalRepository(tenant);
			List<Journal> journals = journalRepository.GetARInvoiceJournals(taxReport.TaxReportMonth, tenant);
			//List<string> invoiceIds = new List<string>();

			//foreach(Journal a in journals)
			//{
			//    invoiceIds.Add(a.AccountingEntityId);
			//}

			ARInvoiceRepository aRInvoiceRepository = new ARInvoiceRepository(tenant);
			//List<ARInvoice> aRInvoices = aRInvoiceRepository.GetInvoicesListFromIdListByDate(invoiceIds, TaxReportMonth, tenant);

			IAccountingContext MyContext = AccountingContext.GetContext(taxReport.Tenant);
			TaxReportUpdateService updateService = new TaxReportUpdateService(MyContext, new Dictionary<string, IContext>(), taxReport.Tenant);
			APInvoiceQuery aPInvoiceQueryService = new APInvoiceQuery(tenant);
			CardRepository cardRepository = new CardRepository(tenant);
			TenantQuery tenantQuery = new TenantQuery(tenant);
			TenantPM tenantPM = tenantQuery.GetSinglePM(tenant);
            string  vatNumber = null;
			//Outputs
			foreach (Journal a in journals)
			{
				string reference = null;
				ARInvoice invoice = aRInvoiceRepository.GetSingleARInvoice(a.AccountingEntityId, a.Tenant);
				if (invoice != null)
				{
					if (invoice.InvoiceNumber.Length == 9)
					{
						reference = invoice.InvoiceNumber.Substring(invoice.InvoiceNumber.Length - 9);
					}
					else
					{
						reference = invoice.InvoiceNumber;
					}

                    if (!string.IsNullOrEmpty(invoice.VatNumber))
                    {
                        vatNumber = invoice.VatNumber;
                    }
					TaxReportLinePM line = new TaxReportLinePM()
					{
						VatNumber = vatNumber,
						Reference = reference,
						ReferecneGroup = "0000",
						ReferenceDate = invoice.InvoiceDate,
						JournalId = a.Id,
						OutputOrInput = "O",
						VatAmount = MethodHelper.Round(invoice.TotalVAT, 2),
						VatableInvoiceAmount = MethodHelper.Round(invoice.TotaVatableAmountForTaxReport, 2),
						IsManuallyChanged = false,
						TransmitStatusCode = "1",
						TaxReportId = taxReport.Id,
						ChangeSetOp = ChangeSetOperation.Insert,
						LastUpdateDateTime = DateTime.Now,
						UpdatedByUserId = taxReport.UpdatedByUserId,
						Tenant = tenant,


					};
                 
                    if(line.VatNumber == null) {
                        line.VatNumber = "999999999";

                    } 

					taxReport.TaxReportLines.Add(line);
				}
			}


			//inputs
			LedgerTransactionRepository ledgerTransactionRepository = new LedgerTransactionRepository(tenant);
			List<TaxReportData> ledgerTransactons = ledgerTransactionRepository.GetLedgerTransactionsForTaxReport(taxReport.TaxReportMonth, tenant);
			GLAccountQueryService gLAccountQueryService = new GLAccountQueryService(tenant);
			FullAccountingSettingRepository fullAccountingSettingRepository = new FullAccountingSettingRepository(tenant);
			FullAccountingSetting setting = fullAccountingSettingRepository.GetSingleFullAccountingSetting(tenant);
			string VatNumber = null;
			decimal? VatAmount = null;
			decimal? InvoiceAmount = null;
			bool isEquipment = false;
			APInvoicePM aPInvoice = null;
			foreach (TaxReportData a in ledgerTransactons)
			{
				Simplog.Data.CommonDataModel.EntityPOCOs.Card card = cardRepository.GetCardByGLAccountId(a.OppositGLAccount, tenant);
				if (a.AccountingEntity == "4")
				{
					aPInvoice = aPInvoiceQueryService.GetSinglePM(a.AccountingEntityId, tenant);
					if (aPInvoice != null)
					{
						VatNumber = aPInvoice.VATNumber;
						//   VatAmount = aPInvoice.TotalVATs.Sum(d=> d.); ;
						//  InvoiceAmount = aPInvoice.tot
					}
				}
				else
				{
					VatAmount = a.LocalAmountDebit;

					if (card != null)
					{
						VatNumber = card.VatNumber;
					}
					else
					{

						VatNumber = "999999999";
					}

					InvoiceAmount = ledgerTransactons.Where(d => d.JournalId == a.JournalId && d.Reference == a.Reference).Sum(d => d.LocalAmountCredit);
				}

                if(VatNumber == null)
                {
                    VatNumber = "999999999";
                }

				GLAccountPM gLAccountPM = gLAccountQueryService.GetSinglePM(a.OppositGLAccount, tenant);
				if (gLAccountPM != null)
				{
					if (gLAccountPM.IsEquipmentVendor)
					{
						isEquipment = true;
					}

				}
				TaxReportLinePM taxReportLine = new TaxReportLinePM()
				{

					VatNumber = VatNumber,
					Reference = a.Reference,
					ReferenceDate = a.ReferenceDate,
					ReferecneGroup = "0000",
					JournalId = a.JournalId,
					OutputOrInput = "I",
					VatAmount = VatAmount,
					VatableInvoiceAmount = InvoiceAmount,
					IsEquipment = isEquipment,
					IsManuallyChanged = true,
					TaxReportId = taxReport.Id,
					ChangeSetOp = ChangeSetOperation.Insert,
					LastUpdateDateTime = DateTime.Now,
					UpdatedByUserId = taxReport.UpdatedByUserId,
					Tenant = tenant,


				};


				JournalQueryService journalQueryService = new JournalQueryService(tenant);
				JournalPM journal = journalQueryService.GetSingle(a.JournalId, true, false);
				string CreditAccountId = journal.JournalLines.FirstOrDefault().CreditAccountId;
				GLAccountPM account = gLAccountQueryService.GetSinglePM(CreditAccountId, tenant);


				if (aPInvoice != null)
				{
					if (aPInvoice.VATNumber == tenantPM.VatNumber)
					{
						taxReportLine.LineTypeCode = "C";
					}
				}
				else if (journal.JournalLines.Count > 0 && CreditAccountId == setting.CustomsGLAccountId)
				{

					taxReportLine.LineTypeCode = "R";

				}
				else if (card != null && card.IsAutonomy)
				{
					taxReportLine.LineTypeCode = "P";
				}

				else if (account != null && account.AccountTypeCode != "3")
				{
					taxReportLine.LineTypeCode = "K";
				}
				else
				{
					taxReportLine.LineTypeCode = "C";
				}

               
               

               



                taxReport.TaxReportLines.Add(taxReportLine);


			}

			if (taxReport.TaxReportLines.Count > 0)
			{
				taxReport.TaxableOutputAmount = taxReport.TaxReportLines.Where(d => d.OutputOrInput == "O" && d.VatAmount != 0).Sum(d => d.VatableInvoiceAmount);
				taxReport.OutputTaxAmount = taxReport.TaxReportLines.Where(d => d.OutputOrInput == "O" && d.VatAmount != 0).Sum(d => d.VatAmount);
				taxReport.ExemptTaxableOutput = taxReport.TaxReportLines.Where(d => d.OutputOrInput == "O" && d.VatAmount != 0 && d.StatusCode == "6").Sum(d => d.VatableInvoiceAmount);
				taxReport.OutputLinesCount = taxReport.TaxReportLines.Where(d => d.OutputOrInput == "O").Count();
				taxReport.OtherInputsTaxAmount = taxReport.TaxReportLines.Where(d => d.OutputOrInput == "I" && d.StatusCode == "6" && d.IsEquipment == false).Sum(d => d.VatAmount);
				taxReport.InputLinesCount = taxReport.TaxReportLines.Where(d => d.OutputOrInput == "I").Count();
                taxReport.EquipmentInputsTaxAmount = taxReport.TaxReportLines.Where(d => d.OutputOrInput == "I" && d.StatusCode == "6" && d.IsEquipment == true).Sum(d => d.VatAmount);
            }


			taxReport.TaxableOutputsWithDiffPercent = 0;
			taxReport.OutputTaxAmountWithDiffPercent = 0;

			taxReport.AmountForPayRefund = taxReport.OutputTaxAmount - (taxReport.OtherInputsTaxAmount + taxReport.EquipmentInputsTaxAmount);
            if(taxReport.AmountForPayRefund == null)
            {
                taxReport.AmountForPayRefund = 0;
            }
			taxReport.StatusCode = "D";
			taxReport.ProcessEndDate = DateTime.Now;


			taxReport.ChangeSetOp = ChangeSetOperation.Update;
			updateService.Update(taxReport, true);
		}

        public static BatchTaskExecutionPM CreatePNCFileInBatch(string taxReportId, int tenant)
        {
            BatchTaskExecutionPM taskExe;
            using (TransactionScope scope = TransactionFactory.GetTransaction())
            {
                // 1- create BTE record
                PNCFileArgs args = new PNCFileArgs() { ReportId = taxReportId, Tenant = tenant };
                var stringwriter = new System.IO.StringWriter();
                var serializer = new XmlSerializer(typeof(PNCFileArgs));
                serializer.Serialize(stringwriter, args);
                string xmlParameters = stringwriter.ToString();

                taskExe = new BatchTaskExecutionPM()
                {
                    Subject = "Create PNC Flat file for Tax Report",
                    Tenant = tenant,
                    ChangeSetOp = ChangeSetOperation.Insert,
                    ClassName = "Logitude.Accounting.BL.TestService.BatchTaxReportService,Logitude.Accounting.BL",
                    CreateDate = DateTime.Now,
                    PrametersXml = xmlParameters,
                    StatusCode = "C",

                };


                IInfrastructureContext MyContext = InfrastructureContext.GetContext(tenant);
                BatchTaskExecutionUpdateService bteUpdateService = new BatchTaskExecutionUpdateService(MyContext, new Dictionary<string, IContext>(), tenant);
                bteUpdateService.Update(taskExe, true);

                // 2- Send to queue
                IQueueService queueservice = new DbQueueService();
                queueservice.InitializeQueue("batchtaskexecutionqueue", 0);
                queueservice.Send(new Dictionary<string, string>()
                {
                    { "BatchTaskExecutionId", taskExe.Id },
                    { "Tenant", tenant.ToString() }
                });
                scope.Complete();
            }

            return taskExe;

        }

		public static DocumentsFilingPM CreatePNC874File(string taxReportId, int tenant)
        {
            //get tax report
            TaxReportQueryService reportQS = new TaxReportQueryService(tenant);
            TaxReportPM taxReport = reportQS.GetSingle(taxReportId, true, false);

            //DECLARATIONS
            List<string> linesArray = new List<string>();
			List<string> errors = new List<string>();


			if (errors.Count > 0)
			{
				string errorsString = "";
				foreach (var error in errors)
				{
					errorsString += error + ';';
				}
				throw new ApplicationException(errorsString.TrimEnd(';'));
			}

			//
			// Line: [1] 
			//
			string firstLine = "";
			firstLine += "O";

			if (taxReport.VatNumber.Length > 9) taxReport.VatNumber = taxReport.VatNumber.Substring(0, 9);
			firstLine += taxReport.VatNumber.PadLeft(9, '0');
			firstLine += taxReport.TaxReportMonth == null ? "000000" : taxReport.TaxReportMonth.ToString("yyyyMM");
			firstLine += "1";
			firstLine += taxReport.CreateDate.ToString("yyyyMMdd");

			//TotalTaxableOutputAmount
			firstLine += taxReport.TaxableOutputAmount >= 0 ? '+' : '-';
			firstLine += Math.Abs(Convert.ToInt32(taxReport.TaxableOutputAmount)).ToString().PadLeft(9, '0');

			//OutputTaxAmount
			firstLine += taxReport.OutputTaxAmount >= 0 ? '+' : '-';
			firstLine += Math.Abs(Convert.ToInt32(taxReport.OutputTaxAmount)).ToString().PadLeft(9, '0');

			//TaxableOutputsWithDiffPercent
			firstLine += '+';
			firstLine += Math.Abs(Convert.ToInt32(taxReport.TaxableOutputsWithDiffPercent)).ToString().PadLeft(9, '0');

			//OutputTaxAmountWithDiffPercent
			firstLine += '+';
			firstLine += Math.Abs(Convert.ToInt32(taxReport.OutputTaxAmountWithDiffPercent)).ToString().PadLeft(11, '0');

			//OutputLinesCount
			firstLine += taxReport.OutputLinesCount == null ? "000000000" : taxReport.OutputLinesCount.Value.ToString().PadLeft(9, '0');


			//ExemptTaxableOutput
			firstLine += taxReport.ExemptTaxableOutput >= 0 ? '+' : '-';
			firstLine += Math.Abs(Convert.ToInt32(taxReport.ExemptTaxableOutput)).ToString().PadLeft(11, '0');

			//OtherInputsTaxAmount
			firstLine += taxReport.OtherInputsTaxAmount >= 0 ? '+' : '-';
			firstLine += Math.Abs(Convert.ToInt32(taxReport.OtherInputsTaxAmount)).ToString().PadLeft(9, '0');

			//EquipmentInputsTaxAmount
			firstLine += taxReport.EquipmentInputsTaxAmount >= 0 ? '+' : '-';
			firstLine += Math.Abs(Convert.ToInt32(taxReport.EquipmentInputsTaxAmount)).ToString().PadLeft(9, '0');

			//InputLinesCount
			firstLine += taxReport.InputLinesCount == null ? "000000000" : taxReport.InputLinesCount.Value.ToString().PadLeft(9, '0');

			//AmountForPayRefund
			firstLine += taxReport.AmountForPayRefund >= 0 ? '+' : '-';
			firstLine += Math.Abs(Convert.ToInt32(taxReport.AmountForPayRefund)).ToString().PadLeft(11, '0');

			linesArray.Add(firstLine);

			//
			// Line: [report lines]
			//
			foreach (TaxReportLinePM itemPM in taxReport.TaxReportLines)
			{
				// validations
				if (itemPM.ReferenceDate == null) throw new ApplicationException("Reference Date is empty! line:" + itemPM.Line);

				//create line 
				string line = "";
				line += itemPM.LineTypeCode;

				if (itemPM.VatNumber == null) itemPM.VatNumber = "0";
				if (itemPM.VatNumber.Length > 9) itemPM.VatNumber = itemPM.VatNumber.Substring(0, 9);
				line += itemPM.VatNumber.PadLeft(9, '0');

				line += itemPM.ReferenceDate.Value.ToString("yyyyMMdd");

				if (itemPM.ReferecneGroup.Length > 4) itemPM.ReferecneGroup = itemPM.ReferecneGroup.Substring(0, 4);
				line += itemPM.ReferecneGroup.PadLeft(4, '0');

				if (itemPM.Reference.Length > 9) itemPM.Reference = itemPM.Reference.Substring(0, 9);
				line += itemPM.Reference.PadLeft(9, '0');

				//VatAmount
				line += Math.Abs(Convert.ToInt32(itemPM.VatAmount)).ToString().PadLeft(9, '0');

				//VatableInvoiceAmount
				line += itemPM.VatableInvoiceAmount >= 0 ? '+' : '-';
				line += Math.Abs(Convert.ToInt32(itemPM.VatableInvoiceAmount)).ToString().PadLeft(10, '0');


				line += "000000000";

				linesArray.Add(line);

			}


			//
			// Line: [last one]
			//
			string lastLine = "";
			lastLine += "X";
			lastLine += taxReport.VatNumber.PadLeft(9, '0');
			linesArray.Add(lastLine);



			//// write to a file
			//using (System.IO.StreamWriter file = new System.IO.StreamWriter(FilePath))
			//{
			//    foreach (string line in linesArray)
			//    {
			//        file.WriteLine(line);

			//    }
			//}

			//update entity
			IAccountingContext MyContext = AccountingContext.GetContext(tenant);
			TaxReportUpdateService updateService = new TaxReportUpdateService(MyContext, new Dictionary<string, IContext>(), tenant);
            taxReport.ChangeSetOp = ChangeSetOperation.Update;
			taxReport.StatusCode = "T"; // T- Transmitted
            taxReport.NeedsRebulid = false;
			updateService.Update(taxReport, true);

			DocumentsFilingPM docOut = CreateDocumnetFiling(linesArray, taxReport);

			return docOut;
		}

		private static DocumentsFilingPM CreateDocumnetFiling(List<string> linesArray, TaxReportPM taxReport, bool isFromWR=false)
		{
			// prepare file string
			string file = string.Join(Environment.NewLine, linesArray.Select(x => x.ToString()).ToArray());

			// create document
			int tenant = taxReport.Tenant;
			ICommonDataContext MyContext = CommonDataContext.GetContext(tenant);
			DocumentsFilingService docService = new DocumentsFilingService(MyContext, tenant);
			DocumentTypeRepository docTypeReposioty = new DocumentTypeRepository(MyContext);

			ObjectTableRepository tableRep = new ObjectTableRepository(tenant);
			ObjectTable table = tableRep.GetObjectTableByName("TaxReport", 0, true);

            // user
			User loggedUser = GetLoggedUser(tenant);
			DocumentType docType = docTypeReposioty.GetSingleDocumentTypeByCode("PCN874", tenant);

			string _code = CodeCounter.GetNumber("DocumentsFiling", tenant).ToString();
			DocumentsFilingPM document = new DocumentsFilingPM()
			{
				Description = "PCN874 Text File",
				DocumentTypeId = docType.Id,
				Tenant = tenant,
				DirectionCode = "I",
				EntityId = taxReport.Id,
				EntityNumber = taxReport.TaxReportNumber,
				ObjectTableId = table.Id,
				Code = _code,
				CreatedByUserId = loggedUser.Id,
				OwnerId = loggedUser.Id,
				CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
				UpdatedByUserId = loggedUser.Id,
				UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
				FileExtension = "txt",
				SecurityId = "100",
			};

			document.FileData = file.Select(d => Convert.ToByte(d)).ToArray();
			docService.Create(document, file.Select(d => Convert.ToByte(d)).ToArray(),loggedUser.Id);


			//get document out
			DocumentsFilingQuery queryService = new DocumentsFilingQuery(tenant);
			string code = (Convert.ToInt32(_code) + 1).ToString();
			DocumentsFilingPM docFiling = queryService.GetDocumentsFilingByDocumentCode(code, tenant);


			return docFiling;
		}

        private static Contact GetLoggedContact(int tenant)
        {
            ContactRepository contactRepository = new ContactRepository(tenant);
            Contact loggedContact;
            if (HttpContext.Current != null)
            {
                string email = HttpContext.Current.User.Identity.Name;
                loggedContact = contactRepository.GetSingleContactByEmail(email, tenant);
            }
            else
            {
                string systemContactEmail = "system@tenant" + tenant.ToString() + ".com";
                loggedContact = contactRepository.GetSingleContactByEmail(systemContactEmail, tenant);

            }
            return loggedContact;
        }

        private static User GetLoggedUser(int tenant)
        {
            UserRepository userRepository = new UserRepository(tenant);
            User loggedContact;
            if (HttpContext.Current != null)
            {
                string email = HttpContext.Current.User.Identity.Name;
                loggedContact = userRepository.GetSingleUserByCodeOrEmail(null, email, tenant, true);
            }
            else
            {
                string systemContactEmail = "system@tenant" + tenant.ToString() + ".com";
                loggedContact = userRepository.GetSingleUserByCodeOrEmail(null, "system@tenant" + tenant + ".com", tenant, true);

            }
            return loggedContact;
        }

    }
    public class PNCFileArgs
    {
        //public TaxReportPM ReportPM { get; set; }
        public string ReportId { get; set; }
        public int Tenant { get; set; }
    }
}
