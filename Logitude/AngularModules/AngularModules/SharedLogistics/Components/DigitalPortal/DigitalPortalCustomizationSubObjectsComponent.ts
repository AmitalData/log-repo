import { Component } from '@angular/core';
import { ObservableCollection } from '../../../Infrastructure/Utilities/ObservableCollection';
import { DigitalTextService } from '../../../Infrastructure/Services/WebServices/DigitalTextService'
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';

@Component({
    templateUrl: './DigitalPortalCustomizationSubObjectsComponent.html',
})

export class DigitalPortalCustomizationSubObjectsComponent {
    public ProfileCode: string;
    public ProfileId: string;
    public ObjectTableId: string;
    public ItemsSource: ObservableCollection;
    private digitalTextService: DigitalTextService;

    constructor() {
        this.ItemsSource = new ObservableCollection([]);
        this.digitalTextService = new DigitalTextService();
    }

    SetWindowArgs(args: any) {

    }

    public BuildItemsSource() {
        var objectTablesFilterList = [];
        this.digitalTextService.GetDigitalSubObjectsProfilesObjetTables(this.ObjectTableId).subscribe((myResult) => {
            if (!myResult.HasError) {
                var objectTables = myResult.Result;
                objectTables.forEach(item => {
                    objectTablesFilterList.push(new SubObjectsItem(item.ObjectTableId, item.ObjectTableName));
                });

                this.ItemsSource.InsertCollection(objectTablesFilterList);
            }
        });
    }

    EditButtonClicked(subObjectsItem) {
        var windowArgs: any = {};
        windowArgs.ObjectTableId = subObjectsItem.objectTableId;
        windowArgs.ProfileId = this.ProfileId;
        windowArgs.ProfileCode = this.ProfileCode;
        windowArgs.ParentObjectTableId = this.ObjectTableId;
        var logWindow = new LogitudeWindow();
        logWindow.IsFullScreen = true;
        logWindow.ShowCloseButton = true;
        logWindow.WindowArgs = windowArgs;
        logWindow.Show('./SharedLogistics/Components/DigitalPortal/DigitalPortalCustomizationShowHideFieldsComponent');
    }

}

export class SubObjectsItem {

    constructor(objectTableNId, objectTableName) {
        this.objectTableName = objectTableName;
        this.objectTableId = objectTableNId;
    }

    private objectTableName: string = null;
    public get ObjectTableName() { return this.objectTableName; }

    private objectTableId: string = null;
    public get ObjectTableId() { return this.objectTableId; }
  
}
