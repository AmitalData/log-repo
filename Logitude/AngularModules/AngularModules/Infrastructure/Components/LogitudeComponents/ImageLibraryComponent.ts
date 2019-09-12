import { Component, OnInit, ChangeDetectorRef, AfterViewInit, EventEmitter, Output } from '@angular/core';
import { MessageWindow } from '../../../Controls/Windows/MessageWindow';
import { SessionInfo } from '../../Utilities/SessionInfo';
import { FeatureLocator } from '../../../Infrastructure/Utilities/FeatureLocator';
import { SendHtmlDocumentFilter } from '../../../InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/DocsOut/Filters/SendHtmlDocumentFilter';
import { FroalaEditorSetting } from '../../../InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/DocsOut/FroalaEditorSetting';
import { DocumentOutPMService } from '../../../Common/Services/ExtendedPMs/DocumentOutPMService';
import { DocumentOutCopyViewModel } from '../../../InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/DocsOut/ViewModel/DocumentOutCopyViewModel';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { Guid } from '../../../Infrastructure/Utilities/Guid';
import { DocumentTypeListExtendedService } from '../../../Common/Services/ExtendedLists/DocumentTypeListExtendedService';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { AttachmentsList } from '../../../InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/DocsOut/Filters/AttachmentsList';
import { DocumentsFilingExtendedPMService } from '../../../Common/Services/ExtendedPMs/DocumentsFilingExtendedPMService';
import { EntityPartner } from '../../../Infrastructure/DataContracts/EntityPartner';
import { HtmlEditorService } from '../../../Common/Services/DocumentServices/HtmlEditorService';
import { DocumentTypeList } from '../../../Common/EntityLists/DocumentTypeList'
import { DocumentsFilingPM } from '../../../Common/EntityPMs/DocumentsFilingPM';
import { DocumentOutPM } from '../../../Common/EntityPMs/DocumentOutPM';
import { DocumentTypePM } from '../../../Common/EntityPMs/DocumentTypePM';
import { AttachmentDocment } from '../../../InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/DocsOut/ViewModel/AttachmentDocment';
import { DocumentExtendedService } from '../../../Common/Services/ExtendedPMs/DocumentExtendedService';
import { DocumentPM } from '../../../Common/EntityPMs/DocumentPM';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';
import { AppTool } from '../../../Infrastructure/Tools';
import { ServiceLocator } from '../../../Infrastructure/Locators/ServiceLocator';
import { Observable, Observer } from 'rxjs/Rx';

declare var System: any;
declare var window: any;
declare var htmlComponentProparitiesTrue, htmlComponentProparitiesFalse: any;

@Component({
    moduleId: module.id,
    selector: 'ImageLibraryComponent',
    templateUrl: './ImageLibraryComponent.html',

})


export class ImageLibraryComponent implements OnInit, AfterViewInit {
    private CurrentSession = SessionLocator.SelectedSession;

    Imageurls: string[] = [];
    ngAfterViewInit(): void {
        //throw new Error("Method not implemented.");
       
    }
    ngOnInit(): void {
        this.Imageurls.push('https://homepages.cae.wisc.edu/~ece533/images/airplane.png');
        this.Imageurls.push('https://homepages.cae.wisc.edu/~ece533/images/arctichare.png');
        this.Imageurls.push('https://homepages.cae.wisc.edu/~ece533/images/baboon.png');
        this.Imageurls.push('https://homepages.cae.wisc.edu/~ece533/images/cat.png');
        this.Imageurls.push('https://homepages.cae.wisc.edu/~ece533/images/girl.png');
        this.Imageurls.push('https://homepages.cae.wisc.edu/~ece533/images/watch.png');
        this.Imageurls.push('https://homepages.cae.wisc.edu/~ece533/images/airplane.png');
        this.Imageurls.push('https://homepages.cae.wisc.edu/~ece533/images/arctichare.png');
        this.Imageurls.push('https://homepages.cae.wisc.edu/~ece533/images/baboon.png');
        this.Imageurls.push('https://homepages.cae.wisc.edu/~ece533/images/cat.png');
        this.Imageurls.push('https://homepages.cae.wisc.edu/~ece533/images/girl.png');
        this.Imageurls.push('https://homepages.cae.wisc.edu/~ece533/images/watch.png');
        //throw new Error("Method not implemented.");
    }

    OnImageClick($imageUrl) {
        this.CurrentSession.CurrentWindow.Close($imageUrl);
        
        //if ($imageUrl) {
        //    this.getBase64ImageFromURL($imageUrl).subscribe(base64data => {
        //        console.log(base64data);
        //        // this is the image as dataUrl
        //        let base64Image:string  = 'data:image/jpg;base64,' + base64data;

        //        this.CurrentSession.CurrentWindow.Close(base64Image);
        //    });
        //}
        //else
        //    this.CurrentSession.CurrentWindow.Close(null);
    }

    getBase64ImageFromURL(url: string) {
        return Observable.create((observer: Observer<string>) => {
             
            // create an image object
            let img = new Image();
            img.crossOrigin = 'anonymous';
           // img.src = url;
            img.src = 'https://cors-anywhere.herokuapp.com/' + url;
            if (!img.complete) {
                // This will call another method that will create image from url
                img.onload = () => {
                    observer.next(this.getBase64Image(img));
                    observer.complete();
                }; img.onerror = (err) => {
                    observer.error(err);
                };
            } else {
                observer.next(this.getBase64Image(img));
                observer.complete();
            }
        });
    }

    getBase64Image(img: HTMLImageElement) {
        // We create a HTML canvas object that will create a 2d image
        var canvas = document.createElement("canvas");
        canvas.width = img.width;
        canvas.height = img.height;
        var ctx = canvas.getContext("2d");   // This will draw image    
        ctx.drawImage(img, 0, 0);
        // Convert the drawn image to Data URL
        var dataURL = canvas.toDataURL("image/png"); return dataURL.replace(/^data:image\/(png|jpg);base64,/, "");
    }
}
