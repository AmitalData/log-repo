
import {Component} from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {CourierMasterPM} from '../../../../Customs/EntityPMs/CourierMasterPM';
import {CourierMasterService} from '../../../../Customs/Services/Others/CourierMasterService';
import {CourierMasterValidator} from '../../../../Customs/Validators/CourierMasterValidator';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';


@Component({
    moduleId: module.id,
    templateUrl: './CourierMasterGeneralTabComponent.html',
})


export class CourierMasterGeneralTabComponent extends BaseComponent {
    ObjectTableName: string = "Customs.CourierMaster";
    DataContext: any = this;
    entityPM: CourierMasterPM;
    CourierMasterValidator: CourierMasterValidator = new CourierMasterValidator();
    CourierMasterService: CourierMasterService = new CourierMasterService();

    public CurrentEditComponentId: string;

    constructor(public entityArgs: EntityArgs) {
        super();
        this.entityPM = entityArgs.EntityPM;
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


    get AirlineId() { return this.entityPM.AirlineId; }
    set AirlineId(value: string) {

        if (this.entityPM.AirlineId != value) {
            this.entityPM.AirlineId = value;
        }
    }

    get MAWB() { return this.entityPM.MAWB; }
    set MAWB(value: string) {
        if (this.entityPM.MAWB != value) {
            this.entityPM.MAWB = value;
        }
    }

    get HAWB() { return this.entityPM.HAWB; }
    set HAWB(value: string) {
        if (this.entityPM.HAWB != value) {
            this.entityPM.HAWB = value;
        }
    }

    get OriginPortCode() { return this.entityPM.OriginPortCode; }
    set OriginPortCode(value: string) {
        if (this.entityPM.OriginPortCode != value) {
            this.entityPM.OriginPortCode = value;
        }
    }

    get ManifestNumber() { return this.entityPM.ManifestNumber; }
    set ManifestNumber(value: string) {
        if (this.entityPM.ManifestNumber != value) {
            this.entityPM.ManifestNumber = value;
        }
    }

    get GatewayPortCode() { return this.entityPM.GatewayPortCode; }
    set GatewayPortCode(value: string) {
        if (this.entityPM.GatewayPortCode != value) {
            this.entityPM.GatewayPortCode = value;
        }
    }

    get FlightNumber() { return this.entityPM.FlightNumber; }
    set FlightNumber(value: string) {
        if (this.entityPM.FlightNumber != value) {
            this.entityPM.FlightNumber = value;
        }
    }

    get IsOpen() { return this.entityPM.IsOpen; }
    set IsOpen(value: boolean) {
        if (this.entityPM.IsOpen != value) {
            this.entityPM.IsOpen = value;
        }
    }

    get DepartureDate() { return this.entityPM.DepartureDate; }
    set DepartureDate(value: Date) {
        if (this.entityPM.DepartureDate != value) {
            this.entityPM.DepartureDate = value;
        }
    }

    get PackageQuantity() { return this.entityPM.PackageQuantity; }
    set PackageQuantity(value: number) {
        if (this.entityPM.PackageQuantity != value) {
            this.entityPM.PackageQuantity = value;
        }
    }

    get GrossMassMeasure() { return this.entityPM.GrossMassMeasure; }
    set GrossMassMeasure(value: number) {
        if (this.entityPM.GrossMassMeasure != value) {
            this.entityPM.GrossMassMeasure = value;
        }
    }

    get EstimatedArrivalDateOnly() { return this.entityPM.EstimatedArrivalDateOnly; }
    set EstimatedArrivalDateOnly(value: Date) {
        if (this.entityPM.EstimatedArrivalDateOnly != value) {
            this.entityPM.EstimatedArrivalDateOnly = value;
        }
    }

    get EstimatedArrivalTimeOnly() { return this.entityPM.EstimatedArrivalTimeOnly; }
    set EstimatedArrivalTimeOnly(value: Date) {
        if (this.entityPM.EstimatedArrivalTimeOnly != value) {
            this.entityPM.EstimatedArrivalTimeOnly = value;
        }
    }

    get EstimatedArrivalDate() { return this.entityPM.EstimatedArrivalDate; }
    set EstimatedArrivalDate(value: Date) {
        if (this.entityPM.EstimatedArrivalDate != value) {
            this.entityPM.EstimatedArrivalDate = value;
        }
    }

    AirLineIdLostFocus(value: any) {

        //this.CourierMasterService.GetIfCourierMasterExists(this.entityPM.Id, this.entityPM.AirlineId, this.entityPM.HAWB, this.entityPM.MAWB).subscribe(Result => {
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

    SaveAndOpenWorksheet() {
        if (this.entityPM.IsDirty) {


            SessionLocator.CurrentSession.StartBusyIndicatorSaving();
            var sub = SessionLocator.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                SessionLocator.CurrentSession.StopBusyIndicator();
                sub.unsubscribe();
                if (isSaveSuccess) {
                    this.OpenWorksheet();
                }
            });
            SessionLocator.CurrentSession.CurrentEditComponent.SaveChanges();

        }
        else {
            this.OpenWorksheet();

        }
    }
    OpenWorksheet(){
        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Width = 1029;
        logitudeWindow.Height = 750;
        logitudeWindow.ShowCloseButton = true;
        logitudeWindow.IsFillScreen = true;

        logitudeWindow.WindowArgs = { EntityPM: this.entityPM };
        //logitudeWindow.Title = this.ObjectTableName + " Search";
        //logitudeWindow.Show('./Customs/Components/Courier/CourierWorkSheet/CourierWorksheetComponent');
        logitudeWindow.Show('./CustomsModules/CustomsCourier/Components/CourierWorkSheet/CourierWorksheetComponent');
        logitudeWindow.WindowClosed.subscribe(($event: any) => SessionLocator.CurrentSession.CurrentEditComponent.ReloadEntityPM());

    }
}
