import {Component} from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {AppTool} from '../../../../Infrastructure/Tools';
import {TenantPM} from '../../../../Common/EntityPMs/TenantPM';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {TenantPMService} from '../../../../Common/Services/StandardPMs/TenantPMService';
import {InfraSettings} from '../../../../Infrastructure/Utilities/InfraSettings';
import {ApiQueryFilters} from '../../../../Infrastructure/DataContracts/ApiQueryFilters';

@Component({
    selector: 'TicketSettingsComponent',
    moduleId: module.id,
    templateUrl: './TicketSettingsComponent.html',
})

export class TicketSettingsComponent extends BaseComponent {

    public DataContext: TicketSettingsComponent = this;
    public ObjectTableName: string = "Tenant";
    public TenantPm: TenantPM = new TenantPM();
    public IsVisibile: boolean = false;
    public Filters: ApiQueryFilters;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.Filters = new ApiQueryFilters();
        this.Filters.GetAll = true;
        this.LoadTenantPMMethod();
    }
    LoadTenantPMMethod() {
        var myService: TenantPMService = new TenantPMService();
        myService.get(SessionLocator.TenantPM.Id).subscribe((response: ServiceResponse) => {
            this.TenantPm = response.Result;
            this.IsVisibile = true;
        });
    }

    get IsCorrespondenceRightToLeftEnabled() { return this.TenantPm.IsCorrespondenceRightToLeftEnabled; }
    set IsCorrespondenceRightToLeftEnabled(value: boolean) {
        if (this.TenantPm.IsCorrespondenceRightToLeftEnabled != value) {
            this.TenantPm.IsCorrespondenceRightToLeftEnabled = value;
        }
    }

    get IsInternalTicketByDefault() { return this.TenantPm.IsInternalTicketByDefault; }
    set IsInternalTicketByDefault(value: boolean) {
        if (this.TenantPm.IsInternalTicketByDefault != value) {
            this.TenantPm.IsInternalTicketByDefault = value;
        }
    }

    get DefaultSLAId() {
        return this.TenantPm.DefaultSLAId;
    }
    set DefaultSLAId(value: string) {
        if (this.TenantPm.DefaultSLAId != value) {
            this.TenantPm.DefaultSLAId = value;
        }
    }

    // Commands 
    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
    OkButtonClicked() {
        this.CurrentSession.StartBusyIndicator("Saving...");
        var myService: TenantPMService = new TenantPMService();
        myService.update(this.TenantPm).subscribe((myResponse: ServiceResponse) => {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    InfraSettings.TenantPM = this.TenantPm;
                    this.CurrentSession.CloseCurrentWindowEmit("ok");
                }
            }
        });
    }
}
