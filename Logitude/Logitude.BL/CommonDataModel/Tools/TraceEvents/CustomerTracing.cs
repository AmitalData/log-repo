using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Security;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Logitude.BL.Helpers;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure;
namespace Logitude.BL.CommonDataModel.Tools.TraceEvents
{
    public class CustomerTracing
    {
        private int myTenant;
        private bool isNewEntity;
        private CustomerPM entityPM;
        private Customer entityPOCO;
        private string loggedContactId;
        private string myTableName;
        public CustomerTracing(CustomerPM entityPM, Customer entityPOCO, string loggedContactId, bool isNewEntity)
        {
            this.myTenant = entityPM.Tenant;
            this.isNewEntity = isNewEntity;
            this.entityPM = entityPM;
            this.entityPOCO = entityPOCO;
            this.loggedContactId = loggedContactId;
            this.myTableName = "Customer";
        }

        public void Trace()
        {
            if (isNewEntity)
            {
                if (entityPM.CustomerStatusCode == "POT")
                {
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = myTenant,
                        EventTypeCode = "POTC",
                        UserId = loggedContactId,
                        EntityId = entityPM.Id,
                        ObjectTableName = myTableName,
                    });
                }

                else
                {
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = myTenant,
                        EventTypeCode = "CRCU",
                        UserId = loggedContactId,
                        EntityId = entityPM.Id,
                        ObjectTableName = myTableName,
                    });
                }                
            }

            else
            {
                string notes = null;
                if (!string.IsNullOrEmpty(entityPM.ReceivablesAccountingCard) && string.IsNullOrEmpty(entityPOCO.Card.ReceivablesAccountingCard))
                {
                    notes = "External ID added";
                }

                else if(!string.IsNullOrEmpty(entityPM.ReceivablesAccountingCard) && !string.IsNullOrEmpty(entityPOCO.Card.ReceivablesAccountingCard))
                {
                    if (entityPM.ReceivablesAccountingCard != entityPOCO.Card.ReceivablesAccountingCard)
                    {
                        notes = "External ID updated";
                    }
                }

                else if (string.IsNullOrEmpty(entityPM.ReceivablesAccountingCard) && !string.IsNullOrEmpty(entityPOCO.Card.ReceivablesAccountingCard))
                {
                    notes = "External ID removed";
                }

                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = myTenant,
                    EventTypeCode = "UPCU",
                    UserId = loggedContactId,
                    EntityId = entityPM.Id,
                    ObjectTableName = myTableName,
                    Notes = notes,
                });

                this.TraceCreditLimitFields();
            }

            if (entityPM.SetActivated)
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = myTenant,
                    EventTypeCode = "CSAV",
                    UserId = loggedContactId,
                    EntityId = entityPM.Id,
                    ObjectTableName = myTableName,
                    Notes = entityPM.EventNote,
                });
            }

            else if (entityPM.SetReady)
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = myTenant,
                    EventTypeCode = "CSWA",
                    UserId = loggedContactId,
                    EntityId = entityPM.Id,
                    ObjectTableName = myTableName,
                    Notes = entityPM.EventNote,
                });
            }

            else if (entityPM.SetInActive)
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = myTenant,
                    EventTypeCode = "CSIN",
                    UserId = loggedContactId,
                    EntityId = entityPM.Id,
                    ObjectTableName = myTableName,
                    Notes = entityPM.EventNote,
                });
            }

            else if (entityPM.SetReActivated)
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = myTenant,
                    EventTypeCode = "CSRA",
                    UserId = loggedContactId,
                    EntityId = entityPM.Id,
                    ObjectTableName = myTableName,
                    Notes = entityPM.EventNote,
                });
            }

            else if (entityPM.SetAsPotential)
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = myTenant,
                    EventTypeCode = "SPOT",
                    UserId = loggedContactId,
                    EntityId = entityPM.Id,
                    ObjectTableName = myTableName,
                    Notes = entityPM.EventNote,
                });
            }

            if (entityPM.IsCustomer && !entityPOCO.IsCustomer)
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = myTenant,
                    EventTypeCode = "CSMC",
                    UserId = loggedContactId,
                    EntityId = entityPM.Id,
                    ObjectTableName = myTableName,
                    Notes = entityPM.EventNote,
                });
            }

            if (!entityPM.IsCustomer && entityPOCO.IsCustomer)
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = myTenant,
                    EventTypeCode = "CSNC",
                    UserId = loggedContactId,
                    EntityId = entityPM.Id,
                    ObjectTableName = myTableName,
                    Notes = entityPM.EventNote,
                });
            }

            if (!string.IsNullOrEmpty(entityPM.SalesmanUserId))
            {
                if (entityPM.SalesmanUserId != entityPOCO.SalesmanUserId)
                {
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = myTenant,
                        EventTypeCode = "SLCH",
                        UserId = loggedContactId,
                        EntityId = entityPM.Id,
                        ObjectTableName = myTableName,
                        Notes = entityPM.EventNote,
                    });
                }
            }

            if (entityPM.ActivityWatch && !entityPOCO.ActivityWatch)
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = myTenant,
                    EventTypeCode = "PWEN",
                    UserId = loggedContactId,
                    EntityId = entityPM.Id,
                    ObjectTableName = myTableName,
                });
            }

            else if (!entityPM.ActivityWatch && entityPOCO.ActivityWatch)
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = myTenant,
                    EventTypeCode = "PWDS",
                    UserId = loggedContactId,
                    EntityId = entityPM.Id,
                    ObjectTableName = myTableName,
                });
            }
        }

        private void TraceCreditLimitFields()
        {
            if (!this.isNewEntity)
            {
                bool isChanged = false;
                if (entityPM.IsCreditLimitEnabled != entityPOCO.IsCreditLimitEnabled)
                {
                    isChanged = true;
                }

                else if (entityPM.CreditLimitAmount != entityPOCO.CreditLimitAmount)
                {
                    isChanged = true;
                }

                else if (entityPM.CreditLimitOpenBalance != entityPOCO.CreditLimitOpenBalance)
                {
                    isChanged = true;
                }

                else if (entityPM.CreditLimitWarningPercentage != entityPOCO.CreditLimitWarningPercentage)
                {
                    isChanged = true;
                }

                if (isChanged)
                {
                    string myEventNotes = "";
                    double myLimitAmount = entityPM.CreditLimitAmount == null ? 0 : entityPM.CreditLimitAmount.Value;
                    double myOpenBalance = entityPM.CreditLimitOpenBalance == null ? 0 : entityPM.CreditLimitOpenBalance.Value;
                    double myPercentage = entityPM.CreditLimitWarningPercentage == null ? 0 : entityPM.CreditLimitWarningPercentage.Value;

                    myEventNotes += "Credit Limit: " + String.Format("{0:N2}", myLimitAmount);
                    myEventNotes += Environment.NewLine + "Open Balance: " + String.Format("{0:N2}", myOpenBalance);
                    myEventNotes += Environment.NewLine + "Warning Percentage: " + myPercentage + "%";

                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = myTenant,
                        EventTypeCode = "CRDL",
                        UserId = loggedContactId,
                        EntityId = entityPM.Id,
                        ObjectTableName = myTableName,
                        Notes = myEventNotes,
                    });
                }
            }
        }

        public void TraceProducts(List<CustomerProductPM> dataChangeSet, bool isNewEntity)
        {
            List<string> myAddedItemsTexts = new List<string>();
            List<string> myUpdatedItemsTexts = new List<string>();
            List<string> myDeletedItemsTexts = new List<string>();
           
            foreach (CustomerProductPM itemPM in dataChangeSet)
            {
                if (isNewEntity)
                {
                    myAddedItemsTexts.Add(itemPM.ProductTypeName);
                }

                else
                {
                    switch (itemPM.ChangeSetOp)
                    {
                        case ChangeSetOperation.Insert:
                            {
                                myAddedItemsTexts.Add(itemPM.ProductTypeName);
                                break;
                            }

                        case ChangeSetOperation.Update:
                            {
                                myUpdatedItemsTexts.Add(itemPM.ProductTypeName);
                                break;
                            }

                        case ChangeSetOperation.Delete:
                            {
                                myDeletedItemsTexts.Add(itemPM.ProductTypeName);
                                break;
                            }
                    }
                }
            }

            if (myAddedItemsTexts.Count + myUpdatedItemsTexts.Count + myDeletedItemsTexts.Count > 0)
            {
                this.CreateEntityEvent("PRUP", "Customer", entityPM.Id, loggedContactId, entityPM.Tenant, myAddedItemsTexts, myUpdatedItemsTexts, myDeletedItemsTexts);
            }
        }

        public void TraceCompetitors(List<CustomerCompetitorPM> dataChangeSet, bool isNewEntity)
        {
            List<string> myAddedItemsTexts = new List<string>();
            List<string> myUpdatedItemsTexts = new List<string>();
            List<string> myDeletedItemsTexts = new List<string>();

            foreach (CustomerCompetitorPM itemPM in dataChangeSet)
            {
                if (isNewEntity)
                {
                    myAddedItemsTexts.Add(itemPM.CompetitorName);
                }

                else
                {
                    switch (itemPM.ChangeSetOp)
                    {
                        case ChangeSetOperation.Insert:
                            {
                                myAddedItemsTexts.Add(itemPM.CompetitorName);
                                break;
                            }

                        case ChangeSetOperation.Update:
                            {
                                myUpdatedItemsTexts.Add(itemPM.CompetitorName);
                                break;
                            }

                        case ChangeSetOperation.Delete:
                            {
                                myDeletedItemsTexts.Add(itemPM.CompetitorName);
                                break;
                            }
                    }
                }
            }

            if (myAddedItemsTexts.Count + myUpdatedItemsTexts.Count + myDeletedItemsTexts.Count > 0)
            {
                this.CreateEntityEvent("CMUP", "Customer", entityPM.Id, loggedContactId, entityPM.Tenant, myAddedItemsTexts, myUpdatedItemsTexts, myDeletedItemsTexts);
            }
        }

        public void TraceAdditionalServices(List<CustomerAdditionalServicePM> dataChangeSet, bool isNewEntity)
        {
            List<string> myAddedItemsTexts = new List<string>();
            List<string> myUpdatedItemsTexts = new List<string>();
            List<string> myDeletedItemsTexts = new List<string>();

            foreach (CustomerAdditionalServicePM itemPM in dataChangeSet)
            {                
                if (isNewEntity)
                {
                    myAddedItemsTexts.Add(itemPM.AdditionalServiceName);                   
                }

                else
                {
                    switch (itemPM.ChangeSetOp)
                    {
                        case ChangeSetOperation.Insert:
                            {
                                myAddedItemsTexts.Add(itemPM.AdditionalServiceName);  
                                break;
                            }

                        case ChangeSetOperation.Update:
                            {
                                myUpdatedItemsTexts.Add(itemPM.AdditionalServiceName);  
                                break;
                            }

                        case ChangeSetOperation.Delete:
                            {
                                myDeletedItemsTexts.Add(itemPM.AdditionalServiceName);  
                                break;
                            }
                    }
                }
            }

            if (myAddedItemsTexts.Count + myUpdatedItemsTexts.Count + myDeletedItemsTexts.Count > 0)
            {
                this.CreateEntityEvent("ADUP", "Customer", entityPM.Id, loggedContactId, entityPM.Tenant, myAddedItemsTexts, myUpdatedItemsTexts, myDeletedItemsTexts);
            }
        }

        public void TraceProductItems(List<ProductItemPM> dataChangeSet, bool isNewEntity)
        {
            List<string> addedItemsTexts = new List<string>();
            List<string> updatedItemsTexts = new List<string>();
            List<string> deletedItemsTexts = new List<string>();

            foreach (ProductItemPM itemPM in dataChangeSet)
            {
                if (isNewEntity)
                {
                    addedItemsTexts.Add(itemPM.Name);
                }

                else
                {
                    switch (itemPM.ChangeSetOp)
                    {
                        case ChangeSetOperation.Insert:
                            {
                                addedItemsTexts.Add(itemPM.SKU);
                                break;
                            }

                        case ChangeSetOperation.Update:
                            {
                                updatedItemsTexts.Add(itemPM.SKU);
                                break;
                            }

                        case ChangeSetOperation.Delete:
                            {
                                deletedItemsTexts.Add(itemPM.SKU);
                                break;
                            }
                    }
                }
            }

            if (addedItemsTexts.Count + updatedItemsTexts.Count + deletedItemsTexts.Count > 0)
            {
                this.CreateEntityEvent("PIUP", "Customer", entityPM.Id, loggedContactId, entityPM.Tenant, addedItemsTexts, updatedItemsTexts, deletedItemsTexts);
            }
        }


        private void CreateEntityEvent(string myEventCode, string myTableName, string myEntityId, string myLoggedContactId, int myTenant, List<string> myAddedItemsTexts, List<string> myUpdatedItemsTexts, List<string> myDeletedItemsTexts)
        {
            string myEventNotes = null;

            if (myAddedItemsTexts.Count > 0)
            {
                if (!string.IsNullOrEmpty(myEventNotes))
                {
                    myEventNotes += "\n";
                }

                myEventNotes += "Added:";

                foreach (string item in myAddedItemsTexts)
                {
                    myEventNotes += "\n" + item;
                }
            }

            if (myUpdatedItemsTexts.Count > 0)
            {
                if (!string.IsNullOrEmpty(myEventNotes))
                {
                    myEventNotes += "\n";
                }

                myEventNotes += "Updated:";

                foreach (string item in myUpdatedItemsTexts)
                {
                    myEventNotes += "\n" + item;
                }
            }

            if (myDeletedItemsTexts.Count > 0)
            {
                if (!string.IsNullOrEmpty(myEventNotes))
                {
                    myEventNotes += "\n";
                }

                myEventNotes += "Deleted:";

                foreach (string item in myDeletedItemsTexts)
                {
                    myEventNotes += "\n" + item;
                }
            }

            EventTracer.CreateTraceEvent(new EventTracerArgs()
            {
                Tenant = myTenant,
                EventTypeCode = myEventCode,
                UserId = myLoggedContactId,
                EntityId = myEntityId,
                ObjectTableName = myTableName,
                Notes = myEventNotes,
            });
        }
    }
}
