import { Component, OnInit} from '@angular/core';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { TenantPM } from '../../../../Common/EntityPMs/TenantPM';
import { TenantPMService } from '../../../../Common/Services/StandardPMs/TenantPMService';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { InfraSettings } from '../../../../Infrastructure/Utilities/InfraSettings';
import { Validator } from '../../../../Infrastructure/Validators/Validator';
import { CodeNameClass } from '../../../../Infrastructure/DataContracts/CodeNameClass';

@Component({
    templateUrl: './ContainerSettingsComponent.html',
})

export class ContainerSettingsComponent extends BaseComponent implements OnInit {
    public DataContext: ContainerSettingsComponent = this;
    public ObjectTableName: string = "Tenant";
    public tenant: TenantPM = new TenantPM();
    public IsVisible = false;
    private CurrentSession = SessionLocator.SelectedSession;
    public ValidationErrorsList: string[];
    public reloadingTranslation: boolean;
    public ClosingContainerToolTipMessage: string = "How many days after the Actual Empty Return Date to wait before automatically closing the container.";
    public ClosingFieldsItemSource: CodeNameClass[] = [];
    constructor() {
        super();
    }

    ngOnInit() {
        this.GetCurrentTenant();        
    }

    private GetCurrentTenant() {
        var myService: TenantPMService = new TenantPMService();
        myService.get(SessionLocator.TenantPM.Id).subscribe((response: ServiceResponse) => {
            this.tenant = response.Result;
            this.IsVisible = true;
            this.BuildClosingFieldsItemSource();
        });
    }

    BuildClosingFieldsItemSource() {
        this.ClosingFieldsItemSource = [];
        this.ClosingFieldsItemSource.push(new CodeNameClass("EMPTR", "Empty Return"));
        this.ClosingFieldsItemSource.push(new CodeNameClass("SHATA", "Shipment ATA"));

        this.selectedClosingField = this.ClosingFieldsItemSource.filter(d => d.Code == this.AutomaticallyClosingField)[0];
    }

    private selectedClosingField: CodeNameClass;
    get SelectedClosingField() { return this.selectedClosingField; }
    set SelectedClosingField(value: CodeNameClass) {
        if (this.selectedClosingField != value) {
            this.selectedClosingField = value;

            if (value) this.AutomaticallyClosingField = value.Code;
            else {
                this.AutomaticallyClosingField = null;
                this.AutomaticallyCloseDays = null;
            }
        }
    }

    get AutomaticallyCloseDays() { return this.tenant.AutomaticallyCloseDays; }
    set AutomaticallyCloseDays(value: number) {
        if (this.tenant.AutomaticallyCloseDays != value) {
            this.tenant.AutomaticallyCloseDays = value;
        }
    }

    get AutomaticallyClosingField() { return this.tenant.AutomaticallyClosingField; }
    set AutomaticallyClosingField(value: string) {
        if (this.tenant.AutomaticallyClosingField != value) {
            this.tenant.AutomaticallyClosingField = value;
        }
    }

    // Commands 
    CancelButtonClicked() {
        this.tenant= null;
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        var errors: string[] = [];
        Validator.TryValidateObject(this.DataContext.tenant, this.DataContext.ObjectTableName, errors);
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            this.SaveTenant();
        }
    }

    SaveTenant() {
        this.CurrentSession.StartBusyIndicator("Saving...");
        var myService: TenantPMService = new TenantPMService();
        myService.update(this.tenant).subscribe((myResponse: ServiceResponse) => {
            if (myResponse) {
                if (!myResponse.HasError) {
                    InfraSettings.TenantPM = this.tenant;
                    this.CurrentSession.CloseCurrentWindowEmit("ok");
                }
                else {
                    this.ValidationErrorsList = myResponse.ErrorsArray;
                    this.CurrentSession.StopBusyIndicator();
                }
            }
        });
    }

}
