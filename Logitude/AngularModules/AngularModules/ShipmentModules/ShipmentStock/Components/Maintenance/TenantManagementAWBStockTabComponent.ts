import {Component, OnInit}  from '@angular/core';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {TenantManagementPM} from '../../../../Infrastructure/EntityPMs/TenantManagementPM';
import {FeatureLocator} from '../../../../Infrastructure/Utilities/FeatureLocator';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {ShipmentDomainService} from '../../../../Shipment/Services/ShipmentDomainService';
import {MessagingStockList} from '../../../../Shipment/EntityLists/MessagingStockList';
import {MessagingStockPM} from '../../../../Shipment/EntityPMs/MessagingStockPM';
import {MessagingStockPMService} from '../../../../Shipment/Services/StandardPMs/MessagingStockPMService';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
import {DateTool} from '../../../../Infrastructure/Tools';

@Component({
    moduleId: module.id,
    templateUrl: './TenantManagementAWBStockTabComponent.html',
})

export class TenantManagementAWBStockTabComponent implements OnInit {
    public ObjectTableName: string = "TenantManagement";
    public EntityPM: TenantManagementPM;
    constructor(public entityArgs: EntityArgs, private entityResourceService: EntityResourceService) {
        this.EntityPM = this.entityArgs.EntityPM;
    }

    public IsEditingAllowed: boolean = false;
    ngOnInit() {
        if (this.EntityPM != null) {
            this.entityResourceService.getEntityResourceByTableName("MessagingStock").subscribe((res1: any) => {
                this.IsEditingAllowed = this.IsTenantManagementEditable();

                this.BuilItemsSource();
            });
        }
    }

    private IsTenantManagementEditable() {
        var myResult: boolean = false;

        if (FeatureLocator.HasFeaturePermession("TenantManagement", "EnableTenantManagementEdit")) {
            myResult = true;
        }

        return myResult;
    }

    public ItemsSource: MessagingStockList[];
    public BuilItemsSource() {
        this.ItemsSource = [];

        var service: ShipmentDomainService = new ShipmentDomainService();
        service.GetMessagingStockListForTenantManagmentTab(this.EntityPM.Id).subscribe(result => {
            var allStocks: MessagingStockList[] = result.Result;

            allStocks.sort((a, b) => { return (DateTool.GetDateFromDate(a.StartDate) === DateTool.GetDateFromDate(b.StartDate)) ? 0 : (DateTool.GetDateFromDate(a.StartDate) > DateTool.GetDateFromDate(b.StartDate)) ? -1 : 1 }).forEach(item => {
                this.ItemsSource.push(item);
            });
        });
    }

    AddStockClicked() {
        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Title = "New Messaging Stock";
        logitudeWindow.Width = 800;
        logitudeWindow.Height = 600;

        var args: StockArgs = new StockArgs();
        args.IsNewEntity = true;
        args.FatherComponent = this;

        logitudeWindow.WindowArgs = args;
        logitudeWindow.Show('./ShipmentModules/ShipmentStock/Components/Maintenance/AddEditAWBStockComponent');
    }

    public EditStockClicked(item: MessagingStockList) {
        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Title = "Edit Messaging Stock";
        logitudeWindow.Width = 800;
        logitudeWindow.Height = 600;

        var args: StockArgs = new StockArgs();
        args.IsNewEntity = false;
        args.FatherComponent = this;
        args.EntityId = item.Id

        logitudeWindow.WindowArgs = args;
        logitudeWindow.Show('./ShipmentModules/ShipmentStock/Components/Maintenance/AddEditAWBStockComponent');
    }
}

export class StockArgs {
    public IsNewEntity: boolean;
    public FatherComponent: TenantManagementAWBStockTabComponent;
    public EntityId: string;
}
