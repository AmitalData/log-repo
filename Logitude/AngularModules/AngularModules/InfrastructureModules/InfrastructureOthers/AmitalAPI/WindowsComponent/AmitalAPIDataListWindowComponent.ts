import { Component, OnInit } from "@angular/core";
import { AmitalApiSettings } from "Common/Services/AmitalAPISchemaWebService";
import { LogitudeWindow } from "Controls/Windows/LogitudeWindow";
import { AmitalApiClientapiExpanded } from "../APISettingsComponent";

export type AmitalAPIDataListWindowParams = {
    clientAPIs: AmitalApiClientapiExpanded[];
    amitalApiSettings: AmitalApiSettings;
    azureClientId: string;
    secretValue: string;
    azureManagedApplObjId: string;
};

@Component({
    template: `
            <h1>Partner {{data.clientAPIs[0].PartnerName}}</h1>
            <h1>Authentication Api: Get Access Token (Body params)</h1>
            <p><span class='label'>address:</span> <span class='value'>{{data.amitalApiSettings.authAddress}}</span></p>
            <p><span class='label'>client_id:</span> <span class='value'>{{data.azureClientId}}</span></p>
            <p><span class='label'>client_secret:</span> <span class='value'>{{data.secretValue}}</span></p>
            <p><span class='label'>scope:</span> <span class='value'>{{data.amitalApiSettings.AuthScope}}</span></p>

            <ng-container *ngFor="let clientapi of data.clientAPIs">
                <h1>API Name: {{clientapi.ApiType}} (Header Params)</h1>
                <p><span class='label'>address: </span><span class='value'>{{clientapi.Address}}</span></p>
                <p><span class='label'>partner-token: </span><span class='value'>{{clientapi.PartnerToken}}</span></p>
                <p><span class='label'>api-token: </span><span class='value'>{{clientapi.Id}}</span></p>
                <p><span class='label'>caller-objectid: </span><span class='value'>{{data.azureManagedApplObjId}}</span></p>
            </ng-container>            
    `,
    styles: [`
        :host {
            direction: ltr;
            height: 754px;
            display: block;
            overflow: auto;
        }
        
        h1 {
            font-size: 18px;
            font-weight: bold;
            margin: 38px 3px 9px;
        }

        p {
            margin: 5px;
        }

        span {
            font-size: 15px;
            font-weight: normal;
        }

        span.label {
            font-weight: bold;
            width: 116px;
            display: inline-block;
        }
    `]
})
export class AmitalAPIDataListWindowComponent implements OnInit {
    data: AmitalAPIDataListWindowParams = { clientapis: [], } as any;

    ngOnInit(): void { }

    public static openWindow(params: AmitalAPIDataListWindowParams): Promise<boolean> {
        const logWindow = new LogitudeWindow();
        logWindow.Width = 900;
        logWindow.Height = 800;
        logWindow.Title = "Client API Data List";
        logWindow.WindowArgs = params;
        logWindow.ShowCloseButton = true;
        logWindow.Show("./InfrastructureModules/InfrastructureOthers/AmitalAPI/WindowsComponent/AmitalAPIDataListWindowComponent");
        return new Promise<boolean>(resolve => logWindow.WindowClosed.subscribe(() => resolve(true)));
    }

    SetWindowArgs(params: AmitalAPIDataListWindowParams) {
        this.data = params;
    }
}