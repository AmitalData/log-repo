import { Component, EventEmitter, Output } from "@angular/core";
import { LogitudeWindow } from "Controls/Windows/LogitudeWindow";
import { BaseComponent } from "Infrastructure/Components/LogitudeComponents/BaseComponent";
import { ApiQueryFilters } from "Infrastructure/DataContracts/ApiQueryFilters";
import { SessionLocator } from "Infrastructure/Utilities/SessionLocator";
import { TextCodeTranslator } from "Infrastructure/Utilities/TextCodeTranslator";
import { filter } from "rxjs/operators";

@Component({
    selector: 'DeclarationFiltersMenuComponent',
    templateUrl: './DeclarationFiltersMenuComponent.html',
})


export class DeclarationFiltersMenuComponent
    extends BaseComponent {
    apiQueryFilters: ApiQueryFilters = new ApiQueryFilters();
    @Output() SelectedValueChanged = new EventEmitter();
    private CurrentSession = SessionLocator.SelectedSession;
    TransportFilter_A: string;
    TransportFilter_O: string;
    TransportFilter_I: string;
    isExport: boolean = false;
    titleExportFilterMenu: string = TextCodeTranslator.Translate("Customs.Declaration.O.exportFilterMenu");
    exportFilterData: any;

    constructor() {
        super();
        this.TransportFilter_A = "TransportFilter_A";
        this.TransportFilter_O = "TransportFilter_O";
        this.TransportFilter_I = "TransportFilter_I";
    }

    ngOnInit() {        
        this.initIsExport();
    }

    private initIsExport() {
        this.isExport = this.CurrentSession.CurrentListComponent.MenuTableQuerySection == "Customs.ExportDeclaration";
    }

    SetFiltersMenu(args: any) {
    }

    itemMouseLeave(itemValue: string) {
        if (this.SelectedValue != itemValue) {
            var img_A = document.getElementById(this.TransportFilter_A);
            var img_O = document.getElementById(this.TransportFilter_O);
            var img_I = document.getElementById(this.TransportFilter_I);

            switch (itemValue) {
                case "A": {
                    img_A.setAttribute("src", "./Images/TransportModes/A_g.png");
                    break;
                }

                case "O": {
                    img_O.setAttribute("src", "./Images/TransportModes/O_g.png");
                    break;
                }

                case "L": {
                    img_I.setAttribute("src", "./Images/TransportModes/I_g.png");
                    break;
                }
            }
        }
    }

    itemMouseOver(itemValue: string) {
        if (this.SelectedValue != itemValue) {
            var img_A = document.getElementById(this.TransportFilter_A);
            var img_O = document.getElementById(this.TransportFilter_O);
            var img_I = document.getElementById(this.TransportFilter_I);

            switch (itemValue) {
                case "A": {
                    img_A.setAttribute("src", "./Images/TransportModes/A.png");
                    break;
                }

                case "O": {
                    img_O.setAttribute("src", "./Images/TransportModes/O.png");
                    break;
                }

                case "L": {
                    img_I.setAttribute("src", "./Images/TransportModes/I.png");
                    //img_I.style.top = "1px";
                    break;
                }
            }
        }
    }

    transportmodeId: string = "All";
    itemClicked(itemValue: string) {
        this.transportmodeId = itemValue;
        var RemoveFilter = false;
        if (this.SelectedValue != itemValue) { 
            this.SelectedValue = itemValue;
        }
        if (this.apiQueryFilters.AdditionalFilters.length > 0) {
            this.apiQueryFilters.AdditionalFilters = this.apiQueryFilters.AdditionalFilters.filter(a => a.FieldName != "TransportModeForExport")
        }
        this.apiQueryFilters.addAdditionalFilter("TransportModeForExport", itemValue, null, null, "Equals", false, true, false, "string", (itemValue == "All" ? true : false));
        this.SelectedValueChanged.emit({ Filters: this.apiQueryFilters, RemoveFilter: RemoveFilter });
        this.ApplyTransportSelectedStyle();
    }

    ApplyTransportSelectedStyle() {
        var itemValue = this.SelectedValue;
        var img_A = document.getElementById(this.TransportFilter_A);
        var img_O = document.getElementById(this.TransportFilter_O);
        var img_I = document.getElementById(this.TransportFilter_I);
        if (img_A) {
            this.CurrentSession.ChangeSessionHeader({ TransportId: itemValue });
            img_A.setAttribute("src", "./Images/TransportModes/A_g.png");
            img_O.setAttribute("src", "./Images/TransportModes/O_g.png");
            img_I.setAttribute("src", "./Images/TransportModes/I_g.png");
            switch (itemValue) {
                case "A": {
                    img_A.setAttribute("src", "./Images/TransportModes/A_w.png");
                    break;
                }

                case "O": {
                    img_O.setAttribute("src", "./Images/TransportModes/O_w.png");
                    break;
                }

                case "L": {
                    img_I.setAttribute("src", "./Images/TransportModes/I_w.png");
                    break;
                }
            }
        }
    }

    private selectedValue: string = "All";
    public get SelectedValue() { return this.selectedValue; }
    public set SelectedValue(value: string) {
        if (this.selectedValue != value) {
            this.selectedValue = value;
        }
    }

    openFiltersWindow() {
        const logitudeWindow = new LogitudeWindow();
        logitudeWindow.Width = 450;
        logitudeWindow.Height = 170;
        logitudeWindow.Title = this.titleExportFilterMenu;
        logitudeWindow.WindowArgs = JSON.parse(JSON.stringify(this.exportFilterData || ''));
        logitudeWindow.Show('./CustomsModules/CustomsDeclarationModules/DeclarationOthers/Components/FiltersMenu/ExportFilterMenuComponent');        
        logitudeWindow.WindowClosed.pipe(filter(x=> x)).subscribe((exportFilterData) => {
            this.exportFilterData = exportFilterData;
            console.log(this.exportFilterData)
            this.SelectedValueChanged.emit({ Filters: exportFilterData.apiQueryFilters, RemoveFilter: false });
        });
    }
}

