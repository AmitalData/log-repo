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
import { CustomMessageProgressComponent, ShowProgressBarParams } from '../../../../CustomsControls/Components/CustomMessageProgressComponent';
import {VendorMessagesService} from '../../../../../Customs/Services/WebServices/VendorMessagesService';
import {CustomsVendorPMService} from '../../../../../Customs/Services/StandardPMs/CustomsVendorPMService';
import { EntityResourceService } from 'Infrastructure/Services/EntityResourceService';
import { ApiQueryFilters } from 'Infrastructure/DataContracts/ApiQueryFilters';
import { CustomsCountryPM } from 'Customs/EntityPMs/CustomsCountryPM';
import { CountryCurrencyPM } from 'Customs/EntityPMs/CountryCurrencyPM';
import { CountryCurrencyService } from 'Customs/Services/WebServices/CountryCurrencyService';
import { CountryCurrencyListService } from 'Customs/Services/StandardLists/CountryCurrencyListService';
import { EditTabComponent } from 'Infrastructure/Components/EditComponent/EditTabComponent';
import { UIProperties } from 'Infrastructure/Components/LogitudeComponents/UIProperties';


@Component({
    
    templateUrl: './CountryCurrencyTabComponent.html',
})

export class CountryCurrencyTabComponent extends EditTabComponent {
    @Output() FillValidationErrorList: EventEmitter<any> = new EventEmitter();
    public EntityPM: CustomsCountryPM;
    public CountryCurrencyListPM: CountryCurrencyPM[];
    public ObjectTableName: string = "Customs.CountryCurrency";
    public DataContext: any = this;
    public ValdationErrorList: any[];
    public isEntityChange: boolean = false;
    IsDelete: boolean = false;
    countryCurrencyListService:CountryCurrencyListService=new CountryCurrencyListService();
    countryCurrencyService:CountryCurrencyService=new CountryCurrencyService();
    CountryCurrencyList: ObservableCollection;
    line = 0;
    isLoad=false;
    isDirty:boolean = false;
    RequestVIA: SendRequestVIA;
    public UIProperties: UIProperties;

    private CurrentSession = SessionLocator.SelectedSession;
  
    constructor(private cdr: ChangeDetectorRef,private entityResourceService: EntityResourceService) {
        
        super(cdr);
        this.UIProperties = new UIProperties;
        this.CountryCurrencyList = new ObservableCollection([]);
        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM

        console.log("EntityPM", this.EntityPM);
        this.entityResourceService.getEntityResourceByTableName("Customs.CountryCurrency").subscribe((response: any) => {
         this.entityResourceService.getEntityResourceByTableName("Customs.VendorCurrency").subscribe((response: any) => {     
            this.entityResourceService.getEntityResourceByTableName("Customs.CurrencyType").subscribe((response: any) => {
            this.getRows();
         });
        });
    });

    }
    get IsDirty() { return this.isDirty|| this.CountryCurrencyListPM?.find(x=>x.IsDirty)?true:false; }

    getRows() {
       
            let filters = new ApiQueryFilters();
    
    
            filters.PageSize = 200;
            filters.PageIndex = 0;
            filters.GetAll = false;
            filters.GetCount = true;
            
            filters.addAdditionalFilter("CountryId", this.EntityPM.Code, null, null, "Equals", false, false, false, "string", false);
            filters.addAdditionalFilter("Tenant", SessionLocator.Tenant, null, null, "Equals", false, false, false, "string",false);
            
    
            return this.countryCurrencyListService.getByFilters(filters)
                .subscribe(r => {
                    this.CountryCurrencyList = new ObservableCollection([]);
                    this.CountryCurrencyListPM = [];
                    if(!r.HasError) {
                       for (let item of r.Result) {
                           this.CountryCurrencyList.Insert(new CountryCurrencyItemModel(item));
                       }
                       this.getRowNumbers();
                       this.line = r.Result.length;
                       this.CountryCurrencyListPM = this.CountryCurrencyList.Collection;
                       this.isLoad=true
                    }
                });
           
        }
   
        getRowNumbers() {
            this.line = 0;
            for (let item of this.CountryCurrencyList.Collection) {
                this.line += +1;
                item.LineNumber = this.line;
            }
        }

    //#region Properties

   

    //#endregion
    OkButtonClicked(){


       
        this.ValdationErrorList =[];
        var errors = [];
            this.CountryCurrencyListPM.forEach((item) => {
                if(this.CountryCurrencyListPM.filter(x=>x.Currency==item.Currency).length>1){
                    errors.push(TextCodeTranslator.Translate('Customs.VendorCurrency.O.DoubleCurrency'));
                    
                }
                if(AppTool.IsNullOrEmpty(item.Currency)){
                    errors.push(TextCodeTranslator.Translate('Customs.VendorCurrency.O.CurrencyRequired'));
                    
                }
            });
      
        if (errors.length > 0) {
            this.ValdationErrorList = errors;
        } else {
         
           this.countryCurrencyService.UpadateListCurrencyByCountry(this.CountryCurrencyListPM).subscribe(res=>{
               if(!res.HasError){
                   var message=new ConfirmWindow();
                   message.YesButtonText = TextCodeTranslator.Translate('General.B.Ok');
                   message.ShowNoButton=false;
                   message.Show(TextCodeTranslator.Translate('Customs.VendorCurrency.O.UpdateCurrency'));
                   this.CountryCurrencyListPM.forEach(x=>x.IsDirty=false); 
                   this.isDirty=false;
               }
           })
        }
    }
 
    
    AddButonClicked() {
            var newCountryCurrencyPM = new CountryCurrencyPM();
            newCountryCurrencyPM.Tenant = SessionLocator.Tenant;
            newCountryCurrencyPM.CountryId = this.EntityPM.Code;
            newCountryCurrencyPM.LineNumber = ++this.line; // it will be override by EntityUpdateService.OnCreating() in server.

              
                this.CountryCurrencyList.Insert(new CountryCurrencyItemModel(newCountryCurrencyPM));
                this.isDirty=true;


    }
    RemoveRow(item: CountryCurrencyItemModel) {
        if (!AppTool.IsNullOrEmpty(item)) {

            var confirmWindow = new ConfirmWindow();
            confirmWindow.YesButtonText = TextCodeTranslator.Translate("Customs.General.B.OK");
            confirmWindow.Show(TextCodeTranslator.Translate("Customs.VendorCurrency.O.DeleteCurrency"));

            confirmWindow.WindowClosed.subscribe((event: any) => {
                if (confirmWindow.Yes) {
                    this.CountryCurrencyList.Remove(item);
                    this.isDirty=true;

                }

            });

        }
    }



    
    //#endregion
}

export class CountryCurrencyItemModel extends BaseComponent {
    public CountryCurrencyPM: CountryCurrencyPM = null;
    public ObjectTableName = "Customs.CountryCurrency";
    public DataContext = this;
    public IsDirty = false;

    constructor(private countryCurrencyPM: CountryCurrencyPM) {
        super();
        this.CountryCurrencyPM = countryCurrencyPM;
    }

    //#region Properties

    get LineNumber() { return this.CountryCurrencyPM.LineNumber; }
    set LineNumber(value: number) {
        if (this.CountryCurrencyPM.LineNumber != value) {
            this.CountryCurrencyPM.LineNumber = value;

        }
    }

    get VendorId() { return this.CountryCurrencyPM.CountryId; }
    set VendorId(value: string) {
        if (this.CountryCurrencyPM.CountryId != value) {
            this.CountryCurrencyPM.CountryId = value;

        }
    }
    get Tenant() { return this.CountryCurrencyPM.Tenant; }
    set Tenant(value: number) {
        if (this.CountryCurrencyPM.Tenant != value) {
            this.CountryCurrencyPM.Tenant = value;

        }
    }
    get Currency() { return this.CountryCurrencyPM.Currency; }
    set Currency(value: string) {
        if (this.CountryCurrencyPM.Currency != value) {
            this.CountryCurrencyPM.Currency = value;
            this.IsDirty = true;
        }
    }
    get CurrencyTypeName() { return this.CountryCurrencyPM.CurrencyTypeName; }
    set CurrencyTypeName(value: string) {
        if (this.CountryCurrencyPM.CurrencyTypeName != value) {
            this.CountryCurrencyPM.CurrencyTypeName = value;

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

