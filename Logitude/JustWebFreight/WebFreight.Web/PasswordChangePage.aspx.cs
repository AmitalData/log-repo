using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebFreight.Web.Security;
using WebFreight.Web.WebServices;

namespace WebFreight.Web
{
    public partial class PasswordChangePage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpContext context = HttpContext.Current;
            string url = context.Request.Url.ToString().Split('/')[2];

            bool enableHttps = true;
            
            if (LogitudeSettings.DeploymentStage != "Dev")
            {
                if (LogitudeSettings.WorkEnvironment == "logbox")
                {

                    if (url.Contains("system.dsv.co.il"))
                    {
                        enableHttps = false;
                    }
                }
            }

            if (enableHttps && LogitudeSettings.ForceHttps)
            {
                SecurityUtility.RedirectToHttps();
            }

            //Email = Request.QueryString["email"];
        }
        //string Email = "";
        //protected void btnSave_Click(object sender, EventArgs e)
        //{

       
        //    btnSave.Enabled = false;
        //    List<ValidationResult> errors = new List<ValidationResult>();

        //    //if (passwordStregthManager.ContainsUserName)
        //    //{
        //    //    errors.Add(new ValidationResult("password mustn't contain user name!"));
        //    //    FillErrors(errors);

        //    //    return;
        //    //}

        //    if (txtNewPassword.Text.Length < 8)
        //    {
        //        errors.Add(new ValidationResult("Passwords minimum length is 8 characters!"));
        //        //FillErrors(errors);

        //        return;
        //    }

        //    if (txtNewPassword.Text.Length > 16)
        //    {
        //        errors.Add(new ValidationResult("Passwords maximum length is 16 characters!"));
        //        //FillErrors(errors);

        //        return;
        //    }

        //    ChangePassword(errors);
        //}

        //private void ChangePassword(List<ValidationResult> errors)
        //{
        //    if (!String.IsNullOrEmpty(this.txtNewPassword.Text))
        //    {
        //        if (this.txtNewPassword.Text == this.txtRetypedPassword.Text)
        //        {

        //            CallChangePass();
        //            //if (this.PasswordStrength == "Weak" || this.PasswordStrength == "Very Weak")
        //            //{
        //            //    errors.Add(new ValidationResult("The password you entered is invalid"));
        //            //}
        //            //else
        //            //{
                        
        //            //}
        //        }
        //        else
        //        {
                   
        //            //txtVerifypassError.Visibility = Visibility.Visible;
        //            //return;
                   
        //        }
        //    }
        //    else
        //    {
        //        errors.Add(new ValidationResult("Password can't be empty!"));
                
        //    }

        //    //this.FillErrors(errors);

        //    if (errors.Count > 0)
        //    {
        //        btnSave.Enabled = true;
        //    }
        //}


        //PasswordCheckService passwordChkService = new PasswordCheckService();

        //void CallChangePass()
        //{
        //    bool succeeded = passwordChkService.ChangeUserPassword(Email, this.txtNewPassword.Text);
        //    if (succeeded)
        //    {
        //        Response.Redirect("login.aspx", true);
        //    }
        //    //passwordChkService.ChangeUserPasswordCompleted += (ss, ee) =>
        //    //{
        //    //    if (ee.Error == null)
        //    //    {
        //    //        if (ee.Result)
        //    //        {
        //    //            if (isResetRequest)
        //    //            {
        //    //                //CommonContext.PasswordResetRequests.Where(r=>r.RequestNumber == App.Current.Resources["ResetRequestNumber"].ToString()).FirstOrDefault().

        //    //                if (App.Current.Resources["ResetRequestNumber"] != null)
        //    //                {

        //    //                    preLoginContext.PasswordResetRequests.Where(r => r.RequestNumber == App.Current.Resources["ResetRequestNumber"].ToString()).FirstOrDefault().IsDone = true;
        //    //                    preLoginContext.SubmitChanges();

        //    //                    App.Current.Resources.Remove("ResetRequestNumber");
        //    //                }
        //    //            }

        //    //            //SessionLocator.ShowSimplogMessageWindow("Password change completed successfully!");
        //    //            strength.Visibility = Visibility.Collapsed;
        //    //            changed.Visibility = Visibility.Visible;
        //    //            OkButton.Visibility = Visibility.Collapsed;
        //    //            backbutton.Visibility = Visibility.Visible;
        //    //            txtNewPassword.IsHitTestVisible = false;
        //    //            txtConfirmedPassword.IsHitTestVisible = false;

        //    //            //viewInjectionService.ClearAllRegionViews("ApplicationRegion");
        //    //            //viewInjectionService.AddViewToRegion("LoginControl", "ApplicationRegion", typeof(LoginView));

        //    //            // regionManager.AddToRegion("ApplicationRegion", new PasswordChangeControl(Email));
        //    //        }
        //    //        else
        //    //        {
        //    //            //SessionLocator.ShowSimplogMessageWindow("Password change failed!");
        //    //            error.Visibility = Visibility.Visible;
        //    //            strength.Visibility = Visibility.Collapsed;
        //    //        }
        //    //    }
        //    //    else
        //    //    {
        //    //        SessionLocator.ShowOperationError(ee);
        //    //    }
        //    //};

          
          
        //}

      
    }
}