import { Component } from '@angular/core';
import { AppTool } from '../../../Infrastructure/Tools';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { CommonDomainService } from '../../../Common/Services/CommonDomainService';

@Component({
    templateUrl: './NewChargifyAWBStockComponent.html',
})

export class NewChargifyAWBStockComponent {

    private CurrentSession = SessionLocator.SelectedSession;
    private commonDomainService: CommonDomainService;

    constructor() {
        this.InitalizeServices();

    }

    InitalizeServices() {
        this.commonDomainService = new CommonDomainService();
    }

    SendButtonClicked() {
        this.commonDomainService.GetChargifyAWBStock().subscribe((myResult: any) => {
            this.CurrentSession.StartBusyIndicatorLoading();
            this.CurrentSession.StopBusyIndicator();
            this.CurrentSession.CloseCurrentWindow();
                
        });
    }
}
