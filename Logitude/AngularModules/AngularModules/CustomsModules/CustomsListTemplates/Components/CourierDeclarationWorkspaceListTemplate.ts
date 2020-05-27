declare var window: any;
import { EditComponent } from "../../../Infrastructure/Components/EditComponent/EditComponent";
import { WebFreightDomainService } from '../../../Infrastructure/Services/WebFreightDomainService';
import { ApiQueryFilters } from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import { Component, ChangeDetectorRef } from '@angular/core';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { CourierMasterList } from '../../../Customs/EntityLists/CourierMasterList';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { AppTool, DateTool } from '../../../Infrastructure/Tools';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';
import { EntityResourceService } from '../../../Infrastructure/Services/EntityResourceService';
import { CourierMasterPMService } from '../../../Customs/Services/StandardPMs/CourierMasterPMService';
import { CourierWorksheetSharedDataService } from "../../../Customs/Services/DataChange/CourierWorksheetSharedDataService";


@Component({    
    templateUrl: './CourierDeclarationWorkspaceListTemplate.html',
})

export class CourierDeclarationWorkspaceListTemplate {

    _CourierMasterList: CourierMasterList;
    public fieldName: any;
    
    private _EntityResourceService: EntityResourceService = new EntityResourceService();
    private _CourierMasterPMService: CourierMasterPMService = new CourierMasterPMService();
    public colorDate: string="Black";

    constructor(private CD: ChangeDetectorRef, public _CourierWorksheetSharedDataService: CourierWorksheetSharedDataService) {
        
    }

    setVariables(courierMasterList: CourierMasterList, fieldName: string) {
        this._CourierMasterList = courierMasterList;
        this.fieldName = fieldName;
        var today = DateTool.GetCurrentDateTimeAsUtc();
        today.setHours(0, 0, 0, 0);
        
        var estimatedArrivalDate = DateTool.GetDateFromDate(this._CourierMasterList.EstimatedArrivalDate);//.setHours(0, 0, 0, 0);
        estimatedArrivalDate.setHours(0, 0, 0, 0);

        this.colorDate = this._CourierMasterList.EstimatedArrivalColor;
        //if (this._CourierMasterList.EstimatedArrivalColor) {
        //    this.colorDate = "Blue";
        //} else {
        //    this.colorDate = "Red";
        //}
        //else if (estimatedArrivalDate.getTime() < today.getTime()) {
        //    this.colorDate = "Red";
        //}
        //else if (estimatedArrivalDate.getTime() === today.getTime()) {
        //    this.colorDate = "Blue";
        // } else {
            

        // }
      
        this.CD.detectChanges();
    }

    ShowCourierWorkSheet(event, courierMasterId: string, type: string) {

        //var selected = event.rowData;
        var windowArgs: any = {};
        this._EntityResourceService.getEntityResourceByTableName("Customs.DeclarationCourierStatus").subscribe(response => {
            this._CourierMasterPMService.get(courierMasterId).subscribe((myResponse: any) => {
                if (myResponse.HasError) {
                    console.log("Error while getting EntityPM", myResponse);
                }
                else {
                    windowArgs.CurrentEntity = myResponse.Result;
                    windowArgs.TabMode = type;
                    var logWindow = new LogitudeWindow();
                    logWindow.Width = 1500;
                    logWindow.Height = 1000;
                    logWindow.WindowArgs = windowArgs;
                    logWindow.ShowCloseButton = true;
                    logWindow.IsFillScreen = true;
                    logWindow.Show('./CustomsModules/CustomsCourier/Components/CourierWorkSheet/CourierWorksheetComponent');
                    logWindow.WindowClosed.subscribe(($event1: any) => {
                        this._CourierWorksheetSharedDataService.SendNextMessage("DoRefresh");

                        //AmitalGatewayUtil.Instance.IsAmitalBackButtonDisable = false;
                        //this.isEditControlOpened = false;
                        //this.OnBackFromEdit(selectedEntityId, $event);
                    });
                }
            });
        });
    }
}
