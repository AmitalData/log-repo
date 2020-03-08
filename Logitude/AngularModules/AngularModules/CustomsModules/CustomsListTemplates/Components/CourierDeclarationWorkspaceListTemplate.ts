declare var window: any;
import { EditComponent } from "../../../Infrastructure/Components/EditComponent/EditComponent";
import { WebFreightDomainService } from '../../../Infrastructure/Services/WebFreightDomainService';
import { ApiQueryFilters } from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import { Component, ChangeDetectorRef } from '@angular/core';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { CourierMasterList } from '../../../Customs/EntityLists/CourierMasterList';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { AppTool } from '../../../Infrastructure/Tools';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';
import { EntityResourceService } from '../../../Infrastructure/Services/EntityResourceService';
import { CourierMasterPMService } from '../../../Customs/Services/StandardPMs/CourierMasterPMService';


@Component({
    moduleId: module.id,
    templateUrl: './CourierDeclarationWorkspaceListTemplate.html',
})

export class CourierDeclarationWorkspaceListTemplate {

    _CourierMasterList: CourierMasterList;
    public fieldName: any;
    
    private _EntityResourceService: EntityResourceService = new EntityResourceService();
    private _CourierMasterPMService: CourierMasterPMService = new CourierMasterPMService();


    constructor(private CD: ChangeDetectorRef) {
        
    }

    setVariables(courierMasterList: CourierMasterList, fieldName: string) {
        this._CourierMasterList = courierMasterList;
        this.fieldName = fieldName;
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
                        //AmitalGatewayUtil.Instance.IsAmitalBackButtonDisable = false;
                        //this.isEditControlOpened = false;
                        //this.OnBackFromEdit(selectedEntityId, $event);
                    });
                }
            });
        });
    }
}
