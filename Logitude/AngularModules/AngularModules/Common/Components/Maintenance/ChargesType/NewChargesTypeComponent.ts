import {Component} from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {CachedDataManager} from '../../../../Infrastructure/Utilities/CachedDataManager';
import {AppTool} from '../../../../Infrastructure/Tools';
import {ChargesTypePM} from '../../../EntityPMs/ChargesTypePM';
import {ChargesTypePMService} from '../../../../Common/Services/StandardPMs/ChargesTypePMService';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {Validator} from '../../../../Infrastructure/Validators/Validator';
import {ChargesGroupListService} from '../../../../Infrastructure/Services/StandardLists/ChargesGroupListService';
import {ObjectsLocator} from '../../../../Infrastructure/Locators/ObjectsLocator';
import {VatTypeList} from '../../../EntityLists/VatTypeList';
import {VatTypeListService} from '../../../Services/StandardLists/VatTypeListService';
import { ApiQueryFilters } from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { ChargesTypePMInitService } from 'Common/EntityPMInitServices/ChargesTypePMInitService';

@Component({
    
    templateUrl: './NewChargesTypeComponent.html',
})

export class NewChargesTypeComponent extends BaseComponent {
    public DataContext: NewChargesTypeComponent = this;
    public ObjectTableName: string = "ChargesType";
    public EntityPM: ChargesTypePM;
    private CurrentSession = SessionLocator.SelectedSession;
    public MeasurementsQueryFilters: ApiQueryFilters;
    public IsChargeTypesRestrictedFeatureToggleOn = false;
    public _chargesTypePMService: ChargesTypePMService = new ChargesTypePMService();
    public AccountingActivated: boolean = SessionLocator.TenantPM.AccountingActivated;
    public ReceivableCreditGLAccountFilterItems: ApiQueryFilters;
    public PayableDebitGLAcountFilterItems: ApiQueryFilters;

    constructor() {
        super();

        this.EntityPM = this._chargesTypePMService.GetNewEntityPM();
        if (this.AccountingActivated) {
            ChargesTypePMInitService.InitValuesForAccounting(this.EntityPM, true);
        }
        else {
            ChargesTypePMInitService.InitValues(this.EntityPM, true);
        }
        this.EntityPM.AddedManually = true;

        if (SessionLocator.TenantPM.TenantVATManagement == false) {
            var myService = new VatTypeListService();
            myService.getAllFromCache().subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {
                    var allVats: VatTypeList[] = myResponse.Result;
                    if (allVats) {
                        var myZEROVat = allVats.filter(f => f.Code == "ZERO")[0];
                        if (myZEROVat) {
                            this.EntityPM.VatTypeId = myZEROVat.Id;
                        }
                    }
                }
            });
        }

        this.BuildQueryFilters(); 
        this.SetUIProperties();
        this.SetUIProperties_DirectionFields();
        this.ReadChargeTypesRestrictedFeatureToggleFeature();

        this.ReceivableCreditGLAccountFilterItems = new ApiQueryFilters();
        this.PayableDebitGLAcountFilterItems = new ApiQueryFilters();
        this.ReceivableCreditGLAccountFilterItems.addAdditionalFilter("ReceivableCreditFilter", "1", null, null, "Equals", true, false, false, "string", false, true);
        this.PayableDebitGLAcountFilterItems.addAdditionalFilter("PayableDebitFilter", "2", null, null, "Equals", true, false, false, "string", false, true);
    }

    ReadChargeTypesRestrictedFeatureToggleFeature() {
        this.IsChargeTypesRestrictedFeatureToggleOn = SessionLocator.FeatureToggles.filter(d => d.ToggleCode == "CTR")[0]
            != null ? true : false;
    }

    private BuildQueryFilters() {
        this.MeasurementsQueryFilters = new ApiQueryFilters();
        this.MeasurementsQueryFilters.addAdditionalFilter("Code", "STFE", null, null, "Exclude", false, false, false, "string", false, true, true);
    }

    public CustomsFieldsIsVisible: boolean = false;
    private SetUIProperties() {
        this.UIProperties.SetRequired("MeasurementId", this.ObjectTableName, AppTool.IsNullOrEmpty(this.MeasurementId));
        this.UIProperties.SetEnabled("DueTypeCode", this.ObjectTableName, this.IsAir);
        this.UIProperties.SetEnabled("IATACodeId", this.ObjectTableName, this.IsAir);
        this.UIProperties.SetEnabled("AWBPrintDescription", this.ObjectTableName, this.IsAir);

        if (ObjectsLocator.CustomsInterfaceSettingPM != null) {
            if (ObjectsLocator.CustomsInterfaceSettingPM.ActivateCustomsManagementInShipments) {
                this.CustomsFieldsIsVisible = true;
            }
        }
    }

    // Properties
    get Code() { return this.EntityPM.Code; }
    set Code(newValue: string) {
        if (this.EntityPM.Code != newValue) {
            this.EntityPM.Code = newValue;
        }
    }

    get EnglishName() { return this.EntityPM.EnglishName; }
    set EnglishName(newValue: string) {
        if (this.EntityPM.EnglishName != newValue) {
            this.EntityPM.EnglishName = newValue;
        }
    }

    get LocalName() { return this.EntityPM.LocalName; }
    set LocalName(newValue: string) {
        if (this.EntityPM.LocalName != newValue) {
            this.EntityPM.LocalName = newValue;
        }
    }

    get ChargesGroupCode() { return this.EntityPM.ChargesGroupCode; }
    set ChargesGroupCode(newValue: string) {
        if (this.EntityPM.ChargesGroupCode != newValue) {
            this.EntityPM.ChargesGroupCode = newValue;
        }
    }

    get ChargesGroupId() { return this.EntityPM.ChargesGroupId; }
    set ChargesGroupId(newValue: string) {
        if (this.EntityPM.ChargesGroupId != newValue) {
            this.EntityPM.ChargesGroupId = newValue;
            if (!AppTool.IsNullOrEmpty(newValue)) {
                var myService: ChargesGroupListService = new ChargesGroupListService();
                myService.getSingleFromCache(this.EntityPM.ChargesGroupId).subscribe((myResponse: ServiceResponse) => {

                    if (!myResponse.HasError && myResponse.Result) {
                        this.ChargesGroupCode = myResponse.Result.Code;
                    }
                });
            }
            else this.ChargesGroupCode = newValue;
            
        }
    }
    
    get MeasurementId() { return this.EntityPM.MeasurementId; }
    set MeasurementId(newValue: string) {
        if (this.EntityPM.MeasurementId != newValue) {
            this.EntityPM.MeasurementId = newValue;
            
            this.SetUIProperties();
        }
    }

    get ContainerMeasurementId() { return this.EntityPM.ContainerMeasurementId; }
    set ContainerMeasurementId(newValue: string) {
        if (this.EntityPM.ContainerMeasurementId != newValue) {
            this.EntityPM.ContainerMeasurementId = newValue;
        }
    }

    get IsAir() { return this.EntityPM.IsAir; }
    set IsAir(newValue: boolean) {
        if (this.EntityPM.IsAir != newValue) {
            this.EntityPM.IsAir = newValue;

            this.SetUIProperties();
        }
    }

    get IsInland() { return this.EntityPM.IsInland; }
    set IsInland(newValue: boolean) {
        if (this.EntityPM.IsInland != newValue) {
            this.EntityPM.IsInland = newValue;
        }
    }

    get IsOcean() { return this.EntityPM.IsOcean; }
    set IsOcean(newValue: boolean) {
        if (this.EntityPM.IsOcean != newValue) {
            this.EntityPM.IsOcean = newValue;
        }
    }

    get IsAutoDisplayInQuote() { return this.EntityPM.IsAutoDisplayInQuote; }
    set IsAutoDisplayInQuote(newValue: boolean) {
        if (this.EntityPM.IsAutoDisplayInQuote != newValue) {
            this.EntityPM.IsAutoDisplayInQuote = newValue;
        }
    }

    get IsAutoDisplayInShipment() { return this.EntityPM.IsAutoDisplayInShipment; }
    set IsAutoDisplayInShipment(newValue: boolean) {
        if (this.EntityPM.IsAutoDisplayInShipment != newValue) {
            this.EntityPM.IsAutoDisplayInShipment = newValue;
        }
    }

    get IsAutoDisplayInConsolidation() { return this.EntityPM.IsAutoDisplayInConsolidation; }
    set IsAutoDisplayInConsolidation(newValue: boolean) {
        if (this.EntityPM.IsAutoDisplayInConsolidation != newValue) {
            this.EntityPM.IsAutoDisplayInConsolidation = newValue;
        }
    }

    get IsAutoDisplayInCustoms() { return this.EntityPM.IsAutoDisplayInCustoms; }
    set IsAutoDisplayInCustoms(newValue: boolean) {
        if (this.EntityPM.IsAutoDisplayInCustoms != newValue) {
            this.EntityPM.IsAutoDisplayInCustoms = newValue;
        }
    }

    get IsImport() { return this.EntityPM.IsImport; }
    set IsImport(newValue: boolean) {
        if (this.EntityPM.IsImport != newValue) {
            this.EntityPM.IsImport = newValue;
        }
    }

    get IsExport() { return this.EntityPM.IsExport; }
    set IsExport(newValue: boolean) {
        if (this.EntityPM.IsExport != newValue) {
            this.EntityPM.IsExport = newValue;
        }
    }

    get IsDrop() { return this.EntityPM.IsDrop; }
    set IsDrop(newValue: boolean) {
        if (this.EntityPM.IsDrop != newValue) {
            this.EntityPM.IsDrop = newValue;
        }
    }

    get IsDomestic() { return this.EntityPM.IsDomestic; }
    set IsDomestic(newValue: boolean) {
        if (this.EntityPM.IsDomestic != newValue) {
            this.EntityPM.IsDomestic = newValue;
        }
    }

    get DueTypeCode() { return this.EntityPM.DueTypeCode; }
    set DueTypeCode(newValue: string) {
        if (this.EntityPM.DueTypeCode != newValue) {
            this.EntityPM.DueTypeCode = newValue;
        }
    }

    get IATACodeId() { return this.EntityPM.IATACodeId; }
    set IATACodeId(newValue: string) {
        if (this.EntityPM.IATACodeId != newValue) {
            this.EntityPM.IATACodeId = newValue;
        }
    }

    get AWBPrintDescription() { return this.EntityPM.AWBPrintDescription; }
    set AWBPrintDescription(newValue: boolean) {
        if (this.EntityPM.AWBPrintDescription != newValue) {
            this.EntityPM.AWBPrintDescription = newValue;
        }
    }

    get VatTypeId() { return this.EntityPM.VatTypeId; }
    set VatTypeId(newValue: string) {
        if (this.EntityPM.VatTypeId != newValue) {
            this.EntityPM.VatTypeId = newValue;
        }
    }

    get ViewOrder() { return this.EntityPM.ViewOrder; }
    set ViewOrder(newValue: number) {
        if (this.EntityPM.ViewOrder != newValue) {
            this.EntityPM.ViewOrder = newValue;
        }
    }


    get IsDirectionRestricted() { return this.EntityPM.IsDirectionRestricted; }
    set IsDirectionRestricted(newValue: boolean) {
        if (this.EntityPM.IsDirectionRestricted != newValue) {
            this.EntityPM.IsDirectionRestricted = newValue;
            this.SetUIProperties_DirectionFields();
        }
    }

    SetIsDirectionRestricted(value: boolean) {
        this.IsDirectionRestricted = value;
    }

    private SetUIProperties_DirectionFields() {
        this.UIProperties.SetEnabled("IsActiveInExport", this.ObjectTableName, this.IsDirectionRestricted);
        this.UIProperties.SetEnabled("IsActiveInImport", this.ObjectTableName, this.IsDirectionRestricted);
        this.UIProperties.SetEnabled("IsActiveInDomestic", this.ObjectTableName, this.IsDirectionRestricted);
        this.UIProperties.SetEnabled("IsActiveInDrop", this.ObjectTableName, this.IsDirectionRestricted);
    }

    get IsActiveInDomestic() { return this.EntityPM.IsActiveInDomestic; }
    set IsActiveInDomestic(newValue: boolean) {
        if (this.EntityPM.IsActiveInDomestic != newValue) {
            this.EntityPM.IsActiveInDomestic = newValue;
        }
    }
    get IsActiveInDrop() { return this.EntityPM.IsActiveInDrop; }
    set IsActiveInDrop(newValue: boolean) {
        if (this.EntityPM.IsActiveInDrop != newValue) {
            this.EntityPM.IsActiveInDrop = newValue;
        }
    }
    get IsActiveInExport() { return this.EntityPM.IsActiveInExport; }
    set IsActiveInExport(newValue: boolean) {
        if (this.EntityPM.IsActiveInExport != newValue) {
            this.EntityPM.IsActiveInExport = newValue;
        }
    }
    get IsActiveInImport() { return this.EntityPM.IsActiveInImport; }
    set IsActiveInImport(newValue: boolean) {
        if (this.EntityPM.IsActiveInImport != newValue) {
            this.EntityPM.IsActiveInImport = newValue;
        }
    }

    // Pages Properties
    public Page1Hidden: boolean = false;
    public Page2Hidden: boolean = true;
    public Page3Hidden: boolean = true;

    public IsPreviousEnabled: boolean = false;
    public IsNextEnabled: boolean = true;
    public IsFinishEnabled: boolean = false;

    get PayableDebitAccount() { return this.EntityPM.PayableDebitAccount; }
    set PayableDebitAccount(value: string) {
        if (this.EntityPM.PayableDebitAccount != value) {
            this.EntityPM.PayableDebitAccount = value;
        }
    }

    get PayableDebitGLAcountId() { return this.EntityPM.PayableDebitGLAcountId; }
    set PayableDebitGLAcountId(value: string) {
        if (this.EntityPM.PayableDebitGLAcountId != value) {
            this.EntityPM.PayableDebitGLAcountId = value;
            if (this.EntityPM.PayableDebitGLAcountId == null) this.EntityPM.PayDebitGLAcountLocalName = null;
        }
    }
    
    get ReceivableCreditAccount() { return this.EntityPM.ReceivableCreditAccount; }
    set ReceivableCreditAccount(value: string) {
        if (this.EntityPM.ReceivableCreditAccount != value) {
            this.EntityPM.ReceivableCreditAccount = value;
        }
    }

    get ReceivableCreditGLAccountId() { return this.EntityPM.ReceivableCreditGLAccountId; }
    set ReceivableCreditGLAccountId(value: string) {
        if (this.EntityPM.ReceivableCreditGLAccountId != value) {
            this.EntityPM.ReceivableCreditGLAccountId = value;
            if (this.EntityPM.ReceivableCreditGLAccountId == null) this.EntityPM.RecCreditGLAcountLocalName = null;
        }
    }
    
    // Commands
    PreviousButtonClicked() {
        this.IsFinishEnabled = true;

        if (!this.Page2Hidden) {
            this.IsPreviousEnabled = false;
            this.IsNextEnabled = true;

            this.Page1Hidden = false;
            this.Page2Hidden = true;
            this.Page3Hidden = true;
        }

        else if (!this.Page3Hidden) {
            this.IsPreviousEnabled = true;
            this.IsNextEnabled = true;

            this.Page1Hidden = true;
            this.Page2Hidden = false;
            this.Page3Hidden = true;
        }
    }

    NextButtonClicked() {
        this.IsFinishEnabled = true;

        if (!this.Page1Hidden) {
            this.IsNextEnabled = true;
            this.IsPreviousEnabled = true;

            this.Page1Hidden = true;
            this.Page2Hidden = false;
        }

        else if (!this.Page2Hidden) {
            this.IsPreviousEnabled = true;
            this.IsNextEnabled = false;

            this.Page1Hidden = true;
            this.Page2Hidden = true;
            this.Page3Hidden = false;
        }

        else if (!this.Page3Hidden) {
            this.IsPreviousEnabled = true;
            this.IsNextEnabled = false;
        }
    }

    public ValidationErrorsList: string[];
    FinishButtonClicked() {
        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);        

        if (this.EntityPM.IsAutoDisplayInQuote || this.EntityPM.IsAutoDisplayInShipment || this.EntityPM.IsAutoDisplayInConsolidation || this.EntityPM.IsAutoDisplayInCustoms) {
            if (!this.EntityPM.IsExport && !this.EntityPM.IsImport && !this.EntityPM.IsDomestic && !this.EntityPM.IsDrop) {
                errors.push("Please select at least one direction (export, import, domestic or drop)");
            }
        }

        this.ValidationErrorsList = errors;

        if (this.ValidationErrorsList.length == 0) {
            this.EntityPM.IsReceivable = true;
            this.EntityPM.IsPayable = true;

            this.CurrentSession.StartBusyIndicatorSaving();
            
            this._chargesTypePMService.insert(this.EntityPM).subscribe((myResponse: ServiceResponse) => {

                this.CurrentSession.StopBusyIndicator();

                if (!myResponse.HasError) {
                    CachedDataManager.RefreshTableData(this.ObjectTableName, true);

                    this.CurrentSession.CloseCurrentWindowEmit(this.EntityPM.Id);
                }

                else {
                    this.ValidationErrorsList = myResponse.ErrorsArray;
                }
            });
        }
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
}
