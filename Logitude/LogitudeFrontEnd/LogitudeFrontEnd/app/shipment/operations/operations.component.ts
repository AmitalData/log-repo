import {Component} from 'angular2/core';
import {Router} from 'angular2/router';
import {ShipmentsListComponent} from '../shipments-list/shipments-list.component';
import {IconButton} from '../../ApplicationControls/IconButton'

@Component({
    templateUrl: 'Views/OperationsPageView.html',
    directives: [IconButton],
})


export class OperationsComponent {

    public SearchText: string = "Search Partners / Ports / Ref.#";
    public SelectedTransportFilter: string;
    public SelectedDirectionFilter: string;

    constructor(private _router: Router) {
        this.onSelectTransport('All');
        this.onSelectDirection('All');
    }

    onSelectTransport(myArgs: string) {
        this.SelectedTransportFilter = myArgs;
    }

    onSelectDirection(myArgs: string) {
        this.SelectedDirectionFilter = myArgs;
    }

    onClickQuery() {
        this._router.navigate(['ShipmentsList']);
    }
}