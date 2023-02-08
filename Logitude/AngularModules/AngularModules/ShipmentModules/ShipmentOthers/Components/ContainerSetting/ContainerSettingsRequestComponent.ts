import { Component, Input, OnInit } from "@angular/core";
import { SessionLocator } from "Infrastructure/Utilities/SessionLocator";
import { ContainerSettingsComponent, ShippingLineItem } from "./ContainerSettingsComponent";

@Component({
    templateUrl: './ContainerSettingsRequestComponent.html',
    selector: 'ContainerSettingsRequest',
    styleUrls: ['./ContainerSettingsComponent.scss']
})

export class ContainerSettingsRequestComponent implements OnInit {
    @Input() DataContext!: ContainerSettingsComponent;
    @Input() ShippingLines: ShippingLineItem[] = [];
    public ObjectTableName = this.DataContext?.ObjectTableName;
    public IsVisibleForTenantZero: boolean = false;
    public ItemSource: ShippingLineItem[] = [];
    public Tenant: number = 0;

    constructor() {
        this.Tenant = SessionLocator.Tenant;
    }

    ngOnInit(): void {
        this.ItemSource = Object.assign([], this.ShippingLines);
    }

    OnSearchTextChangeEvent(searchText: string) {
        if (!searchText) searchText = "";
        if (!searchText) {
            this.ItemSource = Object.assign([], this.ShippingLines);
            return;
        }
        this.ItemSource = this.ShippingLines.filter(d => (d.Code && d.Code.toLowerCase().indexOf(searchText.toLowerCase()) > -1)
            || (d.SCACCode && d.SCACCode.toLowerCase().indexOf(searchText.toLowerCase()) > -1)
            || (d.Name && d.Name.toLowerCase().indexOf(searchText.toLowerCase()) > -1));
    }
}

