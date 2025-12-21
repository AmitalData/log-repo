using Devart.Data.Linq;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.Messaging.Customs;
using Logitude.Customs.BL.TraceEvents;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Def.EntityQueryServicesExt;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Helpers;
using Logitude.CustomsMessaging.MessagingServices;
using Logitude.Server.Tools.Contracts;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Logitude.Customs.CustomsMessaging.Tasks
{
    public class ConnectDocumentToDeclarationCourier : ICustomsConnectDocumentToDeclarationCourier
	{
		private DocumentsFilingMetaDataValueQuery _documentsFilingMetaDataValueQuery;
		private ICommonDataContext _DataContext;
		public void StartRun(string taskId, int seedDefaultTenant)
        {

            var customsSettingQueryService = new CustomsSettingQueryService(seedDefaultTenant);
            var allCustomsSetting = customsSettingQueryService.GetAll()?.Where(x=>x.CompanyType == "B");
            if (allCustomsSetting != null && allCustomsSetting.Count() > 0)
            {
				foreach (var t in allCustomsSetting) 
				{ 
					Run(t);
					Thread.Sleep(1000);
				}
            }
        }

        private void Run(CustomsSettingPM t)
        {
            if (t != null)
            {
                try
                {
					LogMessagingUtil.Instance.AppendLine("start ConnectDocumentToDeclarationCourier" + t.Tenant);

					_DataContext = CommonDataContext.GetContext(t.Tenant);
					_documentsFilingMetaDataValueQuery = new DocumentsFilingMetaDataValueQuery(_DataContext);
					DeclarationQueryService declarationQuery = new DeclarationQueryService(t.Tenant);

					var documentsFilingMetaDataValues = _documentsFilingMetaDataValueQuery.GetDocumentsFilingMetaDataValuesByCARFI(t.Tenant, "CARFI");

					foreach (var item in documentsFilingMetaDataValues)
					{
						LogMessagingUtil.Instance.AppendLine($"start ConnectDocumentToDeclarationCourier DocumentsFilingId: {item.DocumentsFilingId}");
						string integratore = _documentsFilingMetaDataValueQuery.GetDocumentsFilingMetaDataValueByINTGR_R(t.Tenant,item.DocumentsFilingId, "INTGR_R")?.MetaDataValue;
						DeclarationPM declaration = declarationQuery.GetDeclarationsByHawbAndIntegratore(item.Tenant, item.MetaDataValue, integratore);
						UpdateDocumentsFiling(item.DocumentsFilingId, declaration.Id, t.Tenant,null, declaration.CustomFileNo);
						LogMessagingUtil.Instance.AppendLine($"finish ConnectDocumentToDeclarationCourier DocumentsFilingId: {item.DocumentsFilingId}");
					}
					LogMessagingUtil.Instance.AppendLine($"finish ConnectDocumentToDeclarationCourier" + t.Tenant);

				}
				catch (Exception ex)
                {
                    LogMessagingUtil.Instance.AppendLine("Exception was thrown while ConnectDocumentToDeclarationCourier " + t.Tenant + Environment.NewLine + ex.Message);
                }
            }
        }

		private void UpdateDocumentsFiling(string documentsFilingId, string DeclarationId, int Tenant, string LoggedUserId, string CustomFileNo = null)
		{
			try
			{
				_DataContext = CommonDataContext.GetContext(Tenant);
				var documentsFilingService = new UnifreightDocumentsFilingService(_DataContext, Tenant, new CustomDocumentsFilingParams() { MainInterfaceCode = "" });
				DocumentsFilingQuery _documentsFilingQuery = new DocumentsFilingQuery(Tenant);
				DocumentsFilingPM documentsFilingPM = _documentsFilingQuery.GetSinglePM(documentsFilingId, Tenant);
				if (documentsFilingPM != null && documentsFilingPM.EntityId != DeclarationId)
				{
					documentsFilingPM.EntityId = DeclarationId;
					documentsFilingPM.ObjectTableId = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
					documentsFilingPM.IsHybrid = true;
					if (!string.IsNullOrEmpty(CustomFileNo))
					{
						documentsFilingPM.EntityId = DeclarationId;
						documentsFilingPM.EntityNumber = CustomFileNo;
						documentsFilingPM.ExternalEntityName = "CFIFILEM";
						documentsFilingPM.ExternalEntityReference = CustomFileNo;
					}
					documentsFilingService.Update(documentsFilingPM, null, LoggedUserId);
					LogMessagingUtil.Instance.AppendLine($"Task UpdateDocumentsFiling: documentsFilingId {documentsFilingId} Connect  document  {documentsFilingPM.Code}  to DeclarationId: {DeclarationId}");
				}
				else
				{
					LogMessagingUtil.Instance.AppendLine($"Task UpdateDocumentsFiling: documentsFilingId {documentsFilingId} GetSinglePM(documentsFilingId)-bad:{documentsFilingPM?.EntityId}");
				}
			}
			catch (Exception ex)
			{
				LogMessagingUtil.Instance.AppendLine($"Task UpdateDocumentsFiling:EXCEPTION:{ex.Message.ToString()} {documentsFilingId}");


			}

		}


	}
}
