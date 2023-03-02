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
import { EntityResourceService } from 'Infrastructure/Services/EntityResourceService';
import { ApiQueryFilters } from 'Infrastructure/DataContracts/ApiQueryFilters';
import { CustomsCountryPM } from 'Customs/EntityPMs/CustomsCountryPM';
import { AddressCurrencyPM } from 'Customs/EntityPMs/AddressCurrencyPM';
import { AddressCurrencyService } from 'Customs/Services/WebServices/AddressCurrencyService';
import { AddressCurrencyListService } from 'Customs/Services/StandardLists/AddressCurrencyListService';
import { EditTabComponent } from 'Infrastructure/Components/EditComponent/EditTabComponent';
import { UIProperties } from 'Infrastructure/Components/LogitudeComponents/UIProperties';


@Component({
    
    templateUrl: './AddressCurrencyTabComponent.html',
})

export class AddressCurrencyTabComponent extends EditTabComponent {
    @Output() FillValidationErrorList: EventEmitter<any> = new EventEmitter();
    public EntityPM: CustomsCountryPM;
    public AddressCurrencyListPM: AddressCurrencyPM[];
    public ObjectTableName: string = "Customs.AddressCurrency";
    public DataContext: any = this;
    public ValdationErrorList: any[];
    public isEntityChange: boolean = false;
    IsDelete: boolean = false;
    addressCurrencyListService:AddressCurrencyListService=new AddressCurrencyListService();
    addressCurrencyService:AddressCurrencyService=new AddressCurrencyService();
    AddressCurrencyList: ObservableCollection;
    line = 0;
    isLoad=false;
    RequestVIA: SendRequestVIA;
    public UIProperties: UIProperties;

    private CurrentSession = SessionLocator.SelectedSession;
  
    constructor(private cdr: ChangeDetectorRef,private entityResourceService: EntityResourceService) {
        
        super(cdr);
        this.UIProperties = new UIProperties;
        this.AddressCurrencyList = new ObservableCollection([]);
        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM

        console.log("EntityPM", this.EntityPM);
        this.entityResourceService.getEntityResourceByTableName("Customs.AddressCurrency").subscribe((response: any) => {
         this.entityResourceService.getEntityResourceByTableName("Customs.VendorCurrency").subscribe((response: any) => {     
            this.entityResourceService.getEntityResourceByTableName("Customs.CurrencyType").subscribe((response: any) => {
            this.getRows();
         });
        });
    });

    }
    getRows() {
       
            let filters = new ApiQueryFilters();
    
    
            filters.PageSize = 200;
            filters.PageIndex = 0;
            filters.GetAll = false;
            filters.GetCount = true;
            
            filters.addAdditionalFilter("AddressId", this.EntityPM.Code, null, null, "Equals", false, false, false, "string", false);
            filters.addAdditionalFilter("Tenant", SessionLocator.Tenant, null, null, "Equals", false, false, false, "string",false);
            
    
            return this.addressCurrencyListService.getByFilters(filters)
                .subscribe(r => {
                    this.AddressCurrencyList = new ObservableCollection([]);
                    this.AddressCurrencyListPM = [];
                    if(!r.HasError) {
                       for (let item of r.Result) {
                           this.AddressCurrencyList.Insert(new AddressCurrencyItemModel(item));
                       }
                       this.getRowNumbers();
                       this.line = r.Result.length;
                       this.AddressCurrencyListPM = this.AddressCurrencyList.Collection;
                       this.isLoad=true
                    }
                });
           
        }
   
        getRowNumbers() {
            this.line = 0;
            for (let item of this.AddressCurrencyList.Collection) {
                this.line += +1;
                item.LineNumber = this.line;
            }
        }

    //#region Properties

   

    //#endregion
    OkButtonClicked(){


       
        this.ValdationErrorList =[];
        var errors = [];
            this.AddressCurrencyListPM.forEach((item) => {
                if(this.AddressCurrencyListPM.filter(x=>x.Currency==item.Currency).length>1){
                    errors.push(TextCodeTranslator.Translate('Customs.VendorCurrency.O.DoubleCurrency'));
                    
                }
            });
      
        if (errors.length > 0) {
            this.ValdationErrorList = errors;
        } else {
         
           this.addressCurrencyService.UpadateListCurrencyByAddress(this.AddressCurrencyListPM).subscribe(res=>{
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
            var newAddressCurrencyPM = new AddressCurrencyPM();
            newAddressCurrencyPM.Tenant = SessionLocator.Tenant;
            newAddressCurrencyPM.AddressId = this.EntityPM.Code;
            newAddressCurrencyPM.LineNumber = ++this.line; // it will be override by EntityUpdateService.OnCreating() in server.

              
                this.AddressCurrencyList.Insert(new AddressCurrencyItemModel(newAddressCurrencyPM));

       
    }
    RemoveRow(item: AddressCurrencyItemModel) {
        if (!AppTool.IsNullOrEmpty(item)) {

            var confirmWindow = new ConfirmWindow();
            confirmWindow.YesButtonText = TextCodeTranslator.Translate("Customs.General.B.OK");
            confirmWindow.Show(TextCodeTranslator.Translate("Customs.VendorCurrency.O.DeleteCurrency"));

            confirmWindow.WindowClosed.subscribe((event: any) => {
                if (confirmWindow.Yes) {
                    this.AddressCurrencyList.Remove(item);
                }

            });

        }
    }



    
    //#endregion
}

export class AddressCurrencyItemModel extends BaseComponent {
    public AddressCurrencyPM: AddressCurrencyPM = null;
    public ObjectTableName = "Customs.AddressCurrency";
    public DataContext = this;

    constructor(private addressCurrencyPM: AddressCurrencyPM) {
        super();
        this.AddressCurrencyPM = addressCurrencyPM;
    }

    //#region Properties

    get LineNumber() { return this.AddressCurrencyPM.LineNumber; }
    set LineNumber(value: number) {
        if (this.AddressCurrencyPM.LineNumber != value) {
            this.AddressCurrencyPM.LineNumber = value;

        }
    }

    get VendorId() { return this.AddressCurrencyPM.AddressId; }
    set VendorId(value: string) {
        if (this.AddressCurrencyPM.AddressId != value) {
            this.AddressCurrencyPM.AddressId = value;

        }
    }
    get Tenant() { return this.AddressCurrencyPM.Tenant; }
    set Tenant(value: number) {
        if (this.AddressCurrencyPM.Tenant != value) {
            this.AddressCurrencyPM.Tenant = value;

        }
    }
    get Currency() { return this.AddressCurrencyPM.Currency; }
    set Currency(value: string) {
        if (this.AddressCurrencyPM.Currency != value) {
            this.AddressCurrencyPM.Currency = value;

        }
    }
    get CurrencyTypeName() { return this.AddressCurrencyPM.CurrencyTypeName; }
    set CurrencyTypeName(value: string) {
        if (this.AddressCurrencyPM.CurrencyTypeName != value) {
            this.AddressCurrencyPM.CurrencyTypeName = value;

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

