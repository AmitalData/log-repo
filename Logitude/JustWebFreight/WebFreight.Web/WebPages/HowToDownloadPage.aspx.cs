using System;
using System.Web;

using WebFreight.Web.WebServices;

namespace WebFreight.Web.WebPages
{
    public partial class HowToDownloadPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string headerRequest = Request["id"];

            Uploader manager = new Uploader();
            byte[] data = null;
            string documentName="";
            switch (headerRequest)
            {
                case "1":
                    documentName = "define-signature.pdf";
                    data = manager.DownloadStaticFile(documentName, "how-to");
                    break;

                case "2":
                    documentName = "change-measurements.html";
                    data = manager.DownloadStaticFile(documentName, "how-to");
                    break;

                case "3":
                    documentName = "manage-awb-stock.html";
                    data = manager.DownloadStaticFile(documentName, "how-to");
                    break;

                case "4":
                    documentName = "cancel-invoice.html";
                    data = manager.DownloadStaticFile(documentName, "how-to");
                    break;

                case "5":
                    documentName = "shared-logistics.pdf";
                    data = manager.DownloadStaticFile(documentName, "how-to");
                    break;

                case "6":
                    documentName = "getting_around.pdf";
                    data = manager.DownloadStaticFile(documentName, "how-to");
                    break;

                case "7":
                    documentName = "eawb_tutorial.pdf";
                    data = manager.DownloadStaticFile(documentName, "how-to");
                    break;

                case "8":
                    documentName = "howto_issue_consolidatedinvoice.pdf";
                    data = manager.DownloadStaticFile(documentName, "how-to");
                    break;

                case "9":
                    documentName = "eawb_tutorial_french.pdf";
                    data = manager.DownloadStaticFile(documentName, "how-to");
                    break;

                case "10":
                    documentName = "howto_build_consolidationshipment.pdf";
                    data = manager.DownloadStaticFile(documentName, "how-to");
                    break;

                case "11":
                    documentName = "generic_invoice_interface.pdf";
                    data = manager.DownloadStaticFile(documentName, "how-to");
                    break;

                case "12":
                    documentName = "manage_users.pdf";
                    data = manager.DownloadStaticFile(documentName, "how-to");
                    break;

                case "13":
                    documentName = "advanced_features_workbook.pdf";
                    data = manager.DownloadStaticFile(documentName, "how-to");
                    break;

                case "14":
                    documentName = "eawb_quicktour.pdf";
                    data = manager.DownloadStaticFile(documentName, "how-to");
                    break;

                case "15":
                    documentName = "managing_mail_templates.pdf";
                    data = manager.DownloadStaticFile(documentName, "how-to");
                    break;

                case "16":
                    documentName = "currency_management.pdf";
                    data = manager.DownloadStaticFile(documentName, "how-to");
                    break;

                case "17":
                    documentName = "analyzing_crm_data.pdf";
                    data = manager.DownloadStaticFile(documentName, "how-to");
                    break;

                case "18":
                    documentName = "managing_opportunities.pdf";
                    data = manager.DownloadStaticFile(documentName, "how-to");
                    break;

                case "19":
                    documentName = "working_with_activities.pdf";
                    data = manager.DownloadStaticFile(documentName, "how-to");
                    break;

                case "20":
                    documentName = "eawb_tutorial_spanish.pdf";
                    data = manager.DownloadStaticFile(documentName, "how-to");
                    break;

                case "21":
                    documentName = "getting_around_spanish.pdf";
                    data = manager.DownloadStaticFile(documentName, "how-to");
                    break;

                case "22":
                    documentName = "managing_customers.pdf";
                    data = manager.DownloadStaticFile(documentName, "how-to");
                    break;

                case "23":
                    documentName = "outlook_connection.pdf";
                    data = manager.DownloadStaticFile(documentName, "how-to");
                    break;

                case "24":
                    documentName = "custom_roles.pdf";
                    data = manager.DownloadStaticFile(documentName, "how-to");
                    break;

                case "25":
                    documentName = "airline_account_number.pdf";
                    data = manager.DownloadStaticFile(documentName, "how-to");
                    break;

                case "26":
                    documentName = "managing_customers_hebrew.pdf";
                    data = manager.DownloadStaticFile(documentName, "how-to");
                    break;

                case "27":
                    documentName = "ebooking_tutorial.pdf";
                    data = manager.DownloadStaticFile(documentName, "how-to");
                    break;

                case "28":
                    documentName = "howto_change_password.pdf";
                    data = manager.DownloadStaticFile(documentName, "how-to");
                    break;

                case "29":
                    documentName = "subscribe_to_bluesnap.pdf";
                    data = manager.DownloadStaticFile(documentName, "how-to");
                    break;

                case "30":
                    documentName = "unifreight_mobile_user_guide_hebrew.pdf";
                    data = manager.DownloadStaticFile(documentName, "how-to");
                    break;

                case "31":
                    documentName = "unifreight_mobile_invitation_sharing_hebrew.pdf";
                    data = manager.DownloadStaticFile(documentName, "how-to");
                    break;

                case "32":
                    documentName = "outlook_connection_hebrew.pdf";
                    data = manager.DownloadStaticFile(documentName, "how-to");
                    break;

                case "33":
                    documentName = "analyzing_crm_data_hebrew.pdf";
                    data = manager.DownloadStaticFile(documentName, "how-to");
                    break;

                case "34":
                    documentName = "outlook_installation_hebrew.pdf";
                    data = manager.DownloadStaticFile(documentName, "how-to");
                    break;

                case "35":
                    documentName = "managing_opportunities_hebrew.pdf";
                    data = manager.DownloadStaticFile(documentName, "how-to");
                    break;

                case "36":
                    documentName = "unifreight_crm_r5_2015_hebrew.pdf";
                    data = manager.DownloadStaticFile(documentName, "how-to");
                    break;

                case "37":
                    documentName = "unifreight_crm_r1_2016_hebrew.pdf";
                    data = manager.DownloadStaticFile(documentName, "how-to");
                    break;

                case "38":
                    documentName = "unifreight_crm_r2_2016_hebrew.pdf";
                    data = manager.DownloadStaticFile(documentName, "how-to");
                    break;

                case "39":
                    documentName = "outlook_connection_setup_guide.pdf";
                    data = manager.DownloadStaticFile(documentName, "how-to");
                    break;

                case "40":
                    documentName = "logitude_mobile.pdf";
                    data = manager.DownloadStaticFile(documentName, "how-to");
                    break;

                case "41":
                    documentName = "shared_logistics_mobile_setup.pdf";
                    data = manager.DownloadStaticFile(documentName, "how-to");
                    break;

                case "42":
                    documentName = "analyzing_crm_data_french.pdf";
                    data = manager.DownloadStaticFile(documentName, "how-to");
                    break;

                case "43":
                    documentName = "managing_opportunities_french.pdf";
                    data = manager.DownloadStaticFile(documentName, "how-to");
                    break;

                case "44":
                    documentName = "working_with_activities_french.pdf";
                    data = manager.DownloadStaticFile(documentName, "how-to");
                    break;

                case "45":
                    documentName = "quotes_tutorial.pdf";
                    data = manager.DownloadStaticFile(documentName, "how-to");
                    break;

                case "46":
                    documentName = "manage_users_french.pdf";
                    data = manager.DownloadStaticFile(documentName, "how-to");
                    break;

                case "47":
                    documentName = "quick_books_connection.pdf";
                    data = manager.DownloadStaticFile(documentName, "how-to");
                    break;

                case "48":
                    documentName = "vat_type_management.pdf";
                    data = manager.DownloadStaticFile(documentName, "how-to");
                    break;

                case "49":
                    documentName = "build_consolidation_spanish.pdf";
                    data = manager.DownloadStaticFile(documentName, "how-to");
                    break;

                case "50":
                    documentName = "shared_logistics_mobile_spanish.pdf";
                    data = manager.DownloadStaticFile(documentName, "how-to");
                    break;

                case "51":
                    documentName = "follow_ups.pdf";
                    data = manager.DownloadStaticFile(documentName, "how-to");
                    break;

                case "52":
                    documentName = "shared_logistics_with_agents.pdf";
                    data = manager.DownloadStaticFile(documentName, "how-to");
                    break;

                case "53":
                    documentName = "sat_profact_connection.pdf";
                    data = manager.DownloadStaticFile(documentName, "how-to");
                    break;

                case "54":
                    documentName = "sat_profact_connection_spanish.pdf";
                    data = manager.DownloadStaticFile(documentName, "how-to");
                    break;

                case "55":
                    documentName = "crm_quick_tour.pdf";
                    data = manager.DownloadStaticFile(documentName, "how-to");
                    break;

                case "56":
                    documentName = "use_percent_freight_value.pdf";
                    data = manager.DownloadStaticFile(documentName, "how-to");
                    break;

                case "57":
                    documentName = "multiple_vat_management.pdf";
                    data = manager.DownloadStaticFile(documentName, "how-to");
                    break;

                case "58":
                    documentName = "print_rate_confirmation.pdf";
                    data = manager.DownloadStaticFile(documentName, "how-to");
                    break;

                case "59":
                    documentName = "containers_followup.pdf";
                    data = manager.DownloadStaticFile(documentName, "how-to");
                    break;

                case "60":
                    documentName = "build_master_packages.pdf";
                    data = manager.DownloadStaticFile(documentName, "how-to");
                    break;

                case "61":
                    documentName = "manage_customs_shipments.pdf";
                    data = manager.DownloadStaticFile(documentName, "how-to");
                    break;

                case "62":
                    documentName = "sat_profact_payment.pdf";
                    data = manager.DownloadStaticFile(documentName, "how-to");
                    break;

                case "63":
                    documentName = "sat_profact_payment_spanish.pdf";
                    data = manager.DownloadStaticFile(documentName, "how-to");
                    break;

                case "64":
                    documentName = "clear_logitude_cache.pdf";
                    data = manager.DownloadStaticFile(documentName, "how-to");
                    break;

                case "65":
                    documentName = "users_contacts_protection.pdf";
                    data = manager.DownloadStaticFile(documentName, "how-to");
                    break;

                case "66":
                    documentName = "fwb_fhl_recommendationsn.pdf";
                    data = manager.DownloadStaticFile(documentName, "how-to");
                    break;

                case "67":
                    documentName = "documents_filing_inbox.pdf";
                    data = manager.DownloadStaticFile(documentName, "how-to");
                    break;

                case "68":
                    documentName = "accounting_setting_guide.pdf";
                    data = manager.DownloadStaticFile(documentName, "how-to");
                    break;

                case "69":
                    documentName = "generic_payment_interface.pdf";
                    data = manager.DownloadStaticFile(documentName, "how-to");
                    break;

                case "REL001":
                    documentName = "december_2015_release.pdf";
                    data = manager.DownloadStaticFile(documentName, "how-to");
                    break;

                case "REL002":
                    documentName = "february_2016_release.pdf";
                    data = manager.DownloadStaticFile(documentName, "how-to");
                    break;

                case "REL003":
                    documentName = "may_2016_release.pdf";
                    data = manager.DownloadStaticFile(documentName, "how-to");
                    break;

                case "REL004":
                    documentName = "july_2016_release.pdf";
                    data = manager.DownloadStaticFile(documentName, "how-to");
                    break;

                case "REL005":
                    documentName = "october_2016_release.pdf";
                    data = manager.DownloadStaticFile(documentName, "how-to");
                    break;

                case "REL006":
                    documentName = "december_2016_release.pdf";
                    data = manager.DownloadStaticFile(documentName, "how-to");
                    break;

                case "REL007":
                    documentName = "feb_2017_release.pdf";
                    data = manager.DownloadStaticFile(documentName, "how-to");
                    break;

                case "REL008":
                    documentName = "may_2017_release.pdf";
                    data = manager.DownloadStaticFile(documentName, "how-to");
                    break;

                case "REL009":
                    documentName = "july_2017_release.pdf";
                    data = manager.DownloadStaticFile(documentName, "how-to");
                    break;

                case "REL010":
                    documentName = "october_2017_release.pdf";
                    data = manager.DownloadStaticFile(documentName, "how-to");
                    break;

                case "REL011":
                    documentName = "december_2017_release.pdf";
                    data = manager.DownloadStaticFile(documentName, "how-to");
                    break;

                case "REL012":
                    documentName = "february_2018_release.pdf";
                    data = manager.DownloadStaticFile(documentName, "how-to");
                    break;

                case "REL013":
                    documentName = "may_2018_release.pdf";
                    data = manager.DownloadStaticFile(documentName, "how-to");
                    break;

                case "REL014":
                    documentName = "july_2018_release.pdf";
                    data = manager.DownloadStaticFile(documentName, "how-to");
                    break;

                case "REL015":
                    documentName = "september_2018_release.pdf";
                    data = manager.DownloadStaticFile(documentName, "how-to");
                    break;

                case "REL016":
                    documentName = "december_2018_release.pdf";
                    data = manager.DownloadStaticFile(documentName, "how-to");
                    break;

                case "REL017":
                    documentName = "february_2019_release.pdf";
                    data = manager.DownloadStaticFile(documentName, "how-to");
                    break;
            }

            if (data != null)
            {
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.AddHeader("Content-Length", data.Length.ToString());

                HttpContext.Current.Response.AddHeader("Content-Disposition", "inline;filename=" + documentName);
                if (documentName.Split('.')[1].ToString() == "pdf")
                {
                    HttpContext.Current.Response.ContentType = "application/" + "pdf";
                }
                else
                {
                    HttpContext.Current.Response.ContentType = "application/" + "html";
                }

                HttpContext.Current.Response.BinaryWrite(data);

                if (HttpContext.Current.Response.IsClientConnected)
                {
                    HttpContext.Current.Response.Flush();
                    HttpContext.Current.Response.Close();
                    HttpContext.Current.ApplicationInstance.CompleteRequest();

                }
            }

            else
            {
                Response.Output.Write("Document is not available ! ");
                //throw new ApplicationException("Sorry your not authinticated to view this document.");
            }
        }

    }
}