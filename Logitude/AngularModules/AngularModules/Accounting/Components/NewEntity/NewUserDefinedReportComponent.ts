import {Component, ChangeDetectorRef, OnDestroy} from '@angular/core';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {Validator} from '../../../Infrastructure/Validators/Validator';
import {TenantPM} from '../../../Common/EntityPMs/TenantPM';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {EntityListService} from '../../../Infrastructure/Services/EntityListService';
import { ObjectsLocator } from '../../../Infrastructure/Locators/ObjectsLocator';
import { EntityArgs } from '../../../Infrastructure/DataContracts/EntityArgs';
import { AppTool } from '../../../Infrastructure/Tools';
import { UserDefinedReportPM } from 'Accounting/EntityPMs/UserDefinedReportPM';
import { UserDefinedReportPMService } from 'Accounting/Services/StandardPMs/UserDefinedReportPMService';

@Component({
    selector: 'NewUserDefinedReportComponent',
    
    providers: [EntityListService],
    templateUrl: './NewUserDefinedReportComponent.html',
})

export class NewUserDefinedReportComponent extends BaseComponent implements OnDestroy {

    public EntityPM: UserDefinedReportPM;
    public ObjectTableName: string = "UserDefinedReport";
    public DataContext: NewUserDefinedReportComponent = this;
    private CurrentSession = SessionLocator.SelectedSession;
    public TenantPM: TenantPM;
    public isRTL: boolean = false;
    public ValidationErrorsList: string[] = [];
    myService: UserDefinedReportPMService;
    constructor(public entityArgs: EntityArgs) {
        super();
        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        this.EntityPM = entityArgs.EntityPM;
        this.TenantPM = SessionLocator.TenantPM;
        this.EntityPM = new UserDefinedReportPM();
        this.EntityPM.Tenant = this.TenantPM.Id;
        this.myService = new UserDefinedReportPMService();
        this.Listen();
    }
 
    private SaveCompletedEvent: any = null;
    private LoadCompletedEvent: any = null;
    private Listen() {
        if (this.entityArgs.EditComponent != null) {
            this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                }
            });

            this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                if (isLoadSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                }
            });
        }
    }

    get EnglishName() {
        if (this.EntityPM != null) {
            return this.EntityPM.EnglishName;
        }
        else
            return null;
    }
    set EnglishName(newValue: string) {
        if (this.EntityPM.EnglishName != newValue) {
            this.EntityPM.EnglishName= newValue;
        }
    }

    get LocalName() {
        if (this.EntityPM != null) {
            return this.EntityPM.LocalName;
        }
        else
            return null;
    }
    set LocalName(newValue: string) {
        if (this.EntityPM.LocalName != newValue) {
            this.EntityPM.LocalName = newValue;
        }
    }
    OkButtonClicked(){
        this.SubmitChanges();
    }

    SubmitChanges() {
        this.CurrentSession.StartBusyIndicatorLoading();
        this.myService.insert(this.EntityPM).subscribe((myResult:any) => {
            this.CurrentSession.StopBusyIndicator();
            var iServiceResponse: ServiceResponse = myResult;
            if (!iServiceResponse.HasError) {
                var entity = iServiceResponse.Result;
                this.CurrentSession.CloseCurrentWindowEmit("ok");
                SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent',
                    this.CurrentSession.SessionLocation.viewContainerRef)
                    .then(cmpRef => {
                        cmpRef.instance.ComponentRef = cmpRef;
                        cmpRef.instance.Run({ EntityId: entity.Id, ObjectTableName: this.ObjectTableName });
                        cmpRef.instance.BackCompleted.subscribe(($event: any) => {
                            this.CancelButtonClicked();
                        });
                    });
            }
            else {
                this.ValidationErrorsList = iServiceResponse.ErrorsArray;
                this.CurrentSession.StopBusyIndicator();
            }
        });
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    ngOnDestroy() {
        AppTool.KillEventEmitter(this.SaveCompletedEvent);
        AppTool.KillEventEmitter(this.LoadCompletedEvent);
    }
     
}
