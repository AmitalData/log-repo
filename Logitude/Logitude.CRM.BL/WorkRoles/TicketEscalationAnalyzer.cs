using Logitude.CRM.BL.EntityPMs;
using Logitude.CRM.BL.EntityQueryServices;
using Logitude.CRM.Data;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.Data.Repsitories;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.QueueService;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CRM.BL.WorkRoles
{
    public class TicketEscalationAnalyzer
    {
        private ICRMContext context;
        TicketPM ticketPM { get; set; }
        int Tenant { get; set; }
        private SLAHeaderQueryService sLAHeaderQuery;
        private bool isNewEntity;

        ContactRepository contatcRep;
        EmployeeGroupQueryService employeeGroupQuery;
        TicketClassificationQueryService classificationQuery;
        //List<Contact> contactsList = new List<Contact>();
        SLALineRepository slaRep;
        BusinessHourRepository businessHourRep;
        TicketEscalationRepository escalationRepository;

        public TicketEscalationAnalyzer(TicketPM Ticket, bool isNewEntity)
        {
            this.ticketPM = Ticket;
            this.Tenant = Ticket.Tenant;
            this.isNewEntity = isNewEntity;
            this.context = CRMContext.GetContext(Tenant);
            this.sLAHeaderQuery = new SLAHeaderQueryService(context);
            this.employeeGroupQuery = new EmployeeGroupQueryService(context);
            this.classificationQuery = new TicketClassificationQueryService(context);
            this.slaRep = new SLALineRepository(context);
            this.escalationRepository = new TicketEscalationRepository(context);

            this.businessHourRep = new BusinessHourRepository(this.Tenant);
            this.contatcRep = new ContactRepository(this.Tenant);

            this.AnalyzeTicketEscalation();
        }

        private void AnalyzeTicketEscalation()
        {
            DateTime? myNearestDueDate = null;
            List<TicketEscalation> myTicketEscalations = new List<TicketEscalation>();

            myTicketEscalations = escalationRepository.GetTicketEscalations(ticketPM.Id, Tenant).ToList();
            SLAHeaderPM slaHeaderPM = sLAHeaderQuery.GetSinglePMByTenant(Tenant);

            if (slaHeaderPM != null)
            {
                List<SLALinePM> slaLines = slaHeaderPM.SLALines.Where(a => a.SeverityId == ticketPM.SeverityId && (a.FirstResponseEscalate == true || a.ResolveWithinEscalate == true)).ToList();
                List<SLAEscalationPM> slaEscalations = slaHeaderPM.SLAEscalations.ToList();

                //loop to delete
                foreach (TicketEscalation item in myTicketEscalations.Where(d => d.IsClose == false))
                {
                    if (!slaEscalations.Where(d => d.LineNumber == item.LineNumber && d.EscalationFor == item.EscalationFor).Any())
                    {
                        escalationRepository.Remove(item);
                    }

                    else
                    {
                        myNearestDueDate = CheckNearestDueDate(myNearestDueDate, item.DueDate);
                    }
                }

                escalationRepository.SubmitChanges();

                //if (slaEscalations != null && slaEscalations.Count() > 0)
                //{
                //    this.contactsList = contatcRep.GetActiveContacts(Tenant).ToList();
                //}

                foreach (SLAEscalationPM item in slaEscalations.OrderBy(o => o.LineNumber))
                {
                    List<string> myEmails = GetRecipients(item);
                    string recipients = string.Join(";", myEmails);

                    DateTime? dueDateTime = this.GetDueDate(item);
                    TicketEscalation entity = myTicketEscalations.Where(d => d.EscalationFor == item.EscalationFor && d.LineNumber == item.LineNumber).FirstOrDefault();

                    if (entity == null)
                    {
                        entity = new TicketEscalation()
                        {
                            Id = IdCounter.GetNumber("TicketEscalation", Tenant),
                            Tenant = Tenant,
                            TicketId = ticketPM.Id,
                            CreateDate = ticketPM.CreateDate,
                            UpdateDate = ticketPM.UpdateDate,
                            IsSLAViolated = false,
                            IsClose = false,
                            EscalationFor = item.EscalationFor,
                            LineNumber = item.LineNumber,
                            Recepients = recipients,
                            DueDate = dueDateTime,
                        };

                        string mySearchFields = "";

                        if (!string.IsNullOrEmpty(entity.EscalationFor))
                        {
                            string escalationForName = entity.EscalationFor == "FR" ? "First Response" : "Resolve Within";
                            MethodHelper.AddToSearchFields(ref mySearchFields, escalationForName);
                        }

                        if (!string.IsNullOrEmpty(entity.Recepients))
                        {
                            MethodHelper.AddToSearchFields(ref mySearchFields, entity.Recepients);
                        }

                        if (mySearchFields.Length > 1000)
                        {
                            mySearchFields = mySearchFields.Substring(0, 1000);
                        }

                        entity.SearchFields = mySearchFields;

                        escalationRepository.Add(entity);
                        myNearestDueDate = CheckNearestDueDate(myNearestDueDate, dueDateTime);
                    }

                    else
                    {
                        if (!entity.IsClose)
                        {
                            entity.UpdateDate = dueDateTime;
                            entity.DueDate = dueDateTime;
                            entity.Recepients = recipients;
                            escalationRepository.Update(entity);
                            myNearestDueDate = CheckNearestDueDate(myNearestDueDate, dueDateTime);
                        }
                    }
                }

                context.SaveChanges();
                this.Run(ticketPM, myNearestDueDate);
            }

        }

        private DateTime? CheckNearestDueDate(DateTime? myNearestDueDate, DateTime? itemDueDate)
        {
            if (myNearestDueDate == null)
            {
                myNearestDueDate = itemDueDate;
            }

            else if (itemDueDate < myNearestDueDate)
            {
                myNearestDueDate = itemDueDate;
            }

            return myNearestDueDate;
        }

        private DateTime? GetDueDate(SLAEscalationPM item)
        {
            DateTime? dueDate = null;
            SLALine slaLine = slaRep.GetSLALineBySeverityId(Tenant, ticketPM.SeverityId, ticketPM.SLAId);
            BusinessHour businessHour = new BusinessHour();

            if (slaLine != null)
            {
                businessHour = businessHourRep.GetSingleBusinessHours(slaLine.BusinessHoursId, slaLine.Tenant);
                BusinessHourCalcualtions businessCalculation = new BusinessHourCalcualtions(businessHour);

                switch (item.EscalationActionTimeIndicator)
                {
                    case "IM":

                        if (item.EscalationFor == "FR" && slaLine.FirstResponseEscalate)
                        {
                            dueDate = ticketPM.FirstResponseDue;
                        }
                        else if (item.EscalationFor == "RW" && slaLine.ResolveWithinEscalate)
                        {
                            dueDate = ticketPM.ResolveWithinDue;
                        }
                        break;

                    case "AF":
                        // Calender
                        if (businessHour.Is247)
                        {
                            if (item.EscalationFor == "FR" && slaLine.FirstResponseEscalate)
                            {
                                //myDueDate = myTicket.FirstResponseDue.Value.AddMinutes((double)item.EscalaitonTimeInMinutes);
                                bool isHolidayDay = businessCalculation.isHolidayDay(ticketPM.FirstResponseDue.Value);
                                if (isHolidayDay)
                                {
                                    dueDate = businessCalculation.NextDayAfterHoliday24Hour(ticketPM.FirstResponseDue.Value).AddMinutes((double)item.EscalaitonTimeInMinutes);
                                }

                                else
                                {
                                    dueDate = ticketPM.FirstResponseDue.Value.AddMinutes((double)item.EscalaitonTimeInMinutes);
                                }
                            }

                            else if (item.EscalationFor == "RW" && slaLine.ResolveWithinEscalate)
                            {
                                //myDueDate = myTicket.ResolveWithinDue.Value.AddMinutes((double)item.EscalaitonTimeInMinutes);
                                bool isHolidayDay = businessCalculation.isHolidayDay(ticketPM.ResolveWithinDue.Value);
                                if (isHolidayDay)
                                {
                                    dueDate = businessCalculation.NextDayAfterHoliday24Hour(ticketPM.ResolveWithinDue.Value).AddMinutes((double)item.EscalaitonTimeInMinutes);
                                }

                                else
                                {
                                    dueDate = ticketPM.ResolveWithinDue.Value.AddMinutes((double)item.EscalaitonTimeInMinutes);
                                }

                            }
                        }

                        // Business Hour 
                        else
                        {
                            if (item.EscalationFor == "FR" && slaLine.FirstResponseEscalate)
                            {
                                dueDate = businessCalculation.addResolveMinutes(ticketPM.FirstResponseDue.Value, (int)item.EscalaitonTimeInMinutes);
                            }

                            else if (item.EscalationFor == "RW" && slaLine.ResolveWithinEscalate)
                            {
                                dueDate = businessCalculation.addResolveMinutes(ticketPM.ResolveWithinDue.Value, (int)item.EscalaitonTimeInMinutes);
                            }
                        }

                        break;

                    case "BF":
                        // Calender
                        if (businessHour.Is247)
                        {
                            if (item.EscalationFor == "FR" && slaLine.FirstResponseEscalate)
                            {
                                //myDueDate = myTicket.FirstResponseDue.Value.AddMinutes((-1) * (double)item.EscalaitonTimeInMinutes);
                                bool isHolidayDay = businessCalculation.isHolidayDay(ticketPM.FirstResponseDue.Value);
                                if (isHolidayDay)
                                {
                                    dueDate = businessCalculation.NextDayAfterHoliday24Hour(ticketPM.FirstResponseDue.Value).AddMinutes((-1) * (double)item.EscalaitonTimeInMinutes);
                                }

                                else
                                {
                                    dueDate = ticketPM.FirstResponseDue.Value.AddMinutes((-1) * (double)item.EscalaitonTimeInMinutes);
                                }
                            }

                            else if (item.EscalationFor == "RW" && slaLine.ResolveWithinEscalate)
                            {
                                //myDueDate = myTicket.ResolveWithinDue.Value.AddMinutes((-1) * (double)item.EscalaitonTimeInMinutes);

                                bool isHolidayDay = businessCalculation.isHolidayDay(ticketPM.ResolveWithinDue.Value);
                                if (isHolidayDay)
                                {
                                    dueDate = businessCalculation.NextDayAfterHoliday24Hour(ticketPM.ResolveWithinDue.Value).AddMinutes((-1) * (double)item.EscalaitonTimeInMinutes);
                                }

                                else
                                {
                                    dueDate = ticketPM.ResolveWithinDue.Value.AddMinutes((-1) * (double)item.EscalaitonTimeInMinutes);
                                }
                            }
                        }

                        // Business Hour 
                        else
                        {
                            if (item.EscalationFor == "FR" && slaLine.FirstResponseEscalate)
                            {
                                dueDate = businessCalculation.addResolveMinutes(ticketPM.FirstResponseDue.Value, ((-1) * (int)item.EscalaitonTimeInMinutes));
                            }
                            else if (item.EscalationFor == "RW" && slaLine.ResolveWithinEscalate)
                            {
                                dueDate = businessCalculation.addResolveMinutes(ticketPM.ResolveWithinDue.Value, ((-1) * (int)item.EscalaitonTimeInMinutes));
                            }
                        }

                        break;
                }
            }

            return dueDate;
        }

        private List<string> GetRecipients(SLAEscalationPM item)
        {
            List<string> myEmails = new List<string>();
            List<string> groupUserIds = new List<string>();

            List<SLAEscalationRecepientPM> escalationUsers = item.SLAEscalationRecepients.Where(a => a.UserId != null).ToList();
            List<SLAEscalationRecepientPM> escalationPredefinitions = item.SLAEscalationRecepients.Where(a => a.PreDefinitionId != null).ToList();
            List<string> usersEmails = new List<string>();
            List<string> predifinitionEmails = new List<string>();
            List<string> groupEmails = new List<string>();
            string classificationManagerEmail = "", groupManagerEmail = "";

            EmployeeGroupPM employeeGroupPM = new EmployeeGroupPM();
            TicketClassificationPM classificationPM = new TicketClassificationPM();

            if (escalationUsers != null && escalationUsers.Count > 0)
            {
                //usersEmails = contactsList.Where(a => escalationUsers.Select(p => p.UserId).ToList().Contains(a.Id)).Select(a => a.Email).ToList();
                usersEmails = escalationUsers.Select(a => a.UserEmail).ToList();
            }

            if (escalationPredefinitions != null && escalationPredefinitions.Count > 0)
            {
                foreach (SLAEscalationRecepientPM recipient in escalationPredefinitions)
                {
                    if (recipient.PreDefinitionName.Trim() == "Owner")
                    {
                        //string ownerEmail = contactsList.Where(a => a.Id == ticketPM.OwnerId && a.Tenant == recipient.Tenant).Select(a => a.Email).FirstOrDefault();
                        string ownerEmail = ticketPM.OwnerEmail;
                        predifinitionEmails.Add(ownerEmail);
                    }

                    else if (recipient.PreDefinitionName.Trim() == "Classification Manager")
                    {
                        TicketClassificationPM classification = new TicketClassificationPM();
                        if (!string.IsNullOrEmpty(ticketPM.SecondaryClassificationId))
                        {
                            classification = classificationQuery.GetSingle(ticketPM.SecondaryClassificationId, true, false);
                        }
                        else
                        {
                            classification = classificationQuery.GetSingle(ticketPM.MainClassificationId, true, false);
                        }

                        //classificationManagerEmail = contactsList.Where(a => a.Id == classification.ManagerUserId && a.Tenant == classification.Tenant).Select(a => a.Email).FirstOrDefault();
                        classificationManagerEmail = classification.ManagerUserEmail;
                    }

                    else if (recipient.PreDefinitionName.Trim() == "Group Manager")
                    {
                        EmployeeGroupPM groupManager = employeeGroupQuery.GetSingle(ticketPM.EmployeeGroupId, true, false);
                        if (groupManager != null)
                        {
                            //groupManagerEmail = contactsList.Where(a => a.Id == groupManager.ManagerUserId && a.Tenant == groupManager.Tenant).Select(a => a.Email).FirstOrDefault();
                            groupManagerEmail = groupManager.ManagerUserEmail;
                        }
                    }

                    else if (recipient.PreDefinitionName.Trim() == "Group Notify")
                    {
                        employeeGroupPM = employeeGroupQuery.GetSingle(ticketPM.EmployeeGroupId, true, false);
                        if (employeeGroupPM != null)
                        {
                            groupEmails.Add(employeeGroupPM.EscalationNotify);
                        }
                    }

                    else if (recipient.PreDefinitionName.Trim() == "Classification Notify")
                    {
                        if (!string.IsNullOrEmpty(ticketPM.SecondaryClassificationId))
                        {
                            classificationPM = classificationQuery.GetSingle(ticketPM.SecondaryClassificationId, true, false);
                        }
                        else
                        {
                            classificationPM = classificationQuery.GetSingle(ticketPM.MainClassificationId, true, false);
                        }

                        groupEmails.Add(classificationPM.EscalationNotify);
                    }
                }
            }

            // Join All arrays in escalationRecipinetsEmails
            myEmails.AddRange(usersEmails);
            myEmails.AddRange(predifinitionEmails);
            myEmails.AddRange(groupEmails);
            if (!string.IsNullOrEmpty(classificationManagerEmail))
            {
                myEmails.Add(classificationManagerEmail);
            }
            if (!string.IsNullOrEmpty(groupManagerEmail))
            {
                myEmails.Add(groupManagerEmail);
            }
            return myEmails;
        }

        private void Run(TicketPM myTicket, DateTime? myNearestDueDate)
        {
            try
            {
                //IQueueService queueservice = QueueServiceManager.GetQueueService("ticketqueue", Tenant);
                IQueueService queueservice = new DbQueueService();
                queueservice.InitializeQueue("ticketqueue", 0);
                if (myNearestDueDate != null)
                {
                    DateTime myCreateDate = TenantServerConfigration.GetCurrentDateTime(Tenant);

                    TimeSpan myTimeSpan = new TimeSpan();
                    myTimeSpan = myNearestDueDate.Value - myCreateDate;
                    Dictionary<string, string> param = new Dictionary<string, string>() { { "Tenant", Tenant.ToString() }, { "TicketId", myTicket.Id.ToString() } };

                    queueservice.Send(param, Tenant, myTimeSpan);
                }
            }

            catch (Exception ex)
            {

            }
        }
    }
}
