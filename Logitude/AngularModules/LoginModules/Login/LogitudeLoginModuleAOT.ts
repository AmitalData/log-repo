import {NgModule}      from '@angular/core';
import {BrowserModule} from '@angular/platform-browser';
import {FormsModule, ReactiveFormsModule}   from '@angular/forms';
import {HttpModule} from '@angular/http';

import {RootComponentAOT}   from './RootComponentAOT';
import {LoginComponents} from './ModuleDeclarations';

import {ErrorHandler} from '@angular/core';
import {LoginService} from './LoginService';
import {PasswordChangeService} from './PasswordChangeService';
import { PrivateLabelsBrandingDataService } from './PrivateLabels/Services/PrivateLabelsBrandingDataService';
import { BrandingDataService } from './PrivateLabels/Services/BrandingDataService';
import { CommonModule } from '@angular/common';
import { SessionInfo } from './SessionInfo';
import { PrivateLabelsService } from './PrivateLabels/Services/PrivateLabelsService';

export function getBaseUrl() {
    const logitudeURL = SessionInfo.GetLogitudeURL();
    return logitudeURL;
}

@NgModule({

    imports: [BrowserModule, FormsModule, ReactiveFormsModule, HttpModule, CommonModule],

    declarations:
    [ 
        RootComponentAOT,
        ...LoginComponents,

    ],


    //exports: [
    //    ...Pipes,
    //    ...Directives,
    //    ...InfrastructureControlsComponents,
    //    FormsModule,
    //    ReactiveFormsModule,
    //    LogitudeControlsModule,
        
    //],

    entryComponents: [
        RootComponentAOT,
        ...LoginComponents,
        
    ],

    providers:
    [
        LoginService,
            PasswordChangeService,
            PrivateLabelsBrandingDataService,
            PrivateLabelsService,
            BrandingDataService ,
              { provide: 'BASE_URL', useFactory: getBaseUrl, deps: [] }
    ],

    bootstrap: [RootComponentAOT]
})
     

export class LogitudeLoginModuleAOT { }