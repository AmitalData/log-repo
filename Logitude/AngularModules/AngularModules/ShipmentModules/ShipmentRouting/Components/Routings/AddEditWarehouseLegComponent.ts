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
import {AddressList} from '../../../../Common/EntityLists/AddressList';
import {CardListService} from '../../../../Common/Services/StandardLists/CardListService';
import {AddressListService} from '../../../../Common/Services/StandardLists/AddressListService';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {TenantPM} from '../../../../Common/EntityPMs/TenantPM';
import {WarehouseHelper} from '../../../../Warehouse/Helpers/WarehouseHelper';
import {FeatureLocator} from '../../../../Infrastructure/Utilities/FeatureLocator';
import {ShipmentPickUpPM} from '../../../../Shipment/EntityPMs/ShipmentPickUpPM';
import { CardPMService } from '../../../../Common/Services/StandardPMs/CardPMService';
import { CardPM } from '../../../../Common/EntityPMs/CardPM';

@Component({
    moduleId: './ShipmentModules/ShipmentRouting/Components/Routings/',
    templateUrl: 'AddEditWarehouseLegComponent.html',
})

export class AddEditWarehouseLegComponent extends BaseComponent {

    public EntityPM: ShipmentPM;
    public TenantPM: TenantPM;
    public ObjectTableName: string;
    public DataContext = this;
    public LegType: string;
    public IsNewLeg: boolean = false;
    public IsFCLEntity: boolean = false;
    public TransportModeId: string = null;
    public ValidationErrorsList: string[] = [];
    public FatherComponent: RoutingsTabComponent;
    public IsImportShipment: boolean = false;
    IsShowNewWarehouseEntryButton: Boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.TenantPM = SessionLocator.TenantPM;
        this.InitServices();
    }

    private myAddressListService: AddressListService;
    private cardService: CardPMService;
    InitServices() {
        this.myAddressListService = new AddressListService();
        this.cardService = new CardPMService();
    }

    GetShipmentDirection() {
        if (this.EntityPM.DirectionId == "I") {
            this.IsImportShipment = true;
        }
    }

    InitFreeDaysStorage() {
        this.cardService.get(this.EntityPM.ConsigneeId).subscribe((myResponse: ServiceResponse) => {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    var result = myResponse.Result;
                    if (result) {
                        if (result.IsCustomer)
                            this.WarehouseStorageFreeDays = result.StorageFreeDays;
                    }
                }
            }
        });
    }
     
    SetWindowArgs(args: any) {
        this.EntityPM = args['EntityPM'];
        this.LegType = args['LegType'];
        this.IsNewLeg = args['IsNewLeg'];

        if (this.EntityPM) {
            this.TransportModeId = this.EntityPM.TransportModeId;
            this.IsFCLEntity = AppTool.IsFCLEntity(this.EntityPM.TransportModeId, this.EntityPM.ShipmentTypeId);

            if (this.EntityPM.ShipmentLevelCode == "D" || this.EntityPM.ShipmentLevelCode == "H") {
                if (FeatureLocator.HasFeaturePermession("WarehouseEntry", "Module")) {
                    this.IsShowNewWarehouseEntryButton = true;
                }
            }
            this.GetShipmentDirection();
            this.SetStorageDays();
        }
         
        this.ObjectTableName = args['ObjectTableName'];
        this.FatherComponent = args['FatherComponent'];
        this.WarehouseAddressList = this.FatherComponent.WarehouseAddressList;
        this.SetUIProperties();
        this.Clone();
         
        if (this.IsNewLeg) {
            if (this.LegType == "WarehouseLeg_Pickups") {
                if (this.EntityPM.ShipmentPickUps.length > 0) {
                    var FirstPickup: ShipmentPickUpPM = this.EntityPM.ShipmentPickUps.sort(function (a, b) { return a.PickUpDeliveryNumber.toLowerCase() == b.PickUpDeliveryNumber.toLowerCase() ? 0 : a.PickUpDeliveryNumber.toLowerCase() < b.PickUpDeliveryNumber.toLowerCase() ? -1 : 1; })[0];
                    if (FirstPickup) {
                        this.WarehouseLegExpectedEntryDate = FirstPickup.ETA
                        this.WarehouseLegActualEntryDate = FirstPickup.ATA;
                    }
                }
            }
            this.GetShipmentDirection();
            this.InitFreeDaysStorage();
        }
    }

    public IsEditingEnabled: boolean = true;
    public IsFirmCodeVisible: boolean = true;
    SetUIProperties() {
        var isEditingEnabled = ShipmentTool.IsEditingEnabled(this.EntityPM);   
        this.IsEditingEnabled = isEditingEnabled;
        this.IsFirmCodeVisible = (this.TenantPM.CountryCode.toUpperCase()) == "US" ? true : false;

        this.UIProperties.SetEnabled("WarehouseLegWarehouseId", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("WarehouseLegAddressId", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("WarehouseLegTerminalCode", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("WarehouseLegRemarks", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("WarehouseLegLastFreeDate", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("WarehouseLegExpectedEntryDate", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("WarehouseLegExpectedReleaseDate", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("WarehouseLegActualEntryDate", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("WarehouseLegActualReleaseDate", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("WarehouseLegReference", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("TerminalAvailable", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetRequired("WarehouseLegWarehouseId", this.ObjectTableName, AppTool.IsNullOrEmpty(this.WarehouseLegWarehouseId) ? true : false);
        this.SetUIProperties_ValidateActualDates();
    }
    SetUIProperties_ValidateActualDates() {

        this.UIProperties.SetValidity("WarehouseLegActualEntryDate", this.ObjectTableName, true, null);
        this.UIProperties.SetValidity("WarehouseLegActualReleaseDate", this.ObjectTableName, true, null);

        if (this.LegType == "WarehouseLeg_Pickups") {
            var date: Date = DateTool.GetCurrentDateAsUtc();

            if (this.IsDateBigger(this.WarehouseLegActualEntryDate, date)) {
                var errorMessage = DateTool.ActualDateMessage.replace("Field", TextCodeTranslator.Translate("Shipment.O.Routings.WarehouseLegActualEntryDate"));
                this.UIProperties.SetValidity("WarehouseLegActualEntryDate", this.ObjectTableName, false, errorMessage);
            }

            if (this.IsDateBigger(this.WarehouseLegActualReleaseDate, date)) {
                var errorMessage = DateTool.ActualDateMessage.replace("Field", TextCodeTranslator.Translate("Shipment.O.Routings.WarehouseLegActualReleaseDate"));
                this.UIProperties.SetValidity("WarehouseLegActualReleaseDate", this.ObjectTableName, false, errorMessage);
            }
        }

        else {
            if (!DateTool.IsActualDateValid(this.WarehouseLegActualEntryDate)) {
                var errorMessage = DateTool.ActualDateMessage.replace("Field", TextCodeTranslator.Translate("Shipment.O.Routings.WarehouseLegActualEntryDate"));
                this.UIProperties.SetValidity("WarehouseLegActualEntryDate", this.ObjectTableName, false, errorMessage);
            }

            if (!DateTool.IsActualDateValid(this.WarehouseLegActualReleaseDate)) {
                var errorMessage = DateTool.ActualDateMessage.replace("Field", TextCodeTranslator.Translate("Shipment.O.Routings.WarehouseLegActualReleaseDate"));
                this.UIProperties.SetValidity("WarehouseLegActualReleaseDate", this.ObjectTableName, false, errorMessage);
            }
        }
    }

    private IsDateBigger(date1: any, date2: any) {
        var myResult: boolean = false;

        if (!AppTool.IsNullOrEmpty(date1) && !AppTool.IsNullOrEmpty(date2)) {
            var Date1Parts = DateTool.GetDateParts(date1);
            var Date2Parts = DateTool.GetDateParts(date2);

            var Date1Ticks = (Date1Parts.Day * 1) + (Date1Parts.Month * 30) + (Date1Parts.Year * 365);
            var Date2Ticks = (Date2Parts.Day * 1) + (Date2Parts.Month * 30) + (Date2Parts.Year * 365);

            if (Date1Ticks > Date2Ticks) {
                myResult = true;
            }
        }

        return myResult;
    }

    get WarehouseLegWarehouseId() { return this.EntityPM.WarehouseLegWarehouseId; }
    set WarehouseLegWarehouseId(newValue: string) {
        if (this.EntityPM.WarehouseLegWarehouseId != newValue) {
            this.EntityPM.WarehouseLegWarehouseId = newValue;
            if (!AppTool.IsNullOrEmpty(newValue)) {
                this.GetAddress();
            }
            else {
                this.WarehouseLegAddressId = null;
                this.FatherComponent.WarehouseLegTerminalName = "";
            }

            this.SetUIProperties();
        }         
    }
    GetAddress() {
        var myService = new CardListService();
        myService.getSingle(this.WarehouseLegWarehouseId).subscribe((myResponse: ServiceResponse) => {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    var result = myResponse.Result;
                    if (result) {
                        this.WarehouseLegAddressId = result.MainAddressId;
                        this.FatherComponent.WarehouseLegTerminalName = result.EnglishName;
                        this.WarehouseLegTerminalCode = result.FirmCode;
                    }
                }
            }
        });
    }

    private myWarehouseAddressList: AddressList;
    get WarehouseAddressList() { return this.myWarehouseAddressList; }
    set WarehouseAddressList(newValue: AddressList) {
        this.myWarehouseAddressList = newValue;
        this.FatherComponent.WarehouseAddressList = newValue;
    } 

    get WarehouseLegAddressId() { return this.EntityPM.WarehouseLegAddressId; }
    set WarehouseLegAddressId(newValue: string) {
        if (this.EntityPM.WarehouseLegAddressId != newValue) {
            this.EntityPM.WarehouseLegAddressId = newValue;

            if (AppTool.IsNullOrEmpty(newValue)) {
                this.WarehouseAddressList = null;
            }

            else {
                this.myAddressListService.getSingle(newValue).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        this.WarehouseAddressList = myResponse.Result;
                    }
                });
            }
        }
    }

    get WarehouseLegTerminalCode() { return this.EntityPM.WarehouseLegTerminalCode; }
    set WarehouseLegTerminalCode(newValue: string) {
        if (this.EntityPM.WarehouseLegTerminalCode != newValue) {
            this.EntityPM.WarehouseLegTerminalCode = newValue;
        }
    }

    get WarehouseLegReference() { return this.EntityPM.WarehouseLegReference; }
    set WarehouseLegReference(newValue: string) {
        if (this.EntityPM.WarehouseLegReference != newValue) {
            this.EntityPM.WarehouseLegReference = newValue;
        }
    }

    get WarehouseLegRemarks() { return this.EntityPM.WarehouseLegRemarks; }
    set WarehouseLegRemarks(newValue: string) {
        if (this.EntityPM.WarehouseLegRemarks != newValue) {
            this.EntityPM.WarehouseLegRemarks = newValue;
        }
    }

    get WarehouseLegLastFreeDate() { return this.EntityPM.WarehouseLegLastFreeDate; }
    set WarehouseLegLastFreeDate(newValue: Date) {
        if (this.EntityPM.WarehouseLegLastFreeDate != newValue) {
            this.EntityPM.WarehouseLegLastFreeDate = newValue;
            if (newValue == null) {
                this.WarehouseLegLastFreeDate = null;
            } else {
                this.SetStorageFreeDays();
            }
        }
    }

    get WarehouseLegExpectedEntryDate() { return this.EntityPM.WarehouseLegExpectedEntryDate; }
    set WarehouseLegExpectedEntryDate(value: Date) {
        if (this.EntityPM.WarehouseLegExpectedEntryDate != value) {
            this.EntityPM.WarehouseLegExpectedEntryDate = value;
        }
    }

    get WarehouseLegExpectedReleaseDate() { return this.EntityPM.WarehouseLegExpectedReleaseDate; }
    set WarehouseLegExpectedReleaseDate(value: Date) {
        if (this.EntityPM.WarehouseLegExpectedReleaseDate != value) {
            this.EntityPM.WarehouseLegExpectedReleaseDate = value;
        }
    }

    get WarehouseLegActualEntryDate() { return this.EntityPM.WarehouseLegActualEntryDate; }
    set WarehouseLegActualEntryDate(value: Date) {
        if (this.EntityPM.WarehouseLegActualEntryDate != value) {
            this.EntityPM.WarehouseLegActualEntryDate = value;
            this.SetUIProperties_ValidateActualDates();
            if (value == null) {
                this.WarehouseLegActualEntryDate == null;
                this.StorageDays = null;
                this.Days = null;
            } else {
                this.SetLastFreeDate();
                this.SetStorageDays();
            }
        }
    }

    get WarehouseLegActualReleaseDate() { return this.EntityPM.WarehouseLegActualReleaseDate; }
    set WarehouseLegActualReleaseDate(value: Date) {
        if (this.EntityPM.WarehouseLegActualReleaseDate != value) {
            this.EntityPM.WarehouseLegActualReleaseDate = value;
            this.SetUIProperties_ValidateActualDates();
            if (value == null) {
                this.WarehouseLegActualReleaseDate = null;
                this.StorageDays = null;
                this.Days = null;
            } else {
                this.SetStorageDays();
            }
        }
    }

    get TerminalAvailable() { return this.EntityPM.TerminalAvailable; }
    set TerminalAvailable(value: Date) {
        if (this.EntityPM.TerminalAvailable != value) {
            this.EntityPM.TerminalAvailable = value;
            this.FatherComponent.EntityPM.TerminalAvailable = value;
        }
    }

    get WarehouseLegCutOffDate() { return this.EntityPM.WarehouseLegCutOffDate; }
    set WarehouseLegCutOffDate(value: Date) {
        if (this.EntityPM.WarehouseLegCutOffDate != value) {
            this.EntityPM.WarehouseLegCutOffDate = value;            
        }
    }

    get WarehouseLegVGMCutOffDate() { return this.EntityPM.WarehouseLegVGMCutOffDate; }
    set WarehouseLegVGMCutOffDate(value: Date) {
        if (this.EntityPM.WarehouseLegVGMCutOffDate != value) {
            this.EntityPM.WarehouseLegVGMCutOffDate = value;            
        }
    }

    get WarehouseStorageFreeDays() { return this.EntityPM.WarehouseStorageFreeDays; }
    set WarehouseStorageFreeDays(value: number) {
        if (this.EntityPM.WarehouseStorageFreeDays != value) {
            this.EntityPM.WarehouseStorageFreeDays = value;
            if (value == null) {
                this.WarehouseStorageFreeDays = null;
            } else {
                this.SetLastFreeDate();
            }
        }
    }

    private SetLastFreeDate() {
        if (this.WarehouseLegActualEntryDate != null && this.WarehouseStorageFreeDays != null) {
            var date = DateTool.AddDays(this.WarehouseLegActualEntryDate, this.WarehouseStorageFreeDays);
            if (date == null) {
                this.WarehouseStorageFreeDays = 0;
            } else {
                this.WarehouseLegLastFreeDate = date;
            }
        }
    } 

    private SetStorageFreeDays() {
        if (this.WarehouseLegActualEntryDate != null && this.WarehouseLegLastFreeDate != null) {
            if (DateTool.GetDateFromDate(this.WarehouseLegLastFreeDate) >= DateTool.GetDateFromDate(this.WarehouseLegActualEntryDate)) {
                var days = DateTool.GetDaysBetweenDates(this.WarehouseLegActualEntryDate, this.WarehouseLegLastFreeDate);
                if (days == null) {
                    this.WarehouseStorageFreeDays = 0;
                } else if (this.WarehouseStorageFreeDays != days) {
                    this.WarehouseStorageFreeDays = days;
                }
            } else {
                this.WarehouseStorageFreeDays = 0;
            }
        }
    }

    public StorageDays: number;
    public Days: string;
    private SetStorageDays() {
        if (this.WarehouseLegActualEntryDate != null && this.WarehouseLegActualReleaseDate != null) {
            if (DateTool.GetDateFromDate(this.WarehouseLegActualReleaseDate) >= DateTool.GetDateFromDate( this.WarehouseLegActualEntryDate)) {
                var days = DateTool.GetDaysBetweenDates(this.WarehouseLegActualEntryDate, this.WarehouseLegActualReleaseDate);
                this.StorageDays = days;
                this.Days = " Days";
            } else {
                this.StorageDays = null;
                this.Days = null;
            }
        }
    }
    

    NewWarehouseEntryButtonClicked() {
        var windowArgs: any = {};
        windowArgs.ExpectedEntryDate = this.WarehouseLegExpectedEntryDate;
        windowArgs.ActualEntryDate = this.WarehouseLegActualEntryDate;
        windowArgs.WarehouseId = this.WarehouseLegWarehouseId;
        windowArgs.EntityPM = this.EntityPM;
        windowArgs.IsNotSetWarehouseIdForWarehouseLegShipment = true;
        windowArgs.ConnectedTo = "Warehouse/Terminal";
        var warehouseHelper: WarehouseHelper = new WarehouseHelper();
        warehouseHelper.ShowNewWarehouseEntryComponent(windowArgs);
    }
    SetActualDateClicked(fieldName: string) {
        switch (fieldName) {
            case "WarehouseLegExpectedReleaseDate": {
                this.WarehouseLegActualReleaseDate = DateTool.GetDateParts(this.WarehouseLegExpectedReleaseDate).DateObject; break;
            }
            case "WarehouseLegExpectedEntryDate": {
                this.WarehouseLegActualEntryDate = DateTool.GetDateParts(this.WarehouseLegExpectedEntryDate).DateObject; break;
            }
        }
    }

    CancelButtonClicked() {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    }
    OkButtonClicked() {
        var errors: string[] = [];
        var msg = TextCodeTranslator.Translate("General.M.FieldIsRequired");

        if (AppTool.IsNullOrEmpty(this.WarehouseLegWarehouseId)) {
            errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate("Shipment.O.Routings.WarehouseLegWarehouseId")));
        }

        // Series Dates
        RoutingHelper.ValidateRoutingsSeriesDates(this.EntityPM, errors, this.LegType);

        // Actual Dates
        if (this.LegType == "WarehouseLeg_Pickups") {
            var date: Date = DateTool.GetCurrentDateAsUtc();

            if (this.IsDateBigger(this.WarehouseLegActualEntryDate, date)) {
                errors.push(DateTool.ActualDateMessage.replace("Field", TextCodeTranslator.Translate("Shipment.O.Routings.WarehouseLegActualEntryDate")));
            }

            if (this.IsDateBigger(this.WarehouseLegActualReleaseDate, date)) {
                errors.push(DateTool.ActualDateMessage.replace("Field", TextCodeTranslator.Translate("Shipment.O.Routings.WarehouseLegActualReleaseDate")));
            }
        }

        else {
            if (!DateTool.IsActualDateValid(this.WarehouseLegActualEntryDate)) {
                errors.push(DateTool.ActualDateMessage.replace("Field", TextCodeTranslator.Translate("Shipment.O.Routings.WarehouseLegActualEntryDate")));
            }

            if (!DateTool.IsActualDateValid(this.WarehouseLegActualReleaseDate)) {
                errors.push(DateTool.ActualDateMessage.replace("Field", TextCodeTranslator.Translate("Shipment.O.Routings.WarehouseLegActualReleaseDate")));
            }
        }

        this.ValidationErrorsList = errors;

        if (errors.length == 0) {
            this.FatherComponent.BuildItemsCollection();
            this.CurrentSession.CloseCurrentWindowEmit("OK");
        }
    }

    private myCloner: Cloner;
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
        this.myCloner.AddField('WarehouseLegWarehouseId');
        this.myCloner.AddField('WarehouseLegAddressId');
        this.myCloner.AddField('WarehouseLegTerminalCode');
        this.myCloner.AddField('WarehouseLegExpectedReleaseDate');
        this.myCloner.AddField('WarehouseLegExpectedEntryDate');
        this.myCloner.AddField('WarehouseLegActualEntryDate');
        this.myCloner.AddField('WarehouseLegActualReleaseDate');
        this.myCloner.AddField('WarehouseLegLastFreeDate');
        this.myCloner.AddField('WarehouseLegRemarks');
        this.myCloner.AddField('WarehouseLegTerminalName');
        this.myCloner.AddField('TerminalAvailable');
        this.myCloner.AddField('WarehouseLegCutOffDate');
        this.myCloner.AddField('WarehouseLegVGMCutOffDate');
        this.myCloner.AddField('WarehouseStorageFreeDays');
        this.myCloner.AddEntity(this.EntityPM);
        this.myCloner.AddEntity(this.WarehouseAddressList);
        this.myCloner.AddEntity(this.FatherComponent.WarehouseAddressList);
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
    }
}
