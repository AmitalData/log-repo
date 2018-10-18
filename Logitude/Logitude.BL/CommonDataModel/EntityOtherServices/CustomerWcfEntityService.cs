using System.Collections.Generic;
using System.Linq;
using System.Text;
using Logitude.CRM.BL.EntityPMs;
using Logitude.CRM.BL.EntityQueryServices;
using Logitude.CRM.Data;

namespace Logitude.BL.CommonDataModel.EntityOtherServices
{
    public class CustomerWcfEntityService
    {
        public string GetActivationQuestionnaireAnswers(string QuestionnaireId, int tenant, string tableId, string entityId)
        {
            StringBuilder HtmlTemplate = new StringBuilder();
            ICRMContext context = CRMContext.GetContext(tenant);
            QuestionnaireQueryService questionnaireQuery = new QuestionnaireQueryService(context);

            QuestionnairePM entityPM = questionnaireQuery.GetSingle(QuestionnaireId, false, false);

            QuestionnaireAnswerQueryService questionnaireAnswerQuery = new QuestionnaireAnswerQueryService(context);
            QuestionnaireAnswerPM QuestionnaireAnswerPM = questionnaireAnswerQuery.GetQuestionnaireAnswerPMByquestioneerIdAndversion(QuestionnaireId, tenant, tableId, entityId);
            
            if (QuestionnaireAnswerPM != null)
            {
                QuestionnaireQuestionQueryService questionsQuery = new QuestionnaireQuestionQueryService(context);
                List<QuestionnaireQuestionPM> QuestionnaireQuestions = questionsQuery.GetQuestionnaireQuestionByQuestioneerVersion(QuestionnaireId, QuestionnaireAnswerPM.VersionNumber, tenant);

                if (QuestionnaireAnswerPM.QuestionnaireAnswerLines != null)
                {
                    HtmlTemplate.Append("<!DOCTYPE html>");
                    HtmlTemplate.Append("<html  lang='ar'>");
                    HtmlTemplate.Append("<head>");
                    HtmlTemplate.Append("<title></title>");
                    HtmlTemplate.Append("<meta charset='utf-8'>");
                    HtmlTemplate.Append("</head>");
                    HtmlTemplate.Append("<body>");

                    if (!QuestionnaireAnswerPM.HasTwoColumn)
                    {
                        if (entityPM.RightToLeft)
                        {
                            HtmlTemplate.Append("<div  dir='rtl'  style ='margin-left:2%; margin-right:2%;'>");
                        }
                        else
                        {
                            HtmlTemplate.Append("<div style ='margin-left:2%; margin-right:2%;'>");
                        }

                        HtmlTemplate.Append("<div style ='width:100%'>");
                        foreach (QuestionnaireQuestionPM question in QuestionnaireQuestions)
                        {
                            if (question.QuestionTypeCode == "HL")
                            {
                                HtmlTemplate.Append("<p  style='font-weight:bold;height:auto;display:block;background-color:lightslategray;font-size:20px;'>" + question.Question + "</p>");
                            }
                            else
                            {
                                HtmlTemplate.Append("<p  style='font-weight:bold;height:24px; font-size:18px;'>" + question.Question + "</p>");
                            }

                            string anser = QuestionnaireAnswerPM.QuestionnaireAnswerLines.Where(d => d.QuestionNumber == question.QuestionNumber).Max(d => d.AnswerValue);
                            
                            if (question.QuestionTypeCode != "HL")
                            {
                                HtmlTemplate.Append("<p style='font-weight:normal;height:20px; font-size:16px;margin-top:-10px;'>" + anser + "</p>");
                            }
                        }

                        HtmlTemplate.Append("</div>");
                    }

                    else
                    {
                        if (entityPM.RightToLeft)
                        {
                            HtmlTemplate.Append("<div  dir='rtl'  style ='margin-left:2%; margin-right:2%;'>");
                        }
                        else
                        {
                            HtmlTemplate.Append("<div style ='margin-left:2%; margin-right:2%;'>");
                        }

                        HtmlTemplate.Append("<table  width='100%'>");
                        List<QuestionnaireQuestionPM> QuestionnaireQuestionsList1 = new List<QuestionnaireQuestionPM>(QuestionnaireQuestions);
                        List<QuestionnaireQuestionPM> QuestionnaireQuestionsList2 = new List<QuestionnaireQuestionPM>();
                        foreach (QuestionnaireQuestionPM question in QuestionnaireQuestions)
                        {
                            QuestionnaireQuestionPM questionPM = QuestionnaireQuestionsList2.Where(d => d.QuestionNumber == question.QuestionNumber).Max();

                            if (questionPM == null)
                            {
                                if (question.QuestionTypeCode == "HL")
                                {
                                    HtmlTemplate.Append("<tr  style='font-weight:bold;height:auto;display:block;background-color:lightslategray; font-size:20px;margin-bottom:15px;margin-top:5px;'>" + "<td>" + "<div>" + question.Question + "</div>" + "</td>" + "</tr>");
                                    QuestionnaireQuestionsList1.Remove(question);
                                    QuestionnaireQuestionsList2.Remove(question);
                                }
                                else
                                {
                                    HtmlTemplate.Append("<tr style ='display:block;width:auto;'>");

                                    for (int i = 0; QuestionnaireQuestionsList1.Count > i && QuestionnaireQuestionsList1[i].QuestionTypeCode != "HL" && i != 2; i++)
                                    {
                                        HtmlTemplate.Append("<td style ='display:inline-block;width:auto;'>");

                                        HtmlTemplate.Append("<div  style='font-weight:bold;height:auto;display:block;max-width:900px; font-size:18px;word-wrap: break-word'>" + QuestionnaireQuestionsList1[i].Question + "</div>");
                                        string anser = QuestionnaireAnswerPM.QuestionnaireAnswerLines.Where(d => d.QuestionNumber == QuestionnaireQuestionsList1[i].QuestionNumber).Max(d => d.AnswerValue);
                                        HtmlTemplate.Append("<div style='font-weight:normal;height:auto;display:block;max-width:900px;word-wrap: break-word; font-size:16px;margin-top:-1px;'>" + anser + "</div>");
                                        HtmlTemplate.Append("<div style='font-weight:normal;height:5px;'>" + "</div>");
                                        QuestionnaireQuestionsList2.Add(QuestionnaireQuestionsList1[i]);
                                        
                                        HtmlTemplate.Append("</td>");
                                        HtmlTemplate.Append("<td style ='display:inline-block;width:100px;'>");
                                        HtmlTemplate.Append("</td>");
                                    }

                                    foreach (QuestionnaireQuestionPM item in QuestionnaireQuestionsList2)
                                    {
                                        QuestionnaireQuestionsList1.Remove(item);
                                    }

                                    HtmlTemplate.Append("</tr>");
                                }
                            }
                            else
                            {
                                QuestionnaireQuestionsList2.Remove(questionPM);
                            }

                        }
                        HtmlTemplate.Append("</table>");
                    }

                    HtmlTemplate.Append("</div>");
                    HtmlTemplate.Append("</body>");
                    HtmlTemplate.Append("</html>");
                }
            }

            return HtmlTemplate.ToString();
        }
    }
}
