declare var window: any;

import {Component, Input} from '@angular/core';
import {NotificationExtendedListService} from '../../Customs/Services/ExtendedLists/NotificationExtendedListService';
import {SessionInfo} from '../../Infrastructure/Utilities/SessionInfo';
import {NotificationPM} from '../../Customs/EntityPMs/NotificationPM';
import {ObservableCollection} from '../../Infrastructure/Utilities/ObservableCollection';
import { DateTool, AppTool} from '../../Infrastructure/Tools';
import {SessionLocator} from '../../Infrastructure/Utilities/SessionLocator';
import {LogitudeWindow} from '../Windows/LogitudeWindow';
import {EntityListService} from '../../Infrastructure/Services/EntityListService';
import {Guid} from '../../Infrastructure/Utilities/Guid';
import { NotificationWebService } from '../../Customs/Services/WebServices/NotificationWebService';
import {MessageWindow} from '../Windows/MessageWindow';
import {TextCodeTranslator} from '../../Infrastructure/Utilities/TextCodeTranslator';
import { CustomsCollateralPMService } from '../../Customs/Services/StandardPMs/CustomsCollateralPMService';
import { DeclarationExtendedListService } from '../../Customs/Services/ExtendedLists/DeclarationExtendedListService';
import {EntityResourceService} from '../../Infrastructure/Services/EntityResourceService';

@Component({
    selector: 'NotificationBellComponent',
    
    templateUrl: './NotificationBellComponent.html',
})


export class NotificationBellComponent {

    LayoutDirection: string = 'ltr';
    ItemsSource: NotificationBellLine[];
    Notifications: NotificationPM[];
    notificationExtendedListService: NotificationExtendedListService = new NotificationExtendedListService();
    notificationWebService: NotificationWebService = new NotificationWebService();
    customsCollateralPMService: CustomsCollateralPMService = new CustomsCollateralPMService();
    declarationExtendedListService: DeclarationExtendedListService = new DeclarationExtendedListService();
    IsVisibile: boolean;
    @Input() ParentComponent;
    DataContext = this;
    PreventSelect: boolean = false;
    constructor(private entityResourceService: EntityResourceService) {
        entityResourceService.getEntityResourceByTableName("Customs.Notification").subscribe((response:any) => {
            entityResourceService.getEntityResourceByTableName("Customs.PaymentOrder").subscribe((response:any) => {
                entityResourceService.getEntityResourceByTableName("Customs.PaymentOrderLine").subscribe((response:any) => {
                    entityResourceService.getEntityResourceByTableName("Customs.PaymentOrderMethod").subscribe((response:any) => {
                        entityResourceService.getEntityResourceByTableName("Customs.PaymentOrderProtestReason").subscribe((response:any) => {
                            entityResourceService.getEntityResourceByTableName("Customs.CustomsSetting").subscribe((response:any) => {

                                this.IsVisibile = true;
                                this.GetOpenNotificationsCount();
                                this.GetNotifications();
                            });
                        });
                    });
                });
            });
        });
    }
    GetNotifications() {
        this.ItemsSource = [];
        this.Notifications = [];
        this.notificationExtendedListService.GetGetTopTenNotifications(SessionInfo.LoggedUserId).subscribe((response:any) => {


            if (response) {
                if (!response.HasError) {
                    this.Notifications = response.Result;
                    this.BuildList();

                    this.notificationExtendedListService.PutNotificationBadjCount(new NotificationPM()).subscribe((response:any) => {


                    });
                }
                else {
                    var msg = new MessageWindow();
                    var s: string[] = response.ErrorsArray;
                    msg.Show(s[0]);

                }
            }
        });

    }

    BuildList() {
        this.ItemsSource = [];
        for (let item of this.Notifications.filter(d => !d.IsClosedByAssignee)) {
            this.ItemsSource.push(new NotificationBellLine(item, this));
        }
    }

    count: number;

    GetOpenNotificationsCount() {
        this.notificationExtendedListService.GetOpenNotificationsCount(SessionInfo.LoggedUserId).subscribe((response:any) => {


            if (response) {
                if (!response.HasError) {
                    this.count = response.Result;
                }
                else {
                    
                    var msg = new MessageWindow();
                    var s: string[] = response.ErrorsArray;
                        msg.Show(s[0]);
                    
                }
            }
        });
    }

    SelectedLine(item: NotificationBellLine) {

        // this.CurrentSession.CloseNotificationBellEvent.emit({  });
        if (!this.PreventSelect) {

            var selected = item.entity;

            if (selected) {

                var customEditIdentityKey = Guid.newGuid();
                var control = null;

                var logitudeWindow = new LogitudeWindow();
                //logitudeWindow.ZIndex = 5;
                var currentScreenCode = "";

                switch (selected.ObjectTableName) {

                    case "Customs.Declaration":
                        {
                            switch (selected.NotificationDefinitionCode) {
                                case "3050N":
                                case "3050C":
                                case "3050U":
                                case "3052P":
                                    {
                                        currentScreenCode = "DCPO";
                                        break;

                                    }

                                case "190N":
                                case "190U":
                                case "196E":
                                case "190C":
                                    {
                                        currentScreenCode = "DCPC";
                                        break;

                                    }

                                case "2470N":
                                case "2470C":
                                case "2470P":
                                case "5018N":
                                case "8400C":
                                case "5117N":
                                case "8400A":
                                case "5101C":
                                case "5101D":
                                case "5101G":
                                //case "5101I":
                                case "5101S":
                                case "5101T":
                                case "5101U":
                                case "5101B":
                                case "5107N":
                                case "2754N":
                                case "70N":
                                case "70C":
                                case "60A":
                                case "5117C":
                                case "5117W":
                                case "5117D":
                                case "5117A":
                                case "5117P":
                                    {
                                      //  var tab = window.ObjectTableTabs.find(d => d.ObjectTableId == selected.ObjectTableId && d.IndexOrder == 0);
                                        var tab;
                                        var tabs = window.ObjectTableTabs.find(d => d.ObjectTableId == selected.ObjectTableId);
                                        if (tabs.lenght > 0) {
                                            tabs = tabs.sort((n1, n2) => {
                                                if (n1.IndexOrder > n2.IndexOrder) {
                                                    return 1;
                                                }

                                                if (n1.IndexOrder < n2.IndexOrder) {
                                                    return -1;
                                                }

                                                return 0;
                                            });

                                            tab = tabs[0];
                                        }
                                        else {
                                            tab = tabs;
                                        }

                                        if (tab) {
                                            currentScreenCode = tab.Code;
                                        }
                                        else {
                                            var msg = new MessageWindow();
                                            //msg.ZIndex = 5;
                                            msg.Show(TextCodeTranslator.Translate("Customs.General.O.WrongEntityName"));
                                        }
                                        break;
                                    }

                                case "5101N":
                                case "5101R":
                                case "5101A":
                                case "5101E":
                                    {
                                        currentScreenCode = "DCNT";
                                        break;
                                    }

                                case "8215A":
                                case "8215D":
                                case "8215C":
                                    {
                                        currentScreenCode = "DCCA";
                                        break;
                                    }

                                case "8227N":
                                case "8227D":
                                case "8227A":
                                case "8228D":
                                case "8228A":
                                    {
                                        currentScreenCode = "DCCD";
                                        break;
                                    }
                                case "1812N":
                                case "1812U":
                                case "2020N":
                                case "2000N":
                                case "2753A":
                                case "5110N":
                                case "5108N":
                                    {
                                        currentScreenCode = "DCTP";
                                        break;

                                    }

                                default:
                                    {

                                        var msg = new MessageWindow();
                                        //msg.ZIndex = 5;
                                        msg.Show("לא נמצאה ישות להצגה");
                                        break;
                                    }


                            }
                            break;
                        }

                    case "Customs.PaymentOrder":
                        {
                            switch (selected.NotificationDefinitionCode) {
                                case "3050N":
                                case "3050C":
                                case "3050U":
                                case "3052P":
                                    {

                                        var tab = window.ObjectTableTabs.find(d => d.ObjectTableId == selected.ObjectTableId && d.IndexOrder == 0);

                                        if (tab) {
                                            currentScreenCode = tab.Code;
                                        }

                                        else {
                                            var msg = new MessageWindow();
                                            //msg.ZIndex = 5;
                                            msg.Show(TextCodeTranslator.Translate("Customs.General.O.WrongEntityName"));
                                        }

                                        break;

                                    }

                                default:
                                    {
                                        var msg = new MessageWindow();
                                        //msg.ZIndex = 5;
                                        msg.Show("לא נמצאה ישות להצגה");
                                        break;
                                    }


                            }
                            break;

                        }



                    case "Customs.PhysicalCheck":
                        {
                            switch (selected.NotificationDefinitionCode) {
                                case "190N":
                                case "190U":
                                case "196E":
                                case "190C":
                                    {
                                        var tab = window.ObjectTableTabs.find(d => d.ObjectTableId == selected.ObjectTableId && d.IndexOrder == 0);

                                        if (tab) {
                                            currentScreenCode = tab.Code;
                                        }

                                        else {
                                            var msg = new MessageWindow();
                                            //msg.ZIndex = 5;
                                            msg.Show(TextCodeTranslator.Translate("Customs.General.O.WrongEntityName"));
                                        }
                                        break;

                                    }

                                default:
                                    {
                                        var msg = new MessageWindow();
                                        //msg.ZIndex = 5;
                                        msg.Show("לא נמצאה ישות להצגה");
                                        break;
                                    }

                            }
                            break;

                        }

                    case "Customs.CustomsCollateral":
                        {
                            switch (selected.NotificationDefinitionCode) {
                                case "8213N":
                                case "8211N":
                                case "8211U":

                                    {
                                        this.entityResourceService.getEntityResourceByTableName("Customs.Declaration").subscribe((response:any) => {
                                            this.entityResourceService.getEntityResourceByTableName("Customs.CustomsCollateral").subscribe((response:any) => {
                                                this.entityResourceService.getEntityResourceByTableName("Customs.CustomsCollateralsAnswer").subscribe((response:any) => {
                                                    this.entityResourceService.getEntityResourceByTableName("Customs.CustomsCollateralsCondition").subscribe((response:any) => {


                                                        this.customsCollateralPMService.get(selected.EntityId).subscribe((response: any) => {

                                                            var result = response.Result;
                                                            console.log("[response] customsCollateralPMService.get", result);
                                                            if (!AppTool.IsNullOrEmpty(result)) {
                                                              control = './CustomsModules/CustomsCollateral/Components/CustomsCollateralComponent';
                                                                logitudeWindow.Title = TextCodeTranslator.Translate("Customs.Declaration.O.EditCustomsCollateral");
                                                                logitudeWindow.WindowArgs = { CurrentEntity: result };
                                                                logitudeWindow.Height = 730;
                                                                logitudeWindow.Width = 660;
                                                                //logitudeWindow.ZIndex = 5;
                                                                logitudeWindow.Show(control);

                                                                var Ids: string[] = [];
                                                                Ids.push(selected.Id);
                                                                Ids.push(selected.Id);
                                                                this.notificationWebService.SetNotificationsStatus(Ids, "Read").subscribe((res: any) => {
                                                                    this.SetStatusCompleted(selected.Id, event);

                                                                });
                                                            } else {
                                                                console.log("No collateral found!!!!!!");
                                                                return;
                                                            }



                                                        });
                                                    });
                                                });
                                            });
                                        });
                                        break;
                                    }

                                default:
                                    {
                                        var msg = new MessageWindow();
                                        //msg.ZIndex = 5;
                                        msg.Show("לא נמצאה ישות להצגה");
                                        break;
                                    }

                            }
                            break;

                        }



                    default:
                        {
                            if (!AppTool.IsNullOrEmpty(selected.Reference1Number)) {
                                this.declarationExtendedListService.GetSingleDeclarationByCustomFileNo(selected.Reference1Number).subscribe((res: any) => {
                                    var declaration = res.Result;
                                    console.log("[reponse] GetSingleDeclarationByCustomFileNo: ", declaration);
                                    if (!AppTool.IsNullOrEmpty(declaration)) {
                                        selected.ObjectTableName = "Customs.Declaration";
                                        currentScreenCode = "DEGC";
                                        this.EditEntity(selected.ObjectTableName, selected.EntityId, null, currentScreenCode);


                                        var Ids: string[] = [];
                                        Ids.push(selected.Id);
                                        Ids.push(selected.Id);
                                        this.notificationWebService.SetNotificationsStatus(Ids, "Read").subscribe((res: any) => {
                                            this.SetStatusCompleted(selected.Id, event);
                                        });

                                    }
                                });
                            }
                            else {

                                var msg = new MessageWindow();

                                msg.Show(TextCodeTranslator.Translate("Customs.General.O.WrongEntityName"));
                            }
                            break;
                        }

                }

                if (!AppTool.IsNullOrEmpty(control)) {
                    if (selected.ObjectTableName == "Customs.ProceduralFault") {
                        logitudeWindow.Height = 400;
                        logitudeWindow.Width = 820;
                        logitudeWindow.ShowCloseButton = true;
                    }
                    else {
                        logitudeWindow.Height = 730;
                        logitudeWindow.Width = 660;
                    }

                    logitudeWindow.Show(control);


                    var Ids: string[] = [];
                    Ids.push(selected.Id);
                    Ids.push(selected.Id);
                    this.notificationWebService.SetNotificationsStatus(Ids, "Read").subscribe((res: any) => {
                        this.SetStatusCompleted(selected.Id, event);

                    });

                }
                else {
                    if (!AppTool.IsNullOrEmpty(currentScreenCode)) {
                        this.EditEntity(selected.ObjectTableName, selected.EntityId, null, currentScreenCode);

                        var Ids: string[] = [];
                        Ids.push(selected.Id);
                        Ids.push(selected.Id);
                        this.notificationWebService.SetNotificationsStatus(Ids, "Read").subscribe((res: any) => {
                            this.SetStatusCompleted(selected.Id, event);

                        });


                    }
                }


            }
            this.ParentComponent.IsControlVisibile = false;
        }
        this.PreventSelect = false;







    }


    public EditEntity(objectTableName: string, entityId: string, windowTitle: string, defaultSelectedTabCode: string) {

        var editWindow = new LogitudeWindow();

        editWindow.ShowHeaderButtons = true;
        editWindow.Title = windowTitle;
        editWindow.Height = 770;
        editWindow.Width = 1500;

        editWindow.ShowEditComponent(entityId, objectTableName, defaultSelectedTabCode);
        editWindow.WindowClosed.subscribe((res:any) => {



        });

    }

    SetStatusCompleted(id: string, $event: any) {


        //this.entityListService.getSingle(id, this.ObjectTableName).then((res: any) => {
        //    //var re = res;
        //    res.subscribe((aa: any) => {
        //        $event.BackFromEdit.emit({ Data: aa.Result, rowIndex: $event.rowIndex });
        //    })
        //});
    }
}

export class NotificationBellLine {
    entity: NotificationPM;
    Parent: NotificationBellComponent
    notificationExtendedListService: NotificationExtendedListService = new NotificationExtendedListService();
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(entityPM: NotificationPM, parent: NotificationBellComponent) {
        this.entity = entityPM;
        this.Parent = parent;
        if (this.entity.IsSeenByAssignee) {
            this.Background = "#F1F1F1";

        }
        else {
            this.Background = "white";
            this.IsSeenFontWeight = "bold";

        }

        if (this.entity.AssigneToNotificationTypeCode == "A") {
            this.IconeVisibility = true;
            this.BlueIconeVisibility = false;
        }
        else {
            this.IconeVisibility = false;
        }

        if (this.entity.NotificationDefinitionCode == "5101N" || this.entity.NotificationDefinitionCode == "5101R" || this.entity.NotificationDefinitionCode == "5101A" || this.entity.NotificationDefinitionCode == "5101E") {
            this.BlueIconeVisibility = true;
            this.IconeVisibility = false;

        }


        var valueDate = new Date(this.entity.DueDate.valueOf()).valueOf();
        var today = DateTool.GetCurrentDateAsUtc().valueOf();
        if (valueDate != null && valueDate < today) {
            this.datecolor = "#ff6a00";
            this.fontcolor = "#ffffff";
        }

        else {
            this.datecolor = "#E2E2E2";
            this.fontcolor = "#6E7172";
        }


        if (!AppTool.IsNullOrEmpty(this.entity.CustomerName) && !AppTool.IsNullOrEmpty(this.entity.Reference1Number)) {
            this.Reference1NumberWithCustomer = entityPM.Reference1Number + " * " + entityPM.CustomerName;
        }
        else if (AppTool.IsNullOrEmpty(this.entity.CustomerName) && !AppTool.IsNullOrEmpty(this.entity.Reference1Number)) {
            this.Reference1NumberWithCustomer = entityPM.Reference1Number;
        }

        else if (!AppTool.IsNullOrEmpty(entityPM.CustomerName) && AppTool.IsNullOrEmpty(entityPM.Reference1Number)) {
            this.Reference1NumberWithCustomer = entityPM.CustomerName;
        }



    }

    Reference1NumberWithCustomer: string;
    fontcolor: string;
    datecolor: string;
    IsSeenFontWeight: string;
    get NotificationDefinitionName() { return this.entity.NotificationDefinitionName; }
    get DueDate() { return this.entity.DueDate; }
    get Description() { return this.entity.Description; }
    get CreateDate() { return this.entity.CreateDate; }
    get NotificationDefinitionCode() { return this.entity.NotificationDefinitionCode; }
    Background: string;
    IconeVisibility: boolean;
    BlueIconeVisibility: boolean;



    ClosedByAssigneeClicked() {
        this.Parent.count -= 1;
        this.Parent.PreventSelect = true;
        this.Parent.ParentComponent.IsControlVisibile = true;
        this.entity.IsClosedByAssignee = true;
        this.Parent.BuildList();
        this.notificationExtendedListService.PutNotificationsStatus(this.entity).subscribe((response:any) => {
            if (response) {
                if (!response.HasError) {
                    this.Parent.PreventSelect = false;

                }
            }
        });



    }


}
