import {NgModule}      from '@angular/core';
import {BrowserModule} from '@angular/platform-browser';
import {FormsModule, ReactiveFormsModule}   from '@angular/forms';
import {HttpModule} from '@angular/http';
 
import {RootComponent}   from './RootComponent'; 
import {LoginComponents} from './ModuleDeclarations';

import {ErrorHandler} from '@angular/core';
import {LoginService} from './LoginService';
import {PasswordChangeService} from './PasswordChangeService';

@NgModule({
    imports: [BrowserModule, FormsModule, ReactiveFormsModule, HttpModule],

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
        PasswordChangeService
    ],

    bootstrap: [RootComponent]
})

export class LogitudeLoginModule { }
