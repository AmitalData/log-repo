import { Component } from '@angular/core';
import { QuoteDashboardComponent } from './QuoteDashboardComponent';

@Component({
    selector: 'quote-conversion',
    moduleId: module.id,
    templateUrl: './QuotesConversionComponent.html',
})

export class QuotesConversionComponent {
    private Wizard: QuoteDashboardComponent;
    InitTab(wizard: QuoteDashboardComponent) {
        this.Wizard = wizard;
        console.log("Init Tab");
    }
    RefreshTab() {
        console.log("Refresh Tab");
    }
}
