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
import { CustomsRequestsSheetPM } from '../../../../Customs/EntityPMs/CustomsRequestsSheetPM';

@Component({
    
    templateUrl: './CourierMasterGeneralTabComponent.html',
})


export class CourierMasterGeneralTabComponent extends BaseComponent {
    ObjectTableName: string = "Customs.CourierMaster";
    DataContext: any = this;
    //entityPM: CourierMasterPM;
    CourierMasterValidator: CourierMasterValidator = new CourierMasterValidator();
    CourierMasterService: CourierMasterService = new CourierMasterService();

    public CurrentEditComponentId: string;
    public CarrierDependencyProperty1: string;

    public WeightValueFilterItems: ApiQueryFilters;

    public IsDisplayOnly: boolean = false;
    public DisplayOnlyMessage: string = "";
    timerToken: any;

    constructor(public entityArgs: EntityArgs) {
        super();
         this.EntityPM = entityArgs.EntityPM;
        this.WeightValueFilterItems = new ApiQueryFilters();
        this.WeightValueFilterItems.addAdditionalFilter("PaymentMethodCode", "CC,CA,NC,PO,PP", null, null, "InListExact", true, false, false, "string", false, true);
        this.UIProperties.SetEnabled("StorageSiteCode", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("IntegratorCode", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("IntegratorName", this.ObjectTableName, false);
        this.DisplayOnlyCheck();
        this.CarrierDependencyProperty1 = "TR";
        this.Listen();

    }

    private Listen() {

        if (SessionLocator.SelectedSession.CurrentEditComponent != null) {
            this.CurrentEditComponentId = SessionLocator.SelectedSession.CurrentEditComponent.ComponentId;
            SessionLocator.SelectedSession.CurrentEditComponent.SubscriptionAdd(
                SessionLocator.SelectedSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                    if (isSaveSuccess) {
                        this.EntityPM = SessionLocator.SelectedSession.CurrentEditComponent.EntityPM;
                    }
                })
            );
            SessionLocator.SelectedSession.CurrentEditComponent.SubscriptionAdd(
                SessionLocator.SelectedSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                    if (isLoadSuccess && SessionLocator.SelectedSession.CurrentEditComponent) {
                        this.EntityPM = SessionLocator.SelectedSession.CurrentEditComponent.EntityPM;
                    }
                })
            );

            SessionLocator.SelectedSession.CurrentEditComponent.SubscriptionAdd(
                SessionLocator.SelectedSession.CurrentEditComponent.TabSelected.subscribe((tabCode: string) => {
                    if (this.CurrentEditComponentId == SessionLocator.SelectedSession.CurrentEditComponent.ComponentId) {
                        if (tabCode == "COGN") {
                            this.SetScreenFieldsEditability();
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
    get CourierMasterRemarks() { return this.EntityPM.CourierMasterRemarks; }
    set CourierMasterRemarks(value: string) {
        if (this.EntityPM.CourierMasterRemarks != value) {
            this.EntityPM.CourierMasterRemarks = value;
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

    get PackageQuantityInMAWB() { return this.EntityPM.PackageQuantityInMAWB; }
    set PackageQuantityInMAWB(value: number) {
        if (this.EntityPM.PackageQuantityInMAWB != value) { 
            this.EntityPM.PackageQuantityInMAWB = value;
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
    get LandingDateDateOnly() { return this.EntityPM.LandingDateDateOnly; }
    set LandingDateDateOnly(value: Date) {
        if (this.EntityPM.LandingDateDateOnly != value) {
            this.EntityPM.LandingDateDateOnly = value;
        }
    }

    get LandingDateTimeOnly() { return this.EntityPM.LandingDateTimeOnly; }
    set LandingDateTimeOnly(value: Date) {
        if (this.EntityPM.LandingDateTimeOnly != value) {
            this.EntityPM.LandingDateTimeOnly = value;
        }
    }

    get LandingDate() { return this.EntityPM.LandingDate; }
    set LandingDate(value: Date) {
        if (this.EntityPM.LandingDate != value) {
            this.EntityPM.LandingDate = value;
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

    get TruckerId() { return this.EntityPM.TruckerId; }
    set TruckerId(value: string) {
        if (this.EntityPM.TruckerId != value) {
            this.EntityPM.TruckerId = value;
        }
    }

    get IntegratorCode() { return this.EntityPM.IntegratorCode; }
    set IntegratorCode(value: string) {
        if (this.EntityPM.IntegratorCode != value) {
            this.EntityPM.IntegratorCode = value;
        }
    }

    get IntegratorName() { return this.EntityPM.IntegratorName; }
    set IntegratorName(value: string) {
        if (this.EntityPM.IntegratorName != value) {
            this.EntityPM.IntegratorName = value;

        }
    }

    AirLineIdLostFocus(value: any) {

        //this.CourierMasterService.GetIfCourierMasterExists(this.EntityPM.Id, this.EntityPM.AirlineId, this.EntityPM.HAWB, this.EntityPM.MAWB).subscribe((Result:any) => {
        //    var mm: ServiceResponse = Result;
        //    if (!mm.HasError) {
        //        if (mm.Result) {
        //            var errorMsg: string = "Already exist";
        //            this.UIProperties.SetValidity("AirlineId", "Customs.CourierMaster", false,"Already exist");
        //            //this.CourierMasterValidator.ValidationErrorMessageCodes.push(errorMsg);
        //            //SessionLocator.SelectedSession.CurrentEditComponent.ValidationErrorsList.push(errorMsg);
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
                let customsRequestsSheetPM: CustomsRequestsSheetPM = displayOnlyCheckResult.filter(r => r.InterfaceTypeCode == "UCBCMSS")[0];
                if (customsRequestsSheetPM != null) {
                    this.IsDisplayOnly = true;
                    this.DisplayOnlyMessage = "לתצוגה בלבד - קיימת בקשה לשינוי אתר איחסון ברקע ";
                    this.SetScreenFieldsEditability();

                    this.timerToken = setTimeout(() => {
                        this.SetScreenFieldsEditability();
                        clearTimeout(this.timerToken);
                        //this.CD.detectChanges();
                    }, 900);
                }
            }
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
        //this.UIProperties.SetEnabled("EstimatedArrivalTimeOnly", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("EstimatedArrivalTimeOnly_timepicker", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("EstimatedArrivalDateOnly", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("LandingDateTimeOnly_timepicker", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("LandingDateDateOnly", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("PackageQuantity", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("GrossMassMeasure", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("WeightValueCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("TruckerId", this.ObjectTableName, !this.IsDisplayOnly);
    }

    RefreshEntity() {
        SessionLocator.SelectedSession.CurrentEditComponent.ReloadEntityPM();
        this.DisplayOnlyCheck();
    }

}
