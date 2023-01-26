import { Component, Input } from "@angular/core";
import { SessionLocator } from "Infrastructure/Utilities/SessionLocator";
import { ContainerSettingsComponent, ShippingLineItem } from "./ContainerSettingsComponent";

@Component({
    templateUrl: './ContainerSettingsRequestComponent.html',
    selector: 'ContainerSettingsRequest',
    styleUrls: ['./ContainerSettingsComponent.scss']
})

export class ContainerSettingsRequestComponent {
    @Input() DataContext!: ContainerSettingsComponent;
    @Input() ShippingLines: ShippingLineItem[] = [];
    public ObjectTableName = this.DataContext?.ObjectTableName;
    public IsVisibleForTenantZero: boolean = false;

    constructor() {
        if (SessionLocator.Tenant == 0) this.IsVisibleForTenantZero = true;
    }
    
    OnSearchTextChangeEvent(searchText: string) {
        if (!searchText) searchText = "";

    }
}

