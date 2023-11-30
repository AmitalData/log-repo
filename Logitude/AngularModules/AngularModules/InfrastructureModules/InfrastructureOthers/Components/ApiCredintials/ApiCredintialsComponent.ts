import {Component} from '@angular/core';
import {ApiCredintialsPM} from '../../../../Infrastructure/EntityPMs/ApiCredintialsPM';
import {ApiCredintialsPMService} from '../../../../Infrastructure/Services/StandardPMs/ApiCredintialsPMService';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {AppTool, DateTool} from '../../../../Infrastructure/Tools';
import {Validator} from '../../../../Infrastructure/Validators/Validator';

@Component({
    
    templateUrl: './ApiCredintialsComponent.html',
})

export class ApiCredintialsComponent extends BaseComponent {
    public EntityPM: ApiCredintialsPM = null;
    public ObjectTableName = "ApiCredintials";
    public DataContext = this;
    public EntityId: string = null;
    public IsNewEntity: boolean = false;
    public ValidationErrorsList: string[] = [];
    public IsEntityReady: boolean = false;
    public IsResourcesReady: boolean = false;
    private myService: ApiCredintialsPMService;
    private isPrimaryGenerated: boolean = false;
    private isSecondaryGenerated: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private entityResourceService: EntityResourceService) {
        super();
        this.myService = new ApiCredintialsPMService();
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
        this.entityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe((res: any) => {

            this.IsResourcesReady = true;

            if (this.IsNewEntity) {
                this.EntityPM = new ApiCredintialsPM();
                this.EntityPM.Tenant = SessionLocator.Tenant;
                this.EntityPM.AllowedIPs = "*";
                this.EntityPM.CreateDate = DateTool.GetCurrentDateTimeAsUtc();
                this.EntityPM.UpdateDate = DateTool.GetCurrentDateTimeAsUtc();
                this.EntityPM.CreatedBy = SessionLocator.LoggedUserPM.EnglishName;
                this.EntityPM.UpdatedBy = SessionLocator.LoggedUserPM.EnglishName;
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

    get UsedFor() { return this.EntityPM.UsedFor; }
    set UsedFor(value: string) {
        if (this.EntityPM.UsedFor != value) {
            this.EntityPM.UsedFor = value;
        }
    }

    get AllowedIPs() { return this.EntityPM.AllowedIPs; }
    set AllowedIPs(value: string) {
        if (this.EntityPM.AllowedIPs != value) {
            this.EntityPM.AllowedIPs = value;
        }
    }

    get HashedPrimaryAccessKey() {

        if (AppTool.IsNullOrEmpty(this.EntityPM.HashedPrimaryAccessKey)) {
            this.EntityPM.HashedPrimaryAccessKey = AppTool.GetNewGuid();
        }

        if (this.IsNewEntity || this.isPrimaryGenerated) {
            return this.EntityPM.HashedPrimaryAccessKey;
        }

        else {
            return this.EntityPM.MaskedPrimaryAccessKey;
        }
    }
    set HashedPrimaryAccessKey(value: string) {
        if (this.EntityPM.HashedPrimaryAccessKey != value) {
            this.EntityPM.HashedPrimaryAccessKey = value;
        }
    }

    get HashedSeconderyAccessKey() {

        if (AppTool.IsNullOrEmpty(this.EntityPM.HashedSeconderyAccessKey)) {
            this.EntityPM.HashedSeconderyAccessKey = AppTool.GetNewGuid();
        }

        if (this.IsNewEntity || this.isSecondaryGenerated) {
            return this.EntityPM.HashedSeconderyAccessKey;
        }

        else {
            return this.EntityPM.MaskedSeconderyAccessKey;
        }
    }
    set HashedSeconderyAccessKey(value: string) {
        if (this.EntityPM.HashedSeconderyAccessKey != value) {
            this.EntityPM.HashedSeconderyAccessKey = value;
        }
    }

    get MaskedPrimaryAccessKey() { return this.EntityPM.MaskedPrimaryAccessKey; }
    set MaskedPrimaryAccessKey(value: string) {
        if (this.EntityPM.MaskedPrimaryAccessKey != value) {
            this.EntityPM.MaskedPrimaryAccessKey = value;
        }
    }

    get MaskedSeconderyAccessKey() { return this.EntityPM.MaskedSeconderyAccessKey; }
    set MaskedSeconderyAccessKey(value: string) {
        if (this.EntityPM.MaskedSeconderyAccessKey != value) {
            this.EntityPM.MaskedSeconderyAccessKey = value;
        }
    }

    get CreatedBy() { return this.EntityPM.CreatedBy; }
    set CreatedBy(value: string) {
        if (this.EntityPM.CreatedBy != value) {
            this.EntityPM.CreatedBy = value;
        }
    }

    get UpdatedBy() { return this.EntityPM.UpdatedBy; }
    set UpdatedBy(value: string) {
        if (this.EntityPM.UpdatedBy != value) {
            this.EntityPM.UpdatedBy = value;
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

    get TokenExpirationTime() { return this.EntityPM.TokenExpirationTime; }
    set TokenExpirationTime(value: number) {
        if (this.EntityPM.TokenExpirationTime != value) {
            this.EntityPM.TokenExpirationTime = value;
        }
    }

    //get ComputingPartnerId() { return this.EntityPM.ComputingPartnerId; }
    //set ComputingPartnerId(value: string) {
    //    if (this.EntityPM.ComputingPartnerId != value) {
    //        this.EntityPM.ComputingPartnerId = value;
    //    }
    //}
    

    GeneratePrimaryKeyClicked() {
        this.isPrimaryGenerated = true;
        this.HashedPrimaryAccessKey = AppTool.GetNewGuid();
    }

    GenerateSeconderyKeyClicked() {
        this.isSecondaryGenerated = true;
        this.HashedSeconderyAccessKey = AppTool.GetNewGuid();
    }

    CreateMaskedString(Key: string) {
        var maskedPKey: string = "";

        if (!AppTool.IsNullOrEmpty(Key)) {
            var i: number = 0;

            for (var index = 0; index < Key.length; index++) {
                var item = Key[index];

                if (item != '-' && i < 32) {
                    maskedPKey += "*";
                }

                else {
                    maskedPKey += item;
                }

                i++;
            }
        }

        return maskedPKey;
    }

    CancelButtonClicked() {        
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        var errors: string[] = [];

        if (!AppTool.IsNullOrEmpty(this.UsedFor) && !AppTool.IsNullOrEmpty(this.AllowedIPs)) {
            this.MaskedPrimaryAccessKey = this.CreateMaskedString(this.HashedPrimaryAccessKey);
            this.MaskedSeconderyAccessKey = this.CreateMaskedString(this.HashedSeconderyAccessKey);
        }

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
}
