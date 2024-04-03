import { Component } from '@angular/core';
import { ObservableCollection } from '../../../Infrastructure/Utilities/ObservableCollection';
import { DigitalTextService } from '../../../Infrastructure/Services/WebServices/DigitalTextService'
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { DigitalPortalCustomizationMainComponent } from './DigitalPortalCustomizationMainComponent';

@Component({
    templateUrl: './DigitalPortalCustomizationSubObjectsComponent.html',
})

export class DigitalPortalCustomizationSubObjectsComponent {
    public ProfileCode: string;
    public ProfileId: string;
    public ObjectTableId: string;
    public ItemsSource: ObservableCollection;
    public customizationEditComponent: DigitalPortalCustomizationMainComponent;
    private digitalTextService: DigitalTextService;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        this.ItemsSource = new ObservableCollection([]);
        this.digitalTextService = new DigitalTextService();
    }

    public BuildItemsSource() {
        this.CurrentSession.StartBusyIndicatorLoading();
        var objectTablesFilterList = [];
        this.digitalTextService.GetDigitalSubObjectsProfilesObjetTables(this.ObjectTableId).subscribe((myResult) => {
            if (!myResult.HasError) {
                var objectTables = myResult.Result;
                objectTables.forEach(item => {
                    objectTablesFilterList.push(new SubObjectsItem(item.ObjectTableId, item.ObjectTableName));
                });

                this.ItemsSource.InsertCollection(objectTablesFilterList);
                this.CurrentSession.StopBusyIndicator();
            }
        });
    }

    EditButtonClicked(subObjectsItem) {
        var windowArgs: any = {};
        windowArgs.ObjectTableId = subObjectsItem.objectTableId;
        windowArgs.ProfileId = this.ProfileId;
        windowArgs.ProfileCode = this.ProfileCode;
        windowArgs.ParentObjectTableId = this.ObjectTableId;
        windowArgs.customizationEditComponent = this.customizationEditComponent ? this.customizationEditComponent : null;
        var logWindow = new LogitudeWindow();
        logWindow.Title = (subObjectsItem.objectTableName + "Manager").replace(/([a-z])([A-Z])/g, '$1 $2');
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
