
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
@Component({
    moduleId: module.id,

    selector: 'TermsofUseSignature',
    templateUrl: './TermsofUseSignatureComponent.html',
    providers: [TermsofUseSignatureExtendedPM]

})
export class TermsofUseSignatureComponent implements OnInit {


    TermsofUseSignaturePMLists: TermsofUseSignaturePMViewModel[];


    TermsofUseSignatureSelectedViewModel: TermsofUseSignaturePMViewModel;
    constructor(public _termsofUseSignatureExtendedPM: TermsofUseSignatureExtendedPM) {
 

    }

    ngOnInit(


    ) {


    }


    SetDataContext(data: any) {
        this.LoadData();
    }



    LoadData() {
        SessionLocator.CurrentSession.CurrentWindow.StartBusyIndicator("Loading...");
        this.TermsofUseSignaturePMLists = [];
        this._termsofUseSignatureExtendedPM.GetTermsofUseSignatures(SessionInfo.LoggedUserTenant, SessionInfo.LoggedUserId).subscribe(res => {

            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {

                    myResult.forEach((item) => {
                        this.TermsofUseSignaturePMLists.push(new TermsofUseSignaturePMViewModel(item));
                    });

                }
                SessionLocator.CurrentSession.CurrentWindow.StopBusyIndicator();

            }
            else {
                SessionLocator.CurrentSession.CurrentWindow.StopBusyIndicator();
            }




           
           
        });
    }


    ViewFile(item: TermsofUseSignaturePMViewModel) {
  
        if (SessionLocator.PrivateLableSettings) {
            var documentName = SessionLocator.PrivateLableSettings.PrivateLabelShortName + "-" + item.TermsofUseVersion + "_termsofuses";// +"." + CurrentDocument.Extension;
            DownloadManager.DownloadPage(documentName);
            
        }
        else {
            var documentName = item.TermsofUseVersion + "_termsofuses";// +"." + CurrentDocument.Extension;
            DownloadManager.DownloadPage(documentName);
        } 

    }



}


class TermsofUseSignaturePMViewModel {


    SignedDatetime: Date;
    TermsofUseVersion: number;
    constructor(item: TermsofUseSignaturePM) {
        this.SignedDatetime = item.SignedDatetime;
        this.TermsofUseVersion = item.TermsofUseVersion;
    }

}
