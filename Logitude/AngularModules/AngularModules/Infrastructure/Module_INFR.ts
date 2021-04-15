import {NgModule}      from '@angular/core';
import {ControlsModule} from '../Controls/Module_CTRL';
import {Pipes, Directives, Components, ControlsComponents, ModuleDeclarations} from './ModuleDeclarations';
import { ModuleProviders } from './ModuleProviders';
//import { ScrollingModule } from '@angular/cdk-experimental/scrolling';
//import { AutoSizeVirtualScrollStrategy } from '@angular/cdk-experimental/scrolling';
import { MaterialModule } from '../Infrastructure/material-module';
//import { ScrollingModule } from '@angular/cdk/scrolling';
// Services
import {EntityArgs} from './DataContracts/EntityArgs';
import {ServiceArgs} from './DataContracts/ServiceArgs';
import {LoginService} from './Services/LoginService';
import {EntityListService} from './Services/EntityListService';
import {EntityPMService} from './Services/EntityPMService';
import {IndexedDbService} from './Services/IndexedDbService';
import {EntityResourceService} from './Services/EntityResourceService';
import {EntityLastActivityService} from './Services/EntityLastActivityService';
import {TotangoService} from './Services/WebServices/TotangoService';
import {LogitudeErrorHandler} from './Utilities/LogitudeErrorHandler'
import {ErrorHandler} from '@angular/core';
import {PrivateLabelsBrandingDataService} from './Services/WebServices/PrivateLabelsBrandingDataService';

@NgModule({
    imports: [ControlsModule, MaterialModule],
  declarations: [...Pipes, ...Directives, ...Components, ...ControlsComponents],
    exports: [...Pipes, ...Directives, ...ControlsComponents, ControlsModule],
    entryComponents: [...Components, ...ControlsComponents],

    providers:
    [
        EntityArgs,
        ServiceArgs,
        LoginService,
        EntityListService,
        EntityPMService,
        IndexedDbService,
        EntityResourceService,
         EntityLastActivityService,
         PrivateLabelsBrandingDataService,
        TotangoService,


        //Islam: if you comment it dont check in!
        { provide: ErrorHandler, useClass: LogitudeErrorHandler }
    ],
})

export class InfrastructureModule {
    public static GetComponent(name: string) {
        return ModuleDeclarations.Get(name);
    }

    public static GetInstance(name: string) {
        return ModuleProviders.GetInstance(name);
    }
}
