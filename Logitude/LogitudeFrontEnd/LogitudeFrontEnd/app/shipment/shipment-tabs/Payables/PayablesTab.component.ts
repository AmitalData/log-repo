import {Component, OnInit}  from 'angular2/core';
import {RouteParams, Router} from 'angular2/router';
import {ShipmentsService} from '../../services/shipment-service/shipments.service';
import {TextcodeTranslationPipe} from '../../../infrastructure/pipes/textcode-translation/textcode-translation.pipe';
import {IconButton} from '../../../ApplicationControls/IconButton'

class ShipmentPayableItem {
    public EntityPM: any;
    ChargesTypeCode: string;
    ChargesTypeName: string;
    PrepaidCollectId: string;
    VendorName: string;
    MeasurementCode: string;
    CurrencyCode: string;
    UnitPrice: number;
    Quantity: number;
    ExpectedAmount: number;
    AccountedAmount: number;
    OpenAmount: number;

    constructor(entity: any) {

        this.EntityPM = entity;
        this.ChargesTypeCode = entity.ChargesTypeCode;
        this.ChargesTypeName = entity.ChargesTypeName;
        this.PrepaidCollectId = entity.PrepaidCollectId;
        this.VendorName = entity.VendorName;
        this.CurrencyCode = entity.CurrencyCode;
        this.MeasurementCode = entity.MeasurementCode;
        this.Quantity = entity.Quantity;
        this.UnitPrice = entity.UnitPrice;
        this.ExpectedAmount = entity.ExpectedAmount;
        this.AccountedAmount = entity.AccountedAmount;
        this.OpenAmount = entity.OpenAmount;
    }
}

@Component({
    templateUrl: 'Views/Shipment/Tabs/PayablesTab.html',
    pipes: [TextcodeTranslationPipe],
    directives: [IconButton],
})

export class PayablesComponent implements OnInit {

    private _entityId: string;
    public EntityPM: any;
    public IsDataLoaded: boolean = false;
    public ItemsSource: ShipmentPayableItem[];
    public IsGenerateButtonsVisible: boolean = false;

    constructor(private _router: Router, routeParams: RouteParams, private _shipmentsService: ShipmentsService) {
        this._entityId = routeParams.get('id');
    }

    BuildItemsSource() {
        if (this.ItemsSource == null) {
            this.ItemsSource = new Array<ShipmentPayableItem>();
        }

        else {
            this.ItemsSource = [];
        }

        this.EntityPM.ShipmentPayables.forEach((item) => {
            this.ItemsSource.push(new ShipmentPayableItem(item));
        })  

        //if (this.ItemsSource.length == 0) {
        //    this.IsGenerateButtonsVisible = true;
        //}

        //else {
        //    this.IsGenerateButtonsVisible = false;
        //}
    }

    ngOnInit() {

        this.EntityPM = this._shipmentsService.getSingleShipmentByIdFromArray(this._entityId, 1);

        if (this.EntityPM != null && this.EntityPM !== undefined) {
            this.IsDataLoaded = true;
            this.BuildItemsSource();
        }
    }
}
