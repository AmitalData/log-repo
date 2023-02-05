import { ConfirmWindow } from '../../../../Controls/Windows/ConfirmWindow';
import { MessageWindow } from '../../../../Controls/Windows/MessageWindow';
import { Component, Output, EventEmitter, OnInit, ComponentRef, ViewChild} from '@angular/core';
import { AppTool, DateTool } from '../../../../Infrastructure/Tools';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';
import { CourierMasterPMService } from '../../../../Customs/Services/StandardPMs/CourierMasterPMService';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { SendALLStorageSiteRequestParams } from '../../../../Customs/DataContract/RequestParams/SendALLStorageSiteRequestParams';
import { CourierMasterPM } from '../../../../Customs/EntityPMs/CourierMasterPM';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { CourierMasterService } from 'Customs/Services/Others/CourierMasterService';
import { QueueMessagesWebService } from 'Infrastructure/Services/WebServices/QueueMessagesWebService';

@Component({
    selector: 'UpdatePriorityComponent',
    templateUrl: './UpdatePriorityComponent.html',
})

export class UpdatePriorityComponent extends BaseComponent {
    public DataContext: UpdatePriorityComponent = this;
    public ObjectTableName: string = "Customs.CourierMaster";
    public ValidationErrorsList: string[];
    CourierMasterPM: CourierMasterPM = new CourierMasterPM();

    _CourierMasterService: CourierMasterService = new CourierMasterService();
    _CourierMasterPMService: CourierMasterPMService = new CourierMasterPMService();
    numbers=[1,2,3,4,5,6,7,8,9,10];
    num:number=1;
    InterfaceTypeCode:string;
    constructor(private _entityResourceService: EntityResourceService, public entityArgs: EntityArgs) {
        super();
        
    }

    SetWindowArgs(args: any) {
        if (!AppTool.IsNullOrEmpty(args)) {
            this.CourierMasterPM = args.CourierMasterPM;
            this.InterfaceTypeCode=args.InterfaceTypeCode 

        }
    }
   
   
  
   
    OkButtonClicked() {
      
      
        var service = new QueueMessagesWebService();

        service.UpdateTenantPriorityStatistics(this.CourierMasterPM.Tenant, this.CourierMasterPM.Id,this.InterfaceTypeCode,this.num).subscribe((myResult: ServiceResponse) => {
            var myResponse: ServiceResponse = myResult;

            if (!myResponse.HasError) {
                            
            }
        });

        var Message = new MessageWindow()
        Message.RTL = true;
        Message.Show("העדכון יבוצע בתהליך רקע");
        Message.WindowClosed.subscribe(($event: any) => {
            SessionLocator.SelectedSession.CurrentWindow.Close("0");
        });
         



    }
    
    CancelButtonClicked() {
        SessionLocator.SelectedSession.CloseCurrentWindow();
    }

    



}
