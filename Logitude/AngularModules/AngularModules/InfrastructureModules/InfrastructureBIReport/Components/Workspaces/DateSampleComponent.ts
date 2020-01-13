import { Component, Input, OnInit, ChangeDetectorRef, OnDestroy, Directive, Output, EventEmitter } from '@angular/core';
import { DWObjectFieldsDetails } from '../../../../CommonModules/CommonOthers/Components/DWQueryBuilder/DWQueryBuilderComponent';
import { DWObjectTablePMService } from '../../../../Infrastructure/Services/StandardPMs/DWObjectTablePMService';
import { DWObjectFieldExtendedPMService } from '../../../../Infrastructure/Services/ExtendedPMs/DWObjectFieldExtendedPMService';
import { DWQueryBuilderService } from '../../../../Infrastructure/Services/ExtendedPMs/DWQueryBuilderService';
import { DWQueryBuilderHelper } from '../../../../Infrastructure/Helpers/DWQueryBuilderHelper';
import { DWQueryData } from '../../../../Common/DataContracts/DWQueryData';
import { DWSubQueryPMService } from '../../../../Infrastructure/Services/StandardPMs/DWSubQueryPMService';
import { AppTool } from '../../../../Infrastructure/Tools';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';

@Component({
    selector: 'DateSampleComponent',
    moduleId: module.id,
    templateUrl: './DateSampleComponent.html',
    inputs: ['ShowSampleDateCommand','OrigionalDate']
})

export class DateSampleComponent implements OnInit {
 
    DateSample = ""; 
    //DateFilter: DWObjectFieldsDetails
    public _DWQueryBuilderService: DWQueryBuilderService;
    public ShowSampleDateCommand: EventEmitter<any>;

    constructor() {
        this._DWQueryBuilderService = new DWQueryBuilderService();
     }

    ngOnInit() { 
        
        if (this.ShowSampleDateCommand) {
            this.ShowSampleDateCommand.subscribe((DateFilter) => {
                this._DWQueryBuilderService.GetDateFilterSample(DateFilter).subscribe(myResult => {
                    if (!myResult.HasError) {
                        this.DateSample = myResult.Result;
                        //SessionLocator.SelectedSession.StopBusyIndicator();
                        //this.RunReportComplete.emit({ rowData: this.rowData, Msg: "MT5000", Count: this.count });// more than 50000
                    }
                    else {
                        //SessionLocator.SelectedSession.StopBusyIndicator();
                    }
                });
            });
        }
    }


    private origionalDate: any;// = "";
    public get OrigionalDate() { return this.origionalDate; }
    public set OrigionalDate(newValue: any) {
        this.origionalDate = newValue;
        this._DWQueryBuilderService.GetDateFilterSample(this.origionalDate).subscribe(myResult => {
            if (!myResult.HasError) {
                this.DateSample = myResult.Result;
                //SessionLocator.CurrentSession.StopBusyIndicator();
                //this.RunReportComplete.emit({ rowData: this.rowData, Msg: "MT5000", Count: this.count });// more than 50000
            }
            else {
                //SessionLocator.CurrentSession.StopBusyIndicator();
            }
        });
    }
}

