import {Component} from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {CourierMasterPM} from '../../../../Customs/EntityPMs/CourierMasterPM';
import {CourierMasterService} from '../../../../Customs/Services/Others/CourierMasterService';
import {CourierMasterValidator} from '../../../../Customs/Validators/CourierMasterValidator';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import { ApiQueryFilters } from '../../../../Infrastructure/DataContracts/ApiQueryFilters';


@Component({
    moduleId: module.id,
    templateUrl: './CourierMasterGeneralTabComponent.html',
})


export class CourierMasterGeneralTabComponent extends BaseComponent {
    ObjectTableName: string = "Customs.CourierMaster";
    DataContext: any = this;
    //entityPM: CourierMasterPM;
    CourierMasterValidator: CourierMasterValidator = new CourierMasterValidator();
    CourierMasterService: CourierMasterService = new CourierMasterService();

    public CurrentEditComponentId: string;

    public WeightValueFilterItems: ApiQueryFilters;

    public IsDisplayOnly: boolean = false;
    public DisplayOnlyMessage: string = "";

    constructor(public entityArgs: EntityArgs) {
        super();
        this.EntityPM = entityArgs.EntityPM;
        this.WeightValueFilterItems = new ApiQueryFilters();
        this.WeightValueFilterItems.addAdditionalFilter("Code", "CC,CA,NC,PO,PP", null, null, "InListExact", false, false, false, "string", false, true);
        this.UIProperties.SetEnabled("StorageSiteCode", this.ObjectTableName, false);
        this.DisplayOnlyCheck();
        this.Listen();
    }

    private Listen() {
        if (SessionLocator.CurrentSession.CurrentEditComponent != null) {
            this.CurrentEditComponentId = SessionLocator.CurrentSession.CurrentEditComponent.ComponentId;
            SessionLocator.CurrentSession.CurrentEditComponent.SubscriptionAdd(
                SessionLocator.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                    if (isSaveSuccess) {
                        this.EntityPM = SessionLocator.CurrentSession.CurrentEditComponent.EntityPM;
                    }
                })
            );
            SessionLocator.CurrentSession.CurrentEditComponent.SubscriptionAdd(
                SessionLocator.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                    if (isLoadSuccess && SessionLocator.CurrentSession.CurrentEditComponent) {
                        this.EntityPM = SessionLocator.CurrentSession.CurrentEditComponent.EntityPM;
                    }
                })
            );

            SessionLocator.CurrentSession.CurrentEditComponent.SubscriptionAdd(
                SessionLocator.CurrentSession.CurrentEditComponent.TabSelected.subscribe((tabCode: string) => {
                    if (this.CurrentEditComponentId == SessionLocator.CurrentSession.CurrentEditComponent.ComponentId) {
                        if (tabCode == "COGN") {

                        }
                    }
                })
            );
        }
    }


    get AirlineId() { return this.EntityPM.AirlineId; }
    set AirlineId(value: string) {

        if (this.EntityPM.AirlineId != value) {
            this.EntityPM.AirlineId = value;
        }
    }

    get MAWB() { return this.EntityPM.MAWB; }
    set MAWB(value: string) {
        if (this.EntityPM.MAWB != value) {
            this.EntityPM.MAWB = value;
        }
    }

    get HAWB() { return this.EntityPM.HAWB; }
    set HAWB(value: string) {
        if (this.EntityPM.HAWB != value) {
            this.EntityPM.HAWB = value;
        }
    }

    get OriginPortCode() { return this.EntityPM.OriginPortCode; }
    set OriginPortCode(value: string) {
        if (this.EntityPM.OriginPortCode != value) {
            this.EntityPM.OriginPortCode = value;
        }
    }

    get ManifestNumber() { return this.EntityPM.ManifestNumber; }
    set ManifestNumber(value: string) {
        if (this.EntityPM.ManifestNumber != value) {
            this.EntityPM.ManifestNumber = value;
        }
    }

    get GatewayPortCode() { return this.EntityPM.GatewayPortCode; }
    set GatewayPortCode(value: string) {
        if (this.EntityPM.GatewayPortCode != value) {
            this.EntityPM.GatewayPortCode = value;
        }
    }

    get FlightNumber() { return this.EntityPM.FlightNumber; }
    set FlightNumber(value: string) {
        if (this.EntityPM.FlightNumber != value) {
            this.EntityPM.FlightNumber = value;
        }
    }

    get IsOpen() { return this.EntityPM.IsOpen; }
    set IsOpen(value: boolean) {
        if (this.EntityPM.IsOpen != value) {
            this.EntityPM.IsOpen = value;
        }
    }

    get DepartureDate() { return this.EntityPM.DepartureDate; }
    set DepartureDate(value: Date) {
        if (this.EntityPM.DepartureDate != value) {
            this.EntityPM.DepartureDate = value;
        }
    }

    get PackageQuantity() { return this.EntityPM.PackageQuantity; }
    set PackageQuantity(value: number) {
        if (this.EntityPM.PackageQuantity != value) {
            this.EntityPM.PackageQuantity = value;
        }
    }

    get GrossMassMeasure() { return this.EntityPM.GrossMassMeasure; }
    set GrossMassMeasure(value: number) {
        if (this.EntityPM.GrossMassMeasure != value) {
            this.EntityPM.GrossMassMeasure = value;
        }
    }

    get EstimatedArrivalDateOnly() { return this.EntityPM.EstimatedArrivalDateOnly; }
    set EstimatedArrivalDateOnly(value: Date) {
        if (this.EntityPM.EstimatedArrivalDateOnly != value) {
            this.EntityPM.EstimatedArrivalDateOnly = value;
        }
    }

    get EstimatedArrivalTimeOnly() { return this.EntityPM.EstimatedArrivalTimeOnly; }
    set EstimatedArrivalTimeOnly(value: Date) {
        if (this.EntityPM.EstimatedArrivalTimeOnly != value) {
            this.EntityPM.EstimatedArrivalTimeOnly = value;
        }
    }

    get EstimatedArrivalDate() { return this.EntityPM.EstimatedArrivalDate; }
    set EstimatedArrivalDate(value: Date) {
        if (this.EntityPM.EstimatedArrivalDate != value) {
            this.EntityPM.EstimatedArrivalDate = value;
        }
    }

    get WeightValueCode() { return this.EntityPM.WeightValueCode; }
    set WeightValueCode(value: string) {
        if (this.EntityPM.WeightValueCode != value) {
            this.EntityPM.WeightValueCode = value;
        }
    }

    get StorageSiteCode() { return this.EntityPM.StorageSiteCode; }
    set StorageSiteCode(value: string) {
        if (this.EntityPM.StorageSiteCode != value) {
            this.EntityPM.StorageSiteCode = value;
        }
    }

    AirLineIdLostFocus(value: any) {

        //this.CourierMasterService.GetIfCourierMasterExists(this.EntityPM.Id, this.EntityPM.AirlineId, this.EntityPM.HAWB, this.EntityPM.MAWB).subscribe(Result => {
        //    var mm: ServiceResponse = Result;
        //    if (!mm.HasError) {
        //        if (mm.Result) {
        //            var errorMsg: string = "Already exist";
        //            this.UIProperties.SetValidity("AirlineId", "Customs.CourierMaster", false,"Already exist");
        //            //this.CourierMasterValidator.ValidationErrorMessageCodes.push(errorMsg);
        //            //SessionLocator.CurrentSession.CurrentEditComponent.ValidationErrorsList.push(errorMsg);
        //        }
        //    }
        //});
    }

    MAWBLostFocus(value: any) {

    }

    HAWBLostFocus(value: any) {
    }

    DisplayOnlyCheck() {
        this.IsDisplayOnly = false;

        //Check if changing StorageSiteCode
        this.CourierMasterValidator.SetEntityPM(this.EntityPM);
        this.CourierMasterValidator.CheckRequestInProgressForCourierMaster(this.EntityPM.Tenant, "UCBCMSS", this.EntityPM.Id).subscribe((response: any) => {
            var displayOnlyCheckResult = response.Result;
            if (displayOnlyCheckResult != null && displayOnlyCheckResult.length > 0) {
                this.IsDisplayOnly = true;
                this.DisplayOnlyMessage = "לתצוגה בלבד - קיימת בקשה לשינוי אתר איחסון ברקע ";
            }
            this.SetScreenFieldsEditability();
        });
    }

    SetScreenFieldsEditability() {
        this.UIProperties.SetEnabled("AirlineId", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("MAWB", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("IsOpen", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("HAWB", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("GatewayPortCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("OriginPortCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("FlightNumber", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("DepartureDate", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("EstimatedArrivalTimeOnly", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("EstimatedArrivalDateOnly", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("PackageQuantity", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("GrossMassMeasure", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("WeightValueCode", this.ObjectTableName, !this.IsDisplayOnly);
    }

    RefreshEntity() {
        SessionLocator.CurrentSession.CurrentEditComponent.ReloadEntityPM();
        this.DisplayOnlyCheck();
    }

}
