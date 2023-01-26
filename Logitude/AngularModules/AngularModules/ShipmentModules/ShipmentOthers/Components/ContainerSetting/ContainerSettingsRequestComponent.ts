import { Component, Input } from "@angular/core";
import { ContainerSettingsComponent } from "./ContainerSettingsComponent";

@Component({
    templateUrl: './ContainerSettingsRequestComponent.html',
    selector: 'ContainerSettingsRequest',
    styleUrls: ['./ContainerSettingsComponent.scss']
})

export class ContainerSettingsRequestComponent {
    @Input() DataContext!: ContainerSettingsComponent;
    public ObjectTableName = this.DataContext?.ObjectTableName;

    OnSearchTextChangeEvent(searchText: string) {
        if (!searchText) searchText = "";

    }
}

