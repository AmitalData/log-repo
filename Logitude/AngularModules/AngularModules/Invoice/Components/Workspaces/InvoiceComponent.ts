import {Component, ViewChildren, QueryList} from '@angular/core';
import {LocationDirective} from '../../../Infrastructure/Utilities/LocationDirective';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {AccountReceivablesComponent} from '../Workspaces/AccountReceivablesComponent';
import {AccountPayablesComponent} from '../Workspaces/AccountPayablesComponent';
import {AccountingTransferComponent} from '../Workspaces/AccountingTransferComponent';
import {EntityResourceService} from '../../../Infrastructure/Services/EntityResourceService';
import {ObjectsLocator} from '../../../Infrastructure/Locators/ObjectsLocator';

@Component({
    selector: 'OperationsComponent',
    moduleId: module.id,
    templateUrl: './InvoiceComponent.html',
})

export class InvoiceComponent {
    @ViewChildren(LocationDirective) public AllLocations: QueryList<LocationDirective>;
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    public isRTL: boolean = false;

    constructor() {
        if (ObjectsLocator.GlobalSetting) {
            this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        }
        this.RunComponent();
    }

    private isLoaderReady: boolean = false;
    RunComponent() {
        if (this.AllLocations) {

            if (this.AllLocations.length == 0) {
                this.RunComponentTimer();
            }

            else {
                this.isLoaderReady = true;
                this.SelectedItem = "RECEIVABLE";
            }
        }

        else {
            this.RunComponentTimer();
        }
    }

    private Retries: number = 0;
    private timerToken: any;
    private RunComponentTimer() {
        this.Retries++;

        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }

        if (this.Retries < 20) {
            this.timerToken = setTimeout(() => this.RunComponent(), 1);
        }
    }

    private selectedItem: string;
    get SelectedItem() { return this.selectedItem; }
    set SelectedItem(newValue: string) {
        if (this.selectedItem != newValue) {
            this.selectedItem = newValue;
            this.SelectionChanged();
        }
    }

    private Page_AR: AccountReceivablesComponent = null;
    private Page_AP: AccountPayablesComponent = null;
    private Page_AT: AccountingTransferComponent = null;
    private Page_SET: AccountingTransferComponent = null;

    SelectionChanged() {
        if (this.isLoaderReady) {
            if (this.SelectedItem != null) {
                let myLocation: LocationDirective = this.AllLocations.toArray().filter(d => d.Code == this.SelectedItem)[0];
                if (myLocation != null) {

                    switch (this.SelectedItem) {

                        case "RECEIVABLE": {
                            if (this.Page_AR == null) {

                                this._entityResourceService.getEntityResourceByTableName("ARInvoice", 0).subscribe((response: any) => {
                                    this._entityResourceService.getEntityResourceByTableName("ARPayment", 0).subscribe((response: any) => {
                                        SessionLocator.DynamicLoader.Load("./Invoice/Components/Workspaces/AccountReceivablesComponent", myLocation.viewContainerRef)
                                            .then(cmpRef => {
                                                this.Page_AR = cmpRef.instance;
                                                this.Page_AR.InitComponent();
                                            });
                                    });
                                });
                            }

                            break;
                        }

                        case "PAYABLE": {
                            if (this.Page_AP == null) {

                                this._entityResourceService.getEntityResourceByTableName("APInvoice", 0).subscribe((response: any) => {
                                    this._entityResourceService.getEntityResourceByTableName("APPayment", 0).subscribe((response: any) => {
                                        SessionLocator.DynamicLoader.Load("./Invoice/Components/Workspaces/AccountPayablesComponent", myLocation.viewContainerRef)
                                            .then(cmpRef => {
                                                this.Page_AP = cmpRef.instance;
                                                this.Page_AP.InitComponent();
                                            });
                                    });
                                });
                            }

                            break;
                        }

                        case "TRANSFER": {
                            if (this.Page_AT == null) {

                                SessionLocator.DynamicLoader.Load("./Invoice/Components/Workspaces/AccountingTransferComponent", myLocation.viewContainerRef)
                                    .then(cmpRef => {
                                        this.Page_AT = cmpRef.instance;
                                        //this.Page_AT.InitComponent();
                                    });
                            }

                            break;
                        }
                        case "SETTINGS": {
                            if (this.Page_SET == null) {

                                SessionLocator.DynamicLoader.Load("./Invoice/Components/Workspaces/SettingsComponent", myLocation.viewContainerRef)
                                    .then(cmpRef => {
                                        this.Page_SET = cmpRef.instance;
                                      
                                    });
                            }

                            break;
                        }
                    }
                }
            }
        }
    }

}