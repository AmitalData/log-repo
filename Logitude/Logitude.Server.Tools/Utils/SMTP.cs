using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mail;
using System.Windows.Forms;

namespace Logitude.Server.Tools.Utils
{
    public class SMTP
    {
        static bool ValidConfig = true;

        public delegate void SendItdelegate(string Body);
        public static void SendItDefault(string Body)
        {
            SendItDefault(Body, null);
        }
        public static void SendItDefault(string Body,string overSubject)
        {
            string To = "", Subject = "";
            try
            {
                if (!ValidConfig) return;
                Subject = "Error Alert from  Unifreight Application:" + Path.GetFileName(Application.ExecutablePath);
                if (!string.IsNullOrWhiteSpace(overSubject))
                {
                    Subject = overSubject;
                }
                try
                {
                    To = ConfigurationManager.AppSettings["SMTP.ErrorNotifyEMAIL"].ToString();
                }
                catch (Exception e)
                {
                    ValidConfig = false;
                    NetCommonHelper.Logger.DevLog.Instance.WriteFatal(e,"pls ErrorNotifyEMAIL add key to AppSettings:SMTP.ErrorNotifyEMAIL ");
                    return;
                }
                {
                    ValidConfig = false;
                    NetCommonHelper.Logger.DevLog.Instance.WriteInfo("pls ErrorNotifyEMAIL add key to AppSettings:SMTP.ErrorNotifyEMAIL ");
                    return;
                }
                SendIt(To, Subject, Body, "", "", "");
            }
            catch (Exception e)
            {
                NetCommonHelper.Logger.DevLog.Instance.WriteFatal(e, "Send Email To:" + To + Environment.NewLine + Body);

                ValidConfig = false;
            }
        }
        public static void SendEmailByConfig(
              string To, string Subject, string Body)
        {
            try
            {
                SendIt(To, Subject, Body,
                 "", "", "");
            }
            catch (Exception e)
            {


            }
        }
        public static void SendIt(
            string To, string Subject, string Body,
         string smtpserver, string sendusername, string sendpassword)
        {


            try
            {
                if (String.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["SMTP.ForceBother"]))
                {
                    NetCommonHelper.Logger.DevLog.Instance.WriteTrace("Sorry Due SMTP.ForceBother IsNullOrWhiteSpace suppress send email !!");
                    return;
                }
                if (!ValidConfig)
                    return;
                try
                {
                    if (smtpserver == "")
                    {
                        smtpserver = ConfigurationManager.AppSettings["SMTP.server"].ToString();
                    }
                    if (sendusername == "")
                    {
                        sendusername = ConfigurationManager.AppSettings["SMTP.sendusername"].ToString();
                    }
                    if (sendpassword == "")
                    {
                        sendpassword = ConfigurationManager.AppSettings["SMTP.sendpassword"].ToString();
                    }
                }
                catch(Exception e)
                {
                    ValidConfig = false;
                   NetCommonHelper.Logger.DevLog.Instance.WriteFatal(e,"not ValidConfig AppSettings not Init ");
                    return;

                }
                if (smtpserver == "" || sendusername == "" || sendpassword == "")
                {
                    ValidConfig = false;
                    NetCommonHelper.Logger.DevLog.Instance.WriteError("not ValidConfig one of Config is missing ");
                    return;
                }

                MailMessage mail = new MailMessage();
                mail.To = To;
                mail.From = sendusername; // "\"Unfreight Applications MachineName =" + Environment.MachineName + " UserDomainName=" + Environment.UserDomainName + "   \" <Error." + sendusername + "@" + smtpserver + ">";

                mail.Subject = Subject;
                mail.BodyFormat = MailFormat.Text;
                ///mail.BodyFormat = MailFormat.Html;
                mail.Body = Body + Environment.NewLine
                 + "MachineName =" + Environment.MachineName + " UserDomainName=" + Environment.UserDomainName + "   smtp User " + sendusername + "@" + smtpserver;
                mail.Headers.Add("X-Organization", "Error Alert from Application" + Path.GetFileName(Application.ExecutablePath));
                mail.Fields["http://schemas.microsoft.com/cdo/configuration/smtpserver"] = smtpserver;
                mail.Fields["http://schemas.microsoft.com/cdo/configuration/smtpserverport"] = 25;
                mail.Fields["http://schemas.microsoft.com/cdo/configuration/sendusing"] = 2;
                mail.Fields["http://schemas.microsoft.com/cdo/configuration/smtpauthenticate"] = 1;
                mail.Fields["http://schemas.microsoft.com/cdo/configuration/sendusername"] = sendusername;
                mail.Fields["http://schemas.microsoft.com/cdo/configuration/sendpassword"] = sendpassword;

                SmtpMail.SmtpServer = smtpserver;  //your real server goes here
                SmtpMail.Send(mail);
                NetCommonHelper.Logger.DevLog.Instance.WriteTrace("Send Email To:" + To + Environment.NewLine + Body);
            }
            catch (Exception e)
            {
                NetCommonHelper.Logger.DevLog.Instance.WriteFatal(e);
                ValidConfig = false;
            }

        }

    }
}
