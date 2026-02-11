using Logitude.AmitalMessaging.Customs.CustomFile.ReleaseFile;
using Logitude.AmitalMessaging.Infrastructure;
using Logitude.AmitalMessaging.Utils;
using Logitude.Customs.Def.Contracts;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.Messaging.Customs;
using Logitude.Customs.BL.Messaging.U2L.ImportDeclaration;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityLists;
using Logitude.Customs.Data.Repsitories;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.Server.Tools.Contracts;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.Models;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using Simplog.Server.Infrastructure.Helpers;
using System.Transactions;
using Unifreight.BL.EntityPMs;
using Unifreight.BL.EntityQueryServices;
using Unifreight.BL.EntityUpdateServices;
using Unifreight.Data.AmitalModel;
using Logitude.Customs.BL.Validators;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Security;
using Logitude.Customs.Def.Messaging.Customs;
using Logitude.Customs.BL.Messaging.L2U.CustomFile;
using Logitude.AmitalMessaging.Customs.CustomFile;
using Logitude.Customs.BL.TraceEvents;
using Unifreight.Data.AmitalModel.Repsitories;
using Logitude.BL.CommonDataModel.APIDataContract.ApiV1;
using Simplog.Data.CommonDataModel;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using System.Net;
using System.Web;
using User = Simplog.Data.CommonDataModel.EntityPOCOs.User;
using Logitude.Customs.BL.BL;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.Messaging.U2L.Scheduler
{
	public class SchedulerService : UnifreightGenericService
	{
		private LOGISCHEDULER _LOGISCHEDULER;
		private LogitudeScheduler _LogitudeScheduler;
		private ICustomContext _context;

		public const string UpsertActionConst = "Logitude.Customs.BL.Messaging.U2L.Scheduler.SchedulerService.Upsert()";
		private DeclarationPM _MyDeclarationPM;
		private Stopwatch _Stopwatch;
		public Boolean forcePersonalSign;
		public Boolean changeDraftDate;

		public SchedulerService()
			: base(
			"1.000.000001",
			System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
			true
			)
		{

		}

		public override void ProccessGenericRequest(
			  string xmlLOGISCHEDULER,
			  ref string MoreParams,
			  out string MessageOut)
		{
			MessageOut = "";
			_Stopwatch = Stopwatch.StartNew();
			MyCommunicationsParams.Subject = "SchedulerService ";

			DeserilazeObject(xmlLOGISCHEDULER);
			AppendLogLine("DeserilazeObject:Took:" + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart();

			CheckIntegrity();
			AppendLogLine("CheckIntegrity:Took:" + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart();
			MyGenericResponseObj.Stage = "GetContext";
			_context = CustomContext.GetContext(ResolvedTenant());
			AppendLogLine("GetContext:Took:" + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart();

			MyGenericResponseObj.Stage = "Send Request " + _LogitudeScheduler.Request_Code;

			if (_LogitudeScheduler.Param2 == "SIGN") forcePersonalSign = true;
			if (_LogitudeScheduler.More_Param == "CHANGE_DATE") changeDraftDate = true;

			switch (_LogitudeScheduler.Request_Code)
			{
				case "8347":
					SendCurrencyRateRequest();
					break;
				case "8361":
					SendCustomsBookUpdateRequest();
					break;
				case "8302":
					SendDeclarationPrintRequest();
					break;
				case "8240":
					SendCargoQueryRequest();
					break;
				case "8250":
					SendDeclarationStatusRequest();
					break;
				case "DIAMONDS":
					DeclarationChecksForDiamonds();
					break;
				case "DeletePending":
					DeletePending();
					break;
				case "Payment":
					AutoPayment();
					break;
				case "IsReferantAddOn":
					CheckIsReferantAddOn();
					break;
				case "IsDeclarationDisplayOnly":
					CheckIsDeclarationDisplayOnly();
					break;
				case "TEST":
					SendGenericRequest();
					break;
				default:
					SendGenericRequest(); // moran 31.7.16 - AMI-57751 - unifreight use of interface 2750 for sending declaration to customs
					break;
			}

			AppendLogLine("send request:Took:" + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart();
			MyGenericResponseObj.Stage = "Done All ";
			MyGenericResponseObj.StatusType = GenericResponseObj.StatusEnum.Success;

		}

		private void CheckIsDeclarationDisplayOnly()
		{
			string user = this.MyCommunicationsParams.LoggingUserId;
			if (String.IsNullOrWhiteSpace(user)) user = AuthenticationUtil.ResolveUserId(ResolvedTenant());

			if (!String.IsNullOrWhiteSpace(_LogitudeScheduler.Param1))
			{
				GetDeclarationPM(_LogitudeScheduler.Param1);
				if (_MyDeclarationPM == null)
				{
					throw new BusinessErrorException("Declaration with ID " + _LogitudeScheduler.Param1 + " Doesn't exist");
				}
				else
				{
					var responseXML = new IsDeclarationDisplayOnlyResponseXML();

					CustomsRequestsSheetQueryService customsRequestsSheetQueryService = new CustomsRequestsSheetQueryService(_context);
					List<CustomsRequestsSheetPM> customsRequestsSheetPMList = customsRequestsSheetQueryService.GetRequestInProgress(_MyDeclarationPM.Tenant, "2750", "", "", null, null, _MyDeclarationPM.CustomFileNo, true);
					if (customsRequestsSheetPMList != null)
					{
						if (customsRequestsSheetPMList.Count > 0)
						{
							var RequestInProgressInterfaceTypeName = customsRequestsSheetPMList.First().InterfaceTypeName;
							var text = TranslateTextsClass.Translate("Customs.General.RequestInProgress", _MyDeclarationPM.Tenant, true);
							MyGenericResponseObj.Message = String.Format(text, RequestInProgressInterfaceTypeName);
							responseXML.isDeclarationDisplayOnly = "T";
							responseXML.message = MyGenericResponseObj.Message;
						}
					}
					if (responseXML == null || responseXML.isDeclarationDisplayOnly != "T")
					{
						//Check if Declaration was already paid, constraint in progress or Future payment was done
						var declarationValidator = new Logitude.Customs.BL.Validators.DeclarationValidator(_MyDeclarationPM);
						declarationValidator.DeclarationViewDisplayOnlyChecks();
						if (declarationValidator.ErrorCode.Count > 0)
						{
							MyGenericResponseObj.Message = TranslateTextsClass.Translate(declarationValidator.ErrorCode[0], _MyDeclarationPM.Tenant, true);
							//MyGenericResponseObj.StatusType = GenericResponseObj.StatusEnum.BusinessError;
							responseXML.isDeclarationDisplayOnly = "T";
							responseXML.message = MyGenericResponseObj.Message;
						}
					}

					if (responseXML != null)
					{
						var xml = XmlGenericUtil<IsDeclarationDisplayOnlyResponseXML>.SerializeObject(responseXML);
						MyGenericResponseObj.ResponseXml = xml;
					}
				}
			}
			else
			{
				throw new BusinessErrorException("Declaration ID is missing");
			}

			MyGenericResponseObj.ApplicationId = _MyDeclarationPM.Id;
		}
		private void DeletePending()
		{
			DeclarationCourierStatusPM currentDeclarationCourierStatusPM;
			if (!String.IsNullOrWhiteSpace(_LogitudeScheduler.Param1))
			{
				GetDeclarationPM(_LogitudeScheduler.Param1);
				if (_MyDeclarationPM == null)
				{
					throw new BusinessErrorException("Declaration with ID " + _LogitudeScheduler.Param1 + " Doesn't exist");
				}
				else
				{
					var declarationPendingCode = _LogitudeScheduler.Param2;
					MyGenericResponseObj.ApplicationId = _MyDeclarationPM.Id;
					if (String.IsNullOrWhiteSpace(declarationPendingCode))
					{
						throw new BusinessErrorException("Pending code is missing");
					}

					if (_MyDeclarationPM.DeclarationStatusTypeCode == "1")
					{
						AppendLogLine("Declaration Status Request canceled because Declaration Status is 1 (canceled) " + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart();
						return;
					}
					if (_MyDeclarationPM.IsClose)
					{
						AppendLogLine("Declaration Status Request canceled because Declaration Is Closed " + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart();
						return;
					}

					DeclarationCourierStatusQueryService declarationCourierStatusQueryService = new DeclarationCourierStatusQueryService(_context);
					currentDeclarationCourierStatusPM = declarationCourierStatusQueryService.GetSingle(_MyDeclarationPM.Id, true, false);


					if (currentDeclarationCourierStatusPM != null)
					{
						CourierPendingReasonQueryService myCourierPendingReasonQueryService = new CourierPendingReasonQueryService(_context);
						CourierPendingReasonPM courierPendingReasonPM = myCourierPendingReasonQueryService.GetSingleCourierPendingReasonByCode(declarationPendingCode, _MyDeclarationPM.Tenant);

						//  CourierPendingReasonPM courierPendingReasonPM = myCourierPendingReasonQueryService.GetSingleCourierPendingReasonByCode(declarationPendingCode, _MyDeclarationPM.Tenant);
						if (courierPendingReasonPM == null)
						{
							LogMessagingUtil.Instance.AppendLine("לא קיים קוד Pending = " + declarationPendingCode + " בטבלת סיבות Pending");
							return;
						}
						LogMessagingUtil.Instance.AppendLine("Pending - " + declarationPendingCode);
						DeclarationPendingPM _declarationPendingPM = null;
						if (currentDeclarationCourierStatusPM.DeclarationPendings != null && currentDeclarationCourierStatusPM.DeclarationPendings.Count() > 0)
						{
							_declarationPendingPM = currentDeclarationCourierStatusPM.DeclarationPendings.Where(r => r.DeclarationID == currentDeclarationCourierStatusPM.DeclarationId && r.CourierPendingReasonCode == declarationPendingCode).FirstOrDefault();
						}
						if (_declarationPendingPM == null)
						{
							LogMessagingUtil.Instance.AppendLine("לא קיים קוד Pending = " + declarationPendingCode + " בהצהרה");
							return;
						}
						else if (_declarationPendingPM.Status != "A")
						{
							LogMessagingUtil.Instance.AppendLine(" Pending = " + declarationPendingCode + " לא פעיל");
							return;
						}
						_declarationPendingPM.ChangeSetOp = ChangeSetOperation.Update;
						_declarationPendingPM.Status = "S";
						LogMessagingUtil.Instance.AppendLine("Set Courier Pending Reason Code " + declarationPendingCode + " as Solved");
						if (currentDeclarationCourierStatusPM.ChangeSetOp != ChangeSetOperation.Update) currentDeclarationCourierStatusPM.ChangeSetOp = ChangeSetOperation.Update;
						DeclarationCourierStatusUpdateService declarationCourierStatusUpdateService = new DeclarationCourierStatusUpdateService(_context, new Dictionary<string, IContext>(), _MyDeclarationPM.Tenant);
						declarationCourierStatusUpdateService.Update(currentDeclarationCourierStatusPM, true);
					}
					LogMessagingUtil.Instance.AppendLine("ImporterId= " + _MyDeclarationPM.ImporterId + "  ImporterCode=" + _MyDeclarationPM.ImporterCode);

					if (_MyDeclarationPM.ImporterId != null || _MyDeclarationPM.ImporterCode != null)
					{
						FeatureQuery featureQuery = new FeatureQuery(_MyDeclarationPM.Tenant);
						var usrid = AuthenticationUtil.ResolveUserId(_MyDeclarationPM.Tenant);
						var features = featureQuery.GetAllowedFeaturesForLoggedUser(usrid, _MyDeclarationPM.Tenant);
						var feature = features.Features.FirstOrDefault(x => x.Code == "SendDeclaration902");
						if (feature != null)
						{
							_LogitudeScheduler.Request_Code = "2750";
							SendGenericRequest();
						}
					}

				}
			}
			else
			{
				throw new BusinessErrorException("Declaration ID is missing");
			}
		}

		private static string SetBankIdInUnifreightListOnServerOnly(string internalBankId)
		{
			var dic = new Dictionary<string, string>();
			dic.Add("InternalBankId", internalBankId);
			var UnifreightListOnServerOnly = UnifreightListsUtil.Serialize(dic);
			return UnifreightListOnServerOnly;
		}

		public void DelSertPayment(
		DeclarationPM myDeclarationPM,
		string bankId)
		{
			ICustomContext dbContext = CustomContext.GetContext(myDeclarationPM.Tenant);
			ICommonDataContext MyContext = CommonDataContext.GetContext(myDeclarationPM.Tenant);
			UserRepository userRepository = new UserRepository(MyContext);
			var myUser = userRepository.GetSingleUser(myDeclarationPM.SignedByUserId, myDeclarationPM.Tenant);
			var user = AuthenticationUtil.ResolveUserId(myDeclarationPM.Tenant);
			var myQueryService = new DeclarationQueryService(dbContext);


			var mydeclarationPaymentQueryService = new DeclarationPaymentQueryService(dbContext);
			var declarationPaymentPM = mydeclarationPaymentQueryService.GetSingle(myDeclarationPM.Id, true, false);
			if (declarationPaymentPM != null)  //@itzik M אם כבר קיימות שורות אבל אין תאריך תשלום Declaration PaymentDate המשמעות היא שלא שולם בפועל, ולכן למחוק ולכתוב מחדש לפי נתונים נוכחיים.

			{
				if (declarationPaymentPM.DeclarationPaymentMethods.Any())
				{
					foreach (var item in declarationPaymentPM.DeclarationPaymentMethods)
					{
						item.ChangeSetOp = ChangeSetOperation.Delete;
					}

					var myDeclarationPaymentMethodUpdateService = new DeclarationPaymentMethodUpdateService(dbContext, new Dictionary<string, IContext>(), myDeclarationPM.Tenant);
					myDeclarationPaymentMethodUpdateService.UpdateMulti(declarationPaymentPM.DeclarationPaymentMethods, new List<DeclarationPaymentMethodPM>(), declarationPaymentPM
						, true);
				}
				declarationPaymentPM.AutomaticPayment = 1;

				declarationPaymentPM = mydeclarationPaymentQueryService.GetSingle(myDeclarationPM.Id, true, false);
				declarationPaymentPM.ChangeSetOp = ChangeSetOperation.Update;


			}

			declarationPaymentPM = declarationPaymentPM ?? new DeclarationPaymentPM()
			{
				DeclarationId = myDeclarationPM.Id,
				Tenant = myDeclarationPM.Tenant,
				ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
			};



			declarationPaymentPM.PaymentDate = DateTime.Now;
			if (myUser != null)
				declarationPaymentPM.CreatedByUserId = myDeclarationPM.SignerPersonalId == myUser.PersonalId ? myDeclarationPM.SignedByUserId : user;
			else
			{
				declarationPaymentPM.CreatedByUserId = user;
			}
			declarationPaymentPM.AutomaticPayment = 1;
			CustomsSettingQueryService customsSettingQuery = new CustomsSettingQueryService(dbContext);
			CustomsSettingPM CustomsSetting = customsSettingQuery.GetSingleByTenant(myDeclarationPM.Tenant);
			declarationPaymentPM.SignatoryIdentification = myDeclarationPM.IsCourierDeclaration ? CustomsSetting.CustomsAgentId : myDeclarationPM.SignerPersonalId;

			{


				CustomBankQueryService customBankQueryService = new CustomBankQueryService(dbContext);


				var customBank = customBankQueryService.GetSingle(bankId, false, false);
				if (customBank == null)
				{
					throw new System.Exception($"customBankQueryService.GetSingle(bankId={bankId}  return null !! - maybe clear cache on client !!!!!!!@!!!!!@@@@ ");
				}

				DeclarationPaymentMethodPM declarationPaymentMethod = new DeclarationPaymentMethodPM()
				{
					Tenant = myDeclarationPM.Tenant,
					DeclarationId = myDeclarationPM.Id,
					Line = 1,
					SequenceNumeric = 1,
					MethodTypeCode = "1",
					Amount = myDeclarationPM.TotalTax,
					ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,

					BankCode = customBank.BankCode,
					BranchCode = customBank.BranchCode,
					PayerActivityTypeCode = "0",// customBank.PayerTypeCode,
					AccountNumber = customBank.AccountNumber,
					CustomsBranchId = customBank.CustomsBranchId
				};

				declarationPaymentPM.DeclarationPaymentMethods.Add(declarationPaymentMethod);
			}

			DeclarationPaymentUpdateService declarationPaymentUpdateService = new DeclarationPaymentUpdateService(dbContext, new Dictionary<string, IContext>(), myDeclarationPM.Tenant);
			declarationPaymentUpdateService.Update(declarationPaymentPM, true);








		}






		private bool CheckFileCredit(DeclarationPM declarationPM, string user)
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
			CUSTOMCREDIT_UL creditResponseData = myCustomFileCreditService.CheckFileCredit();
			if (!string.IsNullOrEmpty(creditResponseData.CustomFileCredit[0].ErrorMessage))
			{
				return false;
			}

			return true;
		}

		private void AutoPayment()
		{

			MyGenericResponseObj.ApplicationId = _LogitudeScheduler.Param1;

			string user = this.MyCommunicationsParams.LoggingUserId;
			if (String.IsNullOrWhiteSpace(user)) user = AuthenticationUtil.ResolveUserId(ResolvedTenant());

			if (!String.IsNullOrWhiteSpace(_LogitudeScheduler.Param1))
			{
				GetDeclarationPM(_LogitudeScheduler.Param1);
				if (_MyDeclarationPM == null)
				{
					throw new BusinessErrorException("Declaration with ID " + _LogitudeScheduler.Param1 + " Doesn't exist");
				}

			}
			else
			{
				throw new BusinessErrorException("Declaration ID is missing");
			}
			AutoPaymentService autoPaymentService = new AutoPaymentService(_MyDeclarationPM);
			if (this.CheckLock(_MyDeclarationPM))
			{
				this.SendFailedEventAPAYF("התיק נעול", _MyDeclarationPM);
			}
			try
			{


				autoPaymentService.ValidateBeforeCreatingPayment();

				var declarationPaymentPM = autoPaymentService.CreateDeclarationPaymentByLogic(false);

				bool IsValidSend = autoPaymentService.validateBeforeSend(user);

				if (IsValidSend)
				{
					autoPaymentService.SendPaymentIsCheckFileCredit(IsValidSend, _MyDeclarationPM, declarationPaymentPM, user,false);
				}
			}
			catch (Exception ex)
			{
				this.SendFailedEventAPAYF(ex.Message, _MyDeclarationPM);
			}

		}
		private bool CheckLock(DeclarationPM declarationPM)
		{
			LogMessagingUtil.Instance.AppendLine("CourierMaster Send Batch===> CheckLock");

			long lCUSTOMFILENO;
			if (!long.TryParse(declarationPM.CustomFileNo, out lCUSTOMFILENO))
			{
				throw new BusinessErrorException("_DirtyDeclarationPaymentPM.DeclarationId could not convert to long ");
			}
			var myCCUFILEMRepository = new CCUFILEMRepository(declarationPM.Tenant);
			var ccufilem = myCCUFILEMRepository.GetFILENOByCUSTOMFILENO(lCUSTOMFILENO, declarationPM.Tenant);


			var myCCUQUELOCKRepository = new CCUQUELOCKRepository(declarationPM.Tenant);
			try
			{
				var cculock = myCCUQUELOCKRepository.GetSingleGeneralLockNOWAIT("CCUFILEM", ccufilem.ToString());
				return false;

			}
			catch (System.Exception)
			{
				return true;
			}
		}

		private void SendFailedEventAPAYF(string eventRemarks, DeclarationPM _MyDeclarationPM)
		{
			string user = this.MyCommunicationsParams.LoggingUserId;
			ICustomContext dbContext = CustomContext.GetContext(_MyDeclarationPM.Tenant);
			var MyUnifreightEventParam = new UnifreightEventParam()
			{
				Code = "APAYF",
				Mode = UnifreightEventMode.@new,
				EventDateTime = DateTime.Now,
				Entname = "CFIFILEM",
				PrimaryNum = _MyDeclarationPM.CustomFileNo,
				EventRemarks = eventRemarks,
			};

			LogMessagingUtil.Instance.AppendLine("MyUnifreightEventParam = " + MyUnifreightEventParam ?? "NULL");
			var myOpenUnifreighTask = new UnifreightEventTaskService();
			myOpenUnifreighTask.UpsertEventLE2U(
				_MyDeclarationPM.Tenant,
			  user,
				MyUnifreightEventParam);


			DeclarationReferantDataQueryService declarationReferantDataQueryService = new DeclarationReferantDataQueryService(_MyDeclarationPM.Tenant);
			DeclarationReferantDataUpdateService updateService = new DeclarationReferantDataUpdateService(dbContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), _MyDeclarationPM.Tenant);

			var decRef = declarationReferantDataQueryService.GetSingle(_MyDeclarationPM.Id, false, false);

			if (decRef != null)
			{
				decRef.IsManualPayment = true;
				decRef.ChangeSetOp = ChangeSetOperation.Update;
				updateService.Update(decRef, true);
			}
		}

		private void SendDeclarationStatusRequest()
		{
			string user = this.MyCommunicationsParams.LoggingUserId;
			if (String.IsNullOrWhiteSpace(user)) user = AuthenticationUtil.ResolveUserId(ResolvedTenant());

			if (!String.IsNullOrWhiteSpace(_LogitudeScheduler.Param1))
			{
				GetDeclarationPM(_LogitudeScheduler.Param1);
				if (_MyDeclarationPM == null)
				{
					throw new BusinessErrorException("Declaration with ID " + _LogitudeScheduler.Param1 + " Doesn't exist");
				}
				else
				{
					if (_MyDeclarationPM.DeclarationStatusTypeCode == "1")
					{
						AppendLogLine("Declaration Status Request canceled because Declaration Status is 1 (canceled) " + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart();
						return;
					}
					if (_MyDeclarationPM.IsClose)
					{
						AppendLogLine("Declaration Status Request canceled because Declaration Is Closed " + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart();
						return;
					}
				}
			}
			else
			{
				throw new BusinessErrorException("Declaration ID is missing");
			}

			var requestParams = new DeclarationStatusRequestParams()
			{
				LoggingEnabled = true,
				IsFakeResponse = true,
				InterfaceTypeCode = _LogitudeScheduler.Request_Code,
				CustomFileNo = _MyDeclarationPM.CustomFileNo,
				DeclarationNumber = _MyDeclarationPM.DeclarationNumber,
				Tenant = ResolvedTenant(),
				RequestName = "Declaration Status Search",
				ResponseName = "Declaration Status Search",
				CargoRadio = false,
				DeclarationRadio = true,
				OldReshimonRadio = false,
				OldReshimonNumber = null,
				LoggingEntityId = _MyDeclarationPM.Id,
				RequestVIA = SendRequestVIA.WebServiceBatch,
				SuppressSplitWR = true
			};

			MyGenericResponseObj.ApplicationId =
			SBQMessageService.CreateSheetSBQMessage<DeclarationStatusRequestParams>(requestParams, false);
		}

		private void SendCargoQueryRequest()
		{

			string CargoTypeCode = null;
			string ManifestNumber = null;
			string SecondCargoID = null;
			string ThirdCargoID = null;
			string user = this.MyCommunicationsParams.LoggingUserId;
			if (String.IsNullOrWhiteSpace(user)) user = AuthenticationUtil.ResolveUserId(ResolvedTenant());

			if (!String.IsNullOrWhiteSpace(_LogitudeScheduler.Param1))
			{
				GetDeclarationPM(_LogitudeScheduler.Param1);
				if (_MyDeclarationPM == null)
				{
					throw new BusinessErrorException("Declaration with ID " + _LogitudeScheduler.Param1 + " Doesn't exist");
				}
				if (_MyDeclarationPM.DeclarationStatusTypeCode == "1")
				{
					AppendLogLine("Declaration Status Request canceled because Declaration Status is 1 (canceled) " + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart();
					return;
				}
				if (_MyDeclarationPM.IsClose)
				{
					AppendLogLine("Declaration Status Request canceled because Declaration Is Closed " + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart();
					return;
				}
			}

			if (_MyDeclarationPM.Consignments != null && _MyDeclarationPM.Consignments.Count() > 0)
			{
				CargoTypeCode = _MyDeclarationPM.Consignments[0].CargoTypeCode;
				ManifestNumber = _MyDeclarationPM.Consignments[0].ManifestNumber;
				SecondCargoID = _MyDeclarationPM.Consignments[0].SecondCargoID;
				ThirdCargoID = _MyDeclarationPM.Consignments[0].ThirdCargoID;
			}

			if (string.IsNullOrEmpty(CargoTypeCode) || string.IsNullOrEmpty(ManifestNumber))
			{
				string error = TranslateTextsClass.Translate("Customs.Declaration.O.CargoDataMissing", _MyDeclarationPM.Tenant);
				throw new BusinessErrorException(error);
			}

			CargoQueryRequestParams requestParams = new CargoQueryRequestParams()
			{
				Tenant = ResolvedTenant(),
				IsFakeResponse = true,
				LoggingEnabled = true,
				InterfaceTypeCode = _LogitudeScheduler.Request_Code,
				LoggingUserId = user,
				RequestVIA = SendRequestVIA.WebServiceBatch,
				CustomsFile = _MyDeclarationPM.CustomFileNo,
				DeclarationNumber = _MyDeclarationPM.DeclarationNumber,
				DeclarationId = _MyDeclarationPM.Id,
				CargoTypeCode = CargoTypeCode,
				ManifestNumber = ManifestNumber,
				SecondCargoID = SecondCargoID,
				ThirdCargoID = ThirdCargoID,
				RequestName = "Manifest Status Query",
				ResponseName = "Manifest Status Query",
				AutoSend = true,
			};
			MyGenericResponseObj.ApplicationId =
			SBQMessageService.CreateSheetSBQMessage<CargoQueryRequestParams>(requestParams, false);
		}

		private void SendDeclarationPrintRequest()
		{
			string user = this.MyCommunicationsParams.LoggingUserId;
			int tennat = ResolvedTenant();
			if (String.IsNullOrWhiteSpace(user)) user = AuthenticationUtil.ResolveUserId(tennat);
			ICustomContext dbContext = CustomContext.GetContext(tennat);
			DeclarationRepository declarationRepository = new DeclarationRepository(dbContext);

			DF_NG_8302_Web03_DeclarationPrintRequestParams requestParams = new DF_NG_8302_Web03_DeclarationPrintRequestParams()
			{
				IsFakeResponse = true,
				LoggingEnabled = false,
				RequestName = "בקשה לטופס הצהרה",
				ResponseName = "בקשה לטופס הצהרה",
				Tenant = ResolvedTenant(),
				IsSearchByDeclarationRadio = true,
				IsSearchByCargoRadio = false,
				InterfaceTypeCode = _LogitudeScheduler.Request_Code,
				LoggingUserId = user,
				RequestVIA = SendRequestVIA.WebServiceBatch,
			};
			if (!string.IsNullOrWhiteSpace(_LogitudeScheduler.Param1))
			{
				var declarationNumber = _LogitudeScheduler.Param1;
				requestParams.DeclarationNumber = new List<string>();
				requestParams.DeclarationNumber.Add(declarationNumber);
				var id = declarationRepository.GetIdByDeclarationNumber(declarationNumber, tennat);
				requestParams.LoggingEntityId = id;
			}

			MyGenericResponseObj.ApplicationId =
			SBQMessageService.CreateSheetSBQMessage<DF_NG_8302_Web03_DeclarationPrintRequestParams>(requestParams, false);
		}

		private void SendCustomsBookUpdateRequest()
		{
			string user = this.MyCommunicationsParams.LoggingUserId;
			if (String.IsNullOrWhiteSpace(user)) user = AuthenticationUtil.ResolveUserId(ResolvedTenant());



			var logContext = CustomContext.GetContext(ResolvedTenant());
			var customsBookQueryService = new CustomsBookQueryService(logContext);
			CustomsBookPM dbCustomsBookPM = customsBookQueryService.GetCustomsBookData();
			DateTime fromDate = DateTime.Now.AddDays(-7);
			DateTime toDate = DateTime.Now;
			if (dbCustomsBookPM != null)
			{
				fromDate = (DateTime)dbCustomsBookPM.LastUpdateDate == null ? DateTime.Now.AddDays(-7) : (DateTime)dbCustomsBookPM.LastUpdateDate;
				toDate = (DateTime)dbCustomsBookPM.LastUpdateDate == null ? DateTime.Now : fromDate.AddDays(7);

			}
			string fromDateString = fromDate.ToString("yyyyMMdd");
			string toDateString = toDate.ToString("yyyyMMdd");

			CustomsBookInRequestParams requestParams = new CustomsBookInRequestParams()
			{
				IsFakeResponse = true,
				LoggingEnabled = false,
				RequestName = "עדכון ספר סיווג",
				ResponseName = "עדכון ספר סיווג",
				Tenant = ResolvedTenant(),
				isGetHistoricalData = false,
				//fromDate = AmitalConvertUtil.GetUnifreightFormatedDate(_LogitudeScheduler.Start_Date, "_LogitudeScheduler.Start_Date"),
				fromDate = AmitalConvertUtil.GetUnifreightFormatedDate(fromDateString, "fromDateString"),
				fromDateSpecified = true,
				//toDate = AmitalConvertUtil.GetUnifreightFormatedDate(_LogitudeScheduler.End_Date, "_LogitudeScheduler.End_Date"),
				toDate = AmitalConvertUtil.GetUnifreightFormatedDate(toDateString, "toDateString"),
				toDateSpecified = true,
				InterfaceTypeCode = _LogitudeScheduler.Request_Code,
				LoggingUserId = user,
				RequestVIA = SendRequestVIA.DCABatch,
			};
			MyGenericResponseObj.ApplicationId =
			SBQMessageService.CreateSheetSBQMessage<CustomsBookInRequestParams>(requestParams, false);
		}


		private void SendCurrencyRateRequest()
		{

			string user = this.MyCommunicationsParams.LoggingUserId;
			if (String.IsNullOrWhiteSpace(user)) user = AuthenticationUtil.ResolveUserId(ResolvedTenant());

			CD_NG_8347_Web01_CurrencyRateSearchRequestParams requestParams = new CD_NG_8347_Web01_CurrencyRateSearchRequestParams()
			{
				Tenant = ResolvedTenant(),
				IsFakeResponse = true,
				RequestName = "Send Currency Rate Request",
				ResponseName = "Send Currency Rate Response",
				CurrencyTypeId = _LogitudeScheduler.Param1,
				FromDate = AmitalConvertUtil.GetUnifreightFormatedDate(_LogitudeScheduler.Start_Date, "_LogitudeScheduler.Start_Date"),
				ToDate = AmitalConvertUtil.GetUnifreightFormatedDate(_LogitudeScheduler.End_Date, "_LogitudeScheduler.End_Date"),
				LoggingEnabled = true,
				InterfaceTypeCode = _LogitudeScheduler.Request_Code,
				LoggingUserId = user,
				RequestVIA = SendRequestVIA.WebServiceBatch,

			};
			MyGenericResponseObj.ApplicationId =
			SBQMessageService.CreateSheetSBQMessage<CD_NG_8347_Web01_CurrencyRateSearchRequestParams>(requestParams, false);
		}

		private void SendGenericRequest()
		{
			string id = null;
			string tableId = null;
			string declarationNumber = null;
			string requestName = "Send Generic Request";
			string responseName = "Send Generic Response";

			if (String.IsNullOrWhiteSpace(_LogitudeScheduler.Request_Code))
			{
				throw new BusinessErrorException("Request code is missing");
			}

			if (_LogitudeScheduler.Request_Code == "2750")
			{
				requestName = "Declaration Request";
				responseName = "Declaration Response";
			}
			if (!String.IsNullOrWhiteSpace(_LogitudeScheduler.Param1))
			{
				GetDeclarationPM(_LogitudeScheduler.Param1);
				if (_MyDeclarationPM != null)
				{
					id = _MyDeclarationPM.Id;
					tableId = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
					declarationNumber = _MyDeclarationPM.DeclarationNumber;
				}
			}

			if (_LogitudeScheduler.Request_Code == "2750" && this._MyDeclarationPM != null && !String.IsNullOrWhiteSpace(this._MyDeclarationPM.CustomFileNo))
			{
				ClearCCUFILEMdraftStatus();
				if (changeDraftDate == true)
				{
					this._MyDeclarationPM.ChangeSetOp = ChangeSetOperation.Update;
					this._MyDeclarationPM.TaxationDateTime = DateTime.Now;
				}
				if (this._MyDeclarationPM.ChangeSetOp == ChangeSetOperation.Update)
				{
					UpdateDeclaration();
				}
			}
			string user = this.MyCommunicationsParams.LoggingUserId;
			if (String.IsNullOrWhiteSpace(user)) user = AuthenticationUtil.ResolveUserId(ResolvedTenant());

			GenericRequestParams requestParams = new GenericRequestParams()
			{
				Tenant = ResolvedTenant(),
				IsFakeResponse = true,
				RequestName = requestName,
				ResponseName = responseName,
				LoggingEnabled = true,
				LoggingEntityId = id,
				AppicationId = id,
				InterfaceTypeCode = _LogitudeScheduler.Request_Code,
				LoggingObjectTableId = tableId,
				LoggingEntityReference = declarationNumber,
				LoggingUserId = user,
				RequestVIA = SendRequestVIA.WebServiceBatch,
				ForcePersonalSign = forcePersonalSign,
			};
			MyGenericResponseObj.ApplicationId =
			SBQMessageService.CreateSheetSBQMessage<GenericRequestParams>(requestParams, false);
			AppendLogLine(requestName + " Took:" + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart();
		}

		private void UpdateDeclaration()
		{

			MyGenericResponseObj.Stage = "Update Declaration";

			TransactionScope scope = null;
			ICustomContext context;

			if (this._MyDeclarationPM == null)
			{
				throw new BusinessErrorException("Declaration is missing");
			}
			if (String.IsNullOrWhiteSpace(this._MyDeclarationPM.CustomFileNo))
			{
				throw new BusinessErrorException("Declaration Customs File No. is missing");
			}

			if (!DbContextBaseUtil.UnifreightDataIncludedInMain_FeatureOn)
			{
				scope = TransactionFactory.GetNewOracleReadCommittedTransaction();
			}
			try
			{
				context = CustomContext.GetContext(ResolvedTenant());
				var myDeclarationUpdateService = new DeclarationUpdateService(context, new Dictionary<string, IContext>(), ResolvedTenant());
				if (this._MyDeclarationPM.ChangeSetOp != ChangeSetOperation.Update) this._MyDeclarationPM.ChangeSetOp = ChangeSetOperation.Update;

				using (var logger = (context as DbContextBase).CreateLogger())
				{
					try
					{
						myDeclarationUpdateService.Update(this._MyDeclarationPM, true);
					}
					catch (Exception eUpdate)
					{
						LogMessagingUtil.Instance.Append("DeclarationUpdateService.Update:");
						LogMessagingUtil.Instance.AppendLine(logger.ToString(2040));
						throw;
					}
					AppendLogLine("DeclarationUpdateService.Update:Took:" + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart();
				}
			}

			finally
			{
				if (scope != null)
				{
					scope.Dispose();
				}
			}
		}

		private void ClearCCUFILEMdraftStatus()
		{
			MyGenericResponseObj.Stage = "CCUFILEMUpdate (Clear MEHESDRAFTSTATUS) ";
			CCUFILEMPM _CCUFILEMPM;
			long lCUSTOMFILENO;
			AmitalContext _AmitalContext;
			if (!long.TryParse(this._MyDeclarationPM.CustomFileNo, out lCUSTOMFILENO))
			{
				throw new BusinessErrorException("dirtyDeclarationPM.CustomFileNo could not convert to long ");
			}
			TransactionScope scope = null;

			if (!DbContextBaseUtil.UnifreightDataIncludedInMain_FeatureOn)
			{
				scope = TransactionFactory.GetNewOracleReadCommittedTransaction();
			}
			try
			{
				using (_AmitalContext = AmitalContext.GetContext(this._MyDeclarationPM.Tenant))
				{

					var myCCUFILEMQueryService = new CCUFILEMQueryService(_AmitalContext);
					var myCCUFILEMUpdateService = new CCUFILEMUpdateService(_AmitalContext);
					myCCUFILEMUpdateService.DontAddTransaction = true;//we cant add a transaction with isolation level snap shot inside a read committed one so you have to assign this prop to true mohammad.

					int? FILENO = myCCUFILEMQueryService.GetFILENOByCUSTOMFILENO(lCUSTOMFILENO,_MyDeclarationPM.Tenant);
					if (FILENO.HasValue)
					{
						int? FILENO1 = myCCUFILEMQueryService.GetFILENOByCUSTOMFILENO_forUpdateNOWAIT(lCUSTOMFILENO, this._MyDeclarationPM.Tenant);

						//do not need the composite due we delete all down entities !!!_CCUFILEMPM = myCCUFILEMQueryService.GetSingle(FILENO.Value, true, false);
						_CCUFILEMPM = myCCUFILEMQueryService.GetSingle(FILENO.Value, _MyDeclarationPM.Tenant, false, false);
						_CCUFILEMPM.ChangeSetOp = ChangeSetOperation.Update;
						_CCUFILEMPM.MEHESDRAFTSTATUS = null;
						using (var logger = (_AmitalContext as DbContextBase).CreateLogger())
						{
							try
							{
								myCCUFILEMUpdateService.Update(_CCUFILEMPM, true);
							}
							catch (Exception eUpdate)
							{
								LogMessagingUtil.Instance.Append("CCUFILEMUpdateService.Update:");
								LogMessagingUtil.Instance.AppendLine(logger.ToString(2040));
								throw;
							}
							AppendLogLine("CCUFILEMUpdateService.Update:Took:" + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart();
						}
					}
				}
			}
			finally
			{
				if (scope != null)
				{
					scope.Dispose();
				}
			}
		}

		private void GetDeclarationPM(string logitudeFile)
		{

			if (String.IsNullOrWhiteSpace(logitudeFile))
			{
				throw new BusinessErrorException("LOGITUDE FILE is missing");
			}
			var myQueryService = new DeclarationQueryService(_context);

			MyGenericResponseObj.Stage = "GetSingle";
			this._MyDeclarationPM = myQueryService.GetSingle(logitudeFile, true, false);
			if (this._MyDeclarationPM == null)
			{
				throw new BusinessErrorException("LOGITUDE FILE is " + logitudeFile + " but not found");
			}
			AppendLogLine("GetSingle:Took:" + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart();

		}


		private void DeclarationChecksForDiamonds()
		{
			string user = this.MyCommunicationsParams.LoggingUserId;
			if (String.IsNullOrWhiteSpace(user)) user = AuthenticationUtil.ResolveUserId(ResolvedTenant());

			if (!String.IsNullOrWhiteSpace(_LogitudeScheduler.Param1))
			{
				GetDeclarationPM(_LogitudeScheduler.Param1);
				if (_MyDeclarationPM == null)
				{
					throw new BusinessErrorException("Declaration with ID " + _LogitudeScheduler.Param1 + " Doesn't exist");
				}
				else
				{
					if (_MyDeclarationPM.DeclarationStatusTypeCode == "1")
					{
						AppendLogLine("Declaration Status Request canceled because Declaration Status is 1 (canceled) " + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart();
						return;
					}
					if (_MyDeclarationPM.IsClose)
					{
						AppendLogLine("Declaration Status Request canceled because Declaration Is Closed " + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart();
						return;
					}
				}
			}
			else
			{
				throw new BusinessErrorException("Declaration ID is missing");
			}

			Boolean ticketValidStatus = DeclarationTicketsStatus(_MyDeclarationPM);

			Boolean mandatoryFields = SendDeclarationMandatoryFields(_MyDeclarationPM);

			bool mandatoryDoc = IsDocumentMissing(_MyDeclarationPM);

			var responseXML = new diamonsResponseXML();
			if (!ticketValidStatus)
			{
				responseXML.MISS_REFERENCE = "T";
			}
			if (!mandatoryFields)
			{
				responseXML.MAND_FIELDS = "T";
			}

			if (!mandatoryDoc)
			{
				responseXML.MAND_DOC = "T";

			}
			if (responseXML != null)
			{
				var xml = XmlGenericUtil<diamonsResponseXML>.SerializeObject(responseXML);
				MyGenericResponseObj.ResponseXml = xml;
			}
			MyGenericResponseObj.ApplicationId = _MyDeclarationPM.Id;
		}


		private void CheckIsReferantAddOn()
		{
			try
			{
				int tenant = ResolvedTenant();
				string email = AuthenticationUtil.ResolveUserIdentityName(tenant);
				string id = RequestSheetContext.Current.GetContextOrDefault().GetUserFromRequestParam();

				if (!string.IsNullOrWhiteSpace(id))
				{
					ContactRepository contactrep = new ContactRepository(tenant);
					var contact = contactrep.GetSingleContact(id, tenant);
					email = contact.Email;

				}
				//InjectionUtil.Instance.CheckContactFeature("Customs.Declaration", "ReferantData", tenant, email);
				FeatureQuery featureQuery = new FeatureQuery(tenant);
				var features = featureQuery.GetAllowedFeaturesForLoggedUser(AuthenticationUtil.ResolveUserId(tenant), tenant);
				var feature = features.Features.FirstOrDefault(x => x.Code == "UniReferantData");

				if (feature != null)
				{
					var responseXML = new isReferantAddOnResponseXML();
					responseXML.isReferantAddOn = "T";
					if (responseXML != null)
					{
						var xml = XmlGenericUtil<isReferantAddOnResponseXML>.SerializeObject(responseXML);
						MyGenericResponseObj.ResponseXml = xml;
						AppendLogLine("Check for UniReferantData was Successful, Referant Data will be transfered from unifreight (user:" + AuthenticationUtil.ResolveUserId(tenant) + ")");
					}
				}
				else
				{
					AppendLogLine("Check for UniReferantData Feature Failed, Referant Data will not be transfered from unifreight (user:" + AuthenticationUtil.ResolveUserId(tenant) + ")");
				}
			}
			catch (SecurityException ex)
			{
				AppendLogLine("Check for UniReferantData Feature Failed,  Referant Data will not be transfered from unifreight (user:" + AuthenticationUtil.ResolveUserId(ResolvedTenant()) + "), Message: " + ex.Message);
			}
		}

		private bool IsDocumentMissing(DeclarationPM myDeclarationPM)
		{
			var customContext = CustomContext.GetContext(myDeclarationPM.Tenant);
			CustomsDocumentsTicketQueryService myCustomsDocumentsTicketQueryService = new CustomsDocumentsTicketQueryService(customContext);
			//     List<CustomsDocumentsTicketPM> customsDocumentsTicketPMList = myCustomsDocumentsTicketQueryService.GetCustomsDocumentsTicketPMsByEntityIdAndChilds(myDeclarationPM.Id, "", "", "", myDeclarationPM.Tenant, "Declaration").Where(r => r.DocumentStatusCode == "1").ToList();
			CustomDocumentTypeQueryService docTypeQuery = new CustomDocumentTypeQueryService(customContext);

			List<CustomDocumentTypePM> documentTypePMs = docTypeQuery.GetMandatoryCustomDocumentTypes(myDeclarationPM.Tenant);

			foreach (var doc in documentTypePMs)
			{
				List<CustomsDocumentsTicketPM> customsDocumentsTicketPMList = myCustomsDocumentsTicketQueryService.GetCustomsDocumentsTicketPMsByEntityIdAndChilds(myDeclarationPM.Id, "", "", "", myDeclarationPM.Tenant, "Declaration").Where(r => r.DocumentTypeCode == doc.Code).ToList();
				if (customsDocumentsTicketPMList == null || customsDocumentsTicketPMList.Count() < 1)
					return true;
			}


			//foreach (CustomsDocumentsTicketPM customsDocumentsTicketPMItem in customsDocumentsTicketPMList)
			//{
			//    CustomDocumentTypePM docType = docTypeQuery.GetSingle(customsDocumentsTicketPMItem.DocumentTypeCode, false, false);
			//    if (docType.IsManadatory)
			//    {

			//}


			return false;

		}
		private bool SendDeclarationMandatoryFields(DeclarationPM myDeclarationPM)
		{
			Boolean sendDeclarationMandatory = true;
			CustomsRequiredFieldErrors errorsForDeclaration = CustomsRequiredFieldsValidator.GetRequiredFieldErrorsForDeclaration(myDeclarationPM.Id, myDeclarationPM.Tenant, myDeclarationPM);
			if (errorsForDeclaration != null && errorsForDeclaration.RequiredFields != null && errorsForDeclaration.RequiredFields.Count() > 0)
			{
				sendDeclarationMandatory = false;
			}
			return sendDeclarationMandatory;
		}

		private Boolean DeclarationTicketsStatus(DeclarationPM entityPM)
		{
			CustomsDocumentsTicketQueryService myCustomsDocumentsTicketQueryService = new CustomsDocumentsTicketQueryService(_context);
			Boolean ticketValidStatus = true;

			List<CustomsDocumentsTicketPM> customsDocumentsTicketPMList = myCustomsDocumentsTicketQueryService.GetCustomsDocumentsTicketPMsByEntityIdAndChilds(entityPM.Id, "", "", "", entityPM.Tenant, "Declaration").ToList();
			if (customsDocumentsTicketPMList != null && customsDocumentsTicketPMList.Count() > 0)
			{

				var DocumentsFilingIdList = new List<string>();
				foreach (var customsDocumentsTicketPM in customsDocumentsTicketPMList)
				{
					if (!string.IsNullOrWhiteSpace(customsDocumentsTicketPM.DocumentsFilingId))
					{
						DocumentsFilingIdList.Add(customsDocumentsTicketPM.DocumentsFilingId);
					}
				}
				var customsDocumentPMList = new List<CustomsDocumentPM>();
				if (DocumentsFilingIdList != null)
				{
					var myCustomsDocumentQueryService = new CustomsDocumentQueryService(_context);
					customsDocumentPMList = myCustomsDocumentQueryService.GetCustomsDocumentList(DocumentsFilingIdList, entityPM.Tenant);
				}
				if (customsDocumentPMList != null && customsDocumentPMList.Count() > 0)

				{
					foreach (var customsDocumentPM in customsDocumentPMList)
					{
						if (customsDocumentPM.DocumentStatusCode != "1")
						{
							ticketValidStatus = false;
							break;
						}
					}
				}
			}
			return ticketValidStatus;
		}


		void DeserilazeObject(string xmlLOGISCHEDULER)
		{

			MyGenericResponseObj.Stage = "Initalize ProccessRequest";
			AppendLogLine("SchedulerService.ProccessRequest");

			AppendLogLine("Deserialize(DataIn1) ..");


			if (string.IsNullOrWhiteSpace(xmlLOGISCHEDULER))
			{
				throw new BusinessErrorException("DataIn1 is missing");
			}
			if (xmlLOGISCHEDULER.Length > 1000)
			{
				AppendLogLine("XmlIn=" + xmlLOGISCHEDULER.Substring(0, 1000));
				AppendLogLine(".Substring(0, 1000)");
			}
			else
			{
				AppendLogLine("XmlIn=" + xmlLOGISCHEDULER);
			}


			AppendLogLine("Tring DeserilazeObject");
			MyGenericResponseObj.Stage = "Trying DeserilazeObject";
			this._LOGISCHEDULER = XmlGenericUtil<LOGISCHEDULER>.DeSerializeObject(xmlLOGISCHEDULER);

			if (_LOGISCHEDULER.LogitudeScheduler == null || _LOGISCHEDULER.LogitudeScheduler.Length != 1)
			{
				throw new BusinessErrorException("_LOGISCHEDULER.Scheduler.Length != 1");
			}
			this._LogitudeScheduler = _LOGISCHEDULER.LogitudeScheduler[0];
		}

		private void CheckIntegrity()
		{
			MyGenericResponseObj.Stage = "Check integrity ";

			if (String.IsNullOrWhiteSpace(this._LogitudeScheduler.Request_Code))
			{
				throw new BusinessErrorException("Request Code is missing");
			}
			AppendLogLine("Request Code = " + this._LogitudeScheduler.Request_Code);

		}



		public override string GetAssemblyQualifiedName()
		{
			throw new NotImplementedException();
		}

		public override string GetExampleDataIn1()
		{
			return "";
		}

		public override string GetExampleDataIn2()
		{
			return "";
		}

		public override string GetExampleDataout1()
		{
			return "";
		}

		public override string GetExampleDataout2()
		{
			return "";
		}

		public override void ProccessRequest(string DataIn1, string DataIn2, out string DataOut1, out string DataOut2, out string SUCCESS, ref string MoreParams, out string MessageOut)
		{
			throw new NotImplementedException();
		}

		public override void ProccessBASE64Request(string BASE64DataIn1, string BASE64DataIn2, string BASE64DataIn3, out string BASE64DataOut1, out string BASE64DataOut2, out string BASE64DataOut3, out string SUCCESS, ref string MoreParams, out string MessageOut)
		{
			throw new NotImplementedException();
		}


	}

	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
	[System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
	public class diamonsResponseXML
	{

		public string MISS_REFERENCE;

		public string MAND_FIELDS;
		public string MAND_DOC;

	}

	public class isReferantAddOnResponseXML
	{

		public string isReferantAddOn;


	}


	public class IsDeclarationDisplayOnlyResponseXML
	{
		public string isDeclarationDisplayOnly;
		public string message;
	}
}

