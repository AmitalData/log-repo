declare var window: any;
import { EditComponent } from "../../../Infrastructure/Components/EditComponent/EditComponent";
import { WebFreightDomainService } from '../../../Infrastructure/Services/WebFreightDomainService';
import { ApiQueryFilters } from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import { Component, ChangeDetectorRef } from '@angular/core';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { CourierMasterList } from '../../../Customs/EntityLists/CourierMasterList';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { AppTool } from '../../../Infrastructure/Tools';


@Component({
    moduleId: module.id,
    templateUrl: './CourierDeclarationWorkspaceListTemplate.html',
})

export class CourierDeclarationWorkspaceListTemplate {

    _CourierMasterList: CourierMasterList;
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
    setVariables(courierMasterList: CourierMasterList, fieldName: string) {
        this._CourierMasterList = courierMasterList;
        this.fieldName = fieldName;
    }
}
