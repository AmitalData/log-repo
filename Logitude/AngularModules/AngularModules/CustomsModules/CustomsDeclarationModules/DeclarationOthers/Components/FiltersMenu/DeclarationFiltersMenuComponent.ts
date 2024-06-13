import { Component, EventEmitter, Output } from "@angular/core";
import { LogitudeWindow } from "Controls/Windows/LogitudeWindow";
import { BaseComponent } from "Infrastructure/Components/LogitudeComponents/BaseComponent";
import { ApiQueryFilters } from "Infrastructure/DataContracts/ApiQueryFilters";
import { SessionLocator } from "Infrastructure/Utilities/SessionLocator";
import { TextCodeTranslator } from "Infrastructure/Utilities/TextCodeTranslator";
import { filter } from "rxjs/operators";
import { DeclarationExtendedListService } from '../../../../../Customs/Services/ExtendedLists/DeclarationExtendedListService';
import { ServiceResponse } from "Infrastructure/DataContracts/ServiceResponse";
import { ConfirmWindow } from "Controls/Windows/ConfirmWindow";

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
    private _DeclarationExtendedListService: DeclarationExtendedListService = new DeclarationExtendedListService();
    currentQuery: string;
    selectedRows: boolean;

    // diamonds vars
    diamondsMenusCount = {};
    diamondsMenuTranslates = [];
    @Output() CustomGetTotalCount = new EventEmitter();
    diamondsDeclarationsQuery = "Customs.Declaration.DiamondsDeclarations";
    selectedDiamondsMenu: string = "DeclarationsWithDeficiencies"; // the default diamonds filter
    diamondsMenus = ["ReleasedDeclarations", "PaidDeclarations", "CorrectDraft", "IncorrectDeclarations", "DeclarationsWithDeficiencies"]; // list of diamonds filters

    constructor() {
        super();
        this.TransportFilter_A = "TransportFilter_A";
        this.TransportFilter_O = "TransportFilter_O";
        this.TransportFilter_I = "TransportFilter_I";

        // prepare diamonds menus translates
        this.diamondsMenus.forEach(menu => this.diamondsMenuTranslates[menu] = TextCodeTranslator.Translate("Customs.Declaration.O." + menu));

        // on query/menu changed or refresh clicked
        this.CurrentSession.CurrentListComponent.onQueryChangeEvent.subscribe(data => {
            this.DeclarationFilterChanged(data);
        });
    }

    ngOnInit() {        
        this.initIsExport();
    }

    DeclarationFilterChanged(data) {

        if (data.QueryCode == this.diamondsDeclarationsQuery) {
            // if the query has been changed and it is now diamonds menu, get the diamonds declarations according to the selected menu filter
            if (data.QueryCode != this.currentQuery) {
                setTimeout(() => this.ShowDiamondsDeclarationByMenu(), 200);                    
            }

            this._DeclarationExtendedListService.GetDiamondsDeclarationsCounts(this.diamondsMenus)
                .subscribe((myResponse: ServiceResponse) => {
                    if (myResponse != null) {
                        if (!myResponse.HasError) {
                            this.diamondsMenusCount = myResponse?.Result?.Counts;
                            this.CustomGetTotalCount.emit(myResponse?.Result?.TotalCount);
                        }
                    }
                });

            this.CurrentSession.CurrentListComponent.SelectedRows.subscribe(data => {
                this.selectedRows = data;
            });
        }
        else {
            this.CustomGetTotalCount.emit(null);
        }

        this.currentQuery = data.QueryCode;
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

    ShowDiamondsDeclarationByMenu(menu?: string, forceMenuChange=false) {

        if (menu) {

            if (!forceMenuChange && this.selectedRows) {
                var confirmWindow = new ConfirmWindow();
                var confirmMsg = TextCodeTranslator.Translate("Customs.Declaration.O.CancelSelectedRowsConfirm");
                confirmWindow.Title = TextCodeTranslator.Translate("General.O.Confirm");
                confirmWindow.Width = 400;
                confirmWindow.Height = 180;
                confirmWindow.YesButtonText = TextCodeTranslator.Translate("Customs.General.B.OK");
                confirmWindow.NoButtonText = TextCodeTranslator.Translate("General.B.Cancel");
                confirmWindow.Show(confirmMsg);
                confirmWindow.WindowClosed.subscribe((event: any) => {
                    if (confirmWindow.Yes) {
                        this.ShowDiamondsDeclarationByMenu(menu, true);
                    }
                });
                return;
            }
    
            this.SelectedDiamondsMenu = menu;
        }

        // add the selected menu to the filter
        if (this.apiQueryFilters.AdditionalFilters.length > 0) {
            this.apiQueryFilters.AdditionalFilters = this.apiQueryFilters.AdditionalFilters.filter(a => a.FieldName != "DiamondsDeclarationFilter");
        }
        this.apiQueryFilters.addAdditionalFilter("DiamondsDeclarationFilter", this.SelectedDiamondsMenu, null, null, "Equals", true, false, true, "string", false);

        // specify this filter is only for the current diamonds menu and not for others queries
        var index = this.apiQueryFilters.AdditionalFilters.findIndex(d=> d.FieldName == "DiamondsDeclarationFilter");
        if (index > -1) {
            this.apiQueryFilters.AdditionalFilters[index]["SpecificMenuFilter"] = true;
        }

        this.SelectedValueChanged.emit({ Filters: this.apiQueryFilters, RemoveFilter: false, RowCount: this.diamondsMenusCount[this.SelectedDiamondsMenu] });
    }

    // RemoveDiamondsDeclarationFilter() {
    //     setTimeout(() => {
    //         // set to ignore the diamonds filter
    //         var item = this.apiQueryFilters.AdditionalFilters.filter(d=> d.FieldName == "DiamondsDeclarationFilter")[0];
    //         if (item) {
    //             var index = this.apiQueryFilters.AdditionalFilters.indexOf(item);
    //             this.apiQueryFilters.AdditionalFilters[index].IgnoreFilter = true;
    //         }
    //         this.SelectedValueChanged.emit({ Filters: this.apiQueryFilters, RemoveFilter: false });
    //     }, 200);
    // }

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

    public get SelectedDiamondsMenu() { return this.selectedDiamondsMenu; }
    public set SelectedDiamondsMenu(value: string) {
        if (this.selectedDiamondsMenu != value) {
            this.selectedDiamondsMenu = value;
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

