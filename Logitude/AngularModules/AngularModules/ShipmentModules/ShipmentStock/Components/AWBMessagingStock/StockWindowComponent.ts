import {Component} from '@angular/core';
import {ShipmentDomainService} from '../../../../Shipment/Services/ShipmentDomainService';
import {AWBMessagingStockList} from '../../../../Shipment/EntityLists/AWBMessagingStockList';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {FontTool, DateTool} from '../../../../Infrastructure/Tools';

@Component({
    moduleId: module.id,

    templateUrl: './StockWindowComponent.html',
})

export class StockWindowComponent {
    public ItemsSource: AWBMessagingStockListItem[];
    constructor() {
        this.ItemsSource = [];
        this.LoadData();
    }

    private myDomainService: ShipmentDomainService;
    private DataSource: AWBMessagingStockList[];
    private LoadData() {

        this.DataSource = [];               
        SessionLocator.CurrentSession.StartBusyIndicatorLoading();
            
        if (this.myDomainService == null) {
            this.myDomainService = new ShipmentDomainService();
        }

        this.myDomainService.GetLoggedTenantAWBMessagingStockLists().subscribe((myResult:any) => {
            this.DataSource = myResult;
            this.BuildItemsSource();
            SessionLocator.CurrentSession.StopBusyIndicator();
        });
    }

    public SelectedFilter: string = "ALL";
    FilterChanged(filterCode) {
        this.SelectedFilter = filterCode;
        this.BuildItemsSource();
    }

    private BuildItemsSource() {
        this.ItemsSource = []; 

        if (this.DataSource != null) {

            if (this.SelectedFilter == "ALL") {
                this.DataSource.forEach(item => {
                    this.ItemsSource.push(new AWBMessagingStockListItem(item));
                });
            }

            else if (this.SelectedFilter == "ACT") {
                this.DataSource.filter(f => f.Status == "New" || f.Status == "Active").forEach(item => {
                    this.ItemsSource.push(new AWBMessagingStockListItem(item));
                });
            }

            else if (this.SelectedFilter == "INA") {
                this.DataSource.filter(f => f.Status != "New" && f.Status != "Active").forEach(item => {
                    this.ItemsSource.push(new AWBMessagingStockListItem(item));
                });
            }            
        }
    }

    ViewHistory(item: AWBMessagingStockListItem) {
        var logWindow = new LogitudeWindow();
        logWindow.Title = "Messaging Stock History";
        logWindow.WindowArgs = item.Id;
        logWindow.Show("./ShipmentModules/ShipmentStock/Components/AWBMessagingStock/StockHistoryComponent");
    }

    CloseButtonClicked() {
        SessionLocator.CurrentSession.CloseCurrentWindow();
    }
}

class AWBMessagingStockListItem {
    constructor(public entity: AWBMessagingStockList) {
        this.SetForegrounds();
    }

    get Id() { return this.entity.Id; }
    get StartDate() { return this.entity.StartDate; }
    get EndDate() { return this.entity.EndDate; }
    get Amount() { return this.entity.Amount; }
    get Remaining() { return this.entity.Remaining; }
    get Status() { return this.entity.Status; }

    public EndDateForeground: string;
    public RemainingForeground: string;
    public StatusForeground: string;
    private SetForegrounds() {
        var _black = FontTool.Black;
        var _green = FontTool.Green;
        var _red = FontTool.Red;

        var endDateForeground = _black;
        var remainingForeground = _black;
        var statusForeground = _black;

        if (this.Status == "New" || this.Status == "Active") {
            statusForeground = _green;
        }

        if (this.Status == "Cancelled" || this.Status == "Used" || this.Status == "Expired") {
            endDateForeground = _black;
            remainingForeground = _black;
        }

        else {
            if (this.Remaining < 10) {
                remainingForeground = _red;
            }

            var isRedColor = true;

            if (this.EndDate != null) {
                var todayDate = DateTool.GetCurrentDateAsUtc();

                if (this.EndDate.valueOf() > todayDate.valueOf()) {
                    var myTotalDays: number = DateTool.GetDaysBetweenDates(this.EndDate, todayDate);

                    if (myTotalDays > 14) {
                        isRedColor = false;
                    }
                }
            }

            if (isRedColor) {
                endDateForeground = _red;
            }
        }

        this.EndDateForeground = endDateForeground;
        this.RemainingForeground = remainingForeground;
        this.StatusForeground = statusForeground;
    }
}

