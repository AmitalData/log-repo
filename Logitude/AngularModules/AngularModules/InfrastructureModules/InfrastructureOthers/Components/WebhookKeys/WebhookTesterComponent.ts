import { Component } from '@angular/core';
import { WebhookKeysPM } from '../../../../Infrastructure/EntityPMs/WebhookKeysPM';
import { WebhookKeysExtendedPMService } from '../../../../Infrastructure/Services/ExtendedPMs/WebhookKeysExtendedPMService';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { AppTool, DateTool } from '../../../../Infrastructure/Tools';
import { Validator } from '../../../../Infrastructure/Validators/Validator';
import { MessageWindow } from '../../../../Controls/Windows/MessageWindow';

@Component({
    moduleId: module.id,
    templateUrl: './WebhookTesterComponent.html',
})

export class WebhookTesterComponent extends BaseComponent {
    //public EntityPM: WebhookKeysPM = null;
    public ObjectTableName = "WebhookKeys";
    public DataContext = this;
    //public EntityId: string = null;
    //public IsNewEntity: boolean = false;
    public ValidationErrorsList: string[] = [];
    //public IsEntityReady: boolean = false;
    //public IsResourcesReady: boolean = false;
    private myService: WebhookKeysExtendedPMService;
    //private isPrimaryGenerated: boolean = false;
    //private isSecondaryGenerated: boolean = false;
    public Operators = ["In Header", "In URL"];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private entityResourceService: EntityResourceService) {
        super();
        this.myService = new WebhookKeysExtendedPMService();
    }

    SetWindowArgs(args: any) {
        this.AccessKey = args['AccessKey'];
        //var test = 'https://system.logitudeworld.com/Angular18123111/index.html';
        var MyURL = window.location.href.split('Angular')[0];
        this.PageURL = MyURL + "WebhooksReceiver.aspx";
        this.InitializeComponent();
    }
    SetNewWizardArgs(args: any) {
        //this.IsNewEntity = args['IsNewEntity'];
        //this.InitializeComponent();
    }
    InitializeComponent() {
        //this.entityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe((res: any) => {

        //    this.IsResourcesReady = true;

        //    if (this.IsNewEntity) {
        //        this.EntityPM = new WebhookKeysPM();
        //        this.EntityPM.Tenant = SessionLocator.Tenant;
        //        this.EntityPM.CreateDate = DateTool.GetCurrentDateTimeAsUtc();
        //        this.EntityPM.UpdateDate = DateTool.GetCurrentDateTimeAsUtc(); 
        //        this.EntityPM.CreatedByUserName = SessionLocator.LoggedUserPM.EnglishName;
        //        this.EntityPM.UpdatedByUserName = SessionLocator.LoggedUserPM.EnglishName; 
        //        this.IsEntityReady = true;
        //    }

        //    else {

        //        this.CurrentSession.StartBusyIndicatorLoading();

        //        this.myService.get(this.EntityId).subscribe((myResponse: ServiceResponse) => {
        //            if (myResponse.HasError) {
        //                this.ValidationErrorsList = myResponse.ErrorsArray;
        //            }

        //            else {
        //                this.EntityPM = myResponse.Result;

        //                if (this.EntityPM) {
        //                    this.IsEntityReady = true;
        //                }
        //            }

        //            this.CurrentSession.StopBusyIndicator();
        //        });
        //    }
        //});
    }



    accessKey: string;
    get AccessKey() { return this.accessKey; }
    set AccessKey(value: string) {

        this.accessKey = value;

    }

    pageURL: string;
    get PageURL() { return this.pageURL; }
    set PageURL(value: string) {

        this.pageURL = value;

    }

    contentToPush: string;
    get ContentToPush() { return this.contentToPush; }
    set ContentToPush(value: string) {

        this.contentToPush = value;

    }

    operation: string = "In Header";
    get Operation() { return this.operation; }
    set Operation(value: string) {
        this.operation = value;
    }


    OperationChanged(event) {
        this.Operation = event;
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
    private messageWindow: MessageWindow = new MessageWindow();
    OkButtonClicked() {
        this.CurrentSession.StartBusyIndicatorCreating();
        if (AppTool.IsNullOrEmpty(this.PageURL) || AppTool.IsNullOrEmpty(this.ContentToPush)) {
            this.ValidationErrorsList.push("Both URL and Content Fields Are Required .");
            return;
        }
        var DataToPush = { URL: this.PageURL, Operation: this.Operation, AccessKey: this.AccessKey, ContentToPush: this.ContentToPush };
        this.myService.PushHookContent(DataToPush).subscribe((myResponse: ServiceResponse) => {

            this.CurrentSession.StopBusyIndicator();

            if (!myResponse.HasError) {
                this.messageWindow.Width = 300;
                this.messageWindow.Height = 150;
                this.messageWindow.Title = "Success";
                this.messageWindow.Message = "your data pushed to the webhook successfully";
                this.messageWindow.Show(this.messageWindow.Message);
                
            }

            else {
                //this.messageWindow.Width = 300;
                //this.messageWindow.Height = 150;
                //this.messageWindow.Title = "Error";
                //this.messageWindow.Message = "your data didn't pushed successfully";
                //this.messageWindow.Show(this.messageWindow.Message);
            }
        });

    }

}
