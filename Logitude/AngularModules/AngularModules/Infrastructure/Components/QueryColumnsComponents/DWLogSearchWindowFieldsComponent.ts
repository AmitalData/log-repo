declare var window: any;
import {Component, ViewContainerRef, OnInit, ViewChildren, QueryList, Output, EventEmitter, ChangeDetectorRef} from '@angular/core';
import {CommonDomainService} from '../../../Common/Services/CommonDomainService';
import {TenantPM} from '../../../Common/EntityPMs/TenantPM';
import {InfraSettings} from '../../../Infrastructure/Utilities/InfraSettings';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {PartnersDomainService} from '../../../Common/Services/PartnersDomainService';
import {EntityListService} from '../../../Infrastructure/Services/EntityListService';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {CardList} from '../../../Common/EntityLists/CardList';
import {CachedDataManager} from '../../../Infrastructure/Utilities/CachedDataManager';
import {AppTool} from '../../../Infrastructure/Tools';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import {TextCodeTranslator} from '../../Utilities/TextCodeTranslator';
import {MessageWindow} from '../../../Controls/Windows/MessageWindow';
import {FeatureLocator} from '../../Utilities/FeatureLocator';
import {ObjectTablePM} from '../../EntityPMs/ObjectTablePM';

@Component({
    moduleId: module.id,

    selector: 'DWLogSearchWindowFieldsComponent',
    templateUrl: './DWLogSearchWindowFieldsComponent.html',
})

export class DWLogSearchWindowFieldsComponent implements OnInit {

    public rowData: any;
    public fieldName: any;
    public LookUpTableName: any;
    public LookUpTable: ObjectTablePM;
    private PartnerTypes: Array<any> = [];
    constructor(private CD: ChangeDetectorRef, private _entityListService: EntityListService) {
    }

    setVariables(rowData: any, fieldName: string) {
        this.rowData = rowData;
         
        this.fieldName = fieldName;
       
        var isDestroyed: boolean = this.CD['destroyed'];
        if (!isDestroyed) {
            this.CD.detectChanges();
        }
    }

    ngOnInit() {

    }


}