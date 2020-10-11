
declare var System: any;
declare var window: any;


import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {Component, OnInit}  from '@angular/core';

import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {CommunicationLogPM} from '../../../../Common/EntityPMs/CommunicationLogPM';
import {DocumentExtendedService} from '../../../../Common/Services/ExtendedPMs/DocumentExtendedService';
import {ServiceHelper} from '../../../../Infrastructure/Utilities/ServiceHelper';
import {DownloadManager} from '../../../../Infrastructure/Utilities/DownloadManager';
import {ImageLibraryService} from '../../../../Common/Services/Others/ImageLibraryService';


import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';

import {UIProperty, UIProperties}  from '../../../../Infrastructure/Components/LogitudeComponents/UIProperties';

import { FormGroup, FormBuilder} from '@angular/forms';


import {AppTool} from '../../../../Infrastructure/Tools';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';

@Component({
    
    selector: 'CommunicationLogMessageBody',
    templateUrl: './CommunicationLogMessageBodyComponent.html',
    providers: [DocumentExtendedService, ImageLibraryService ],
})

export class CommunicationLogMessageBodyComponent extends BaseComponent implements OnInit {
    public EntityPM: CommunicationLogPM;
    public myForm: FormGroup;
    public MessageBody: string;
    public ResponseBody: string;
    public Logs: string;
    MessageWidth: string;
    constructor(public entityArgs: EntityArgs, fb: FormBuilder, public _documentExtendedService: DocumentExtendedService, public _imageLibraryService: ImageLibraryService) {
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
            if (!this.EntityPM.IsBodySecured) {
                this.GetMessageBodyFileName();
            }
            else {
                this.MessageBody = "The body of this message is secured and cannot be displayed";
            }
            if (!AppTool.IsNullOrEmpty(this.EntityPM.ResponseDocumentId)) {
                this.GetResponseBodyFileName();
            }

            if (this.EntityPM.To == "FTP")
                this.Logs = this.EntityPM.Logs;


            this.Logs = this.EntityPM.Logs;//always 
        }

       

  

    }



    GetMessageBodyFileName() {

        this._documentExtendedService.GetDocumentById(this.EntityPM.DocumentId, this.EntityPM.Tenant).subscribe((res:any) => {

            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                var document = pmResponse.Result;
                if (document) {
                    var filename = document.Id + "." + document.Extension;
                    this.UpdateScreen(document, "MessageBody");
                }

            }

        });

    }

    GetResponseBodyFileName() {

        this._documentExtendedService.GetDocumentById(this.EntityPM.ResponseDocumentId, this.EntityPM.Tenant).subscribe((res:any) => {

            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                var document = pmResponse.Result;
                if (document) {
                    var filename = document.Id + "." + document.Extension;
                    this.UpdateScreen(document,"ResponseBody");
                }

            }

        });

    }

  
    UpdateScreen(document: any , type:string) {

        this._imageLibraryService.DownloadFile(document.Id, document.Extension, document.Folder, this.EntityPM.Tenant).subscribe((res:any) => {

            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    if (type == "MessageBody") {
                        this.MessageBody = myResult;
                    } if (type == "ResponseBody") {
                        this.ResponseBody = myResult;
                    }
                    //window.atob(myResult);
                }

            }
        });
    }

     
    ViewMessageBodyButtonClicked() {
        if (this.EntityPM && !this.EntityPM.IsBodySecured) {
            DownloadManager.DownloadPage(this.EntityPM.DocumentId);
        }
    } 

 
    ViewResponseBodyButtonClicked() {
        if (this.EntityPM && !AppTool.IsNullOrEmpty(this.EntityPM.ResponseDocumentId)) {

            DownloadManager.DownloadPage(this.EntityPM.ResponseDocumentId);
        }
    } 


}






