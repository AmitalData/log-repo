import {Component} from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {UIProperty, UIProperties}  from '../../../../Infrastructure/Components/LogitudeComponents/UIProperties'
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import { MessagingStockPM } from '../../../../Shipment/EntityPMs/MessagingStockPM';
import { MessagingStockUsageHistoryPM } from '../../../../Shipment/EntityPMs/MessagingStockUsageHistoryPM';
import { MessagingStockPMService } from '../../../../Shipment/Services/StandardPMs/MessagingStockPMService';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';

@Component({
    moduleId:'./ShipmentModules/ShipmentStock/Components/MessagingStock/',
    templateUrl: './StockGeneralTabComponent.html',
})

export class StockGeneralTabComponent extends BaseComponent {
    public EntityPM: MessagingStockPM;
    public ObjectTableName: string = "MessagingStock";
    public DataContext = this;
    public ItemsSource: MessagingStockUsageHistoryPM[];
    constructor(public entityArgs: EntityArgs) {
        super();
        this.EntityPM = entityArgs.EntityPM;
        this.ItemsSource = [];
        this.SetUIProperties();
        this.BuildItemsSource();
        this.Listen();
    }

    private Listen() {
        if (SessionLocator.CurrentSession.CurrentEditComponent != null) {
            SessionLocator.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.SetUIProperties();
                }
            });
        }
    }

    private SetUIProperties() {
        this.UIProperties.SetEnabled("Remaining", this.ObjectTableName, false);

        var isFieldEnabled = true;

        if (this.EntityPM.IsCancelled) {
            isFieldEnabled = false;
        }

        else if (this.EntityPM.Status != "New") {
            isFieldEnabled = false;
        }

        this.UIProperties.SetEnabled("StartDate", this.ObjectTableName, isFieldEnabled);
        this.UIProperties.SetEnabled("EndDate", this.ObjectTableName, isFieldEnabled);
        this.UIProperties.SetEnabled("Amount", this.ObjectTableName, isFieldEnabled);
        this.UIProperties.SetEnabled("Notes", this.ObjectTableName, isFieldEnabled);
        this.UIProperties.SetEnabled("TotalPrice", this.ObjectTableName, isFieldEnabled);
    }

    get TenantNumber() { return this.EntityPM.TenantNumber; }
    set TenantNumber(newValue: number) {
        if (this.EntityPM.TenantNumber != newValue) {
            this.EntityPM.TenantNumber = newValue;
            this.SetUIProperties();
        }
    }

    get StartDate() { return this.EntityPM.StartDate; }
    set StartDate(newValue: Date) {
        if (this.EntityPM.StartDate != newValue) {
            this.EntityPM.StartDate = newValue;
        }
    }

    get EndDate() { return this.EntityPM.EndDate; }
    set EndDate(newValue: Date) {
        if (this.EntityPM.EndDate != newValue) {
            this.EntityPM.EndDate = newValue;
        }
    }

    get Amount() { return this.EntityPM.Amount; }
    set Amount(newValue: number) {
        if (this.EntityPM.Amount != newValue) {
            this.EntityPM.Amount = newValue;
        }
    }

    get Remaining() { return this.EntityPM.Remaining; }
    set Remaining(newValue: number) {
        if (this.EntityPM.Remaining != newValue) {
            this.EntityPM.Remaining = newValue;
        }
    }

    get TotalPrice() { return this.EntityPM.TotalPrice; }
    set TotalPrice(newValue: number) {
        if (this.EntityPM.TotalPrice != newValue) {
            this.EntityPM.TotalPrice = newValue;
        }
    }

    get Notes() { return this.EntityPM.Notes; }
    set Notes(newValue: string) {
        if (this.EntityPM.Notes != newValue) {
            this.EntityPM.Notes = newValue;
        }
    }

    private BuildItemsSource() {
        this.ItemsSource = this.EntityPM.StockUsageHistories.sort(function (a, b) { return a.LastActionDate.valueOf() == b.LastActionDate.valueOf() ? 0 : a.LastActionDate.valueOf() < b.LastActionDate.valueOf() ? -1 : 1; });
    }

    private myService: MessagingStockPMService;
    RefreshButtonClicked() {
        SessionLocator.CurrentSession.StartBusyIndicatorLoading();

        if (this.myService == null) {
            this.myService = new MessagingStockPMService();
        }

        this.myService.get(this.EntityPM.Id).subscribe((myResponse: ServiceResponse) => {

            SessionLocator.CurrentSession.StopBusyIndicator();

            if (myResponse != null) {
                if (!myResponse.HasError) {
                    this.EntityPM = myResponse.Result;
                    this.BuildItemsSource();
                }                
            }
        });
    }
}
