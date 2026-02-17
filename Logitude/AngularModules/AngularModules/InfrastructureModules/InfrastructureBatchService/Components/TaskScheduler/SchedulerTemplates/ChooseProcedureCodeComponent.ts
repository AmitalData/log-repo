import { Component, Output, EventEmitter } from '@angular/core';
import { BaseComponent } from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { Validator } from '../../../../../Infrastructure/Validators/Validator';
import { SessionLocator } from '../../../../../Infrastructure/Utilities/SessionLocator';
import { ServiceResponse } from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import { TenantPM } from '../../../../../Common/EntityPMs/TenantPM';
import { DateTool, AppTool } from '../../../../../Infrastructure/Tools';
//import { ComputingPartnerPM } from '../../../Common/EntityPMs/ComputingPartnerPM';
import { ObservableCollection } from '../../../../../Infrastructure/Utilities/ObservableCollection';
//import { ComputingPartnerTablePM } from '../../../../../Common/EntityPMs/ComputingPartnerTablePM';
import { LogitudeWindow } from '../../../../../Controls/Windows/LogitudeWindow';
//import { ComputingPartnerTranslationPMService } from '../../../../../Common/Services/StandardPMs/ComputingPartnerTranslationPMService';
import { EntityListService } from '../../../../../Infrastructure/Services/EntityListService';
import { CommonDomainService, CustomApiQueryFilters, TranslationItem } from '../../../../../Common/Services/CommonDomainService';
import { ComputingPartnerTranslationPM } from '../../../../../Common/EntityPMs/ComputingPartnerTranslationPM';
import { SchedulerProcedureList } from '../../../../../Infrastructure/EntityLists/SchedulerProcedureList';
import { SchedulerProcedureListService } from '../../../../../Infrastructure/Services/StandardLists/SchedulerProcedureListService';
import { ApiQueryFilters, FilterItem } from '../../../../../Infrastructure/DataContracts/ApiQueryFilters';


declare var window: any;

@Component({
    moduleId: module.id,

    selector: 'ChooseProcedureCodeComponent',
    templateUrl: './ChooseProcedureCodeComponent.html',
})

export class ChooseProcedureCodeComponent {
    searchFieldFilter: FilterItem;
    private CurrentSession = SessionLocator.SelectedSession;
    private schedulerprocedurelistservice: SchedulerProcedureListService;
    private _entityListService: EntityListService = new EntityListService();
    public schedulerprocedureList: SchedulerProcedureList[] = [];
    public searchFields: string;
    public searchText: string = "";
    public ObjectTableName: string;

    constructor() {
        this.schedulerprocedurelistservice = new SchedulerProcedureListService();
        this.LoadData();

    }

    private timerToken: any;
    TextChanged(searchtext) {
        if (!AppTool.IsNullOrEmpty(searchtext)) {



            this.timerToken = setTimeout(() => {
                this.searchText = searchtext;
                this.LoadData();
            }, 500);
        }


        else {
            this.searchText = null;
            this.LoadData();
        }
    }

    LoadData() {
        this.schedulerprocedureList = [];

        if (AppTool.IsNullOrEmpty(this.searchText)) {
            this.schedulerprocedurelistservice.getAll().subscribe((myResponse: ServiceResponse) => {
                if (myResponse != null) {
                    if (!myResponse.HasError) {

                        this.schedulerprocedureList = myResponse.Result;




                    }
                }
            });

        }
        else {

            this.schedulerprocedurelistservice.getAll().subscribe((myResponse: ServiceResponse) => {
                if (myResponse != null) {
                    if (!myResponse.HasError) {

                        this.schedulerprocedureList = myResponse.Result;

                        this.schedulerprocedureList = this.schedulerprocedureList.filter(d => d.SearchFields.toLowerCase().includes(this.searchText.toLowerCase()));


                    }
                }
            });



        }
    }

    SendCode(item: any) {

        this.CurrentSession.CloseCurrentWindowEmit(item.Code);

    }


    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

}
