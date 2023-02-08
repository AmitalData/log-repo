import { Component, OnInit, AfterViewInit, ViewChild, ViewContainerRef } from '@angular/core';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { TenantPM } from '../../../Common/EntityPMs/TenantPM';
import { ChildDirective } from '../../../Infrastructure/Directives/ChildDirective';
import { DataCustomObjectPM } from '../../EntityPMs/DataCustomObjectPM';
import { ObjectTablePM } from '../../EntityPMs/ObjectTablePM';
import { ReferenceCustomObjectPM } from '../../EntityPMs/ReferenceCustomObjectPM';
import { DateTool } from '../../Tools';
import { Validator } from '../../Validators/Validator';
import { ServiceResponse } from '../../DataContracts/ServiceResponse';
import { DataCustomObjectPMService } from '../../Services/StandardPMs/DataCustomObjectPMService';
import { ReferenceCustomObjectPMService } from '../../Services/StandardPMs/ReferenceCustomObjectPMService';
import { ListComponent } from '../ListComponent/ListComponent';
import { ClassLevelValidator } from '../../Validators/ClassLevelValidator';


@Component({
    templateUrl: './NewCustomObjectComponent.html',
})

export class NewCustomObjectComponent extends BaseComponent implements OnInit {
    public TenantPM: TenantPM;
    public EntityPM: any;
    public DataContext = this;
    public ObjectTablePM: ObjectTablePM;
    public ObjectTableName: string;
    public ValidationErrorsList: string[] = [];
    public SessionIndex: number;
    @ViewChild('GeneratedArea', { read: ViewContainerRef, static: false }) viewGeneratedAreaRef: ViewContainerRef;
    private CurrentSession = SessionLocator.SelectedSession;
    private dataCustomObjectPMService: DataCustomObjectPMService;
    private referenceCustomObjectPMService: ReferenceCustomObjectPMService;
    public FatherComponent: ListComponent;
    constructor() {
        super();
        this.SessionIndex = this.CurrentSession.SessionIndex;
        this.TenantPM = SessionLocator.TenantPM;
        this.dataCustomObjectPMService = new DataCustomObjectPMService();
        this.referenceCustomObjectPMService = new ReferenceCustomObjectPMService();
    }

    ngOnInit() {

    }

    SetWindowArgs(args: any) {
        this.ObjectTablePM = args["ObjectTablePM"];
        this.ObjectTableName = this.ObjectTablePM.Name;
        this.EntityPM = this.ObjectTablePM.ObjectTableTypeCode == "BR" ? new DataCustomObjectPM("DataCustomObject") : new ReferenceCustomObjectPM("ReferenceCustomObject");
        this.FatherComponent = args["FatherComponent"];
        this.LoadGeneratedArea();
        this.SetEntityPM();
    }

    private GeneratedComponent: any;
    
    public LoadGeneratedArea() {
        if (!this.viewGeneratedAreaRef) {
            this.RunComponentTimer();
            return;
        }
        this.LoadGeneratedAreaComponent();
    }

    private Retries: number = 0;
    private timerToken: any;
    private RunComponentTimer() {
        this.Retries++;

        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }

        if (this.Retries < 20) {
            this.timerToken = setTimeout(() => this.LoadGeneratedArea(), 1);
        }
    }

    LoadGeneratedAreaComponent() {
        SessionLocator.DynamicLoader.Load('./Infrastructure/GenericComponents/GeneratedComponent', this.viewGeneratedAreaRef)
            .then(cmpRef => {
                this.GeneratedComponent = cmpRef.instance;
                cmpRef.instance.HideLastColumn = true;
                cmpRef.instance.BuildLighteningScreenAsClassicScreen = true;
                cmpRef.instance.Run(this.EntityPM, this.ObjectTableName, this.ObjectTablePM.NewWizardControlName);
            });
    }

    

    private SetEntityPM() {
        this.EntityPM.CreateDate = DateTool.GetCurrentDateTimeAsUtc();
        this.EntityPM.UpdateDate = DateTool.GetCurrentDateTimeAsUtc();
        this.EntityPM.CreatedBy = SessionLocator.LoggedUserId;
        this.EntityPM.UpdatedBy = SessionLocator.LoggedUserId;
        this.EntityPM.Tenant = SessionLocator.Tenant;
        this.EntityPM.ObjectTableId = this.ObjectTablePM.Id;
        this.EntityPM.IsDirty = false;
    }

    // Commands

    OkButtonClicked() {
        this.ValidationErrorsList = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, this.ValidationErrorsList);
        this.ValidationErrorsList = this.ValidationErrorsList.concat(new ClassLevelValidator().ValidateCustomEntity(this.ObjectTableName, this.EntityPM));
        if (this.ValidationErrorsList.length == 0) {
            this.Save();
        }
        
    }
    private Save() {
        this.CurrentSession.StartBusyIndicatorCreating();
        if (this.EntityPM instanceof DataCustomObjectPM) {
            this.SaveNewDataCustomObjectPM();
        }
        else if (this.EntityPM instanceof ReferenceCustomObjectPM) {
            this.SaveNewReferenceCustomObjectPM();
        }
    }
    private SaveNewDataCustomObjectPM() {
        this.dataCustomObjectPMService.insert(this.EntityPM).subscribe((response: ServiceResponse) => {
            if (response.HasError) return;
            this.CurrentSession.StopBusyIndicator();
            this.FatherComponent.DoRefresh();
            this.CurrentSession.CloseCurrentWindow();
        });
    }
    private SaveNewReferenceCustomObjectPM() {
        this.referenceCustomObjectPMService.insert(this.EntityPM).subscribe((response: ServiceResponse) => {
            if (response.HasError) return;
            this.CurrentSession.StopBusyIndicator();
            this.FatherComponent.DoRefresh();
            this.CurrentSession.CloseCurrentWindow();
        });
    }
    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
   
}


