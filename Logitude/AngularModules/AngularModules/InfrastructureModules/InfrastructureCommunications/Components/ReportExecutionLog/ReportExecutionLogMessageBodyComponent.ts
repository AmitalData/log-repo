declare var System: any;
declare var window: any;

import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms'; 
import { ReportExecutionLogPM } from '../../../../Common/EntityPMs/ReportExecutionLogPM';
import { ReportsTemplatePMExtendedService } from '../../../../Common/Services/ExtendedPMs/ReportsTemplatePMExtendedService';
import { ImageLibraryService } from '../../../../Common/Services/Others/ImageLibraryService';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { AppTool } from '../../../../Infrastructure/Tools';
import { DownloadManager } from '../../../../Infrastructure/Utilities/DownloadManager';

@Component({
    selector: 'ReportExecutionLogMessageBody',
    templateUrl: './ReportExecutionLogMessageBodyComponent.html',
    providers: [ReportsTemplatePMExtendedService, ImageLibraryService],
})
export class ReportExecutionLogMessageBodyComponent extends BaseComponent implements OnInit {
    public EntityPM: ReportExecutionLogPM;
    public myForm: FormGroup;
    public MessageBody: string;
    public ResponseBody: string;
    public Logs: string;
    MessageWidth: string;
    constructor(public entityArgs: EntityArgs, fb: FormBuilder, public reportsTemplatePMExtendedService: ReportsTemplatePMExtendedService, public _imageLibraryService: ImageLibraryService) {
        super();
        this.myForm = fb.group({});
        if (window.innerWidth > 1380) {
            this.MessageWidth = "1380px";
        }
        else {
            this.MessageWidth = (window.innerWidth - 250).toString();
        }
    }
    ngOnInit() {
        this.EntityPM = this.entityArgs.EntityPM;
        if (this.EntityPM) { 
            this.MessageBody = this.EntityPM.ReportFilterXML;
        }
        
    } 
   
}
