import {Component} from '@angular/core';
import { WebhookKeysPM } from '../../../../Infrastructure/EntityPMs/WebhookKeysPM';
import { WebhookKeysExtendedPMService } from '../../../../Infrastructure/Services/ExtendedPMs/WebhookKeysExtendedPMService';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {AppTool, DateTool} from '../../../../Infrastructure/Tools';
import { Validator } from '../../../../Infrastructure/Validators/Validator';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { FeatureLocator } from '../../../../Infrastructure/Utilities/FeatureLocator';

@Component({
    moduleId: module.id,
    templateUrl: './WebhookKeysComponent.html',
})

export class WebhookKeysComponent extends BaseComponent {
    public EntityPM: WebhookKeysPM = null;
    public ObjectTableName = "WebhookKeys";
    public DataContext = this;
    public EntityId: string = null;
    public IsNewEntity: boolean = false;
    public ShowTesterButton: boolean = false;
    public ValidationErrorsList: string[] = [];
    public IsEntityReady: boolean = false;
    public IsResourcesReady: boolean = false;
    private myService: WebhookKeysExtendedPMService;
    private isPrimaryGenerated: boolean = false;
    private isSecondaryGenerated: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private entityResourceService: EntityResourceService) {
        super();
        this.myService = new WebhookKeysExtendedPMService();
    }

    SetWindowArgs(args: any) {
        this.EntityId = args['EntityId'];
        this.InitializeComponent();
    }
    SetNewWizardArgs(args: any) {
        this.IsNewEntity = args['IsNewEntity'];
        this.InitializeComponent();
    }
    InitializeComponent() {
        if (FeatureLocator.HasFeaturePermession("WebhookKeys", "WebhookKeysTester")) {
            this.ShowTesterButton = true;
        }
        this.entityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe((res: any) => {
            //WebhookKeysTester
            this.IsResourcesReady = true;

            if (this.IsNewEntity) {
                this.EntityPM = new WebhookKeysPM();
                this.EntityPM.Tenant = SessionLocator.Tenant;
                this.EntityPM.CreateDate = DateTool.GetCurrentDateTimeAsUtc();
                this.EntityPM.UpdateDate = DateTool.GetCurrentDateTimeAsUtc(); 
                this.EntityPM.CreatedByUserName = SessionLocator.LoggedUserPM.EnglishName;
                this.EntityPM.UpdatedByUserName = SessionLocator.LoggedUserPM.EnglishName; 
                this.IsEntityReady = true;
            }

            else {

                this.CurrentSession.StartBusyIndicatorLoading();

                this.myService.get(this.EntityId).subscribe((myResponse: ServiceResponse) => {
                    if (myResponse.HasError) {
                        this.ValidationErrorsList = myResponse.ErrorsArray;
                    }

                    else {
                        this.EntityPM = myResponse.Result;

                        if (this.EntityPM) {
                            this.IsEntityReady = true;
                        }
                    }

                    this.CurrentSession.StopBusyIndicator();
                });
            }
        });
    }
    /*
        Id: string;
		AccessKey: string;
		Tenant: number;
		PartnerName: string;
		InActive: boolean;
		CreatedByUserName: string;
		CreateDate: Date;
		UpdatedByUserName: string;
		UpdateDate: Date;
		Description: string;
    */
    get Description() { return this.EntityPM.Description; }
    set Description(value: string) {
        if (this.EntityPM.Description != value) {
            this.EntityPM.Description = value;
        }
    }

    get AccessKey() { return this.EntityPM.AccessKey; }
    set AccessKey(value: string) {
        if (this.EntityPM.AccessKey != value) {
            this.EntityPM.AccessKey = value;
        }
    }

    get PartnerName() { return this.EntityPM.PartnerName; }
    set PartnerName(value: string) {
        if (this.EntityPM.PartnerName != value) {
            this.EntityPM.PartnerName = value;
        }
    }

    get InActive() { return this.EntityPM.InActive; }
    set InActive(value: boolean) {
        if (this.EntityPM.InActive != value) {
            this.EntityPM.InActive = value;
        }
    }

     

    get CreatedByUserName() { return this.EntityPM.CreatedByUserName; }
    set CreatedByUserName(value: string) {
        if (this.EntityPM.CreatedByUserName != value) {
            this.EntityPM.CreatedByUserName = value;
        }
    }

    get UpdatedByUserName() { return this.EntityPM.UpdatedByUserName; }
    set UpdatedByUserName(value: string) {
        if (this.EntityPM.UpdatedByUserName != value) {
            this.EntityPM.UpdatedByUserName = value;
        }
    }

    get CreateDate() { return this.EntityPM.CreateDate; }
    set CreateDate(value: Date) {
        if (this.EntityPM.CreateDate != value) {
            this.EntityPM.CreateDate = value;
        }
    }

    get UpdateDate() { return this.EntityPM.UpdateDate; }
    set UpdateDate(value: Date) {
        if (this.EntityPM.UpdateDate != value) {
            this.EntityPM.UpdateDate = value;
        }
    }
     
    GeneratePrimaryKeyClicked() {
        this.isPrimaryGenerated = true;
        this.AccessKey = AppTool.GetNewGuid();
    }
     
    CancelButtonClicked() {        
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        var errors: string[] = [];

        //if (!AppTool.IsNullOrEmpty(this.UsedFor) && !AppTool.IsNullOrEmpty(this.AllowedIPs)) {
        //    this.MaskedPrimaryAccessKey = this.CreateMaskedString(this.HashedPrimaryAccessKey);
        //    this.MaskedSeconderyAccessKey = this.CreateMaskedString(this.HashedSeconderyAccessKey);
        //}

        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);

        this.ValidationErrorsList = errors;

        if (errors.length == 0) {           

            if (this.IsNewEntity) {

                this.CurrentSession.StartBusyIndicatorCreating();

                this.myService.insert(this.EntityPM).subscribe((myResponse: ServiceResponse) => {

                    this.CurrentSession.StopBusyIndicator();

                    if (myResponse.HasError) {
                        this.ValidationErrorsList = myResponse.ErrorsArray;
                    }

                    else {
                        this.CurrentSession.CloseCurrentWindowEmit(this.EntityPM.Id);
                    }
                });
            }

            else {

                this.CurrentSession.StartBusyIndicatorSaving();

                this.myService.update(this.EntityPM).subscribe((myResponse: ServiceResponse) => {

                    this.CurrentSession.StopBusyIndicator();

                    if (myResponse.HasError) {
                        this.ValidationErrorsList = myResponse.ErrorsArray;
                    }

                    else {
                        this.CurrentSession.CloseCurrentWindowEmit(this.EntityPM.Id);
                    }
                });
            }            
        }
    }

    TestButtonClicked() {
        var windowArgs: any = {};
        windowArgs.AccessKey = this.AccessKey;

        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.WindowArgs = windowArgs;
        logitudeWindow.Width = 570;
        logitudeWindow.Height = 600;
        logitudeWindow.Title = "WebHook Tester";
        logitudeWindow.Show('./InfrastructureModules/InfrastructureOthers/Components/WebhookKeys/WebhookTesterComponent');
    }
}
