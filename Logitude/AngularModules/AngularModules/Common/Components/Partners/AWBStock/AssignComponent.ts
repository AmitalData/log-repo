import {Component} from '@angular/core';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {AWBStackDomainService, StockSeriesListClass, StockSeries} from '../../../Services/AWBStackDomainService';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';

@Component({
    moduleId: module.id,
    templateUrl: './AssignComponent.html',
})

export class AssignComponent {
    private AirlineId: string = null;
    private CustomerId: string = null;
    private IsCustomerMode: boolean = false;
    public ItemsCount: number = 0;
    public ItemsSource: StockSeries[] = [];
    private StackDomainService: AWBStackDomainService;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        this.StackDomainService = new AWBStackDomainService();
    }

    SetWindowArgs(args:any) {
        this.AirlineId = args['AirlineId'];
        this.CustomerId = args['CustomerId'];
        this.IsCustomerMode = args['IsCustomerMode'];
        this.LoadData();
    }

    private LoadData() {
        this.ItemsCount = 0;
        this.ItemsSource = [];

        this.CurrentSession.StartBusyIndicatorLoading();

        if (this.IsCustomerMode) {
            this.StackDomainService.GetAllAvailableStockSeries().subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {

                    var myClass: StockSeriesListClass = myResponse.Result;

                    myClass.StockSeriesList.forEach(item => {
                        this.ItemsSource.push(item);
                        this.ItemsCount += item.Total;
                    });
                }

                this.CurrentSession.StopBusyIndicator();
            });
        }

        else {
            this.StackDomainService.GetAirlineAvailableStockSeries(this.AirlineId).subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {

                    var myClass: StockSeriesListClass = myResponse.Result;

                    myClass.StockSeriesList.forEach(item => {
                        this.ItemsSource.push(item);
                        this.ItemsCount += item.Total;
                    });
                }

                this.CurrentSession.StopBusyIndicator();
            });
        }
    }

    AssignClicked(item: StockSeries) {
        var logWindow = new LogitudeWindow();
        logWindow.Title = "Assign to shipper";
        logWindow.Width = 700;
        logWindow.Height = 450;
        logWindow.WindowArgs = { ShipperId: this.CustomerId, IsCustomerMode: this.IsCustomerMode, StockSeriesItem: item };
        logWindow.Show('./Common/Components/Partners/AWBStock/AssignToShipperComponent');
        logWindow.WindowClosed.subscribe(s => {
            if (s == "OK") {
                this.LoadData();
                this.isReloadingData = true;
            }
        });
    }

    private isReloadingData: boolean = false;
    CloseClicked() {
        if (this.isReloadingData) {
            this.CurrentSession.CloseCurrentWindowEmit("OK");
        }

        else {
            this.CurrentSession.CloseCurrentWindow();
        }
    }
}
