import {Component, OnInit} from '@angular/core';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { CustomsCollateralsAnswerPM } from '../../../Customs/EntityPMs/CustomsCollateralsAnswerPM';
import { CustomsCollateralPM } from '../../../Customs/EntityPMs/CustomsCollateralPM';
import {EntityArgs} from '../../../Infrastructure/DataContracts/EntityArgs';
import {AppTool} from '../../../Infrastructure/Tools';
import {ConfirmWindow} from '../../../Controls/Windows/ConfirmWindow';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { CollateralsRequestFileCondPM } from '../../../Customs/EntityPMs/CollateralsRequestFileCondPM';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';
import { CustomsSettingExtendedListService } from '../../../Customs/Services/ExtendedLists/CustomsSettingExtendedListService';
import { DeclarationPMService } from '../../../Customs/Services/StandardPMs/DeclarationPMService';
import { DeclarationPM } from '../../../Customs/EntityPMs/DeclarationPM';
import { CardListService } from '../../../Common/Services/StandardLists/CardListService'
import { CardList } from '../../../Common/EntityLists/CardList';


declare var window: any;
import {EntityResourceService} from '../../../Infrastructure/Services/EntityResourceService';
import { CustomsCollateralPMService } from '../../../Customs/Services/StandardPMs/CustomsCollateralPMService';
import { forEach } from '@angular/router/src/utils/collection';
import { DeclarationExtendedListService } from '../../../Customs/Services/ExtendedLists/DeclarationExtendedListService';
import { SendCollateralRequestParams } from '../../../Customs/DataContract/RequestParams/SendCollateralRequestParams';
import { Observable } from 'rxjs';
import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { CustomsCollateralAnswerSharedDataService} from '../../../Customs/Services/DataChange/CustomsCollateralAnswerSharedDataService'
import { subscribeOn } from 'rxjs/operator/subscribeOn';
import { MessageWindow } from '../../../Controls/Windows/MessageWindow';
@Component({
    moduleId: module.id,
    templateUrl: './CustomsCollateralAnswerComponent.html',
    providers: [CustomsCollateralPMService, DeclarationExtendedListService, CustomsCollateralAnswerSharedDataService]
})
export class CustomsCollateralAnswerComponent extends BaseComponent implements OnInit {

    public ObjectTableName: string = "Customs.CustomsCollateralsAnswer";
    public DataContext: any = this;
    public EntityPM: CustomsCollateralsAnswerPM;
    collateralPM: CustomsCollateralPM;
    answerFileFilterItems: ApiQueryFilters;
    IsClosed: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    private isGuaranteeDefaultList: boolean = false;
    private isGuaranteeDefaultShow: boolean = false;
    private GuaranteeDefaultList: string[] = [];
    cardListService: CardListService = new CardListService();
    IsConcentrated: boolean;
    collateralToSendlist: string[];
    public ValidationErrorsList: string[] = [];

    constructor(private _customsCollateralAnswerSharedDataService:CustomsCollateralAnswerSharedDataService , public entityArgs: EntityArgs, private EntityResourceService: EntityResourceService, private _customsCollateralPMService: CustomsCollateralPMService,private _declarationExtendedListService: DeclarationExtendedListService) {
        super();
    }

    ngOnInit(): void {
     }

    AnswerForCollateralStatusVisibility: boolean;
    SetTabArgs(args: any) {
        this.EntityPM = args.EntityPM;
        this.collateralPM = args.Parent;
        
        this.IsClosed = this.collateralPM.IsClosed;
        this.CurrentSession.SubscriptionAdd(
            this.CurrentSession.CollateralAnswerRefreshEvent.subscribe((res) => {
                this.SetClosedCollateralScreesn(res.IsClosed);

            })
        );

        this.firstTime = true;
        this.SetClosedCollateralScreesn(this.collateralPM.IsClosed);
        this.BuildAccountingCustomFilesList();
      
        if (this.EntityPM.IsClosed) {
            this.IsClosed = true;
            this.DisplayOnlyMessageVisibility = true;
            this.UIProperties.SetEnabled("RequestFileTypeCode", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("RequestFileAmount", this.ObjectTableName, false);

            this.UIProperties.SetEnabled("AnswerEntityTypeCode", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("AllocatedAmount", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("CustomsTapgFile", this.ObjectTableName, false);
            this.IsGuaranteeDefaultShow = false;
            this.UIProperties.SetEnabled("Remarks", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("CustomsNumeral", this.ObjectTableName, false);

            this.AnswerSentTextVisibility = true;
            this.DisplayOnlyMessageVisibility = true;
            this.RequestNumberLabelVisibility = false;
            this.TapagFileLabelVisibility = false;

            if (this.EntityPM.AnswerForCollateralStatusCode) {
                this.AnswerForCollateralStatusVisibility = true;
            }
        }
        else {
            this.DisplayOnlyMessageVisibility = false;
        }

        if (this.EntityPM.CustomsTapgFile != null) {
            this.RequestNumberLabelVisibility = true;
            this.TapagFileLabelVisibility = true;
            if (!AppTool.IsNullOrEmpty(this.EntityPM.RequestedTapagFile) && !AppTool.IsNullOrEmpty(this.EntityPM.RequestedTapagNumeral)) {
                this.RequestNumber = this.EntityPM.RequestedTapagFile + "/" + this.EntityPM.RequestedTapagNumeral;
            }

            else if (!AppTool.IsNullOrEmpty(this.EntityPM.RequestedTapagFile) && AppTool.IsNullOrEmpty(this.EntityPM.RequestedTapagNumeral)) {
                this.RequestNumber = this.EntityPM.RequestedTapagFile;
            }

            else if (AppTool.IsNullOrEmpty(this.EntityPM.RequestedTapagFile) && !AppTool.IsNullOrEmpty(this.EntityPM.RequestedTapagNumeral)) {
                this.RequestNumber = this.EntityPM.RequestedTapagNumeral;
            }


            if (!AppTool.IsNullOrEmpty(this.CustomsTapgFile) && !AppTool.IsNullOrEmpty(this.CustomsNumeral)) {
                this.TapagFile = this.CustomsTapgFile + "/" +this.CustomsNumeral;
            }

            else if (!AppTool.IsNullOrEmpty(this.CustomsTapgFile) && AppTool.IsNullOrEmpty(this.CustomsNumeral)) {
                this.TapagFile = this.CustomsTapgFile;
            }

            else if (AppTool.IsNullOrEmpty(this.CustomsTapgFile) && !AppTool.IsNullOrEmpty(this.CustomsNumeral)) {
                this.TapagFile = this.CustomsNumeral;
            }
        }



        if (this.EntityPM.PaymentOrderId != null) {
            this.PaymentOrderVsibility = true;
            this.PaymentOrder = this.EntityPM.PaymentOrderNumber;
            this.PaymentOrderStatus = this.EntityPM.PaymentOrderStatus;

           
        }

        
    }

    SetClosedCollateralScreesn(IsClosed: boolean) {
        if (IsClosed) {
            this.IsClosed = true;
            this.UIProperties.SetEnabled("RequestFileTypeCode", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("RequestFileAmount", this.ObjectTableName, false);

            this.UIProperties.SetEnabled("AnswerEntityTypeCode", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("AllocatedAmount", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("CustomsTapgFile", this.ObjectTableName, false);
            this.IsGuaranteeDefaultShow = false;
            this.UIProperties.SetEnabled("Remarks", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("CustomsNumeral", this.ObjectTableName, false);
            if (this.EntityPM.NewFileRequest) {
                this.IsNewFile = true;
            }
            else {
              
                if (this.EntityPM.AnswerEntityTypeCode != null && this.EntityPM.AllocatedAmount != null) {
                    this.IsTapag = true;
                }
            }
           

        }
        else {
            this.IsClosed = false;
            if (!this.EntityPM.NewFileRequest) {

                if (this.EntityPM.AnswerEntityTypeCode != null && this.EntityPM.AllocatedAmount != null) {
                    this.IsTapag = true;
                }

                else if (this.EntityPM.AnswerEntityTypeCode == null && this.EntityPM.AllocatedAmount == null) {
                    return;

                }


                this.IsTapag = true;
                if (this.EntityPM.AnswerEntityTypeCode == null) {
                    this.AnswerEntityRedIconVisibility = true;
                }
                else {
                    this.AnswerEntityRedIconVisibility = false;
                }

                if (AppTool.IsNullOrEmpty(this.EntityPM.CustomsTapgFile)) {
                    this.CustomsTapgFileRedIconVisibility = true;
                }
                else {
                    this.CustomsTapgFileRedIconVisibility = false;
                }

                if (this.EntityPM.AllocatedAmount == null) {
                    this.AllocatedAmountRedIconVisibility = true;
                }
                else {
                    this.AllocatedAmountRedIconVisibility = false;
                }
            }

            if (this.EntityPM.NewFileRequest) {
                this.IsNewFile = true;

                if (this.EntityPM.RequestFileTypeCode != null) {
                    this.RequestFileCodeRedIconVisibility = false;
                }
                else {
                    this.RequestFileCodeRedIconVisibility = true;
                }

                if (this.EntityPM.RequestFileAmount != null) {
                    this.RequestFileRedIconVisibility = false;
                }
                else {
                    this.RequestFileRedIconVisibility = true;
                }

            }
        }

    }
    private timerToken: any;
    UseRequestNewFile(newValue: string) {

        if (!this.collateralPM.IsClosed) {
            this.IsNewFile = null;
            this.IsTapag = null;

            if (this.EntityPM.AnswerEntityTypeCode != null || !AppTool.IsNullOrEmpty(this.EntityPM.CustomsTapgFile) || this.EntityPM.AllocatedAmount != null || !AppTool.IsNullOrEmpty(this.EntityPM.CustomsNumeral) || !AppTool.IsNullOrEmpty(this.EntityPM.Remarks)) {
                var confirmWindow = new ConfirmWindow();
                confirmWindow.YesButtonText = TextCodeTranslator.Translate("Customs.General.B.OK");
                confirmWindow.Show(TextCodeTranslator.Translate("Customs.Declaration.O.DeleteCollateral"));
                this.timerToken = setTimeout(() => {
                    confirmWindow.WindowClosed.subscribe((event: any) => {
                        if (confirmWindow.Yes) {
                            this.IsNewFile = true;
                            this.IsTapag = false;
                            if (this.EntityPM.NewFileRequest) {
                                this.CustomsTapgFile = null;
                                this.AllocatedAmount = null;
                                this.AnswerEntityTypeCode = null;
                                this.CustomsNumeral = null;
                                this.Remarks = null;
                                this.AnswerEntityRedIconVisibility = false;
                                this.AllocatedAmountRedIconVisibility = false;
                                this.CustomsTapgFileRedIconVisibility = false;
                            }
                            else {
                                this.RequestFileAmount = null;
                                this.RequestFileTypeCode = null;
                                this.RequestFileCodeRedIconVisibility = false;
                                this.RequestFileRedIconVisibility = false;
                            }
                        }
                        else {
                            this.IsNewFile = false;
                            this.IsTapag = true;
                        }

                    });
                }, 200);






            }

            else {
                this.IsNewFile = true;
                this.IsTapag = false;

                this.UIProperties.SetEnabled("RequestFileTypeCode", this.ObjectTableName, true);
                this.UIProperties.SetEnabled("RequestFileAmount", this.ObjectTableName, true);
            }

            this.AnswerEntityRedIconVisibility = false;
            this.AllocatedAmountRedIconVisibility = false;
            this.CustomsTapgFileRedIconVisibility = false;


            //else {
            //    this.AnswerEntityRedIconVisibility = false;
            //    this.AllocatedAmountRedIconVisibility = false;
            //    this.CustomsTapgFileRedIconVisibility = false;

            //}

        }
    }
    UseExistingTapagFile(newValue: string) {

        if (!this.collateralPM.IsClosed) {

            this.IsTapag = null;
            this.IsNewFile = null;



            if (this.EntityPM.RequestFileAmount != null || this.EntityPM.RequestFileTypeCode != null) {
                var confirmWindow = new ConfirmWindow();
                confirmWindow.YesButtonText = TextCodeTranslator.Translate("Customs.General.B.OK");
                confirmWindow.Show(TextCodeTranslator.Translate("Customs.Declaration.O.DeleteCollateral"));
                this.timerToken = setTimeout(() => {
                    confirmWindow.WindowClosed.subscribe((event: any) => {
                        if (confirmWindow.Yes) {
                            this.IsTapag = true;
                            this.IsNewFile = false;
                            if (this.EntityPM.NewFileRequest) {
                                this.CustomsTapgFile = null;
                                this.AllocatedAmount = null;
                                this.AnswerEntityTypeCode = null;
                                this.CustomsNumeral = null;
                                this.Remarks = null;
                                this.AnswerEntityRedIconVisibility = false;
                                this.AllocatedAmountRedIconVisibility = false;
                                this.CustomsTapgFileRedIconVisibility = false;
                            }
                            else {
                                this.RequestFileAmount = null;
                                this.RequestFileTypeCode = null;
                                this.RequestFileCodeRedIconVisibility = false;
                                this.RequestFileRedIconVisibility = false;
                            }
                        }
                        else {
                            this.IsTapag = false;
                            this.IsNewFile = true;
                        }

                    });
                }, 1000);
            }
            else {
                this.IsTapag = true;
                this.IsNewFile = false;
                this.UIProperties.SetEnabled("AnswerEntityTypeCode", this.ObjectTableName, true);
                this.UIProperties.SetEnabled("AllocatedAmount", this.ObjectTableName, true);
                this.UIProperties.SetEnabled("CustomsTapgFile", this.ObjectTableName, true);
                if (this.IsGuaranteeDefaultList && (this.AnswerEntityTypeCode == "2" || this.IsConcentrated)) this.IsGuaranteeDefaultShow = true;
                this.UIProperties.SetEnabled("Remarks", this.ObjectTableName, true);
                this.UIProperties.SetEnabled("CustomsNumeral", this.ObjectTableName, true);

            }


            this.RequestFileCodeRedIconVisibility = false

            this.RequestFileRedIconVisibility = false;



            //this.RequestFileCodeRedIconVisibility = false

            //this.RequestFileRedIconVisibility = false;

        }
        
    }

    OpenGuaranteeDefaultListScreen() {

        if (!this.IsGuaranteeDefaultShow) {
            return;
        }

        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Width = 300;
        logitudeWindow.Height = 400;
        logitudeWindow.IsShowCloseButton = true;
        logitudeWindow.Title = "רשימת מספרי ערבות";
        logitudeWindow.WindowArgs = this.GuaranteeDefaultList;
        logitudeWindow.WindowClosed.subscribe(($event: any) => this.OnGuaranteeDefaultListScreenWindowClosed($event));
        logitudeWindow.Show('./CustomsModules/CustomsPaymentOrder/Components/EditTabs/General/AccountingCustomFilesComponent');
    }

    OnGuaranteeDefaultListScreenWindowClosed(arg: any) {
        if (!AppTool.IsNullOrEmpty(arg)) {
            this.CustomsTapgFile = arg;
        }
    }
    
    private BuildAccountingCustomFilesList() {
         this.GuaranteeDefaultList = [];
        var customerCode = "";
        debugger;
        if (!AppTool.IsNullOrEmpty(this.collateralPM.DeclarationId)) {
            let myDeclarationPMService: DeclarationPMService = new DeclarationPMService()
            myDeclarationPMService.get(this.collateralPM.DeclarationId).subscribe(rsptPMget => {
                let entitypm: DeclarationPM = rsptPMget.Result;
                if (entitypm != null && entitypm.CustomerCode != null) {
                    customerCode = entitypm.CustomerCode;
                }
                else {
                    if (AppTool.IsNullOrEmpty(this.collateralPM.CustomerId)) {
                        this.cardListService.getSingle(this.collateralPM.CustomerId)
                            .subscribe(res => {
                                let cardList: CardList = res.Result;
                                if (!AppTool.IsNullOrEmpty(cardList)) {
                                    customerCode = cardList.Code;
                                }
                            });
                    }
                }
                if (!AppTool.IsNullOrEmpty(customerCode))
                {
                    var myCustomsSettingExtendedListService = new CustomsSettingExtendedListService();
                    myCustomsSettingExtendedListService.GetDefault("ISRAEL", "CIM_GUARANTEE_N", "NON", customerCode, SessionLocator.Tenant)
                        .subscribe(response => {
                            this.IsGuaranteeDefaultList = false;
                            if (!response.HasError) {// reEdit this default !!!
                                if (response.Result != null) {
                                    if (!AppTool.IsNullOrEmpty(response.Result.DefaultValue)) {
                                        this.IsGuaranteeDefaultList = true;
                                    }
                                    var result = response.Result.DefaultValue.split(";");
                                    result.forEach((item) => {
                                        this.GuaranteeDefaultList.push(item);
                                    });

                                }
                            }
                        });
                }
            });
        }
        

    }
    

    //#region properties

    public get IsGuaranteeDefaultList() { return this.isGuaranteeDefaultList; }
    public set IsGuaranteeDefaultList(newValue: boolean) { this.isGuaranteeDefaultList = newValue; }

    public get IsGuaranteeDefaultShow() { return this.isGuaranteeDefaultShow; }
    public set IsGuaranteeDefaultShow(newValue: boolean) { this.isGuaranteeDefaultShow = newValue; }
    
    private paymentOrderStatus: string;
    public get PaymentOrderStatus() { return this.paymentOrderStatus; }
    public set PaymentOrderStatus(newValue: string) {
        this.paymentOrderStatus = newValue;
    }

    private paymentOrder: string;
    public get PaymentOrder() { return this.paymentOrder; }
    public set PaymentOrder(newValue: string) {
        this.paymentOrder = newValue;
    }

    firstTime: boolean;
    private isNewFile: boolean;
    public get IsNewFile() { return this.isNewFile; }
    public set IsNewFile(newValue: boolean)
    {
        this.isNewFile = newValue;
        if (newValue && !this.collateralPM.IsClosed) {

            this.IsTapag = false;
            this.UIProperties.SetEnabled("RequestFileTypeCode", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("RequestFileAmount", this.ObjectTableName, true);

            this.UIProperties.SetEnabled("AnswerEntityTypeCode", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("AllocatedAmount", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("CustomsTapgFile", this.ObjectTableName, false);
            this.IsGuaranteeDefaultShow = false;
            this.UIProperties.SetEnabled("Remarks", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("CustomsNumeral", this.ObjectTableName, false);
            this.EntityPM.NewFileRequest = true;

            if (this.EntityPM.RequestFileTypeCode != null) {
                this.RequestFileCodeRedIconVisibility = false;
            }
            else {
                this.RequestFileCodeRedIconVisibility = true;
            }

            if (this.EntityPM.RequestFileAmount != null) {
                this.RequestFileRedIconVisibility = false;
            }
            else {
                this.RequestFileRedIconVisibility = true;
            }

          
        //    this.firstTime = false;

            if ((this.EntityPM.CollateralsRequestFileConds == null || this.EntityPM.CollateralsRequestFileConds.length == 0) && (this.collateralPM != null && this.collateralPM.CustomsCollateralsConditions != null)) 
            {
                for (var item of this.collateralPM.CustomsCollateralsConditions)
                {
                    var collateralsRequestFileCond: CollateralsRequestFileCondPM = new CollateralsRequestFileCondPM(this.collateralPM);
                    collateralsRequestFileCond.CustomsCollateralId = this.EntityPM.CustomsCollateralId;
                    collateralsRequestFileCond.LineNumber = this.EntityPM.LineNumber;
                    collateralsRequestFileCond.Tenant = this.EntityPM.Tenant;
                    collateralsRequestFileCond.ConditionCode = item.ConditionCode;
                    collateralsRequestFileCond.ConditionName = item.ConditionName;
                    collateralsRequestFileCond.RequestedAmount = item.RequestedAmount;
                    this.EntityPM.AddCollateralsRequestFileCond(collateralsRequestFileCond);
                }
            }

        }
    }
   

   private isTapag: boolean;
     public get IsTapag() { return this.isTapag; }
     public set IsTapag(newValue: boolean) {
         this.isTapag = newValue;

         if (newValue && !this.collateralPM.IsClosed) {

             this.IsNewFile = false;
             this.UIProperties.SetEnabled("RequestFileTypeCode", this.ObjectTableName, false);
             this.UIProperties.SetEnabled("RequestFileAmo unt", this.ObjectTableName, false);

             this.UIProperties.SetEnabled("AnswerEntityTypeCode", this.ObjectTableName, true);
             this.UIProperties.SetEnabled("AllocatedAmount", this.ObjectTableName, true);
             this.UIProperties.SetEnabled("CustomsTapgFile", this.ObjectTableName, true);
             if (this.IsGuaranteeDefaultList && (this.AnswerEntityTypeCode == "2" || this.IsConcentrated)) this.IsGuaranteeDefaultShow = true;
             this.UIProperties.SetEnabled("Remarks", this.ObjectTableName, true);
             this.UIProperties.SetEnabled("CustomsNumeral", this.ObjectTableName, true);
             this.EntityPM.NewFileRequest = false;

             if (this.EntityPM.AnswerEntityTypeCode == null) {
                 this.AnswerEntityRedIconVisibility = true;
             }
             else {
                 this.AnswerEntityRedIconVisibility = false;
             }

             if (AppTool.IsNullOrEmpty(this.EntityPM.CustomsTapgFile)) {
                 this.CustomsTapgFileRedIconVisibility = true;
             }
             else {
                 this.CustomsTapgFileRedIconVisibility = false;
             }

             if (this.EntityPM.AllocatedAmount == null) {
                 this.AllocatedAmountRedIconVisibility = true;
             }
             else {
                 this.AllocatedAmountRedIconVisibility = false;
             }
            
           //  this.firstTime = false;

         }
     }
 

     private displayOnlyMessageVisibility: boolean;
     public get DisplayOnlyMessageVisibility() { return this.displayOnlyMessageVisibility; }
     public set DisplayOnlyMessageVisibility(newValue: boolean) { this.displayOnlyMessageVisibility = newValue; }

    private tapagFile: string;
    public get TapagFile() { return this.tapagFile; }
    public set TapagFile(newValue: string) { this.tapagFile = newValue; }

    private requestNumber: string;
    public get RequestNumber() { return this.requestNumber; }
    public set RequestNumber(newValue: string) { this.requestNumber = newValue; }

    public get AnswerEntityTypeCode() { return this.EntityPM ? this.EntityPM.AnswerEntityTypeCode : null; }
    public set AnswerEntityTypeCode(newValue: string) {
        this.EntityPM.AnswerEntityTypeCode = newValue;
        if (AppTool.IsNullOrEmpty(newValue)) {
            this.AnswerEntityRedIconVisibility = true;
        }
        else {
            this.AnswerEntityRedIconVisibility = false;
        }
        if (newValue == "2") {
            if (this.IsGuaranteeDefaultList) {
                this.IsGuaranteeDefaultShow = true;
            }
        }
        else {
            this.IsGuaranteeDefaultShow = false;
        }
    }

    public get AnswerForCollateralStatusName() { return this.EntityPM ? this.EntityPM.AnswerForCollateralStatusName : null; }

    public get AllocatedAmount() { return this.EntityPM ? this.EntityPM.AllocatedAmount : null; }
    public set AllocatedAmount(newValue: number) {
        this.EntityPM.AllocatedAmount = newValue;
        if (AppTool.IsNullOrEmpty(newValue)) {
            this.AllocatedAmountRedIconVisibility = true;
        }
        else {
            this.AllocatedAmountRedIconVisibility = false;
        }
    }


    public get CustomsTapgFile() { return this.EntityPM ? this.EntityPM.CustomsTapgFile : null; }
    public set CustomsTapgFile(newValue: string) {
        this.EntityPM.CustomsTapgFile = newValue;
        if (AppTool.IsNullOrEmpty(newValue)) {
            this.CustomsTapgFileRedIconVisibility = true;
        }
        else {
            this.CustomsTapgFileRedIconVisibility = false;
        }
    }

    public get CustomsNumeral() { return this.EntityPM ? this.EntityPM.CustomsNumeral : null; }
    public set CustomsNumeral(newValue: string) { this.EntityPM.CustomsNumeral = newValue; }

    public get Remarks() { return this.EntityPM ? this.EntityPM.Remarks : null; }
    public set Remarks(newValue: string) { this.EntityPM.Remarks = newValue; }

    public get RequestFileTypeName() { return this.EntityPM ? this.EntityPM.RequestFileTypeName : null; }
    public set RequestFileTypeName(newValue: string) { this.EntityPM.RequestFileTypeName = newValue; }

    public get RequestFileTypeCode() { return this.EntityPM ? this.EntityPM.RequestFileTypeCode : null; }
    public set RequestFileTypeCode(newValue: string) {
        this.EntityPM.RequestFileTypeCode = newValue;
        if (AppTool.IsNullOrEmpty(newValue)) {
            this.RequestFileCodeRedIconVisibility = true;
        }
        else {
            this.RequestFileCodeRedIconVisibility = false;
        }
    }


    public get RequestFileAmount() { return this.EntityPM ? this.EntityPM.RequestFileAmount : null; }
    public set RequestFileAmount(newValue: number) {
        this.EntityPM.RequestFileAmount = newValue;
        if (AppTool.IsNullOrEmpty(newValue)) {
            this.RequestFileRedIconVisibility = true;
        }
        else {
            this.RequestFileRedIconVisibility = false;
        }
    }

    public get NewFileRequest() { return this.EntityPM ? this.EntityPM.NewFileRequest : null; }
    public set NewFileRequest(newValue: boolean) {
        this.EntityPM.NewFileRequest = newValue;

    }

    private customsTapgFileRedIconVisibility: boolean;
    public get CustomsTapgFileRedIconVisibility() { return this.customsTapgFileRedIconVisibility; }
    public set CustomsTapgFileRedIconVisibility(newValue: boolean) { this.customsTapgFileRedIconVisibility = newValue; }

    private allocatedAmountRedIconVisibility: boolean;
    public get AllocatedAmountRedIconVisibility() { return this.allocatedAmountRedIconVisibility; }
    public set AllocatedAmountRedIconVisibility(newValue: boolean) { this.allocatedAmountRedIconVisibility = newValue; }

    private answerEntityRedIconVisibility: boolean;
    public get AnswerEntityRedIconVisibility() { return this.answerEntityRedIconVisibility; }
    public set AnswerEntityRedIconVisibility(newValue: boolean) { this.answerEntityRedIconVisibility = newValue; }

    private requestFileRedIconVisibility: boolean;
    public get RequestFileRedIconVisibility() { return this.requestFileRedIconVisibility; }
    public set RequestFileRedIconVisibility(newValue: boolean) { this.requestFileRedIconVisibility = newValue; }

    private requestFileCodeRedIconVisibility: boolean;
    public get RequestFileCodeRedIconVisibility() { return this.requestFileCodeRedIconVisibility; }
    public set RequestFileCodeRedIconVisibility(newValue: boolean) { this.requestFileCodeRedIconVisibility = newValue; }

    private paymentOrderVsibility: boolean;
    public get PaymentOrderVsibility() { return this.paymentOrderVsibility; }
    public set PaymentOrderVsibility(newValue: boolean) { this.paymentOrderVsibility = newValue; }

    private tapagFileLabelVisibility: boolean;
    public get TapagFileLabelVisibility() { return this.tapagFileLabelVisibility; }
    public set TapagFileLabelVisibility(newValue: boolean) { this.tapagFileLabelVisibility = newValue; }

    private requestNumberLabelVisibility: boolean;
    public get RequestNumberLabelVisibility() { return this.requestNumberLabelVisibility; }
    public set RequestNumberLabelVisibility(newValue: boolean) { this.requestNumberLabelVisibility = newValue; }


    private answerSentTextVisibility: boolean;
    public get AnswerSentTextVisibility() { return this.answerSentTextVisibility; }
    public set AnswerSentTextVisibility(newValue: boolean) { this.answerSentTextVisibility = newValue; }

    currentScreenCode: string;
    OpenPaymentOrder() {
        this.EntityResourceService.getEntityResourceByTableName("Customs.PaymentOrder").subscribe(response => {
            this.EntityResourceService.getEntityResourceByTableName("Customs.PaymentOrderLine").subscribe(response => {
                this.EntityResourceService.getEntityResourceByTableName("Customs.PaymentOrderMethod").subscribe(response => {
                    this.EntityResourceService.getEntityResourceByTableName("Customs.PaymentOrderProtestReason").subscribe(response => {
                        this.EntityResourceService.getEntityResourceByTableName("Customs.CustomsSetting").subscribe(response => {

                            this.EditEntity("Customs.PaymentOrder", this.EntityPM.PaymentOrderId, null, "POGN");
                        });
                    });
                });
            });
        });


                        }




    public EditEntity(objectTableName: string, entityId: string, windowTitle: string, defaultSelectedTabCode: string) {

        var editWindow = new LogitudeWindow();

        editWindow.ShowHeaderButtons = true;
        editWindow.Title = windowTitle;
        editWindow.Height = 770;
        editWindow.Width = 1500;

        editWindow.ShowEditComponent(entityId, objectTableName, defaultSelectedTabCode);
        editWindow.WindowClosed.subscribe(res => {



        });

    }

    SetWindowArgs(args: any) {
      //  this.AnswerEntityTypeCode = "2";
        this.IsGuaranteeDefaultList = true;
        this.IsConcentrated = true;
         this.collateralToSendlist = args.collateralToSendlist;
        this.EntityPM = new CustomsCollateralsAnswerPM(null);
        this.collateralPM = new CustomsCollateralPM();
        this.collateralPM.DeclarationId = args.DeclarationId;
        debugger;
          this.BuildAccountingCustomFilesList();

    }


    CancelButtonClicked() {


        this.CurrentSession.CloseCurrentWindow();
    }
    FIELD_IS_REQUIERD: string;

    GetRequierdFieldErrorText(fieldName) {
        this.FIELD_IS_REQUIERD = TextCodeTranslator.Translate("General.M.FieldIsRequired");

        return this.FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator.Translate(fieldName));
    }
    OnCustomSendOptionsButtonClick() {
        var currentEntity: CustomsCollateralPM;
        var errors: string[] = [];
        this.ValidationErrorsList = [];

        if (this.EntityPM.CustomsTapgFile == null) {
            errors.push(this.GetRequierdFieldErrorText("Customs.CustomsCollateralsAnswer.F.CustomsTapgFile"));

        }

     
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length > 0) {
            return;
        }
        let count: number=0;
        for (var i = 0; i < this.collateralToSendlist.length; i++) {
            this._customsCollateralPMService.get(this.collateralToSendlist[i].toString()).subscribe(
                data => {
                    currentEntity = (data.Result as CustomsCollateralPM);
                     currentEntity.AddCustomsCollateralsAnswer(this.EntityPM);
                    this._customsCollateralPMService.update(currentEntity).subscribe(res => {

                   
                });
        }


            );
        }



        let requestParams: SendCollateralRequestParams = new SendCollateralRequestParams();

        requestParams.Collaterals = this.collateralToSendlist;
        requestParams.Tenant = 1;

        SessionLocator.SelectedSession.StartBusyIndicatorLoading();

        this._declarationExtendedListService.PostSendCollateral8212(requestParams).subscribe(res => {
            SessionLocator.SelectedSession.StopBusyIndicator();
            var myMessageWindow = new MessageWindow();
            myMessageWindow.Show(res.Result);
        });
 
    }

    //#endregion
}
