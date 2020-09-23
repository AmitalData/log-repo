declare var window: any;
import { MenuButtonPM } from '../../../Infrastructure/EntityPMs/MenuButtonPM'
import { TenantPM } from '../../../Common/EntityPMs/TenantPM';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { EntityArgs } from '../../../Infrastructure/DataContracts/EntityArgs';
import { FeatureLocator } from '../../../Infrastructure/Utilities/FeatureLocator';
import { InterestBasesTypePM } from '../../EntityPMs/InterestBasesTypePM';
import { InterestReportPM } from '../../EntityPMs/InterestReportPM';
import { GeneralPrintHelper } from '../../../Infrastructure/Helpers/GeneralPrintHelper';
import { ServiceLocator } from '../../../Infrastructure/Locators/ServiceLocator';
import { ConfirmWindow } from '../../../Controls/Windows/ConfirmWindow';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { InterestTransactionExtendedListService } from '../../Services/ExtendedLists/InterestTransactionExtendedListService';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { InterestReportPMService } from '../../Services/StandardPMs/InterestReportPMService';
import { ARInvoicePM } from '../../../Invoice/EntityPMs/ARInvoicePM';
import { InvoicePartnerType, InvoiceTool } from '../../../Invoice/Tools';
import { ARInvoiceEntityPM } from '../../../Invoice/EntityPMs/ARInvoiceEntityPM';
import { ARInvoiceLinePM } from '../../../Invoice/EntityPMs/ARInvoiceLinePM';
 import { DateTool, AppTool } from '../../../Infrastructure/Tools';
import { ChargesTypeListService } from '../../../Common/Services/StandardLists/ChargesTypeListService';
import { ApiQueryFilters } from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import { ChargesTypeList } from '../../../Common/EntityLists/ChargesTypeList';
import { VatTypeList } from '../../../Common/EntityLists/VatTypeList';
import { VatTypeListService } from '../../../Common/Services/StandardLists/VatTypeListService';
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { reject } from 'q';
import { CurrencyRatesService, LastRate } from '../../../Common/Services/CurrencyRatesService';
import { CardListService } from '../../../Common/Services/StandardLists/CardListService';
import { CardList } from '../../../Common/EntityLists/CardList';
import { GLAccountPMService } from '../../Services/StandardPMs/GLAccountPMService';
import { GLAccountPM } from '../../EntityPMs/GLAccountPM';
import { CommonDomainService } from '../../../Common/Services/CommonDomainService';
import { VatTypePercentagePM } from '../../../Common/EntityPMs/VatTypePercentagePM';
import { MessageWindow } from '../../../Controls/Windows/MessageWindow';
import { InterestReportExtendedListService } from 'Accounting/Services/ExtendedLists/InterestReportExtendedListService';

export class InterestReportMenuButtonsHandler extends BaseComponent  {
    public EntityPM: InterestReportPM;
    public entityArgs: EntityArgs;
    public TenantPM: TenantPM;
    public ObjectTableName: string = "InterestReport";
    public chargesTypeList: ChargesTypeList;
    public InterestReportService: InterestTransactionExtendedListService;
    public ChargesTypePMService: ChargesTypeListService;
    private CurrentSession = SessionLocator.SelectedSession;
    private myCurrencyRatesService: CurrencyRatesService;
    private myCardListService: CardListService;
    private interestReportExtendedListService: InterestReportExtendedListService;

    public SetEntityPM(entityArgs: EntityArgs) {
        this.TenantPM = SessionLocator.TenantPM;
        this.entityArgs = entityArgs;
        this.EntityPM = entityArgs.EntityPM;
        this.InterestReportService = new InterestTransactionExtendedListService();
        this.ChargesTypePMService = new ChargesTypeListService();
        this.myCurrencyRatesService = new CurrencyRatesService();
        this.myCardListService = new CardListService();
        this.interestReportExtendedListService = new InterestReportExtendedListService();

     }
 

    public CheckButtonState(menuButtons: MenuButtonPM[]) {
        if (this.EntityPM != null) {
            if (this.entityArgs.EditComponent != null) {
                for (var i = 0; i < menuButtons.length; i++) {
                    var button = menuButtons[i];
                    switch (button.EventCode) {
                        case "InterestPrint":
                            {
                                button.IsDisabled = false;
                                break;
                            }
                        case "CreateInvoice" :
                        case "CloseWithoutInvoice" :
                            {

                                if (this.EntityPM.InterestReportStatusCode == "1" || this.EntityPM.InterestReportStatusCode == "9")
                                    button.IsDisabled = false;
                                else
                                    button.IsDisabled = true;
                                break;
                            }
                        case "IRCN":
                            {
                                if (this.EntityPM.InterestReportStatusCode == "3")
                                    button.IsDisabled = true;
                                else
                                    button.IsDisabled = false;
                                break;
                            }
                    }
                }
            }
        }
        return menuButtons;
    }


    public MenuButtonClick(menuButton: MenuButtonPM) {
        var errors = [];

        if (errors.length == 0) {
            switch (menuButton.EventCode) {
                case "InterestPrint":
                    {
                        this.PrintInterestReport();
                        break;
                    }
                case "CreateInvoice":
                    {
                        this.CheckInterestReportStatusCodeAndCreateInvoice();
                        break;
                    }
                case "CloseWithoutInvoice":
                    {
                        this.ConfirmCreateInvoice(TextCodeTranslator.Translate('InterestReport.O.ConfirmClosingWithoutInvoice'));
                        break;
                    }
                case "IRCN": {
                    this.CancelReport();
                    break;
                }
            }
        }
        else {
            this.entityArgs.EditComponent.ValidationErrorsList = [];
            this.entityArgs.EditComponent.ValidationErrorsList = errors;
        }
    }

    private PrintInterestReport() {
        var myPrintHelper = new GeneralPrintHelper(this.ObjectTableName, "ITDT", this.EntityPM.Id, null, this.EntityPM.ReportNumber, null);
        if (myPrintHelper.IsLoadPrintControl) {
            ServiceLocator.SendTotangoUserActivity(this.ObjectTableName, "InterestReport");
            myPrintHelper.ShowPrintControl();
        }
    }

    private _ARInvoicePM: ARInvoicePM;

    private ConfirmCreateInvoice(ConfirmText:string) {
        let confirmWindow = new ConfirmWindow();
        confirmWindow.Width = 400;
        confirmWindow.YesButtonText = TextCodeTranslator.Translate('InterestReport.O.Approve');
        confirmWindow.NoButtonText = TextCodeTranslator.Translate('InterestReport.O.Cancel');

        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {
                this.ApproveConfirmCreateInvoice();
            }
        });
        confirmWindow.Show(ConfirmText);
    }
    ShowErrorMessage() {

        var messageWindow = new MessageWindow();
        messageWindow.Show(TextCodeTranslator.Translate("InterestReport.O.CantCancel")); 
    }
    UpdateReport() {
        this.EntityPM.InterestReportStatusCode = "3";
        this.entityArgs.EditComponent.SaveChanges();
        this.entityArgs.EditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
            if (isSaveSuccess) {
                this.entityArgs.EditComponent.ReloadEntityPM();
            }
        });

    }
    OpenConfirmWindow() {
        var confirmMessage: string = null;
        let confirmWindow = new ConfirmWindow();
      if (this.EntityPM.InterestReportStatusCode == "1" || this.EntityPM.InterestReportStatusCode == "4" || this.EntityPM.InterestReportStatusCode == "6" || this.EntityPM.InterestReportStatusCode == "9") {
            confirmMessage = TextCodeTranslator.Translate("InterestReport.O.ConfirmCancelling");
           
        } else if (this.EntityPM.InterestReportStatusCode == "2") {
            confirmMessage = TextCodeTranslator.Translate("InterestReport.O.CancelingInvoicedReportMessage");
        }
        else if (this.EntityPM.InterestReportStatusCode == "5") {
            confirmMessage = TextCodeTranslator.Translate("InterestReport.O.TheReportisinProgress");
            confirmWindow.ShowNoButton=false;
        }
      else if (this.EntityPM.InterestReportStatusCode == "8") {
        confirmMessage = TextCodeTranslator.Translate("InterestReport.O.ReportIsBeingInvoiced");
      }
        confirmWindow.Width = 400;
        confirmWindow.YesButtonText = TextCodeTranslator.Translate('Accounting.General.B.OK');
        confirmWindow.NoButtonText = TextCodeTranslator.Translate('Accounting.General.B.Cancel');
        
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {
                if(this.EntityPM.InterestReportStatusCode == "5"){
                    confirmWindow.Close();
                }
                else{
                    this.UpdateReport();
                }
            }
        });
        confirmWindow.Show(confirmMessage);

    }

 
    CancelReport() {
        this.CurrentSession.StartBusyIndicatorLoading();
        this.InterestReportService.GetCheckRecentReports(this.EntityPM.InterestCalculationDate, this.EntityPM.CustomerId).subscribe((myResult: ServiceResponse) => {
            this.CurrentSession.StopBusyIndicator();
            var response: ServiceResponse = myResult;
            if (!response.HasError) {
                if (response.Result) {
                    this.ShowErrorMessage();
                } else {
                    this.OpenConfirmWindow();
                }
            }
            else {

                this.entityArgs.EditComponent.ValidationErrorsList = response.ErrorsArray;
            }
        });
    }
    ApproveConfirmCreateInvoice() {
        this.CurrentSession.StartBusyIndicatorLoading();
        this.InterestReportService.PutConfirmCreateInvoice(this.EntityPM).subscribe((myResult: ServiceResponse) => {
            this.CurrentSession.StopBusyIndicator();
            var mm: ServiceResponse = myResult;
            if (!mm.HasError) {
                this.EntityPM = mm.Result;
                this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
            }
            else {


            }
        });
    }

    
    private CheckInterestReportStatusCodeAndCreateInvoice(){
        this.CurrentSession.StartBusyIndicatorLoading();
        this.interestReportExtendedListService.IsCreateInvoicedValid(this.EntityPM).subscribe((myResult: ServiceResponse) => {
            this.CurrentSession.StopBusyIndicator();
            var response: ServiceResponse = myResult;
            if (!response.HasError) {
               var IsValid:boolean =response.Result;
               if(IsValid){

                if ((!this.EntityPM.TotalAmount) ||
                   (!this.EntityPM.TotalAmount && !this.EntityPM.GLAccountMinimumInterest) ||
                   (this.EntityPM.GLAccountMinimumInterest && this.EntityPM.GLAccountMinimumInterest >= this.EntityPM.TotalAmount)) {
                      
                       this.ConfirmCreateInvoice(TextCodeTranslator.Translate('InterestReport.O.ReportTotalAmountIslowerthanGLAccountMinimumamount'));
                    }
               
                else {

                     this.GeTARInvoice();

                  }
               }
               else{
                  this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
               }
            }
            else {
                this.entityArgs.EditComponent.ValidationErrorsList = response.ErrorsArray;
                this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                
            }
        });
    }

public GetARInvoicePMWithLine(): ARInvoicePM {
        var _ARInvoicePM: ARInvoicePM = this.getMappingARInvoicePM();
        var _ARInvoiceLinePM: ARInvoiceLinePM = this.getMappingARInvoiceLinePM(_ARInvoicePM);
        var _ARInvoiceEntityPM: ARInvoiceEntityPM = this.getMappingARInvoiceEntityPM();
        _ARInvoicePM.InvoiceEntities.push(_ARInvoiceEntityPM);
        _ARInvoicePM.InvoiceLines.push(_ARInvoiceLinePM);
        return _ARInvoicePM;
  }
 

  private getMappingARInvoiceLinePM(_ARInvoicePM: ARInvoicePM ){
      

    var _ARInvoiceLinePM: ARInvoiceLinePM = new ARInvoiceLinePM(_ARInvoicePM);
    _ARInvoiceLinePM.DateForInterest = _ARInvoicePM.InvoiceDate;
    _ARInvoiceLinePM.Tenant = this.TenantPM.Id;
    _ARInvoiceLinePM.InvoiceLocalCurrencyCode = this.TenantPM.CurrencyCode;
    _ARInvoiceLinePM.ForiegnCurrencyCode = this.TenantPM.CurrencyCode;
    _ARInvoiceLinePM.ForiegnCurrencyId = this.TenantPM.CurrencyId;
    _ARInvoiceLinePM.UnitPrice = this.EntityPM.TotalAmount;
    _ARInvoiceLinePM.Quantity = 1; 
    _ARInvoiceLinePM.UnitPrice = this.EntityPM.TotalAmount; 
    _ARInvoiceLinePM.ForiegnCurrencyAmount = this.EntityPM.TotalAmount;
    _ARInvoiceLinePM.InvoiceCurrencyAmount = this.EntityPM.TotalAmount;
    _ARInvoiceLinePM.ProfitCurrencyAmount = this.EntityPM.TotalAmount;
    _ARInvoiceLinePM.LocalCurrencyAmount = this.EntityPM.TotalAmount; 
        _ARInvoiceLinePM.InvoiceCurrencyCode = this.TenantPM.CurrencyCode;
        var length = this.EntityPM.InterestReportLinesByDates.length;
        _ARInvoiceLinePM.Description = "Interest between " +this.getDateString(this.EntityPM.InterestReportLinesByDates.sort()[0].FromDate) + " and " + this.getDateString(this.EntityPM.InterestReportLinesByDates.sort()[length-1].ToDate);
        _ARInvoiceLinePM.LocalDescription = "ריבית לתאריכים " + this.getDateString(this.EntityPM.InterestReportLinesByDates.sort()[0].FromDate) + " עד " + this.getDateString(this.EntityPM.InterestReportLinesByDates.sort()[length-1].ToDate);
     _ARInvoiceLinePM.ChargesTypeId = this.chargesTypeList? this.chargesTypeList.Id:null;
        _ARInvoiceLinePM.VatTypeId =  this.chargesTypeList.VatTypeId; 
     _ARInvoiceLinePM.GLAccountId = this.chargesTypeList.ReceivableCreditGLAccountId;
    _ARInvoiceLinePM.ForiegnExchangeRate = _ARInvoiceLinePM.ForiegnCurrencyAmount / _ARInvoiceLinePM.LocalCurrencyAmount;
    _ARInvoiceLinePM.VatPercentage = this.GetVatTypePercentage(_ARInvoiceLinePM.VatTypeId);
    _ARInvoiceLinePM.VatTypeName = this.VatTypeName;
    
      return  _ARInvoiceLinePM;
  }


  private getMappingARInvoiceEntityPM(){
      
    var _ARInvoiceEntityPM: ARInvoiceEntityPM = new ARInvoiceEntityPM();
    _ARInvoiceEntityPM.Tenant = this.TenantPM.Id;
    _ARInvoiceEntityPM.EntityId = this.EntityPM.Id;
    _ARInvoiceEntityPM.EntityReference = this.EntityPM.ReportNumber;
    var objectTable = window.ObjectTables.filter(d => d.Name === "InterestReport")[0];
    var objectTableId = objectTable.Id;
    _ARInvoiceEntityPM.ObjectTableId = objectTableId;

    return  _ARInvoiceEntityPM;
  }



  private getMappingARInvoicePM(){
    var _ARInvoicePM: ARInvoicePM = new ARInvoicePM();
    _ARInvoicePM.ARInvoiceTypeCode = "IT";
     _ARInvoicePM.BillToGLAccountId = this.EntityPM.GLAccountId;
     _ARInvoicePM.BillToId = this.EntityPM.CustomerId;
    _ARInvoicePM.BillToLocalName = this.EntityPM.CustomerLocalName;
    _ARInvoicePM.BillToName = this.EntityPM.CustomerName;
    _ARInvoicePM.Tenant = this.TenantPM.Id;
      this.InvoicePartners = InvoiceTool.GetARInvoicePartners(null);
     this.PartnersTypeSelectionMethod(this.InvoicePartners[0]);
    _ARInvoicePM.BillToPartnerTypeId = this.BillToPartnerTypeId;
    _ARInvoicePM.AmountInLocalCurrency = this.EntityPM.TotalAmount;
    _ARInvoicePM.LocalCurrencyId = this.TenantPM.CurrencyId;
    _ARInvoicePM.InvoiceCurrencyId = this.TenantPM.CurrencyId;
    _ARInvoicePM.AmountInInvoiceCurrency = this.EntityPM.TotalAmount;
    _ARInvoicePM.AmountInProfitCurrency = this.EntityPM.TotalAmount;
    _ARInvoicePM.BranchId  = SessionLocator.LoggedUserPM.BranchId;;
    var todayDate = DateTool.GetCurrentDateAsUtc();
    _ARInvoicePM.CreateDate = todayDate;
    _ARInvoicePM.UpdateDate = todayDate;
    _ARInvoicePM.InvoiceDate = this.EntityPM.InterestCalculationDate;
    _ARInvoicePM.BranchId = SessionLocator.LoggedUserPM.BranchId;
    _ARInvoicePM.LocalCurrencyId = SessionLocator.TenantPM.CurrencyId;
    _ARInvoicePM.IssuedByUserId = SessionLocator.LoggedUserId;
    _ARInvoicePM.CreatedByUserId = SessionLocator.LoggedUserId;
    _ARInvoicePM.UpdatedByUserId = SessionLocator.LoggedUserId;
    _ARInvoicePM.MainEntityId = null;
    _ARInvoicePM.MainEntityReference = null;
    _ARInvoicePM.HasInterestFeature=true;
    _ARInvoicePM.HouseNumber = null;
    _ARInvoicePM.MasterNumber = null;
    var myDescription: string = null;
    _ARInvoicePM.Description = myDescription;
    _ARInvoicePM.IsGeneralInvoice = true;
    _ARInvoicePM.IsFullAccounting = true;
    _ARInvoicePM.InvoiceCurrencyId = SessionLocator.TenantPM.CurrencyId;
    _ARInvoicePM.InvoiceCurrencyCode = this.TenantPM.CurrencyCode;
     InvoiceTool.ComputeARInvoiceDueDate(_ARInvoicePM);
    _ARInvoicePM.ProfitCurrencyId = SessionLocator.TenantPM.ProfitCurrencyId;
    _ARInvoicePM.ProfitCurrencyCode = SessionLocator.TenantPM.ProfitCurrencyCode;
    if (this.cardList  != null) {
         _ARInvoicePM.BillToName = this.cardList .EnglishName;
         _ARInvoicePM.BillToLocalName = this.cardList .LocalName;
        if (!AppTool.IsNullOrEmpty(this.cardList .SATPaymentMethodCode)) {
             _ARInvoicePM.SATPaymentMethodCode = this.cardList .SATPaymentMethodCode;
        }

        if (!AppTool.IsNullOrEmpty(this.cardList .InvoiceCurrencyId)) {
             _ARInvoicePM.InvoiceCurrencyId = this.cardList .InvoiceCurrencyId;
        }

        if (!AppTool.IsNullOrEmpty(this.cardList .PaymentTermId)) {
             _ARInvoicePM.PaymentTermId = this.cardList .PaymentTermId;
        }

        if (!AppTool.IsNullOrEmpty(this.cardList .VatNumber)) {
             _ARInvoicePM.VatNumber = this.cardList .VatNumber;
        }

        if (!AppTool.IsNullOrEmpty(this.cardList .BillingAddressId)) {
            _ARInvoicePM.BillToAddressId = this.cardList .BillingAddressId;
        }

        else if (!AppTool.IsNullOrEmpty(this.cardList .MainAddressId)) {
            _ARInvoicePM.BillToAddressId = this.cardList.MainAddressId;
        }

    }
    return _ARInvoicePM;
  
  }
  public  getDateString(DateTime: Date): string {
        var month =DateTool.GetDateParts(DateTime).Month;
        var year = DateTool.GetDateParts(DateTime).Year;
        var day = DateTool.GetDateParts(DateTime).Day;
        return day + "/" + month + "/" + year
    }

    public InvoicePartners: InvoicePartnerType[] = [];
    public SelectedPartnerType: InvoicePartnerType = null;
    PartnersTypeSelectionMethod(selected: InvoicePartnerType) {
        if (this.SelectedPartnerType != selected) {
            this.SelectedPartnerType = selected;
            this.BillToPartnerTypeId = selected.PartnerTypeId;
        }
    }

    private VatTypePercentagesList: VatTypePercentagePM[] = [];
    getVatTypePercentegeListByDates() {
        return new Promise(resolve => {
        var loadingDate = this.EntityPM.InterestCalculationDate;
        if (loadingDate == null) {
            loadingDate = DateTool.GetCurrentDateAsUtc();
        }
        var myService: CommonDomainService = new CommonDomainService();
        myService.GetVatTypePercentagePMByDate(loadingDate).subscribe((myResponse2: ServiceResponse) => {
            if (!myResponse2.HasError) {
                this.VatTypePercentagesList = myResponse2.Result;
                resolve(myResponse2.Result);
             }
             else {
                 reject();
             }
        });
    });
    }
public VatTypeName:string;
   public GetVatTypeName(vatTypeId: string){
        return new Promise(resolve => {
        var myVatTypeListService: VatTypeListService = new VatTypeListService();

        myVatTypeListService.getSingleFromCache(vatTypeId).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                var list: VatTypeList = myResponse.Result;
                if (list != null) {
                    this.VatTypeName = list.EnglishName;
                    resolve(myResponse.Result);
                 }
                 else {
                    resolve(null);
                }
            }
        });
    });
    }

    GetVatTypePercentage(vatTypeId: string) {
        var myResult: number = null;

        var vatTypePercentagePM = this.VatTypePercentagesList.filter(d => d.VatTypeId == vatTypeId)[0];
        if (vatTypePercentagePM != null) {
            myResult = vatTypePercentagePM.Percentage;
        }

        return myResult;
    }
    private billToPartnerTypeId: string;
    get BillToPartnerTypeId() { return this.billToPartnerTypeId; }
    set BillToPartnerTypeId(newValue: string) {
        if (this.billToPartnerTypeId != newValue) {
            this.billToPartnerTypeId = newValue;
        }
    }

    private GeTARInvoice() {

        var filters = new ApiQueryFilters(true);
        filters.addAdditionalFilter("Code", "INT", null, null, "Equals", false, false, false, "string");
        filters.addAdditionalFilter("Tenant", this.TenantPM.Id, null, null, "Equals", false, false, false, "string");
        filters.addAdditionalFilter("InActive", false, null, null, "Equals", false, false, false, "Boolean");
        this.CurrentSession.StartBusyIndicatorLoading();
        this.ChargesTypePMService.getByFilters(filters).subscribe((myResponse: ServiceResponse) => {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    this.chargesTypeList = myResponse.Result[0];
                    this.GetBillToCard( this.EntityPM.CustomerId).then(res => {  
                        this.getVatTypePercentegeListByDates().then(res => {   
                            this.GetVatTypeName(this.chargesTypeList.VatTypeId).then(res => {
                                this._ARInvoicePM = this.GetARInvoicePMWithLine();
                                   this.LoadCurrencyRates().then(res => {
                                    this.CurrentSession.StopBusyIndicator();
                                        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                                          .then(cmpRef => {
                                              cmpRef.instance.ComponentRef = cmpRef;
                                              cmpRef.instance.Run({ EntityPM: this._ARInvoicePM, ObjectTableName: 'ARInvoice' });
                                              cmpRef.instance.BackCompleted.subscribe(($event: any) => this.CurrentSession.CurrentEditComponent.ReloadEntityPM());
                                         });
                                    });
                        });
                      });
                    });
                 
                }
            }
        });
    }
    
 
    private LastRatesList: LastRate[] = [];
    LoadCurrencyRates() {
        return new Promise(resolve => {
            var loadingDate = this._ARInvoicePM.InvoiceDate || DateTool.GetCurrentDateAsUtc();
            this.myCurrencyRatesService.GetCurrenciesExchangeRateByValueDate(SessionLocator.TenantPM.CurrencyId, loadingDate).subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {
                    this.LastRatesList = myResponse.Result;
                    this.SetCurrencyRateData();
                    resolve(myResponse.Result);
                }
                else {
                    reject();
                }
            });
        });

    }
    SetCurrencyRateData() {
        var myRate: number = null;
        var myRateDate: Date = null;

        if (!AppTool.IsNullOrEmpty(this._ARInvoicePM.InvoiceCurrencyId)) {
            if (this._ARInvoicePM.InvoiceCurrencyId == SessionLocator.TenantPM.CurrencyId) {
                myRate = 1;
            }

            else {
                var lastRate: LastRate = this.LastRatesList.filter(d => d.ForeignCurrencyId == this._ARInvoicePM.InvoiceCurrencyId)[0];
                if (lastRate != null) {
                    myRate = lastRate.Rate;
                    myRateDate = lastRate.ValueDate;
                }
            }
        }

        this._ARInvoicePM.InvoiceCurrencyExchangeRate = myRate;
        this._ARInvoicePM.ProfitCurrencyExchangeRate = myRate;
        this._ARInvoicePM.ExchangeRateDate = myRateDate;
    }
    public cardList: CardList;
    GetBillToCard(BillToId:string) {
        return new Promise(resolve => {
        this.myCardListService.getSingle(BillToId).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                var list: CardList = myResponse.Result;
                this.cardList = list;
                resolve(myResponse.Result);
            }
            else {
                reject();
            }
        }); 
    });
   }



     

}
