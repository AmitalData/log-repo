declare var window: any;
import { Component, Output, EventEmitter, OnInit, ComponentRef } from '@angular/core';
import { BaseComponent } from       '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { TextCodeTranslator } from  '../../../Infrastructure/Utilities/TextCodeTranslator';
import { EntityResourceService } from '../../../Infrastructure/Services/EntityResourceService';
import { FeatureLocator } from '../../../Infrastructure/Utilities/FeatureLocator';
import { AppTool } from '../../../Infrastructure/Tools';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';
import { ListComponentArgs } from '../../../Infrastructure/Args';

import { ApiQueryFilters } from  '../../../Infrastructure/DataContracts/ApiQueryFilters';
import { ServiceResponse } from  '../../../Infrastructure/DataContracts/ServiceResponse';
import { EntityListService } from   '../../../Infrastructure/Services/EntityListService';





import { ObservableCollection } from '../../../Infrastructure/Utilities/ObservableCollection';
import { DocumentTypeCustomsDataPM } from '../../../Customs/EntityPMs/DocumentTypeCustomsDataPM';
import { DocumentTypeCustomsDataList } from '../../../Customs/EntityLists/DocumentTypeCustomsDataList';

//C: \LW\Customs\AngularModules\AngularModules\Customs\Services\StandardPMs\DocumentTypeCustomsDataPMService.ts
import { DocumentTypeCustomsDataPMService } from '../../../Customs/Services/StandardPMs/DocumentTypeCustomsDataPMService';
import { DocumentTypeCustomsDataListService } from '../../../Customs/Services/StandardLists/DocumentTypeCustomsDataListService';
import { DocumentTypeCustomsDataExtendPMService } from '../../../Customs/Services/ExtendedPMs/DocumentTypeCustomsDataExtendPMService';


@Component({
    moduleId: module.id,
    templateUrl: './GeneralLOVComponent.html',
})





export class GeneralLOVComponent
    extends BaseComponent
    implements OnInit {
    

    public DataContext: GeneralLOVComponent = this;
    public ObjectTableName: string = "";//"Customs.DocumentTypeCustomsData";
    public columns: any[] = null;



    private _DocumentTypeCustomsDataPMService: DocumentTypeCustomsDataPMService = new DocumentTypeCustomsDataPMService();
    private _DocumentTypeCustomsDataListService: DocumentTypeCustomsDataListService = new DocumentTypeCustomsDataListService();
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    

    
    entityPM: any;

    ValidationErrorsList: string[] = [];
    

    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
    }
    Loaded: boolean = false;
    EntityResource: boolean = false;
    ngOnInit() {
        ///this.CurrentSession.StartBusyIndicatorLoading();
        

    }
    LogitudeEntity: string;
    LogitudeEntityNumber: string;
    LOVText: string;
    SetWindowArgs(arg) {
        this.ObjectTableName = this.LogitudeEntity = arg.LogitudeEntity ;
        this.Code =this.LogitudeEntityNumber = arg.LogitudeEntityNumber;
        this.LOVText = arg.LOVText;

        this._entityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe(response => {
            this.CurrentSession.StopBusyIndicator();
            this.EntityResource = true;
            this.Loaded = true;

            if (AppTool.IsNullOrEmpty(this.Code)) {
                if (this.EntityResource && this.Loaded) {
                    
                }
            }
            
            
        });
    
    }
    ///#region Properties

    //IsUnifreightCertificateActivatedEnabled: boolean = true;


    _Code: string;
    get Code() { return this._Code }
    set Code(value: string) { this._Code = value; }
                             

    

         

    //#endregion

   
    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindowEmit("ShowGeneralLOVReturnSelectedCancel");
    }
        
    OkButtonClicked() {
        this.CurrentSession.CloseCurrentWindowEmit(this.Code);
    }
}
