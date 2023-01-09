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
import { AmitalGatewayUtil, UnifreightMessageM } from 'Infrastructure/Utilities/AmitalGatewayUtil';

@Component({
    
    templateUrl: './PartnersItemsDescreptionSelectionComponent.html',
})

export class PartnersItemsDescreptionSelectionComponent extends BaseComponent {
    public DataContext: any = this;
    public IsDisplayOnly: boolean = false;
    declarationId: string;
    customFileNo: string;
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
            this.declarationId = args.declarationId;
            this.customFileNo = args.customFileNo;
            this.searchText = args.searchText;

            // Load screen data
            this.LoadCustomsPartnersItems();
           
            
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

            }
            else{
                if (!AppTool.IsNullOrEmpty(data)){
                    this.ItemsSource.Clear();
                    this.ItemsSource.InsertCollection(data);
                }
            }

    }
    LoadCustomsPartnersItems() {

        SessionLocator.SelectedSession.StartBusyIndicatorLoading();

        let sub = AmitalGatewayUtil.Instance.UnifaceRequestArrived
            .subscribe(
                (mess: UnifreightMessageM) => {
                    var IsMatchUnifreightCallbackCommand = (
                        mess.LogitudeEntity == AmitalGatewayUtil.Instance.DeclarationMessaging.LogitudeEntityDeclaration &&
                        mess.LogitudeEntityNumber == this.declarationId &&
                        mess.LogitudeViewModel == "SupplierInvoiceGeneralTabComponent.ts-CustomExportPratMehesList");
                    if (IsMatchUnifreightCallbackCommand) {
                        sub.unsubscribe();
                        SessionLocator.SelectedSession.StopBusyIndicator();
                        let PratMehesList = UnifreightMessageM.GetStringValue(mess, "PratMehesList"); 
                    
                        var res =   PratMehesList.split(';').map((value)=>{
                            const keyValue = value.split('~');
        
                                    
                                return {
                                   
                                    ClassificationCode: keyValue[0],
                                    ItemDescription: keyValue[1],
                                    SearchFields:keyValue[0]+","+keyValue[1]
                                          };
                                    })

                                    if (!AppTool.IsNullOrEmpty(res)) {
                                        this.ItemsSource.Clear();
                                        this.ItemsSource.InsertCollection(res);
                                        this.originalData = res;
                    
                                        if (!AppTool.IsNullOrEmpty(this.searchText))
                                            this.Search(this.searchText);
                                    }

                          
                     
                        
                    }
                }
            );
        
        
        var unifreightMessageM =
            AmitalGatewayUtil.Instance.
                DeclarationMessaging.GetMessage(this.customFileNo, this.declarationId, "SupplierInvoiceGeneralTabComponent.ts-CustomExportPratMehesList", "BFIFILE");
        
        AmitalGatewayUtil.Instance.SendRequestToUnifreightAsync(
            "AmitalGatewayUtil.CustomExportPratMehesList",
            "BFIHMAIN.LogitudeTask",
            "CustomExportPratMehesList",
            unifreightMessageM,
            "רשימת פרטי המכס");
                  
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
