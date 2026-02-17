declare var window: any;
declare var System: any;
import { Component, Output, EventEmitter, Input } from '@angular/core';
import { ObjectTablePM } from '../../../../Infrastructure/EntityPMs/ObjectTablePM'
import { MenuButtonPM } from '../../../../Infrastructure/EntityPMs/MenuButtonPM'
import { MenuButtonGroupPM } from '../../../../Infrastructure/EntityPMs/MenuButtonGroupPM'
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator'
import { ServiceHelper } from '../../../../Infrastructure/Utilities/ServiceHelper'
import { AppTool, DateTool } from '../../../../Infrastructure/Tools'


import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';

import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';

import { Validator } from '../../../../Infrastructure/Validators/Validator';
import { GenericRequestParams } from '../../../../Customs/DataContract/RequestParams/GenericRequestParams';
import { UpdateDeleteVehicleRequestParams } from '../../../../Customs/DataContract/RequestParams/UpdateDeleteVehicleRequestParams';
import { SendRequestVIA } from '../../../../Customs/DataContract/RequestParams/RequestParamsBase';
import { CustomMessageProgressComponent } from '../../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent';
import { ClientSearchResponseData } from '../../../../Customs/DataContract/ResponseData/ClientSearchResponseData';
import { EntityPMService } from '../../../../Infrastructure/Services/EntityPMService';

import { AmitalGatewayUtil } from '../../../../Infrastructure/Utilities/AmitalGatewayUtil';
import { UnifreightController } from '../../../../Customs/Controller/UnifreightController'; 
import { IIGGeneralMessagesService } from '../../../../Customs/Services/WebServices/IIGGeneralMessagesService';
import { VehiclePM } from '../../../../Customs/EntityPMs/VehiclePM';
import { SendVehicleComponent } from './SendVehicleComponent';

@Component({
    moduleId: module.id,
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
        
        SendVehicleComponent.SaveEntityChanges(customSendOptionsArgs, this.EntityPM,true);
    }

  




}