import {Component, ChangeDetectorRef} from '@angular/core';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';
import {Validator} from '../../../Infrastructure/Validators/Validator';
import {TenantPM} from '../../../Common/EntityPMs/TenantPM';
import {ReconcileExternalPagePM} from '../../EntityPMs/ReconcileExternalPagePM';
import {ReconcileExternalPageLinePM} from '../../EntityPMs/ReconcileExternalPageLinePM';
import {BankAccountPM} from '../../EntityPMs/BankAccountPM';
import {GLAccountPM} from '../../EntityPMs/GLAccountPM';
import {EntityResourceService} from '../../../Infrastructure/Services/EntityResourceService';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ReconcileExternalPagePMService} from '../../Services/StandardPMs/ReconcileExternalPagePMService';
import {CurrencyPMService} from '../../../Common/Services/StandardPMs/CurrencyPMService';
import {CurrencyListService} from '../../../Common/Services/StandardLists/CurrencyListService';
import {ReconcileExternalPageExtendedPMService} from '../../Services/ExtendedPMs/ReconcileExternalPageExtendedPMService';
import {EntityListService} from '../../../Infrastructure/Services/EntityListService';
import {AppTool, DateTool} from '../../../Infrastructure/Tools';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import {ObservableCollection} from '../../../Infrastructure/Utilities/ObservableCollection';
import {ObjectsLocator} from '../../../Infrastructure/Locators/ObjectsLocator';


@Component({
    selector: 'LoadRecoExPageComponent',
    moduleId: module.id,
    providers: [EntityListService],
    templateUrl: './LoadRecoExPageComponent.html',
})

export class LoadRecoExPageComponent extends BaseComponent {
    public ReconcileExternalPagePM: ReconcileExternalPagePM;
    public BankAccountPM: BankAccountPM;
    public PrevBankPagePM: ReconcileExternalPagePM;
    public DataContext: LoadRecoExPageComponent = this;
    public ObjectTableName: string = "ReconcileExternalPage";
    public ValidationErrorsList: string[] = [];
    isNewEntity: boolean = false;
    IsCancelApprovedEnabled: boolean = false;
    public IsDisplayOnly: boolean = false;
    public IsMultiCurrency: boolean = false;
    currency: any;
    AMOUNT_TEXT = TextCodeTranslator.Translate("ReconcileExternalPageLine.F.Amount");
    AmountColHeader: string;
    PageLinesList: ObservableCollection;
    public isRTL: boolean = false;
    public TotalSum: number = 0.0;
    public Difference: number = 0.0;

    _entityResourceService: EntityResourceService = new EntityResourceService();
    _ReconcileExternalPagePMService: ReconcileExternalPagePMService = new ReconcileExternalPagePMService();
    _ReconcileExternalPageExtendedPMService: ReconcileExternalPageExtendedPMService = new ReconcileExternalPageExtendedPMService();
    _CurrencyPMService: CurrencyPMService = new CurrencyPMService();
    currencyListService: CurrencyListService = new CurrencyListService();


    constructor(private CD: ChangeDetectorRef, public entityListService: EntityListService) {
        super();
        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");

        this.PageLinesList = new ObservableCollection([]);

        
    }

    SetWindowArgs(args) {

    }

    //#region Properties


    tenantCurrency: any;
    get TenantCurrency() { return this.tenantCurrency }
    set TenantCurrency(value: any) {
        if (this.tenantCurrency != value) {
            this.tenantCurrency = value;

        }
    }
    //#endregion

    //#region Buttons Handlers



    CancelButtonClicked() {
        SessionLocator.CurrentSession.CloseCurrentWindow();
    }

    //* grid handlers in seperate region

    //#endregion

   



    //#region Prev Bank Page

    //#endregion



    //#region Lines Grid
}
