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

                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = 0,
                        EventTypeCode = "UPMG",
                        UserId = loggedContact.Id,
                        EntityId = poco.Id.ToString(),
                        ObjectTableName = "TenantManagement",
                    });

                    if (entityPM.PackageCode != poco.PackageCode)
                    {
                        PackageRepository packageRep = new PackageRepository(0);
                        Package package_Poco = packageRep.GetSinglePackage(poco.PackageCode);
                        Package package_Pm = packageRep.GetSinglePackage(entityPM.PackageCode);

                        string note = "Package changed from " + package_Poco.Name + " to " + package_Pm.Name;
 
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
                        });
                    }

                    if (entityPM.NumberOfUsers != poco.NumberOfUsers || entityPM.FreeUsers != poco.FreeUsers)
                    {
                        EventTracer.CreateTraceEvent(new EventTracerArgs()
                        {
                            Tenant = 0,
                            EventTypeCode = "USMG",
                            UserId = loggedContact.Id,
                            EntityId = poco.Id.ToString(),
                            ObjectTableName = "TenantManagement",
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
    }
}