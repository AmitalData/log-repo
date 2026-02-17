import {Component} from '@angular/core';
import {AppTool} from '../../../../Infrastructure/Tools';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
//import {GetStackWindowArgs} from '../../../../Args';


import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
import {ConfirmWindow} from '../../../../Controls/Windows/ConfirmWindow';
import {FBLStockPM} from '../../../../Shipment/EntityPMs/FBLStockPM';
import {FBLStockExtenedPMService} from '../../../../Shipment/Services/ExtendedPMs/FBLStockExtenedPMService';
import {GetStackWindowArgs} from '../../../../Common/Args';


@Component({
    moduleId: module.id,
    templateUrl: './FBLStackSelectionComponent.html',
})

export class FBLStackSelectionComponent {
    public ShipperId: string = null;
    public AirlineId: string = null;
    public AirlineName: string = null;
    private FBLStockExtenedPMService: FBLStockExtenedPMService;
    public ItemsCount: number = 0;
    public ItemsSource: FBLStockPM[];
    public SelectedItem: FBLStockPM = null;
    public ObjectTableName: string = "FBLStock";
    public IsResourcesReady: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private entityResourceService: EntityResourceService) {
        this.ItemsSource = [];
        this.FBLStockExtenedPMService = new FBLStockExtenedPMService();
        this.CurrentSession.StartBusyIndicator("Loading FBL Numbers");
    }

    private args: GetStackWindowArgs;
    SetWindowArgs(windowArgs: GetStackWindowArgs) {
        this.args = windowArgs;
        this.entityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe((res: any) => {
            this.IsResourcesReady = true;
            this.Load();
        });
    }

    private Load() {
        this.ItemsCount = 0;
        this.ItemsSource = [];


        //this.FBLStockExtenedPMService.GetFBLStockPMsByTenant(SessionLocator.Tenant, 100, 1).subscribe((response: any) => {
        this.FBLStockExtenedPMService.GetAllFBLStockPMsByTenant(SessionLocator.Tenant).subscribe((response: any) => {
            if (response.Result) {
                var data: FBLStockPM[] = response.Result;
                this.ItemsSource = data.sort((a, b) => { return a.Number - b.Number });
            }
            this.CurrentSession.StopBusyIndicator();
        });

        this.FBLStockExtenedPMService.GetAllFBLStockPMsCountByTenant(SessionLocator.Tenant).subscribe((response: any) => {
            this.ItemsCount = response.Result;
        });



    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        if (this.SelectedItem != null) {
            this.args.SelectedFBLStock = this.SelectedItem;
            this.CurrentSession.CloseCurrentWindowEmit("OK");

        }
    }
}
