declare var window: any;
import {Component, AfterViewInit, ChangeDetectorRef, Output, EventEmitter}  from '@angular/core';
import {EntityArgs} from '../../../../../Infrastructure/DataContracts/EntityArgs';
import {AppTool, ArrayTool} from '../../../../../Infrastructure/Tools';
import {FeatureLocator} from '../../../../../Infrastructure/Utilities/FeatureLocator';
import {SessionLocator} from '../../../../../Infrastructure/Utilities/SessionLocator';
import {TextCodeTranslator} from '../../../../../Infrastructure/Utilities/TextCodeTranslator';
import {BaseComponent} from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ObservableCollection} from '../../../../../Infrastructure/Utilities/ObservableCollection';
import {ConfirmWindow} from '../../../../../Controls/Windows/ConfirmWindow';
import {ServiceResponse} from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import {LogitudeWindow} from '../../../../../Controls/Windows/LogitudeWindow';
import {Validator} from '../../../../../Infrastructure/Validators/Validator';
import {SendRequestVIA} from '../../../../../Customs/DataContract/RequestParams/RequestParamsBase';

import {CustomsVendorPM} from '../../../../../Customs/EntityPMs/CustomsVendorPM';
import {VendorCommunicationPM} from '../../../../../Customs/EntityPMs/VendorCommunicationPM';

import {CustomsHouseTypeExtendedPMService} from '../../../../../Customs/Services/ExtendedPMs/CustomsHouseTypeExtendedPMService';

// Send Request
import {INF_MSG_GenericResponseData} from '../../../../../Customs/DataContract/ResponseData/INF_MSG_GenericResponseData';
import {VendorCommunicationResult} from '../../../../../Customs/DataContract/ResponseData/VendorCommunicationResult';
import {VendorInsertUpdateDeleteMessageRequestParams, OperationTypes} from '../../../../../Customs/DataContract/RequestParams/VendorInsertUpdateDeleteMessageRequestParams';
import { CustomMessageProgressComponent, ShowProgressBarParams } from '../../../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent';
import {VendorMessagesService} from '../../../../../Customs/Services/WebServices/VendorMessagesService';
import {CustomsVendorPMService} from '../../../../../Customs/Services/StandardPMs/CustomsVendorPMService';
import { VendorCurrencyPM } from 'Customs/EntityPMs/VendorCurrencyPM';
import { EntityResourceService } from 'Infrastructure/Services/EntityResourceService';
import { ApiQueryFilters } from 'Infrastructure/DataContracts/ApiQueryFilters';
import { VendorCurrencyListService } from 'Customs/Services/StandardLists/VendorCurrencyListService';


@Component({
    
    templateUrl: './VendorGeneralTabComponent.html',
})

export class VendorCurrencyTabComponent extends BaseComponent {
    @Output() FillValidationErrorList: EventEmitter<any> = new EventEmitter();
    public EntityPM: CustomsVendorPM;
    public vendorCurrencyListPM: VendorCurrencyPM[];
    public ObjectTableName: string = "Customs.c";
    public DataContext: any = this;
    public IsNewEntity: boolean = false;
    public ValdationErrorList: any[];
    public isEntityChange: boolean = false;
    IsDelete: boolean = false;
    vendorCurrencyListService:VendorCurrencyListService
    VendorCurrencyList: ObservableCollection;
    line = 0;
    //vendorMessagesService: VendorMessagesService = new VendorMessagesService();
    //customsVendorPMService: CustomsVendorPMService = new CustomsVendorPMService();

    RequestVIA: SendRequestVIA;

    private CurrentSession = SessionLocator.SelectedSession;
    constructor( private EntityResourceService: EntityResourceService) {
        super();
        this.VendorCurrencyList = new ObservableCollection([]);

    }

    SetTabArgs(args: any, valdationErrorList: any[] = []) {
        this.EntityPM = args.EntityPM;
        this.IsNewEntity = args.IsNewEntity;

        console.log("EntityPM", this.EntityPM);

        this.EntityResourceService.getEntityResourceByTableName("Customs.VendorCurrency").subscribe((response: any) => {
            this.getRows();
      
        });

    }
    getRows() {
       
            let filters = new ApiQueryFilters();
    
    
            filters.PageSize = 200;
            filters.PageIndex = 0;
            filters.GetAll = false;
            filters.GetCount = true;
            
            filters.addAdditionalFilter("VendorId", this.EntityPM.Id, null, null, "Equals", false, false, false, "string", false);
            filters.addAdditionalFilter("Tenant", this.EntityPM.Tenant, null, null, "Equals", false, false, false, "string",false);
            
    
            return this.vendorCurrencyListService.getByFilters(filters)
                .subscribe(r => {
                    this.VendorCurrencyList = new ObservableCollection([]);
                    for (let item of r.Result) {
                        this.VendorCurrencyList.Insert(item);
                    }
                    this.line = r.Result.length;
                    this.vendorCurrencyListPM = r.Result;
                });
           
        }
   


    //#region Properties

    // get Currency() { return this.VendorCurrencyPM.Currency; }
    // set Currency(value: string) {
    //     if (this.VendorCurrencyPM.Currency != value) {
    //         this.VendorCurrencyPM.Currency = value;

    //     }
    // }

    //#endregion

    
    AddButonClicked() {
       
            var newVendorCurrencyPM = new VendorCurrencyPM(this.EntityPM);
            newVendorCurrencyPM.Tenant = SessionLocator.Tenant;
            newVendorCurrencyPM.VendorId = this.EntityPM.Id;
            newVendorCurrencyPM.LineNumber = this.line++; // it will be override by EntityUpdateService.OnCreating() in server.

            if (!this.vendorCurrencyListPM.includes(newVendorCurrencyPM)) {
              
                this.VendorCurrencyList.Insert(newVendorCurrencyPM);
                this.AddVendorCurrency(newVendorCurrencyPM)
            }
       
    }
    RemoveRow(item: VendorCurrencyPM) {
        if (!AppTool.IsNullOrEmpty(item)) {

            var confirmWindow = new ConfirmWindow();
            confirmWindow.YesButtonText = TextCodeTranslator.Translate("Customs.General.B.OK");
            confirmWindow.Show(TextCodeTranslator.Translate("Customs.VendorCurrency.O.DeleteCurrency"));

            confirmWindow.WindowClosed.subscribe((event: any) => {
                if (confirmWindow.Yes) {
                    this.VendorCurrencyList.Remove(item);
                    this.RemoveVendorCurrency(item)
                }

            });

        }
    }

    public AddVendorCurrency(item: VendorCurrencyPM) {
        if (item != null) {
            var index = this. vendorCurrencyListPM.indexOf(item);
            if (index == -1) {
                item.EntityParentPM = this;
                this. vendorCurrencyListPM.push(item);
               
            }
        }
    }
    public RemoveVendorCurrency(item: VendorCurrencyPM) {
        if (item != null) {
            var index = this. vendorCurrencyListPM.indexOf(item);
            if (index > -1) {
                this. vendorCurrencyListPM.splice(index, 1);
                
            }
        }
    }


    //#endregion
}

