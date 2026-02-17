declare var window: any;
import {Component, AfterViewInit, ChangeDetectorRef}  from '@angular/core';
import {EntityArgs} from '../../../../../Infrastructure/DataContracts/EntityArgs';
import {AppTool, ArrayTool} from '../../../../../Infrastructure/Tools';
import {FeatureLocator} from '../../../../../Infrastructure/Utilities/FeatureLocator';
import {SessionLocator} from '../../../../../Infrastructure/Utilities/SessionLocator';
import {TextCodeTranslator} from '../../../../../Infrastructure/Utilities/TextCodeTranslator';
import {BaseComponent} from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ObservableCollection} from '../../../../../Infrastructure/Utilities/ObservableCollection';
import {ConfirmWindow} from '../../../../../Controls/Windows/ConfirmWindow';
import {MessageWindow} from '../../../../../Controls/Windows/MessageWindow';
import {ServiceResponse} from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import {LogitudeWindow} from '../../../../../Controls/Windows/LogitudeWindow';
import { DeclarationDisplayOnlyChecks, DisplayOnlyCheckResult } from '../../../../../Customs/Utilities/DeclarationDisplayOnlyChecks';

import {DeclarationPM} from '../../../../../Customs/EntityPMs/DeclarationPM';
import {SupplierInvoicePM} from '../../../../../Customs/EntityPMs/SupplierInvoicePM';
import { DeclarationEventManager } from '../../../../../Customs/Utilities/DeclarationEventManager';
import {DeclarationWebService} from '../../../../../Customs/Services/WebServices/DeclarationWebService';
import { CustomsSettingListService } from '../../../../../Customs/Services/StandardLists/CustomsSettingListService';
import { CustomsVendorListService } from '../../../../../Customs/Services/StandardLists/CustomsVendorListService';

@Component({
    moduleId: module.id,
    templateUrl: './PartnersItemsSelectionComponent.html',
})

export class PartnersItemsSelectionComponent extends BaseComponent {
    public DataContext: any = this;
    public IsDisplayOnly: boolean = false;
    invoicePM: SupplierInvoicePM;
    customerCode: string;
    vendorNumber: string;
    searchText: string = "";
    ValidationErrorsList: string[] = [];

    originalData: any[];
    ItemsSource = new ObservableCollection([]);

    //Services
    private declarationWebService: DeclarationWebService = new DeclarationWebService;
    private customsSettingListService: CustomsSettingListService = new CustomsSettingListService;
    private customsVendorListService: CustomsVendorListService = new CustomsVendorListService();
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();

    }

    IsConnectedToUniFreight: boolean = false;
    SetWindowArgs(args: any) {
        if (!AppTool.IsNullOrEmpty(args)) {
            this.invoicePM = args.invoicePM;
            this.customerCode = args.customerCode;
            this.searchText = args.searchText;

            // Load screen data
            this.customsSettingListService.getAll().subscribe((response: ServiceResponse) => {
                var list = response.Result;
                console.log("[response/customsSettingListService.getAll]", list);
                if (!AppTool.IsNullOrEmpty(list)) {
                    var customsSettingList = list[0];
                    var isConnectedToUniFreight: boolean = false;
                    isConnectedToUniFreight = customsSettingList.IsConnectedToUniFreight;
                    this.IsConnectedToUniFreight = isConnectedToUniFreight;

                    if (isConnectedToUniFreight) {
                        this.LoadGTBITEMS();
                    }
                    else {
                        this.LoadCustomsPartnersItems();
                    }

                }
            });
            
        }
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        this.CurrentSession.CloseCurrentWindowEmit(this.SelectedRow);
    }

    Search(text: string) {
        this.searchText = text;

        if (this.IsConnectedToUniFreight) {
            var tkn = setTimeout(() => {

                if (!this.searchText || this.searchText.length != 1) {
                    this.LoadGTBITEMS();
                }
            }, 200);
        }
        else {

            var data = this.originalData;
            if (!AppTool.IsNullOrEmpty(text) && !AppTool.IsNullOrEmpty(data)) {

                var tkn = setTimeout(() => {

                    var filteredData = data.filter((el) => {
                        var searchField = el["SearchFields"] == undefined ? "" : el["SearchFields"];
                        if (searchField.toLowerCase().includes(text.trim().toLowerCase()))
                            return true;
                        else
                            return false;
                    });

                    this.ItemsSource.Clear();
                    this.ItemsSource.InsertCollection(filteredData);

                }, 200);

            //} else {
            //    var tkn = setTimeout(() => {
            //        this.ItemsSource.Clear();
            //        this.ItemsSource.InsertCollection(data);
            //    }, 202);

                //
           }

        }

    }

    LoadCustomsPartnersItems() {
        this.declarationWebService.GetCustomsPartnersItemsForSelection(this.invoicePM.VendorId, null)
            .subscribe((response: ServiceResponse) => {
                var res = response.Result;
                if (!AppTool.IsNullOrEmpty(res)) {
                    this.ItemsSource.Clear();
                    this.ItemsSource.InsertCollection(res);
                    this.originalData = res;

                    if (!AppTool.IsNullOrEmpty(this.searchText))
                        this.Search(this.searchText);
                }
            });
    }

    LoadGTBITEMS() {
        //if (AppTool.IsNullOrEmpty(this.invoicePM.VendorId)) {
        //    this.vendorNumber = "NULL";
        //}

        if (!AppTool.IsNullOrEmpty(this.invoicePM.VendorId) && AppTool.IsNullOrEmpty(this.vendorNumber)) {
            this.customsVendorListService.getSingle(this.invoicePM.VendorId)
                .subscribe((customsVendorList: ServiceResponse) => {
                    if (customsVendorList) {
                        this.vendorNumber = customsVendorList.Result.VendorNumber;
                        this.GetGITITEMPartnersItemList();
                    }
                });
        }
        else {
            this.GetGITITEMPartnersItemList();
        }
    }

    public GetGITITEMPartnersItemList() {
        this.declarationWebService.GetGITITEMPartnersItemList(this.vendorNumber, this.customerCode, this.searchText, 30, true)
            .subscribe((response: ServiceResponse) => {
                var res = response.Result;
                if (!AppTool.IsNullOrEmpty(res)) {
                    this.ItemsSource.Clear();
                    this.ItemsSource.InsertCollection(res);
                }
            });
    }


    public SelectedRow: any = null;
    OnRowSelected(item: any) {
        this.SelectedRow = item;
    }
    OnRowDoubleClick(item: any) {
        this.SelectedRow = item;
        this.CurrentSession.CloseCurrentWindowEmit(this.SelectedRow);

    }

}
