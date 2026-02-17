import { Component } from '@angular/core';
import { ExternalLinkPM } from 'Common/EntityPMs/ExternalLinkPM';
import { ExternalLinkPMService } from 'Common/Services/StandardPMs/ExternalLinkPMService';
import { LogitudeWindowTemplateComponent } from 'Controls/Windows/LogitudeWindow';
import { MessageWindow } from 'Controls/Windows/MessageWindow';
import { UIProperties } from 'Infrastructure/Components/LogitudeComponents/UIProperties';
import { EntityArgs } from "Infrastructure/DataContracts/EntityArgs";
import { ServiceResponse } from 'Infrastructure/DataContracts/ServiceResponse';
import { EntityResourceService } from 'Infrastructure/Services/EntityResourceService';
import { SessionLocator } from 'Infrastructure/Utilities/SessionLocator';
import { Observable } from 'rxjs';

@Component({
    selector: 'app-external-link',
    templateUrl: './ExternalLinkComponent.html',
    styles: [`
        .view-container {
            padding: 15px;
        }
        
        .data-row {
            width: 200px;
        }

        BooleanFilter {
            width: 71px;
            display: block;
            margin: 14px 0px;
        }

        .button-wrapper {
            direction: ltr;
            position: absolute;
            bottom: 16px;
            left: 23px;
        }

        .Button {
            display: inline-block;
            width: 50px;
            margin: 0 5px;
        }
    `]
})
export class ExternalLinkComponent {
    externalLinkPM: ExternalLinkPM = new ExternalLinkPM();
    ObjectTableName: string = "ExternalLink";
    isNew: boolean = false;
    dataReady: boolean = false;
    tableDataInit: boolean = false;
    errors: string[] = [];

    constructor(entityArgs: EntityArgs) {
        this.initObjectTable();

        if (entityArgs.EntityPM)
            this.initExistsData(entityArgs.EntityPM);

        if (!SessionLocator.SelectedSession.CurrentEditComponent) {
            (<LogitudeWindowTemplateComponent>SessionLocator.SelectedSession.CurrentWindow?.ComponentRef.instance).logWindow.Height = 350;
            (<LogitudeWindowTemplateComponent>SessionLocator.SelectedSession.CurrentWindow?.ComponentRef.instance).logWindow.Width = 450;
            (<LogitudeWindowTemplateComponent>SessionLocator.SelectedSession.CurrentWindow?.ComponentRef.instance).SetWindowSize();
        }
    }

    async initObjectTable() {
        await new Promise<void>(res => new EntityResourceService().getEntityResourceByTableName("ExternalLink").subscribe((myResult: ServiceResponse) => res())),
        this.tableDataInit = true;
    }

    SetNewWizardArgs(args: any): void {
        this.initNewData();
    }

    initNewData(): void {
        SessionLocator.SelectedSession.StartBusyIndicator('');
        const externalLinkPM: ExternalLinkPM = new ExternalLinkPM();
        externalLinkPM.Tenant = 0
        this.isNew = true;
        this.initData(externalLinkPM);
    }

    async initExistsData(externalLinkPM: ExternalLinkPM): Promise<void> {        
        SessionLocator.SelectedSession.StartBusyIndicator('');
        await this.initData(externalLinkPM);        
    }

    initData(externalLinkPM: ExternalLinkPM): void {
        this.externalLinkPM = externalLinkPM;
        this.externalLinkPM.UIProperties = new UIProperties();
        SessionLocator.SelectedSession.StopBusyIndicator();
        this.dataReady = true;
    }

    async close(save: boolean): Promise<void> {
        if (save) {
            const success: boolean = await this.sendToServer();
            if (!success) return;
        }

        SessionLocator.SelectedSession?.CloseCurrentWindow();
    }
    
    async sendToServer(): Promise<boolean> {
        let successSend: boolean = false;
        try {
            SessionLocator.SelectedSession.StartBusyIndicator('');

            const serviceRequest: Observable<ServiceResponse> = this.isNew ?
            new ExternalLinkPMService().insert(this.externalLinkPM) : new ExternalLinkPMService().update(this.externalLinkPM);
            
            const result = await new Promise<ServiceResponse>(res => serviceRequest.subscribe((myResult: ServiceResponse) => res(myResult)));
            if (result.HasError)
                this.errors = result.ErrorsArray;
            else
                successSend = true;

        } catch (error) {            
            MessageWindow.showErrorMessage(error?.message || error);
        } finally {
            SessionLocator.SelectedSession.StopBusyIndicator();
            return successSend;
        }
    }
}
