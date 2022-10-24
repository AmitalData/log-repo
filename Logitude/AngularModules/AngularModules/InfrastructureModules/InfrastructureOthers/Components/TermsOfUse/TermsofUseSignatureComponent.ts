
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
import { AppTool } from '../../../../Infrastructure/Tools';
@Component({
    

    selector: 'TermsofUseSignature',
    templateUrl: './TermsofUseSignatureComponent.html',
    providers: [TermsofUseSignatureExtendedPM]

})
export class TermsofUseSignatureComponent implements OnInit {


    TermsofUseSignaturePMLists: TermsofUseSignaturePMViewModel[];


    TermsofUseSignatureSelectedViewModel: TermsofUseSignaturePMViewModel;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public _termsofUseSignatureExtendedPM: TermsofUseSignatureExtendedPM) {
 

    }

    ngOnInit(


    ) {


    }


    SetDataContext(data: any) {
        this.LoadData();
    }



    LoadData() {
        this.CurrentSession.CurrentWindow.StartBusyIndicator("Loading...");
        this.TermsofUseSignaturePMLists = [];
        this._termsofUseSignatureExtendedPM.GetTermsofUseSignatures(SessionInfo.LoggedUserTenant, SessionInfo.LoggedUserId).subscribe((res:any) => {

            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {

                    myResult.forEach((item) => {
                        this.TermsofUseSignaturePMLists.push(new TermsofUseSignaturePMViewModel(item));
                    });

                }
                this.CurrentSession.CurrentWindow.StopBusyIndicator();

            }
            else {
                this.CurrentSession.CurrentWindow.StopBusyIndicator();
            }




           
           
        });
    }


    ViewFile(item: TermsofUseSignaturePMViewModel) {

        if (item.VersionDocumentId == null) {
            DownloadManager.DownloadPage(item.TermsOfUseId + "_termsofuses");
        } else {
            DownloadManager.DownloadTermsOfUse(null, item.VersionDocumentId);
        }

       
    }  
}

 


class TermsofUseSignaturePMViewModel {
      

    SignedDatetime: Date;
    TermsofUseVersion: number;
    VersionDocumentId: string;
    TermsOfUseId: number;

    constructor(item: TermsofUseSignaturePM) {
        this.SignedDatetime = item.SignedDatetime;
        this.TermsofUseVersion = item.VersionNumber;
        this.VersionDocumentId = item.VersionDocumentId;
        this.TermsOfUseId = item.TermsofUseId;
    }

}
