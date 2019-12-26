using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using WebFreight.Web.Helpers.TicketAnalyzer;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel;
using EAGetMail;

namespace WebFreight.Web.Helpers
{
    public class InboundEmailGeneralHelperMethods
    {
        HttpRequest request;

        public InboundEmailGeneralHelperMethods(HttpRequest request)
        {
            this.request = request;

        }

        public List<FileAttachment> FillAttachments()
        {
            List<FileAttachment> attachmentsFiles = new List<FileAttachment>();
            string fileName = "";
            for (int i = 0; i < request.Files.Count; i++)
            {
                if (request.Files[i].FileName.ToString().Contains("@"))
                {
                    fileName = request.Files[i].FileName.Split('@')[0];
                }
                else
                {
                    fileName = request.Files[i].FileName;
                }
                if (fileName != null && fileName.ToLower().Contains("winmail.dat"))
                {
                    Attachment[] tatts = null;
                    try
                    {
                        Mail oMail = new Mail("EG-C1508812802-00231-D7D3CB86FA99TU25-C22U9T5EED9826FF");
                        tatts = Mail.ParseTNEF(ReadFully(request.Files[i].InputStream), true);
                    }
                    catch (Exception ep)
                    {

                    }
                    int y = tatts.Length;
                    for (int x = 0; x < y; x++)
                    {
                        Attachment tatt = tatts[x];
                        if (tatt != null && tatt.Name != null && !tatt.Name.ToLower().Contains(".rtf"))
                        {
                            attachmentsFiles.Add(new FileAttachment()
                            {
                                ContentLength = tatt.Content.Length,
                                ContentType = tatt.ContentType,
                                FileName = tatt.Name,
                                InputStream = tatt.Content,
                            });
                        }
                    }
                }
                else
                {
                    attachmentsFiles.Add(new FileAttachment()
                    {
                        ContentLength = request.Files[i].ContentLength,
                        ContentType = request.Files[i].ContentType,
                        FileName = fileName,
                        InputStream = ReadFully(request.Files[i].InputStream),
                    });
                }
            }

            return attachmentsFiles;
        }

        public static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        public string getSubject(string subject)
        {
            string updatedSubject = "";
            int countCommas = subject.Count(a => a == ',');

            if (!string.IsNullOrEmpty(subject) && countCommas == 1)
            {
                updatedSubject = subject.Split(',')[0];
            }

            if (!string.IsNullOrEmpty(subject) && countCommas > 1)
            {
                int commmaIndex = countCommas / 2;
                int splitIndex = IndexOfNth(subject, ",", commmaIndex + 1);
                updatedSubject = subject.Substring(splitIndex + 1);
            }

            if (!string.IsNullOrEmpty(subject) && countCommas == 0)
            {
                updatedSubject = subject;
            }

            return updatedSubject;
        }

        public int IndexOfNth(string str, string value, int nth)
        {
            if (nth <= 0)
            {
                nth = 1;
            }

            int offset = str.IndexOf(value);
            for (int i = 1; i < nth; i++)
            {
                if (offset == -1)
                    return -1;

                offset = str.IndexOf(value, offset + 1);
            }
            return offset;
        }

        public string GetValue(string property)
        {
            try
            {
                string temp = request.Form[property];
                if (string.IsNullOrEmpty(temp) || temp == null)
                {
                    return "";
                }
                else
                {
                    return temp.ToString();
                }
            }
            catch (Exception e)
            {
                return "";
            }
        }

        public string Truncate(string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }

        public string TruncateCc(string value, int maxLength)
        {
            string lastText = value;
            if (string.IsNullOrEmpty(lastText))
                return lastText;

            if (lastText.Length <= maxLength)
            {
                lastText = value;
            }

            else
            {
                int idx = lastText.LastIndexOf(';');
                lastText = lastText.Substring(0, idx);
                while (idx > maxLength)
                {
                    idx = lastText.LastIndexOf(';');
                    lastText = lastText.Substring(0, idx);
                }
            }

            return lastText;
        }

        public TenantManagement GetTenantBySupportEmail(string supportEmail)
        {
            TenantManagementRepository tenantManagementRepository = new TenantManagementRepository();
            List<string> emails = GetListOfFilteredEmails(supportEmail);
            emails = this.GetSupportEmail(emails);
            TenantManagement myTenant = new TenantManagement();
            myTenant = tenantManagementRepository.GetSingleTenantManagementPMByListOfEmails(emails);

            return myTenant;
        }

        public List<string> GetSupportEmail(List<string> supportEmail)
        {
            List<string> emailList = new List<string>();
            string email = "";
            foreach (string item in supportEmail)
            {
                if (item != null && item.Split('@')[0].Contains("+"))
                {
                    email = item.Split('+')[0] + "@" + item.Split('@')[1];

                    if (email != null && email.Split('@')[0].Contains("-"))
                    {
                        email = email.Split('-')[0] + "@" + email.Split('@')[1];
                    }
                }

                else if (item != null && item.Split('@')[0].Contains("-"))
                {
                    email = item.Split('-')[0] + "@" + item.Split('@')[1];
                    if (email != null && email.Split('@')[0].Contains("+"))
                    {
                        email = email.Split('+')[0] + "@" + email.Split('@')[1];
                    }
                }

                else
                {
                    email = item;
                }

                emailList.Add(email);
            }

            return emailList;
        }

        public List<string> GetListOfFilteredEmails(string currentEmail)
        {
            string myEmail = Regex.Replace(currentEmail, @"\s+", "");
            List<string> emails = new List<string>();

            if (!string.IsNullOrEmpty(myEmail))
            {
                emails = currentEmail.Split(';').ToList<string>();
            }

            List<string> filteredEmail = new List<string>();
            foreach (string item in emails)
            {
                filteredEmail.Add(GetCorrectEmailFormat(item));
            }

            return filteredEmail;
        }

        public List<string> GetListOfFilteredEmails(List<string> emails)
        {
            List<string> filteredEmail = new List<string>();
            foreach (string item in emails)
            {
                filteredEmail.Add(GetCorrectEmailFormat(item));
            }

            return filteredEmail;
        }

        public string GetCorrectEmailFormat(string email)
        {
            string myEmail = "";

            if (email.Contains('<'))
            {
                myEmail = email.Split('<')[1].Split('>')[0];
            }

            else
            {
                myEmail = email;
            }

            return myEmail;
        }

        public User GetTenant_LogBox(string supportEmail)
        {
            ICommonDataContext context = CommonDataContext.GetContext(0);
            UserRepository userRepository = new UserRepository(context);
            List<string> emails = GetListOfFilteredEmails(supportEmail);
            List<string> DocumentFilingInbox = new List<string>();
            foreach (var item in emails)
            {
                DocumentFilingInbox.Add(item.Split('@')[0]);
            }
            User myUser = myUser = userRepository.GetSingleUserByDocumentFilingInbox(DocumentFilingInbox);
            return myUser;
        }
    }
}