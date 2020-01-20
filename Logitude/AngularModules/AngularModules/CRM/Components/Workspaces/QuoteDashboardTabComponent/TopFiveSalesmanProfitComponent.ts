import { Component } from '@angular/core';
import { QuoteDashboardComponent } from './QuoteDashboardComponent';

@Component({
    selector: 'top-five-salesman-profit',
    moduleId: module.id,
    templateUrl: './TopFiveSalesmanProfitComponent.html',
})

export class TopFiveSalesmanProfitComponent {
    private Wizard: QuoteDashboardComponent;
    InitTab(wizard: QuoteDashboardComponent) {
        this.Wizard = wizard;
        console.log("Init Tab");
    }
    RefreshTab() {
        console.log("Refresh Tab");
    }
}
