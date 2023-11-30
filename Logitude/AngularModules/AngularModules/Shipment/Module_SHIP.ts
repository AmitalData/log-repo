import { NgModule } from '@angular/core';
import { InfrastructureModule } from '../Infrastructure/Module_INFR';
import { Components, ControlsComponents, ModuleDeclarations } from './ModuleDeclarations';
import { ModuleProviders } from './ModuleProviders';
import { FusionChartsModule } from "angular-fusioncharts";

// Import FusionCharts library and chart modules
import * as FusionCharts from "fusioncharts";
import * as charts from "fusioncharts/fusioncharts.charts";
import * as FusionTheme from "fusioncharts/themes/fusioncharts.theme.fusion";

// Pass the fusioncharts library and chart modules
FusionChartsModule.fcRoot(FusionCharts, charts, FusionTheme);
FusionCharts.options['license']({
    key: 'prD4C-8eiA7A3A3C7E6G4B4A3J4C7B2D3D2nyqE1C3fd1npaE4D4tlA-21D7E4F4F1F1E1F4F1A10A8C2C5F5E2F2D1hwqD1B5D1aG4A19A32twbC6D3G4lhJ-7A9C11A5B-13ddA2I3A1B9B3D7A2B4G2H3H1F-7smC8B2XB4cetB8A7A5mxD3SG4F2tvgB2A3B2E4C3I3C7B3A4A3A2D3D2G4J-7==',
    creditLabel: false,
});

@NgModule({
    imports: [InfrastructureModule, FusionChartsModule],
    declarations: [...Components, ControlsComponents],
    entryComponents: [...Components, ControlsComponents],
})

export class Shipment_Module {
    public static GetComponent(name: string) {
        return ModuleDeclarations.Get(name);
    }

    public static GetInstance(name: string) {
        return ModuleProviders.GetInstance(name);
    }
}
