import { Component } from '@angular/core';import { QuoteDashboardComponent } from './QuoteDashboardComponent';
;

@Component({
    selector: 'quotes-by-country',
    moduleId: module.id,
    templateUrl: './QuotesByCountryComponent.html'
})

export class QuotesByCountryComponent {
    private Wizard: QuoteDashboardComponent;
    InitTab(wizard: QuoteDashboardComponent) {
        this.Wizard = wizard;
        console.log("Init Tab");
    }
    RefreshTab() {
        console.log("Refresh Tab");
    }

}
