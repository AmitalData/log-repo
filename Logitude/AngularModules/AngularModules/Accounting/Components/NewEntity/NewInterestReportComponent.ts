import {Component, ChangeDetectorRef, OnDestroy} from '@angular/core';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {Validator} from '../../../Infrastructure/Validators/Validator';
import {TenantPM} from '../../../Common/EntityPMs/TenantPM';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {EntityListService} from '../../../Infrastructure/Services/EntityListService';
import { InterestReportPMService } from '../../Services/StandardPMs/InterestReportPMService';
import { ObjectsLocator } from '../../../Infrastructure/Locators/ObjectsLocator';
import { InterestReportPM } from '../../EntityPMs/InterestReportPM';
import { InterestReportGeneralTabComponent } from '../EditTabs/InterestReport/GeneralTab/InterestReportGeneralTabComponent';
import { EntityArgs } from '../../../Infrastructure/DataContracts/EntityArgs';
import { AppTool } from '../../../Infrastructure/Tools';

@Component({
    selector: 'NewInterestReportComponent',
    moduleId: module.id,
    providers: [EntityListService],
    templateUrl: './NewInterestReportComponent.html',
})

export class NewInterestReportComponent extends BaseComponent implements OnDestroy {

    public EntityPM: InterestReportPM;
    public ObjectTableName: string = "InterestReport";
    public DataContext: InterestReportGeneralTabComponent = this;
    private CurrentSession = SessionLocator.SelectedSession;
    public TenantPM: TenantPM;
    public isRTL: boolean = false;
    public ValidationErrorsList: string[] = [];
    myService: InterestReportPMService;
    constructor(public entityArgs: EntityArgs) {
        super();
        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        this.EntityPM = entityArgs.EntityPM;
        this.TenantPM = SessionLocator.TenantPM;
        this.EntityPM = new InterestReportPM();
        this.EntityPM.Tenant = this.TenantPM.Id;
        this.myService = new InterestReportPMService();
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

    get InterestCalculationDate() {
        if (this.EntityPM != null) {
            return this.EntityPM.InterestCalculationDate;
        }
        else
            return null;
    }
    set InterestCalculationDate(newValue: Date) {
        if (this.EntityPM.InterestCalculationDate != newValue) {
            this.EntityPM.InterestCalculationDate = newValue;
        }
    }

    get GLAccountId() {
        if (this.EntityPM != null) {
            return this.EntityPM.GLAccountId;
        }
        else
            return null;
    }
    set GLAccountId(newValue: string) {
        if (this.EntityPM.GLAccountId != newValue) {
            this.EntityPM.GLAccountId = newValue;
        }
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
    OkButtonClicked() {
        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        if (errors.length == 0) {
            this.SubmitChanges();
        } else {
            this.ValidationErrorsList = errors;
        }
    }

    SubmitChanges() {
        this.myService.insert(this.EntityPM).subscribe(myResult => {
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

    ngOnDestroy() {
        AppTool.KillEventEmitter(this.SaveCompletedEvent);
        AppTool.KillEventEmitter(this.LoadCompletedEvent);
    }
     
}
