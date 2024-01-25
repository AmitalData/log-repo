declare var window: any;
import {Component, AfterViewInit, ChangeDetectorRef, Output, Input}  from '@angular/core';

import { SessionInfo } from 'Infrastructure/Utilities/SessionInfo';
import { ImageLibraryService } from 'Common/Services/Others/ImageLibraryService';
import { BaseComponent } from 'Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ObservableCollection } from 'Infrastructure/Utilities/ObservableCollection';
import { EntityResourceService } from 'Infrastructure/Services/EntityResourceService';
import { TextCodeTranslator } from 'Infrastructure/Utilities/TextCodeTranslator';
import { ServiceResponse } from 'Infrastructure/DataContracts/ServiceResponse';
import { DeclarationEventManager } from 'Customs/Utilities/DeclarationEventManager';
import { AppTool } from 'Infrastructure/Tools';
import { MessageWindow } from 'Controls/Windows/MessageWindow';
import { CertificateOfOriginPM } from 'Customs/EntityPMs/CertificateOfOriginPM';
import * as xmljs from 'xml-js';
import { SessionLocator } from 'Infrastructure/Utilities/SessionLocator';


@Component({    
    templateUrl: './CertificateAnswersComponent.html',
    selector:"CertificateAnswers"
})

export class CertificateAnswersComponent extends BaseComponent  {
  
    public certificateOfOriginPM: CertificateOfOriginPM;
    Feedbacklist: ObservableCollection;
    Errorslist: ObservableCollection;
    Warninglist: ObservableCollection;

    FeedbackCount: string = "";
    ErrorsCount: string = "";
    WarningsCount: string = "";
    isLoad: boolean = false;

    //#endregion
    private CurrentSession = SessionLocator.SelectedSession;
    constructor( private EntityResourceService: EntityResourceService) {
        super();

        
        this.EntityResourceService.getEntityResourceByTableName("Customs.CertificateOfOrigin").subscribe((response:any) => {
            this.EntityResourceService.getEntityResourceByTableName("Customs.DeclarationConstraint").subscribe((response:any) => {
              this.isLoad = true
            });
        });

        

    }


    InitTab(EntityPM: CertificateOfOriginPM) {
        this.certificateOfOriginPM = EntityPM;
        this.Feedbacklist = new ObservableCollection([]);
        this.Errorslist  = new ObservableCollection([]);
        this.Warninglist = new ObservableCollection([]);
      
        this.SetAnswerDescreption();
 
        this.SetAnswerCount();
    }
    
    SetAnswerDescreption(){
         // Feedbacklist
         var descriptionFeedback = "";
         if(!AppTool.IsNullOrEmpty(this.certificateOfOriginPM.FeedbackRemark))
            descriptionFeedback += (this.certificateOfOriginPM.FeedbackRemark +" ")
         if(!AppTool.IsNullOrEmpty(this.certificateOfOriginPM.RejectCancelReason))
            descriptionFeedback += (this.certificateOfOriginPM.RejectCancelReason +" ")
         if(!AppTool.IsNullOrEmpty(descriptionFeedback))
            this.Feedbacklist.Insert(new LineModel(descriptionFeedback));
        
        
         if(!AppTool.IsNullOrEmpty(this.certificateOfOriginPM.ErrXml)) {
              const options = {
                 compact: true,
                 ignoreComment: true,
                 spaces: 4
               };
        
               const jsonResult = xmljs.xml2json(this.certificateOfOriginPM.ErrXml, options);
               const parsedObject = JSON.parse(jsonResult);
        
               parsedObject.ArrayOfException.Exception.forEach((item) => {
                   var descriptionErrorOrWarning = "";
                    if(!AppTool.IsNullOrEmpty(item.ExceptionParms))
                     descriptionErrorOrWarning += (item.ExceptionParms._text + " ")
                    if(!AppTool.IsNullOrEmpty(item.ExeptionDescription))
                      descriptionErrorOrWarning += (item.ExeptionDescription._text)
        
                    if(item.ExceptionLevel._text == '3') {
                      this.Errorslist.Insert(new LineModel(descriptionErrorOrWarning));
                    }
                    else if(item.ExceptionLevel._text == '1' || item.ExceptionLevel._text == '2') {
                      this.Warninglist.Insert(new LineModel(descriptionErrorOrWarning));
                    }
               });
            }
    }

    SetAnswerCount(){
        if (this.Feedbacklist.Length > 0) {
            this.FeedbackCount = "(" + this.Feedbacklist.Length + ")";
        }
        else {
            this.FeedbackCount = "";
        }
    
        if (this.Errorslist.Length > 0) {
            this.ErrorsCount = "(" + this.Errorslist.Length + ")";
        }
        else {
            this.ErrorsCount = "";
        }
    
        if (this.Warninglist.Length > 0) {
            this.WarningsCount = "(" + this.Warninglist.Length + ")";
        }
        else {
            this.WarningsCount = "";
        }
       
    }
 
}
export class LineModel extends BaseComponent {
    description:string
    constructor (Description: string) {
        super();
        this.description = Description;

        
    }
    get Description() { return this.description; }
    set Description(newValue: string) {
        this.description = newValue;
    }

}

