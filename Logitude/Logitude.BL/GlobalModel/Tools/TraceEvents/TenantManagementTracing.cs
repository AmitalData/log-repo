using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.GlobalModel.EntityPMs;
using Logitude.BL.Security;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Global.Data.GlobalModel;

namespace Logitude.BL.GlobalModel.Tools.TraceEvents
{
    public class TenantManagementTracing
    {
        public static void Trace(TenantManagementPM entityPM, TenantManagement poco, bool isNewEntity)
        {
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                Contact loggedContact = null;
                ContactRepository contactRep = null;
                
                if (isNewEntity)
                {
                    contactRep = new ContactRepository(poco.Id);
                    loggedContact = contactRep.GetSingleContactByEmail("system@tenant" + poco.Id + ".com", poco.Id);

                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = 0,
                        EventTypeCode = "CRMG",
                        UserId = loggedContact.Id,
                        EntityId = poco.Id.ToString(),
                        ObjectTableName = "TenantManagement",
                    });
                }

                else
                {
                    contactRep = new ContactRepository(0);
                    loggedContact = contactRep.GetSingleContactByEmail(SecurityUtility.GetAuthenticatedUser(), 0);
                    if (loggedContact == null)
                    {
                        loggedContact = contactRep.GetSingleContactByEmail("system@tenant0.com", 0);
                    }

                    string notes = null;
                    using (TransactionScope updateScope = TransactionFactory.GetNewTransaction())
                    {    
                        if (entityPM.IsRecurring != poco.IsRecurring)
                        {
                            notes = "Is recurring was updated";
                        }

                        if (entityPM.PackageCode != poco.PackageCode)
                        {
                            PackageRepository repo = new PackageRepository(0);
                            Package entity_Pm = repo.GetSinglePackage(entityPM.PackageCode);

                            if (string.IsNullOrEmpty(notes))
                            {
                                notes = entity_Pm == null ? "Package changed" : "Package changed to " + entity_Pm.Name;
                            }
                            else
                            {
                                notes = notes + Environment.NewLine + (entity_Pm == null ? "Package changed" : "Package changed to " + entity_Pm.Name);
                            }
                        }

                        if (entityPM.RecurringPeriodCode != poco.RecurringPeriodCode)
                        {
                            RecurringPeriodRepository repo = new RecurringPeriodRepository(0);
                            RecurringPeriod entity_Pm = repo.GetSingleRecurringPeriod(entityPM.RecurringPeriodCode);

                            if (string.IsNullOrEmpty(notes))
                            {
                                notes = entity_Pm == null ? "Recurring period changed" : "Recurring period changed to " + entity_Pm.Name;
                            }

                            else
                            {
                                notes = notes + Environment.NewLine + (entity_Pm == null ? "Recurring period changed" : "Recurring period changed to " + entity_Pm.Name);
                            }
                        }

                        if (entityPM.FirstPaymentDate != poco.FirstPaymentDate)
                        {
                            if (string.IsNullOrEmpty(notes))
                            {
                                notes = "First payment date changed from " + poco.FirstPaymentDate + " to " + entityPM.FirstPaymentDate;
                            }

                            else
                            {
                                notes = notes + Environment.NewLine + "First payment date changed from " + poco.FirstPaymentDate + " to " + entityPM.FirstPaymentDate;
                            }
                        }

                        if (entityPM.PaidUntilDate != poco.PaidUntilDate)
                        {
                            if (string.IsNullOrEmpty(notes))
                            {
                                notes = "Paid until date changed from " + poco.PaidUntilDate + " to " + entityPM.PaidUntilDate;
                            }

                            else
                            {
                                notes = notes + Environment.NewLine + "Paid until date changed from " + poco.PaidUntilDate + " to " + entityPM.PaidUntilDate;
                            }
                        }

                        if (entityPM.PaymentCurrencyCode != poco.PaymentCurrencyCode)
                        {
                            PaymentCurrencyRepository repo = new PaymentCurrencyRepository(0);
                            PaymentCurrency entity_Pm = repo.GetSinglePaymentCurrency(entityPM.PaymentCurrencyCode);

                            if (string.IsNullOrEmpty(notes))
                            {
                                notes = entity_Pm == null ? "Payment currency changed" : "Payment currency changed to " + entity_Pm.Name;
                            }

                            else
                            {
                                notes = notes + Environment.NewLine + (entity_Pm == null ? "Payment currency changed" : "Payment currency changed to " + entity_Pm.Name);
                            }
                        }

                        if (entityPM.PaymentChannelCode != poco.PaymentChannelCode)
                        {
                            PaymentChannelRepository repo = new PaymentChannelRepository(0);
                            PaymentChannel entity_Pm = repo.GetSinglePaymentChannel(entityPM.PaymentChannelCode);

                            if (string.IsNullOrEmpty(notes))
                            {
                                notes = entity_Pm == null ? "Payment channel changed" : "Payment channel changed to " + entity_Pm.Name;
                            }

                            else
                            {
                                notes = notes + Environment.NewLine + (entity_Pm == null ? "Payment channel changed" : "Payment channel changed to " + entity_Pm.Name);
                            }
                        }

                        if (entityPM.PaymentMethodCode != poco.PaymentMethodCode)
                        {
                            PaymentMethodRepository repo = new PaymentMethodRepository(0);
                            PaymentMethod entity_Pm = repo.GetSinglePaymentMethod(entityPM.PaymentMethodCode);

                            if (string.IsNullOrEmpty(notes))
                            {
                                notes = entity_Pm == null ? "Payment method changed" : "Payment method changed to " + entity_Pm.Name;
                            }

                            else
                            {
                                notes = notes + Environment.NewLine + (entity_Pm == null ? "Payment method changed" : "Payment method changed to " + entity_Pm.Name);
                            }
                        }

                        if (entityPM.BluesnapAccount != poco.BluesnapAccount)
                        {
                            if (string.IsNullOrEmpty(notes))
                            {
                                notes = "Bluesnap account was updated";
                            }

                            else
                            {
                                notes = notes + Environment.NewLine + "Bluesnap account was updated";
                            }
                        }

                        if (entityPM.MainContract != poco.MainContract)
                        {
                            if (string.IsNullOrEmpty(notes))
                            {
                                notes = "Main contract was updated";
                            }

                            else
                            {
                                notes = notes + Environment.NewLine + "Main contract was updated";
                            }
                        }

                        if (entityPM.LicensePrice != poco.LicensePrice)
                        {
                            if (string.IsNullOrEmpty(notes))
                            {
                                notes = "License price was updated";
                            }

                            else
                            {
                                notes = notes + Environment.NewLine + "License price was updated";
                            }
                        }

                        if (entityPM.BluesnapContractId != poco.BluesnapContractId)
                        {
                            IGlobalContext context = GlobalContext.GetContext(0);
                            BluesnapContractRepository repo = new BluesnapContractRepository(context);
                            BluesnapContract entity_Pm = repo.GetSingleBluesnapContract(entityPM.BluesnapContractId, 0);

                            if (string.IsNullOrEmpty(notes))
                            {
                                notes = entity_Pm == null ? "Bluesnap contract changed" : "Bluesnap contract changed to " + entity_Pm.Name;
                            }

                            else
                            {
                                notes = notes + Environment.NewLine + (entity_Pm == null ? "Bluesnap contract changed" : "Bluesnap contract changed to " + entity_Pm.Name);
                            }
                        }

                        if (entityPM.BillingByLogitude != poco.BillingByLogitude)
                        {
                            if (string.IsNullOrEmpty(notes))
                            {
                                notes = "Billing by Logitude was updated";
                            }

                            else
                            {
                                notes = notes + Environment.NewLine + "Billing by Logitude was updated";
                            }
                        }

                        if (entityPM.ResellerCommission != poco.ResellerCommission)
                        {
                            if (string.IsNullOrEmpty(notes))
                            {
                                notes = "Reseller commission was updated";
                            }

                            else
                            {
                                notes = notes + Environment.NewLine + "Reseller commission was updated";
                            }
                        }

                        updateScope.Complete();
                    }

                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = 0,
                        EventTypeCode = "UPMG",
                        UserId = loggedContact.Id,
                        EntityId = poco.Id.ToString(),
                        ObjectTableName = "TenantManagement",
                        Notes = notes,
                    });

                    if (entityPM.PackageCode != poco.PackageCode || entityPM.TenantManagementLicenses != null)
                    {
                        string note = "";
                        if (entityPM.PackageCode != poco.PackageCode)
                        {
                            note = BuildPackagesNotes(poco, entityPM);
                        }
                        if (entityPM.TenantManagementLicenses != null)
                        {
                            note = note + BuildTenantManagementLicensesNotes(entityPM);
                        }
                        if (!string.IsNullOrEmpty(note))
                        {
                            EventTracer.CreateTraceEvent(new EventTracerArgs()
                            {
                                Tenant = 0,
                                EventTypeCode = "PCMG",
                                UserId = loggedContact.Id,
                                EntityId = poco.Id.ToString(),
                                ObjectTableName = "TenantManagement",
                                Notes = note,
                            });
                        }
                    }

                    if (!entityPM.IsActive && poco.GlobalTenant.IsActive)
                    {
                        EventTracer.CreateTraceEvent(new EventTracerArgs()
                        {
                            Tenant = 0,
                            EventTypeCode = "INMG",
                            UserId = loggedContact.Id,
                            EntityId = poco.Id.ToString(),
                            ObjectTableName = "TenantManagement",
                        });
                    }

                    if (entityPM.IsActive && !poco.GlobalTenant.IsActive)
                    {
                        EventTracer.CreateTraceEvent(new EventTracerArgs()
                        {
                            Tenant = 0,
                            EventTypeCode = "ACMG",
                            UserId = loggedContact.Id,
                            EntityId = poco.Id.ToString(),
                            ObjectTableName = "TenantManagement",
                        });
                    }

                    if (entityPM.TrialStartDate != poco.TrialStartDate)
                    {
                        EventTracer.CreateTraceEvent(new EventTracerArgs()
                        {
                            Tenant = 0,
                            EventTypeCode = "TSMG",
                            UserId = loggedContact.Id,
                            EntityId = poco.Id.ToString(),
                            ObjectTableName = "TenantManagement",
                            Notes = "Trial start date changed from " + poco.TrialStartDate + " to " + entityPM.TrialStartDate,
                        });
                    }

                    if (entityPM.TrialEndDate != poco.TrialEndDate)
                    {
                        EventTracer.CreateTraceEvent(new EventTracerArgs()
                        {
                            Tenant = 0,
                            EventTypeCode = "TEMG",
                            UserId = loggedContact.Id,
                            EntityId = poco.Id.ToString(),
                            ObjectTableName = "TenantManagement",
                            Notes = "Trial end date changed from " + poco.TrialEndDate + " to " + entityPM.TrialEndDate,
                        });
                    }

                    if (entityPM.NumberOfUsers != poco.NumberOfUsers || entityPM.FreeUsers != poco.FreeUsers)
                    {
                        string usersNotes = null;
                        if(entityPM.NumberOfUsers != poco.NumberOfUsers)
                        {
                            usersNotes = "Number of users changed from " + poco.NumberOfUsers + " to " + entityPM.NumberOfUsers;
                        }

                        if (entityPM.FreeUsers != poco.FreeUsers)
                        {
                            if (string.IsNullOrEmpty(usersNotes))
                            {
                                usersNotes = "Number of free users changed from " + poco.FreeUsers + " to " + entityPM.FreeUsers;
                            }

                            else
                            {
                                usersNotes = usersNotes + Environment.NewLine +  "Number of free users changed from " + poco.FreeUsers + " to " + entityPM.FreeUsers;
                            }
                        }

                        EventTracer.CreateTraceEvent(new EventTracerArgs()
                        {
                            Tenant = 0,
                            EventTypeCode = "USMG",
                            UserId = loggedContact.Id,
                            EntityId = poco.Id.ToString(),
                            ObjectTableName = "TenantManagement",
                            Notes = usersNotes,
                        });
                    }

                    if (entityPM.PaymentFailure != poco.PaymentFailure || entityPM.SuspendDate != poco.SuspendDate)
                    {
                        EventTracer.CreateTraceEvent(new EventTracerArgs()
                        {
                            Tenant = 0,
                            EventTypeCode = "PFMG",
                            UserId = loggedContact.Id,
                            EntityId = poco.Id.ToString(),
                            ObjectTableName = "TenantManagement",
                        });
                    }

                    if (entityPM.TTY != poco.TTY)
                    {
                        string note = "TTY was changed from " + poco.TTY + " to " +  entityPM.TTY;

                        EventTracer.CreateTraceEvent(new EventTracerArgs()
                        {
                            Tenant = 0,
                            EventTypeCode = "TMTP",
                            UserId = loggedContact.Id,
                            EntityId = poco.Id.ToString(),
                            ObjectTableName = "TenantManagement",
                            Notes = note,
                        });
                    }

                    if (entityPM.PIMA != poco.PIMA)
                    {
                        string note = "PIMA was changed from " + poco.PIMA + " to " + entityPM.PIMA;

                        EventTracer.CreateTraceEvent(new EventTracerArgs()
                        {
                            Tenant = 0,
                            EventTypeCode = "TMTP",
                            UserId = loggedContact.Id,
                            EntityId = poco.Id.ToString(),
                            ObjectTableName = "TenantManagement",
                            Notes = note,
                        });
                    }
                }

                scope.Complete();
            }
        }

        private static string BuildPackagesNotes(TenantManagement poco, TenantManagementPM entityPM)
        {
            var notesTemp = "";
            PackageRepository repo = new PackageRepository(0);
            Package entity_Pm = repo.GetSinglePackage(entityPM.PackageCode);
            Package package_Poco = repo.GetSinglePackage(poco.PackageCode);

            var numberOfFreeUsers_poco = "";
            var numberOfFreeUsers_pm = "";
            if (poco.FreeUsers != null)
            {
                numberOfFreeUsers_poco = "+" + poco.FreeUsers;
            }
            if (entityPM.FreeUsers != null)
            {
                numberOfFreeUsers_pm = "+" + entityPM.FreeUsers;
            }
            notesTemp = entity_Pm == null ? "Package changed" : "Package changed from " + package_Poco.Name + " (" + poco.NumberOfUsers + numberOfFreeUsers_poco + ") " + " to "
                                                                                    + entity_Pm.Name + " (" + entityPM.NumberOfUsers + numberOfFreeUsers_pm + ") " + "\n";
            return notesTemp;
        }

        private static string BuildTenantManagementLicensesNotes(TenantManagementPM entityPM)
        {
            var notesTemp = "";
            var additionalPackagesAdded = entityPM.TenantManagementLicenses.Where(a => a.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert).ToList();
            var additionalPackagesRemoved = entityPM.TenantManagementLicenses.Where(a => a.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Delete).ToList();

            if (additionalPackagesAdded != null && additionalPackagesAdded.Count() > 0)
            {
                notesTemp = notesTemp + "Additional Packages Added : " + BuildAdditionalPackageNotes(additionalPackagesAdded);
            }
            if (additionalPackagesRemoved != null && additionalPackagesRemoved.Count() > 0)
            {
                notesTemp = notesTemp + "Additional Packages Removed : " + BuildAdditionalPackageNotes(additionalPackagesRemoved);
            }

            if (!string.IsNullOrEmpty(notesTemp))
            {
                notesTemp = notesTemp.TrimEnd(',') + "\n";
            }

            return notesTemp;
        }

        private static string BuildAdditionalPackageNotes(List<TenantManagementLicensePM> additionalPackages)
        {
            var notes = "";
            PackageRepository repo = new PackageRepository(0);
            Package package;

            foreach (var item in additionalPackages)
            {
                var numberOfFreeUsers = "";
                if (item.FreeUsers != null)
                {
                    numberOfFreeUsers = "+" + item.FreeUsers;
                }

                package = repo.GetSinglePackage(item.PackageCode);
                notes = notes + package.Name + " (" + item.NumberOfUsers + numberOfFreeUsers + ")" + ", ";
            }

            if (!string.IsNullOrEmpty(notes))
            {
                notes = notes.TrimEnd(' ').TrimEnd(',') + "\n";
            }
            return notes; 
        }
    }
}