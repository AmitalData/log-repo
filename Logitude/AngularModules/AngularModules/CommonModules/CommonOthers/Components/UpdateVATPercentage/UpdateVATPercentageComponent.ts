import {Component} from '@angular/core';
import {AppTool, DateTool} from '../../../../Infrastructure/Tools';
import {Validator} from '../../../../Infrastructure/Validators/Validator';
import {VatTypePM} from '../../../../Common/EntityPMs/VatTypePM';
import {VatTypePercentagePM} from '../../../../Common/EntityPMs/VatTypePercentagePM';
import {VatTypePMService} from '../../../../Common/Services/StandardPMs/VatTypePMService';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';

@Component({
    moduleId: module.id,
    templateUrl: './UpdateVATPercentageComponent.html',
})

export class UpdateVATPercentageComponent extends BaseComponent {
    public VatTypeId: string;
    public VatTypePM: VatTypePM = null;
    public EntityPM: VatTypePercentagePM = null;
    public ValidationErrorsList: string[] = [];
    public ObjectTableName = "VatTypePercentage";
    public DataContext = this;
    private myService: VatTypePMService;
    public IsResourcesReady: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private entityResourceService: EntityResourceService) {
        super();
        this.myService = new VatTypePMService();
    }

    SetWindowArgs(myVatTypeId: string) {
        this.VatTypeId = myVatTypeId;

        this.EntityPM = new VatTypePercentagePM(null);
        this.EntityPM.Tenant = SessionLocator.Tenant;
        this.EntityPM.VatTypeId = this.VatTypeId;
        this.EntityPM.FromDate = DateTool.GetCurrentDateAsUtc();

        this.entityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe((res: any) => {
            this.IsResourcesReady = true;

            if (!AppTool.IsNullOrEmpty(this.VatTypeId)) {

                this.CurrentSession.StartBusyIndicatorLoading();

                this.myService.get(this.VatTypeId).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        this.VatTypePM = myResponse.Result;

                        if (this.VatTypePM != null) {
                            this.VatTypePM.AddVatTypePercentagePM(this.EntityPM);
                        }
                    }

                    this.CurrentSession.StopBusyIndicator();
                });
            }
        });
    }

    get FromDate() { return this.EntityPM.FromDate; }
    set FromDate(newValue: Date) {
        if (this.EntityPM.FromDate != newValue) {
            this.EntityPM.FromDate = newValue;
        }
    }

    get Percentage() { return this.EntityPM.Percentage; }
    set Percentage(newValue: number) {
        if (this.EntityPM.Percentage != newValue) {
            this.EntityPM.Percentage = AppTool.Round(newValue, 2);
        }
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);

        if (errors.length == 0) {
            if (this.FromDate == null) {
                errors.push("From Date is required");
            }

            if (this.Percentage == null) {
                errors.push("Percentage is required");
            }
        }

        this.ValidationErrorsList = errors;

        if (errors.length == 0) {

            this.CurrentSession.StartBusyIndicatorSaving();

            this.myService.update(this.VatTypePM).subscribe((myResponse: ServiceResponse) => {
                if (myResponse.HasError) {
                    this.ValidationErrorsList = myResponse.ErrorsArray;
                    this.CurrentSession.StopBusyIndicator();
                }

                else {
                    this.CurrentSession.CloseCurrentWindowEmit("OK");
                }
            });
        }
    }
}
