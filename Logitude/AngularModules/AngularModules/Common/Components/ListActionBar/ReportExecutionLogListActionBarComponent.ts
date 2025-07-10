import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { ReportExecutionLogPMService } from 'Common/Services/StandardPMs/ReportExecutionLogPMService';
import { SessionLocator } from 'Infrastructure/Utilities/SessionLocator';

@Component({
    templateUrl: './ReportExecutionLogListActionBarComponent.html',

})
export class ReportExecutionLogListActionBarComponent implements OnInit {
    @Output() SelectedValueChanged = new EventEmitter();

    reportExecutionLogPMService : ReportExecutionLogPMService= new ReportExecutionLogPMService(); 
    constructor() { }

    ngOnInit(): void {
      
    }
    Cancle(){
        SessionLocator.SelectedSession.StartBusyIndicator("Canceling");
         this.reportExecutionLogPMService.Cancel(null).subscribe((response) => {
                   
           SessionLocator.SelectedSession.StopBusyIndicator();
        });            

    }

}