using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Outlook = Microsoft.Office.Interop.Outlook;

using Microsoft.Office.Interop.Outlook;
using OutlookConnection.Common.Utils;
using OutlookConnection.Common.FeatureWcfServiceReference;
using OutlookConnection.Common.Repos;

namespace OutlookConnection.Common
{

    public static class AmitalSetting
    {

        public enum Edition : int
        {
            Internal = 0,
            Logitude = 1,
            UniFreight = 2,
            Debug = 3,
            InProduction = 4
        }

        public enum OutlookVersion : int
        {
            Outlook2007 = 12,
            Outlook2010 = 14,
            Outlook2013 = 15
        }

        public enum OutlookLanguage : int
        {
            English = 1033,
            Hebrew = 1037
        }

        public static Accounts accounts;
        public static string AccountId;
        public static bool SyncStop = false;
        public static string companyName = "";
        public static OutlookVersion outlookVersion;
        public static OutlookLanguage outlookLanguage;
        public static Edition edition;
        public static FeatureAccessInfo[] outlookConnectionFeatures;

        public const string OutlookConnectionVersion = "2.4.06";
        public static DateTime Time { get; set; }

       
        static AmitalSetting()
        {
            Time = DateTime.Now;
           
            //AccountId = "perla@amital.co.il";
            //AccountId = Globals.ThisAddIn.Application.ActiveExplorer().Session.CurrentUser.AddressEntry.Address;

            try
            {
                //accounts = Globals.ThisAddIn.Application.Session.Accounts;
                //AccountId = Globals.ThisAddIn.Application.ActiveExplorer().Session.CurrentUser.AddressEntry.GetExchangeUser().PrimarySmtpAddress;
            }
            catch (System.Exception e)
            {
                LogFileUtil.Log("AmitalSetting AmitalSetting() Failed: " + e.Message, LogFileUtil.LogLevel.Debug);

            }

        }

        public static void SetOutlookLanguage(int version)
        {
            try
            {
                switch (version)
                {

                    case 1033:
                        {
                            outlookLanguage = OutlookLanguage.English;
                        }
                        break;
                    case 1037:
                        {
                            outlookLanguage = OutlookLanguage.Hebrew;
                        }
                        break;
                    default:
                        {
                            outlookLanguage = OutlookLanguage.English;
                        }
                        break;


                }
                LogFileUtil.Log("Outlook Language: " + outlookLanguage.ToString(), LogFileUtil.LogLevel.Debug);

            }
            catch (System.Exception e)
            {
                LogFileUtil.Log("AmitalSetting SetOutlookLanguage() Failed: " + e.Message, LogFileUtil.LogLevel.Debug);
            }

        }

        public static void SetOfficeVersion(int version)
        {
            try
            {
                switch (version)
                {
                    case 12:
                        {
                            outlookVersion = OutlookVersion.Outlook2007;
                        }
                        break;
                    case 14:
                        {
                            outlookVersion = OutlookVersion.Outlook2010;
                        }
                        break;
                    case 15:
                        {
                            outlookVersion = OutlookVersion.Outlook2013;
                        }
                        break;
                }
                LogFileUtil.Log("Outlook version: " + outlookVersion.ToString(), LogFileUtil.LogLevel.Debug);
            }
            catch (System.Exception e)
            {
                LogFileUtil.Log("AmitalSetting SetOfficeVersion() Failed: " + e.Message, LogFileUtil.LogLevel.Debug);
            }


        }

        public static void SetUserFeatures(int tenent)
        { 

         try

            {
             List<FeatureAccessInfo> UserFeaturesList = new List<FeatureAccessInfo>();
             FeatureRepo myFeatureRepo = new FeatureRepo(); 
             Response myResponse = new Response();
             UserFeaturesList.Add(new FeatureAccessInfo() { FeatureCode = "OUTLOOKCONNETION", ObjectTableName = "Shipment" });
             UserFeaturesList.Add(new FeatureAccessInfo() { FeatureCode = "OUTLOOKCONNETION", ObjectTableName = "Customer" });
             UserFeaturesList.Add(new FeatureAccessInfo() { FeatureCode = "OUTLOOKCONNETION", ObjectTableName = "Opportunity" });
             UserFeaturesList.Add(new FeatureAccessInfo() { FeatureCode = "OUTLOOKCONNETION", ObjectTableName = "Quote" });
             UserFeaturesList.Add(new FeatureAccessInfo() { FeatureCode = "OUTLOOKCONNETION", ObjectTableName = "General" });
             FeatureAccessInfo[] myFeatureAccessInfoArray = myFeatureRepo.GetActiveFeaturesForUser(UserFeaturesList.ToArray(), tenent, ref myResponse);

              if (myFeatureAccessInfoArray != null)
              {
                  outlookConnectionFeatures = myFeatureAccessInfoArray;

                  foreach (FeatureAccessInfo item in myFeatureAccessInfoArray)
                  {
                      if (item.HasAccess)
                      {
                          LogFileUtil.Log("User have " + item.FeatureCode + " access to " + item.ObjectTableName.ToString() + " object ", LogFileUtil.LogLevel.Debug);
                      }
                      else
                      {
                          LogFileUtil.Log("User does not have " + item.FeatureCode + "access to " + item.ObjectTableName.ToString() + " object ", LogFileUtil.LogLevel.Debug);
                      }
                  }
              }
                 
        
               
               

            }
         catch (System.Exception e)
         {
             LogFileUtil.Log("AmitalSetting SetUserFeatures() Failed: " + e.Message, LogFileUtil.LogLevel.Debug);

         }
        }

        public static bool CheckOutlookVersion(string version)
        {
            FeatureRepo myFeatureRepo = new FeatureRepo();
            var temp = myFeatureRepo.CheckOutlookVersion(version);
            return temp;
        }




     
    
    }
}
