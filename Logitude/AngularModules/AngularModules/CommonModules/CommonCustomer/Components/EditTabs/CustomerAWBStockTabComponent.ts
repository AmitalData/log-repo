import {Component} from '@angular/core';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {CustomerPM} from '../../../../Common/EntityPMs/CustomerPM';
import {MAWBStackPM} from '../../../../Common/EntityPMs/MAWBStackPM';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {AWBStackDomainService, StockSeriesListClass, StockSeries} from '../../../../Common/Services/AWBStackDomainService';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {ConfirmWindow} from '../../../../Controls/Windows/ConfirmWindow';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
@Component({
    moduleId: module.id,
    templateUrl: './CustomerAWBStockTabComponent.html',
})

export class CustomerAWBStockTabComponent {
    public EntityPM: CustomerPM;
    public ObjectTableName: string = "Customer";
    public ItemsCount: number = 0;
    public ItemsSource: StockSeries[] = [];
    private StackDomainService: AWBStackDomainService;
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    public IsVisible: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private entityArgs: EntityArgs) {
        this._entityResourceService.getEntityResourceByTableName("MAWBStack").subscribe((response: any) => {
            this.IsVisible = true;
            this.EntityPM = entityArgs.EntityPM;
            this.StackDomainService = new AWBStackDomainService();
            this.LoadData();
        });
    }

    private LoadData() {
        this.ItemsCount = 0;
        this.ItemsSource = [];

        this.CurrentSession.StartBusyIndicatorLoading();

        this.StackDomainService.GetCustomerStockSeries(this.EntityPM.Id).subscribe((myResponse: ServiceResponse) => {
            this.CurrentSession.StopBusyIndicator();

            if (myResponse != null) {
                if (!myResponse.HasError) {
                    var myClass: StockSeriesListClass = myResponse.Result;

                    myClass.StockSeriesList.forEach(item => {
                        this.ItemsSource.push(item);
                        this.ItemsCount += item.Total;
                    });
                }
            }
        });
    }

    AddStockClicked() {
        var logWindow = new LogitudeWindow();
        logWindow.Title = TextCodeTranslator.Translate("MAWBStack.O.NewAirWayBillNumbers");
        logWindow.WindowArgs = { CustomerId: this.EntityPM.Id, IsCustomerMode: true };
        logWindow.Show('./Common/Components/Partners/AWBStock/NewStackComponent');
        logWindow.WindowClosed.subscribe(s => {
            if (s == "OK") {
                this.LoadData();
            }
        });
    }

    AssignClicked() {
        var logWindow = new LogitudeWindow();
        logWindow.Title = "Assign to shipper";
        logWindow.WindowArgs = { CustomerId: this.EntityPM.Id, IsCustomerMode: true };
        logWindow.Show('./Common/Components/Partners/AWBStock/AssignComponent');
        logWindow.WindowClosed.subscribe(s => {
            if (s == "OK") {
                this.LoadData();
            }
        });
    }

    UnassignClicked(item: StockSeries) {
        if (item != null) {
            var confirmWindow = new ConfirmWindow();
            confirmWindow.Width = 450;
            confirmWindow.Height = 190;
            confirmWindow.Title = "Unassign Series";
            confirmWindow.YesButtonText = "Unassign";
            confirmWindow.NoButtonText = "Cancel";
            confirmWindow.Show("Are you sure you want to Unassign this series?");
            confirmWindow.WindowClosed.subscribe((event: any) => {
                if (confirmWindow.Yes) {
                    this.CurrentSession.StartBusyIndicatorSaving();

                    this.StackDomainService.UnAssignStockSeriesToUser(item.From, item.To, item.AirlineId).subscribe((myResponse: ServiceResponse) => {

                        this.CurrentSession.StopBusyIndicator();

                        if (myResponse != null) {
                            if (!myResponse.HasError) {
                                this.LoadData();
                            }
                        }
                    });
                }
            });
        }
    }
}
