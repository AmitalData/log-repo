declare var window: any;
import { Component, Output, EventEmitter, OnInit, ComponentRef } from '@angular/core';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { EntityResourceService } from '../../../Infrastructure/Services/EntityResourceService';
import { FeatureLocator } from '../../../Infrastructure/Utilities/FeatureLocator';
import { AppTool } from '../../../Infrastructure/Tools';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';
import { ListComponentArgs } from '../../../Infrastructure/Args';
import { CustomsClosedTableList } from '../../../Customs/EntityLists/CustomsClosedTableList';

import { ApiQueryFilters } from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import { EntityListService } from '../../../Infrastructure/Services/EntityListService';
import { IIGGeneralMessagesService } from '../../../Customs/Services/WebServices/IIGGeneralMessagesService';
import { SystemTableRequestParams } from '../../../Customs/DataContract/RequestParams/SystemTableRequestParams';
import { SendRequestVIA } from '../../../Customs/DataContract/RequestParams/RequestParamsBase';

import { ObservableCollection } from '../../../Infrastructure/Utilities/ObservableCollection';

@Component({
    moduleId: module.id,
    templateUrl: './ClosedTableNotExistedComponent.html',
})





export class ClosedTableNotExistedComponent implements OnInit {

    public DataContext: ClosedTableNotExistedComponent = this;
    public ObjectTableName: string = "Customs.CustomsClosedTable";
    public columns: any[] = null;

    _ObservableList: ObservableCollection;
    _CustomsClosedTable: CustomsClosedTableList;
    //private _entityResourceService: EntityResourceService = new EntityResourceService();
    ///public ComponentRef: ComponentRef<ClosedTableNotExistedComponent>;

    IsShowTipArea: boolean;

    rowCount: any;
    _Rows: SYSTBL_NG_9001_MSG_SystemTablesResponseTableData[] = [];
    
    constructor() {
        
        this._ObservableList = new ObservableCollection([]);
     

    }
    ngOnInit() {
        //this._entityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe(response => {
            //this._entityListService = new EntityListService();
            
            //this.RefreshBtnClick()
        //});
            let systemTableRequestParams = new SystemTableRequestParams();
            systemTableRequestParams.TableId = this._CustomsClosedTable.Id;
            systemTableRequestParams.Tenant = SessionLocator.Tenant;
            systemTableRequestParams.RequestVIA == SendRequestVIA.WebServiceBatch;//all the time 

            let myIIGGeneralMessagesService = new IIGGeneralMessagesService();
            myIIGGeneralMessagesService.PostFillNotExistedClosedTables(systemTableRequestParams).subscribe(
                res => {
                    this._Rows = res.Result;
                    //this._ObservableList.InsertCollection(res.Result);
                    this.rowCount = this._Rows.length;
                    this.filterRows();
                }
            )
    }
    
    SetWindowArgs(customsClosedTable: CustomsClosedTableList) {
        this._CustomsClosedTable = customsClosedTable;

    }
    
 
 
  
    _SearchText: string;
    onSearchTextChangeEvent(text: string) {
        this._SearchText = text;
        this.filterRows();
    }
    filterRows() {
        var itemsSource = this._Rows;
        if (AppTool.IsNullOrEmpty(this._SearchText)) {
            //this.ItemsSource = this._CustomsRequestMenuService.CustomsRequestMenuItems;
        }
        else {
            itemsSource = itemsSource.filter(f => !AppTool.IsNullOrEmpty(f.nameField ));

            itemsSource = itemsSource.filter(f => f.nameField.toUpperCase().includes(this._SearchText.toUpperCase()) || f.idField.toUpperCase().includes(this._SearchText.toUpperCase()));
        }
        this._ObservableList.Clear();
        this._ObservableList.InsertCollection(itemsSource)
        
        
    }
  
}
class SYSTBL_NG_9001_MSG_SystemTablesResponseTableData {
    extraStringDataField: string;
    idField: string;
    nameField: string;
    stateField: number;
    
}