using Logitude.BookingLib.BL.EntityPMs;
using Logitude.BookingLib.Data;
using Logitude.BookingLib.Data.EntityPOCOs;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BookingLib.BL.Helpers
{
    public class BookingEmailAlertsHelper
    {
        public void SendEmailAlert(BookingPM entityPM, int tenant, string alertCode)
        {
            ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
            IBookingContext crmContext = BookingContext.GetContext(tenant);

            ContactRepository contactRepository = new ContactRepository(commonContext);            
            EmailAlertSettingRepository emailAlertSettingRepository = new EmailAlertSettingRepository(tenant);
            EmailAlertSetting alertSettings = emailAlertSettingRepository.GetSingleEmailAlertSettingByCodeObjectTableName(alertCode, "Booking", tenant);

            if (alertSettings != null)
            {
                EmailAlersManager emailAlertManager = new EmailAlersManager();
                Contact toContact = contactRepository.GetSingleContact(entityPM.LastSentByUserId, tenant);

                string subject = "";
                List<string> toEmails = new List<string>();

                if (alertCode == "BOKC")
                {
                    subject = "Booking Confirmed for " + entityPM.LongMaster + " at " + entityPM.FirstFlight;
                    emailAlertManager.UseThanksText = true;
                    emailAlertManager.HtmlTemplate.Append("Your Booking request for AWB = " + entityPM.LongMaster + " , from " + entityPM.MainFromPortCode + " to " 
                                                            + entityPM.FinalDestinationPortCode
                                                            + ", Flight " + entityPM.FirstFlight + " on " 
                                                            + String.Format("{0:dd/MM/yyyy}", entityPM.MainCarriageETD) + " was confirmed ");
                }               

                if (toContact != null)
                {
                    toEmails.Add(toContact.Email);                    
                }

                if (entityPM.MainCarriageCarrierCode == "LH" && tenant == 925)
                {
                    toEmails.Add("perla@amital.co.il");
                }

                if (toEmails.Count > 0)
                {
                    emailAlertManager.SendEmailAlertForConfirmedBooking(tenant, toEmails, subject);
                }
            }
        }
    }
}
