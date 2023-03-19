using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Security;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.BL.InfrastructureModel.EntityQueries;

namespace Logitude.BL.CommonDataModel.Tools.TraceEvents
{
    public class ChargesTypeTracing
    {
        public static void Trace(ChargesTypePM entityPM, ChargesType poco, bool isNewEntity)
        {
            ContactPM loggedContact = new ContactQuery(entityPM.Tenant).GetContactByEmailOnly(SecurityUtility.GetAuthenticatedUser(), entityPM.Tenant);

            if (isNewEntity)
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "CRCT",
                    UserId = loggedContact.Id,
                    EntityId = entityPM.Id,
                    ObjectTableName = "ChargesType",
                });
            }

            else
            {
                string notes = "";
                //IATA Code:
                if (entityPM.IATACodeId != poco.IATACodeId)
                {
                    IATACodeQuery IATACodeQuery = new IATACodeQuery(poco.Tenant);

                    if (poco.IATACodeId == null)
                    {
                        var NewIATACode = IATACodeQuery.GetSinglePM(entityPM.IATACodeId);
                        notes = TranslateTextsClass.Translate("ChargesType.F.DueTypeCode", poco.Tenant) + TranslateTextsClass.Translate("Accounting.General.O.NewValue", poco.Tenant) + NewIATACode.Code;
                    }
                    else if (entityPM.IATACodeId == null)
                    {
                        var OldIATACode = IATACodeQuery.GetSinglePM(poco.IATACodeId);
                        notes = TranslateTextsClass.Translate("ChargesType.F.DueTypeCode", poco.Tenant) + "," + TranslateTextsClass.Translate("Accounting.General.O.OldValue", poco.Tenant) + OldIATACode.Code + TranslateTextsClass.Translate("Accounting.General.O.NewValue", poco.Tenant) + "";
                    }
                    else
                    {
                        var NewIATACode = IATACodeQuery.GetSinglePM(entityPM.IATACodeId);
                        var OldIATACode = IATACodeQuery.GetSinglePM(poco.IATACodeId);
                        notes = TranslateTextsClass.Translate("ChargesType.F.DueTypeCode", poco.Tenant) + "," + TranslateTextsClass.Translate("Accounting.General.O.OldValue", poco.Tenant) + OldIATACode.Code + TranslateTextsClass.Translate("Accounting.General.O.NewValue", poco.Tenant) + NewIATACode.Code;

                    }
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "UPCT",
                        UserId = loggedContact.Id,
                        EntityId = entityPM.Id,
                        ObjectTableName = "ChargesType",
                        Notes = notes,
                    });
                }
                //Due Type:
                if (entityPM.DueTypeCode != poco.DueTypeCode)
                {
                    DueTypeQuery dueTypeQuery = new DueTypeQuery(poco.Tenant);

                    if (poco.DueTypeCode == null)
                    {
                        var NewDueType = dueTypeQuery.GetSinglePM(entityPM.DueTypeCode, entityPM.Tenant);
                        notes = TranslateTextsClass.Translate("ChargesType.F.DueTypeCode", poco.Tenant) + TranslateTextsClass.Translate("Accounting.General.O.NewValue", poco.Tenant) + NewDueType.Name;
                    }
                    else if (entityPM.DueTypeCode == null)
                    {
                        var OldDueType = dueTypeQuery.GetSinglePM(poco.DueTypeCode, poco.Tenant);
                        notes = TranslateTextsClass.Translate("ChargesType.F.DueTypeCode", poco.Tenant) + "," + TranslateTextsClass.Translate("Accounting.General.O.OldValue", poco.Tenant) + OldDueType.Name + TranslateTextsClass.Translate("Accounting.General.O.NewValue", poco.Tenant) + "";
                    }
                    else
                    {
                        var NewDueType = dueTypeQuery.GetSinglePM(entityPM.DueTypeCode, entityPM.Tenant);
                        var OldDueType = dueTypeQuery.GetSinglePM(poco.DueTypeCode, poco.Tenant);
                        notes = TranslateTextsClass.Translate("ChargesType.F.DueTypeCode", poco.Tenant) + "," + TranslateTextsClass.Translate("Accounting.General.O.OldValue", poco.Tenant) + OldDueType.Name + TranslateTextsClass.Translate("Accounting.General.O.NewValue", poco.Tenant) + NewDueType.Name;
                    }
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "UPCT",
                        UserId = loggedContact.Id,
                        EntityId = entityPM.Id,
                        ObjectTableName = "ChargesType",
                        Notes = notes,
                    });
                }
                //Default Currency Payables:
                if (entityPM.PayablesDefaultCurrencyId != poco.PayablesDefaultCurrencyId)
                {
                    CurrencyQuery PayablesDefaultCurrencyQuery = new CurrencyQuery(poco.Tenant);

                    if (poco.PayablesDefaultCurrencyId == null)
                    {
                        var NewPayablesDefaultCurrency = PayablesDefaultCurrencyQuery.GetSinglePM(entityPM.PayablesDefaultCurrencyId, entityPM.Tenant);
                        notes = TranslateTextsClass.Translate("ChargesType.F.PayablesDefaultCurrencyId", poco.Tenant) + TranslateTextsClass.Translate("Accounting.General.O.NewValue", poco.Tenant) + NewPayablesDefaultCurrency.Code;
                    }
                    else if (entityPM.PayablesDefaultCurrencyId == null)
                    {
                        var OldPayablesDefaultCurrency = PayablesDefaultCurrencyQuery.GetSinglePM(poco.PayablesDefaultCurrencyId, poco.Tenant);
                        notes = TranslateTextsClass.Translate("ChargesType.F.PayablesDefaultCurrencyId", poco.Tenant) + "," + TranslateTextsClass.Translate("Accounting.General.O.OldValue", poco.Tenant) + OldPayablesDefaultCurrency.Code + TranslateTextsClass.Translate("Accounting.General.O.NewValue", poco.Tenant) + "";
                    }
                    else
                    {
                        var NewPayablesDefaultCurrency = PayablesDefaultCurrencyQuery.GetSinglePM(entityPM.PayablesDefaultCurrencyId, entityPM.Tenant);
                        var OldPayablesDefaultCurrency = PayablesDefaultCurrencyQuery.GetSinglePM(poco.PayablesDefaultCurrencyId, poco.Tenant);
                        notes = TranslateTextsClass.Translate("ChargesType.F.PayablesDefaultCurrencyId", poco.Tenant) + "," + TranslateTextsClass.Translate("Accounting.General.O.OldValue", poco.Tenant) + OldPayablesDefaultCurrency.Code + TranslateTextsClass.Translate("Accounting.General.O.NewValue", poco.Tenant) + NewPayablesDefaultCurrency.Code;
                    }

                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "UPCT",
                        UserId = loggedContact.Id,
                        EntityId = entityPM.Id,
                        ObjectTableName = "ChargesType",
                        Notes = notes,
                    });
                }
                //Default Currency Receivables:
                if (entityPM.ReceivablesDefaultCurrencyId != poco.ReceivablesDefaultCurrencyId)
                {
                    CurrencyQuery ReceivablesDefaultCurrencyQuery = new CurrencyQuery(poco.Tenant);

                    if (poco.ReceivablesDefaultCurrencyId == null)
                    {
                        var NewReceivablesDefaultCurrency = ReceivablesDefaultCurrencyQuery.GetSinglePM(entityPM.ReceivablesDefaultCurrencyId, entityPM.Tenant);
                        notes = TranslateTextsClass.Translate("ChargesType.F.ReceivablesDefaultCurrencyId", poco.Tenant) + TranslateTextsClass.Translate("Accounting.General.O.NewValue", poco.Tenant) + NewReceivablesDefaultCurrency.Code;
                    }
                    else if (entityPM.ReceivablesDefaultCurrencyId == null)
                    {
                        var OldReceivablesDefaultCurrency = ReceivablesDefaultCurrencyQuery.GetSinglePM(poco.ReceivablesDefaultCurrencyId, poco.Tenant);
                        notes = TranslateTextsClass.Translate("ChargesType.F.ReceivablesDefaultCurrencyId", poco.Tenant) + "," + TranslateTextsClass.Translate("Accounting.General.O.OldValue", poco.Tenant) + OldReceivablesDefaultCurrency.Code + TranslateTextsClass.Translate("Accounting.General.O.NewValue", poco.Tenant) + "";
                    }
                    else
                    {
                        var NewReceivablesDefaultCurrency = ReceivablesDefaultCurrencyQuery.GetSinglePM(entityPM.ReceivablesDefaultCurrencyId, entityPM.Tenant);
                        var OldReceivablesDefaultCurrency = ReceivablesDefaultCurrencyQuery.GetSinglePM(poco.ReceivablesDefaultCurrencyId, poco.Tenant);
                        notes = TranslateTextsClass.Translate("ChargesType.F.ReceivablesDefaultCurrencyId", poco.Tenant) + "," + TranslateTextsClass.Translate("Accounting.General.O.OldValue", poco.Tenant) + OldReceivablesDefaultCurrency.Code + TranslateTextsClass.Translate("Accounting.General.O.NewValue", poco.Tenant) + NewReceivablesDefaultCurrency.Code;
                    }

                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "UPCT",
                        UserId = loggedContact.Id,
                        EntityId = entityPM.Id,
                        ObjectTableName = "ChargesType",
                        Notes = notes,
                    });
                }
                //VAT Type:
                if (entityPM.VatTypeId != poco.VatTypeId)
                {
                    VatTypeQuery vatTypeQuery = new VatTypeQuery(poco.Tenant);
                    var OldVatType = vatTypeQuery.GetSinglePM(poco.VatTypeId, poco.Tenant);
                    var NewVatType = vatTypeQuery.GetSinglePM(entityPM.VatTypeId, entityPM.Tenant);
                    notes = TranslateTextsClass.Translate("ChargesType.F.VatTypeId", poco.Tenant) + "," + TranslateTextsClass.Translate("Accounting.General.O.OldValue", poco.Tenant) + (OldVatType.EnglishName ?? OldVatType.LocalName) + TranslateTextsClass.Translate("Accounting.General.O.NewValue", poco.Tenant) + (NewVatType.EnglishName ?? NewVatType.LocalName);
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "UPCT",
                        UserId = loggedContact.Id,
                        EntityId = entityPM.Id,
                        ObjectTableName = "ChargesType",
                        Notes = notes,
                    });
                }
                //Container Measurement:
                if (entityPM.ContainerMeasurementId != poco.ContainerMeasurementId)
                {
                    MeasurementQuery measurementQuery = new MeasurementQuery(poco.Tenant);

                    if (poco.ContainerMeasurementId == null)
                    {
                        var NewMeasurement = measurementQuery.GetSinglePM(entityPM.ContainerMeasurementId, entityPM.Tenant);
                        notes = TranslateTextsClass.Translate("ChargesType.F.ContainerMeasurementId", poco.Tenant) + TranslateTextsClass.Translate("Accounting.General.O.NewValue", poco.Tenant) + (NewMeasurement.Name ?? NewMeasurement.LocalName);
                    }
                    else if (entityPM.ContainerMeasurementId == null)
                    {
                        var OldMeasurement = measurementQuery.GetSinglePM(poco.ContainerMeasurementId, poco.Tenant);
                        notes = TranslateTextsClass.Translate("ChargesType.F.ContainerMeasurementId", poco.Tenant) + "," + TranslateTextsClass.Translate("Accounting.General.O.OldValue", poco.Tenant) + (OldMeasurement.Name ?? OldMeasurement.LocalName) + TranslateTextsClass.Translate("Accounting.General.O.NewValue", poco.Tenant) + "";

                    }
                    else
                    {
                        var NewMeasurement = measurementQuery.GetSinglePM(entityPM.ContainerMeasurementId, entityPM.Tenant);
                        var OldMeasurement = measurementQuery.GetSinglePM(poco.ContainerMeasurementId, poco.Tenant);
                        notes = TranslateTextsClass.Translate("ChargesType.F.ContainerMeasurementId", poco.Tenant) + "," + TranslateTextsClass.Translate("Accounting.General.O.OldValue", poco.Tenant) + (OldMeasurement.Name ?? OldMeasurement.LocalName) + TranslateTextsClass.Translate("Accounting.General.O.NewValue", poco.Tenant) + (NewMeasurement.Name ?? NewMeasurement.LocalName);
                    }

                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "UPCT",
                        UserId = loggedContact.Id,
                        EntityId = entityPM.Id,
                        ObjectTableName = "ChargesType",
                        Notes = notes,
                    });
                }
                //Measurement:
                if (entityPM.MeasurementId != poco.MeasurementId)
                {
                    MeasurementQuery measurementQuery = new MeasurementQuery(poco.Tenant);
                    var OldMeasurement = measurementQuery.GetSinglePM(poco.MeasurementId, poco.Tenant);
                    var NewMeasurement = measurementQuery.GetSinglePM(entityPM.MeasurementId, entityPM.Tenant);
                    notes = TranslateTextsClass.Translate("ChargesType.F.ContainerMeasurementId", poco.Tenant) + "," + TranslateTextsClass.Translate("Accounting.General.O.OldValue", poco.Tenant) + (OldMeasurement.Name ?? OldMeasurement.LocalName) + TranslateTextsClass.Translate("Accounting.General.O.NewValue", poco.Tenant) + (NewMeasurement.Name ?? NewMeasurement.LocalName);
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "UPCT",
                        UserId = loggedContact.Id,
                        EntityId = entityPM.Id,
                        ObjectTableName = "ChargesType",
                        Notes = notes,
                    });
                }
                //Group Code:
                if (entityPM.ChargesGroupId != poco.ChargesGroupId)
                {
                    ChargesGroupRepository chargesGroupsRepository = new ChargesGroupRepository(poco.Tenant);
                    ChargesGroupQuery chargesGroupQuery = new ChargesGroupQuery(chargesGroupsRepository);
                    chargesGroupQuery = new ChargesGroupQuery(poco.Tenant);
                    var OldChargesGroup = chargesGroupQuery.GetSinglePM(poco.ChargesGroupId, poco.Tenant);
                    var NewChargesGroup = chargesGroupQuery.GetSinglePM(entityPM.ChargesGroupId, entityPM.Tenant);
                    notes = TranslateTextsClass.Translate("ChargesType.F.ChargesGroupId", poco.Tenant) + "," + TranslateTextsClass.Translate("Accounting.General.O.OldValue", poco.Tenant) + (OldChargesGroup.Name ?? OldChargesGroup.LocalName) + TranslateTextsClass.Translate("Accounting.General.O.NewValue", poco.Tenant) + (NewChargesGroup.Name ?? NewChargesGroup.LocalName);
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "UPCT",
                        UserId = loggedContact.Id,
                        EntityId = entityPM.Id,
                        ObjectTableName = "ChargesType",
                        Notes = notes,
                    });
                }
                //Code:
                if (entityPM.Code != poco.Code)
                {
                    notes = TranslateTextsClass.Translate("ChargesType.F.Code", poco.Tenant) + "," + TranslateTextsClass.Translate("Accounting.General.O.OldValue", poco.Tenant) + poco.Code + TranslateTextsClass.Translate("Accounting.General.O.NewValue", poco.Tenant) + entityPM.Code;
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "UPCT",
                        UserId = loggedContact.Id,
                        EntityId = entityPM.Id,
                        ObjectTableName = "ChargesType",
                        Notes = notes,
                    });
                }
                //Name:
                if (entityPM.EnglishName != poco.EnglishName)
                {
                    notes = TranslateTextsClass.Translate("ChargesType.F.EnglishName", poco.Tenant) + "," + TranslateTextsClass.Translate("Accounting.General.O.OldValue", poco.Tenant) + poco.EnglishName + TranslateTextsClass.Translate("Accounting.General.O.NewValue", poco.Tenant) + entityPM.EnglishName;
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "UPCT",
                        UserId = loggedContact.Id,
                        EntityId = entityPM.Id,
                        ObjectTableName = "ChargesType",
                        Notes = notes,
                    });
                }
                //Local Name:
                if (entityPM.LocalName != poco.LocalName)
                {
                    notes = TranslateTextsClass.Translate("ChargesType.F.LocalName", poco.Tenant) + "," + TranslateTextsClass.Translate("Accounting.General.O.OldValue", poco.Tenant) + poco.LocalName + TranslateTextsClass.Translate("Accounting.General.O.NewValue", poco.Tenant) + entityPM.LocalName;
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "UPCT",
                        UserId = loggedContact.Id,
                        EntityId = entityPM.Id,
                        ObjectTableName = "ChargesType",
                        Notes = notes,
                    });
                }
                //View Order:
                if (entityPM.ViewOrder != poco.ViewOrder)
                {
                    notes = TranslateTextsClass.Translate("ChargesType.F.ViewOrder", poco.Tenant) + "," + TranslateTextsClass.Translate("Accounting.General.O.OldValue", poco.Tenant) + poco.ViewOrder + TranslateTextsClass.Translate("Accounting.General.O.NewValue", poco.Tenant) + entityPM.ViewOrder;
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "UPCT",
                        UserId = loggedContact.Id,
                        EntityId = entityPM.Id,
                        ObjectTableName = "ChargesType",
                        Notes = notes,
                    });
                }
                //Description:
                if (entityPM.Description != poco.Description)
                {
                    notes = TranslateTextsClass.Translate("ChargesType.F.Description", poco.Tenant) + "," + TranslateTextsClass.Translate("Accounting.General.O.OldValue", poco.Tenant) + poco.Description + TranslateTextsClass.Translate("Accounting.General.O.NewValue", poco.Tenant) + entityPM.Description;
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "UPCT",
                        UserId = loggedContact.Id,
                        EntityId = entityPM.Id,
                        ObjectTableName = "ChargesType",
                        Notes = notes,
                    });
                }
                //Inactive Charge Type:
                if (entityPM.InActive && !poco.InActive)
                {
                    notes = "Charges Type Inactivated";
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "UPCT",
                        UserId = loggedContact.Id,
                        EntityId = entityPM.Id,
                        ObjectTableName = "ChargesType",
                        Notes = notes,
                    });
                }
                if (!entityPM.InActive && poco.InActive)
                {
                    notes = "Charges Type Activated";
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "UPCT",
                        UserId = loggedContact.Id,
                        EntityId = entityPM.Id,
                        ObjectTableName = "ChargesType",
                        Notes = notes,
                    });
                }
                //Delivery:
                if (entityPM.HasDelivery && !poco.HasDelivery)
                {
                    notes = "Charges Type Has Delivery";
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "UPCT",
                        UserId = loggedContact.Id,
                        EntityId = entityPM.Id,
                        ObjectTableName = "ChargesType",
                        Notes = notes,
                    });
                }
                if (!entityPM.HasDelivery && poco.HasDelivery)
                {
                    notes = "Charges Type Has No Delivery";
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "UPCT",
                        UserId = loggedContact.Id,
                        EntityId = entityPM.Id,
                        ObjectTableName = "ChargesType",
                        Notes = notes,
                    });
                }
                //Pickup:
                if (entityPM.HasPickup && !poco.HasPickup)
                {
                    notes = "Charges Type Has Pickup";
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "UPCT",
                        UserId = loggedContact.Id,
                        EntityId = entityPM.Id,
                        ObjectTableName = "ChargesType",
                        Notes = notes,
                    });
                }
                if (!entityPM.HasPickup && poco.HasPickup)
                {
                    notes = "Charges Type Has No Pickup";
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "UPCT",
                        UserId = loggedContact.Id,
                        EntityId = entityPM.Id,
                        ObjectTableName = "ChargesType",
                        Notes = notes,
                    });
                }
                //Expense Charge:
                if (entityPM.IsExpense && !poco.IsExpense)
                {
                    notes = "Charges Type Has Expense Charge";
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "UPCT",
                        UserId = loggedContact.Id,
                        EntityId = entityPM.Id,
                        ObjectTableName = "ChargesType",
                        Notes = notes,
                    });
                }
                if (!entityPM.IsExpense && poco.IsExpense)
                {
                    notes = "Charges Type Has No Expense Charge";
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "UPCT",
                        UserId = loggedContact.Id,
                        EntityId = entityPM.Id,
                        ObjectTableName = "ChargesType",
                        Notes = notes,
                    });
                }
                //Payable:
                if (entityPM.IsPayable && !poco.IsPayable)
                {
                    notes = "Charges Type Has Payable";
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "UPCT",
                        UserId = loggedContact.Id,
                        EntityId = entityPM.Id,
                        ObjectTableName = "ChargesType",
                        Notes = notes,
                    });
                }
                if (!entityPM.IsPayable && poco.IsPayable)
                {
                    notes = "Charges Type Has No Payable";
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "UPCT",
                        UserId = loggedContact.Id,
                        EntityId = entityPM.Id,
                        ObjectTableName = "ChargesType",
                        Notes = notes,
                    });
                }
                //Receivable:
                if (entityPM.IsReceivable && !poco.IsReceivable)
                {
                    notes = "Charges Type Has Receivable";
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "UPCT",
                        UserId = loggedContact.Id,
                        EntityId = entityPM.Id,
                        ObjectTableName = "ChargesType",
                        Notes = notes,
                    });
                }
                if (!entityPM.IsReceivable && poco.IsReceivable)
                {
                    notes = "Charges Type Has No Receivable";
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "UPCT",
                        UserId = loggedContact.Id,
                        EntityId = entityPM.Id,
                        ObjectTableName = "ChargesType",
                        Notes = notes,
                    });
                }
                //AWB Print Description:
                if (entityPM.AWBPrintDescription && !poco.AWBPrintDescription)
                {
                    notes = "Charges Type Has AWB Print Description";
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "UPCT",
                        UserId = loggedContact.Id,
                        EntityId = entityPM.Id,
                        ObjectTableName = "ChargesType",
                        Notes = notes,
                    });
                }
                if (!entityPM.AWBPrintDescription && poco.AWBPrintDescription)
                {
                    notes = "Charges Type Has No AWB Print Description";
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "UPCT",
                        UserId = loggedContact.Id,
                        EntityId = entityPM.Id,
                        ObjectTableName = "ChargesType",
                        Notes = notes,
                    });
                }
                //Auto Display-->Drop
                if (entityPM.IsDrop && !poco.IsDrop)
                {
                    notes = "Auto Display Drop Activated";
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "UPCT",
                        UserId = loggedContact.Id,
                        EntityId = entityPM.Id,
                        ObjectTableName = "ChargesType",
                        Notes = notes,
                    });
                }
                if (!entityPM.IsDrop && poco.IsDrop)
                {
                    notes = "Auto Display Drop Inactivated";
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "UPCT",
                        UserId = loggedContact.Id,
                        EntityId = entityPM.Id,
                        ObjectTableName = "ChargesType",
                        Notes = notes,
                    });
                }
                //Auto Display-->Domestic
                if (entityPM.IsDomestic && !poco.IsDomestic)
                {
                    notes = "Auto Display Domestic Activated";
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "UPCT",
                        UserId = loggedContact.Id,
                        EntityId = entityPM.Id,
                        ObjectTableName = "ChargesType",
                        Notes = notes,
                    });
                }
                if (!entityPM.IsDomestic && poco.IsDomestic)
                {
                    notes = "Auto Display Domestic Inactivated";
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "UPCT",
                        UserId = loggedContact.Id,
                        EntityId = entityPM.Id,
                        ObjectTableName = "ChargesType",
                        Notes = notes,
                    });
                }
                //Auto Display-->Import
                if (entityPM.IsImport && !poco.IsImport)
                {
                    notes = "Auto Display Import Activated";
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "UPCT",
                        UserId = loggedContact.Id,
                        EntityId = entityPM.Id,
                        ObjectTableName = "ChargesType",
                        Notes = notes,
                    });
                }
                if (!entityPM.IsImport && poco.IsImport)
                {
                    notes = "Auto Display Import Inactivated";
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "UPCT",
                        UserId = loggedContact.Id,
                        EntityId = entityPM.Id,
                        ObjectTableName = "ChargesType",
                        Notes = notes,
                    });
                }
                //Auto Display-->Export
                if (entityPM.IsExport && !poco.IsExport)
                {
                    notes = "Auto Display Export Activated";
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "UPCT",
                        UserId = loggedContact.Id,
                        EntityId = entityPM.Id,
                        ObjectTableName = "ChargesType",
                        Notes = notes,
                    });
                }
                if (!entityPM.IsExport && poco.IsExport)
                {
                    notes = "Auto Display Export Inactivated";
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "UPCT",
                        UserId = loggedContact.Id,
                        EntityId = entityPM.Id,
                        ObjectTableName = "ChargesType",
                        Notes = notes,
                    });
                }
                //Auto Display-->Quote
                if (entityPM.IsAutoDisplayInQuote && !poco.IsAutoDisplayInQuote)
                {
                    notes = "Auto Display Quote Activated";
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "UPCT",
                        UserId = loggedContact.Id,
                        EntityId = entityPM.Id,
                        ObjectTableName = "ChargesType",
                        Notes = notes,
                    });
                }
                if (!entityPM.IsAutoDisplayInQuote && poco.IsAutoDisplayInQuote)
                {
                    notes = "Auto Display Quote Inactivated";
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "UPCT",
                        UserId = loggedContact.Id,
                        EntityId = entityPM.Id,
                        ObjectTableName = "ChargesType",
                        Notes = notes,
                    });
                }
                //Auto Display-->Shipment
                if (entityPM.IsAutoDisplayInShipment && !poco.IsAutoDisplayInShipment)
                {
                    notes = "Auto Display In Shipment";
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "UPCT",
                        UserId = loggedContact.Id,
                        EntityId = entityPM.Id,
                        ObjectTableName = "ChargesType",
                        Notes = notes,
                    });
                }
                if (!entityPM.IsAutoDisplayInShipment && poco.IsAutoDisplayInShipment)
                {
                    notes = "Auto Display Not In Shipment";
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "UPCT",
                        UserId = loggedContact.Id,
                        EntityId = entityPM.Id,
                        ObjectTableName = "ChargesType",
                        Notes = notes,
                    });
                }
                //Auto Display-->Master
                if (entityPM.IsAutoDisplayInConsolidation && !poco.IsAutoDisplayInConsolidation)
                {
                    notes = "Auto Display In Master";
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "UPCT",
                        UserId = loggedContact.Id,
                        EntityId = entityPM.Id,
                        ObjectTableName = "ChargesType",
                        Notes = notes,
                    });
                }
                if (!entityPM.IsAutoDisplayInConsolidation && poco.IsAutoDisplayInConsolidation)
                {
                    notes = "Auto Display Not In Master";
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "UPCT",
                        UserId = loggedContact.Id,
                        EntityId = entityPM.Id,
                        ObjectTableName = "ChargesType",
                        Notes = notes,
                    });
                }
                //Transport-->Air
                if (entityPM.IsAir && !poco.IsAir)
                {
                    notes = "Transport In Air";
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "UPCT",
                        UserId = loggedContact.Id,
                        EntityId = entityPM.Id,
                        ObjectTableName = "ChargesType",
                        Notes = notes,
                    });
                }
                if (!entityPM.IsAir && poco.IsAir)
                {
                    notes = "Transport Not In Air";
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "UPCT",
                        UserId = loggedContact.Id,
                        EntityId = entityPM.Id,
                        ObjectTableName = "ChargesType",
                        Notes = notes,
                    });
                }
                //Transport-->Inland
                if (entityPM.IsInland && !poco.IsInland)
                {
                    notes = "Transport In Inland";
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "UPCT",
                        UserId = loggedContact.Id,
                        EntityId = entityPM.Id,
                        ObjectTableName = "ChargesType",
                        Notes = notes,
                    });
                }
                if (!entityPM.IsInland && poco.IsInland)
                {
                    notes = "Transport Not In Inland";
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "UPCT",
                        UserId = loggedContact.Id,
                        EntityId = entityPM.Id,
                        ObjectTableName = "ChargesType",
                        Notes = notes,
                    });
                }
                //Transport-->Ocean
                if (entityPM.IsOcean && !poco.IsOcean)
                {
                    notes = "Transport In Ocean";
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "UPCT",
                        UserId = loggedContact.Id,
                        EntityId = entityPM.Id,
                        ObjectTableName = "ChargesType",
                        Notes = notes,
                    });
                }
                if (!entityPM.IsOcean && poco.IsOcean)
                {
                    notes = "Transport Not In Ocean";
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "UPCT",
                        UserId = loggedContact.Id,
                        EntityId = entityPM.Id,
                        ObjectTableName = "ChargesType",
                        Notes = notes,
                    });
                }
            }
        }
    }
}
