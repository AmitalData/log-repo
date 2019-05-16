using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using WebFreight.Web.Helpers;
using Logitude.BL.InfrastructureModel.APIDataContract.ApiV1;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.Repositories;
using Logitude.Server.Tools.StorageService;
using Simplog.Server.Infrastructure.Azure;
using Microsoft.Practices.Unity;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.BL.Resolvers;
using Logitude.Server.Tools.Utils;

namespace Logitude.Accounting.BL.CoreBL
{
    public class System1000Service : ISystem1000Service
    {
        StringBuilder _sb = new StringBuilder();
        private FullAccountingSettingPM _FullAccountingSettingPM;
        private IQueryable<CardGLAccountDataView> _AllVendorGLAccountCards;



        public System1000Service()
        {
            
            
        }


        public List<string> GetSystem1000FlatFile(IAccountingContext accountingContext, int tenant)
        {
            StringBuilder flatFile = new StringBuilder();
            _sb.AppendLine($"GetDeductionFileNumberFromAccSetting({tenant})");
            _FullAccountingSettingPM = GetDeductionFileNumberFromAccSetting(accountingContext, tenant);
            _AllVendorGLAccountCards = GetQAllVendorGLAccountCards(accountingContext, tenant);

            var listOfAccounts = _AllVendorGLAccountCards.ToList();
            if (listOfAccounts.Count == 0)
            {
                return null;
            }
            int chunkSize = 1000;
            var listOf1000 = listOfAccounts.Select((x, i) => new { Index = i, Value = x })
            .GroupBy(x => x.Index / chunkSize)
            .Select(x => x.Select(v => v.Value).ToList())
            .ToList();

            var res = new List<string>();
            foreach (List<CardGLAccountDataView> listOfAccountsMax1000 in listOf1000)
            {
                string header = "A" + _FullAccountingSettingPM.DeductionFileNumber.Replace(" ", "").PadLeft(9, '0').Substring(0, 9);
                flatFile.AppendLine(header);
                int count = 0;
                foreach (var obj in listOfAccountsMax1000)
                {
                    string line = "B" + obj.DisplayNumber.Replace(" ", "").PadLeft(15, '0').Substring(0, 15)
                        + obj.DeductionFileNumber.Replace(" ", "").PadLeft(9, '0').Substring(0, 9)
                        + obj.VatNumber.Replace(" ", "").PadLeft(9, '0').Substring(0, 9);
                    flatFile.AppendLine(line);
                    count++;
                }
                string footer = "Z" + _FullAccountingSettingPM.DeductionFileNumber.Replace(" ", "").PadLeft(9, '0').Substring(0, 9)
                    + count.ToString().PadLeft(4, '0');
                flatFile.AppendLine(footer);

                res.Add(flatFile.ToString());

            }
            return res;
        }

        public string EmailIt(string Email, List<string> flatFiles, int tenant)
        {

            using (var scope = TransactionFactory.GetTransaction())
            {


                System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
                //HtmlEditorHelper htmlEditorHelper = new HtmlEditorHelper();
                EncodedHtmlHelper encodedHtmlHelper = new EncodedHtmlHelper();
                //string htmlstring = "<html>Attach flowing ...</html>";


                //htmlstring = filter.Htmlstring;
                //htmlstring = htmlEditorHelper.GetLogoHtmlString(htmlstring);
                //htmlstring = encodedHtmlHelper.EncodedHtmlScript(htmlstring);


                var context = CommonDataContext.GetContext(tenant);
                var documentRep = new DocumentRepository(context);
                var attList = new List<string>();
                var fileNames = new List<string>();
                int filecount = 1;
                foreach (var flatFile in flatFiles)
                {
                    byte[] flatFileData = (new System.Text.UTF8Encoding()).GetBytes(flatFile);

                    Simplog.Data.CommonDataModel.EntityPOCOs.Document document = new Simplog.Data.CommonDataModel.EntityPOCOs.Document()
                    {
                        CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                        Extension = "txt",
                        FileSize = Convert.ToInt32(flatFileData.Length),
                        Tenant = Convert.ToInt32(tenant),
                        Id = IdCounter.GetNumber("Document", tenant).ToString(),
                        Folder = "docsout",
                        HasFile = true,
                        FileName = $"1000system_Part_{filecount}"

                    };
                    fileNames.Add($"{document.FileName}.txt");
                    documentRep.Add(document);

                    string filename = document.Id + "." + document.Extension;
                    string filePath = "tenant" + tenant.ToString() + "/" + StorageAcountDetails.GetBlobNameByLocation(filename.ToLower(), document.Folder);
                    IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
                    BlobFileInfo fileInfo = new BlobFileInfo()
                    {
                        FileName = document.Id,
                        FolderName = document.Folder,
                        Extension = document.Extension,
                        Tenant = tenant,
                        FileSize = flatFileData.Length,

                    };
                    storageservice.Write(flatFileData, fileInfo);


                    attList.Add(document.Id);
                    filecount++;
                }
                context.SaveChanges();


                string htmlPlainString = "Attach System 1000 List ..." +
                    Environment.NewLine +
                    string.Join(Environment.NewLine, fileNames);

                byte[] bytePlainTextdata = enc.GetBytes(htmlPlainString);


                string userId = AuthenticationUtil.ResolveUserId(tenant);
                string objectTableId = ObjectTableRepository.GetObjectTableByName("GLAccount");
                //string res = ///_HtmlEditorHelper
                //    InjectionUtil.Instance
                //    .SendEmailOutActivityForEntity(null, bytePlainTextdata, tenant,
                //   Email, "Subject", "", "",
                //   userId
                //   , "", "", objectTableId, "", "", "", "");

                string internalDocumentId = null;
                string externalDocumentId = null;

                string entityId = null;
                string attachments = String.Join(",", attList.ToArray()); ;
                string entityReference = null;
                string from = "no-reply@LogitudeWorld.com";
                string replyTo = "";
                string res = /*htmlEditorHelper*/InjectionUtil.Instance.SendHtmlDocument(
                    bytePlainTextdata/*htmlData*/, internalDocumentId, externalDocumentId, tenant, Email, "subject", "", "", userId, entityId, objectTableId, attachments,
                    entityReference, from, replyTo); // Islam: circular reference issue with the web project
                scope.Complete();
                return null;
            }
        }
        public virtual string TranslateTextsClassTranslate(string textCodeCode, int tenant, bool getLocalDefaultText)
        {
            return TranslateTextsClass.Translate(textCodeCode, tenant, getLocalDefaultText);
        }



        private IQueryable<CardGLAccountDataView> GetQAllVendorGLAccountCards(IAccountingContext accountingContext, int tenant)
        {
            var myGLAccountQueryService = new GLAccountQueryService(accountingContext);
            var myVendorGLAccountCardList = myGLAccountQueryService.GetQAllVendorGLAccountCardsHavingDeduction(tenant);
            return myVendorGLAccountCardList;
        }


        private FullAccountingSettingPM GetDeductionFileNumberFromAccSetting(IAccountingContext accountingContext, int tenant)
        {
            bool useLocal = true;
            string text;
            var myFullAccountingSettingQueryService = new FullAccountingSettingQueryService(accountingContext);
            var myFullAccountingSettingPM = myFullAccountingSettingQueryService.GetSingleFullAccountingSetting(tenant);
            if (myFullAccountingSettingPM == null)
            {
                throw new Exception("No FullAccountingSettingPM  for tenant ");
            }
            if (string.IsNullOrWhiteSpace(myFullAccountingSettingPM.DeductionFileNumber))
            {
                //   throw new Exception("No myFullAccountingSettingPM.DeductionFileNumber  for tenant ");
                text = TranslateTextsClassTranslate("System1000.O.DeductionFileNumber", 0, useLocal);
                // Deduction File Number is undefined.
                throw new Exception(text);
            }
            return myFullAccountingSettingPM;
        }




    }
}
