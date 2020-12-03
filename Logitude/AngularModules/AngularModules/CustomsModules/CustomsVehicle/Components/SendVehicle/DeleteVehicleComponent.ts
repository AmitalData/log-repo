declare var window: any;
declare var System: any;
import { Component, Output, EventEmitter, Input } from '@angular/core';
import { ObjectTablePM } from '../../../../Infrastructure/EntityPMs/ObjectTablePM'
import { SendRequestVIA } from '../../../../Customs/DataContract/RequestParams/RequestParamsBase';
import { CustomMessageProgressComponent } from '../../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent';
import { ClientSearchResponseData } from '../../../../Customs/DataContract/ResponseData/ClientSearchResponseData';
import { EntityPMService } from '../../../../Infrastructure/Services/EntityPMService';
import { VehiclePM } from '../../../../Customs/EntityPMs/VehiclePM';
import { SendVehicleComponent } from './SendVehicleComponent';

@Component({
    
    selector: 'DeleteVehicleComponent',
    templateUrl: "DeleteVehicleComponent.html",
})

export class DeleteVehicleComponent {

    //-----------------Properties----------------------------//

    ObjectTable: ObjectTablePM;
    ValidationErrors: string[];
    EntityPM: VehiclePM;

    RequestVIA: SendRequestVIA;
    ForcePersonalSign: boolean;
    Option: string;
    ResponseData: ClientSearchResponseData;
    //------------------------------------------------------//
    ObjectTableName = "Customs.Vehicle";
    constructor(private entityPMService: EntityPMService) {

    }
    Run(args: any) {
        this.EntityPM = args.EntityPM;
        this.ObjectTable = args.ObjectTable;
    }

    OnCustomSendOptionsButtonClick(customSendOptionsArgs) {
        this.RequestVIA = customSendOptionsArgs.RequestVIA;
        this.Option = customSendOptionsArgs.Option;
        this.ForcePersonalSign = customSendOptionsArgs.ForcePersonalSign;

        var sendVehicleComponent: SendVehicleComponent = new SendVehicleComponent(new EntityPMService());
        sendVehicleComponent.SaveEntityChanges(customSendOptionsArgs, this.EntityPM, true);
    }

  




}
