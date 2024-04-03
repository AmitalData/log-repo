using System; 
using System.Web;  
using Simplog.Data.CommonDataModel.Repositories; 
using WebFreight.Web.WebServices;  
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System.Transactions;
using Simplog.Global.Data.GlobalModel;
using Simplog.Server.Infrastructure.Helpers;
using System.Linq;
using WebFreight.Web.Helpers;
using Logitude.BL.CommonDataModel.EntityQueries;

namespace WebFreight.Web.WebPages
{
    public partial class TermsOfUseDownloadPage : System.Web.UI.Page
    {
        int? tenant = null;
        string documentId = null;
        //CheckIfDocumentExist

        public void Page_Load(object sender, EventArgs e)
        { 
            string token = Request["tempId"] ?? "";
            string privateLabeldId = Request["PrivateLableId"] ?? "";

            SecurityDocumentResult securityDocumentResult = SecurityDocumentHelper.ValidationDocumentToken(token);
            bool isValid = false; 
            tenant = securityDocumentResult.Tenant;
            documentId = CheckDocumentId(Request["DocumentId"]);

            isValid = IsValidUser(isValid, securityDocumentResult.Email); 

            if (isValid)
            {
               DownloadTermsOfUse(privateLabeldId); 
            }
            else
            {
               ShowExceptionMessage(securityDocumentResult.ExceptionResult);
            }
        }

        private string CheckDocumentId(string documentId)
        {
            if (string.IsNullOrEmpty(documentId) || tenant == null) return documentId;
            if (new TermsofUseQuery(0).CheckIfDocumentExist((int)tenant, documentId)) return documentId;
            return null;

        }

        private bool IsValidUser(bool isValid, string email)
        {
            if (IsUser(email, (int)tenant) && CheckAvailablityTenantsForEmail(email, (int)tenant) || tenant == 0) isValid = true;
            return isValid;
        }

        private void DownloadTermsOfUse(string privateLabeldId)
        {
            byte[] datainByte;

            if (!string.IsNullOrEmpty(privateLabeldId) && privateLabeldId != "null")
            {
                documentId = GetTermOfUseDocumentId(privateLabeldId);
            }

            Document termsOfUseDocument = GetDocument(documentId);

            if (termsOfUseDocument == null)
            {
                this.ShowExceptionMessage("Document Not Found!");
                return ;

            }
            datainByte = DownloadFileFromStorage(termsOfUseDocument);

            WriteFileToPageResponse(datainByte, termsOfUseDocument);
        }

        private static void WriteFileToPageResponse(byte[] datainByte, Document termsOfUseDocument)
        {
            if(datainByte == null)
            {
                return;
            }
 
           CreateResponse(datainByte, termsOfUseDocument);

           if (GetIsClientConnected())
           {
            CompleteResponse();
           }
          
        }

        private static byte[] DownloadFileFromStorage(Document termsOfUseDocument)
        {
            byte[] datainByte;
            Uploader up = new Uploader();
            datainByte = up.DownloadFile(termsOfUseDocument.Id, termsOfUseDocument.Extension, termsOfUseDocument.Folder, termsOfUseDocument.Tenant);
            return datainByte;
        }

        public bool CheckAvailablityTenantsForEmail(string email, int tenant)
        {
            ContactRepository contactRep = new ContactRepository(tenant);
            bool available = contactRep.CheckEmailAvailabilityForTenant(email, tenant);
            if (!available)
            {
                return contactRep.CheckEmailAvailabilityForTenant(email, 0);
            }
            return available;
        }


        private Document GetDocument(string documentId)
        {
              DocumentRepository documentRepository = new DocumentRepository((int)tenant);
              Document document = documentRepository.GetSingleDocument(documentId);
              return document;
        }

        private string GetTermOfUseDocumentId(string privateLabeldId)
        {
            TermsofUseQuery termsofUseQuery = new TermsofUseQuery(0);
            return termsofUseQuery.GetLatestTermsOfUseDocumentId(privateLabeldId); 
        }
         

        private static bool GetIsUser(string email, int tenant)
        { 
            IGlobalContext globalContext = GlobalContext.GetContext();
            return globalContext.GlobalContacts.Where(c => c.Email == email && (c.GlobalTenantId == tenant || c.GlobalTenantId == 0) && c.IsUser == true).Any();
       
        }

        private static bool IsUser(string email, int tenant)
        { 
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                return  GetIsUser(email, tenant);
            } 
        }


        private static void CreateResponse(byte[] datainByte, Document termsOfUseDocument)
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.AddHeader("Content-Length", datainByte.Length.ToString());
            HttpContext.Current.Response.ContentType = "application/pdf";
            HttpContext.Current.Response.BinaryWrite(datainByte);
        }

        private static void CompleteResponse()
        {
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.Close();
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        private static bool GetIsClientConnected()
        {
            return HttpContext.Current.Response.IsClientConnected;
        }

        private void ShowExceptionMessage(string exceptionMessage)
        {
            var message = exceptionMessage;
            if (string.IsNullOrEmpty(exceptionMessage)) message = "Sorry you’re not authenticated to view this backup.";
            Response.Output.Write(message);
        }

    }

}
 