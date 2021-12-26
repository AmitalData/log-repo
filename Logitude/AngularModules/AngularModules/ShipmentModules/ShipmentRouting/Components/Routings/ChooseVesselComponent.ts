import { Component } from '@angular/core';
import { AppTool } from '../../../../Infrastructure/Tools';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { VesselList } from '../../../../Common/EntityLists/VesselList';
import { VesselListService } from '../../../../Common/Services/StandardLists/VesselListService';
import { ApiQueryFilters } from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { EntityPMService } from '../../../../Infrastructure/Services/EntityPMService';
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';

@Component({
    templateUrl: './ChooseVesselComponent.html',
})

export class ChooseVesselComponent {
    public EntityPM: any;
    public IdProperty: string;
    public NameProperty: string;
    public ItemsSource: VesselList[] = [];
    public MyVesselsCount: number = 0;
    private myService: VesselListService;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        this.myService = new VesselListService();
        this.LoadAllData();
    }

    SetWindowArgs(args: any) {
        this.EntityPM = args['EntityPM'];
        this.IdProperty = args['IdProperty'];
        this.NameProperty = args['NameProperty'];
    }

    private searchText: string = null;
    get SearchText() { return this.searchText; }
    set SearchText(newValue: string) {
        if (this.searchText != newValue) {
            this.searchText = newValue;
            this.LoadAllData();
        }
    }

    private LoadAllData() {
        this.LoadTenantData();
    }

    private LoadTenantData() {
        var filters = new ApiQueryFilters();
        filters.PageIndex = 0;
        filters.PageSize = 100;
        filters.SortBy = "EnglishName";
        filters.SortDirection = "Descending";

        filters.addAdditionalFilter("InActive", false, null, null, "Equals", false, false, false, "boolean");

        if (!AppTool.IsNullOrEmpty(this.SearchText)) {
            filters.addAdditionalFilter("SearchFields", this.SearchText, null, null, "Contains", false, false, false, "string");
        }

        this.myService.getByFilters(filters).subscribe((myResponse: ServiceResponse) => {
            if (myResponse == null) {
                this.ItemsSource = [];
            }

            else {
                if (!myResponse.HasError) {
                    this.ItemsSource = myResponse.Result;
                }
            }

            this.MyVesselsCount = this.ItemsSource == null ? 0 : this.ItemsSource.length;
        });
    }

    Selecting(item: VesselList) {
        this.SetField(item);

        if (item == null) {
            this.SetField(null);
        }

        else {
            this.SetField(item);
        }

        this.Close();
    }

    SetField(item: VesselList) {
        var id: string = null;
        var name: string = null;

        if (item) {
            id = item.Id;
            name = item.EnglishName;
        }

        if (this.EntityPM[this.IdProperty] != id) {
            this.EntityPM[this.IdProperty] = id;
        }

        if (this.EntityPM[this.NameProperty] != name) {
            this.EntityPM[this.NameProperty] = name;
        }
    }

    CloseButtonClicked() {
        this.Close();
    }

    Close() {
        this.CurrentSession.CloseCurrentWindow();
    }

    AddVesselsClicked() {
        var myService: EntityPMService = new EntityPMService();
        var componentPath = "./Infrastructure/GenericComponents/NewEntityComponent";

        myService.getNewEntity("Vessel").then(response => {
            var args = new EntityArgs();
            args.EntityPM = response;
            args.ObjectTableName = "Vessel";

            var windowTitle = TextCodeTranslator.Translate("General.O.NewEntity").replace("%Entity", TextCodeTranslator.Translate("Vessel"));

            var logWindow = new LogitudeWindow();
            logWindow.Width = 960;
            logWindow.Height = 570;
            logWindow.WindowArgs = args;
            logWindow.Title = windowTitle;
            logWindow.WindowClosed.subscribe(($event: any) => this.OnNewEntityWindowClosed($event));
            logWindow.Show(componentPath);
        });
    }

    private OnNewEntityWindowClosed($event: any) {
        console.log($event);
        if ($event && $event != "event") {
            this.LoadTenantData();
        }
    }
}
