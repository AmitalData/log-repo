using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Blob;
using Simplog.Server.Infrastructure.Azure;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Logitude.Server.Tools.Helpers;
using Microsoft.Practices.Unity;
using System.Collections.Specialized;
using SendGrid.SmtpApi;
using System.Diagnostics;

namespace CommunicationWorkerRole
{
    public class EmailingHelper
    {
        public static void SendEmail(EmailParameters parameters)
        {
            EmailProvider provider = GetEmailProvider(parameters.Tenant, parameters.Retries, parameters.IsProviderNumberSpecified, parameters.ProviderNumber);

            //   if (provider != null)
            //  {
            string strRegex = @"^([a-zA-Z0-9_\'\‘\-\.]+)@((\[[0-9]{1,3}" +
@"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
@".)+))([a-zA-Z]{2,6}|[0-9]{1,3})(\]?)$";

            SmtpClient myClient;
            MailMessage myMessage;
            int port = int.Parse(provider.Port);
            myClient = new SmtpClient(provider.Domain, port);//"retail.smtp.com" 2525 //  SendGid => "smtp.sendgrid.net", Convert.ToInt32(587)
            myClient.Credentials = new NetworkCredential(provider.UserName, provider.Password);//("ihab@simplogworld.com", "Saas256");

            myMessage = new MailMessage();
            Encoding encoding = Encoding.UTF8;
            myMessage.BodyEncoding = encoding;
            myMessage.IsBodyHtml = parameters.IsBodyHtml;
            myMessage.Body = parameters.Body;
            // myMessage.Subject = parameters.Subject != null ? parameters.Subject.Trim() : parameters.Subject;

            if (provider != null && provider.SupportsEmailDelivery)
            {
                myMessage.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                string createDate = parameters.CommunicationLogCreateDate != null ? parameters.CommunicationLogCreateDate.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"): "";
                var header = new Header();
                var uniqueArgs = new Dictionary<string, string> {
                  {"CommunicationLogId", parameters.CommunicationLogId},
                  {"Tenant", parameters.Tenant +""},
                  { "CommunicationLogCreateDate", createDate}
                };
                header.AddUniqueArgs(uniqueArgs);
                var xmstpapiJson = header.JsonString();
                myMessage.Headers.Add("X-SMTPAPI", xmstpapiJson);
            }

            string subject = parameters.Subject;
            if (!string.IsNullOrEmpty(subject))
            {
                subject = subject.Replace(System.Environment.NewLine, " ");
                subject = subject.Replace('\r', ' ').Replace('\n', ' ');
            }

            myMessage.Subject = subject;
            myMessage.BodyEncoding = System.Text.Encoding.UTF8;

            if (!string.IsNullOrWhiteSpace(provider.UserName) && parameters.SwitchFromWithUserNameIfValid && Regex.IsMatch(provider.UserName, strRegex))
            {
                myMessage.From = new MailAddress(provider.UserName);
            }
            else
            if (string.IsNullOrEmpty(parameters.SentByUser))
            {
                if (parameters.From == "no-reply@")
                {
                    parameters.From += "logitudeworld.com";
                }
                myMessage.From = new MailAddress(parameters.From);
            }
            else
            {
                myMessage.From = new MailAddress(parameters.From, parameters.SentByUser);
            }

            foreach (string replyMail in parameters.ReplyToList)
            {


                myMessage.ReplyToList.Add(new MailAddress(replyMail));
            }

            //myMessage.
            string[] emailList = new string[] { };

            if (parameters.To != null)
            {
                emailList = parameters.To.Split(';');
            }

            for (int i = 0; i < emailList.Length; i++)
            {
                if (!string.IsNullOrEmpty(emailList[i]))
                {
                    emailList[i] = emailList[i].Trim();
                    Regex re = new Regex(strRegex);
                    if (re.IsMatch(emailList[i]))
                    {
                        MailAddress mailaddress = new MailAddress(emailList[i]);
                        myMessage.To.Add(mailaddress);
                    }
                }
            }

            if (!string.IsNullOrEmpty(parameters.Cc))
            {
                string[] ccList = parameters.Cc.Split(';');
                for (int i = 0; i < ccList.Length; i++)
                {
                    if (!string.IsNullOrEmpty(ccList[i]))
                    {
                        Regex re = new Regex(strRegex);
                        if (re.IsMatch(ccList[i]))
                        {
                            MailAddress mailaddress = new MailAddress(ccList[i]);
                            myMessage.CC.Add(mailaddress);
                        }

                    }
                }

            }

            //Bcc
            if (!string.IsNullOrEmpty(parameters.Bcc))
            {
                string[] bccList = parameters.Bcc.Split(';');//to be fixed to Bcc when field is ready!
                for (int i = 0; i < bccList.Length; i++)
                {
                    if (!string.IsNullOrEmpty(bccList[i]))
                    {
                        Regex re = new Regex(strRegex);
                        if (re.IsMatch(bccList[i]))
                        {
                            MailAddress mailaddress = new MailAddress(bccList[i]);
                            myMessage.Bcc.Add(mailaddress);

                        }

                    }
                }

            }



            //=======================================================================================================================
            if (!string.IsNullOrEmpty(parameters.EmailView) && parameters.Body != null)
            {
                //AlternateView alternateview = AlternateView.CreateAlternateViewFromString(parameters.Body, null, parameters.EmailView);
                //myMessage.AlternateViews.Add(alternateview);
                if (parameters.IsBodyHtml)
                {
                    List<LinkedResource> resourceList = new List<LinkedResource>();

                    string[] htmlStringArray = parameters.Body.Split('<');

                    List<string> imagesList = htmlStringArray.Where(s => s.StartsWith("img")).ToList();

                    foreach (string imageString in imagesList)
                    {
                        if (imageString.IndexOf("cid:") != -1)
                        {
                            //int startIndex = imageString.IndexOf("cid:") + 4;
                            //int endIndex = imageString.IndexOf("/>") - startIndex;
                            //string imageName = imageString.Substring(startIndex, endIndex).Trim();
                            //if (imageName.Contains("'"))
                            //{
                            //    imageName = imageName.Replace("'", "");
                            //}


                            string imageName = getBetween(imageString, "cid:", "'");
                            string fileName = !string.IsNullOrEmpty(imageName) ? imageName.ToLower() : "";

                    
                            // Download file from Azure Storage
                        
                       
                            int tenant = (fileName == "logo0" || fileName == "smalllogo0" || fileName == "sharedlogtsitcslogo0" || fileName == "appmobilelogo" || fileName == "applestore" || fileName == "googleplay") ? 0 : parameters.Tenant;

                            if ((fileName == "logo0" || fileName == "smalllogo0" || fileName == "sharedlogtsitcslogo0"))
                            {
                                string newImageName = SystemLogoHelper.GetEnvironmentLogoName(imageName.Contains("small"));
                                parameters.Body = parameters.Body.Replace(imageName, newImageName);
                                imageName = newImageName;
                            }


                            #region Download Image
                            string extension = (!string.IsNullOrEmpty(imageName) && imageName.Split('.').Length > 1) ? imageName.Split('.')[1] : "jpg";
                            string originalImageName = !string.IsNullOrEmpty(imageName) ? imageName.Split('.')[0] : "";

                            if (!string.IsNullOrEmpty(fileName) && fileName.Contains("sharedlogtsitcslogo"))
                            {
                                extension = "png";
                                originalImageName = "sharedLogtsitcslogo" + tenant;
                            }


                            string image = originalImageName + "." + extension;

                            Logitude.Server.Tools.BlobFileInfo fileInfo = new Logitude.Server.Tools.BlobFileInfo()
                            {
                                FileName = originalImageName,
                                FolderName = GetFolderName(originalImageName.ToLower()),
                                Extension = extension,
                                Tenant = tenant,

                            };
                            Logitude.Server.Tools.StorageService.IBlobService storageservice = Logitude.Server.Tools.ContainerAccessor.Container.Resolve(typeof(Logitude.Server.Tools.StorageService.IBlobService), "StorageService", new ParameterOverride("", 1)) as Logitude.Server.Tools.StorageService.IBlobService;
                            byte[] datainByte = storageservice.Read(fileInfo);


                            #endregion


                            //CloudBlobContainer blobContainer = StorageAcountDetails.GetCurrentContainer(tenant);
                            //var blobfile = blobContainer.GetBlockBlobReference(StorageAcountDetails.GetBlobNameByLocation(image, "logos"));

                            //if (blobfile.Exists())
                            //{
                            //    MemoryStream memstream = new MemoryStream();

                            //    if (blobfile != null)
                            //    {
                            //blobfile.DownloadToStream(memstream);
                            if (datainByte != null)
                            {
                                MemoryStream memstream = new MemoryStream(datainByte);
                                memstream.Seek(0, SeekOrigin.Begin);

                                LinkedResource logo = new LinkedResource(memstream, "image/" + extension);
                                logo.ContentId = imageName;
                                logo.ContentType.Name = image;

                                resourceList.Add(logo);
                            }
                            //    }
                            //}
                        }


                    }

                    AlternateView alternateview = AlternateView.CreateAlternateViewFromString(parameters.Body, null, parameters.EmailView);
                    myMessage.AlternateViews.Add(alternateview);

                    foreach (LinkedResource logo in resourceList)
                    {
                        alternateview.LinkedResources.Add(logo);
                    }
                }
                else
                {
                    AlternateView alternateview = AlternateView.CreateAlternateViewFromString(parameters.Body, null, parameters.EmailView);
                    myMessage.AlternateViews.Add(alternateview);
                }
            }
            //=======================================================================================================================
            if (parameters.Attachments != null)
            {
                foreach (Attachment att in parameters.Attachments)
                {
                    myMessage.Attachments.Add(att);
                }
            }


            if (myMessage.To.Count != 0 || myMessage.CC.Count != 0 || myMessage.Bcc.Count != 0) // Add CC & Bcc -Maheera 
            {
                try
                {
                    myClient.Send(myMessage);
                }
                catch (Exception ee)
                {

                    Debug.WriteLine("Exception!!!!!!myClient.Send(myMessage):" + ee.ToString());
                    throw;
                }
                
            }


           // }
        }


        private static string GetFolderName(string fileName)
        {
            return (fileName.Contains("logo") || fileName == "applestore" || fileName == "googleplay") ? "logos" : "images";
        }

        private static string getBetween(string strSource, string strStart, string strEnd)
        {
            int Start, End;
            if (strSource.Contains(strStart) && strSource.Contains(strEnd))
            {
                Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                End = strSource.IndexOf(strEnd, Start);
                return strSource.Substring(Start, End - Start);
            }
            else
            {
                return "";
            }
        }

        public static EmailProvider GetEmailProvider(int tenant, int retries, bool isProviderNumberSpecified, int providerNumber)
        {
            EmailProviderRepository emailProviderRepository = new EmailProviderRepository(0);
            EmailProvider provider = null;
            if (!isProviderNumberSpecified)
            {
                List<EmailProvider> activeProviders = emailProviderRepository.GetActiveEmailProviders().ToList();

                if (retries < 3)
                {
                    if (tenant % 2 == 0)
                    {
                        if (activeProviders.Count > 1)
                        {
                            provider = activeProviders.Where(d => d.ProviderNumber == "2").FirstOrDefault();
                        }
                        else
                        {
                            provider = activeProviders.FirstOrDefault();
                        }
                    }
                    else
                    {
                        if (activeProviders.Count > 1)
                        {
                            provider = activeProviders.Where(d => d.ProviderNumber == "1").FirstOrDefault();
                        }
                        else
                        {
                            provider = activeProviders.FirstOrDefault();
                        }
                    }
                }
                else
                {
                    if (tenant % 2 != 0)
                    {
                        if (activeProviders.Count > 1)
                        {
                            provider = activeProviders.Where(d => d.ProviderNumber == "2").FirstOrDefault();
                        }
                        else
                        {
                            provider = activeProviders.FirstOrDefault();
                        }
                    }
                    else
                    {
                        if (activeProviders.Count > 1)
                        {
                            provider = activeProviders.Where(d => d.ProviderNumber == "1").FirstOrDefault();
                        }
                        else
                        {
                            provider = activeProviders.FirstOrDefault();
                        }
                    }
                }
            }
            else
            {
                provider = emailProviderRepository.GetSingleEmailProvider(providerNumber.ToString());
            }
            return provider;
        }

    }

    public class EmailParameters
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Cc { get; set; }
        public string Bcc { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsBodyHtml { get; set; }
        public List<Attachment> Attachments { get; set; }
        public string SentByUser { get; set; }
        public int Tenant { get; set; }
        public string EmailView { get; set; }
        public int Retries { get; set; }
        public int ProviderNumber { get; set; }
        public bool IsProviderNumberSpecified { get; set; }
        List<string> replyToList;

        public List<string> ReplyToList
        {
            get
            {
                if (replyToList == null)
                    replyToList = new List<string>();

                return replyToList;
            }
            set
            {
                replyToList = value;
            }
             
        }

        public string CommunicationLogId { get; set; }
        public DateTime? CommunicationLogCreateDate { get; set; }


        public bool SwitchFromWithUserNameIfValid { get; set; }
    }
}
