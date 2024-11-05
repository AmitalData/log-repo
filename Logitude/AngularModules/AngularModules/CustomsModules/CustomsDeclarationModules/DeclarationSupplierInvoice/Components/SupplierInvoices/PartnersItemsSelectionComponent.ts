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
import { List } from 'Infrastructure/DataContracts/Dashboard/List';
import { CustomsPartnersItemList } from 'Customs/EntityLists/CustomsPartnersItemList';
import { AmitalGatewayUtil, UnifreightMessageM } from 'Infrastructure/Utilities/AmitalGatewayUtil';
import { CustomsCountryListService } from 'Customs/Services/StandardLists/CustomsCountryListService';

@Component({
    
    templateUrl: './PartnersItemsSelectionComponent.html',
})

export class PartnersItemsSelectionComponent extends BaseComponent {
    public DataContext: any = this;
    public IsDisplayOnly: boolean = false;
    invoicePM: SupplierInvoicePM;
    customerCode: string;
    vendorNumber: string;
    searchText: string = "";
    declarationPM: DeclarationPM;
    ValidationErrorsList: string[] = [];

    originalData: any[];
    ItemsSource = new ObservableCollection([]);

    //Services
    private declarationWebService: DeclarationWebService = new DeclarationWebService;
    private customsSettingListService: CustomsSettingListService = new CustomsSettingListService;
    private customsVendorListService: CustomsVendorListService = new CustomsVendorListService();
    customsCountryListService: CustomsCountryListService = new CustomsCountryListService();

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
            this.declarationPM = args.declarationPM;

            // Load screen data
            this.customsSettingListService.getSingleFromCache(SessionLocator.Tenant.toString()).subscribe((response: ServiceResponse) => {
                var customsSettingList = response.Result;
                console.log("[response/customsSettingListService.getSingleFromCache]", customsSettingList);
                if (!AppTool.IsNullOrEmpty(customsSettingList)) {                
                    var isConnectedToUniFreight: boolean = false;
                    isConnectedToUniFreight = customsSettingList.IsConnectedToUniFreight;
                    this.IsConnectedToUniFreight = isConnectedToUniFreight;

                    if (isConnectedToUniFreight || this.declarationPM.Direction != 'E') {
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

        if(this.IsConnectedToUniFreight)
        {
            this.declarationWebService.GetGITITEMPartnersItemList(this.vendorNumber, this.customerCode, this.searchText, 30, true)
                .subscribe((response: ServiceResponse) => {
                    var res = response.Result;
                    if (!AppTool.IsNullOrEmpty(res)) {
                        this.ItemsSource.Clear();
                        this.ItemsSource.InsertCollection(res);
                    }
                });
        }
        else {
            this.GetGITITEMPartnersItemListFromUnifreight(this.vendorNumber, this.customerCode, this.searchText, 30,'ALL', true) .then(async (response) => {
                var res = response.getAll()
                if (!AppTool.IsNullOrEmpty(res)) {
                    this.ItemsSource.Clear();
                    this.ItemsSource.InsertCollection(res);
                }
            });
        }
    }
    GetGITITEMPartnersItemListFromUnifreight(vendorId: string, customerCode: string, searchText: string, top: number, searchBy: string, searchNULLVendor: boolean): Promise<List<CustomsPartnersItemList>> {
        return new Promise((resolve, reject) => {
        SessionLocator.SelectedSession.StartBusyIndicatorLoading();
        let sub = AmitalGatewayUtil.Instance.UnifaceRequestArrived
            .subscribe(
                (message: UnifreightMessageM) => {
                    var IsMatchUnifreightCallbackCommand = (                      
                        message.LogitudeEntityNumber == this.declarationPM.Id &&
                        message.LogitudeViewModel == "PartnersItemsSelectionComponent.ts-GetGITITEMS");
                    if (IsMatchUnifreightCallbackCommand) {
                        sub.unsubscribe();
                        let XMLResponse = UnifreightMessageM.GetStringValue(message, "XMLResponse");
                        alert(XMLResponse);
                        const xmlData = (xml: string) => xml.replace(/&lt;/g, '<').replace(/&gt;/g, '>').replace(/&amp;/g, '&');
                        const result = this.parseXml(xmlData(XMLResponse));
                        SessionLocator.SelectedSession.StopBusyIndicator();
                        resolve(result); // מחזירים את התוצאה עם resolve
                       
                    }
                }
            );

        var unifreightMessageM =
            AmitalGatewayUtil.Instance.
                DeclarationMessaging.GetMessage(this.declarationPM.CustomFileNo, this.declarationPM.Id, "PartnersItemsSelectionComponent.ts-GetGITITEMS", AmitalGatewayUtil.Instance.DeclarationMessaging.UnifreightEntity());
        unifreightMessageM.Requset.push(["XMLRequest", this.convertToXML(vendorId, customerCode, searchText, top, searchBy,searchNULLVendor)]);
        alert(this.convertToXML(vendorId, customerCode, searchText, top, searchBy,searchNULLVendor));     

        AmitalGatewayUtil.Instance.SendRequestToUnifreightAsync(
            "AmitalGatewayUtil.GetGITITEMS",
            "CFIHMAIN.LogitudeTask",
            "GetGITITEMS",
            unifreightMessageM,
            "");
        });
    }


    parseXml(xmlString: string): List<CustomsPartnersItemList> {
       
        // Parse the XML string into a DOM Document
        const parser = new DOMParser();
        const xmlDoc = parser.parseFromString(xmlString, 'application/xml');

        // Extract values from the XML
        const RESPONSE = xmlDoc.getElementsByTagName('RESPONSE')[0];
        const items = RESPONSE.getElementsByTagName('ITEM');
        var customsPartnersItemLists:List<CustomsPartnersItemList> = new List<CustomsPartnersItemList>();
        var customsPartnersItemList: CustomsPartnersItemList;
        for (let i = 0; i < items.length; i++) {
            const item = items[i];
            customsPartnersItemList = new CustomsPartnersItemList();

            customsPartnersItemList.Id = item.getElementsByTagName('COUNTER')[0]?.textContent || '';
            customsPartnersItemList.Tenant = this.declarationPM.Tenant;
            customsPartnersItemList.ItemCode = item.getElementsByTagName('ITEMNO')[0]?.textContent || '';
            customsPartnersItemList.ClassificationCode = item.getElementsByTagName('PRATID')[0]?.textContent || '';
            customsPartnersItemList.Name = item.getElementsByTagName('NAMEENG')[0]?.textContent || '';
            customsPartnersItemList.SearchFields = item.getElementsByTagName('SEARCHENG')[0]?.textContent || '';
            customsPartnersItemList.CustomerId = item.getElementsByTagName('PARTNERID')[0]?.textContent || '';
            customsPartnersItemList.VendorId = item.getElementsByTagName('SAPAKID')[0]?.textContent || '';
            customsPartnersItemList.CustomerName = item.getElementsByTagName('PARTNERID')[0]?.textContent || '';
            customsPartnersItemList.VendorName = item.getElementsByTagName('VENDORNAME')[0]?.textContent || '';
            customsPartnersItemList.OriginCountryCode = item.getElementsByTagName('ORIGINCOUNTRY')[0]?.textContent || '';
            if(!AppTool.IsNullOrEmpty(customsPartnersItemList.OriginCountryCode)) 
            {
                this.customsCountryListService.getSingleFromCache(customsPartnersItemList.OriginCountryCode).subscribe(res=>{
    
                    if(!AppTool.IsNullOrEmpty(res?.Result?.LocalName)){
                       customsPartnersItemList.OriginCountryName = res?.Result?.LocalName;
                    }
                    else{
                        customsPartnersItemList.OriginCountryCode = null;
                        customsPartnersItemList.OriginCountryName = null;
                    }
                });
            }
            customsPartnersItemList.InvoiceQuantityType = item.getElementsByTagName('UNITID')[0]?.textContent || '';

           
            customsPartnersItemLists.add(customsPartnersItemList);
         
        }               
        return customsPartnersItemLists;
    }
    
    convertToXML(vendorId: string, customerCode: string, searchText: string, top: number, searchBy: string, searchNULLVendor: boolean) {      
        if(AppTool.IsNullOrUndefined(searchText)){
            searchText = "";
        }
         return `<GITITEMS>
         <PARTNERID>${customerCode}</PARTNERID>
         <SAPAKID>${vendorId}</SAPAKID>
         <SEARCH>${searchText}</SEARCH>
         <ITEMNO></ITEMNO>
         <SEARCHBY>${searchBy}</SEARCHBY>
         <TOP>${top}</TOP>
         <SEARCHNULLVENDOR>${false}</SEARCHNULLVENDOR>
         </GITITEMS>`;   
      
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
