import {Component, AfterViewInit, ViewChildren, QueryList, Output, EventEmitter} from '@angular/core';
import {ShipmentPM} from '../../../../Shipment/EntityPMs/ShipmentPM';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {LocationDirective} from '../../../../Infrastructure/Utilities/LocationDirective';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {AppTool} from '../../../../Infrastructure/Tools';
import {ShipmentPMService} from '../../../../Shipment/Services/StandardPMs/ShipmentPMService';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {FSRWizardArgs} from '../../../../Shipment/Args';

@Component({
    moduleId: module.id,
    templateUrl: './FSRWizardComponent.html',
    providers: [EntityArgs],
})

export class FSRWizardComponent implements AfterViewInit {
    public EntityPM: ShipmentPM;
    public ShipmentLevelCode: string;
    public ObjectTableName: string;
    public ValidationErrorsList: string[];
    public ValidationWarningsList: string[];
    @Output() LoadCompleted: EventEmitter<boolean> = new EventEmitter<boolean>();
    @ViewChildren(LocationDirective) public AllLocations: QueryList<LocationDirective>;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs) {
        this.ValidationErrorsList = [];
        this.ValidationWarningsList = [];
    }

    SetWindowArgs(windowArgs: FSRWizardArgs) {
        this.EntityPM = windowArgs.EntityPM;
        this.ShipmentLevelCode = windowArgs.ShipmentLevelCode;
        this.ObjectTableName = this.ShipmentLevelCode == "C" ? "Master" : "Shipment";
        this.entityArgs.EntityPM = this.EntityPM;
        this.entityArgs.ObjectTableName = this.ObjectTableName;
        this.InitializeWizard();
    }

    private isViewInited = false;
    ngAfterViewInit() {
        this.isViewInited = true;
        this.InitializeWizard();
    }

    private InitializeWizard() {
        if (this.EntityPM != null && this.isViewInited) {
            let myLocation: LocationDirective = this.AllLocations.toArray().filter(d => d.Code == 'OVE')[0];
            if (myLocation != null) {
                SessionLocator.DynamicLoader.Load("./ShipmentModules/ShipmentAWB/Components/AWBWizard/Overview/AWBOverviewTabComponent", myLocation.viewContainerRef)
                    .then(cmpRef => {
                        cmpRef.instance.InitTab(this.EntityPM, this);
                    });
            }
        }
    }

    public ValidateFSR() {
        var isValid = true;
        var warnings: string[] = [];
        var msg = TextCodeTranslator.Translate("General.M.FieldIsRequired");

        if (AppTool.IsNullOrEmpty(this.EntityPM.MainCarriageCarrierId)) {
            warnings.push(msg.replace("%FieldName", TextCodeTranslator.Translate("Shipment.O.Routings.Airline")));
        }

        if (AppTool.IsNullOrEmpty(this.EntityPM.Master) && AppTool.IsNullOrEmpty(this.EntityPM.MAWBStackNumber)) {
            warnings.push(msg.replace("%FieldName", TextCodeTranslator.Translate("Shipment.O.Routings.MAWB")));
        }

        this.ValidationWarningsList = warnings;

        if (warnings.length > 0) {
            isValid = false;
        }

        return isValid;
    }

    CloseButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    private myService: ShipmentPMService = null;
    public ReloadEntity() {

        this.CurrentSession.StartBusyIndicatorLoading();

        if (this.myService == null) {
            this.myService = new ShipmentPMService();
        }

        this.myService.get(this.EntityPM.Id).subscribe((myResponse:ServiceResponse) => {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    this.EntityPM = myResponse.Result;
                    this.LoadCompleted.emit(true);
                    this.CurrentSession.StopBusyIndicator();
                }

                else {
                    this.ValidationErrorsList = myResponse.ErrorsArray;
                    this.LoadCompleted.emit(false);
                    this.CurrentSession.StopBusyIndicator();
                }
            }
        });
    }
}
