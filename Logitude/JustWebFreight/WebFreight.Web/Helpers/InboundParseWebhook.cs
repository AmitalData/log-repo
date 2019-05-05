using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Logitude.CRM.BL.EntityPMs;
using Logitude.CRM.BL.EntityQueryServices;
using Logitude.CRM.BL.EntityUpdateServices;
using Logitude.CRM.Data;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.Data.Repsitories;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.SystemLogs;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Transactions;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Serialization;
using WebFreight.Web.Helpers.TicketAnalyzer;
using WebFreight.Web.Security;

namespace WebFreight.Web.Helpers
{
    public class InboundParseWebhook
    {
        public EmailUpload emailDetails { get; set; }
        public ICRMContext crmContext;
        public int Tenant { get; set; }

        TicketRepository ticketRep;
        ContactRepository contactRepository;
        CorrespondencesAttachmentRepository attachmentRep;
        UserRepository userRepository;
        InboundEmailRepository myHeaderRep;
        InboundEmail myHeader = null;
        TicketPM myTicket = null;
        Correspondence CorrespondenceLine = new Correspondence();
        string strippedBody = "";
        InboundEmailLine emailLine;
        IWebFreightContext webContext;
        bool IsFirstTicket = false;
        bool IsContactUser { get; set; }
        string AnalyzeQueueId = null;
        string supportEmail = "";
        InboundEmailGeneralHelperMethods helper;
        bool isInternalUser = false;

        public InboundParseWebhook(EmailUpload emailDetails, string AnalyzeQueueId)
        {
            this.emailDetails = emailDetails;
            this.AnalyzeQueueId = AnalyzeQueueId;
            FillInboundEmailTable();
        }

        private void FillInboundEmailTable()
        {
            try
            {
                helper = new InboundEmailGeneralHelperMethods(null);
                TenantManagement tenantManagement = helper.GetTenant(emailDetails.RecipientEmail);

                if (tenantManagement != null)
                {
                    supportEmail = tenantManagement.SupportEmail;
                    this.Tenant = tenantManagement.Id;
                }

                ObjectTableRepository objectTableRepository = new ObjectTableRepository(this.Tenant);
                ObjectTable objectTable = objectTableRepository.GetObjectTableByName("Ticket", 0, true);

                webContext = WebFreightContext.GetContext(Tenant);

                myHeaderRep = new InboundEmailRepository(webContext);
                InboundEmailLineRepository repository = new InboundEmailLineRepository(webContext);

                crmContext = CRMContext.GetContext(Tenant);

                ticketRep = new TicketRepository(crmContext);
                CorrespondenceRepository correspondenceRep = new CorrespondenceRepository(crmContext);
                attachmentRep = new CorrespondencesAttachmentRepository(crmContext);

                contactRepository = new ContactRepository(Tenant);
                userRepository = new UserRepository(Tenant);

                Contact contact = contactRepository.GetSingleContactByEmailSpecificTenant(emailDetails.Sender, Tenant);

                // Ticket Classification 
                TicketClassificationRepository ticketClassificationRep = new TicketClassificationRepository(crmContext);
                TicketClassification classification = ticketClassificationRep.GetTicketClassificationByName("General", Tenant);

                //Ticket Stage 
                TicketStageRepository stageRep = new TicketStageRepository(crmContext);
                TicketStage stage = stageRep.GetTicketStageByCode("OP", Tenant);

                using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                {
                    #region Contact
                    string contactId = null;
                    string companyId = null;

                   
                    if (contact == null )
                    {
                        UserQuery userQuery = new UserQuery(Tenant);
                        UserPM user = userQuery.GetSingleUserByEmailOrIdAndTenantOrTenantZero(null, emailDetails.Sender, Tenant);

                        if (user == null || (user != null && !user.IsCustomerCare))
                        {
                            contactId = IdCounter.GetNumber("Contact", Tenant).ToString();
                            string englishname = "";
                            if (!string.IsNullOrEmpty(emailDetails.Sender))
                            {
                                englishname = emailDetails.Sender.Split('@')[0].ToString();
                            }
                            // create new contact 
                            contact = new Contact()
                            {
                                Id = contactId,
                                Tenant = Tenant,
                                Email = helper.GetCorrectEmailFormat(emailDetails.Sender),
                                EnglishName = englishname,
                                UserType = "R",
                                ComputedKey = (!string.IsNullOrEmpty(emailDetails.Sender) ? emailDetails.Sender : contactId),
                                SearchFields = englishname + "," + emailDetails.Sender,
                            };
                            contactRepository.Add(contact);
                            contactRepository.SubmitChanges();
                        }
                        else
                        {
                            contact = contactRepository.GetSingleContactByEmailSpecificTenant(emailDetails.Sender, 0);
                            if (contact != null)
                            {
                                contactId = contact.Id;
                            }
                        }
                    }
                    else
                    {
                        contactId = contact.Id;
                        CardContactRepository myCardContactRepository = new CardContactRepository(Tenant);
                        CardRepository myCardRepository = new CardRepository(Tenant);

                        List<CardContact> listCardContact = myCardContactRepository.GetCardContactForContact(contactId, Tenant);

                        if (listCardContact != null)
                        {
                            if (listCardContact.Count() == 1)
                            {
                                companyId = listCardContact.FirstOrDefault().CardId;
                            }

                            else if (listCardContact.Count() > 1)
                            {
                                List<Card> listCard = myCardRepository.GetAllCardsByContactId(contactId, Tenant);

                                if (listCard.Count() == 1)
                                {
                                    companyId = listCard.FirstOrDefault().Id;
                                }
                            }
                        }
                    }

                    #endregion

                    isInternalUser = CheckedIfSenderInternaluser();
                    IsContactUser = CheckIfSenderIsUser(contactId);
                  
                    string supportEmailHeader = emailDetails.RecipientEmail.ToLower().Split('@')[0];

                    #region Ticket & Header
                    if (!supportEmailHeader.Contains('+'))
                    {
                        IsFirstTicket = true;
                        CreateNewTicketWithInboundEmail(contactId, companyId, objectTable.Id, classification, stage);
                    }

                    else
                    {
                        IsFirstTicket = false;
                        string headerid = GetInboundEmailId(emailDetails.RecipientEmail.ToLower());
                        myHeader = myHeaderRep.webFreightContext.InboundEmails.Where(d => d.Id == headerid).FirstOrDefault();
                        TicketQueryService myQuery = new TicketQueryService(Tenant);
                        myTicket = myQuery.GetSingle(myHeader.EntityId, true, false);
                        myTicket.ChangeSetOp = ChangeSetOperation.Update;
                        myTicket.IsCreatedFromOutSide = true;
                        strippedBody = StrippedBodyPlain(emailDetails.StrippedBodyPlain);
                        // owner reply from outside 
                        if ((contactId != null && contactId == myTicket.OwnerId) && !isInternalUser)
                        {
                            if (myTicket.FirstResponseTime == null)
                            {
                                myTicket.FirstResponseTime = TenantServerConfigration.GetCurrentDateTime(Tenant);
                            }
                        }
                    }

                    //creating a new ticket is allowed for users only
                    if (!IsFirstTicket)
                    {
                        #region Ticket Stages

                        if (myTicket.StageCode == "RE" || myTicket.StageCode == "WC")
                        {
                            TicketStage myStage = stageRep.GetTicketStageByCode("OP", Tenant);
                            myTicket.StageId = myStage.Id;
                            myTicket.StageCode = myStage.Code;
                            myTicket.StageName = myStage.Name;
                        }

                        #endregion

                        if (IsFirstTicket)
                        {
                            User myUser = userRepository.GetSingleUser(contact.Id, Tenant, false);
                            if (myUser != null)
                            {
                                myTicket.InternalUsers = this.AppendEmails(myTicket.InternalUsers, contact.Email);
                            }
                        }
                    }

                    #endregion

                    #region  Correspondence & Line
                    string correspondenceId = IdCounter.GetNumber("Correspondence", Tenant).ToString();

                    string myRecipientEmail = emailDetails.RecipientEmail;
                    if (emailDetails.RecipientEmail != null && emailDetails.RecipientEmail.Contains("-in"))
                    {
                        myRecipientEmail = emailDetails.RecipientEmail.Split(new string[] { "-in" }, StringSplitOptions.None)[0].ToString() + emailDetails.RecipientEmail.Split(new string[] { "-in" }, StringSplitOptions.None)[1].ToString();
                    }

                    if (emailDetails.RecipientEmail != null && emailDetails.RecipientEmail.Contains("-ex"))
                    {
                        myRecipientEmail = emailDetails.RecipientEmail.Split(new string[] { "-ex" }, StringSplitOptions.None)[0].ToString() + emailDetails.RecipientEmail.Split(new string[] { "-ex" }, StringSplitOptions.None)[1].ToString();
                    }

                    string mySender = emailDetails.Sender;
                    if (emailDetails.Sender != null && emailDetails.Sender.Contains("-in"))
                    {
                        mySender = emailDetails.Sender.Split(new string[] { "-in" }, StringSplitOptions.None)[0].ToString() + emailDetails.Sender.Split(new string[] { "-in" }, StringSplitOptions.None)[1].ToString();
                    }

                    if (emailDetails.Sender != null && emailDetails.Sender.Contains("-ex"))
                    {
                        mySender = emailDetails.Sender.Split(new string[] { "-ex" }, StringSplitOptions.None)[0].ToString() + emailDetails.Sender.Split(new string[] { "-ex" }, StringSplitOptions.None)[1].ToString();
                    }

                    // Inbound Line
                    emailLine = new InboundEmailLine()
                    {
                        Id = IdCounter.GetNumber("InboundEmailLine", Tenant).ToString(),
                        Tenant = this.Tenant,
                        Sender = mySender,
                        Recepient = myRecipientEmail,
                        Subject = emailDetails.Subject,
                        Body = strippedBody,
                        CreateDate = TenantServerConfigration.GetCurrentDateTime(Tenant),
                        Direction = "I",
                        InboundEmailId = myHeader.Id,
                        EntityLineId = correspondenceId, /// Not completely 
                        FullBody = emailDetails.FullBodyPlain,
                        HTMLFullBody = emailDetails.BodyHtml,
                    };

                    CheckLineCcInternalUsers(null, emailLine);

                    //if the internal user forward a message to the system, add him as the contact and to the notify internal 
                    if (IsFirstTicket && userRepository.DoesUserExist(emailDetails.Sender, Tenant))
                    {
                        emailLine.InternalUsers = this.AppendEmails(emailLine.InternalUsers, emailDetails.Sender);
                    }

                    repository.Add(emailLine);


                    // Correspondnce Line
                    CorrespondenceLine = new Correspondence()
                    {
                        Id = correspondenceId,
                        Tenant = Tenant,
                        EntityId = myTicket.Id,
                        CreateDate = TenantServerConfigration.GetCurrentDateTime(Tenant),
                        CreatedByContactId = contactId,
                        IsInternal = isInternalUser,
                        Description = strippedBody,
                        ObjectTableId = objectTable.Id,
                        NotifyMe = false,
                        NotifyOwner = true,
                        Direction = "I",
                        HTMLFullBody = emailDetails.BodyHtml,
                    };

                    CheckLineCcInternalUsers(CorrespondenceLine, null);

                    //if the internal user forward a message to the system, add him as the contact and to the notify internal 
                    if (IsFirstTicket && userRepository.DoesUserExist(emailDetails.Sender, Tenant))
                    {
                        CorrespondenceLine.IsInternal = false;
                        CorrespondenceLine.InternalUsers = this.AppendEmails(CorrespondenceLine.InternalUsers, emailDetails.Sender);
                    }

                    correspondenceRep.Add(CorrespondenceLine);

                    #region Attatchemnts

                    if (emailDetails.AttachmentsFiles.Count > 0)
                    {
                        AttatchmetnsProcessing();
                    }

                    #endregion

                    this.SendOtherCcInternalUsers();

                    TicketUpdateService service = new TicketUpdateService(crmContext, new Dictionary<string, IContext>(), Tenant);
                    service.Update(myTicket, true);
                    #endregion

                    webContext.SaveChanges();
                    crmContext.SaveChanges();

                    if (myTicket != null && CorrespondenceLine != null)
                    {
                        InboundEmailService inboundEmailService = new InboundEmailService(webContext);
                        inboundEmailService.SendEmail(emailLine, myTicket.TicketNumber, myTicket.ContactId, myTicket.OwnerId, CorrespondenceLine.CreatedByContactId, myTicket.GuidId, myTicket.Id, CorrespondenceLine.ObjectTableId, myHeader.ObjectTableId);
                    }

                    scope.Complete();
                }
            }

            catch (Exception errorInfo)
            {
                string errorMessage = errorInfo.Message;
                AzureLog.SaveLogsInStorage("Inbound Parse Webhook error  " + Environment.NewLine + errorMessage, "E", DateTime.Now, errorInfo.Message, errorInfo.StackTrace, 0, null, null, null);
                throw errorInfo;
            }
        }

        private bool CheckIfSenderIsUser(string contactId)
        {
            bool isUser = false;
            User myUser = userRepository.GetSingleUser(contactId, Tenant, false);
            if (myUser != null)
            {
                isUser = true;
            }

            return isUser;
        }

        private void SendOtherCcInternalUsers()
        {
            // Resend to other Cc's and internal users 
            if (!string.IsNullOrEmpty(emailDetails.CCs) || !string.IsNullOrEmpty(myTicket.CCs) || !string.IsNullOrEmpty(myTicket.InternalUsers) || CorrespondenceLine.NotifyOwner)
            {
                // To emails process
                if (!string.IsNullOrEmpty(emailDetails.To))
                {
                    if (helper == null)
                    {
                        helper = new InboundEmailGeneralHelperMethods(null);
                    }

                    List<string> toEmails = helper.GetListOfFilteredEmails(emailDetails.To);
                    toEmails = helper.GetSupportEmail(toEmails); // filtered data 

                    string supportEmailDomain =  supportEmail.Split('@')[1].Trim();
                    List<string> query = toEmails.Where(a => a != null && !a.Split('@')[1].Trim().Contains(supportEmailDomain)).ToList();
                    if (query.Count() > 0)
                    {
                        string emails = string.Join(";", query);

                        foreach (string item in query)
                        {
                            if (!string.IsNullOrEmpty(item))
                            {
                                string iEmail = helper.GetCorrectEmailFormat(item);

                                if (!string.IsNullOrEmpty(iEmail))
                                {
                                    iEmail = iEmail.ToLower();

                                    if (!this.IsSupportEmail(iEmail))
                                    {
                                        emailLine.CCs = this.AppendEmails(emailLine.CCs, iEmail);
                                    }
                                }
                            }
                        }

                        // Ayman: no need to check if is user ? so always add this to the CCS
                        //emailLine.CCs += emails;
                    }
                }

                this.GetCCEmailsList(emailLine, myTicket);

                List<string> TicketCcTemp = new List<string>();
                if (!string.IsNullOrEmpty(myTicket.CCs))
                {
                    TicketCcTemp = myTicket.CCs.Split(';').ToList<string>();
                }

                List<string> emailDetailsTemp = new List<string>();
                if (!string.IsNullOrEmpty(emailDetails.CCs))
                {
                    List<string> ccs = emailDetails.CCs.Split(';').ToList<string>();
                    foreach (string item in ccs)
                    {
                        emailDetailsTemp.Add(helper.GetCorrectEmailFormat(item));
                    }
                }

                List<string> queryCc = (from item in TicketCcTemp
                                        where !emailDetailsTemp.Contains(item)
                                        select item).ToList();

                List<string> TicketInternalTemp = new List<string>();
                if (!string.IsNullOrEmpty(myTicket.InternalUsers))
                {
                    TicketInternalTemp = myTicket.InternalUsers.Split(';').ToList<string>();
                }

                List<string> queryInternal = (from item in TicketInternalTemp
                                              where !emailDetailsTemp.Contains(item)
                                              select item).ToList();

                if (queryCc.Count() > 0)
                {
                    emailLine.Bcc = string.Join(";", queryCc);
                    emailLine.CCs = string.Join(";", queryCc);
                }
                if (queryInternal.Count() > 0)
                {
                    emailLine.InternalUsers = string.Join(";", queryInternal);
                }
            }
        }

        private void CheckLineCcInternalUsers(Correspondence correspondenceLine = null, InboundEmailLine inboundEmailLine = null)
        {
            if (helper == null)
            {
                helper = new InboundEmailGeneralHelperMethods(null);
            }

            if (!string.IsNullOrEmpty(emailDetails.CCs))
            {
                var myList = emailDetails.CCs.Split(';');
                string userEmails = null;
                string contactEmails = null;
                
                foreach (string item in myList)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        string iEmail = helper.GetCorrectEmailFormat(item);

                        if (!string.IsNullOrEmpty(iEmail))
                        {
                            iEmail = iEmail.ToLower();

                            if (!this.IsSupportEmail(iEmail))
                            {
                                if (userRepository.DoesUserExist(iEmail, Tenant))
                                {
                                    userEmails = this.AppendEmails(userEmails, iEmail);                                                                       
                                }

                                else
                                {
                                    contactEmails = this.AppendEmails(contactEmails, iEmail);                                   
                                }
                            }
                        }
                    }                    
                }

                if (correspondenceLine != null)
                {
                    correspondenceLine.CCs = this.AppendEmails(correspondenceLine.CCs, contactEmails);
                    correspondenceLine.InternalUsers = this.AppendEmails(correspondenceLine.InternalUsers, userEmails);                                       
                }

                if (inboundEmailLine != null)
                {
                    inboundEmailLine.CCs = this.AppendEmails(inboundEmailLine.CCs, contactEmails);
                    inboundEmailLine.InternalUsers = this.AppendEmails(inboundEmailLine.InternalUsers, userEmails);                    
                }
            }
        }



        private bool CheckedIfSenderInternaluser()
        {
            if (helper == null)
            {
                helper = new InboundEmailGeneralHelperMethods(null);
            }

            bool isInternal = false;
            UserRepository userRepository = new UserRepository(Tenant);

            if (!string.IsNullOrEmpty(emailDetails.Sender))
            {
                if (emailDetails.RecipientEmail.Contains("-in"))
                {

                    isInternal = true;
                }

                else if (emailDetails.RecipientEmail.Contains("-ex"))
                {

                    isInternal = false;
                }

                else
                {
                    isInternal = userRepository.DoesUserExist(helper.GetCorrectEmailFormat(emailDetails.Sender), Tenant);
                }
            }

            return isInternal;
        }

        private string StrippedBodyPlain(string body)
        {
            string strippedText = "";
            string strippedTextFinal = "";
            if (!string.IsNullOrEmpty(body))
            {
                body = Regex.Replace(body, @">+", "");
                if (body.Contains("#Please type your reply above this line#"))
                {
                    strippedText = body.Split(new string[] { "#Please type your reply above this line#" }, StringSplitOptions.None)[0];
                    List<string> newText = strippedText.TrimEnd().Split(new string[] { "\r\n", "\n" }, System.StringSplitOptions.None).ToList();

                    for (int i = newText.Count() - 1; i > 0; i--)
                    {
                        if (newText[i] == "")
                        {
                            break;
                        }
                        else
                        {
                            newText[i] = newText[i].Replace(newText[i], "");
                        }
                    }

                    strippedText = string.Join("\n", newText.ToArray());

                    strippedTextFinal = strippedText.TrimEnd();
                    //File.WriteAllText(@"C:\Log\strippedFinal.txt", strippedTextFinal);
                }

                else
                {
                    strippedTextFinal = TruncateLongString(body, 4000);
                }
            }

            return strippedTextFinal;
        }

        private void GetCCEmailsList(InboundEmailLine emailLine, TicketPM myTicket)
        {
            if (helper == null)
            {
                helper = new InboundEmailGeneralHelperMethods(null);
            }

            List<string> filteredInboundEmailLineEmails = new List<string>();
            List<string> filteredTicketEmails = new List<string>();
          

            // Fix Email 
            // convert eq: "email@gmail.com" <email@gmail.com> ==> email@gmail.com

            if (!string.IsNullOrEmpty(myTicket.CCs))
            {
                filteredTicketEmails = helper.GetListOfFilteredEmails(myTicket.CCs.ToString());
            }

            if (!string.IsNullOrEmpty(myTicket.InternalUsers))
            {
                if (filteredTicketEmails.Count() > 0)
                {
                    filteredTicketEmails.AddRange(helper.GetListOfFilteredEmails(myTicket.InternalUsers.ToString()));
                }
                else
                {
                    filteredTicketEmails = helper.GetListOfFilteredEmails(myTicket.InternalUsers.ToString());
                }
            }

            if (!string.IsNullOrEmpty(emailLine.CCs))
            {
                filteredInboundEmailLineEmails = helper.GetListOfFilteredEmails(emailLine.CCs.ToString());
            }

            if (!string.IsNullOrEmpty(emailLine.InternalUsers))
            {
                if (filteredInboundEmailLineEmails.Count() > 0)
                {
                    filteredInboundEmailLineEmails.AddRange(helper.GetListOfFilteredEmails(emailLine.InternalUsers.ToString()));
                }
                else
                {
                    filteredInboundEmailLineEmails = helper.GetListOfFilteredEmails(emailLine.InternalUsers.ToString());
                }
            }

            if (filteredInboundEmailLineEmails.Count() > 0)
            {
                List<string> query = filteredInboundEmailLineEmails.Where(item => filteredTicketEmails != null && !filteredTicketEmails.Contains(item)).ToList();
                UserRepository userRepository = new UserRepository(Tenant);

                if (query.Count() > 0)
                {
                    string userEmails = null;
                    string contactEmails = null;

                    foreach (string item in query)
                    {
                        if (!string.IsNullOrEmpty(item))
                        {
                            string iEmail = helper.GetCorrectEmailFormat(item);

                            if (!string.IsNullOrEmpty(iEmail))
                            {
                                iEmail = iEmail.ToLower();

                                if (!this.IsSupportEmail(iEmail))
                                {
                                    if (userRepository.DoesUserExist(iEmail, Tenant))
                                    {
                                        userEmails = this.AppendEmails(userEmails, iEmail);
                                    }

                                    else
                                    {
                                        contactEmails = this.AppendEmails(contactEmails, iEmail);                                       
                                    }
                                }
                            }
                        }
                    }

                    myTicket.CCs = this.AppendEmails(myTicket.CCs, contactEmails);
                    myTicket.InternalUsers = this.AppendEmails(myTicket.InternalUsers, userEmails);

                    crmContext.SaveChanges();
                }
            }
        }

        public string TruncateLongString(string str, int maxLength)
        {
            if (!string.IsNullOrEmpty(str))

                return str.Substring(0, Math.Min(str.Length, maxLength));

            else
                return str;
        }

        private string GetInboundEmailId(string recipient)
        {
            string unikey = null;
            InboundEmail header = null;

            if (!string.IsNullOrEmpty(recipient))
            {
                if (emailDetails.RecipientEmail.Contains("-in"))
                {
                    unikey = recipient.Split(new string[] { "-in" }, StringSplitOptions.None)[0].Split('+')[1].ToString();
                }

                else if (emailDetails.RecipientEmail.Contains("-ex"))
                {
                    unikey = recipient.Split(new string[] { "-ex" }, StringSplitOptions.None)[0].Split('+')[1].ToString();
                }

                else
                {
                    unikey = recipient.Split('@')[0].Split('+')[1].ToString();
                }

                Ticket ticket = ticketRep.GetTicketByGuidId(unikey, Tenant);
                header = myHeaderRep.webFreightContext.InboundEmails.Where(d => d.EntityId == ticket.Id).FirstOrDefault();
            }

            return header.Id;
        }

        private void AttatchmetnsProcessing()
        {
            ICommonDataContext commonContext = CommonDataContext.GetContext(Tenant);
            DocumentTypeRepository docRep = new DocumentTypeRepository(Tenant);
            DocumentType myDocType = docRep.GetSingleDocumentTypeByCode("CUA", Tenant);

            ContactRepository contactRepository = new ContactRepository(Tenant);
            Contact updatedByUser = contactRepository.GetContactByUserTypeAndTenant("S", Tenant);

            foreach (var item in emailDetails.AttachmentsFiles)
            {
                int len = (int)item.ContentLength;
                Byte[] mybytearray = new Byte[len];
                byte[] buffer = item.InputStream;
                Stream stream = new MemoryStream(buffer);
                stream.Read(mybytearray, 0, len);

                DocumentsFilingPM documentInPM = new DocumentsFilingPM()
                {
                    DirectionCode = "I",
                    Tenant = Tenant,
                    EntityId = myTicket.Id,
                    DocumentTypeId = myDocType.Id,
                    ObjectTableId = myDocType.ObjectTableId,
                    CreatedByUserId = updatedByUser.Id,
                    CreateDate = TenantServerConfigration.GetCurrentDateTime(Tenant),
                    OwnerId = updatedByUser.Id,
                    UpdatedByUserId = updatedByUser.Id,
                    UpdateDate = TenantServerConfigration.GetCurrentDateTime(Tenant),
                    //ExternalEntityName = documentDataPM.ExternalEntityName,
                    //ExternalEntityReference = documentDataPM.ExternalEntityReference,
                    //EntityReference = documentDataPM.EntityReference,
                    //FileExtension = item.ContentType.Contains('/') == false ? item.ContentType : item.ContentType.Split('/')[1],
                    FileSize = Convert.ToInt32(mybytearray.Length),
                    Folder = "docsin",
                    HasFile = true,
                    FileName = item.FileName.Contains('.') == false ? item.FileName : item.FileName.Split('.')[0],
                    FileExtension = item.FileName.Contains('.') == false ? "" : item.FileName.Split('.')[1],
                    //FileExtension = "" ,
                    FileData = mybytearray,
                };

                //File.WriteAllText(@"C:\Log\attachname.txt", item.FileName);
                //File.WriteAllText(@"C:\Log\attachextention.txt", item.ContentType);
                //File.WriteAllText(@"C:\Log\getextention.txt", item.GetType().ToString());

                DocumentsFilingService service = new DocumentsFilingService(commonContext, Tenant);
                service.Create(documentInPM, mybytearray, updatedByUser.Id);

                CorrespondencesAttachment attachment = new CorrespondencesAttachment()
                {
                    Id = IdCounter.GetNumber("CorrespondencesAttachment", Tenant).ToString(),
                    Tenant = Tenant,
                    DocumentFilingId = documentInPM.Id,
                    CorrespondenceId = CorrespondenceLine.Id,
                };

                attachmentRep.Add(attachment);
            }
        }

        private void CreateNewTicketWithInboundEmail(string contactId, string companyId, string objectTableId, TicketClassification classification, TicketStage stage)
        {
            if (helper == null)
            {
                helper = new InboundEmailGeneralHelperMethods(null);
            }

            string ticketId = IdCounter.GetNumber("Ticket", Tenant).ToString();
            string InboundEmailId = IdCounter.GetNumber("InboundEmail", Tenant).ToString();
            string ticketNo = CodeCounter.GetNumber("Ticket", this.Tenant).ToString();
            bool IsRejected = false;

            // Updated By User Id 
            Contact updatedByUser = contactRepository.GetContactByUserTypeAndTenant("S", Tenant);
            string updatedByUserId = updatedByUser.Id;

            // Default Owner
            EmployeeGroupLineRepository linesRep = new EmployeeGroupLineRepository(Tenant);
            EmployeeGroupLine defaultOwner = linesRep.GetEmployeeGroupLinesByGroupId(classification.EmployeeGroupId, Tenant);
            string myOwnerId = "";
            if (defaultOwner != null)
            {
                myOwnerId = defaultOwner.UserId;
            }

            //Ticket Source 
            TicketSourceRepository sourceRepository = new TicketSourceRepository(Tenant);
            TicketSource ticketSource = sourceRepository.GetSingle("MAL");

            // Ticket Creted By Type 
            string code = "CUS";
            if (isInternalUser)
            {
                code = "INU";
            }

            TicketCreatedByTypeRepository createdByTypeRepository = new TicketCreatedByTypeRepository(Tenant);
            TicketCreatedByType ticketCreatedByType = createdByTypeRepository.GetSingle(code);

            TenantRepository myTenantRepository = new TenantRepository(Tenant);
            Tenant myTenant = myTenantRepository.GetSingleTenant(Tenant);
           
            // Guid 
            var guid = Guid.NewGuid();
            var base64string = Convert.ToBase64String(guid.ToByteArray()).ToLower();
            base64string = base64string.Substring(0, 22);
            base64string = base64string.Replace("/", "_");
            base64string = base64string.Replace("+", "_"); // "-" must ask about that 
            base64string = base64string.Replace("-", "_");

            strippedBody = emailDetails.FullBodyPlain;
            myTicket = new TicketPM()
            {
                Id = ticketId,
                Tenant = this.Tenant,
                TicketNumber = ticketNo,
                CreateDate = TenantServerConfigration.GetCurrentDateTime(Tenant),
                UpdateDate = TenantServerConfigration.GetCurrentDateTime(Tenant),
                ContactId = contactId,
                CreatedByContactId = contactId,
                UpdatedByUserId = updatedByUserId,
                MainClassificationId = classification.Id,
                SeverityId = classification.DefaultSeverityId,
                EmployeeGroupId = classification.EmployeeGroupId,
                StageId = stage.Id,
                Subject = emailDetails.Subject,
                IsClosed = false,
                IsCancelled = false,
                SearchFields = ticketNo + "," + emailDetails.Subject,
                ChangeSetOp = ChangeSetOperation.Insert,
                IsCreatedFromOutSide = true,
                StageCode = stage.Code,
                StageName = stage.Name,
                TicketDescription = TruncateLongString(emailDetails.FullBodyPlain, 4000),
                CompanyId = companyId,
                GuidId = base64string,
            };

            if (IsContactUser && myTenant != null && myTenant.IsInternalTicketByDefault)
            {
                myTicket.InternalMode = true;
            }

            if (myOwnerId != null && !string.IsNullOrEmpty(myOwnerId))
            {
                myTicket.OwnerId = myOwnerId;
            }

            if (ticketSource != null)
            {
                myTicket.Source = ticketSource.Code;
                myTicket.SourceName = ticketSource.Name;
            }

            if (ticketCreatedByType != null)
            {
                myTicket.CreatedbyType = ticketCreatedByType.Code;
                myTicket.CreatedbyTypeName = ticketCreatedByType.Name;
            }

            List<string> myEmails = emailDetails.CCs.Split(';').ToList<string>();
            foreach (var item in myEmails)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    string iEmail = helper.GetCorrectEmailFormat(item);

                    if (!string.IsNullOrEmpty(iEmail))
                    {
                        iEmail = iEmail.ToLower();

                        if (!this.IsSupportEmail(iEmail))
                        {
                            if (userRepository.DoesUserExist(iEmail, Tenant))
                            {
                                myTicket.InternalUsers = this.AppendEmails(myTicket.InternalUsers, iEmail);                                
                            }

                            else
                            {
                                myTicket.CCs = this.AppendEmails(myTicket.CCs, iEmail);
                            }
                        }
                    }
                }
            }

            myHeader = new InboundEmail()
            {
                Id = InboundEmailId,
                Tenant = this.Tenant,
                EntityId = ticketId,
                ObjectTableId = objectTableId,
                CreateDate = TenantServerConfigration.GetCurrentDateTime(Tenant),
                UpdateDate = TenantServerConfigration.GetCurrentDateTime(Tenant),
                Uniquekey = InboundEmailId,
                IsRejected = IsRejected,
                AnalyzeQueueId = AnalyzeQueueId,
            };

            myHeaderRep.Add(myHeader);
        }

        private string AppendEmails(string toField, string emails)
        {
            string myResult = null;

            if (!string.IsNullOrEmpty(toField))
            {
                myResult = toField;
            }

            if (!string.IsNullOrEmpty(emails))
            {
                if (string.IsNullOrEmpty(myResult))
                {
                    myResult = emails;
                }

                else
                {
                    myResult += ";" + emails;
                }
            }

            return myResult;
        }
        private bool IsSupportEmail(string email)
        {
            bool myResult = false;

            if (!string.IsNullOrEmpty(email))
            {
                email = email.ToLower();

                switch (email)
                {
                    case "s@test.unifreight.co.il":
                    case "support@ilcargo.com":
                    case "support@icl.unifreight.co.il":
                        {
                            myResult = true;
                            break;
                        }
                }
            }

            return myResult;
        }
    }
}