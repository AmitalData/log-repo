
declare var window: any;
import { EditComponent } from "../../../Infrastructure/Components/EditComponent/EditComponent";
import { WebFreightDomainService } from '../../../Infrastructure/Services/WebFreightDomainService';
import { ApiQueryFilters } from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import { Component, ChangeDetectorRef } from '@angular/core';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { ARPaymentExtendedListService } from '../../../Invoice/Services/ExtendedLists/ARPaymentExtendedListService';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { AppTool } from '../../../Infrastructure/Tools';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';
import { MessageWindow } from '../../../Controls/Windows/MessageWindow';
import { ConfirmWindow } from '../../../Controls/Windows/ConfirmWindow';
import { CustomsRequestMenuService } from '../../../Customs/Services/Others/CustomsRequestMenuService';
import { CustomsRequestSheetExtendedPMService } from '../../../Customs/Services/ExtendedPMs/CustomsRequestSheetExtendedPMService';
import { SendALLCorrectRequestParams } from '../../../Customs/DataContract/RequestParams/SendALLCorrectRequestParams';
import { ResponseDataBase } from '../../../Customs/DataContract/ResponseData/ResponseDataBase';
import { CustomsRequestsSheetPM } from '../../../Customs/EntityPMs/CustomsRequestsSheetPM';
import { CourierMasterService } from '../../../Customs/Services/Others/CourierMasterService';
import { SendRequestVIA } from '../../../Customs/DataContract/RequestParams/RequestParamsBase';
import { ShowProgressBarParams, CustomMessageProgressComponent } from '../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent';
import { DeclarationWebService } from '../../../Customs/Services/WebServices/DeclarationWebService';

@Component({
    moduleId: module.id,
    templateUrl: './CourierDeclarationWorkspaceListTemplate.html',
})

export class CourierDeclarationWorkspaceListTemplate {

    //_CourierWorksheet: DeclarationCourierStatusList;
    public fieldName: any;

    
    //private _DeclarationCourierStatusPMService: DeclarationCourierStatusPMService = new DeclarationCourierStatusPMService();
    ////private declarationPendingPMService: DeclarationPendingPMService = new DeclarationPendingPMService();
    //private _CourierMasterService: CourierMasterService = new CourierMasterService();
    //private _DeclarationMamanSpecialActionListService: DeclarationMamanSpecialActionListService = new DeclarationMamanSpecialActionListService();
    //private _DeclarationMamanSpecialActionPMService: DeclarationMamanSpecialActionPMService = new DeclarationMamanSpecialActionPMService;
    //private _DeclarationWebService: DeclarationWebService = new DeclarationWebService;
    //private _DeclarationCourierStatusWebService: DeclarationCourierStatusWebService = new DeclarationCourierStatusWebService();
    //_DeclarationExtendedListService: DeclarationExtendedListService = new DeclarationExtendedListService();
    

    FirePreventSelect() {
        SessionLocator.SelectedSession.PseventRowSelectEvent.emit("CourierWorksheetListTemplate.SendSplitButton");
    }
    FireUnSelect() {
        SessionLocator.SelectedSession.PseventRowSelectEvent.emit("FireUnSelect");
    }

    //  @ViewChild( SplitButtonComponent)  public MySplitButtonComponent: SplitButtonComponent = new SplitButtonComponent(null,null);
    //@ViewChild('ShortTitle', { read: ViewContainerRef }) ShortTitleViewContainerRef: ViewContainerRef;
    //@ViewChild('MySplitButtonComponent', { read: SplitButtonComponent }) MySplitButtonComponent: SplitButtonComponent;

    constructor(private CD: ChangeDetectorRef) {
        
    }

}
