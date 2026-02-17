import { Component } from '@angular/core';
import { EntityArgs } from '../../../../../Infrastructure/DataContracts/EntityArgs';
import { AppTool, ArrayTool, DateTool } from '../../../../../Infrastructure/Tools';
import { FeatureLocator } from '../../../../../Infrastructure/Utilities/FeatureLocator';
import { SessionLocator } from '../../../../../Infrastructure/Utilities/SessionLocator';
import { LogTab } from '../../../../../Infrastructure/Components/LogitudeComponents/LogTabsComponent';
import { TextCodeTranslator } from '../../../../../Infrastructure/Utilities/TextCodeTranslator';
import { DeclarationPM } from '../../../../../Customs/EntityPMs/DeclarationPM';
import { NotificationReplyPM } from '../../../../../Customs/EntityPMs/NotificationReplyPM';
import { NotificationPM } from '../../../../../Customs/EntityPMs/NotificationPM';
import { EntityPMService } from '../../../../../Infrastructure/Services/EntityPMService';
import { MessageToAgentRequestParams } from '../../../../../Customs/DataContract/RequestParams/MessageToAgentRequestParams';
import { BaseComponent } from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ObservableCollection } from '../../../../../Infrastructure/Utilities/ObservableCollection';
import { ServiceResponse } from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import { ObjectTablePM } from '../../../../../Infrastructure/EntityPMs/ObjectTablePM';
import { NotificationListService } from '../../../../../Customs/Services/StandardLists/NotificationListService';
import { NotificationPMService } from '../../../../../Customs/Services/StandardPMs/NotificationPMService';
import { NotificationList } from '../../../../../Customs/EntityLists/NotificationList';
import { NotificationWebService } from '../../../../../Customs/Services/WebServices/NotificationWebService';
import { CustomSendOptionsArgs } from '../../../../../Customs/DataContract/RequestParams/RequestParamsBase';
import { LogitudeWindow } from '../../../../../Controls/Windows/LogitudeWindow';
import { CustomMessageProgressComponent } from '../../../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent';
import { GroupByPipe } from '../../../../../Infrastructure/Pipes/GroupByPipe';
import {EntityResourceService} from '../../../../../Infrastructure/Services/EntityResourceService';
import { DeclarationMenuButtonsHandler } from 'Customs/Components/MenuButtons/DeclarationMenuButtonsHandler';
declare var window: any;

@Component({
    
    templateUrl: './NotificationReplyTabComponent.html',
    selector: 'NotificationReplyTabComponent',
})

export class NotificationReplyTabComponent extends BaseComponent {
    public EntityPM: NotificationPM = null;
    public ObjectTableName = "Customs.Declaration";
    public DataContext: this;
    public ObjectTableId: string;
    public CurrentEditComponentId: string;
    public messageBorderText: string = TextCodeTranslator.Translate("Customs.Declaration.O.NoNotificationsReceived");

    public entityList: NotificationList;
    public declarationPM: DeclarationPM = null;
    private messageBorderVisibility: boolean = true;
  
    //List<NotificationPM> notifications;
    public NotificationsList: DeclarationNotificationItemViewModel[] = [];
    public NotificationsGroupsList: NotificationGroupHeaderViewModel[] = [];

    public notificationWebService: NotificationWebService = new NotificationWebService();
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs, private EntityResourceService: EntityResourceService) {
        super();
        
        if (!AppTool.IsNullOrEmpty(entityArgs)) {
            this.EntityResourceService.getEntityResourceByTableName("Customs.Declaration").subscribe((response: any) => {
                this.EntityResourceService.getEntityResourceByTableName("Customs.Notification").subscribe((response: any) => {
                    this.EntityResourceService.getEntityResourceByTableName("Customs.NotificationReply").subscribe((response: any) => {
                        this.declarationPM = entityArgs.EntityPM;
                        this.Listen();
                        this.LoadNotificationReplies();
                    });
                });
            });
        }
    }

    private Listen() {
        if (this.CurrentSession.CurrentEditComponent != null) {

            this.CurrentEditComponentId = this.CurrentSession.CurrentEditComponent.ComponentId;
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(
                this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                    if (isSaveSuccess) {
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                    }
                })
            );
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(

                this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                    if (isLoadSuccess) {
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                    }
                })
            );

            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(
                this.CurrentSession.CurrentEditComponent.TabSelected.subscribe((tabCode: string) => {
                    if (this.CurrentEditComponentId == this.CurrentSession.CurrentEditComponent.ComponentId) {
                        if (tabCode == "DCNT") {
                            this.LoadNotificationReplies();
                        }
                    }
                })
            );
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(
                this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((tabCode: string) => {
                            this.LoadNotificationReplies();
                      
                })
            );
  
        }
    }

    LoadNotificationReplies() {
        var objecttable: ObjectTablePM = window.ObjectTables.filter(d => d.Name == "Customs.Declaration")[0];
        this.notificationWebService.GetNotificationsByDefinitionCode(objecttable.Id, this.declarationPM.Id, this.declarationPM.Tenant)
            .subscribe((myResponse: ServiceResponse) => {
                this.GetNotificationsByDefinitionCodeOp_Completed(myResponse, false);
            });
    }

    GetNotificationsByDefinitionCodeOp_Completed(myResponse: ServiceResponse, sourceIsCostomFile: boolean) {
        this.MessageBorderVisibility = true;

        if (myResponse.Result != null && myResponse.Result.length > 0) {
            this.MessageBorderVisibility = false;
            this.FillNotificationData(myResponse.Result);
        }
    }

    FillNotificationData(notificationResponseList: NotificationPM[]) {

        if (notificationResponseList != null) {
            //Group list by CreateDate
            this.NotificationsGroupsList = [];
            notificationResponseList.forEach((notificationItem: NotificationPM) => {
                let notificationsList: DeclarationNotificationItemViewModel[] = [];
                
                let listCreate = this.NotificationsGroupsList.filter(item => item.CreateDate == notificationItem.CreateDate);
                let myGroupCreate: NotificationGroupHeaderViewModel = null;
                if (!AppTool.IsNullOrEmpty(listCreate) && listCreate.length > 0) {
                    myGroupCreate = listCreate[0];
                } 
                if (AppTool.IsNullOrEmpty(myGroupCreate)) {
                    myGroupCreate = new NotificationGroupHeaderViewModel(notificationItem, notificationsList);
                }
                myGroupCreate.NotificationsList.push(new DeclarationNotificationItemViewModel(notificationItem, this));
                this.NotificationsGroupsList.push(myGroupCreate);
            });

            //Order list by CreateDate
            this.NotificationsGroupsList.sort((a, b) => new Date(b.CreateDate).getTime() - new Date(a.CreateDate).getTime())



            // this.NotificationsGroupsList.sort(
            //     (a, b) => { return (DateTool.GetDateFromDate(a.CreateDate) === DateTool.GetDateFromDate(b.CreateDate)) ? 0 : (DateTool.GetDateFromDate(a.CreateDate) < DateTool.GetDateFromDate(b.CreateDate)) ? -1 : 1 });
        }
    }

    public get MessageBorderVisibility() { return this.messageBorderVisibility; }
    public set MessageBorderVisibility(newValue: boolean) { this.messageBorderVisibility = newValue; }

    public get MessageBorderText() { return this.messageBorderText; }
    public set MessageBorderText(newValue: string) { this.messageBorderText = newValue; }

}

export class DeclarationNotificationItemViewModel extends BaseComponent {
    public ObjectTableName = "Customs.Declaration";
    public DataContext = this;
    public entityPM: NotificationPM;
    public parent: NotificationReplyTabComponent;
    public ResponseList: NotificationReplyPM[] = [];
    public notificationData: string;
    public senderName: string;
    public notificationReply: string;
    public withAnswerGridVisibility: boolean = false;

    notificationPMService: NotificationPMService = new NotificationPMService();
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(notificationPM: NotificationPM, trigger: NotificationReplyTabComponent) {
        super();

        this.entityPM = notificationPM;
        this.parent = trigger;
        this.NotificationData = this.entityPM.Description;// + Environment.NewLine;        
        let senderName = this.entityPM?.SenderName?.trim();
        this.SenderName = senderName? `${TextCodeTranslator.Translate('Customs.Notification.O.PrivateName')} ${senderName}`:'';


        if (this.entityPM.NotificationRplies.length == 0) {
            this.WithAnswerGridVisibility = false;
        }
        else {
            this.WithAnswerGridVisibility = true;
            for (let reply of this.entityPM.NotificationRplies) {
                this.ResponseList.push(reply);
            }

            //Order list by CreateDate
            this.ResponseList.sort(
                (a, b) => { return (DateTool.GetDateFromDate(a.ReplyDateTime) === DateTool.GetDateFromDate(b.ReplyDateTime)) ? 0 : (DateTool.GetDateFromDate(a.ReplyDateTime) < DateTool.GetDateFromDate(b.ReplyDateTime)) ? -1 : 1 });
        }
    }

    public get NotificationData() { return this.notificationData; }
    public set NotificationData(newValue: string) { this.notificationData = newValue; }

    public get SenderName() { return this.senderName; }
    public set SenderName(newValue: string) { this.senderName = newValue; }

    public get NotificationReply() { return this.notificationReply; }
    public set NotificationReply(newValue: string) { this.notificationReply = newValue; }

    public get WithAnswerGridVisibility() { return this.withAnswerGridVisibility; }
    public set WithAnswerGridVisibility(newValue: boolean) { this.withAnswerGridVisibility = newValue; }

    OnCustomSendOptionsButtonClick(customSendOptionsArgs: CustomSendOptionsArgs) {

        if (AppTool.IsNullOrEmpty(this.NotificationReply)) {
            var validationErrors: string[] = [];
            validationErrors.push(TextCodeTranslator.Translate("Customs.Notification.O.NoReplyEntered"));
            var windowArgs: any = {};
            windowArgs.Errors = validationErrors;
            windowArgs.ComponentHeight = '228px';
            var logWindow = new LogitudeWindow();
            logWindow.Width = 500;
            logWindow.Height = 300;
            logWindow.Title = TextCodeTranslator.Translate("Customs.Notification.O.NotificationReplySendErrors");
            logWindow.ShowCloseButton = false;
            logWindow.WindowArgs = windowArgs;
            logWindow.Show('./CustomsModules/CustomsControls/Components/CustomsErrorsComponent');
            return;
        }

        this.AddNewNotificationReplay(customSendOptionsArgs);
    }

    AddNewNotificationReplay(customSendOptionsArgs: CustomSendOptionsArgs) {

        var newNotificationReplyPM = new NotificationReplyPM(this.entityPM);
        newNotificationReplyPM.NotificationId = this.entityPM.Id;
        newNotificationReplyPM.Tenant = this.entityPM.Tenant;
        newNotificationReplyPM.Line = (ArrayTool.Max(this.entityPM.NotificationRplies, "Line") + 1);
        newNotificationReplyPM.RepliedByUserId = SessionLocator.LoggedUserId;
        newNotificationReplyPM.ReplyDateTime = new Date();
        newNotificationReplyPM.ResponseToCustoms = this.NotificationReply;

        this.ResponseList.push(newNotificationReplyPM);
        this.entityPM.AddNotificationReply(newNotificationReplyPM);

        let entityPMService = new EntityPMService();
        entityPMService.update("Customs.Notification", this.entityPM).then((res: any) => {
            res.subscribe((myResponse: ServiceResponse) => {
                this.CurrentSession.StopBusyIndicator();
                if (myResponse.HasError) {
                    this.CurrentSession.StopBusyIndicator();
                }
                else {
                    this.SendNotificationReplay(customSendOptionsArgs);
                    this.NotificationReply = null;
                }
            }, error => {
                this.CurrentSession.StopBusyIndicator();
            });
        });
    }

    SendNotificationReplay(customSendOptionsArgs: CustomSendOptionsArgs) {

        var currRequestParams = new MessageToAgentRequestParams();
        currRequestParams.LoggingEnabled = true;
        currRequestParams.LoggingUserId = SessionLocator.LoggedUserId;
        currRequestParams.RequestVIA = customSendOptionsArgs.RequestVIA;
        currRequestParams.ForcePersonalSign = customSendOptionsArgs.ForcePersonalSign;
        currRequestParams.Tenant = SessionLocator.Tenant;
        currRequestParams.NotificationId = this.entityPM.Id;
        currRequestParams.DeclarationId = this.parent.declarationPM.Id;

        CustomMessageProgressComponent
            .ShowProgressBar(this.CurrentSession,currRequestParams.PBId,
                TextCodeTranslator.Translate("Customs.Declaration.O.SendReplyMessage"), true)
            .then((res) => {
                //this.responseData = res;
                //this.OnMassageDisplayMethod();
            }
            ).catch((err) => {
                //this.ValidationErrorsList.push(err);
            });

        this.parent.notificationWebService.PostSendNotificationReplyRequest(currRequestParams)
            .subscribe((myServiceResponse: ServiceResponse) => {
            });

        //this.parent.FillNotificationData();

        //NotificationData = null;
        //NotificationData = entityPM.Description + Environment.NewLine + entityPM.ResponseNotes + Environment.NewLine;

    }
}

export class NotificationGroupHeaderViewModel extends BaseComponent {
    public ObjectTableName = "Customs.Declaration";
    public DataContext = this;
    public entityPM: NotificationPM;
    public NotificationsList: DeclarationNotificationItemViewModel[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(notificationPM: NotificationPM, notificationList: DeclarationNotificationItemViewModel[]) {
        super();

        this.entityPM = notificationPM;
        this.NotificationsList = notificationList;
    }

    public get CreateDate() { return this.entityPM.CreateDate; }
    public set CreateDate(newValue: Date) { this.entityPM.CreateDate = newValue; }

}
