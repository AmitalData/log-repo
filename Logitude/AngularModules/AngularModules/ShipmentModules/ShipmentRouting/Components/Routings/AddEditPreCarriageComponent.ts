import {Component} from '@angular/core';
import {AppTool, DateTool} from '../../../../Infrastructure/Tools';
import {ShipmentTool, RoutingHelper} from '../../../../Shipment/Tools';
import {ShipmentPM} from '../../../../Shipment/EntityPMs/ShipmentPM';
import {ShipmentFollowUpPM} from '../../../../Shipment/EntityPMs/ShipmentFollowUpPM';
import {RoutingsTabComponent} from './RoutingsTabComponent';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {Cloner} from '../../../../Infrastructure/Utilities/Cloner';
import {CardList} from '../../../../Common/EntityLists/CardList'; 
import {PortList} from '../../../../Common/EntityLists/PortList';
import {VesselList} from '../../../../Common/EntityLists/VesselList';  
import {CardListService} from '../../../../Common/Services/StandardLists/CardListService';
import {PortListService} from '../../../../Common/Services/StandardLists/PortListService';
import {VesselListService} from '../../../../Common/Services/StandardLists/VesselListService';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';

@Component({
    moduleId: module.id,
    templateUrl: './AddEditPreCarriageComponent.html',
})

export class AddEditPreCarriageComponent extends BaseComponent {
    public EntityPM: ShipmentPM;
    public ObjectTableName: string;
    public DataContext = this;   
    public ValidationErrorsList: string[] = [];
    public FatherComponent: RoutingsTabComponent;
    public IsConnectedHouse: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.InitServices();
    }

    private myPortListService: PortListService;
    private myCardListService: CardListService;
    private myVesselListService: VesselListService;
    InitServices() {
        this.myPortListService = new PortListService();
        this.myCardListService = new CardListService();
        this.myVesselListService = new VesselListService();
    }

    SetWindowArgs(args: any) {
        this.EntityPM = args['EntityPM'];
        this.ObjectTableName = args['ObjectTableName'];
        this.FatherComponent = args['FatherComponent'];
        this.Clone();

        this.SetDefaultValues();
        this.SetUIProperties();
        this.SetDependencies();
    }

    SetDefaultValues() {
        if (this.EntityPM.ShipmentLevelCode == "H" && !AppTool.IsNullOrEmpty(this.EntityPM.MasterShipmentDataId)) {
            this.IsConnectedHouse = true;
            this.PreCarriageToPortId = this.EntityPM.MainCarriageFromPortId;
        }
    }

    public IsEditingEnabled: boolean = true;
    SetUIProperties() {
        var isEditingEnabled = ShipmentTool.IsEditingEnabled(this.EntityPM);

        var isTransportFieldEnabled = false;
        var isCarrierNumberFieldEnabled = false;
        if (isEditingEnabled) {
            if (!AppTool.IsNullOrEmpty(this.PreCarriageTransportModeId)) {
                isTransportFieldEnabled = true;
            }

            if (!AppTool.IsNullOrEmpty(this.PreCarriageCarrierId)) {
                isCarrierNumberFieldEnabled = true;
            }
        }

        this.IsEditingEnabled = isEditingEnabled;
        this.UIProperties.SetEnabled("PreCarriageTransportModeId", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("PreCarriageFromPortId", this.ObjectTableName, isTransportFieldEnabled);
        this.UIProperties.SetEnabled("PreCarriageToPortId", this.ObjectTableName, isTransportFieldEnabled && this.IsConnectedHouse == false);
        this.UIProperties.SetEnabled("PreCarriageCarrierId", this.ObjectTableName, isTransportFieldEnabled);
        this.UIProperties.SetEnabled("PreCarriageCarrierNumber", this.ObjectTableName, isCarrierNumberFieldEnabled);
        this.UIProperties.SetEnabled("PreCarriageVesselId", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("PreCarriageETD", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("PreCarriageETA", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("PreCarriageATD", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("PreCarriageATA", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetVisibility("PreCarriageVesselId", this.ObjectTableName, this.PreCarriageTransportModeId == "O" ? true : false);
        this.SetUIProperties_RequiredFields();
        this.SetUIProperties_ValidateActualDates();
    }
    SetUIProperties_RequiredFields() {
        this.UIProperties.SetRequired("PreCarriageTransportModeId", this.ObjectTableName, AppTool.IsNullOrEmpty(this.PreCarriageTransportModeId) ? true : false);
        this.UIProperties.SetRequired("PreCarriageFromPortId", this.ObjectTableName, AppTool.IsNullOrEmpty(this.PreCarriageFromPortId) ? true : false);
        this.UIProperties.SetRequired("PreCarriageToPortId", this.ObjectTableName, AppTool.IsNullOrEmpty(this.PreCarriageToPortId) ? true : false);
    }
    SetUIProperties_ValidateActualDates() {

        this.UIProperties.SetValidity("PreCarriageATD", this.ObjectTableName, true, null);
        this.UIProperties.SetValidity("PreCarriageATA", this.ObjectTableName, true, null);

        if (!DateTool.IsActualDateValid(this.PreCarriageATD)) {
            var errorMessage = DateTool.ActualDateMessage.replace("Field", TextCodeTranslator.Translate("Shipment.O.Routings.ATD"));
            this.UIProperties.SetValidity("PreCarriageATD", this.ObjectTableName, false, errorMessage);
        }

        if (!DateTool.IsActualDateValid(this.PreCarriageATA)) {
            var errorMessage = DateTool.ActualDateMessage.replace("Field", TextCodeTranslator.Translate("Shipment.O.Routings.ATA"));
            this.UIProperties.SetValidity("PreCarriageATA", this.ObjectTableName, false, errorMessage);
        }
    }

    public CarrierDependencyProperty1: string = null;
    SetDependencies() {
        var myResult: string = null;

        switch (this.PreCarriageTransportModeId) {
            case "A": { myResult = "AL"; break; }
            case "O": { myResult = "SL"; break; }
            case "I": { myResult = "TR"; break; }
        }

        this.CarrierDependencyProperty1 = myResult;
    }


    get PreCarriageTransportModeId() { return this.EntityPM.PreCarriageTransportModeId; }
    set PreCarriageTransportModeId(value: string) {
        if (this.EntityPM.PreCarriageTransportModeId != value) {
            this.EntityPM.PreCarriageTransportModeId = value;
            this.PreCarriageFromPortId = null;

            if (!this.IsConnectedHouse) {
                this.PreCarriageToPortId = null;
            }

            this.PreCarriageCarrierId = null;
            this.PreCarriageCarrierNumber = null;
            this.PreCarriageVesselId = null;
            this.SetUIProperties();
            this.SetDependencies();
        }
    }

    get PreCarriageFromPortId() { return this.EntityPM.PreCarriageFromPortId; }
    set PreCarriageFromPortId(value: string) {
        if (this.EntityPM.PreCarriageFromPortId != value) {
            this.EntityPM.PreCarriageFromPortId = value;
            this.SetUIProperties_RequiredFields();

            if (AppTool.IsNullOrEmpty(value)) {
                this.EntityPM.PreCarriageFromPortCode = null;
                this.EntityPM.PreCarriageFromPortName = null;
                this.EntityPM.PreCarriageFromPortCountryCode = null;
                this.EntityPM.PreCarriageFromPortCountryName = null;
            }

            else {
                this.myPortListService.getSingleFromCache(value).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: PortList = myResponse.Result;
                        if (list) {
                            this.EntityPM.PreCarriageFromPortCode = list.Code;
                            this.EntityPM.PreCarriageFromPortName = list.EnglishName;
                            this.EntityPM.PreCarriageFromPortCountryCode = list.CountryCode;
                            this.EntityPM.PreCarriageFromPortCountryName = list.CountryName;
                        }

                        else {
                            this.myPortListService.getSingle(value).subscribe((myResponse2: ServiceResponse) => {
                                if (!myResponse2.HasError) {
                                    list = myResponse2.Result;
                                    if (list) {
                                        this.EntityPM.PreCarriageFromPortCode = list.Code;
                                        this.EntityPM.PreCarriageFromPortName = list.EnglishName;
                                        this.EntityPM.PreCarriageFromPortCountryCode = list.CountryCode;
                                        this.EntityPM.PreCarriageFromPortCountryName = list.CountryName;
                                    }
                                }
                            });
                        }
                    }
                });
            }
        }
    }

    get PreCarriageToPortId() { return this.EntityPM.PreCarriageToPortId; }
    set PreCarriageToPortId(value: string) {
        if (this.EntityPM.PreCarriageToPortId != value) {
            this.EntityPM.PreCarriageToPortId = value;
            this.SetUIProperties_RequiredFields();

            if (AppTool.IsNullOrEmpty(value)) {
                RoutingHelper.PreCarriageToPortChanged(this.EntityPM, null);
            }

            else {
                this.myPortListService.getSingleFromCache(value).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: PortList = myResponse.Result;

                        if (list) {
                            RoutingHelper.PreCarriageToPortChanged(this.EntityPM, list);
                        }

                        else {
                            this.myPortListService.getSingle(value).subscribe((myResponse2: ServiceResponse) => {
                                if (!myResponse2.HasError) {
                                    list = myResponse2.Result;
                                    RoutingHelper.PreCarriageToPortChanged(this.EntityPM, list);
                                }
                            });
                        }
                    }
                });
            }
        }
    }

    get PreCarriageCarrierId() { return this.EntityPM.PreCarriageCarrierId; }
    set PreCarriageCarrierId(value: string) {
        if (this.EntityPM.PreCarriageCarrierId != value) {
            this.EntityPM.PreCarriageCarrierId = value;
            this.SetUIProperties();

            if (AppTool.IsNullOrEmpty(value)) {                
                this.EntityPM.PreCarriageCarrierCode = null;
                this.EntityPM.PreCarriageCarrierName = null;
                this.EntityPM.PreCarriageCarrierWebSite = null;
            }

            else {
                this.myCardListService.getSingle(value).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: CardList = myResponse.Result;
                        if (list) {
                            this.EntityPM.PreCarriageCarrierCode = list.Code;
                            this.EntityPM.PreCarriageCarrierName = list.EnglishName;
                            this.EntityPM.PreCarriageCarrierWebSite = list.WebSite;
                        }
                    }
                });
            }
        }
    }

    get PreCarriageCarrierNumber() { return this.EntityPM.PreCarriageCarrierNumber; }
    set PreCarriageCarrierNumber(value: string) {
        if (this.EntityPM.PreCarriageCarrierNumber != value) {
            this.EntityPM.PreCarriageCarrierNumber = value;
        }
    }

    get PreCarriageVesselId() { return this.EntityPM.PreCarriageVesselId; }
    set PreCarriageVesselId(value: string) {
        if (this.EntityPM.PreCarriageVesselId != value) {
            this.EntityPM.PreCarriageVesselId = value;

            if (AppTool.IsNullOrEmpty(value)) {
                this.EntityPM.PreCarriageVesselName = null;
            }

            else {
                this.myVesselListService.getSingleFromCache(value).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: VesselList = myResponse.Result;
                        if (list) {
                            this.EntityPM.PreCarriageVesselName = list.EnglishName;
                        }
                    }
                });
            }
        }
    }

    get PreCarriageETD() { return this.EntityPM.PreCarriageETD; }
    set PreCarriageETD(value: Date) {
        if (this.EntityPM.PreCarriageETD != value) {
            this.EntityPM.PreCarriageETD = value;
        }
    }

    get PreCarriageETA() { return this.EntityPM.PreCarriageETA; }
    set PreCarriageETA(value: Date) {
        if (this.EntityPM.PreCarriageETA != value) {
            this.EntityPM.PreCarriageETA = value;
        }
    }

    get PreCarriageATD() { return this.EntityPM.PreCarriageATD; }
    set PreCarriageATD(value: Date) {
        if (this.EntityPM.PreCarriageATD != value) {
            this.EntityPM.PreCarriageATD = value;
            this.SetUIProperties_ValidateActualDates();
        }
    }

    get PreCarriageATA() { return this.EntityPM.PreCarriageATA; }
    set PreCarriageATA(value: Date) {
        if (this.EntityPM.PreCarriageATA != value) {
            this.EntityPM.PreCarriageATA = value;
            this.SetUIProperties_ValidateActualDates();
        }
    }

    SetActualDateClicked(fieldName: string) {
        switch (fieldName) {
            case "PreCarriageETD": { this.PreCarriageATD = DateTool.GetDateParts(this.PreCarriageETD).DateObject; break; }
            case "PreCarriageETA": { this.PreCarriageATA = DateTool.GetDateParts(this.PreCarriageETA).DateObject; break; }
        }
    }

    CancelButtonClicked() {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    }
    OkButtonClicked() {
        var errors: string[] = []; 
        var msg = TextCodeTranslator.Translate("General.M.FieldIsRequired");

        if (AppTool.IsNullOrEmpty(this.PreCarriageTransportModeId)) {
            errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate("Shipment.O.Routings.TransportMode")));
        }

        if (AppTool.IsNullOrEmpty(this.PreCarriageFromPortId)) {
            errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate("Shipment.O.Routings.From")));
        }

        if (AppTool.IsNullOrEmpty(this.PreCarriageToPortId)) {
            errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate("Shipment.O.Routings.To")));
        }

        // Series Dates
        RoutingHelper.ValidateRoutingsSeriesDates(this.EntityPM, errors, "PreCarriage");

        // Actual Dates
        if (!DateTool.IsActualDateValid(this.PreCarriageATD)) {
            errors.push(DateTool.ActualDateMessage.replace("Field", TextCodeTranslator.Translate("Shipment.O.Routings.ATD")));
        }
        if (!DateTool.IsActualDateValid(this.PreCarriageATA)) {
            errors.push(DateTool.ActualDateMessage.replace("Field", TextCodeTranslator.Translate("Shipment.O.Routings.ATA")));
        }

        this.ValidationErrorsList = errors;

        if (errors.length == 0) {
            this.FatherComponent.BuildItemsCollection();
            this.CurrentSession.CloseCurrentWindowEmit("OK");
        }
    }

    private myCloner: Cloner;
    private entityCloner: Cloner;
    private oldFollowups: ShipmentFollowUpPM[] = [];
    private Clone() {

        this.EntityPM.FollowUps.forEach(item => {
            var oldItem: ShipmentFollowUpPM = new ShipmentFollowUpPM(null);
            oldItem.Id = item.Id;
            oldItem.Date = item.Date;
            oldItem.Deleted = item.Deleted;
            oldItem.Done = item.Done;
            oldItem.DoneDateTime = item.DoneDateTime;
            oldItem.DoneNote = item.DoneNote;
            oldItem.EntityDateId = item.EntityDateId;
            oldItem.EventTypeFollowUpName = item.EventTypeFollowUpName;
            oldItem.EventTypeId = item.EventTypeId;
            oldItem.Tenant = item.Tenant;
            oldItem.ExternalDocumentId = item.ExternalDocumentId;
            oldItem.IsNew = item.IsNew;
            oldItem.JobId = item.JobId;
            oldItem.LegType = item.LegType;
            oldItem.ManualActivatedFollowUp = item.ManualActivatedFollowUp;
            oldItem.Note = item.Note;
            oldItem.OwnerUserId = item.OwnerUserId;
            oldItem.OwnerUserName = item.OwnerUserName;
            oldItem.ShipmentId = item.ShipmentId;
            oldItem.ChangeSetOp = item.ChangeSetOp;
            oldItem.OldEntityPM = item.OldEntityPM;
            oldItem.UIProperties = item.UIProperties;
            oldItem.UniqueKey = item.UniqueKey;
            oldItem.IsDirty = item.IsDirty;
            oldItem.EntityParentPM = item.EntityParentPM;
            this.oldFollowups.push(oldItem);
        });

        this.myCloner = new Cloner(this);
        this.myCloner.AddField('PreCarriageTransportModeId');
        this.myCloner.AddField('PreCarriageFromPortId');
        this.myCloner.AddField('PreCarriageToPortId');
        this.myCloner.AddField('PreCarriageCarrierId');
        this.myCloner.AddField('PreCarriageCarrierNumber');
        this.myCloner.AddField('PreCarriageVesselId');
        this.myCloner.AddField('PreCarriageETD');
        this.myCloner.AddField('PreCarriageETA');
        this.myCloner.AddField('PreCarriageATD');
        this.myCloner.AddField('PreCarriageATA');
        this.myCloner.AddEntity(this.EntityPM);

        this.entityCloner = new Cloner(this.EntityPM);
        this.entityCloner.AddField('FromCountryId');
        this.entityCloner.AddField('FromCountryIsEC');
        this.entityCloner.AddField('MainCarriageFromPortId');
        this.entityCloner.AddField('MainCarriageFromPortCode');
        this.entityCloner.AddField('MainCarriageFromPortName');
        this.entityCloner.AddField('MainCarriageFromPortCountryCode');
        this.entityCloner.AddField('MainCarriageFromPortCountryName');
        this.entityCloner.AddEntity(this.EntityPM);
    }
    private RejectChanges() {

        var addedItems: any[] = [];
        var removedItems: any[] = [];

        this.oldFollowups.forEach(item => {
            var existingItem = this.EntityPM.FollowUps.filter(f => f.LegType == item.LegType)[0];
            if (!existingItem) {
                removedItems.push(item);
            }
        });

        this.EntityPM.FollowUps.forEach(item => {
            var oldItem = this.oldFollowups.filter(f => f.LegType == item.LegType)[0];
            if (oldItem == null) {
                addedItems.push(item);
            }
        });

        if (addedItems.length > 0 || removedItems.length > 0) {
            addedItems.forEach(item => {
                this.EntityPM.RemoveShipmentFollowUp(item);
            });

            removedItems.forEach(item => {
                this.EntityPM.AddShipmentFollowUp(item);
            });

            this.CurrentSession.FireEvent("FollowupsChanged");
        }

        this.myCloner.RejectChanges();
        this.entityCloner.RejectChanges();
    }
}
