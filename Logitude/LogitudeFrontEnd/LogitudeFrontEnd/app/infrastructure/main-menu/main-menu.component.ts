import {Component, OnInit, ElementRef, Inject, forwardRef} from 'angular2/core';
import {RouteConfig, Router, RouterOutlet, AsyncRoute/*, ROUTER_DIRECTIVES*/} from 'angular2/router';
import {CORE_DIRECTIVES} from 'angular2/common';

//import {EditControlComponent} from '../edit-control/edit-control.component';
//import {ShipmentsListComponent} from '../../shipment/shipments-list/shipments-list.component';
//import {OperationsComponent} from '../../shipment/operations/operations.component';

import {ObjectField} from '../../shipment/shipment-tabs/general/general.service';
import {TextcodeTranslationPipe} from '../pipes/textcode-translation/textcode-translation.pipe';
import {Modal} from '../../infrastructure/modal-libs/angular2-modal/providers/Modal';
import {SVGPath} from '../../ApplicationControls/SVGPath'

interface MainMenuItem {
    TextCode: string,
    HtmlView: string,
    IndexOfOrder: number,
    ObjectTableId: string,
    IconSource?: string,
    IconSelectedSource?: string,    
}

@Component({
    templateUrl: './app/infrastructure/main-menu/main-menu.component.html',
    providers: [Modal],
    directives: [RouterOutlet, CORE_DIRECTIVES, SVGPath],
    pipes: [TextcodeTranslationPipe]
})

@RouteConfig([
        //{ path: '/operations', name: 'Operations', component: OperationsComponent, useAsDefault: true },
        new AsyncRoute({
            path: '/operations',
            loader: () => System.import('./app/shipment/operations/operations.component').then(m => m["OperationsComponent"]),
            name: 'Operations',
            useAsDefault: true
        }),
        new AsyncRoute({
            path: '/shipments-list',
            loader: () => System.import('./app/shipment/shipments-list/shipments-list.component').then(m => m["ShipmentsListComponent"]),
            name: 'ShipmentsList',
        }),
        //{ path: '/shipments-list', name: 'ShipmentsList', component: ShipmentsListComponent },
])

export class MainMenuComponent implements OnInit {

    public ShowFollowUps: boolean = false;
    constructor(private _router: Router, elementRef: ElementRef) {
        //listPage.mySampleElement = elementRef;
    }

    public MainMenuItems: Array<MainMenuItem>;
    public selectedMenu: MainMenuItem;

    ngOnInit() {

        this.MainMenuItems = this.GetMainMenuItemsFromWindow();
        this.OpenView(this.MainMenuItems[0]);
    }

    GetMainMenuItemsFromWindow() {

        var menu: MainMenuItem[] = [];

        //for (var i = 0; i < window.MenusTables.length; i++) {
        //    if (window.MenusTables[i].MenuTypeCode === "Main") {
        //        menu.push(window.MenusTables[i]);
        //        menu.push({ TextCode: window.MenusTables[i].TextCode, IndexOfOrder: window.MenusTables[i].TextCode.IndexOfOrder });
        //    }
        //}

        window.MenusTables.forEach((item) => {

            var myIcon: string;

            switch (item.TextCode) {
                case "General.MH.Operations": {
                    myIcon = "Box";
                    break;
                }

                case "General.MH.Quotes": {
                    myIcon = "Pen";
                    break;
                }

                case "General.MH.CRM": {
                    myIcon = "Bars";
                    break;
                }

                case "General.MH.Accounting": {
                    myIcon = "Dollar";
                    break;
                }

                case "General.MH.Maintenance": {
                    myIcon = "Maintenance";
                    break;
                }

                default: {
                    myIcon = "Person";
                    break;
                }
            }

            var myIconSource: string = "images/Menu/" + myIcon + ".png";
            var myIconSelectedSource: string = "images/Menu/" + myIcon + ".Selected.png";

            if (item.MenuTypeCode === "Main") {
                menu.push(
                    {
                        TextCode: item.TextCode,
                        IndexOfOrder: item.IndexOfOrder,
                        ObjectTableId: item.ObjectTableId,
                        HtmlView: item.HtmlView,
                        IconSource: myIconSource,
                        IconSelectedSource: myIconSelectedSource,
                });
            }
        })

        menu = menu.sort((a, b) => { return a.IndexOfOrder - b.IndexOfOrder });
        return menu;
    }

    OpenView(mySelectedItem: any) {
        if (this.selectedMenu != mySelectedItem) {
            this.selectedMenu = mySelectedItem;

            if (mySelectedItem.HtmlView == "Operations" || mySelectedItem.HtmlView == "Quotes") {
                this.ShowFollowUps = true;
            }

            else {
                this.ShowFollowUps = false;
            }

            //console.log("HtmlView = " + mySelectedItem.HtmlView, "ObjectTableId = " + mySelectedItem.ObjectTableId);

            if (mySelectedItem.HtmlView != null && mySelectedItem.ObjectTableId != null) {
                this._router.navigate(['MainMenu', mySelectedItem.HtmlView]);
            }


        }
    }

}
