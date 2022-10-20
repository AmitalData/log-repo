
declare var System: any;
declare var window: any;


import {Component, OnInit}  from '@angular/core';

import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {TermsofUseSignaturePM} from '../../../../Common/EntityPMs/TermsofUseSignaturePM';
import {TermsofUseSignatureExtendedPM} from '../../../../Common/Services/ExtendedPMs/TermsofUseSignatureExtendedPM';
import {ServiceHelper} from '../../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {DownloadManager} from '../../../../Infrastructure/Utilities/DownloadManager';
import {SessionInfo} from '../../../../Infrastructure/Utilities/SessionInfo';
import { TermsofUseService } from '../../../../Infrastructure/Services/WebServices/TermsofUseService';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';
import { TermsofUsePM } from '../../../../Common/EntityPMs/TermsofUsePM';
import { Guid } from '../../../../Infrastructure/Utilities/Guid';
import { MessageWindow } from '../../../../Controls/Windows/MessageWindow';

declare var querySelection, resultToUnitArray: any;


@Component({
    templateUrl: './TermsofUseComponent.html',
})
export class TermsofUseComponent implements OnInit {

    private CurrentSession = SessionLocator.SelectedSession;
    TermsofUsePMLists: TermsofUsePMViewModel[] = [];
    TermsofUseSelectedViewModel: TermsofUsePMViewModel;
    private termsofUseService: TermsofUseService = new TermsofUseService();
    private entityResourceService: EntityResourceService = new EntityResourceService();
    public TermsOfUsePM: TermsofUsePM;
    VersionDocumentId: string = Guid.NewRandomString();

    constructor() {
        this.TermsOfUsePM = new TermsofUsePM();
    }

    ngOnInit() {
        this.GetTermsOfUse();
    }

    // Upload Terms Of Use
    OpenUpLoadTemplateFile() {
        let item = document.getElementById(this.VersionDocumentId) as HTMLInputElement;
        item.value = "";
        document.getElementById(this.VersionDocumentId).click();
    }

    FileName: string;
    UpLoadTemplateFileMethod(event: any) {

        var file = querySelection(this.VersionDocumentId);

        if (file) {
            var fileExtension = file.name.split('.')[1];
            this.FileName = file.name.split('.')[0];

            if (fileExtension) {
                if (!this.isPdfExtension(fileExtension)) {
                    this.ShowMessage("File extension must be pdf");
                } else {
                    this.ConvertArrayBufferToBase64(file, this);
                }
            }
        }

    }

    isPdfExtension(fileExtension: any) {
        return fileExtension.toLowerCase() == "pdf"
    }

    ViewFile(item: TermsofUsePMViewModel) {
        var documentName = item.DocumentId
        DownloadManager.DownloadPage(documentName);
    }

    ConvertArrayBufferToBase64(file: any, viewmodel: any) {
        var reader: FileReader = new FileReader();
        var reader = new FileReader();
        reader.onload = function (e) {
            var binary = '';
            var bytes = new Uint8Array(resultToUnitArray(e));
            var len = bytes.byteLength;
            for (var i = 0; i < len; i++) {
                binary += String.fromCharCode(bytes[i]);
            }
            viewmodel.createTermsOfUse(window.btoa(binary));
        };

        reader.onerror = function (e) {

        };
        reader.readAsArrayBuffer(file);
    }

    createTermsOfUse(file: any) {

        var termsofUsePM = new TermsofUsePM();
        termsofUsePM.FileData = file;
        termsofUsePM.Date = new Date();
        termsofUsePM.Tenant = 0;
        termsofUsePM.VersionDocumentName = this.FileName;

        this.InsertTermsOfUse(termsofUsePM);
    }

    private InsertTermsOfUse(termsofUsePM: TermsofUsePM) {
        this.termsofUseService.insert(termsofUsePM).subscribe((res: any) => {

            var response: ServiceResponse = res;
            var response: ServiceResponse = res;
            if (!response.HasError) {
                var myResult = response.Result;
                if (myResult) {
                    this.TermsofUsePMLists.push(new TermsofUsePMViewModel(termsofUsePM));
                }
            }
            else {
                this.HandleServiceError(response)
            }
        });
    }

    public ShowMessage(message: string) {
        var messageWindow: MessageWindow = new MessageWindow();
        messageWindow.Show(message);
    }

    GetTermsOfUse() {
        this.termsofUseService.GetTermsOfUseByTenant().subscribe((res: any) => {

            var serviceResponse: ServiceResponse = res;
            if (!serviceResponse.HasError) {
                var result = serviceResponse.Result;
                if (result && result != null) {
                    result.forEach((item) => {
                        this.TermsofUsePMLists.push(new TermsofUsePMViewModel(item));
                    });
                }
            }
            else {
                this.HandleServiceError(serviceResponse)
            }
        });
    }


    HandleServiceError(serviceResponse: ServiceResponse) {
        if (!serviceResponse.ErrorsArray && serviceResponse.ErrorsArray.length == 0) {
            return;
        }
        var messageWindow: MessageWindow = new MessageWindow();
        messageWindow.Show(serviceResponse.ErrorsArray[0]);

    }

}

class TermsofUsePMViewModel {


    Date: Date;
    VersionNumber: number;
    DocumentId: string;
    constructor(item: TermsofUsePM) {
        this.Date = item.Date;
        this.VersionNumber = item.VersionNumber;
        this.DocumentId = item.VersionDocumentId;
    }

}
