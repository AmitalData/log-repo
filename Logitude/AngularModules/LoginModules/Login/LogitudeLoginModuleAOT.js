import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { RootComponentAOT } from './RootComponentAOT';
import { LoginComponents } from './ModuleDeclarations';
import { LoginService } from './LoginService';
import { PasswordChangeService } from './PasswordChangeService';
import { HybridLabelsBrandingDataService } from './HybridLabels/Services/HybridLabelsBrandingDataService';
import { BrandingDataService } from './HybridLabels/Services/BrandingDataService';
export var LogitudeLoginModuleAOT = (function () {
    function LogitudeLoginModuleAOT() {
    }
    LogitudeLoginModuleAOT.decorators = [
        { type: NgModule, args: [{
                    imports: [BrowserModule, FormsModule, ReactiveFormsModule, HttpModule],
                    declarations: [
                        RootComponentAOT
                    ].concat(LoginComponents),
                    //exports: [
                    //    ...Pipes,
                    //    ...Directives,
                    //    ...InfrastructureControlsComponents,
                    //    FormsModule,
                    //    ReactiveFormsModule,
                    //    LogitudeControlsModule,
                    //],
                    entryComponents: [
                        RootComponentAOT
                    ].concat(LoginComponents),
                    providers: [
                        LoginService,
                        PasswordChangeService,
                        HybridLabelsBrandingDataService,
                        BrandingDataService
                    ],
                    bootstrap: [RootComponentAOT]
                },] },
    ];
    /** @nocollapse */
    LogitudeLoginModuleAOT.ctorParameters = [];
    return LogitudeLoginModuleAOT;
}());
//# sourceMappingURL=LogitudeLoginModuleAOT.js.map