declare var window: any;
import { Component, ChangeDetectorRef } from '@angular/core';
import { WebFreightDomainService } from '../../../Infrastructure/Services/WebFreightDomainService';
import { ServiceArgs } from '../../../Infrastructure/DataContracts/ServiceArgs';
import { OnInit, Output, EventEmitter, ComponentRef, QueryList } from '@angular/core';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';

import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { AppTool, DateTool } from '../../../Infrastructure/Tools';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';
import { MessageWindow } from '../../../Controls/Windows/MessageWindow';
import { ConfirmWindow } from '../../../Controls/Windows/ConfirmWindow';

import { ListComponentArgs } from '../../../Infrastructure/Args';
import { EntityResourceService } from '../../../Infrastructure/Services/EntityResourceService';

import { ClosedTableStatusListService } from '../../../Customs/Services/StandardLists/ClosedTableStatusListService';
import { InterfaceManagementList } from '../../../Customs/EntityLists/InterfaceManagementList';
import { ClosedTableStatusList } from '../../../Customs/EntityLists/ClosedTableStatusList';
import { IIGGeneralMessagesService } from '../../../Customs/Services/WebServices/IIGGeneralMessagesService';
import { SystemTableRequestParams } from '../../../Customs/DataContract/RequestParams/SystemTableRequestParams';
import { SendRequestVIA } from '../../../Customs/DataContract/RequestParams/RequestParamsBase';

@Component({
    moduleId: module.id,
    templateUrl: './InterfaceManagementsListTemplate.html',
})

export class InterfaceManagementsListTemplate {
    _InterfaceManagementList: InterfaceManagementList;
    public fieldName: any;
    
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    constructor(private CD: ChangeDetectorRef) {
        

        //if (AppTool.IsNullOrEmpty(InterfaceManagementsListTemplate.translate_CommunicationLogBView)) {
        //    this._entityResourceService.getEntityResourceByTableName("CommunicationLog")
        //        .subscribe(response => {
        //            InterfaceManagementsListTemplate.translate_CommunicationLogBView = TextCodeTranslator.Translate("CommunicationLog.B.View");// itzik : Translate +_entityResourceService - its bad :due that i done this- 
        //        });
        //}
    }

    static translate_CommunicationLogBView: string = "";
    // itzik : Translate +_entityResourceService - its bad :due that i done this- 
    get CommunicationLogBView() {
        return InterfaceManagementsListTemplate.translate_CommunicationLogBView;
    }

    setVariables(InterfaceManagementList: InterfaceManagementList, fieldName: string) {
        ///console.log(rowData);
        this._InterfaceManagementList = InterfaceManagementList;
                                   
        this.fieldName = fieldName;
        this.RefreshFields();
    }
    SendOptionName: string = "";
    RefreshFields() {

        if (this._InterfaceManagementList.HasDefinition) {
            this.SendOptionName =this._InterfaceManagementList.TenantSendOptionName;
        }
        else {
            this.SendOptionName =this._InterfaceManagementList.DefaultSendOptionName;
        }
        //C	Company
        //N	None
        //P	Personal
        //p	Personal
        if (!AppTool.IsNullOrEmpty(this._InterfaceManagementList.SignatureTypeCode)) {
            if (this._InterfaceManagementList.SignatureTypeCode.toUpperCase() == "C") {
                this._InterfaceManagementList.SignatureTypeName = "חברתי";
            } else if (this._InterfaceManagementList.SignatureTypeCode.toUpperCase() == "C") {
                this._InterfaceManagementList.SignatureTypeName = "אישי";
            }
        }
            
          
        
        //else {
        //    this.TableUpdateButtonIsEnabled = false;
        //    this.TableUpdateButtonOpacity = "0.7";
        //}
        this.CD.detectChanges();
    }


    EditDetailsButtonClick() {
        alert("EditDetailsButtonClick");
    }
   
  
  

}
