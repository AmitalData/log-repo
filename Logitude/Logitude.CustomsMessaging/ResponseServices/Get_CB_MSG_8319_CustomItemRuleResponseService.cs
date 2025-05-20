using DocumentFormat.OpenXml.Office.CustomUI;
using DocumentFormat.OpenXml.Wordprocessing;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.Server.Tools.Helpers;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.VisualBasic.Logging;
using RtfPipe;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.ClassifGuidanceDetailsServiceReference;
using UnifreightIIG.Common.CustomItemClassifGuidanceServiceReference;
using UnifreightIIG.Common.CustomItemLegalDemandsServiceReference;
using UnifreightIIG.Common.CustomItemRuleServiceReference;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class Get_CB_MSG_8319_CustomItemRuleResponseService : ResponseServiceBase<CustomItemRuleResponseData, CB_NG_8319_CustomItemRuleOut, CustomItemRuleRequestParams>
    {
        public override void Update(CB_NG_8319_CustomItemRuleOut customResponse, CustomItemRuleRequestParams requestParams)
        {

            if (customResponse?.CIDetailsHeaderOut == null)
            {
                this.MyResponseData = new CustomItemRuleResponseData();
                this.MyResponseData.Succeeded = true;
                this.MyResponseData.HasException = false;
                this.MyResponseData.UserMessage = "לא התקבלו כללים";
                return;
            }
            if (customResponse?.CIRuleOut != null)
            {
                string sqlConnectionString = ConfigurationManager.ConnectionStrings["LogitudeStr"].ConnectionString;

                using (SqlConnection connection = new SqlConnection(sqlConnectionString))
                {
                    connection.Open();

                    foreach (CB_NG_8319_CustomItemRuleOutCIRuleOut item in customResponse.CIRuleOut)
                    {
                        // הכנה לפקודת SQL
                        string query = @"
                        INSERT INTO customs.TEMP_CB_RuleClassifications(CB_ID, ID, CustomsItemID, ParentID, CustomsBookType, [Index], Rules)
                        VALUES (@CB_ID, @ID, @CustomsItemID, @ParentID, @CustomsBookType, @Index, @Rules)";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            // הוספת הפרמטרים
                            command.Parameters.AddWithValue("@CB_ID", item.ID.ToString());
                            command.Parameters.AddWithValue("@ID", item.ID);
                            command.Parameters.AddWithValue("@CustomsItemID", requestParams.customsItemId);
                            command.Parameters.AddWithValue("@ParentID", (object)item?.ParetnID ?? DBNull.Value);
                            command.Parameters.AddWithValue("@CustomsBookType", requestParams.customsBookType.ToString());
                            command.Parameters.AddWithValue("@Index", item?.Index);

                            if (!string.IsNullOrEmpty(item?.RulesRtf))
                            {
                                if (IsValidRtf(item.RulesRtf))
                                {
                                    command.Parameters.AddWithValue("@Rules", ConvertRtfToHtml(item.RulesRtf));
                                }
                                else
                                {
                                    command.Parameters.AddWithValue("@Rules", item.RulesRtf);
                                }
                            }
                            else
                            {
                                command.Parameters.AddWithValue("@Rules", DBNull.Value);
                            }

                            // ביצוע הפקודה
                            command.ExecuteNonQuery();
                        }
                    }
                }
            }

            //if(customResponse?.CIRuleOut != null)
            //{

            //    ICustomContext context = CustomContext.GetContext(requestParams.Tenant);

            //    foreach (CB_NG_8319_CustomItemRuleOutCIRuleOut item in customResponse.CIRuleOut)
            //    {
            //        CB_RuleClassificationPM rule = new CB_RuleClassificationPM
            //        {
            //            ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
            //            CB_ID = item.ID.ToString(),
            //            ID = item.ID,
            //            CustomsItemID = requestParams.customsItemId,
            //            ParentID = item?.ParetnID,
            //            CustomsBookType = requestParams.customsBookType.ToString(),
            //            Index = item?.Index,
            //        };
            //        if (!string.IsNullOrEmpty(item?.RulesRtf))
            //        {
            //            if (IsValidRtf(item.RulesRtf))
            //            {
            //                rule.Rules = ConvertRtfToHtml(item.RulesRtf);
            //            }
            //            else
            //            {
            //                rule.Rules =  item.RulesRtf;
            //            }
            //        }
            //        CB_RuleClassificationUpdateService service = new CB_RuleClassificationUpdateService(context, new Dictionary<string, IContext>(), requestParams.Tenant);
            //        service.Update(rule , true);
            //    }


            //}
            this.MyResponseData = new CustomItemRuleResponseData();

            this.MyResponseData.Succeeded = true;
            this.MyResponseData.HasException = false;
            this.MyResponseData.UserMessage = "התקבלו כללים בהצלחה";

        }
        public static bool IsValidRtf(string rtf)
        {
            // לבדוק אם הטקסט מתחיל ב-{rtf1 ומסתיים ב-} 
            return rtf.Trim().StartsWith(@"{\rtf1") && rtf.Trim().EndsWith("}");
        }
        public string ConvertRtfToHtml(string rtf)
        {
            string html = Rtf.ToHtml(rtf);

            return html;
        }

        public override CustomItemRuleResponseData GetResponse(CB_NG_8319_CustomItemRuleOut customResponse, CustomItemRuleRequestParams requestParams)
        {
            return this.MyResponseData;
        }
    }
}
