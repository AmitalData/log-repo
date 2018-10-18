using Logitude.Server.Tools;
using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;
using WebFreight.Web.Security;
using Logitude.Server.Tools.Helpers;

namespace WebFreight.Web.WcfApi
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ContactPasswordService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select ContactPasswordService.svc or ContactPasswordService.svc.cs at the Solution Explorer and start debugging.
   [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ContactPasswordService : IContactPasswordService
    {

        public Response ChangeContactPassword(string email, string oldPassword, string newPassword)
        {
            Response response = new Response();
            try
            {

                IGlobalContext globalObjectContext = GlobalContext.GetContext();
                ContactPassword contactPassword = AuthenticationUtil.VerifyContactPassword(email, oldPassword, globalObjectContext);

         
                GlobalContact contact = globalObjectContext.GlobalContacts.Where(m => m.Email == email && m.InActive == false).FirstOrDefault();
                if (contactPassword != null && contact != null && contact.InActive == false)
                {
                    string hashedNewPassword = PasswordGenerator.GetBCryptHashedPassword(email, newPassword);
                    contactPassword.Password = hashedNewPassword;
                    contactPassword.IsLocked = false;
                    contactPassword.NumberOfRetries = 0;
                    contactPassword.MustChangePassword = false;
                    contactPassword.IsBCrypt = true;
                    globalObjectContext.SaveChanges();

                }
                else
                {
                    response.HasError = true;
                    response.ErrorMessage = "bad user email or password";
                }
            }
            catch (Exception ex)
            {
                response.HasError = true;
                response.ErrorMessage = ex.Message;
                response.InnerErrorMessage = (ex.InnerException != null ? (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : ex.InnerException.Message) : null);
                if (!string.IsNullOrEmpty(ex.StackTrace))
                {
                    response.ErrorMessage += Environment.NewLine + ex.StackTrace;
                }
            }
            return response;
        }
    }
}
