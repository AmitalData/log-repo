import {NgModule}      from '@angular/core';
import {BrowserModule} from '@angular/platform-browser';
import {FormsModule, ReactiveFormsModule}   from '@angular/forms';
import {HttpModule} from '@angular/http';
 
import {RootComponent}   from './RootComponent'; 
import {LoginComponents} from './ModuleDeclarations';

import {ErrorHandler} from '@angular/core';
import {LoginService} from './LoginService';
import {PasswordChangeService} from './PasswordChangeService';
import { HybridLabelsBrandingDataService } from './HybridLabels/Services/HybridLabelsBrandingDataService';
import { BrandingDataService } from './HybridLabels/Services/BrandingDataService';
import { CommonModule } from '@angular/common';

@NgModule({
    imports: [BrowserModule, FormsModule, ReactiveFormsModule, HttpModule, CommonModule],

    declarations:
    [
        ...LoginComponents,
    ],

    //exports:
    //[
    //    ...Pipes,
    //    ...Directives,
    //    ...InfrastructureControlsComponents,
    //    FormsModule,
    //    ReactiveFormsModule,
    //    LogitudeControlsModule,
    //],

    providers:
    [
        LoginService,
            PasswordChangeService,
            HybridLabelsBrandingDataService,
            BrandingDataService
    ],

    bootstrap: [RootComponent]
})

export class LogitudeLoginModule { }
