import { Component, ViewChildren, QueryList, AfterViewInit} from '@angular/core';
import {LocationDirective} from '../../../Infrastructure/Utilities/LocationDirective';
import {EntityResourceService} from '../../../Infrastructure/Services/EntityResourceService';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {FeatureLocator} from '../../../Infrastructure/Utilities/FeatureLocator';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';

@Component({
    selector: 'CRMComponent',
    
    templateUrl: './TicketsWorkspaceComponent.html',
    providers: [EntityResourceService],
})

export class TicketsWorkspaceComponent implements AfterViewInit {
    @ViewChildren(LocationDirective) public AllLocations: QueryList<LocationDirective>;
    public IsMenuVisible: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private _entityResourceService: EntityResourceService) {

        this.selectedItem = "TIW";
    }

    public IsTicketDashboardVisible: boolean = false;
    ngAfterViewInit() {
        if (FeatureLocator.HasFeaturePermession("Ticket", "TicketDashboard.Menu")) {
            this.IsTicketDashboardVisible = true;
            this.IsMenuVisible = true;
           
        }

        this.SelectionChanged();
    }

  
    private selectedItem: string;
    get SelectedItem() { return this.selectedItem; }
    set SelectedItem(newValue: string) {
        if (this.selectedItem != newValue) {
            this.selectedItem = newValue;
            this.SelectionChanged();
        }
    }

    private Page_TW: any = null;
    private Page_DW: any = null;
    SelectionChanged() {

        if (this.SelectedItem != null) {

            let myLocation: LocationDirective = this.AllLocations.toArray().filter(d => d.Code == this.SelectedItem)[0];
            if (myLocation != null) {

                switch (this.SelectedItem) {

                    case "TIW": {
                        if (this.Page_TW == null) {
                            this._entityResourceService.getEntityResourceByTableName("Ticket", 0).subscribe((response: any) => {
                                this._entityResourceService.getEntityResourceByTableName("Opportunity", 0).subscribe(response2 => {
                                    SessionLocator.DynamicLoader.Load('./CRM/Components/Workspaces/TicketsComponent', myLocation.viewContainerRef)
                                        .then(cmpRef => {
                                            this.Page_TW = cmpRef.instance;
                                        });
                                });
                            });
                        }

                        else {
                            this.Page_TW.LoadAllScreenData();
                        }

                        break;
                    }

                    case "DBW": {
                        if (this.Page_DW == null) {
                            this._entityResourceService.getEntityResourceByTableName("Ticket", 0).subscribe((response: any) => {
                                SessionLocator.DynamicLoader.Load('./CRM/Components/Workspaces/TicketDashboardComponent', myLocation.viewContainerRef)
                                    .then(cmpRef => {
                                        this.Page_DW = cmpRef.instance;
                                    });
                            });
                        }

                        break;
                    }
                }
                this.CurrentSession.ChangeSessionHeader({ Text: TextCodeTranslator.Translate("General.MH.Ticket")});
            }
        }

    }

    GetPageName() {
        var myResult = "";

        switch (this.SelectedItem) {
            case "TIW": { myResult = TextCodeTranslator.Translate("General.MH.Ticket"); break; }
            case "DBW": { myResult = TextCodeTranslator.Translate("General.MH.AirlineDashboard"); break; }

        }

        return myResult;
    }

    private mySelectedActivityFilter: string = "All";
    get SelectedActivityFilter() { return this.mySelectedActivityFilter; }
    set SelectedActivityFilter(value: string) {
        if (this.mySelectedActivityFilter != value) {
            this.mySelectedActivityFilter = value;
            this.Page_TW.SelectedActivityFilter = value;
        }
    }
}
