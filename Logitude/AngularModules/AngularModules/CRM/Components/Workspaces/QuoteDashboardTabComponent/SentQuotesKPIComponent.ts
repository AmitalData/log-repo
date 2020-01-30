import { Component } from '@angular/core';
import { QuoteDashboardComponent } from './QuoteDashboardComponent';

@Component({
    selector: 'sent-quotes-kpi',
    moduleId: module.id,
    templateUrl: './SentQuotesKPIComponent.html',
})

export class SentQuotesKPIComponent {
    private Wizard: QuoteDashboardComponent;
    InitTab(wizard: QuoteDashboardComponent) {
        this.Wizard = wizard;
        console.log("Init Tab");
    }
    RefreshTab() {
        console.log("Refresh Tab");
    }
}
