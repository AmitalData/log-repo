import { Component } from '@angular/core';
import { ObservableCollection } from '../../../Infrastructure/Utilities/ObservableCollection';
import { DigitalCustomizationService } from '../../../Infrastructure/Services/WebServices/DigitalCustomizationService';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { DigitalPreDefinedComponentList } from "../../../infrastructure/entitylists/DigitalPreDefinedComponentList"

@Component({
    templateUrl: './AddDigitalPredefinedComponent.html',
})

export class AddDigitalPredefinedComponent {

    public PreDefinedItemsSource: ObservableCollection;
    ObjectTableId: string;
    SelectedPredefined: DigitalPreDefinedComponentList;
    digitalCustomizationService: DigitalCustomizationService;
    private CurrentSession = SessionLocator.SelectedSession;

    constructor() {

    }

    SetWindowArgs(args: any) {
        this.PreDefinedItemsSource = new ObservableCollection([]);
        this.digitalCustomizationService = new DigitalCustomizationService();
        this.ObjectTableId = args.ObjectTableId;
        this.BuildItemsSource();
    }

    BuildItemsSource() {
        this.PreDefinedItemsSource = new ObservableCollection([]);
        this.digitalCustomizationService.GetDigitalPreDefinedComponents(this.ObjectTableId, "").subscribe((myResult) => {
            if (!myResult.HasError) {
                var data = myResult.Result;
                this.PreDefinedItemsSource.InsertCollection(data);
            }
        });
    }

    Selecting(item) {
        this.SelectedPredefined = item;
    }

    AddPredefinedComponentClicked() {
        var content = this.SelectedPredefined?.Content;
        this.CurrentSession.CloseCurrentWindowEmit(content);
    }

    CloseButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

}
