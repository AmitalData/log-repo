import {Component, OnInit}  from 'angular2/core';
import {RouteParams, Router} from 'angular2/router';
import {ShipmentsService} from '../../services/shipment-service/shipments.service';
import {TextcodeTranslationPipe} from '../../../infrastructure/pipes/textcode-translation/textcode-translation.pipe';

@Component({
    templateUrl: 'Views/Shipment/Tabs/EventsTab.html',
    pipes: [TextcodeTranslationPipe],
})

export class EventsComponent implements OnInit {

    private _entityId: string;
    public EntityPM: any;
    public IsDataLoaded: boolean = false;

    constructor(private _router: Router, routeParams: RouteParams, private _shipmentsService: ShipmentsService) {
        this._entityId = routeParams.get('id');
    }

    ngOnInit() {

        this.EntityPM = this._shipmentsService.getSingleShipmentByIdFromArray(this._entityId, 1);

        if (this.EntityPM != null && this.EntityPM !== undefined) {
            this.IsDataLoaded = true;
        }
    }
}
