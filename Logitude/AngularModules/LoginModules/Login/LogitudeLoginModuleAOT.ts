import {NgModule}      from '@angular/core';
import {BrowserModule} from '@angular/platform-browser';
import {FormsModule, ReactiveFormsModule}   from '@angular/forms';
import {HttpModule} from '@angular/http';

import {RootComponentAOT}   from './RootComponentAOT';
import {LoginComponents} from './ModuleDeclarations';

import {ErrorHandler} from '@angular/core';
import {LoginService} from './LoginService';
import {PasswordChangeService} from './PasswordChangeService';
import { HybridLabelsBrandingDataService } from './HybridLabels/Services/HybridLabelsBrandingDataService';
import { BrandingDataService } from './HybridLabels/Services/BrandingDataService';
@NgModule({
    imports: [BrowserModule, FormsModule, ReactiveFormsModule, HttpModule],

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
            HybridLabelsBrandingDataService,
            BrandingDataService
    ],

    bootstrap: [RootComponentAOT]
})

export class LogitudeLoginModuleAOT { }