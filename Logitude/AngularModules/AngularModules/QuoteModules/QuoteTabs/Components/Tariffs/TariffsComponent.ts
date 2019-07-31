import {Component} from '@angular/core';
import {AppTool, DateTool, FontTool} from '../../../../Infrastructure/Tools';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {QuotePM} from '../../../../Quote/EntityPMs/QuotePM';
import {QuoteChargePM} from '../../../../Quote/EntityPMs/QuoteChargePM';
import {LCLChargesComponent, QuoteChargeItem} from '../../../QuoteCharges/Components/LCLChargesComponent';
import {TarrifHeaderPM} from '../../../../Common/EntityPMs/TarrifHeaderPM';
import {TarrifChargePM} from '../../../../Common/EntityPMs/TarrifChargePM';
import {PartnersDomainService} from '../../../../Common/Services/PartnersDomainService';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
import {CardList} from '../../../../Common/EntityLists/CardList';
import {CardListService} from '../../../../Common/Services/StandardLists/CardListService';

@Component({
    moduleId: module.id,
    templateUrl: './TariffsComponent.html',
})

export class TariffsComponent {
    public EntityPM: QuotePM;
    private ObjectTableName: string = "TarrifHeader";
    public ItemsSource: TariffsItem[] = [];
    public TarrifCharges: TarrifChargePM[] = [];
    public IsResourcesReady: boolean = false;
    public AllCards: CardList[] = [];
    public myCardListService: CardListService;
    private fatherComponent: LCLChargesComponent;
    private myDomainService: PartnersDomainService;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private entityResourceService: EntityResourceService) {
        this.myCardListService = new CardListService();
        this.myDomainService = new PartnersDomainService();
    }

    SetWindowArgs(args: LCLChargesComponent) {
        this.EntityPM = args.EntityPM;
        this.fatherComponent = args;

        this.entityResourceService.getEntityResourceByTableName("TarrifHeader").subscribe((res: any) => {
            this.entityResourceService.getEntityResourceByTableName("TarrifCharge").subscribe((res2: any) => {
                this.IsResourcesReady = true;

                if (!AppTool.IsNullOrEmpty(this.EntityPM.MainCarriageCarrierId)) {

                    this.CurrentSession.StartBusyIndicatorLoading();

                    this.myCardListService.getSingle(this.EntityPM.MainCarriageCarrierId).subscribe((myResponse: ServiceResponse) => {
                        if (!myResponse.HasError) {
                            var list: CardList = myResponse.Result;
                            if (list) {
                                this.AllCards.push(list);
                            }
                        }

                        this.LoadData();
                    });
                }

                else {
                    this.LoadData();
                }
            });
        });
    }

    private showMyCarrier: boolean = true;
    get ShowMyCarrier() { return this.showMyCarrier; }
    set ShowMyCarrier(value: boolean) {
        if (this.showMyCarrier != value) {
            this.showMyCarrier = value;
            this.showAllCarriers = !value;
            this.LoadData();
        }
    }

    private showAllCarriers: boolean = false;
    get ShowAllCarriers() { return this.showAllCarriers; }
    set ShowAllCarriers(value: boolean) {
        if (this.showAllCarriers != value) {
            this.showAllCarriers = value;
            this.showMyCarrier = !value;
            this.LoadData();
        }
    }

    private selectedItem: TariffsItem = null;
    get SelectedItem() { return this.selectedItem; }
    set SelectedItem(value: TariffsItem) {
        if (this.selectedItem != value) {
            this.selectedItem = value;

            this.TarrifCharges = [];
            if (value) {
                this.TarrifCharges = value.EntityPM.TarrifCharges;
            }
        }
    }

    LoadData() {
        this.ItemsSource = [];
        this.SelectedItem = null;

        if (this.ShowMyCarrier) {
            if (!AppTool.IsNullOrEmpty(this.EntityPM.MainCarriageCarrierId)) {

                this.CurrentSession.StartBusyIndicatorLoading();
                this.myDomainService.GetTarrifHeadersByCardIdAndTypeCode(this.EntityPM.MainCarriageCarrierId, "S", false).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        this.BuildItemsSource(myResponse.Result);
                    }

                    this.CurrentSession.StopBusyIndicator();
                });
            }
        }

        else {
            this.CurrentSession.StartBusyIndicatorLoading();
            this.myDomainService.GetTarrifHeadersByCardIdAndTypeCode(null, "S", false).subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {
                    this.BuildItemsSource(myResponse.Result);
                }

                this.CurrentSession.StopBusyIndicator();
            });
        }
    }

    BuildItemsSource(list: TarrifHeaderPM[]) {
        list.forEach(item => {
            this.ItemsSource.push(new TariffsItem(item, this));
        });

        this.SelectedItem = this.ItemsSource[0];

        if (this.ItemsSource.length == 1) {
            var firstItem: TariffsItem = this.ItemsSource[0];
            if (firstItem != null) {
                firstItem.IsChecked = true;
            }
        }
    }
    
    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
    OkButtonClicked() {
        var itemChecked = this.ItemsSource.filter(f => f.IsChecked == true)[0];
        if (itemChecked != null) {

            var myCharges: TarrifChargePM[] = itemChecked.EntityPM.TarrifCharges;
            myCharges.forEach(item => {

                var chargeItem: QuoteChargeItem = this.fatherComponent.ItemsSource.Collection.filter(d => d.ChargesTypeId == item.ChargesTypeId && d.CostMeasurementId == item.MeasurementId && d.CostCurrencyId == item.CurrencyId)[0];
                if (chargeItem != null) {
                    chargeItem.CostUnitPrice = item.UnitPrice;
                    chargeItem.CostMinAmount = item.MinPrice;
                    chargeItem.CostMaxAmount = item.MaxPrice;
                }

                else {
                    var chargePM = new QuoteChargePM(this.EntityPM);
                    chargePM.Tenant = this.EntityPM.Tenant;
                    chargePM.QuoteId = this.EntityPM.Id;
                    chargePM.UpdatedByUserId = SessionLocator.LoggedUserId;
                    chargePM.MarkUpTypeCode = "F";
                    chargePM.MarkUpValue = 0;
                    chargePM.QuoteTypeCode = this.EntityPM.QuoteTypeCode;
                    chargePM.SaleCurrencyId = this.EntityPM.SaleCurrencyId;
                    chargePM.SaleCurrencyCode = this.fatherComponent.GetCurrencyCode(this.EntityPM.SaleCurrencyId);
                    chargePM.SaleExchangeRate = this.fatherComponent.GetCurrencyRate(this.EntityPM.SaleCurrencyId);                    
                    this.EntityPM.AddQuoteChargePM(chargePM);
                    
                    chargeItem = new QuoteChargeItem(chargePM, this.fatherComponent, false);
                    chargeItem.ChargesTypeId = item.ChargesTypeId;
                    chargeItem.CostMeasurementId = item.MeasurementId;
                    chargeItem.CostCurrencyId = item.CurrencyId;
                    chargeItem.CostUnitPrice = item.UnitPrice;
                    chargeItem.CostMinAmount = item.MinPrice;
                    chargeItem.CostMaxAmount = item.MaxPrice;
                    this.fatherComponent.ItemsSource.Insert(chargeItem);
                }

                chargeItem.ComputeCostAmounts();
                chargeItem.ComputeSalePrice();
            });

            this.fatherComponent.BuildItemsSource();            
            this.CurrentSession.CloseCurrentWindowEmit("OK");
        }
    }
}

class TariffsItem {
    public Id: string;
    public EntityPM: TarrifHeaderPM;

    constructor(entityPM: TarrifHeaderPM, private fatherComponent: TariffsComponent) {
        this.Id = entityPM.Id;
        this.EntityPM = entityPM;
        this.GetCarrier();
        this.GetDateStatus();
    }

    public Carrier: string;
    get CardId() { return this.EntityPM.CardId; }
    GetCarrier() {
        if (this.CardId) {
            var list: CardList = this.fatherComponent.AllCards.filter(f => f.Id == this.CardId)[0];
            if (list) {
                this.Carrier = "(" + list.Code + ") " + list.EnglishName;
            }

            else {
                this.fatherComponent.myCardListService.getSingle(this.CardId).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: CardList = myResponse.Result;
                        if (list) {

                            if (this.fatherComponent.AllCards.filter(f => f.Id == this.CardId).length == 0) {
                                this.fatherComponent.AllCards.push(list);
                            }

                            this.Carrier = "(" + list.Code + ") " + list.EnglishName;
                        }
                    }
                });
            }
        }
    }

    public DateStatus: string;
    public DateStatusColor: string;
    get FromDate() { return this.EntityPM.FromDate; }
    get ToDate() { return this.EntityPM.ToDate; }
    GetDateStatus() {
        if (!AppTool.IsNullOrEmpty(this.FromDate) && !AppTool.IsNullOrEmpty(this.ToDate)) {
            var todayDateTicks = DateTool.GetDateParts(DateTool.GetCurrentDateAsUtc()).DateTicks;
            var fromDateTicks = DateTool.GetDateParts(this.FromDate).DateTicks;
            var toDateTicks = DateTool.GetDateParts(this.ToDate).DateTicks;

            if (todayDateTicks >= fromDateTicks && todayDateTicks <= toDateTicks) {
                this.DateStatus = "(" + TextCodeTranslator.Translate("General.O.Present") + ")";
                this.DateStatusColor = FontTool.Green;
            }

            else if (todayDateTicks < fromDateTicks && todayDateTicks < toDateTicks) {
                this.DateStatus = "(" + TextCodeTranslator.Translate("General.O.Future") + ")";
                this.DateStatusColor = FontTool.Magenta;
            }

            else if (todayDateTicks > fromDateTicks && todayDateTicks > toDateTicks) {
                this.DateStatus = "(" + TextCodeTranslator.Translate("General.O.Past") + ")";
                this.DateStatusColor = FontTool.Red;
            }
        }
    }

    get FromLocationString() { return this.EntityPM.FromLocationString; }
    get ToLocationString() { return this.EntityPM.ToLocationString; }
    get Notes() { return this.EntityPM.Notes; }

    private isChecked: boolean = false;
    get IsChecked() { return this.isChecked; }
    set IsChecked(value: boolean) {
        if (this.isChecked != value) {
            this.isChecked = value;

            if (value) {
                this.fatherComponent.SelectedItem = this;

                this.fatherComponent.ItemsSource.filter(f => f.Id != this.Id).forEach(item => {
                    item.UnCheck();
                });
            }
        }
    }

    public UnCheck() {
        this.isChecked = false;
    }
}
