
declare var System: any;
declare var window: any;


import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {Component, OnInit}  from '@angular/core';

import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {APILogsPM} from '../../../../Infrastructure/EntityPMs/APILogsPM';
import {DocumentExtendedService} from '../../../../Common/Services/ExtendedPMs/DocumentExtendedService';
import {ServiceHelper} from '../../../../Infrastructure/Utilities/ServiceHelper';

import {ImageLibraryService} from '../../../../Common/Services/Others/ImageLibraryService';


import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';

import {UIProperty, UIProperties}  from '../../../../Infrastructure/Components/LogitudeComponents/UIProperties';

import { FormGroup, FormBuilder} from '@angular/forms';



import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';

@Component({
    moduleId: module.id,
    selector: 'APILogsResponceBody',
    templateUrl: './APILogsResponceBodyComponent.html',
    providers: [DocumentExtendedService, ImageLibraryService ],
})

export class APILogsResponceBodyComponent extends BaseComponent implements OnInit {
    public EntityPM: APILogsPM;
    public myForm: FormGroup;
    public MessageBody: string;
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
            this.MessageBody = this.EntityPM.ResponseData; 
        }



    }



    GetFileName() {
        var HeaderData = this.EntityPM.Tenant + "_" + this.EntityPM.Id + "_Res";// +"." + CurrentDocument.Extension;
        //string uri = SessionLocator.GetServerPath() + "/WebPages/APILogsDownLoadPage.aspx?header=" + HeaderData;

        var url = ServiceHelper.GetLogitudeURL() + "/WebPages/APILogsDownLoadPage.aspx?header=" + HeaderData;
        window.open(url);
        //this._documentExtendedService.GetDocumentById(this.EntityPM.DocumentId, this.EntityPM.Tenant).subscribe(res => {

        //    var pmResponse: ServiceResponse = res;
        //    if (!pmResponse.HasError) {
        //        var document = pmResponse.Result;
        //        if (document) {
        //            var filename = document.Id + "." + document.Extension;
        //            this.UpdateScreen(document);
        //        }

        //    }

        //});

    }

  
    UpdateScreen(document: any) {

        //this._imageLibraryService.DownloadFile(document.Id, document.Extension, document.Folder, this.EntityPM.Tenant).subscribe(res => {

        //    var pmResponse: ServiceResponse = res;
        //    if (!pmResponse.HasError) {
        //        var myResult = pmResponse.Result;
        //        if (myResult) {

        //            this.MessageBody = myResult;//window.atob(myResult);
        //            console.log(this.MessageBody);
        //        }

        //    }
        //});
    }


    ViewButtonClicked() {
        var HeaderData = this.EntityPM.Tenant + "_" + this.EntityPM.Id + "_Res";// +"." + CurrentDocument.Extension;
        //string uri = SessionLocator.GetServerPath() + "/WebPages/APILogsDownLoadPage.aspx?header=" + HeaderData;

        var url = ServiceHelper.GetLogitudeURL() + "/WebPages/APILogsDownLoadPage.aspx?header=" + HeaderData;
        window.open(url);

    } 

 



}






