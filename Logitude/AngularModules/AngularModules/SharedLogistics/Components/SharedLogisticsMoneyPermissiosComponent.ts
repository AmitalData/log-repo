import { Component, OnInit } from '@angular/core';
import { SessionLocator } from '../../Infrastructure/Utilities/SessionLocator';
import { ServiceResponse } from '../../Infrastructure/DataContracts/ServiceResponse';
import { BaseComponent } from '../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SharedLogisticsSettingPM } from '../../Infrastructure/EntityPMs/SharedLogisticsSettingPM';
import { SharedLogisticsSettingPMService } from '../../Infrastructure/Services/StandardPMs/SharedLogisticsSettingPMService';
import { EntityResourceService } from '../../Infrastructure/Services/EntityResourceService';
import { ObjectsLocator } from '../../Infrastructure/Locators/ObjectsLocator';

@Component({
    moduleId: module.id,
    templateUrl: './SharedLogisticsMoneyPermissiosComponent.html',
})

export class SharedLogisticsMoneyPermissiosComponent extends BaseComponent implements OnInit {
    public EntityPM: SharedLogisticsSettingPM;
    public DataContext: SharedLogisticsMoneyPermissiosComponent = this;
    public ObjectTableName: string;
    private myService: SharedLogisticsSettingPMService;
    public IsResourcesReady: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityResourceService: EntityResourceService) {
        super();
        this.myService = new SharedLogisticsSettingPMService();
        this.ObjectTableName = "SharedLogisticsSetting";
    }

    ngOnInit() {
        this.entityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe((res: any) => {
            this.myService.get(SessionLocator.Tenant.toString()).subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {

                    if (myResponse.Result) {
                        this.EntityPM = myResponse.Result;
                    }

                    else {
                        this.EntityPM = new SharedLogisticsSettingPM();
                        this.EntityPM.IsDirty = false;
                    }
                    
                    this.IsResourcesReady = true;
                }
            });
        });
    }

    get IsInvoicesMenuEnabled() { return this.EntityPM.IsInvoicesMenuEnabled; }
    set IsInvoicesMenuEnabled(value: boolean) {
        if (this.EntityPM.IsInvoicesMenuEnabled != value) {
            this.EntityPM.IsInvoicesMenuEnabled = value;
        }
    }

    get IsMoneyTabEnabled() { return this.EntityPM.IsMoneyTabEnabled; }
    set IsMoneyTabEnabled(value: boolean) {
        if (this.EntityPM.IsMoneyTabEnabled != value) {
            this.EntityPM.IsMoneyTabEnabled = value;
        }
    }


    get IsShowAmountLocalCurrency() { return this.EntityPM.IsShowAmountLocalCurrency; }
    set IsShowAmountLocalCurrency(value: boolean) {
        if (this.EntityPM.IsShowAmountLocalCurrency != value) {
            this.EntityPM.IsShowAmountLocalCurrency = value;
        }
    }



    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    public ValidationErrorsList: string[] = [];
    OkButtonClicked() {
        this.ValidationErrorsList = [];

        if (this.ValidationErrorsList.length == 0) {
            if (this.EntityPM.IsDirty) {
                this.CurrentSession.StartBusyIndicatorSaving();

                if (this.EntityPM.Tenant == null) {
                    this.EntityPM.Tenant = SessionLocator.Tenant;
                    this.myService.insert(this.EntityPM).subscribe((myRespone: ServiceResponse) => {
                        this.CurrentSession.StopBusyIndicator();

                        if (!myRespone.HasError) {
                            ObjectsLocator.SharedLogisticsSettingPM = this.EntityPM;
                            this.CurrentSession.CloseCurrentWindowEmit("OK");
                        }

                        else {
                            this.ValidationErrorsList = myRespone.ErrorsArray;
                        }
                    });
                }

                else {
                    this.myService.update(this.EntityPM).subscribe((myRespone: ServiceResponse) => {
                        this.CurrentSession.StopBusyIndicator();

                        if (!myRespone.HasError) {
                            ObjectsLocator.SharedLogisticsSettingPM = this.EntityPM;
                            this.CurrentSession.CloseCurrentWindowEmit("OK");
                        }

                        else {
                            this.ValidationErrorsList = myRespone.ErrorsArray;
                        }
                    });
                }
            }

            else {
                this.CurrentSession.CloseCurrentWindow();
            }
        }
    }
}
