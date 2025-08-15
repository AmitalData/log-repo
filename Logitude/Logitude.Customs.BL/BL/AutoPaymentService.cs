using Logitude.AmitalMessaging.Customs.CustomFile;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.Messaging.L2U.CustomFile;
using Logitude.Customs.Data.EntityListQueryServices;
using Logitude.Customs.Data.EntityLists;
using Logitude.Customs.Data;
using Logitude.Customs.Def.EntityPMs;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Logitude.Customs.BL.Messaging.U2L.Scheduler;
using Simplog.Server.Infrastructure.DataContracts;
using Logitude.Customs.BL.BL;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Logitude.Server.Tools;
using Logitude.BL.Security;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Xml.Linq;
using System.Xml;
using static System.Net.Mime.MediaTypeNames;
using Logitude.Customs.BL.Validators;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data.EntityPOCOs;
using System.Data.Entity;
using Simplog.Server.Infrastructure;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Logitude.Customs.BL.Messaging.Customs;
using Logitude.Customs.Data.EntityMapping;
using Logitude.Customs.BL.TraceEvents;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Contexts;

namespace Logitude.Customs.BL.BL
{
	public partial class AutoPaymentService
	{
		public DeclarationPM _MyDeclarationPM;
		public int _tenant;
	    public ICustomContext customContext;
		public AutoPaymentService(DeclarationPM declarationPM)
		{
			_MyDeclarationPM = declarationPM;
			_tenant = declarationPM.Tenant;
			customContext = CustomContext.GetContext(_tenant);
		}
		#region ValidateSend
		string ErrorMessage = "";
		bool IsError = false;


		public void ValidateBeforeCreatingPayment()
		{
			ErrorMessage = "";
			CustomsRequestsSheetQueryService customsRequestsSheetQueryService = new CustomsRequestsSheetQueryService(customContext);
			CustomsRequestsSheetPM requestsSheetPM = customsRequestsSheetQueryService.GetRequestInProgress(_tenant, "2755", ObjectTableRepository.GetObjectTableByName("Customs.Declaration"), _MyDeclarationPM.Id, null, null, null, false).FirstOrDefault();
			if (requestsSheetPM != null)
			{
				ErrorMessage = "קיימת בקשת תשלום בתהליך";//קיימת בקשת תשלום בתהליך
				throw new Exception(ErrorMessage);
			}
			if (_MyDeclarationPM.IsChanged)
			{

				ErrorMessage = TranslateTextsClass.Translate("Customs.Declaration.O.ChangedDeclaration", _MyDeclarationPM.Tenant, true);//בוצעו שינויים בהצהרה , יש לשדר שוב לפני הגשת תשלום
				throw new Exception(ErrorMessage);
			}

			if (_MyDeclarationPM.PaymentDate != null)
			{

				ErrorMessage = TranslateTextsClass.Translate("Customs.Declaration.O.PaidDeclaration", _MyDeclarationPM.Tenant, true);//הצהרה כבר שולמה , המסך לתצוגה בלבד
				throw new Exception(ErrorMessage);

			}

			if (_MyDeclarationPM.DeclarationStatusTypeCode == "11")
			{

				ErrorMessage = TranslateTextsClass.Translate("Customs.Declaration.O.WaitingApproval", _MyDeclarationPM.Tenant, true);//טיוטה הוגשה , ממתינה לאילוץ הגשה
				throw new Exception(ErrorMessage);

			}

			if (_MyDeclarationPM.DeclarationStatusTypeCode == "10")
			{

				ErrorMessage = TranslateTextsClass.Translate("Customs.Declaration.O.FuturePayment", _MyDeclarationPM.Tenant, true);//בוצעה הגשה עתידית
				throw new Exception(ErrorMessage);

			}

			if (_MyDeclarationPM.DeclarationStatusTypeCode == "14")
			{

				ErrorMessage = TranslateTextsClass.Translate("Customs.General.O.SubmitDeclarationAgain", _MyDeclarationPM.Tenant, true);//סטטוס הצהרה מחייב שליחה מחדש למכס
				throw new Exception(ErrorMessage);

			}
			if (_MyDeclarationPM.DeclarationStatusTypeCode == "12")
			{

				ErrorMessage = "טיוטה שגויה";
				throw new Exception(ErrorMessage);

			}
			CheckTotals();
			if (_MyDeclarationPM.AmendmentMessage != null && _MyDeclarationPM.AmendmentMessage != "")
			{
				{
					//this.IsDisplayMessage = true;
					this.ErrorMessage = _MyDeclarationPM.AmendmentMessage;
					if (_MyDeclarationPM.IsAmendmentDisplayOnly) IsError = _MyDeclarationPM.IsAmendmentDisplayOnly;
					throw new Exception(ErrorMessage);

				}
			}
			ValidateCheckError();
			CheckSingValidation();
		}

		private void CheckTotals()
		{

			var totalTax = 0.0;
			if (_MyDeclarationPM.TotalTax == null)
			{
				totalTax = 0;
			}
			else
			{
				totalTax = (double)_MyDeclarationPM.TotalTax;
			}
			// this.DeclarationPM.TotalTax = 0;
			if (_MyDeclarationPM.DeclarationTaxes.Count() > 0)
			{

				var sum = 0.0;
				_MyDeclarationPM.DeclarationTaxes.ForEach(el => { sum += (double)el.TotalAmount; });

				if (totalTax != sum)
				{

					ErrorMessage = TranslateTextsClass.Translate("Customs.Declaration.O.DifferentTotals", _MyDeclarationPM.Tenant, true);//המס לתשלום שונה מהמיסים לתיק , המסך לתצוגה בלבד
					throw new Exception(ErrorMessage);

				}
			}

			else
			{
				if (totalTax > 0)
				{

					ErrorMessage = TranslateTextsClass.Translate("Customs.Declaration.O.DifferentTotals", _MyDeclarationPM.Tenant, true);//המס לתשלום שונה מהמיסים לתיק , המסך לתצוגה בלבד
					throw new Exception(ErrorMessage);
				}
			}
		}
		public void ValidateCheckError()
		{
			if (_MyDeclarationPM.IsCancelled)
			{

				// the declaration is cancelled 

				ErrorMessage = TranslateTextsClass.Translate("Customs.Declaration.O.Cancelled", _MyDeclarationPM.Tenant, true);//נתונים לתצוגה בלבד – ההצהרה מבוטלת
				throw new Exception(ErrorMessage);
			}
			if (_MyDeclarationPM.CancelRequestStatusCode == "2")
			{


				ErrorMessage = TranslateTextsClass.Translate("Customs.Declaration.O.CancelRequestSent", _MyDeclarationPM.Tenant, true);//נשלח מסר ביטול הצהרה - ממתין לטיפול
				throw new Exception(ErrorMessage);

			}
			ValidDeclarationChecks();
			if (ValidationErrorMessageCodes.Count() > 0)
			{

				ErrorMessage = TranslateTextsClass.Translate(ValidationErrorMessageCodes[0], _MyDeclarationPM.Tenant, true);
				throw new Exception(ErrorMessage);
			}
			var objectTableDecId = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
			var displayOnlyCheckResult = GetRequestInProgress(_MyDeclarationPM.Tenant, "DCAMU", objectTableDecId, _MyDeclarationPM.Id, "", "", "", true);

			if (displayOnlyCheckResult != null && displayOnlyCheckResult.Count() > 0)
			{
				CustomsRequestsSheetPM customsRequestsSheetPM = displayOnlyCheckResult.FindAll(r => r.InterfaceTypeCode == "DCAMU").FirstOrDefault();
				if (customsRequestsSheetPM != null)
				{

					string errorMessage = "קיימת בקשה לעדכון קוד תהליך/הנחה פטור ";
					ErrorMessage = errorMessage;

					throw new Exception(ErrorMessage);
				}
			}
			var requestSheets = GetRequestInProgress(_MyDeclarationPM.Tenant, "2750", "", "", "", "", _MyDeclarationPM.CustomFileNo, true);
			if ((requestSheets == null || requestSheets.Count() == 0))
				return;
			if (requestSheets[0].InterfaceTypeCode == null)
				return;

			if (requestSheets[0].InterfaceTypeCode == "2755" && requestSheets[0].FutureSendDateTime != null)
			{
				DateTime? myFutureSendDateTime;
				myFutureSendDateTime = requestSheets[0].FutureSendDateTime;

				if (myFutureSendDateTime > DateTime.Now)
				{
					string stringDatetime = myFutureSendDateTime?.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss");

					string text1 = " הוגדרה בקשה מתוזמנת לתאריך" + stringDatetime +
					"-לא ניתן להמשיך עד לסיום טיפול או ביטול הבקשה";


					ErrorMessage = text1;
					throw new Exception(ErrorMessage);
				}
				var RequestInProgressInterfaceTypeName = requestSheets[0].InterfaceTypeName;
				var text = TranslateTextsClass.Translate("Customs.General.RequestInProgress", _MyDeclarationPM.Tenant, true);
				text = text.Replace("{0}", RequestInProgressInterfaceTypeName);

				ErrorMessage = text;
				throw new Exception(ErrorMessage);

			}
			else
			{
				if (requestSheets[0].InterfaceTypeCode == "DCAUAC")
				{
					string errorMessage = "קיימת בקשה לעדכון פטור 92 גורף ";

					ErrorMessage = errorMessage;
					throw new Exception(ErrorMessage);
				}
				var RequestInProgressInterfaceTypeName = requestSheets[0].InterfaceTypeName;
				var text = TranslateTextsClass.Translate("Customs.General.RequestInProgress", _MyDeclarationPM.Tenant, true);
				text = text.Replace("{0}", RequestInProgressInterfaceTypeName);

				ErrorMessage = text;
				throw new Exception(ErrorMessage);
			}

		}

		List<string> ValidationErrorMessageCodes = new List<string>();
		public void ValidDeclarationChecks()
		{
			this.PaymentDateCheck();
	
			this.CheckIsConvertedDeclaration(); 
			this.CheckIsCloseDeclaration();
			this.CheckIfAutomaticPayment();
		}
		//Check if declaration was already paid
		public void PaymentDateCheck()
		{
			string errorMessage = "";

			if (_MyDeclarationPM != null)
			{


				if (_MyDeclarationPM.IsExportClosed && _MyDeclarationPM.DeclarationStatusTypeCode == "36")
				{
					errorMessage = "Customs.General.O.DeclarationStatClosed";//הצהרה נסגרה עם סטאטוס סגור
					if (!string.IsNullOrEmpty(errorMessage))
					{
						this.ValidationErrorMessageCodes.Add(errorMessage);
					}

				}

			}
		}		

		//Check if it's a converted declaration (IsConvertedDeclaration=True)  // Mirit 02/12/15 Task 18508
		public void CheckIsConvertedDeclaration()
		{

			if (_MyDeclarationPM != null)
			{
				if (_MyDeclarationPM.IsConvertedDeclaration == true && _MyDeclarationPM.IsAmendment != true)
				{
					var errorMessage = "Customs.General.O.IsConvertedDeclaration";
					if (!string.IsNullOrEmpty(errorMessage))
					{
						this.ValidationErrorMessageCodes.Add(errorMessage);
					}
				}
			}
		}
		//Check if declaration is close
		public void CheckIsCloseDeclaration()
		{
			string errorMessage = "";

			if (_MyDeclarationPM != null)
			{
				if (_MyDeclarationPM.IsClose && _MyDeclarationPM.Direction != "E")
				{
					errorMessage = "Customs.Declaration.O.Closed";
					if (!string.IsNullOrEmpty(errorMessage))
					{
						this.ValidationErrorMessageCodes.Add(errorMessage);
					}
				}
				//if (_MyDeclarationPM.IsClose && _MyDeclarationPM.Direction == "E")
				//{
				//	errorMessage = "Customs.Declaration.O.OperationallyClosed";
				//	if (!string.IsNullOrEmpty(errorMessage))
				//	{
				//		this.ValidationErrorMessageCodes.Add(errorMessage);
				//	}
				//}
			}
		}
		public void CheckIfAutomaticPayment()
		{

			string errorMessage = "";

			if (_MyDeclarationPM != null)
			{
				if (_MyDeclarationPM.AutomaticPayment != null&& _MyDeclarationPM.AutomaticPayment > 0)
				{
					//"הצהרה בתהליך תשלום םוטומטי - לתצוגה בלבד"
					errorMessage = "Customs.General.O.InAutomaticPayment";
					if (!string.IsNullOrEmpty(errorMessage))
					{
						this.ValidationErrorMessageCodes.Add(errorMessage);
					}
				}
			}


		}
		public List<CustomsRequestsSheetPM> GetRequestInProgress(int Tenant,
		  string InterfaceTypeCode,
		  string ObjectTableId1, string EntityId1,
		  string ObjectTableId2, string EntityId2,
		  string CustomFileNo,
		  bool displayOnlyMode)
		{
			try
			{
				InterfaceTypeCode = InterfaceTypeCode.Trim();//why " 2750"

				ICustomContext customContext = CustomContext.GetContext(_tenant);
				Boolean include8250IsShaam = false;
				if (InterfaceTypeCode == "2750" && !string.IsNullOrWhiteSpace(CustomFileNo))
				{
					var decQS = new DeclarationQueryService(customContext);

					var declaration = decQS.GetSingleByCustomFileNoFromCache(CustomFileNo, _tenant);
					if (declaration != null)
					{
						include8250IsShaam = declaration.ProcedureCurrentCode == "4070001"; //"ProcedureCurrentCode":"4070001","ProcedureCurrentName":"יבוא מסחרי-שח\"מ
					}


				}
				CustomsRequestsSheetQueryService customsRequestsSheetQuery = new CustomsRequestsSheetQueryService(customContext);
				//List<CustomsRequestsSheetPM> requestSheets = customsRequestsSheetQuery.GetRequestInProgress(Tenant, InterfaceTypeCode, ObjectTableId1, EntityId1,
				//    ObjectTableId2, EntityId2,
				//    CustomFileNo, displayOnlyMode);

				List<CustomsRequestsSheetPM> requestSheets = customsRequestsSheetQuery.GetRequestInProgress(new RequestInProgressParams()
				{
					Tenant = Tenant,
					InterfaceTypeCode = InterfaceTypeCode,
					ObjectTableId1 = ObjectTableId1,
					EntityId1 = EntityId1,
					ObjectTableId2 = ObjectTableId2,
					EntityId2 = EntityId2,
					CustomFileNo = CustomFileNo,
					DisplayOnlyMode = displayOnlyMode,
					Include8250IsShaam = include8250IsShaam
				});
				var payRequest = requestSheets.FirstOrDefault(r => r.InterfaceTypeCode == "2755");
				if (payRequest != null)
				{
					var communicationLogStepQuery = new CommunicationLogStepQuery(_tenant);
					var stepList = communicationLogStepQuery
						.GetCommunicationLogStepsDocumentData(payRequest.RequestComminicationId, _tenant, new int[] { 0 }, true, false);
					var xdoc = XDocument.Parse(stepList.First().DocumentData);
					var FutureSendDateTime = xdoc.Descendants("FutureSendDateTime").FirstOrDefault();
					if (FutureSendDateTime != null)
					{
						try
						{
							payRequest.FutureSendDateTime = XmlConvert.ToDateTime(FutureSendDateTime.Value);
						}
						catch (Exception e)
						{
							try
							{
								//  <FutureSendDateTime xsi:nil="true" />Logger.LogMe("FutureSendDateTime : " + stepList.First().DocumentData, true);
							}
							catch (Exception)
							{

								///throw;
							}

						}

					}
				}
				//From Declaration EditComponent Return //Fast as posibble 
				if (InterfaceTypeCode.Trim() == "2750" && !string.IsNullOrWhiteSpace(CustomFileNo))
				{
					requestSheets = requestSheets ?? new List<CustomsRequestsSheetPM>();
					var firstReq = requestSheets.FirstOrDefault();
					//var repo = new DeclarationRepository(customContext); // stopped the concurrency code and returned only a dummy invoice 4 me 
					//var ConcurrencyGUID = repo.GetConcurrencyGUIDByCustomFileNo(CustomFileNo, tenant);
					//if (!String.IsNullOrWhiteSpace(ConcurrencyGUID))
					//{
					if (firstReq == null)
					{
						firstReq = new CustomsRequestsSheetPM();// DUMMY 4 MOHAMMAD 
						requestSheets.Add(firstReq);
					}
					//firstReq.MainEntityConcurrencyGUID = ConcurrencyGUID;
					//}
				}



				return requestSheets;


			}

			catch (Exception ex)
			{
			}
			return null;
		}



		#endregion

		#region LogicFillAutoPayment
		
		public bool isAutoFill = false;
		public bool BetweenMinAndMax = false;
		public double sumBtl = 0.0;
		public decimal TotalAmount;
		public bool CreateEntity = false;
		//public DateTime? FuturePaymentTime;
		public string _PayWithProtest_Default;
		public string GetCreditInternalBankId;
		public DeclarationPaymentPM paymentPM;
		public PaymentMethodModel paymentMethodModelMax;
		public List<PaymentMethodModel> PaymentMethodsList = new List<PaymentMethodModel>();
		public List<DeclarationPaymentProtestPM> PaymentProtestsList = new List<DeclarationPaymentProtestPM>();
		public List<string> ListMethodType = new List<string> { "2", "79" };
		public DeclarationPaymentPM CreateDeclarationPaymentByLogic(bool IsFromClient)
		{
			
			DefaultValueQueryService defaultValueQueryService = new DefaultValueQueryService(_tenant);
			var objCGG_PAYHAND_FIL = defaultValueQueryService.GetDefault("ISRAEL", "CGG_PAYHAND_FIL", "NON", "NON", _tenant);//מילוי מסך הגשת תשלום

			if (!string.IsNullOrEmpty(objCGG_PAYHAND_FIL))
			{
				if (objCGG_PAYHAND_FIL == "A")
				{
					if (PaymentMethodsList != null)
					{
						isAutoFill = true;
						var objCGG_PAY_AMT_RNG = defaultValueQueryService.GetDefault("ISRAEL", "CGG_PAY_AMT_RNG", "NON", "NON", _tenant);//הגדרת סכום מינימום ומקסימום לתשלום מיסים בקופה
						if (!string.IsNullOrEmpty(objCGG_PAY_AMT_RNG))
						{
							string[] MinAndMax = objCGG_PAY_AMT_RNG.Split('-');
							var min = decimal.Parse(MinAndMax[0].Replace(",", ""));
							var max = decimal.Parse(MinAndMax[1].Replace(",", ""));
							if (min < _MyDeclarationPM.TotalTax && max > _MyDeclarationPM.TotalTax)
							{
								BetweenMinAndMax = true;
							}
						}

					}
				}
			}
			FeatureQuery featureQuery = new FeatureQuery(_tenant);
			var features = featureQuery.GetAllowedFeaturesForLoggedUser(AuthenticationUtil.ResolveUserId(_tenant), _tenant);
			var featureBTPA = features.Features.FirstOrDefault(x => x.Code == "BTPA");
			if ((_MyDeclarationPM.ImporterEntitlementTypeCode == "17" || _MyDeclarationPM.ImporterEntitlementTypeCode == "18") && featureBTPA != null)
			{
				DeclarationQueryService declarationQuery = new DeclarationQueryService(customContext);
				SupplierInvoiceQueryService supplierInvoiceQuery = new SupplierInvoiceQueryService(customContext);
				List<SupplierInvoicePM> supplierInvoices = supplierInvoiceQuery.GetSupplierInvoicesForDeclaration(_MyDeclarationPM.Id, _tenant);
				int count = declarationQuery.GetInvoiceItemsWithTradeAgreementCount(_MyDeclarationPM.Id, _tenant);
				//new { SupplierInvoices = supplierInvoices, Count = count }
				if (supplierInvoices != null)
				{

					supplierInvoices.ForEach(el =>
					{
						if (el.SupplierInvoiceItems != null && el.SupplierInvoiceItems.Count() > 0)
						{
							el.SupplierInvoiceItems.ForEach(si =>
							{
								if (si.SupplierInvoiceItemTaxes != null && si.SupplierInvoiceItemTaxes.Count() > 0)
								{
									si.SupplierInvoiceItemTaxes.ForEach(it =>
									{
										if (it.TotalBtlCoverageNIS != null)
											sumBtl += (double)it.TotalBtlCoverageNIS;
									});

								}
							});

						}
					});

				}


			}


			this.LoadPayment();
			
			this.CalculateTotalAmount();
			var totalAmount = Math.Round(TotalAmount, 2);
			var totalTax = Math.Round((decimal)_MyDeclarationPM?.TotalTax, 2);

			if (totalAmount != totalTax)
			{
				var text = TranslateTextsClass.Translate("Customs.Declaration.O.Totalmustbeequaltototaltax", _MyDeclarationPM.Tenant, true);//סה''כ חייב להיות שווה למס הכולל
				throw new Exception(text);                                                                                       
			}
			DateTime? Requestdate = CheckIfBlockTime();
			Requestdate = Requestdate != DateTime.MinValue ? Requestdate : null;
			paymentPM.FuturePaymentDateTime = Requestdate;

			FillSignData();

			if (!IsFromClient)
			{
				if (PaymentMethodsList?.Count() > 0 && PaymentMethodsList[0]?.BanksList?.Count() > 1 && PaymentMethodsList[0]?.BanksList?.FindAll(x => !x.InActive && x.PayerTypeCode == PaymentMethodsList[0].PayerActivityTypeCode).Count() > 1 &&
					 PaymentMethodsList[0]?.BanksList?.Any(x => !x.InActive && (x.Id == PaymentMethodsList[0].InternalBankId)) == false &&
					!(ListMethodType.Contains( PaymentMethodsList[0].MethodTypeCode)))
				{
					throw new Exception("ישנם ריבוי בנקים");

				}
				paymentPM.ChangeSetOp = this.CreateEntity ? ChangeSetOperation.Insert : ChangeSetOperation.Update;
				this.CreateEntity = false;

				DeclarationPaymentUpdateService declarationPaymentUpdateService = new DeclarationPaymentUpdateService(customContext, new Dictionary<string, IContext>(), _tenant);
				declarationPaymentUpdateService.Update(paymentPM, true);
			}
			return paymentPM;
		}
		public void LoadPayment()
		{

			paymentPM = GetSingleDeclarationPaymentPM();
			if (paymentPM != null && paymentPM.DeclarationPaymentMethods?.Count() > 0) { 
				SetTotalTax();
				FillGridsData();

				return;
			}
			GetGoldPaymentDefaults(_MyDeclarationPM.CustomerCode);//await - sync
			SetDefaultExplain(_MyDeclarationPM.CustomerCode);
			loadPaymentCompleted();

		}

		public string CustomerDefaultGoldPay_CIM_GOLD_PAY = null;
		public string CompanyDefaultMaxPayMASAV_CGG_MAX_AGT_PAY = null;
		public string CompanyDefaultaboveamountagentCash_CGG_ABOVE_AGT_C = null;
		private void GetGoldPaymentDefaults(string CustomerCode)
		{
			//CIM_GOLD_PAY CGG_MAX_AGT_PAY
			DefaultValueQueryService defaultValueQueryService = new DefaultValueQueryService(_tenant);
			if (!string.IsNullOrWhiteSpace(CustomerCode))
			{
				///דיפולט באינדקס לקוח "תשלום בניצול העברת זהב לקוח "
				CustomerDefaultGoldPay_CIM_GOLD_PAY = defaultValueQueryService.GetDefault("ISRAEL", "CIM_GOLD_PAY", "NON", CustomerCode, _tenant);//תשלום בניצול העברת זהב לקוח או קופה

			}
			///דיפולט ברמת חברה "סכום מיסים מקסימלי לתשלום במס"ב סוכן
			CompanyDefaultMaxPayMASAV_CGG_MAX_AGT_PAY = defaultValueQueryService.GetDefault("ISRAEL", "CGG_MAX_AGT_PAY", "NON", "NON", _tenant);//סכום מיסים מקסימלי לתשלום במס"ב סוכן

			//סכום שמעל יבוצע תשלום בקופה סוכן"
			CompanyDefaultaboveamountagentCash_CGG_ABOVE_AGT_C = defaultValueQueryService.GetDefault("ISRAEL", "CGG_ABOVE_AGT_C", "NON", "NON", _tenant);//סכום שמעל יבוצע תשלום בקןפת סוכן

		}
		private DeclarationPaymentPM GetSingleDeclarationPaymentPM()
		{
			DeclarationPaymentQueryService declarationPaymentQueryService = new DeclarationPaymentQueryService(_tenant);

			DeclarationPaymentPM declarationPaymentPM = declarationPaymentQueryService.GetSingle(_MyDeclarationPM.Id, true, false);
			return declarationPaymentPM;
		}

		private void SetTotalTax()
		{
			if (paymentPM.DeclarationPaymentMethods.Count() > 1)
				throw new Exception("ישנו יותר מאמצעי תשלום אחד לא  ניתן לבצע תשלום אוטומטי");

			paymentPM.PaymentDate = DateTime.Now;
			paymentPM.DeclarationPaymentMethods[0].Amount = _MyDeclarationPM.TotalTax;
			paymentPM.DeclarationPaymentMethods[0].ChangeSetOp = ChangeSetOperation.Update;

		}
		private void SetDefaultExplain(string CustomerCode)
		{
			if (!string.IsNullOrWhiteSpace(CustomerCode))
			{
				if (paymentPM == null || paymentPM.DeclarationPaymentProtests == null || paymentPM.DeclarationPaymentProtests.Count == 0 || string.IsNullOrWhiteSpace(paymentPM.DeclarationPaymentProtests.FirstOrDefault().CustomsAgentExplanation))
				{
					DefaultValueQueryService defaultValueQueryService = new DefaultValueQueryService(_tenant);
					string customsAgentExplanationDefault = defaultValueQueryService.GetDefault("ISRAEL", "CIM_PROTEST_PAY", "NON", CustomerCode, _tenant);//דיפולט תשלום באגב מחאה

					if (!string.IsNullOrWhiteSpace(customsAgentExplanationDefault))
					{
						if (paymentPM == null)
						{
							paymentPM = new DeclarationPaymentPM()
							{								
								ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
								CustomsAgentExplanationDefault = customsAgentExplanationDefault,
							};
						}

						else if (paymentPM.DeclarationPaymentProtests == null || paymentPM.DeclarationPaymentProtests.Count() == 0 || string.IsNullOrWhiteSpace(paymentPM.DeclarationPaymentProtests.FirstOrDefault().CustomsAgentExplanation))
						{
							paymentPM.CustomsAgentExplanationDefault = customsAgentExplanationDefault;
						}
					}
				}
			}
		}
		private void loadPaymentCompleted()
		{

			//this.FillGridsData();

			// create new entity if there is no payment
			if (paymentPM == null)
			{
				this.paymentPM = new DeclarationPaymentPM();
			}

			if (paymentPM != null && string.IsNullOrEmpty(this.paymentPM.DeclarationId))
			{
				this.paymentPM.DeclarationId = _MyDeclarationPM.Id;
				this.paymentPM.Tenant =_tenant;
				this.paymentPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
				this.CreateEntity = true;
			}

			this.PostSendCreditToGetBank();

			if (paymentPM.FuturePaymentDateTime != null)
			{
				SetFuturePaymentTime(this.paymentPM.FuturePaymentDateTime);
			}


			// PayWithProtest Default
			if (!string.IsNullOrEmpty(paymentPM.CustomsAgentExplanationDefault))
			{
				this._PayWithProtest_Default = this.paymentPM.CustomsAgentExplanationDefault;
				if (this.paymentPM.DeclarationPaymentProtests == null || this.paymentPM.DeclarationPaymentProtests.Count() == 0)
				{
					this.NewProtestMethod();
				}
				else if (string.IsNullOrEmpty(this.paymentPM.DeclarationPaymentProtests[0].CustomsAgentExplanation))
				{
					this.paymentPM.DeclarationPaymentProtests[0].CustomsAgentExplanation = this._PayWithProtest_Default;
				}
			}

			
			initDates();

		}
		private void NewProtestMethod()
		{
			this.NewProtestClicked();
		}
		private void NewProtestClicked()
		{

			var line = 0;

			if (this.paymentPM.DeclarationPaymentProtests.Count() > 0)
			{

				line = this.paymentPM.DeclarationPaymentMethods.Max(d => d.Line);
			}

			line++;

			var protest = new DeclarationPaymentProtestPM();
			protest.Tenant = _tenant;
			protest.DeclarationId = _MyDeclarationPM.Id;
			protest.Line = line;
			protest.CustomsAgentExplanation = this._PayWithProtest_Default;

			if (!this.paymentPM.DeclarationPaymentProtests.Contains(protest))
			{
				this.paymentPM.DeclarationPaymentProtests.Add(protest);
			}

			var item = PaymentProtestModel(protest);
			this.PaymentProtestsList.Add(item);

		}

		public void FillGridsData()
		{

			// PaymentMethods List
			if (paymentPM != null)
			{
				foreach (var item in paymentPM.DeclarationPaymentMethods)
				{
					var itemModel = new PaymentMethodModel(this);
					itemModel.DeclarationId = item.DeclarationId;
					itemModel.Line = item.Line;
					itemModel.SequenceNumeric = item.SequenceNumeric;
					itemModel.PayerActivityTypeCode = item.PayerActivityTypeCode;
					itemModel.MethodTypeCode = item.MethodTypeCode;
					itemModel.Amount = item.Amount;
					itemModel.BankCode = item.BankCode;
					itemModel.BranchCode = item.BranchCode;
					itemModel.AccountNumber = item.AccountNumber;
					itemModel.Tenant = item.Tenant;
					itemModel.InternalBankId = item.InternalBankId;
					itemModel.CustomsBranchId = item.CustomsBranchId;
					//itemModel.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
					PaymentMethodsList.Add(itemModel.LogicPaymentMethodModel());


				}
			}

			// Protests List
			if (paymentPM != null)
			{
				foreach (var item in paymentPM.DeclarationPaymentProtests)
				{
					//this.PaymentProtestsList.Insert(new PaymentProtestModel(item, this));
					PaymentProtestsList.Add(PaymentProtestModel(item));
				}
			}

			this.CalculateTotalAmount();

		}
		private void CalculateTotalAmount()
		{
			decimal total = 0;
			if (paymentPM.DeclarationPaymentMethods != null)
			{
				paymentPM.DeclarationPaymentMethods.ForEach(method =>
				{
					total += method.Amount == null ? 0 : (decimal)method.Amount;

				});

			}

			this.TotalAmount = total;
		}



		private DeclarationPaymentProtestPM PaymentProtestModel(DeclarationPaymentProtestPM declarationPaymentProtestPM)
		{
			return null;
		}
		private void PostSendCreditToGetBank()
		{

			if (_MyDeclarationPM.PaymentDate != null)
			{
				return;
			}
			var objectTableDecId = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
			var searchParams = new CustomFileCreditRequestParams();
			{
				searchParams.Tenant = _MyDeclarationPM.Tenant;
				searchParams.AppicationId = _MyDeclarationPM.Id;//this.entityParent.DeclarationId;
				searchParams.LoggingEnabled = true;
				searchParams.LoggingEntityId = _MyDeclarationPM.Id; //entityParent.DeclarationId;
				searchParams.LoggingObjectTableId = objectTableDecId;
				searchParams.LoggingUserId = AuthenticationUtil.ResolveUserId(_MyDeclarationPM.Tenant);
				searchParams.RequestName = "Send Credit to Get Bank Request";
				searchParams.ResponseName = "Get Credit to Get Bank Response";
				searchParams.Mode = "GetBank";
			}

			searchParams.RequestVIA = SendRequestVIA.Default;



			CustomFileCreditResponseData customFileCreditResponseData = this.CustomFileCredit(searchParams);

			DateTime newDate = DateTime.Now;
			DateTime currentDate = new DateTime(newDate.Year, newDate.Month, newDate.Day, newDate.Hour, newDate.Minute, 0); // last of today
			var paymentDate = this.paymentPM.PaymentDate;

			if (customFileCreditResponseData.PaymentDateTime == null)
			{
				customFileCreditResponseData.PaymentDateTime = currentDate;
			}
			var paymentDateTime = customFileCreditResponseData.PaymentDateTime;

			if (paymentDateTime <= currentDate)
			{
				if (paymentDate > currentDate)
				{
					this.paymentPM.PaymentDate = paymentDate;
				}
				else
				{
					this.paymentPM.PaymentDate = customFileCreditResponseData.PaymentDateTime;
				}
				this.paymentPM.FuturePaymentDateTime = newDate;
				SetFuturePaymentTime(null);

			}
			else
			{
				this.paymentPM.PaymentDate = currentDate;
				this.paymentPM.FuturePaymentDateTime = customFileCreditResponseData.PaymentDateTime;
				SetFuturePaymentTime(customFileCreditResponseData.PaymentDateTime);
			}
			if (!string.IsNullOrEmpty(customFileCreditResponseData.BankCode))
			{


				CustomBankListQueryService customBankQuery = new CustomBankListQueryService(customContext);
				List<CustomBankList> result = customBankQuery.GetList(_MyDeclarationPM.Tenant);
				CustomBankList bank = result.FindAll(d => d.InternalCode == customFileCreditResponseData.BankCode && !d.InActive).FirstOrDefault();
				if (bank != null)
				{
					this.GetCreditInternalBankId = bank.Id;
				}
			}
			if (paymentPM.DeclarationPaymentMethods == null || this.paymentPM.DeclarationPaymentMethods.Count() == 0)
			{
				this.AutoFillPaymentByDefault();
			}


		}
		public void AutoFillPaymentByDefault()
		{
			this.NewPaymentMethod();
			if (this.sumBtl != null && this.sumBtl > 0)
			{
				this.JustAutoFillPaymentScreen();
			}
			else
			{

				if (!string.IsNullOrEmpty(this.GetCreditInternalBankId))
				{
					if (this.PaymentMethodsList != null && this.PaymentMethodsList.Count() > 0)
					{
						this.JustAutoFillPaymentScreen();
						return;
					}
				}
				DefaultValueQueryService defaultValueQueryService = new DefaultValueQueryService(_MyDeclarationPM.Tenant);

				string objCIM_PAYCASH_FIL = defaultValueQueryService.GetDefault("ISRAEL", "CIM_PAYCASH_FIL", "NON", _MyDeclarationPM.CustomerCode, _MyDeclarationPM.Tenant);//תשלום הצהרה בקופה
				if (!string.IsNullOrEmpty(objCIM_PAYCASH_FIL))
				{
					if ((this.PaymentMethodsList != null && this.PaymentMethodsList.Count() > 0) || this.paymentMethodModelMax != null)
					{
						this.AutoFillPaymentCash(objCIM_PAYCASH_FIL);
					}
				}
				else
				{
					this.AutoFillPaymentScreen();
				}

			}


		}
		private void NewPaymentMethod()
		{
			var item = new PaymentMethodModel(this);
			item.Tenant = _MyDeclarationPM.Tenant;
			item.DeclarationId = _MyDeclarationPM.Id;
			item.Line = 1;
			item.SequenceNumeric = 1;
			item.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
			this.paymentPM.DeclarationPaymentMethods.Add(item);
			if (this.BetweenMinAndMax && !(this.sumBtl != null && this.sumBtl > 0))
			{

				this.paymentMethodModelMax = item.LogicPaymentMethodModel();
			}
			else
			{
				var itemModel = item.LogicPaymentMethodModel();
				this.PaymentMethodsList.Add(itemModel);

			}
		}
		private void JustAutoFillPaymentScreen()
		{

			if (this.sumBtl != null && this.sumBtl > 0)
			{
				foreach (var method in this.PaymentMethodsList)
				{
					method.Amount = (decimal)_MyDeclarationPM.TotalTax - (decimal)this.sumBtl;
					method.SetMethodTypeCode("1");

				}
				this.NewMethodMethod(true);
				this.PaymentMethodsList[this.PaymentMethodsList.Count() - 1].Amount = (decimal?)this.sumBtl;
			}
			else
			{
				foreach (var method in this.PaymentMethodsList)
				{
					method.Amount = _MyDeclarationPM.TotalTax;
					method.SetMethodTypeCode("1");

				}

				if (this.paymentMethodModelMax != null)
				{
					this.paymentMethodModelMax.Amount = _MyDeclarationPM.TotalTax;
					this.paymentMethodModelMax.SetMethodTypeCode("1");

				}
			}
		}


		//תשלום בקופה אם יש דיפולט של קופה
		private void AutoFillPaymentCash(string defaultValue)
		{

			if (this.sumBtl != null && this.sumBtl > 0)
			{
				foreach (var method in this.PaymentMethodsList)
				{
					method.Amount = (decimal?)_MyDeclarationPM.TotalTax - (decimal?)this.sumBtl;
					method.SetMethodTypeCode("2");
					method.PayerActivityTypeCode = defaultValue;

				}
				this.NewMethodMethod();
				this.PaymentMethodsList[this.PaymentMethodsList.Count() - 1].Amount = (decimal?)this.sumBtl;

			}
			else
			{
				foreach (var method in this.PaymentMethodsList)
				{
					method.Amount = _MyDeclarationPM.TotalTax;
					method.SetMethodTypeCode("2");
					method.PayerActivityTypeCode = defaultValue;


				}

				if (this.paymentMethodModelMax != null)
				{
					this.paymentMethodModelMax.Amount = _MyDeclarationPM.TotalTax;
					this.paymentMethodModelMax.SetMethodTypeCode("2");
					this.paymentMethodModelMax.PayerActivityTypeCode = defaultValue;
					this.PaymentMethodsList.Add(paymentMethodModelMax);


				}
			}
		}
		private void AutoFillPaymentScreen()
		{
			if (this.isAutoFill)
				this.JustAutoFillPaymentScreen();
		}

		public void NewMethodMethod(bool isBtl = false)
		{
			this.AddPaymentMethodClicked(isBtl, true);
		}
		string PaymentMethodMessage = "";
		bool IsPaymentMethodMessageVisible = false;
		bool newLine = false;
		// paymentMethodModelMax: PaymentMethodModel
		public void AddPaymentMethodClicked(bool isBtl, bool isLoad)
		{

			this.newLine = true;
			var line = 0;
			var seq = 0;
			DeclarationPaymentMethodPM method = null;

			if (this.paymentPM.DeclarationPaymentMethods.Count() == 1 && !isBtl)
			{
				method = this.paymentPM.DeclarationPaymentMethods.Find(d => d.PayerActivityTypeCode == null || d.MethodTypeCode == null || d.Amount == null);
			}
			if (method != null)
			{
				//this.IsPaymentMethodMessageVisible = true;
				//this.PaymentMethodMessage = TextCodeTranslator.Translate("Customs.Declaration.O.PaymentMethodFields");
			}
			else
			{
				//this.IsPaymentMethodMessageVisible = false;
				//this.PaymentMethodMessage = "";
				if (this.paymentPM.DeclarationPaymentMethods.Count() > 0)
				{

					//var line = this.paymentPM.DeclarationPaymentMethods.reduce(function(prev, current) { return (prev.Line > current.Line) ? prev : current }).Line;
					//var seq = this.paymentPM.DeclarationPaymentMethods.reduce(function(prev, current) { return (prev.SequenceNumeric > current.SequenceNumeric) ? prev : current }).SequenceNumeric;
					line = this.paymentPM.DeclarationPaymentMethods.Max(x => x.Line);
					seq = this.paymentPM.DeclarationPaymentMethods.Max(x => x.SequenceNumeric);
				}

				line++;
				seq++;

				var item = new PaymentMethodModel(this);
				item.Tenant = _MyDeclarationPM.Tenant;
				item.DeclarationId = _MyDeclarationPM.Id;
				item.Line = line;
				item.SequenceNumeric = seq;
				item.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
				this.paymentPM.DeclarationPaymentMethods.Add(item);
				if (this.BetweenMinAndMax && isLoad && !(this.sumBtl != null && this.sumBtl > 0))
				{

					this.paymentMethodModelMax = item.LogicPaymentMethodModel();
				}
				else
				{
					var itemModel = item.LogicPaymentMethodModel();
					this.PaymentMethodsList.Add(itemModel);

				}
			}

			
		}
		public CustomFileCreditResponseData CustomFileCredit(CustomFileCreditRequestParams requestParamsCredit)
		{
			try
			{

				CustomFileCreditResponseData responseData = new CustomFileCreditResponseData();
				CustomsSettingQueryService customsSettingQuery = new CustomsSettingQueryService(this.customContext);
				CustomsSettingPM customsSetting = customsSettingQuery.GetSingleByTenant(this._tenant);

				if (customsSetting != null && customsSetting.IsConnectedToUniFreight)
				 {
					try
					{
						//ClientProgressBarIndicatorService.UpsertClientProgressBarIndicatorCurrentStage(requestParamsCredit.PBId, "שליחת בקשת העברה לגובה");
						var myCustomFileCreditService = new CustomFileCreditService(requestParamsCredit);
						CUSTOMCREDIT_UL creditResponseData = myCustomFileCreditService.CheckFileCredit();
						//ClientProgressBarIndicatorService.UpsertClientProgressBarIndicatorCurrentStage(requestParamsCredit.PBId, "ניתוח תשובה העברה לגובה");
						responseData.CreditStatus = creditResponseData.CustomFileCredit[0].CreditStatus;
						if (!string.IsNullOrEmpty(creditResponseData.CustomFileCredit[0].ErrorMessage))
						{
							responseData.UserMessage = creditResponseData.CustomFileCredit[0].ErrorMessage;
							responseData.HasException = true;
						}
						responseData.PaymentDateTime = null;
						responseData.BankCode = creditResponseData.CustomFileCredit[0].BankCode;
						responseData.PaymentDate = creditResponseData.CustomFileCredit[0].PaymentDate;
						if (!String.IsNullOrWhiteSpace(responseData.PaymentTime) && creditResponseData.CustomFileCredit[0].PaymentTime.Length >= 12)
						{
							responseData.PaymentTime = creditResponseData.CustomFileCredit[0].PaymentTime != null ? creditResponseData.CustomFileCredit[0].PaymentTime.Substring(8, 4) : null;
						}
						if (!String.IsNullOrWhiteSpace(responseData.PaymentDate))
						{
							responseData.PaymentDateTime = GetUnifreightFormatedDate(responseData.PaymentDate, responseData.PaymentTime, "").GetValueOrDefault();
						}
						responseData.BillingTaxAmount = creditResponseData.CustomFileCredit[0].BillingTaxAmount;
						responseData.IsTRansGove = false;
						responseData.Succeeded = true;
					}
					catch (Exception e)
					{
						responseData.CreditStatus = "0";
						responseData.Succeeded = false;
						responseData.HasException = true;
						responseData.UserMessage = e.ToString();
					}
				}
				else
				{
					responseData.CreditStatus = "5";
					responseData.IsTRansGove = false;
				}
				return responseData;
			}

			catch (Exception ex)
			{
				return null;
			}
		}
		public static DateTime? GetUnifreightFormatedDate(string txt, string time, string dtdField)
		{
			DateTime date;
			if (string.IsNullOrWhiteSpace(txt)) return null;

			//if (!string.IsNullOrWhiteSpace(time))
			//{
			//    txt = string.Concat(txt, time);
			//}
			if (DateTime.TryParseExact(txt, "yyyyMMddHHmm", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
			{
				return date;
			}
			if (DateTime.TryParseExact(txt, "dd'.'MM'.'yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
			{
				return date;
			}
			if (DateTime.TryParseExact(txt, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
			{
				return date;
			}
			if (DateTime.TryParseExact(txt, "yyyyMMddHHmmffff", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
			{
				return date;
			}
			//throw new Exception("AmitalConvertUtil:GetShortDate:value=" + txt + " Field=" + dtdField);
			return null;
		}
		private void SetFuturePaymentTime(DateTime? newValue)
		{
			if (newValue != null)
			{
				DateTime? date = paymentPM.FuturePaymentDateTime;
				if (date != null && paymentPM.AutomaticPayment == 0)
				{
					var datetime = new DateTime(date.Value.Year, date.Value.Month, date.Value.Day, date.Value.Hour, date.Value.Minute, date.Value.Second, 0);
					paymentPM.FuturePaymentDateTime = datetime;
					//SetFuturePaymentTime(datetime);
				}
				else if (date == null && paymentPM.AutomaticPayment == 0)
				{
					//SetFuturePaymentTime(newValue);
				}

			}
			else
			{

				//SetFuturePaymentTime(newValue);


			}
		}
		private void FillSignData()
		{

			// fill user  if the declaration is not payed
			
			ICommonDataContext MyContext = CommonDataContext.GetContext(_MyDeclarationPM.Tenant);

			UserRepository userRepository = new UserRepository(MyContext);
			UserQuery userQuery = new UserQuery(userRepository);
			UserPM user = userQuery.GetSinglePM(_MyDeclarationPM.SignedByUserId, _tenant);
			if (user != null)
			{
				if (user.PersonalId == _MyDeclarationPM.SignerPersonalId)
				{
					this.paymentPM.CreatedByUserId = _MyDeclarationPM.SignedByUserId;
				}
			}

			//fill SignatoryIdentification if the declaration is not payed
			if (!string.IsNullOrEmpty(_MyDeclarationPM.SignerPersonalId))
			{ //if payed -> its display only
				this.paymentPM.SignatoryIdentification = _MyDeclarationPM.SignerPersonalId;
			}

		}
		private void initDates()
		{
			paymentPM.PaymentDate = paymentPM.PaymentDate != null && paymentPM.PaymentDate > DateTime.Now ? paymentPM.PaymentDate : DateTime.Now;
		}
		#endregion

		#region validateAfterFill
		public bool validateBeforeSend(string user)
		{

			DefaultValueQueryService defaultValueQueryService = new DefaultValueQueryService(_tenant);
			string OridefCIM_AUTO_PAY = defaultValueQueryService.GetDefault("ISRAEL", "CIM_AUTO_PAY", "NON", _MyDeclarationPM.CustomerCode, _tenant);

			if (OridefCIM_AUTO_PAY == "Y" && !_MyDeclarationPM.AvailabilityDate.HasValue && !(new string[] { "4070001", "4070005", "7070001", "7070005" }.Contains(_MyDeclarationPM.ProcedureCurrentCode)))
			{
				return false;
			}

			CustomsSettingQueryService settingService = new CustomsSettingQueryService(_tenant);
			CustomsSettingPM setting = settingService.GetSettingByTenantN(_tenant);
			CheckFileCrediteReq checkFileCrediteReq = new CheckFileCrediteReq();
			checkFileCrediteReq.ClassName = "AutoPaymentService";
			checkFileCrediteReq.AppicationId = _MyDeclarationPM.Id; 
			checkFileCrediteReq.LoggingUserId = user;
			string jsonString = System.Text.Json.JsonSerializer.Serialize(checkFileCrediteReq);

			var isCheckFileCredit = CheckFileCredit(_MyDeclarationPM, user, jsonString);
			if (setting.IsConnectedToUniFreight)
			{
				if (!isCheckFileCredit)
				{
					SendEventAPAYF(_MyDeclarationPM, user);
					
					return false;
				}
			}
			else
			{
				return false;
			}


			return true;
		}
		private void SendEventAPAYF(DeclarationPM declarationPM, string user)
		{
			var MyUnifreightEventParam = new UnifreightEventParam()
			{
				Code = "APAYF",
				Mode = UnifreightEventMode.@new,
				EventDateTime = DateTime.Now,
				Entname = "CFIFILEM",
				PrimaryNum = declarationPM.CustomFileNo,
				EventRemarks = "לא אושר בבקרת אשראי",
			};
			LogMessagingUtil.Instance.AppendLine("MyUnifreightEventParam = " + MyUnifreightEventParam ?? "NULL");
			var myOpenUnifreighTask = new UnifreightEventTaskService();
			myOpenUnifreighTask.UpsertEventLE2U(
				declarationPM.Tenant,
			   user,
				MyUnifreightEventParam);
		}
		private bool CheckFileCredit(DeclarationPM declarationPM, string user,string requestParamsJson)
		{
			CustomFileCreditRequestParams requestParamsCredit = new CustomFileCreditRequestParams()
			{
				Tenant = declarationPM.Tenant,
				AppicationId = declarationPM.Id,
				LoggingEnabled = true,
				LoggingEntityId = declarationPM.Id,
				InterfaceTypeCode = "2755",
				LoggingObjectTableId = ObjectTableRepository.GetObjectTableByName("Customs.Declaration"),
				LoggingEntityReference = declarationPM.DeclarationNumber,
				LoggingUserId = user,
				RequestName = "Send to check credit request",
				ResponseName = "Get check credit Response",
				Mode = "Check",
				RequestVIA = SendRequestVIA.WebServiceBatch,
			};
			var myCustomFileCreditService = new CustomFileCreditService(requestParamsCredit);
			CUSTOMCREDIT_UL creditResponseData = myCustomFileCreditService.CheckFileCredit(requestParamsJson);
			if (!string.IsNullOrEmpty(creditResponseData.CustomFileCredit[0].ErrorMessage))
			{
				return false;
			}

			return true;
		}

		List<string> ValidationErrorsList = new List<string>();


		public string validateBeforeSend12312()
		{

			//this.customSendOptions = event;
			// this.Option = event.Option;

			//#region Future Payment date validation
			//if (this.FuturePaymentTime == null && paymentPM.FuturePaymentDateTime != null)
			//{
			//	ValidationErrorsList = new List<string>();
			//	ValidationErrorsList.Add("הזן זמן עתידי"); // Please enter a future time
			//	return;
			//}
			//#endregion

			//#region Validate dates
			//var isFuturePaymentDateValid = this.IsFuturePaymentDateValid(null); // WI 32593
			//var isPaymentDateValid = this.IsPaymentDateValid();
			//if (isFuturePaymentDateValid && isPaymentDateValid)
			//{

			//	this.ValidationErrorsList = new List<string>();
			//}
			//else
			//{

			//	this.ValidationErrorsList = new List<string>();
			//	if (!isFuturePaymentDateValid)
			//		this.ValidationErrorsList.Add(TranslateTextsClass.Translate("Customs.Declaration.O.futuredatecantbepast", _MyDeclarationPM.Tenant, true));
			//	if (!isPaymentDateValid)
			//		this.ValidationErrorsList.Add("לא ניתן להזין תאריך בעבר");
			//}
			//#endregion

			//if (this.ValidationErrorsList.Count() > 0) return ValidationErrorsList[0];
			//DefaultValueQueryService defaultValueQueryService = new DefaultValueQueryService(_tenant);

			var isBlockTime = false;
			//var objCGG_PAY_BLK_RNG = defaultValueQueryService.GetDefault("ISRAEL", "CGG_PAY_BLK_RNG", "NON", "NON", _tenant);//טווח שעות לחסימת תשלום
			//string timeCompany = objCGG_PAY_BLK_RNG;

			//var objCIM_PAY_BLK_RNG = defaultValueQueryService.GetDefault("ISRAEL", "CIM_PAY_BLK_RNG", "NON", _MyDeclarationPM.CustomerCode, _tenant);//טווח שעות לחסימת תשלום
			//string timeCustomer = objCIM_PAY_BLK_RNG;
			//if (string.IsNullOrEmpty(timeCompany) && string.IsNullOrEmpty(timeCustomer))
			//{
			//	isBlockTime = false;
			//}
			//else
			//{

			//	if (paymentPM.AutomaticPayment == 0)
			//	{
			//		if (paymentPM.FuturePaymentDateTime != null)
			//		{
			//			if (!string.IsNullOrEmpty(timeCompany))
			//			{
			//				if (this.CheckIdDateBetween2Times(timeCompany, Convert.ToDateTime(paymentPM.FuturePaymentDateTime)))
			//				{
			//					this.ValidationErrorsList.Add("לא ניתן להגיש תשלום בשעות שהוזנו , לפי הגדרה ברמת חברה");
			//					isBlockTime = true;
			//				}

			//			}
			//			if (!string.IsNullOrEmpty(timeCustomer))
			//			{

			//				if (this.CheckIdDateBetween2Times(timeCustomer, Convert.ToDateTime(paymentPM.FuturePaymentDateTime)))
			//				{
			//					this.ValidationErrorsList.Add("לא ניתן להגיש תשלום בשעות שהוזנו , לפי הגדרה ברמת לקוח");
			//					isBlockTime = true;
			//				}
			//			}
			//		}

			//		else
			//		{
			//			if (!string.IsNullOrEmpty(timeCompany))
			//			{

			//				if (this.CheckIdDateBetween2Times(timeCompany, Convert.ToDateTime(paymentPM.PaymentDate)))
			//				{
			//					this.ValidationErrorsList.Add("לא ניתן להגיש תשלום בשעות שהוזנו , לפי הגדרה ברמת חברה");
			//					isBlockTime = true;
			//				}
			//			}
			//			if (!string.IsNullOrEmpty(timeCustomer))
			//			{

			//				if (this.CheckIdDateBetween2Times(timeCustomer, Convert.ToDateTime(paymentPM.PaymentDate)))
			//				{
			//					this.ValidationErrorsList.Add("לא ניתן להגיש תשלום בשעות שהוזנו , לפי הגדרה ברמת לקוח");
			//					isBlockTime = true;
			//				}
			//			}
			//		}

			//	}
			//}

			//var objCIM_PAY_BLK_RNG = defaultValueQueryService.GetDefault("ISRAEL", "CIM_PAY_BLK_RNG", "NON", _MyDeclarationPM.CustomerCode, _tenant);//טווח שעות לחסימת תשלום
			//string timeCustomer = objCIM_PAY_BLK_RNG;
			//if (string.IsNullOrEmpty(timeCustomer))
			//{

			//}
			//else
			//{

			//}





			if (!isBlockTime)
			{
				// Validate payment date with future date
				if (paymentPM.FuturePaymentDateTime != null && paymentPM.PaymentDate != null)
				{

					//paymentPM.PaymentDate = new Date(Date.parse(this.PaymentDate + "")); // sometimes this variable contains string value of date, so convert it to date
					//paymentPM.FuturePaymentDateTime = new Date(Date.parse(this.FuturePaymentDateTime + "")); // sometimes this variable contains string value of date, so convert it to date

					var paymentDate = new DateTime(paymentPM.PaymentDate.Value.Year, paymentPM.PaymentDate.Value.Month, paymentPM.PaymentDate.Value.Day, 0, 0, 0);
					var futurePaymentDateTime = new DateTime(paymentPM.FuturePaymentDateTime.Value.Year, paymentPM.FuturePaymentDateTime.Value.Month, paymentPM.FuturePaymentDateTime.Value.Day, 0, 0, 0);

					if (futurePaymentDateTime > paymentDate)
					{
						//valid
						//var confirmWindow = new ConfirmWindow();
						//confirmWindow.Title = TextCodeTranslator.Translate("Customs.General.O.Warning");
						//confirmWindow.ShowCancelButton = false;
						//confirmWindow.ShowNoButton = true;
						//confirmWindow.WindowClosed.subscribe((event: any) => {
						//	if (confirmWindow.No)
						//	{
						//		console.log("[!] Send payment canceled");
						//		return;
						//	}
						//	else if (confirmWindow.Yes)
						//	{
						//		this.SendMethodStep1();
						//	}

						//});
						//confirmWindow.Show(TextCodeTranslator.Translate("Customs.Declaration.O.paymentDateSmallerThanFuture"));//תאריך הגשה קטן מתאריך הגשה עתידית האם להמשיך

					}
					else
					{
						this.SendMethodStep1();
					}
				}
				else
				{
					this.SendMethodStep1();
				}

			}
			return ErrorMessage;
		}

		private DateTime CheckIfBlockTime()
		{
			var time = paymentPM.FuturePaymentDateTime != null ? paymentPM.FuturePaymentDateTime.Value.TimeOfDay : DateTime.Now.TimeOfDay;

			DefaultValueQueryService defaultValueQueryService = new DefaultValueQueryService(_tenant);
			TimeSpan toTimeCurrent = new TimeSpan();
			TimeSpan toTime2Current = new TimeSpan();
			TimeSpan toTime = new TimeSpan();
			string timesCustomer = defaultValueQueryService.GetDefault("ISRAEL", "CIM_PAY_BLK_RNG", "NON", _MyDeclarationPM.CustomerCode, _tenant);

			if (!string.IsNullOrEmpty(timesCustomer))
			{
				List<string> times = GetTimesFromDefault(timesCustomer);

				TimeSpan fromTime = DateTime.ParseExact(times[0], "HH:mm",
										CultureInfo.InvariantCulture).TimeOfDay;

				toTime = DateTime.ParseExact(times[1], "HH:mm",
								  CultureInfo.InvariantCulture).TimeOfDay;

				if (time > fromTime && time < toTime)
				{
					toTime2Current = toTime;
				}
			}
			else
			{
				string timesCompany = defaultValueQueryService.GetDefault("ISRAEL", "CGG_PAY_BLK_RNG", "NON", "NON", _tenant);

				if (timesCompany != null && timesCompany != "")
				{
					List<string> times = GetTimesFromDefault(timesCompany);

					TimeSpan fromTime = DateTime.ParseExact(times[0], "HH:mm",
											CultureInfo.InvariantCulture).TimeOfDay;

					toTime = DateTime.ParseExact(times[1], "HH:mm",
									  CultureInfo.InvariantCulture).TimeOfDay;

					if (time > fromTime && time < toTime)
					{
						toTimeCurrent = toTime;
					}

				}
			}



			if (toTime2Current > toTimeCurrent)
			{
				return new DateTime(toTime2Current.Ticks).AddMinutes(5);

			}
			else if (toTimeCurrent > toTime2Current)
			{
				return new DateTime(toTimeCurrent.Ticks).AddMinutes(5);
			}

			return paymentPM.FuturePaymentDateTime != null?paymentPM.FuturePaymentDateTime.Value:DateTime.MinValue;

		}

		private List<string> GetTimesFromDefault(string times)
		{
			var arr = times.Split('-');
			return new List<string>()
			{
				 arr[0].TrimEnd() ,  arr[1].TrimStart()
			};
		}
		private bool IsFuturePaymentDateValid(object event1)
		{
			//	if (paymentPM.AutomaticPayment!=null && event1 != null) {
			//	var myMessageWindow = new MessageWindow

			//	myMessageWindow.Show("לא ניתן לבצע תשלום בזמינות עם תאריך תשלום עתידי");//TextCodeTranslator.Translate("")
			//	this.FuturePaymentDateTime = null;
			//	this.paymentPM.FuturePaymentDateTime = null;
			//	this.FuturePaymentTime = null;
			//	return false;
			//}
			if (paymentPM.FuturePaymentDateTime != null && paymentPM.AutomaticPayment == 0)
			{

				var newDate = new DateTime();
				var currentDate = new DateTime(newDate.Year, newDate.Month, newDate.Day, 0, 0, 0);

				if (paymentPM.FuturePaymentDateTime < currentDate)
				{
					ErrorMessage = TranslateTextsClass.Translate("Customs.Declaration.O.futuredatecantbepast", _MyDeclarationPM.Tenant, true);//השדה תאריך עתידי לא יכול להיות תאריך עבר
					return false;
				}
				else
				{
					return true;
				}

			}
			else // no date entered

			{
				return true;
			}
		}
		private bool IsPaymentDateValid()
		{

			if (paymentPM.PaymentDate != null)
			{

				//var newDate = new Date();
				var newDate = new DateTime();
				var currentDate = new DateTime(newDate.Year, newDate.Month, newDate.Day, 0, 0, 0);

				if (paymentPM.PaymentDate < currentDate)
				{
					ErrorMessage = "לא ניתן להזין תאריך בעבר";
					return false;
				}
				else
				{
					return true;
				}

			}
			else // no date entered
			{
				return true;
			}
		}
		public bool CheckIdDateBetween2Times(string times, DateTime date1)
		{

			if (times == null) return false;
			var startTime = times.Split('-')[0];
			var endTime = times.Split('-')[1];

			if (startTime == null || endTime == null) return false;
			var date = new DateTime(date1.Year, date1.Month, date1.Day, date1.Hour, date1.Minute, 0);

			DateTime dateNow = DateTime.Now;
			var startDate = new DateTime(dateNow.Year, dateNow.Month, dateNow.Day, Convert.ToInt32(startTime.Split(':')[0]), Convert.ToInt32(startTime.Split(':')[1]), 0);

			var endDate = new DateTime(dateNow.Year, dateNow.Month, dateNow.Day, Convert.ToInt32(endTime.Split(':')[0]), Convert.ToInt32(endTime.Split(':')[1]), 0);

			return startDate < date && endDate > date;

		}

		private void SendMethodStep1()
		{
			//SessionLocator.SelectedSession.StartBusyIndicator(TextCodeTranslator.Translate("Customs.General.O.Loading"));

			if (this.CreateEntity)
			{
				//this.declarationPaymentPMService.insert(this.paymentPM).subscribe((response: ServiceResponse) => {
				//	var result = response.Result;
				//	console.log("[Response] declarationPaymentPMService.insert ", result);
				//	this.paymentPM = result;
				//	this.FillGridsData();

				//	if (!AppTool.IsNullOrEmpty(result))
				//	{
				//		this.CheckRequiredFields();
				//	}
				//	else
				//	{
				//		SessionLocator.SelectedSession.StopBusyIndicator();
				//	}
				//});
				paymentPM.ChangeSetOp = ChangeSetOperation.Insert;
				this.CreateEntity = false;
			}
			else
			{
				//this.declarationPaymentPMService.update(this.paymentPM).subscribe((response: ServiceResponse) => {
				//	var result = response.Result;
				//	console.log("[Response] declarationPaymentPMService.insert ", result);
				//	this.paymentPM = result;
				//	this.FillGridsData();

				//	if (!AppTool.IsNullOrEmpty(result))
				//	{
				//		this.CheckRequiredFields();
				//	}
				//	else
				//	{
				//		SessionLocator.SelectedSession.StopBusyIndicator();
				//	}
				//});
				paymentPM.ChangeSetOp = ChangeSetOperation.Insert;
			}
			DeclarationPaymentUpdateService declarationPaymentUpdateService = new DeclarationPaymentUpdateService(customContext);
			declarationPaymentUpdateService.Update(paymentPM, true);
			SendMethod();
			//this.FillGridsData();
			//CheckRequiredFields();
		}
		private void CheckRequiredFields()
		{

			CustomsRequiredFieldErrors errors = CustomsRequiredFieldsValidator.GetRequiredFieldErrorsForDeclarationPayment(_MyDeclarationPM.Id, _tenant);
			//var customsRequiredFieldErrors = errors;
			//
			//if (customsRequiredFieldErrors != null)
			//{
			//
			//
			//	if (customsRequiredFieldErrors.RequiredFields.Count() == 0)
			//	{

			//If Last Declaration was NOT Signed
			//if (!this.DeclarationPM.IsSignedVersion) {  ///If IsCourierDeclaration= false, Check if Last Declaration Signed (IsSignedVersion.Declaration = True) , if NOT   - WI 18211
			//if (!_MyDeclarationPM.IsCourierDeclaration && !_MyDeclarationPM.IsSignedVersion)
			//{
			//
			//	this.CheckBeforeSendPaymentOrder();
			//}
			//else
			//{
			//	this.SendMethod();
			//}

			//}
			//else
			//{
			//
			//	this.FillValidationErrors(TranslateTextsClass.Translate("Customs.General.O.RequiredFields", _MyDeclarationPM.Tenant, true), customsRequiredFieldErrors);
			//}
			//}

		}

		private void FillValidationErrors(string title, CustomsRequiredFieldErrors errors)
		{

			var RequiredFieldsList = new List<string>();

			// 1- build validation errors
			foreach (var error in errors.RequiredFields)
			{
				if (!string.IsNullOrEmpty(error.CustomMessageError))
				{
					RequiredFieldsList.Add(TranslateTextsClass.Translate(error.CustomMessageError, _MyDeclarationPM.Tenant, true));
				}
				else
				{
					var ObjectTable = ObjectTableRepository.GetObjectTableByName(error.TableName);
					ObjectFieldRepository objectFieldRepository = new ObjectFieldRepository();
					var field = objectFieldRepository.GetObjectFieldByName(error.FieldName, ObjectTable, _tenant);

					if (error.TableName.Contains("Customs.SupplierInvoiceItem") && !string.IsNullOrEmpty(error.EntityReference2))
					{
						error.EntityReference = error.EntityReference + " (חשבון " + error.EntityReference2 + ")";
					}
					var requiredField = TranslateTextsClass.GetRequiredFieldForTableMessageTranslation("Customs.General.O.FieldForTableIsRequired", field.FullNameTextCodeCode, error.TableName, error.EntityReference, _tenant);
					RequiredFieldsList.Add(requiredField);
				}

			}
			if (RequiredFieldsList.Count > 0)
				ErrorMessage = RequiredFieldsList[0];

			//// 2- open window
			//var windowArgs: any = { };
			//windowArgs.Errors = RequiredFieldsList;
			//windowArgs.ComponentHeight = '323px';
			//var windowTitle = title;

			//var logWindow = new LogitudeWindow();
			//logWindow.Width = 600;
			//logWindow.Height = 400;
			//logWindow.Title = windowTitle;
			//logWindow.ShowCloseButton = false;
			//logWindow.WindowArgs = windowArgs;
			////logWindow.WindowClosed.subscribe(($event: any) => this.OnAddEditWindowClosed($event));

			//logWindow.Show('./CustomsModules/CustomsControls/Components/CustomsErrorsComponent');
		}
		private void CheckSingValidation()
		{

			if (!_MyDeclarationPM.IsCourierDeclaration && !_MyDeclarationPM.IsSignedVersion)
			{
				string user = AuthenticationUtil.ResolveUserId(_tenant);
				var repository = new UserRepository(_tenant);
				var myUserCard = repository.GetSingleUser(user, _tenant, false);
				if (myUserCard == null)
				{
					myUserCard = repository.GetSingleUser(user, 0, false);
				}
				bool IsCustomerCare = !myUserCard.IsDistributor;
				if (!IsCustomerCare)
				{
					ErrorMessage = TranslateTextsClass.Translate("Customs.Declaration.O.IsSignedVersionError", _tenant, true);//גרסת הצהרה אחרונה אינה חתומה - לא ניתן להגיש תשלום
					throw new Exception(ErrorMessage);

				}
			}
		}
		private void SendMethod()
		{

			//var totalAmount = AppTool.Round(this.TotalAmount, 2);
			//var totalTax = AppTool.Round(this.TotalTax, 2);

			//if (AppTool.IsNullOrEmpty(totalTax))
			//{
			//	totalTax = 0;
			//}
			//if (totalAmount != totalTax)
			//{
			//	var msg = new MessageWindow();
			//	msg.Show(TextCodeTranslator.Translate("Customs.Declaration.O.Totalmustbeequaltototaltax"));//סה''כ חייב להיות שווה למס הכולל
			//																							   // error not sending
			//}
			//else
			//{

			//if (AppTool.IsNullOrEmpty(this.DeclarationPM.CustomFileNo) || !AmitalGatewayUtil.Instance.AmitalBrowserInUse) {
			//if (!AmitalGatewayUtil.Instance.IsDeclarationInUse(this.DeclarationPM.CustomFileNo, this.DeclarationPM.IsConvertedDeclaration, this.DeclarationPM.IsConnectedToUnifreight))
			//{
			//	this.ActualSend();
			//}
			//else
			//{
			//	SessionLocator.SelectedSession.StartBusyIndicator(TextCodeTranslator.Translate("Customs.General.O.UnifreightInstSentMehes"));

			//	var myStoreViewUnifreightInstructionController = new UnifreightController(
			//		this.DeclarationPM,
			//		"Logitude.Customs.ViewModels.DeclarationPayment.DeclarationPaymentTabViewModel.MyStoreViewUnifreightInstructionController");
			//	myStoreViewUnifreightInstructionController.GetPromise()
			//		//myStoreViewUnifreightInstructionController.UnifreightCallbackCompleted += (sender, e) => {
			//		.then((e) =>
			//		{
			//			if (e.UnifreightResponseStatus)
			//			{
			this.ActualSend();
			//			}
			//			else
			//			{
			//				SessionLocator.SelectedSession.StopBusyIndicator();

			//			}
			//		});
			//	SessionLocator.SelectedSession.StartBusyIndicator("");
			//	myStoreViewUnifreightInstructionController.SendRequestInstructionToUnifreightAsync("PAYHAND_STORE");

			//}


			//}

			return;
		}
		private void ActualSend()
		{

			//if (this.customSendOptions == null)
			//{
			//	console.log("[!] no send options!!");
			//	return;
			//}

			var objectTableDecId = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");

			var CustomFileCreditParams = new CustomFileCreditRequestParams();
			CustomFileCreditParams.Tenant = _tenant;
			CustomFileCreditParams.AppicationId = this.paymentPM.DeclarationId;
			//params.TestCase = SelectedTest;
			CustomFileCreditParams.LoggingEnabled = true;
			CustomFileCreditParams.LoggingEntityId = _MyDeclarationPM.Id;
			CustomFileCreditParams.LoggingObjectTableId = objectTableDecId;
			CustomFileCreditParams.LoggingUserId = AuthenticationUtil.ResolveUserId(_tenant);
			CustomFileCreditParams.RequestName = "send declaration payment request";
			CustomFileCreditParams.ResponseName = "send declaration payment response";
			CustomFileCreditParams.Mode = "Check";
			CustomFileCreditParams.RequestVIA = SendRequestVIA.WebServiceInteractive;
			CustomFileCreditParams.ForcePersonalSign = false;
			CustomFileCreditParams.TestCase = null;
			bool splitRequest = true;
			if (splitRequest)
			{
				//if (this.DeclarationPM.IsCourierDeclaration)
				//{
				//	this.OnlySendPayment(params);
				//}
				//else
				//{
				this.CheckCustomFileCreditThenSendPayment(CustomFileCreditParams);
				//}
				return;
			}


		}
		private void CheckCustomFileCreditThenSendPayment(CustomFileCreditRequestParams params1)
		{
			//var myCustomMessageProgressHelper = new CustomMessageProgressHelper(this.CurrentSession);
			//myCustomMessageProgressHelper.BasicResponse = true;
			//myCustomMessageProgressHelper.StartProgress (params.PBId, 5, true);


			//myCustomMessageProgressHelper.MessageArrived = true;
			//SessionLocator.SelectedSession.StopBusyIndicator();
			CustomFileCreditResponseData customFileCreditResponseData = PostCheckCustomFileCreditOnly(params1);

			if (customFileCreditResponseData != null)
			{
				if (customFileCreditResponseData.IsTRansGove)
				{
					//		var confirmWindow = new ConfirmWindow();
					//		confirmWindow.Show(customFileCreditResponseData.UserMessage);
					//		SessionLocator.SelectedSession.StopBusyIndicator();
					//		confirmWindow.WindowClosed.subscribe((event: any) => {
					//if (confirmWindow.Yes)
					//{
					//this.ActualSendToTransfer();
					//this.InstructionActualSendToTransfer();

					//}

					return;
				}
				if (customFileCreditResponseData.IsReTRansGove)
				{
					//                      var confirmWindow = new ConfirmWindow();
					////confirmWindow.Width = 400;
					//confirmWindow.Show(customFileCreditResponseData.UserMessage);
					//                      SessionLocator.SelectedSession.StopBusyIndicator();
					//                      confirmWindow.WindowClosed.subscribe((event: any) => {
					//	if (confirmWindow.Yes)
					//	{
					//this.ActualSendToReTransfer();
					//}

					return;
				}
				if (customFileCreditResponseData.HasException)
				{
					//                      var messageWindow = new MessageWindow();
					//messageWindow.Title = TextCodeTranslator.Translate("Customs.General.O.Warning");
					//                      messageWindow.Width = 250;
					//                      messageWindow.Height = 150;
					//                      messageWindow.OkButtonText = TextCodeTranslator.Translate("Customs.General.B.OK");
					//                      messageWindow.Show(customFileCreditResponseData.UserMessage);
					ErrorMessage = customFileCreditResponseData.UserMessage;


					return;
					//////////////////////////////////////////////////////////////////////////
				}


				//                  if (AmitalGatewayUtil.Instance.IsDeclarationInUse(this.DeclarationPM.CustomFileNo, this.DeclarationPM.IsConvertedDeclaration, this.DeclarationPM.IsConnectedToUnifreight)) {
				//SessionLocator.SelectedSession.StartBusyIndicator(TextCodeTranslator.Translate("Customs.General.O.UnifreightInstSentMehes"));

				//var myStoreViewUnifreightInstructionController = new UnifreightController(
				//	this.DeclarationPM,
				//	"Logitude.Customs.ViewModels.DeclarationPayment.DeclarationPaymentTabViewModel.MyStoreViewUnifreightInstructionController");
				//myStoreViewUnifreightInstructionController.GetPromise()
				//	//myStoreViewUnifreightInstructionController.UnifreightCallbackCompleted += (sender, e) => {
				//	.then((e) =>
				//	{
				//		if (e.UnifreightResponseStatus)
				//		{
				//			this.Send2755(params1);
				//		}
				//		else
				//		{
				//			SessionLocator.SelectedSession.StopBusyIndicator();

				//		}

				//		SessionLocator.SelectedSession.StartBusyIndicator("");
				//		myStoreViewUnifreightInstructionController.SendRequestInstructionToUnifreightAsync("PAYHAND_SEND");


				//	} else
				//{
				/////this.Send2755(params1);
				//}

			}


		}

		private CustomFileCreditResponseData PostCheckCustomFileCreditOnly(CustomFileCreditRequestParams requestParamsCredit)
		{
			try
			{
				CustomFileCreditResponseData responseData = new CustomFileCreditResponseData();


				CustomsSettingQueryService settingService = new CustomsSettingQueryService(requestParamsCredit.Tenant);
				CustomsSettingPM setting = settingService.GetSettingByTenantN(requestParamsCredit.Tenant);

				if (setting.IsConnectedToUniFreight)
				{
					try
					{
						//ClientProgressBarIndicatorService.UpsertClientProgressBarIndicatorCurrentStage(requestParamsCredit.PBId, "שליחת בקשה לבדיקת אשראי");
						var myCustomFileCreditService = new CustomFileCreditService(requestParamsCredit);
						CUSTOMCREDIT_UL creditResponseData = myCustomFileCreditService.CheckFileCredit();
						//ClientProgressBarIndicatorService.UpsertClientProgressBarIndicatorCurrentStage(requestParamsCredit.PBId, "ניתוח תשובה בקרת אשראי");
						responseData.CreditStatus = creditResponseData.CustomFileCredit[0].CreditStatus;
						//if (creditResponseData.CustomFileCredit[0].ErrorMessage != null || creditResponseData.CustomFileCredit[0].ErrorMessage != "")
						if (!string.IsNullOrEmpty(creditResponseData.CustomFileCredit[0].ErrorMessage))
						{
							responseData.UserMessage = creditResponseData.CustomFileCredit[0].ErrorMessage;
							responseData.HasException = true;
						}
						responseData.Succeeded = true;
						responseData.BankCode = creditResponseData.CustomFileCredit[0].BankCode;
						responseData.PaymentDate = creditResponseData.CustomFileCredit[0].PaymentDate;
						responseData.BillingTaxAmount = creditResponseData.CustomFileCredit[0].BillingTaxAmount;
						responseData.IsTRansGove = false;
					}
					catch (Exception e)
					{
						responseData.CreditStatus = "0";
						responseData.Succeeded = true;
						responseData.HasException = true;
						responseData.UserMessage = e.ToString();
					}
				}
				else
				{
					responseData.CreditStatus = "5";
					responseData.IsTRansGove = false;
				}

				if (responseData.CreditStatus == "5" | responseData.CreditStatus == "3")
				{
					// to do booom !!!! in angular 
				}
				else if (responseData.CreditStatus == "1")
				{
					responseData.IsTRansGove = true;
				}
				else if (responseData.CreditStatus == "6")
				{
					responseData.IsReTRansGove = true;
				}

				return responseData;
			}
			catch (Exception ex)
			{
				return null;
			}
		}
		#endregion

        #region SendPayment
		public void SendPaymentIsCheckFileCredit(bool isCheckFileCredit, DeclarationPM declarationPM, DeclarationPaymentPM declarationPaymentPM, string user, bool isFromAPI)
		{
			if (isCheckFileCredit)
			{
				var MyUnifreightEventParam = new UnifreightEventParam()
				{
					Code = "APAYA",
					Mode = UnifreightEventMode.@new,
					EventDateTime = DateTime.Now,
					Entname = "CFIFILEM",
					PrimaryNum = declarationPM.CustomFileNo,
					EventRemarks = "תשלום הצהרה אוטומטי"
				};
				LogMessagingUtil.Instance.AppendLine("MyUnifreightEventParam = " + MyUnifreightEventParam ?? "NULL");
				var myOpenUnifreighTask = new UnifreightEventTaskService();
				myOpenUnifreighTask.UpsertEventLE2U(
					declarationPM.Tenant,
				  user,
					MyUnifreightEventParam);


				var requestParams = new GenericRequestParams()
				{
					LoggingEnabled = true,
					IsFakeResponse = true,
					InterfaceTypeCode = "2755",
					Tenant = declarationPM.Tenant,
					RequestName = "Auto Payment Request",
					ResponseName = "Auto Payment Response",
					LoggingEntityId = declarationPM.Id,
					RequestVIA = SendRequestVIA.WebServiceBatch,
					SuppressSplitWR = true,
					AppicationId = declarationPM.Id,
					// LoggingEntityReference = "AutoPayment",
					//UnifreightListOnServerOnly = SetBankIdInUnifreightListOnServerOnly(autoPaymentService?.PaymentMethodsList[0]?.SelectedBank?.Id)
				};

				if (declarationPaymentPM.FuturePaymentDateTime != null)
				{
					DateTime requestDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, declarationPaymentPM.FuturePaymentDateTime.Value.Hour, declarationPaymentPM.FuturePaymentDateTime.Value.Minute, declarationPaymentPM.FuturePaymentDateTime.Value.Second);
					declarationPaymentPM.PaymentDate = requestDate;

					requestParams.RequestVIAChangeDue = string.Concat("נרשמה בקשה מתוזמנת לתאריך ", requestDate.ToShortDateString(), " שעה ", requestDate.ToShortTimeString());// "הבקשה תשלח בעתיד";
					requestParams.FutureSendDateTime = requestDate;

					SBQMessageService.CreateSheetSBQMessage<GenericRequestParams>(requestParams, false, requestDate);

				}
				else
				{

					SBQMessageService.CreateSheetSBQMessage<GenericRequestParams>(requestParams, false);

				}
			}
			else
			{
				if (isFromAPI)
				{
					SendEventAPAYF(declarationPM, user);
				}

			}
		}
		#endregion
	}
}

#region PaymentMethodModel
public class PaymentMethodModel : DeclarationPaymentMethodPM
{
	public bool BankIsNull { get; set; } = false;
	public List<CustomBankList> BanksList { get; set; }
	public List<CustomBankList> agentBanks { get; set; }
	public bool IsAmountButtonVisibile { get; set; } = false;
	public CustomBankList SelectedBank { get; set; } = null;
	public bool is_ABOVE_MSVLK_agent { get; set; } = false;
	public bool _UsingDsvPayKupa { get; set; } = false;

	AutoPaymentService parent;
	public PaymentMethodModel(AutoPaymentService parent)
	{
		this.parent = parent;
	}
	public PaymentMethodModel LogicPaymentMethodModel()
	{

		//ParamPaymentMethodModel paramPaymentMethodModel = new ParamPaymentMethodModel();
		if (MethodTypeCode == "1")
		{
			this.BanksList = new List<CustomBankList>();
			if (!string.IsNullOrEmpty(InternalBankId))
			{

				CustomBankListQueryService customBankQuery = new CustomBankListQueryService(this.parent.customContext);
				CustomBankList customBankList = customBankQuery.GetSingle(InternalBankId);

				if (customBankList != null)
				{
					if (SelectedBank != customBankList)
					{
						SelectedBank = customBankList;
						SetSelectedBank(customBankList);

					}
				}

			}
			else
			{
				this.LoadBanks();
			}
		}

		//if (parent.PaymentMethodsList.Length == 1)
		//{
		//	this.IsAmountButtonVisibile = true;
		//}

		return this;
	}
	private void SetSelectedBank(CustomBankList value)
	{

		if (SelectedBank != value)
		{
			SelectedBank = value;
			SetInternalBankId(value?.Id);
		}
	}

	private void SetInternalBankId(string value)
	{
		if (InternalBankId != value)
		{
			InternalBankId = value;
			CustomBankListQueryService CustomBankListQuery = new CustomBankListQueryService(this.parent.customContext);
			QueryOperations queryOperations = new QueryOperations();
			queryOperations.GetAll = true;
			List<CustomBankList> responseBankFromCache = CustomBankListQuery.GetList(queryOperations, this.parent._tenant);


			if (responseBankFromCache != null)
			{
				if (!BankIsNull)
				{
					bool usingCustomBank_ImporterMasav = false;

					CustomBankList customBank = responseBankFromCache.FindAll(d => d.Id == value).FirstOrDefault();

					if (customBank == null && BanksList != null)
					{
						customBank = BanksList.FindAll(d => d.Id == value).FirstOrDefault();
					}
					if (customBank != null)
					{
						if (customBank.PayerTypeCode == "0")
						{
							CustomBanksCardQueryService customBanksCardQuery = new CustomBanksCardQueryService(this.parent.customContext);
							CustomBanksCardPM bankCard = customBanksCardQuery.GetSingleCustomBanksCard(value, this.parent._MyDeclarationPM.CustomerId, this.parent._tenant);

							if (bankCard != null)
							{
								BankCode = customBank.BankCode;
								BranchCode = customBank.BranchCode;
								CustomsBranchId = customBank.CustomsBranchId;
								AccountNumber = customBank.AccountNumber;
								PayerActivityTypeCode = customBank.PayerTypeCode;
								PayerActivityTypeName = customBank.PayerTypeName;
								usingCustomBank_ImporterMasav = true;
								//console.log("DefaultPaymentMethod-->מסב הכנסה- יבואן");

							}
							else
							{
								InternalBankId = null;
								SelectedBank = null;
								PayerActivityTypeCode = null;
								SetSelectedBank(null);
							}



						}
						else
						{
							if (MethodTypeCode == "2"/*קופה*/
								||
								MethodTypeCode == "79"/*ניצול העברת זהב*/
							)
							{
								//console.log("אם קופה או ניצול העברת זהב - שדה בנק לאפס ");
								BankCode = null;
								InternalBankName = null;
								BranchCode = null;
								AccountNumber = null;
								InternalBankId = null;
							}
							else
							{

								BankCode = customBank.BankCode;
								BranchCode = customBank.BranchCode;
								CustomsBranchId = customBank.CustomsBranchId;
								AccountNumber = customBank.AccountNumber;
								PayerActivityTypeCode = customBank.PayerTypeCode;
								PayerActivityTypeName = customBank.PayerTypeName;
								if (!string.IsNullOrEmpty(BankCode))
								{
									///usingCustomBank_ImporterMasav = true;
									//console.log("DefaultPaymentMethod-fromCache??->מסב הכנסה- סוכן !");
								}
							}


						}


					}

					if (customBank == null)
					{
						BankCode = null;
						BranchCode = null;
						CustomsBranchId = null;
						AccountNumber = null;
						PayerActivityTypeCode = null;

					}

					if (this.parent.BetweenMinAndMax && PayerActivityTypeCode == "3" && this.parent.PaymentMethodsList.Count() == 0)
					{
						_UsingDsvPayKupa = true;
						//console.log("DefaultPaymentMethod-->קופה DSV");
						MethodTypeCode = "2";
						PayerActivityTypeCode = "3";
						Amount = this.parent._MyDeclarationPM.TotalTax;

						this.parent.PaymentMethodsList.Add(this.parent.paymentMethodModelMax);
					}
					// for dsv when more than one bank
					if (customBank == null && BanksList.Count() > 0)
					{
						foreach (var bank in BanksList)
						{
							if (this.parent.BetweenMinAndMax && bank.PayerTypeCode == "3" && this.parent.PaymentMethodsList.Count() == 0)
							{
								_UsingDsvPayKupa = true;
								//console.log("DefaultPaymentMethod-->קופה DSV");
								MethodTypeCode = "2";
								PayerActivityTypeCode = "3";
								Amount = this.parent._MyDeclarationPM.TotalTax;
								this.parent.PaymentMethodsList.Add(this.parent.paymentMethodModelMax);
							}
						}
					}
					else if (this.parent.CreateEntity && this.parent.PaymentMethodsList.Count() == 0)
					{
						if (this.parent.paymentPM != null)
						{
							foreach (var item in this.parent.paymentPM.DeclarationPaymentMethods)
							{
								var itemModel = new PaymentMethodModel(this.parent);
								itemModel.DeclarationId = item.DeclarationId;
								itemModel.Line = item.Line;
								itemModel.SequenceNumeric = item.SequenceNumeric;
								itemModel.PayerActivityTypeCode = item.PayerActivityTypeCode;
								itemModel.MethodTypeCode = item.MethodTypeCode;
								itemModel.Amount = item.Amount;
								itemModel.BankCode = item.BankCode;
								itemModel.BranchCode = item.BranchCode;
								itemModel.AccountNumber = item.AccountNumber;
								itemModel.Tenant = item.Tenant;
								itemModel.InternalBankId = item.InternalBankId;
								itemModel.CustomsBranchId = item.CustomsBranchId;
								itemModel.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
								this.parent.PaymentMethodsList.Add(itemModel.LogicPaymentMethodModel());
							}
						}
					}

					this.maximumAgentPaymentMethod(usingCustomBank_ImporterMasav);
				}
			}
		}
	}
	public void SetMethodTypeCode(string value)
	{
		if (MethodTypeCode != value)
		{
			MethodTypeCode = value;
			if (value == "1")
			{
				BanksList = new List<CustomBankList>();
				this.LoadBanks();
			}
			else
			{
				SetInternalBankId(null);
				PayerActivityTypeCode = null;
				BanksList = new List<CustomBankList>();
				SetSelectedBank(null);
			}
		}
	}
	private void SetCustomerActivityType(string value)
	{
		//if (methodPM.customerActivityType != value)
		//{
		//	this.customerActivityType = value;
		//}
		//if (!AppTool.IsNullOrEmpty(value))
		//{
		//	this.PayerActivityTypeName = value.LocalName;


		//}
		//else
		//{
		//	this.PayerActivityTypeName = null;
		//	this.PayerActivityTypeCode = null;
		//}
	}
	private void maximumAgentPaymentMethod(bool usingCustomBank_ImporterMasav)
	{
		///old:///Feature 170339: ניצול העברת זהב - מסך הגשת תשלום
		//new://Feature 175382: ניצול העברת זהב - מסך הגשת תשלום

		if (string.IsNullOrEmpty(this.parent.CompanyDefaultMaxPayMASAV_CGG_MAX_AGT_PAY))
		{
			//console.log("DefaultPaymentMethod-->קופהזה יהיה דיפולט כחול ואם לא הוגדר אז לא תהיה התייחסות לניצול העברת זהב וימשיך לעבוד כפי שעבר לפני השיפור ");
			return;
		}
		//console.log("DefaultPaymentMethod-->");
		int maxTaxAgentPayDefault = Convert.ToInt32(this.parent.CompanyDefaultMaxPayMASAV_CGG_MAX_AGT_PAY);
		int aboveAmountAgentCash = Convert.ToInt32(this.parent.CompanyDefaultaboveamountagentCash_CGG_ABOVE_AGT_C);
		if (maxTaxAgentPayDefault <= 0)
		{
			//console.log("DefaultPaymentMethod-->maxTaxAgentPayDefault <= 0");
			return;
		}

		//console.log("DefaultPaymentMethod-->CompanyDefaultMaxPayMASAV_CGG_MAX_AGT_PAY:" + maxTaxAgentPayDefault +
		//	";CustomerDefaultGoldPay_CIM_GOLD_PAY=" + this.CustomerDefaultGoldPay_CIM_GOLD_PAY +
		//	";CompanyDefaultaboveamountagentCash_CGG_ABOVE_AGT_C=" + this.CompanyDefaultaboveamountagentCash_CGG_ABOVE_AGT_C
		//);
		if (usingCustomBank_ImporterMasav)
		{//Task 179470: תשלום במסב לקוח מעל סכום חסימה
			if ("ABOVE_MSVLK" == this.parent.CustomerDefaultGoldPay_CIM_GOLD_PAY)
			{
				if (this.parent._MyDeclarationPM.TotalTax > maxTaxAgentPayDefault)
				{
					//console.log('a.');
					//console.log(`ודיפולט "תשלום בניצול העברת זהב לקוח/קופה" = "סכום מעל סכום החסימה מס"ב לקוח" וגם סכום המיסים גדול מסכום שהוזן בדיפולט "סכום מיסים מקסימלי לתשלום במס"ב סוכן") יבוצע תשלום באמצעות מס"ב לקוח`)


					this.updateDefaultPaymentMethod(
						"1",//מס"ב הכנסה
						"0" //יבואן / יצואן

					);
					is_ABOVE_MSVLK_agent = false;

				}
				else if (this.parent.BetweenMinAndMax)
				{
					this.updateDefaultPaymentMethod(
							"2",/*קופה*/
							"3" //סוכן מכס
						);
				}
				else{
					//console.log('b.');
					//console.log(`דיפולט "תשלום בניצול העברת זהב לקוח/קופה" = "סכום מעל סכום החסימה מס"ב לקוח" וגם סכום המיסים קטן שווה לסכום שהוזן בדיפולט "סכום מיסים מקסימלי לתשלום במס"ב סוכן") יבוצע תשלום באמצעות מס"ב סוכן`)

					this.updateDefaultPaymentMethod(
						"1",//מס"ב הכנסה
						"3" //סוכן מכס

					);

					is_ABOVE_MSVLK_agent = true;
					this.LoadBanks();

				}


			}
			else
			{
				is_ABOVE_MSVLK_agent = false;
				//console.log('C.');
				//console.log(`דיפולט "תשלום ניצול העברת זהב לקוח/קופה" = !(שונה)"מסכום מעל סכום החסימה מס"ב לקוח" יבוצע Cתשלום באמצעות מס"ב לקוח`);
				return;
			}
			return;
		}
		else
		{
			is_ABOVE_MSVLK_agent = false;
		}
		//3.1
		if (this.parent.CustomerDefaultGoldPay_CIM_GOLD_PAY == "ALL")
		{
			///console.log("אם ללקוח מוגדר הדיפולט החדש 'תשלום בניצול העברת זהב לקוח' כל סכום3.1");
			//console.log("DefaultPaymentMethod-->ניצול העברת זהב -יבואן");
			this.updateDefaultPaymentMethod(
				"79",/*ניצול העברת זהב*/
				"0" //יבואן / יצואן

			);
		}
		else if (this.parent.CustomerDefaultGoldPay_CIM_GOLD_PAY == "ALL_KUPA")
		{
			this.updateDefaultPaymentMethod(
							"2",/*קופה*/
							"0" //יבואן / יצואן

						);
		}
		else if (_UsingDsvPayKupa)
		{
			//already set
			//console.log("DefaultPaymentMethod-->קופה טווח 20,000 - 40,000 (ולא מוגדר ניצול העברת זהב - כל סכום)  == DSVKUPA");

			//console.log("אם קופה או ניצול העברת זהב - שדה בנק לאפס ");
			BankCode = null;
			InternalBankName = null;


		}
		//3.2
		else if (
			this.parent.CustomerDefaultGoldPay_CIM_GOLD_PAY == "ABOVE_MAX" &&
			this.parent._MyDeclarationPM.TotalTax > maxTaxAgentPayDefault)
		{//this.methodPM.Amount = 

			//console.log("DefaultPaymentMethod-->ניצול העברת זהב -יבואן");
			this.updateDefaultPaymentMethod(
				"79",/*ניצול העברת זהב*/
				"0" //יבואן / יצואן

			);

		}
		else if (//NEW ניצול העברת זהב/קופה - סכום מעל חסימה-קופה-קופה-יבואן
			this.parent.CustomerDefaultGoldPay_CIM_GOLD_PAY == "KUPA" &&
			this.parent._MyDeclarationPM.TotalTax > maxTaxAgentPayDefault)
		{

			//console.log("DefaultPaymentMethod-->ניצול העברת זהב -קופה");
			this.updateDefaultPaymentMethod(
				"2",/*קופה*/
				"0" //יבואן / יצואן

			);

		}
		//6
		else if (
			string.IsNullOrEmpty(this.parent.CustomerDefaultGoldPay_CIM_GOLD_PAY) &&
			this.parent._MyDeclarationPM.TotalTax > maxTaxAgentPayDefault
			&&
			(
				//(דיפולט "הגדרת סכום שמעל יבוצע תשלום בקופה סוכן" =NULL (לא הוגדר)
				string.IsNullOrEmpty(this.parent.CompanyDefaultaboveamountagentCash_CGG_ABOVE_AGT_C)
				||
				(

					!string.IsNullOrEmpty(this.parent.CompanyDefaultaboveamountagentCash_CGG_ABOVE_AGT_C)
					&&
					//וסכום המיסים קטן מסכום שהוגדר בדיפולט "הגדרת סכום שמעל יבוצע תשלום בקופה סוכן"
					this.parent._MyDeclarationPM.TotalTax < aboveAmountAgentCash
				)
				)
		)
		{

			//console.log("6");
			//console.log("אם הדיפולט -תשלום בניצול העברת זהב לקוח- לא הוגדר,  וסכום המיסים גדול מהסכום שהוזן  בדיפולט החדש -סכום מיסים מקסימלי לתשלום במס-ב סוכן-  וגם (דיפולט -הגדרת סכום שמעל יבוצע תשלום בקופה סוכן- =NULL (לא הוגדר) או (הוגדר סכום בדיפולט -הגדרת סכום שמעל יבוצע תשלום בקופה סוכן- וסכום המיסים קטן מסכום שהוגדר בדיפולט -הגדרת סכום שמעל יבוצע תשלום בקופה סוכן-  - )) יבוצע תשלום באמצעות ניצול העברת זהב סוכן ")

			// console.log("DefaultPaymentMethod-->ניצול העברת זהב -סוכן");
			this.updateDefaultPaymentMethod(
				"79",/*ניצול העברת זהב*/
				"3" //סוכן מכס

			);
		}
		//7 
		else if (
			string.IsNullOrEmpty(this.parent.CustomerDefaultGoldPay_CIM_GOLD_PAY) &&
			!string.IsNullOrEmpty(this.parent.CompanyDefaultaboveamountagentCash_CGG_ABOVE_AGT_C) &&
			aboveAmountAgentCash > 0 &&
			this.parent._MyDeclarationPM.TotalTax >= aboveAmountAgentCash

			)
		{
			//console.log("7");
			//console.log("אם הדיפולט -תשלום בניצול העברת זהב לקוח- לא הוגדר,  וסכום המיסים גדול מהסכום שהוזן  בדיפולט החדש -סכום מיסים מקסימלי לתשלום במס-ב סוכן- וגם בדיפולט -הגדרת סכום שמעל יבוצע תשלום בקופה סוכן- <> NULL וגם סכום המיסים גדול שווה לסכום שהוגדר בדיפולט -הגדרת סכום שמעל יבוצע תשלום בקופה סוכן-  - יבוצע תשלום באמצעות קופה סוכן");
			//
			//console.log("DefaultPaymentMethod-aboveAmountAgentCash->קופה -סוכן");
			this.updateDefaultPaymentMethod(
				"2",/*קופה*/
				"3" //סוכן מכס

			);

		}
		//8
		else if (
			this.parent._MyDeclarationPM.TotalTax <= maxTaxAgentPayDefault)
		{
			//console.log("אחרת סכום המיסים קטן שווה מהסכום שהוזן בדיפולט החדש -סכום מיסים לתשלום בניצול העברת זהב  8");
			//console.log("DefaultPaymentMethod-->מסב הכנסה -סוכן");
			this.updateDefaultPaymentMethod(
				"1",//מס"ב הכנסה
				"3" //סוכן מכס

			);
		}


	}

	private void updateDefaultPaymentMethod(string methodTypeCode, string payerActivityTypeCode)
	{
		MethodTypeCode = methodTypeCode;
		PayerActivityTypeCode = payerActivityTypeCode;
		Amount = this.parent._MyDeclarationPM.TotalTax;
		if (methodTypeCode == "2"/*קופה*/
			||
			methodTypeCode == "79"/*ניצול העברת זהב*/
		)
		{
			//console.log("אם קופה או ניצול העברת זהב - שדה בנק לאפס ");
			BankCode = null;
			InternalBankName = null;
			BranchCode = null;
			AccountNumber = null;
			InternalBankId = null;
		}
	}
	private void LoadBanks()
	{

		CustomBankQueryService customBankQuery = new CustomBankQueryService(this.parent.customContext);
		List<CustomBankList> customBanks = customBankQuery.GetCustomBanksByCard(this.parent._MyDeclarationPM.CustomerId, this.parent._tenant);
		string BlockAgentBankForMasabDefaultValue = "";
		var result = customBanks.FindAll(d => !d.InActive);
		//console.log("[Response] GetCustomBanksForCard: ", result);
		if (result != null)
		{
			DefaultValueQueryService defaultValueQueryService = new DefaultValueQueryService(this.parent._tenant);
			var objCGG_BLOCK_BANK = defaultValueQueryService.GetDefault("ISRAEL", "CGG_BLOCK_BANK", "NON", "NON", this.parent._tenant);//חסימה בבחירת בנק סוכן ללקוח מסב

			if (objCGG_BLOCK_BANK != null)
			{
				BlockAgentBankForMasabDefaultValue = objCGG_BLOCK_BANK;

			}


			CustomsSettingQueryService customsSettingQuery = new CustomsSettingQueryService(this.parent.customContext);
			CustomsSettingPM CustomsSetting = customsSettingQuery.GetSingleByTenant(this.parent._tenant);


			var list = CustomsSetting;

			if (list != null)
			{
				var customsSetting = list;
				BanksList = result;
				if (result.Count() == 0)
				{
					CustomBankListQueryService customBankQueryList = new CustomBankListQueryService(this.parent.customContext);
					QueryOperations queryOperations = new QueryOperations();
					queryOperations.GetAll = true;
					List<CustomBankList> response = customBankQueryList.GetList(queryOperations, this.parent._tenant);
					if (response != null)
					{
						agentBanks = response.FindAll(d => d.PayerTypeCode == "3" && !d.InActive);
						if (agentBanks.Count() == 1)
						{
							SetInternalBankId(agentBanks[0].Id);
							//this.InternalBankId = this.agentBanks[0].Id;
							if (MethodTypeCode != "2" && MethodTypeCode != "79")
							{

								SelectedBank = agentBanks[0];
								SetSelectedBank(agentBanks[0]);
								BanksList = agentBanks;
							}
						}
						else
						{
							if (customsSetting != null && customsSetting.IsConnectedToUniFreight)
							{
								if (!string.IsNullOrEmpty(this.parent.GetCreditInternalBankId))
								{
									SetInternalBankId(this.parent.GetCreditInternalBankId);

								}
								SendCreditToGetBank();
							}
							BanksList = agentBanks;
							if (customsSetting != null && customsSetting.IsConnectedToUniFreight)
							{
								//   GetCustomBankDefaultForCard();
							}
						}
						if (BanksList.Count() > 0)
							setBankis_ABOVE_MSVLK_agent(BanksList.FindAll(x => !x.InActive && x.PayerTypeCode == "3").FirstOrDefault());



					}

				}
				else
				{
					if (customsSetting != null)
					{
						if (!string.IsNullOrEmpty(this.parent.GetCreditInternalBankId))
						{
							SetInternalBankId(this.parent.GetCreditInternalBankId);
							//this.InternalBankId = this.GetCreditInternalBankId;
						}
						//Check GDFDATA - “CGG_BLOCK_BANK” , in case “Y” -   don't allow user to choose a bank that is not connected to the Customer -


						//case: block Agent banks
						if (BlockAgentBankForMasabDefaultValue == "Y")
						{

							CustomBankListQueryService customBankListQuery = new CustomBankListQueryService(this.parent.customContext);
							List<CustomBankList> response = customBankListQuery.GetList(this.parent._tenant);

							if (response != null)
							{

								var agentBanks = new List<CustomBankList>();
								agentBanks = response.FindAll(d => d.PayerTypeCode == "3" && !d.InActive);
								agentBanks = agentBanks.Concat(BanksList).ToList();

								if (result.Count() == 1)
								{
									if (string.IsNullOrEmpty(this.parent.GetCreditInternalBankId))
									{
										SetInternalBankId(BanksList[0].Id);
										//this.InternalBankId = BanksList[0].Id;
									}
									CustomBankList bank = agentBanks.FindAll(d => d.Id == InternalBankId).FirstOrDefault();
									SelectedBank = bank;
									SetSelectedBank(bank);
								}
								else
								{
									PayerActivityTypeCode = "0";
								}

								if (agentBanks.Count() > 0)
									setBankis_ABOVE_MSVLK_agent(agentBanks.FindAll(x => !x.InActive && x.PayerTypeCode == "3").FirstOrDefault());



							}

						}
						//
						//case: do not block Agent banks
						else
						{
							//if no banks
							if (result.Count() == 0)
							{
								CustomBankListQueryService customBankListQuery = new CustomBankListQueryService(this.parent.customContext);
								List<CustomBankList> response = customBankListQuery.GetList(this.parent._tenant);

								if (response != null)
								{
									agentBanks = response.FindAll(d => d.PayerTypeCode == "3" && !d.InActive);
									if (agentBanks.Count() > 0)
									{
										BanksList = agentBanks;
										if (agentBanks.Count() == 1)
										{
											if (InternalBankId == null)
											{
												SetInternalBankId(BanksList[0].Id);
											}
										}
										else
										{
											if (InternalBankId != null)
											{
												CustomBankList bank = BanksList.FindAll(d => d.Id == InternalBankId).FirstOrDefault();
												SelectedBank = bank;
												SetSelectedBank(bank);
											}
										}

										if (BanksList.Count() > 0)
											setBankis_ABOVE_MSVLK_agent(BanksList.FindAll(x => !x.InActive && x.PayerTypeCode == "3").FirstOrDefault());


									}

								}

							}
							//one bank
							else if (result.Count() == 1)
							{
								if (InternalBankId == null)
								{
									SetInternalBankId(BanksList[0].Id);
								}
								CustomBankListQueryService customBankListQuery = new CustomBankListQueryService(this.parent.customContext);
								List<CustomBankList> response = customBankListQuery.GetList(this.parent._tenant);

								if (response != null)

								{
									var agentBanks = new List<CustomBankList>();
									agentBanks = response.FindAll(d => d.PayerTypeCode == "3" && !d.InActive);
									BanksList = BanksList.Concat(agentBanks).ToList();
									if (InternalBankId != null)
									{
										CustomBankList bank = BanksList.FindAll(d => d.Id == InternalBankId).FirstOrDefault();
										SelectedBank = bank;
										SetSelectedBank(bank);
									}

									if (BanksList.Count() > 0)
										setBankis_ABOVE_MSVLK_agent(BanksList.FindAll(x => !x.InActive && x.PayerTypeCode == "3").FirstOrDefault());

								}

							}

							//two or more & dont block agent
							else if (result.Count() > 1)
							{
								List<CustomBankList> agentBanks;
								//fill connected banks
								var connectedBanks = result.FindAll(d => !d.InActive);

								//fill agent
								CustomBankListQueryService customBankListQuery = new CustomBankListQueryService(this.parent.customContext);
								List<CustomBankList> response = customBankListQuery.GetList(this.parent._tenant);

								if (response != null)
								{
									agentBanks = response.FindAll(d => d.PayerTypeCode == "3" && !d.InActive);
									//fill the LOV
									BanksList = (List<CustomBankList>)connectedBanks.Concat(agentBanks).ToList();

									//select bank
									if (InternalBankId != null)
									{
										CustomBankList bank = BanksList.FindAll(d => d.Id == InternalBankId).FirstOrDefault();
										SelectedBank = bank;
										SetSelectedBank(bank);
									}

									if (BanksList.Count() > 0)
										setBankis_ABOVE_MSVLK_agent(BanksList.FindAll(x => !x.InActive && x.PayerTypeCode == "3").FirstOrDefault());

								}
							}


						}
					}
				}

			}
		}

	}
	public void setBankis_ABOVE_MSVLK_agent(CustomBankList bank)
	{
		string myCIM_AGENT_BANK = "";
		CustomBankListQueryService customBankQueryList = new CustomBankListQueryService(this.parent.customContext);
		QueryOperations queryOperations = new QueryOperations();
		queryOperations.GetAll = true;
		List<CustomBankList> allCustomBankList = customBankQueryList.GetList(queryOperations, this.parent._tenant);

		if (!string.IsNullOrWhiteSpace(this.parent._MyDeclarationPM.CustomerCode))
		{
			DefaultValueQueryService defaultValueQueryService = new DefaultValueQueryService(this.parent._tenant);

			myCIM_AGENT_BANK = defaultValueQueryService.GetDefault("ISRAEL", "CIM_AGENT_BANK", "NON", this.parent._MyDeclarationPM.CustomerCode, this.parent._tenant);//דיפןלט בנק
		}

		if (is_ABOVE_MSVLK_agent && (!string.IsNullOrEmpty(myCIM_AGENT_BANK) || bank != null))
		{
			if(!string.IsNullOrEmpty(myCIM_AGENT_BANK))
				this.SetParamInternalBankId(allCustomBankList, myCIM_AGENT_BANK);
			else if (bank != null)
				SetInternalBankId(bank.Id);

			SelectedBank = bank;

		}
	}
	private void SendCreditToGetBank()
	{
		if (!string.IsNullOrEmpty(InternalBankId)) return;
		var objectTableDecId = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");

		CustomFileCreditRequestParams searchParams = new CustomFileCreditRequestParams();
		{
			searchParams.Tenant = this.parent._MyDeclarationPM.Tenant;
			searchParams.AppicationId = this.parent._MyDeclarationPM.Id;//this.entityParent.DeclarationId;
			searchParams.LoggingEnabled = true;
			searchParams.LoggingEntityId = this.parent._MyDeclarationPM.Id; //entityParent.DeclarationId;
			searchParams.LoggingObjectTableId = objectTableDecId;
			searchParams.LoggingUserId = AuthenticationUtil.ResolveUserId(this.parent._tenant);
			searchParams.RequestName = "Send Credit to Get Bank Request";
			searchParams.ResponseName = "Get Credit to Get Bank Response";
			searchParams.Mode = "GetBank";
		}

		searchParams.RequestVIA = SendRequestVIA.Default;

		CustomBankListQueryService customBankQueryList = new CustomBankListQueryService(this.parent.customContext);
		QueryOperations queryOperations = new QueryOperations();
		queryOperations.GetAll = true;
		List<CustomBankList> allCustomBankList = customBankQueryList.GetList(queryOperations, this.parent._tenant);


		CustomFileCreditResponseData customFileCreditResponseData = this.parent.CustomFileCredit(searchParams);
		SetInternalBankId("");
		string mess = this.AnalyzeResponseMessage(allCustomBankList, customFileCreditResponseData);
		if (string.IsNullOrEmpty(InternalBankId) && !string.IsNullOrEmpty(this.parent._MyDeclarationPM.CustomerCode))
		{
			string bank = "";

			if (!string.IsNullOrWhiteSpace(this.parent._MyDeclarationPM.CustomerCode))
			{
				DefaultValueQueryService defaultValueQueryService = new DefaultValueQueryService(this.parent._tenant);

				string myCIM_AGENT_BANK = defaultValueQueryService.GetDefault("ISRAEL", "CIM_AGENT_BANK", "NON", this.parent._MyDeclarationPM.CustomerCode, this.parent._tenant);//דיפןלט בנק


				this.SetParamInternalBankId(allCustomBankList, myCIM_AGENT_BANK);
			}




			//SessionLocator.SelectedSession.StopBusyIndicator();



		}
		else
		{
			//SessionLocator.SelectedSession.StopBusyIndicator();
		}

	}
	public void SetParamInternalBankId(List<CustomBankList> allCustomBankList, string BankCode)
	{

		if (!string.IsNullOrEmpty(BankCode))
		{
			CustomBankList bank = allCustomBankList.FindAll(d => d.InternalCode == BankCode && !d.InActive).FirstOrDefault();

			//.filter(d => d.BankCode == BankCode && !d.InActive)[0];

			if (bank != null)
			{
				SetInternalBankId(bank.Id);
			}
		}
	}

	private string AnalyzeResponseMessage(List<CustomBankList> allCustomBankList, CustomFileCreditResponseData responseData)
	{
		string message = "";

		if (responseData != null)
		{

			SetParamInternalBankId(allCustomBankList, responseData.BankCode);
			bool test = false;
			if (test)
			{
				this.parent.paymentPM.PaymentDate = DateTime.Now.AddDays(-5);
			}
			if (responseData.CreditStatus == "1")
			{
				return null;
			}
			else
			{
				message = responseData.UserMessage;
				if (string.IsNullOrEmpty(responseData.UserMessage))
				{
					if (!responseData.HasException && responseData.Succeeded)
					{
						message = "Send Get Bank Request Succeeded";
					}
					else
					{
						message = "Send Get Bank Request Failed";
					}
				}
				if (!responseData.HasException && responseData.Succeeded)
				{

				}
			}
		}
		else
		{
			//message = "Service returned a null response!";
			message = TranslateTextsClass.Translate("Customs.Declaration.O.Servicereturnedanullresponse", this.parent._tenant, true);
		}

		return message;
	}
}
#endregion
