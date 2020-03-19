declare var window: any;
import {Component, OnDestroy} from '@angular/core';
import {AppTool, DateTool} from '../../../Tools';
import {TextCodeTranslator} from '../../../Utilities/TextCodeTranslator';
import {SessionLocator} from '../../../Utilities/SessionLocator';
import {FeatureLocator} from '../../../Utilities/FeatureLocator';
import {QuoteList} from '../../../../Quote/EntityLists/QuoteList';
import {ShipmentList} from '../../../../Shipment/EntityLists/ShipmentList';
import {InfrastructureDomainService} from  '../../../Services/InfrastructureDomainService';
import {ServiceResponse} from '../../../DataContracts/ServiceResponse';

@Component({
    selector: "MainMenuFollowups",
    moduleId: module.id,
    templateUrl: './MainMenuFollowups.html',
    inputs: ['ObjectTableId', 'IsMainSidebarCollapsed'],
})

export class MainMenuFollowups implements OnDestroy {
    public IconPath: string = "./_Resources/Images/Icons/Followups/Followup.png";
    public ItemsSource: MainMenuFollowupItem[] = [];
    public ObjectTableName: string;
    public BackButtonLabel: string;
    public IsMainSidebarCollapsed: boolean = false;
    private DomainService: InfrastructureDomainService;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        this.DomainService = new InfrastructureDomainService();
        this.Listen();        
    }

    private FollowupsChangedEvent: any = null;
    Listen() {
        if (!this.FollowupsChangedEvent) {
            this.FollowupsChangedEvent = this.CurrentSession.SessionEvent.subscribe(s => {
                if (s == "FollowupsChangedMainMenu") {
                    this.LoadData();
                }
            });
        }
    }
    ngOnDestroy() {
        AppTool.KillEventEmitter(this.FollowupsChangedEvent);
    }

    private objectTableId: string = null;
    get ObjectTableId() { return this.objectTableId; }
    set ObjectTableId(value: string) {
        if (this.objectTableId != value) {
            this.objectTableId = value;
            this.LoadData();
        }
    }

    LoadData() {
        this.ItemsSource = [];
        this.SetIconPath();

        var objectTable = window.ObjectTables.filter(x => x.Id === this.ObjectTableId)[0];
        if (objectTable) {
            this.ObjectTableName = objectTable.Name;

            if (FeatureLocator.HasFeaturePermession(this.ObjectTableName, "READ")) {
                this.DomainService.GetMainMenuFollowups(this.ObjectTableName).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {

                        var list: MainMenuFollowupItem[] = [];

                        if (this.ObjectTableName == "Quote") {
                            this.BackButtonLabel = TextCodeTranslator.Translate("General.MH.Quotes");
                            
                            var myQuotes: QuoteList[] = myResponse.Result;
                            myQuotes.forEach(item => {
                                var newListItem = new MainMenuFollowupItem(this);
                                newListItem.Name = item.FollowUpType;
                                newListItem.Notes = item.FollowUpNotes;
                                newListItem.Date = item.FollowUpDate;
                                newListItem.EntityId = item.Id;
                                newListItem.EntityNumber = item.QuoteNumber;
                                newListItem.Initialize();
                                list.push(newListItem);
                            });
                        }

                        else {
                            this.BackButtonLabel = TextCodeTranslator.Translate("General.MH.Operations");

                            var myShipments: ShipmentList[] = myResponse.Result;
                            myShipments.forEach(item => {
                                var newListItem = new MainMenuFollowupItem(this);
                                newListItem.Name = item.FollowUpType;
                                newListItem.Notes = item.FollowUpNotes;
                                newListItem.Date = item.FollowUpDate;
                                newListItem.EntityId = item.Id;
                                newListItem.EntityNumber = item.ShipmentNumber;
                                newListItem.Initialize();
                                list.push(newListItem);
                            });
                        }

                        this.ItemsSource = list;
                        this.SetIconPath();
                    }
                });
            }
        }
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
}
export class MainMenuFollowupItem {
    public ItemId: string = null;
    public ItemTooltipId: string = null;
    public Name: string = null;
    public Date: Date = null;
    public Notes: string = null;
    public EntityId: string = null;
    public EntityNumber: string = null;
    public IsOld: boolean = false;
    public Background: string = "white";
    public ListItemHeight: number = 70;
    public TooltipHeight: number = 130;
    public TooltipWidth: number = 270;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private father: MainMenuFollowups) {
        var idIndex = this.CurrentSession.GetNewId("FollowupItem");
        this.ItemId = "FollowupItem_" + idIndex;
        this.ItemTooltipId = "FollowupItemTooltip_" + idIndex;
    }

    Initialize() {
        if (this.Date) {
            if (DateTool.GetDateParts(this.Date).DateTicks < DateTool.GetCurrentDateAsUtc().valueOf()) {
                this.IsOld = true;
                this.Background = "#F7E3E3";
            }
        }

        this.SetIconPath();
    }

    public IconPath: string;
    public IconWidth: number;
    public IconHeight: number;
    SetIconPath() {
        var iconPath = "./_Resources/Images/Icons/Followups/Document.png";
        var iconWidth = 11;
        var iconHeight = 14;

        if (this.Name) {
            var name = this.Name.toLowerCase();

            if (name.indexOf("arrived") > -1 || name.indexOf("departed") > -1 || name.indexOf("departure") > -1 || name.indexOf("arrival") > -1) {
                iconPath = "./_Resources/Images/Icons/Followups/Routing.png";
                iconWidth = 20;
                iconHeight = 14;
            }

            else if (name.indexOf("reminder") > -1 || name.indexOf("arranged") > -1) {
                iconPath = "./_Resources/Images/Icons/Followups/Reminder.png";
                iconWidth = 13;
                iconHeight = 13;
            }
        }

        this.IconPath = iconPath;
        this.IconWidth = iconWidth;
        this.IconHeight = iconHeight;
    }

    public IsShowTooltip: boolean = false;
    ShowTooltip(isShowTooltip: boolean) {
        if (!AppTool.IsNullOrEmpty(this.Notes)) {
            if (isShowTooltip) {
                var item = document.getElementById(this.ItemId);
                var itemRect = item.getBoundingClientRect();
                document.getElementById(this.ItemTooltipId).style.top = (itemRect.top - (this.TooltipHeight / 2) + (this.ListItemHeight / 2)) + 'px';
                document.getElementById(this.ItemTooltipId).style.left = (itemRect.left + 130) + 'px';
            }

            this.IsShowTooltip = isShowTooltip;
        }
    }

    ViewEntityClicked() {
        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(cmpRef => {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({ EntityId: this.EntityId, ObjectTableName: this.father.ObjectTableName, BackButtonLabel: this.father.BackButtonLabel });
            });
    }
}
