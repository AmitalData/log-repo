import { Component } from "@angular/core";
import { BaseComponent } from "Infrastructure/Components/LogitudeComponents/BaseComponent";

@Component({
    templateUrl: './ContainerSettingsRequestComponent.html',
    selector: 'ContainerSettingsRequest',
    styleUrls: ['./ContainerSettingsComponent.scss']
})

export class ContainerSettingsRequestComponent extends BaseComponent {
    public DataContext: ContainerSettingsRequestComponent = this;


    OnSearchTextChangeEvent(searchText: string) {
        if (!searchText) searchText = "";

    }
}

