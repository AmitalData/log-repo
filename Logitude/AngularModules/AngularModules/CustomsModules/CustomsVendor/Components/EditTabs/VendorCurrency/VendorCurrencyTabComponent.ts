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
import { VendorCurrencyService } from 'Customs/Services/WebServices/VendorCurrencyService';


@Component({
    
    templateUrl: './VendorCurrencyTabComponent.html',
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
    vendorCurrencyListService:VendorCurrencyListService=new VendorCurrencyListService();
    vendorCurrencyService:VendorCurrencyService=new VendorCurrencyService();
    VendorCurrencyList: ObservableCollection;
    line = 0;
   
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
            this.EntityResourceService.getEntityResourceByTableName("Customs.CurrencyType").subscribe((response: any) => {
            this.getRows();
         });
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
                    this.vendorCurrencyListPM = [];
                    if(!r.HasError) {
                       for (let item of r.Result) {
                           this.VendorCurrencyList.Insert(new VendorCurrencyItemModel(item));
                       }
                       this.getRowNumbers();
                       this.line = r.Result.length;
                       this.vendorCurrencyListPM = this.VendorCurrencyList.Collection;
                    }
                });
           
        }
   
        getRowNumbers() {
            this.line = 0;
            for (let item of this.VendorCurrencyList.Collection) {
                this.line += +1;
                item.LineNumber = this.line;
            }
        }

    //#region Properties

   

    //#endregion
    OkButtonClicked(){


       

        var errors = [];
        this.FillValidationErrorList.emit(errors); // clear validation msgs
            this.vendorCurrencyListPM.forEach((item) => {
                Validator.TryValidateObject(item, "Customs.VendorCurrency", errors);
                if(this.vendorCurrencyListPM.filter(x=>x.Currency==item.Currency).length>1){
                    errors.push(TextCodeTranslator.Translate('Customs.VendorCurrency.O.DoubleCurrency'));
                    
                }
            });
      
        if (errors.length > 0) {
            this.ValdationErrorList = errors;
            this.FillValidationErrorList.emit(errors);
        } else {
         
           this.vendorCurrencyService.UpadateListCurrencyByVendor(this.vendorCurrencyListPM).subscribe(res=>{
               if(!res.HasError){
                   var message=new ConfirmWindow();
                   message.YesButtonText = TextCodeTranslator.Translate('General.B.Ok');
                   message.ShowNoButton=false;
                   message.Show(TextCodeTranslator.Translate('Customs.VendorCurrency.O.UpdateCurrency'));
               }
           })
        }
    }
 
    
    AddButonClicked() {
            var newVendorCurrencyPM = new VendorCurrencyPM();
            newVendorCurrencyPM.Tenant = SessionLocator.Tenant;
            newVendorCurrencyPM.VendorId = this.EntityPM.Id;
            newVendorCurrencyPM.LineNumber = ++this.line; // it will be override by EntityUpdateService.OnCreating() in server.

              
                this.VendorCurrencyList.Insert(new VendorCurrencyItemModel(newVendorCurrencyPM));

       
    }
    RemoveRow(item: VendorCurrencyItemModel) {
        if (!AppTool.IsNullOrEmpty(item)) {

            var confirmWindow = new ConfirmWindow();
            confirmWindow.YesButtonText = TextCodeTranslator.Translate("Customs.General.B.OK");
            confirmWindow.Show(TextCodeTranslator.Translate("Customs.VendorCurrency.O.DeleteCurrency"));

            confirmWindow.WindowClosed.subscribe((event: any) => {
                if (confirmWindow.Yes) {
                    this.VendorCurrencyList.Remove(item);
                }

            });

        }
    }



    
    //#endregion
}

export class VendorCurrencyItemModel extends BaseComponent {
    public VendorCurrencyPM: VendorCurrencyPM = null;
    public ObjectTableName = "Customs.VendorCurrency";
    public DataContext = this;

    constructor(private vendorCurrencyPM: VendorCurrencyPM) {
        super();
        this.VendorCurrencyPM = vendorCurrencyPM;
    }

    //#region Properties

    get LineNumber() { return this.VendorCurrencyPM.LineNumber; }
    set LineNumber(value: number) {
        if (this.VendorCurrencyPM.LineNumber != value) {
            this.VendorCurrencyPM.LineNumber = value;

        }
    }

    get VendorId() { return this.VendorCurrencyPM.VendorId; }
    set VendorId(value: string) {
        if (this.VendorCurrencyPM.VendorId != value) {
            this.VendorCurrencyPM.VendorId = value;

        }
    }
    get Tenant() { return this.VendorCurrencyPM.Tenant; }
    set Tenant(value: number) {
        if (this.VendorCurrencyPM.Tenant != value) {
            this.VendorCurrencyPM.Tenant = value;

        }
    }
    get Currency() { return this.VendorCurrencyPM.Currency; }
    set Currency(value: string) {
        if (this.VendorCurrencyPM.Currency != value) {
            this.VendorCurrencyPM.Currency = value;

        }
    }
    get CurrencyTypeName() { return this.VendorCurrencyPM.CurrencyTypeName; }
    set CurrencyTypeName(value: string) {
        if (this.VendorCurrencyPM.CurrencyTypeName != value) {
            this.VendorCurrencyPM.CurrencyTypeName = value;

        }
    }

    
    //#endregion

    SetLocalName(entity, fieldName) {
       
        if (!AppTool.IsNullOrEmpty(entity)) {
            this[fieldName] = entity.LocalName;
        } else {
            this[fieldName] = null;
        }

    }
}

