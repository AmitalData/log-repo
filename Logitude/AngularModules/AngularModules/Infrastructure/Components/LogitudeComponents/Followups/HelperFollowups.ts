declare var window: any;
import {Component, OnInit, OnDestroy, Output, EventEmitter} from '@angular/core';
import {FollowUpPM} from '../../../EntityPMs/FollowUpPM';
import {QuotePM} from '../../../../Quote/EntityPMs/QuotePM';
import {ShipmentPM} from '../../../../Shipment/EntityPMs/ShipmentPM';
import {QuoteFollowUpPM} from '../../../../Quote/EntityPMs/QuoteFollowUpPM';
import {ShipmentFollowUpPM} from '../../../../Shipment/EntityPMs/ShipmentFollowUpPM';
import {AppTool, DateTool} from '../../../Tools';
import {BaseComponent} from '../BaseComponent';
import {Validator} from '../../../Validators/Validator';
import {SessionLocator} from '../../../Utilities/SessionLocator';
import {FeatureLocator} from '../../../Utilities/FeatureLocator';
import {ServiceResponse} from '../../../DataContracts/ServiceResponse';
import {EventTypeList} from '../../../EntityLists/EventTypeList';
import {EventTypeListService} from '../../../Services/StandardLists/EventTypeListService';
import {EntityResourceService} from '../../../Services/EntityResourceService';
import {Cloner} from '../../../Utilities/Cloner';
import {EntityArgs} from '../../../DataContracts/EntityArgs';

@Component({
    selector: "HelperFollowups",
    
    templateUrl: './HelperFollowups.html',
    inputs: ['QuotePM', 'ShipmentPM', 'IsEnabled'],
})

export class HelperFollowups extends BaseComponent implements OnInit, OnDestroy {
    public ComponentId: string = null;
    public ComponentButtonId: string = null;
    public ComponentContentId: string = null;
    public IsEnabled: boolean = true;
    public IconPath: string;
    public Width: number = 350;
    public Height: number = 130;
    public QuotePM: QuotePM = null;
    public DataContext = this;
    public AddDataContext: AddDataContext;
    public EditDataContext: EditDataContext;
    public ObjectTableName: string = "FollowUp";
    public ItemsSource: HelperFollowup[] = [];
    public EventTypes: EventTypeList[] = [];
    public IsResourcesReady: boolean = false;
    public IsComponentVisible: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private entityArgs: EntityArgs, private entityResourceService: EntityResourceService) {
        super();
        var idIndex = this.CurrentSession.GetNewId("HelperFollowups");
        this.ComponentId = "HelperFollowups_" + idIndex;
        this.ComponentButtonId = "HelperFollowupsButton_" + idIndex;
        this.ComponentContentId = "HelperFollowupsContent_" + idIndex;        
    }

    private SaveCompletedEvent: any = null;
    private FollowupsChangedEvent: any = null;
    Listen() {
        if (!this.FollowupsChangedEvent) {
            this.FollowupsChangedEvent = this.CurrentSession.SessionEvent.subscribe(s => {
                if (s == "FollowupsChanged") {
                    this.BuildItemsSource();
                }
            });
        }

        if (this.entityArgs) {
            this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.BuildItemsSource();
                }
            });
        }
    }


    RefreshEntity() {
        if (this.entityArgs.EditComponent && this.entityArgs.EditComponent.EntityPM) {
            if (this.QuotePM) {
                this.QuotePM = this.entityArgs.EditComponent.EntityPM;
            }

            else {
                this.ShipmentPM = this.entityArgs.EditComponent.EntityPM;
            }
        }
    }


    private shipmentPM: ShipmentPM = null;
    get ShipmentPM() {
        return this.shipmentPM;
    }
    set ShipmentPM(value: ShipmentPM) {
        this.shipmentPM = value;
    }




    ngOnInit() {
        if (this.QuotePM != null) {
            if (FeatureLocator.HasFeaturePermession("Quote", "Quote.Followups")) {
                this.IsComponentVisible = true;
            }
        }

        else if (this.ShipmentPM != null) {
            if (FeatureLocator.HasFeaturePermession("Shipment", "Shipment.Followups")) {
                this.IsComponentVisible = true;
            }
        }

        if (this.IsComponentVisible) {
            this.entityResourceService.getEntityResourceByTableName("FollowUp").subscribe((res: any) => {

                this.Listen();
                this.SetIconPath();
                this.SetPopupHeight();
                this.BuildItemsSource();

                this.IsResourcesReady = true;

                var entityId: string = null;
                var entityObjectTableId: string = null;
                var entityObjectTableName: string = null;
                if (this.QuotePM != null) {
                    entityId = this.QuotePM.Id;
                    entityObjectTableName = "Quote";
                }

                else if (this.ShipmentPM != null) {
                    entityId = this.ShipmentPM.Id;
                    entityObjectTableName = this.ShipmentPM.ShipmentLevelCode == "C" ? "Master" : "Shipment";
                }

                if (!AppTool.IsNullOrEmpty(entityObjectTableName)) {
                    var ObjectTable = window.ObjectTables.filter(x => x.Name === entityObjectTableName)[0];

                    if (ObjectTable) {
                        entityObjectTableId = ObjectTable.Id;

                        var myService = new EventTypeListService();
                        myService.getAll().subscribe((myResponse: ServiceResponse) => {
                            if (!myResponse.HasError) {
                                var lists: EventTypeList[] = myResponse.Result;
                                this.EventTypes = lists.filter(f => f.ObjectTableId == entityObjectTableId && f.ManualActivatedFollowUp == true);
                            }
                        });
                    }
                }
            });
        }
    }
    ngOnDestroy() {
        AppTool.KillEventEmitter(this.SaveCompletedEvent);
        AppTool.KillEventEmitter(this.FollowupsChangedEvent);

        if (this.IsComponentVisible) {
            this.CurrentSession.FireEvent("FollowupsChangedMainMenu");
        }

        this.StopPositionTimer();
    }

    SetIconPath() {
        if (this.ItemsSource.length == 0) {
            this.IconPath = "./_Resources/Images/Icons/Followups/Followup.png";
        }

        else {
            if (this.ItemsSource.filter(f => f.IsOld == true).length > 0) {
                this.IconPath = "./_Resources/Images/Icons/Followups/Followup_Red.png";
            }

            else {
                this.IconPath = "./_Resources/Images/Icons/Followups/Followup_Black.png";
            }
        }
    }
    SetPopupHeight() {
        var PopupHeight = 130;

        if (this.IsAddViewVisible) {
            PopupHeight = 230;
        }

        else if (this.IsEditViewVisible) {
            PopupHeight = 205;
        }

        else {
            var itemsCount = 0;

            if (this.QuotePM) {
                itemsCount = this.QuotePM.FollowUps.length;
            }

            else if (this.ShipmentPM) {
                itemsCount = this.ShipmentPM.FollowUps.length;
            }

            if (itemsCount > 3) {
                if (itemsCount > 5) {
                    itemsCount = 5;
                }

                PopupHeight = (itemsCount * 34) + 25 + 3;
            }
        }

        this.Height = PopupHeight;
    }

    private isOpened: boolean = false;
    get IsOpened() { return this.isOpened; }
    set IsOpened(value: boolean) {
        if (value != undefined) {
            if (this.isOpened != value) {
                this.isOpened = value;

                if (value) {
                    this.SetPopupHeight();
                    this.RunPositionTimer();
                    this.BuildItemsSource();
                }

                else {
                    this.StopPositionTimer();

                    if (this.AddDataContext || this.EditDataContext) {
                        if (this.EditDataContext) {
                            this.EditDataContext.RejectChanges();
                        }

                        this.CloseAddEdit();
                    }
                }
            }
        }
    }

    private timerToken: any;
    private StopPositionTimer() {
        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }
    }
    private RunPositionTimer() {
        this.StopPositionTimer();
        this.timerToken = setInterval(() => this.CalculateFixedPosition(), 0);
    }
    private CalculateFixedPosition() {
        var item = document.getElementById(this.ComponentId);
        if (item) {
            var itemRect = item.getBoundingClientRect();

            document.getElementById(this.ComponentContentId).style.top = (itemRect.top + 24) + 'px';
            document.getElementById(this.ComponentContentId).style.left = (itemRect.left + 40 - this.Width) + 'px';
        }
    }

    public IsMouseOver: boolean = false;
    public IsMouseOverButton: boolean = false;
    public IsMouseOverInputBox: boolean = false;
    public IsInputBoxFocused: boolean = false;
    OnButtonClicked() {
        if (this.IsOpened) {
            this.StopPositionTimer();
            this.IsOpened = false;
        }

        else {
            this.CalculateFixedPosition();
            this.IsOpened = true;
        }
    }
    OnButtonLostFocus() {
        if (!this.IsMouseOverButton) {
            if (this.IsMouseOver || this.IsMouseOverInputBox) {
                if (!this.IsMouseOverInputBox) {
                    document.getElementById(this.ComponentButtonId).focus();                    
                }
            }

            else {
                this.IsOpened = false;
            }
        }
    }
    OnInputBoxLostFocus() {
        console.log('OnInputBoxLostFocus');

        if (!this.IsMouseOverButton) {
            if (this.IsMouseOver) {
                document.getElementById(this.ComponentButtonId).focus();
            }

            else {
                this.IsOpened = false;
            }
        }

        //if (this.IsInputBoxFocused) {
        //    this.IsInputBoxFocused = false;

        //    if (!this.IsMouseOverButton) {
        //        if (this.IsMouseOver) {
        //            document.getElementById(this.ComponentButtonId).focus();
        //        }

        //        else {
        //            this.IsOpened = false;
        //        }
        //    }
        //}
    }

    public IsAddViewVisible: boolean = false;
    public IsEditViewVisible: boolean = false;
    public IsNormalViewVisible: boolean = true;
    InitAllViews() {
        this.IsAddViewVisible = false;
        this.IsEditViewVisible = false;
        this.IsNormalViewVisible = false;
    }

    BuildItemsSource() {
        this.RefreshEntity();

        this.ItemsSource = [];

        if (this.QuotePM) {
            this.QuotePM.FollowUps.filter(f => f.Done == false).forEach(itemPM => {
                this.ItemsSource.push(new HelperFollowup(itemPM));
            });
        }

        else if (this.ShipmentPM) {
            this.ShipmentPM.FollowUps.filter(f => f.Done == false).forEach(itemPM => {
                this.ItemsSource.push(new HelperFollowup(itemPM));
            });
        }

        this.SetIconPath();
    }
    AddFollowupClicked() {
        this.InitAllViews();
        this.IsAddViewVisible = true;
        this.SetPopupHeight();
        this.AddDataContext = new AddDataContext(this);
    }
    EditFollowupClicked(item: HelperFollowup) {
        this.InitAllViews();
        this.IsEditViewVisible = true;
        this.SetPopupHeight();
        this.EditDataContext = new EditDataContext(item, this);
    }
    CloseAddEdit() {
        this.InitAllViews();
        this.IsNormalViewVisible = true;
        this.SetPopupHeight();
        this.AddDataContext = null;
        this.EditDataContext = null;
        this.IsMouseOver = false;
        this.IsMouseOverButton = false;
        this.IsMouseOverInputBox = false;
        this.IsInputBoxFocused = false;
        this.BuildItemsSource();
        document.getElementById(this.ComponentButtonId).focus();
    }
}
export class HelperFollowup {
    public ItemId: string = null;
    public ItemTooltipId: string = null;
    public Done: boolean = false;
    public Date: Date = null;
    public Name: string = null;
    public Notes: string = null;
    public IconPath: string = null;
    public TextColor: string = null;
    public IsOld: boolean = false;
    public Background: string = "white";
    public ListItemHeight: number = 34;
    public TooltipHeight: number = 130;
    public TooltipWidth: number = 270;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityPM: any) {
        var idIndex = this.CurrentSession.GetNewId("FollowupItem");
        this.ItemId = "FollowupItem_" + idIndex;
        this.ItemTooltipId = "FollowupItemTooltip_" + idIndex;

        this.Done = entityPM.Done;
        this.Date = entityPM.Date;
        this.Name = entityPM.EventTypeFollowUpName;
        this.Notes = entityPM.Notes;
        this.TextColor = this.Done ? '#8F9293' : '#292E30';

        if (this.Date) {
            if (DateTool.GetDateParts(this.Date).DateTicks < DateTool.GetCurrentDateAsUtc().valueOf()) {
                this.IsOld = true;
                this.Background = "#F7E3E3";
            }
        }

        this.SetIconPath();
    }

    SetIconPath() {
        var iconPath = "./_Resources/Images/Icons/Followups/Document.png";

        if (this.Done) {
            iconPath = "./_Resources/Images/Icons/Followups/Done.png";
        }

        else if (this.Name) {
            var name = this.Name.toLowerCase();

            if (name.indexOf("arrived") > -1 || name.indexOf("departed") > -1 || name.indexOf("departure") > -1 || name.indexOf("arrival") > -1) {
                iconPath = "./_Resources/Images/Icons/Followups/Routing.png";
            }

            else if (name.indexOf("reminder") > -1 || name.indexOf("arranged") > -1) {
                iconPath = "./_Resources/Images/Icons/Followups/Reminder.png";
            }
        }

        this.IconPath = iconPath;
    }

    public IsShowTooltip: boolean = false;
    ShowTooltip(isShowTooltip: boolean) {
        if (!AppTool.IsNullOrEmpty(this.Notes)) {
            if (isShowTooltip) {
                var item = document.getElementById(this.ItemId);
                var itemRect = item.getBoundingClientRect();
                document.getElementById(this.ItemTooltipId).style.top = (itemRect.top - (this.TooltipHeight / 2) + (this.ListItemHeight / 2)) + 'px';
                document.getElementById(this.ItemTooltipId).style.left = (itemRect.left - this.TooltipWidth + 5) + 'px';
            }

            this.IsShowTooltip = isShowTooltip;
        }
    }
}
export class AddDataContext extends BaseComponent {
    public EntityPM: FollowUpPM;
    public ObjectTableName: string = "FollowUp";
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private father: HelperFollowups) {
        super();
        this.EntityPM = new FollowUpPM();
        this.EntityPM.Tenant = SessionLocator.Tenant;
        this.OwnerUserId = SessionLocator.LoggedUserId;
        this.EntityPM.Done = false;
        this.EntityPM.IsNew = true;
        this.EntityPM.Deleted = false;

        this.SelectedEventType = this.father.EventTypes.filter(f => f.Code == "REMF")[0];
        if (this.SelectedEventType) {
            this.ManualActivatedFollowUp = false;
        }
    }

    private selectedEventType: EventTypeList;
    get SelectedEventType() { return this.selectedEventType; }
    set SelectedEventType(value: EventTypeList) {
        if (this.selectedEventType != value) {
            this.selectedEventType = value;

            var eventTypeId: string = null;
            var eventTypeName: string = null;

            if (value) {
                eventTypeId = value.Id;
                eventTypeName = value.EnglishName;
            }

            this.EventTypeId = eventTypeId
            this.EventTypeFollowUpName = eventTypeName;
        }
    }

    get EventTypeId() { return this.EntityPM.EventTypeId; }
    set EventTypeId(value: string) {
        if (this.EntityPM.EventTypeId != value) {
            this.EntityPM.EventTypeId = value;
            this.UIProperties.SetRequired("EventTypeId", this.ObjectTableName, AppTool.IsNullOrEmpty(value) ? true : false);
        }
    }

    get EventTypeFollowUpName() { return this.EntityPM.EventTypeFollowUpName; }
    set EventTypeFollowUpName(value: string) {
        if (this.EntityPM.EventTypeFollowUpName != value) {
            this.EntityPM.EventTypeFollowUpName = value;
        }
    }

    get ManualActivatedFollowUp() { return this.EntityPM.ManualActivatedFollowUp; }
    set ManualActivatedFollowUp(value: boolean) {
        if (this.EntityPM.ManualActivatedFollowUp != value) {
            this.EntityPM.ManualActivatedFollowUp = value;
        }
    }

    get Date() { return this.EntityPM.Date; }
    set Date(value: Date) {
        if (this.EntityPM.Date != value) {
            this.EntityPM.Date = value;
        }
    }

    get OwnerUserId() { return this.EntityPM.OwnerUserId; }
    set OwnerUserId(value: string) {
        if (this.EntityPM.OwnerUserId != value) {
            this.EntityPM.OwnerUserId = value;
        }
    }

    get Notes() { return this.EntityPM.Notes; }
    set Notes(value: string) {
        if (this.EntityPM.Notes != value) {
            this.EntityPM.Notes = value;
        }
    }

    NumericButtonClicked(isIncreas: boolean) {
        if (this.Date == null) {
            this.Date = DateTool.GetCurrentDateAsUtc();
        }

        else {
            var day: number = isIncreas ? 1 : -1;
            var myDateParts = DateTool.GetDateParts(this.Date);

            var dateObject = new Date();
            dateObject.setUTCMonth(0);
            dateObject.setUTCDate(1);
            dateObject.setUTCFullYear(myDateParts.Year);
            dateObject.setUTCMonth((myDateParts.Month - 1));
            dateObject.setUTCDate(myDateParts.Day + day);
            dateObject.setUTCHours(myDateParts.Hours);
            dateObject.setUTCMinutes(myDateParts.Minutes);
            dateObject.setUTCSeconds(myDateParts.Seconds);
            dateObject.setUTCMilliseconds(myDateParts.Milliseconds);
            this.Date = dateObject;
        }
    }
    CancelButtonClicked() {
        this.father.CloseAddEdit();
    }
    OkButtonClicked() {
        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);

        if (errors.length == 0) {

            if (this.father.QuotePM) {
                var myQuoteFollowUpPM = new QuoteFollowUpPM(null);
                myQuoteFollowUpPM.Tenant = this.father.QuotePM.Tenant;
                myQuoteFollowUpPM.QuoteId = this.father.QuotePM.Id;
                myQuoteFollowUpPM.EventTypeId = this.EventTypeId;
                myQuoteFollowUpPM.EventTypeFollowUpName = this.EventTypeFollowUpName;
                myQuoteFollowUpPM.ManualActivatedFollowUp = this.ManualActivatedFollowUp;
                myQuoteFollowUpPM.Date = this.Date;
                myQuoteFollowUpPM.OwnerUserId = this.OwnerUserId;
                myQuoteFollowUpPM.Notes = this.Notes;
                myQuoteFollowUpPM.Done = this.EntityPM.Done;
                myQuoteFollowUpPM.IsNew = this.EntityPM.IsNew;
                this.father.QuotePM.AddQuoteFollowUpPM(myQuoteFollowUpPM);
            }

            else if (this.father.ShipmentPM) {
                var myShipmentFollowUpPM = new ShipmentFollowUpPM(null);
                myShipmentFollowUpPM.Tenant = this.father.ShipmentPM.Tenant;
                myShipmentFollowUpPM.ShipmentId = this.father.ShipmentPM.Id;
                myShipmentFollowUpPM.EventTypeId = this.EventTypeId;
                myShipmentFollowUpPM.EventTypeFollowUpName = this.EventTypeFollowUpName;
                myShipmentFollowUpPM.ManualActivatedFollowUp = this.ManualActivatedFollowUp;
                myShipmentFollowUpPM.Date = this.Date;
                myShipmentFollowUpPM.OwnerUserId = this.OwnerUserId;
                myShipmentFollowUpPM.Notes = this.Notes;
                myShipmentFollowUpPM.Done = this.EntityPM.Done;
                myShipmentFollowUpPM.IsNew = this.EntityPM.IsNew;
                this.father.ShipmentPM.AddShipmentFollowUp(myShipmentFollowUpPM);
            }

            this.father.CloseAddEdit();
            this.CurrentSession.FireEvent("FollowupsChanged");
        }
    }
}
export class EditDataContext extends BaseComponent {
    public Name: string = null;
    public IconPath: string = null;
    public EntityPM: any = null;
    public ObjectTableName: string = "FollowUp";
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(item: HelperFollowup, private father: HelperFollowups) {
        super();
        this.Name = item.Name;
        this.IconPath = item.IconPath;
        this.EntityPM = item.entityPM;
        this.Clone();
    }

    get Date() { return this.EntityPM.Date; }
    set Date(value: Date) {
        if (this.EntityPM.Date != value) {
            this.EntityPM.Date = value;
        }
    }

    get OwnerUserId() { return this.EntityPM.OwnerUserId; }
    set OwnerUserId(value: string) {
        if (this.EntityPM.OwnerUserId != value) {
            this.EntityPM.OwnerUserId = value;
        }
    }

    get Notes() { return this.EntityPM.Notes; }
    set Notes(value: string) {
        if (this.EntityPM.Notes != value) {
            this.EntityPM.Notes = value;
        }
    }

    get Done() { return this.EntityPM.Done; }
    set Done(value: boolean) {
        if (this.EntityPM.Done != value) {
            this.EntityPM.Done = value;
        }
    }

    get DoneDateTime() { return this.EntityPM.DoneDateTime; }
    set DoneDateTime(value: Date) {
        if (this.EntityPM.DoneDateTime != value) {
            this.EntityPM.DoneDateTime = value;
        }
    }

    get DoneNote() { return this.EntityPM.DoneNote; }
    set DoneNote(value: string) {
        if (this.EntityPM.DoneNote != value) {
            this.EntityPM.DoneNote = value;
        }
    }

    DeleteFollowupClicked() {
        if (this.father.QuotePM) {
            this.father.QuotePM.RemoveQuoteFollowUpPM(this.EntityPM);
        }

        else if (this.father.ShipmentPM) {
            this.father.ShipmentPM.RemoveShipmentFollowUp(this.EntityPM);
        }

        this.father.CloseAddEdit();
        this.CurrentSession.FireEvent("FollowupsChanged");
        this.CurrentSession.FireEvent("FollowupDeleted");
    }
    DoneFollowupClicked() {
        this.Done = true;
        this.DoneDateTime = DateTool.GetCurrentDateTimeAsUtc();
        this.UIProperties.SetEnabled("Date", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("Notes", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("OwnerUserId", this.ObjectTableName, false);
        this.father.Height += 110;
    }
    UnDoneFollowupClicked() {
        this.Done = false;
        this.DoneDateTime = null;
        this.DoneNote = null;
        this.UIProperties.SetEnabled("Date", this.ObjectTableName, true);
        this.UIProperties.SetEnabled("Notes", this.ObjectTableName, true);
        this.UIProperties.SetEnabled("OwnerUserId", this.ObjectTableName, true);
        this.father.Height -= 110;
    }

    DateNumericButtonClicked(isIncreas: boolean) {
        if (this.Date == null) {
            this.Date = DateTool.GetCurrentDateAsUtc();
        }

        else {
            var day: number = isIncreas ? 1 : -1;
            var myDateParts = DateTool.GetDateParts(this.Date);

            var dateObject = new Date();
            dateObject.setUTCMonth(0);
            dateObject.setUTCDate(1);
            dateObject.setUTCFullYear(myDateParts.Year);
            dateObject.setUTCMonth((myDateParts.Month - 1));
            dateObject.setUTCDate(myDateParts.Day + day);
            dateObject.setUTCHours(myDateParts.Hours);
            dateObject.setUTCMinutes(myDateParts.Minutes);
            dateObject.setUTCSeconds(myDateParts.Seconds);
            dateObject.setUTCMilliseconds(myDateParts.Milliseconds);
            this.Date = dateObject;
        }
    }
    DoneDateNumericButtonClicked(isIncreas: boolean) {
        if (this.DoneDateTime == null) {
            this.DoneDateTime = DateTool.GetCurrentDateAsUtc();
        }

        else {
            var day: number = isIncreas ? 1 : -1;
            var myDateParts = DateTool.GetDateParts(this.Date);

            var dateObject = new Date();
            dateObject.setUTCMonth(0);
            dateObject.setUTCDate(1);
            dateObject.setUTCFullYear(myDateParts.Year);
            dateObject.setUTCMonth((myDateParts.Month - 1));
            dateObject.setUTCDate(myDateParts.Day + day);
            dateObject.setUTCHours(myDateParts.Hours);
            dateObject.setUTCMinutes(myDateParts.Minutes);
            dateObject.setUTCSeconds(myDateParts.Seconds);
            dateObject.setUTCMilliseconds(myDateParts.Milliseconds);
            this.DoneDateTime = dateObject;
        }
    }

    CancelButtonClicked() {
        this.RejectChanges();
        this.father.CloseAddEdit();
    }
    OkButtonClicked() {
        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);

        if (this.Notes) {
            if (this.Notes.length > 1000) {
                errors.push("Notes field must be less than 1000 and more than 0");
            }
        }

        if (errors.length == 0) {

            if (this.Done == true) {
                this.CurrentSession.FireEvent("FollowupDeleted");
            }

            this.father.CloseAddEdit();
            this.CurrentSession.FireEvent("FollowupsChanged");
        }
    }

    private myCloner: Cloner;
    private Clone() {
        this.myCloner = new Cloner(this);
        this.myCloner.AddField('Date');
        this.myCloner.AddField('Notes');
        this.myCloner.AddField('OwnerUserId');
        this.myCloner.AddField('Done');
        this.myCloner.AddField('DoneNote');
        this.myCloner.AddField('DoneDateTime');
        this.myCloner.AddEntity(this.EntityPM);
        this.myCloner.AddEntity(this.EntityPM.EntityParentPM);
    }
    RejectChanges() {
        this.myCloner.RejectChanges();
    }
}
