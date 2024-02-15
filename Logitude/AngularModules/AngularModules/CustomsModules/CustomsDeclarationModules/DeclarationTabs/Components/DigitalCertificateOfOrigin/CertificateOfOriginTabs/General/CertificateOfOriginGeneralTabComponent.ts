import { Component } from '@angular/core';
import { CertificateOfOriginPM } from 'Customs/EntityPMs/CertificateOfOriginPM';
import { ClientPM } from 'Customs/EntityPMs/ClientPM';
import { BaseComponent } from 'Infrastructure/Components/LogitudeComponents/BaseComponent';
import { AppTool, DateTool } from 'Infrastructure/Tools';
import { DeclarationPM } from 'Customs/EntityPMs/DeclarationPM';
import { CertificateOfOriginInvoicePM } from 'Customs/EntityPMs/CertificateOfOriginInvoicePM';
import { ObservableCollection } from 'Infrastructure/Utilities/ObservableCollection';
import { CertificateOfOriginItemPM } from 'Customs/EntityPMs/CertificateOfOriginItemPM';
import { CardListService } from 'Common/Services/StandardLists/CardListService';
import { CardPM } from 'Common/EntityPMs/CardPM';
import { StatusCertificateOfOrigin } from '../../DigitalCertificateOfOriginTabComponent';
import { SupplierInvoicePM } from 'Customs/EntityPMs/SupplierInvoicePM';
import { ExportStorageListService } from 'Customs/Services/StandardLists/ExportStorageListService';
import { TextCodeTranslator } from 'Infrastructure/Utilities/TextCodeTranslator';
import { LogCellTemplateComponent } from 'Infrastructure/Components/LogitudeComponents/EditableLogGridComponent/LogCellTemplateComponent';
import { SupplierInvoiceExtendedPMService } from 'Customs/Services/ExtendedPMs/SupplierInvoiceExtendedPMService';
import { MeasurmentUnitListService } from 'Customs/Services/StandardLists/MeasurmentUnitListService';
import { PackingTypeListService } from 'Customs/Services/StandardLists/PackingTypeListService';
import { ServiceResponse } from 'Infrastructure/DataContracts/ServiceResponse';
import { MeasurmentUnitList } from 'Customs/EntityLists/MeasurmentUnitList';
import { PackingTypeList } from 'Customs/EntityLists/PackingTypeList';
import { ConsignmentPM } from 'Customs/EntityPMs/ConsignmentPM';
import { ExportStorageWebService } from 'Customs/Services/WebServices/ExportStorageWebService';
import { ExportStorageList } from 'Customs/EntityLists/ExportStorageList';
import { OriginCriterionListService } from 'Customs/Services/StandardLists/OriginCriterionListService';
import { OriginCriterionList } from 'Customs/EntityLists/OriginCriterionList';
import { LogitudeWindow } from 'Controls/Windows/LogitudeWindow';
import { EventEmitter } from '@angular/core';
import { ApiQueryFilters } from 'Infrastructure/DataContracts/ApiQueryFilters';
import { CertificateOfOriginMandatoryFieldsListService } from 'Customs/Services/StandardLists/CertificateOfOriginMandatoryFieldsListService';
import { CertificateOfOriginWebService } from 'Customs/Services/WebServices/CertificateOfOriginWebService';
import { CertificateOfOriginMandatoryFieldsList } from 'Customs/EntityLists/CertificateOfOriginMandatoryFieldsList';



@Component({
    styleUrls: ['./CertificateOfOriginGeneralTabComponent.scss'],
    templateUrl: './CertificateOfOriginGeneralTabComponent.html',
})

export class CertificateOfOriginGeneralTabComponent extends BaseComponent {
    public ObjectTableName: string = "Customs.CertificateOfOrigin";
    public DataContext = this;
    public isCorporation: boolean;
    public isCitizen: boolean;
    public isPassport: boolean;
    public entityPM: CertificateOfOriginPM;
    public currentDeclaration: DeclarationPM;
    public currentCard: CardPM;
    public CertificateOriginInvoiceItems: ObservableCollection // type <CertificateOfOriginInvoicePM[]>;
    public CertificateOriginItemItems: ObservableCollection // type <CertificateOfOriginItemPM[]>;
    public IsNewOrEdit: StatusCertificateOfOrigin;
    public IsDisplayMode: boolean = true;
    public IsEditMode: boolean = true;
    public isReady: boolean;
    controlEnabled: boolean;
    IsDisplayOnly: boolean = false;
    public ErrorsList: string[];
    constructor() {
        super();
    }


    supplierInvoiceExtendedPMService: SupplierInvoiceExtendedPMService = new SupplierInvoiceExtendedPMService();
    InitTab(EntityPM: CertificateOfOriginPM, currentDeclaration: DeclarationPM, IsNewOrEdit: StatusCertificateOfOrigin, IsDisplayOnly: boolean) {
        this.entityPM = EntityPM;
        this.IsNewOrEdit = IsNewOrEdit;
        this.IsDisplayOnly = IsDisplayOnly;
        this.CertificateOriginInvoiceItems = new ObservableCollection([]);
        this.CertificateOriginItemItems = new ObservableCollection([]);
        this.currentDeclaration = currentDeclaration;

        if (IsNewOrEdit === StatusCertificateOfOrigin.IsNew) {
            this.supplierInvoiceExtendedPMService.GetSupplierInvoicesPMsForDeclaration(currentDeclaration.Id).subscribe((response: any) => {
                var result = response.Result;
                if (!AppTool.IsNullOrEmpty(result)) {
                    var cardListService = new CardListService();
                    cardListService.getSingleFromCache(currentDeclaration.CustomerId).subscribe((myResponse: any) => {
                        if (!myResponse.HasError) {
                            this.currentCard = myResponse.Result;
                            currentDeclaration.SupplierInvoices = result;
                            this.currentDeclaration.SupplierInvoices = result;
                            this.InitNewCertificate(EntityPM);
                            this.isReady = true;
                        }
                    });
                }
            });
        }
        else if (IsNewOrEdit === StatusCertificateOfOrigin.IsEdit) {
            this.InitilizeListsFromCertificateOfOrigin(EntityPM);
            this.isReady = true;
        }

        this.SetPropertiesEnabled();
        this.SetWarning();
        this.SetWarningByCooTypeCode(EntityPM.CooTypeCode);
        this.controlEnabled = StatusCertificateOfOrigin.IsNew ? true : false;
    }

    InitNewCertificate(EntityPM: CertificateOfOriginPM) {
        // this.entityPM.ExporterName = !AppTool.IsNullOrEmpty(this.currentCard.EnglishName) ? this.currentCard.EnglishName : this.currentCard.LocalName;
        this.entityPM.ExporterName = !AppTool.IsNullOrEmpty(this.currentCard.EnglishName) ? this.currentCard.EnglishName : "";
        this.entityPM.ExporterAddress = `${this.currentCard.Address1 ? this.currentCard.Address1 + " ," : ""}${this.currentCard.Address2 ? this.currentCard.Address2 : ""}`;
        this.InitializeRelatedDeclarationData();
        this.InitilizeNewCertificateWithSupplierInvoicesAndConsignments(EntityPM);
    }

    InitilizeNewCertificateWithSupplierInvoicesAndConsignments(EntityPM: CertificateOfOriginPM) {
        // SupplierInvoices for CertificateOriginInvoiceItems:
        this.CertificateOriginInvoiceItems.Clear();
        this.entityPM.CertificateOriginInvoiceItems = [];
        this.currentDeclaration.SupplierInvoices.forEach((supplierInvoice) => {
            const mappedInvoice = new CertificateOfOriginInvoicePM(EntityPM);
            mappedInvoice.Tenant = this.entityPM.Tenant;
            mappedInvoice.InvoicesIdUry = supplierInvoice.SequenceNumeric;
            mappedInvoice.InvoiceNumber = supplierInvoice.InvoiceNumber;
            mappedInvoice.InvoiceDate = supplierInvoice.IssueDate;
            mappedInvoice.InvoiceSum = supplierInvoice.InvoiceAmount?.toString();
            mappedInvoice.CurrencyTypeCode = supplierInvoice.InvoiceCurrencyTypeCode;
            mappedInvoice.DescriptionOfInvoice = "";
            mappedInvoice.IsInvoicesForPrint = true;

            // add to collection
            this.CertificateOriginInvoiceItems.Insert(new CertificateOfOriginInvoiceLine(mappedInvoice, this));
            this.entityPM.CertificateOriginInvoiceItems.push(mappedInvoice);

        });

        // Consignments for CertificateOriginItemItems:
        this.CertificateOriginItemItems.Clear();
        this.entityPM.CertificateOriginItemItems = [];
        this.currentDeclaration.Consignments.forEach((consignment) => {
            const mappedConsignments = new CertificateOfOriginItemPM(EntityPM);
            mappedConsignments.Tenant = this.entityPM.Tenant;

            mappedConsignments.ItemSerial = consignment.SequenceNumeric;
            const consignmentPackage = consignment.ConsignmentPackages[0];
            if (consignmentPackage) {

                mappedConsignments.MarksAndNumbers = consignmentPackage.MarksNumbers;
                mappedConsignments.PackageQuantity = consignmentPackage.PackageQuantity;
                mappedConsignments.Weight = consignmentPackage.GrossMassMeasure;
                mappedConsignments.MeasureType = consignmentPackage.GrossMassMeasureTypeCode;
                mappedConsignments.PackageType = !AppTool.IsNullOrEmpty(consignmentPackage.PackageTypeCode) ? consignmentPackage.PackageTypeCode : "";
                mappedConsignments.PackingTypeName = consignmentPackage.PackageTypeName ? consignmentPackage.PackageTypeName : "";
                mappedConsignments.MeasureTypeName = consignmentPackage.GrossMassMeasureTypeName;
            }
            mappedConsignments.ItemDescription = consignment.CargoDescription;
            mappedConsignments.ItemId = this.currentDeclaration.SupplierInvoices[0]?.SupplierInvoiceItems[0]?.ClassificationCode.substring(0, 6);

            // #101498 after this task is finish- add this field initilize - field ContainerTypeWCO
            this.getContainerTypeWCOData(consignment, mappedConsignments);

            // add to collection    
            this.CertificateOriginItemItems.Insert(new CertificateOfOriginItemLine(mappedConsignments, this));
            this.entityPM.CertificateOriginItemItems.push(mappedConsignments);

        });
    }

    exportStorageWebService = new ExportStorageWebService();
    getContainerTypeWCOData(consignment: ConsignmentPM, mappedConsignments: CertificateOfOriginItemPM) {
        // ContainerTypeWCO
        // find by: CARGOTYPECODE,FIRSTCARGOID,SECONDCARGOID, THIRDCARGOID
        this.exportStorageWebService.GetByCargoKeys(consignment.ManifestNumber, consignment.SecondCargoID, consignment.ThirdCargoID, consignment.CargoTypeCode, this.entityPM.Tenant).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                var result: ExportStorageList = myResponse.Result;
                if (result != null) {
                    mappedConsignments.ContainerIsoCode = result.ContainerTypeWCO;
                }
            }
        });
    }

    InitilizeListsFromCertificateOfOrigin(EntityPM: CertificateOfOriginPM) {
        this.CertificateOriginInvoiceItems.Clear();
        this.CertificateOriginItemItems.Clear();

        // update CertificateOriginInvoice list:
        EntityPM.CertificateOriginInvoiceItems.forEach((item) => {
            this.CertificateOriginInvoiceItems.Insert(new CertificateOfOriginInvoiceLine(item, this));
        });

        // update CertificateOriginItemItems list:
        EntityPM.CertificateOriginItemItems.forEach((item) => {
            this.getMeasureNameFromCache(item.MeasureType, item);
            this.getPackageTypeNameFromCache(item.PackageType, item);
            this.getOriginCriterionCodeNameFromCache(item.OriginCriterionCode, true, item);
            this.CertificateOriginItemItems.Insert(new CertificateOfOriginItemLine(item, this));
        });
    }

    updateEntity(EntityPM: CertificateOfOriginPM) {
        this.entityPM = EntityPM;
        this.InitilizeListsFromCertificateOfOrigin(EntityPM);
    }

    private measurmentUnitListService: MeasurmentUnitListService = new MeasurmentUnitListService();
    private packingTypeListService: PackingTypeListService = new PackingTypeListService();
    private originCriterionListService: OriginCriterionListService = new OriginCriterionListService();

    getMeasureNameFromCache(code, item) {
        this.measurmentUnitListService.getSingleFromCache(code).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                var result: MeasurmentUnitList = myResponse.Result;
                if (result != null) {
                    item.MeasureTypeName = result.LocalName;
                }
            }
        });
    }
    getPackageTypeNameFromCache(code, item) {
        this.packingTypeListService.getSingleFromCache(code).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                var result: PackingTypeList = myResponse.Result;
                if (result != null) {
                    item.PackingTypeName = result.LocalName;
                }
            }
        });
    }
    CriterionTypesFilterItems: ApiQueryFilters;
    getOriginCriterionCodeNameFromCache(code, isInitField, item = null) {
        this.originCriterionListService.getSingle(code).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                var result: OriginCriterionList = myResponse.Result;
                if (result != null) {
                    if (isInitField && item)
                        item.OriginCriterionCodeName = result.OriginCriterionCode;
                }
            }
            // filter data:
            this.CriterionTypesFilterItems = new ApiQueryFilters();
            this.CriterionTypesFilterItems.addAdditionalFilter("CertificateOfOriginTypeCodeID", this.entityPM.CooTypeCode, null, null, "Equals", false, false, false, "number");
        });
    }
    SetPropertiesEnabled() {
        var enabled = !this.IsDisplayOnly;
        this.UIProperties.SetEnabled("CooTypeCode", this.ObjectTableName, enabled);
        this.UIProperties.SetEnabled("RequestReasonCode", this.ObjectTableName, enabled);
        this.UIProperties.SetEnabled("DateOfDeclaration", this.ObjectTableName, enabled);
        this.UIProperties.SetEnabled("ExporterName", this.ObjectTableName, enabled);
        this.UIProperties.SetEnabled("ConsigneeName", this.ObjectTableName, enabled);
        this.UIProperties.SetEnabled("OriginCountry", this.ObjectTableName, enabled);
        this.UIProperties.SetEnabled("ExporterAddress", this.ObjectTableName, enabled);
        this.UIProperties.SetEnabled("ConsigneeAddress", this.ObjectTableName, enabled);
        this.UIProperties.SetEnabled("OriginGroupOfCountry", this.ObjectTableName, enabled);
        this.UIProperties.SetEnabled("DestinationCountry", this.ObjectTableName, enabled);
        this.UIProperties.SetEnabled("ExporterCountry", this.ObjectTableName, enabled);
        this.UIProperties.SetEnabled("ConsigneeCountry", this.ObjectTableName, enabled);
        this.UIProperties.SetEnabled("DestinationGroupOfCountries", this.ObjectTableName, enabled);
        this.UIProperties.SetEnabled("PlaceOfManufacture", this.ObjectTableName, enabled);
        this.UIProperties.SetEnabled("ZipCodeOfManufacture", this.ObjectTableName, enabled);
        this.UIProperties.SetEnabled("TradeAgreementCountry1", this.ObjectTableName, enabled);
        this.UIProperties.SetEnabled("TradeAgreementCountry2", this.ObjectTableName, enabled);
        this.UIProperties.SetEnabled("IsUnitedInvoices", this.ObjectTableName, enabled);
        this.UIProperties.SetEnabled("TradeAgreementGroupOfCountries", this.ObjectTableName, enabled);
        this.UIProperties.SetEnabled("IsInvoicesForPrint", "Customs.CertificateOriginInvoice", enabled);
    }

    SetWarning() {
        this.UIProperties.SetWarning("CooTypeCode", this.ObjectTableName, true);
        this.UIProperties.SetWarning("RequestReasonCode", this.ObjectTableName, true);
    }

    mandatoryFielsList = [];
    certificateOfOriginWebService: CertificateOfOriginWebService = new CertificateOfOriginWebService();
    SetWarningByCooTypeCode(CooTypeCode) {
        this.certificateOfOriginWebService.GetMandatoryFieldsByCooTypeCode(CooTypeCode, this.entityPM.Tenant).subscribe((myResponse: any) => {
            if (!myResponse.HasError) {
                const certificateOfOriginMandatoryFieldsList = myResponse?.Result;
                if (certificateOfOriginMandatoryFieldsList.length <= 0) return;
                certificateOfOriginMandatoryFieldsList.forEach(item => {
                    if (item.IsMandatory) {
                        this.mandatoryFielsList.push(item.MandatoryFieldName)
                        this.UIProperties.SetWarning(item.MandatoryFieldName, this.ObjectTableName, true);// CHANGE TO MandatoryFieldName
                    }

                });
            }
        });
    }

    private getCardById(id: string) {
        var cardListService = new CardListService();
        cardListService.getSingleFromCache(id).subscribe((myResponse: any) => {
            if (!myResponse.HasError) {
                this.currentCard = myResponse.Result;
                this.entityPM.ExporterName = !AppTool.IsNullOrEmpty(this.currentCard.EnglishName) ? this.currentCard.EnglishName : "";
                this.entityPM.ExporterAddress = `${this.currentCard.Address1 ? this.currentCard.Address1 + " ," : ""}${this.currentCard.Address2 ? this.currentCard.Address2 : ""}`;
            }
        });
    }

    InitializeRelatedDeclarationData() {
        this.getCardById(this.currentDeclaration.CustomerId);

        if (this.currentDeclaration.DeclarationExportRecipients.length > 0) {
            var fieldVal = this.currentDeclaration.DeclarationExportRecipients[0]?.RecipientName; // first from list
            if (fieldVal) {
                this.entityPM.ConsigneeName = fieldVal;
            }
        }

        this.entityPM.DestinationCountry = !AppTool.IsNullOrEmpty(this.currentDeclaration.DestinationCountryCode) ? this.currentDeclaration.DestinationCountryCode : "";

        if (this.currentDeclaration.SupplierInvoices.length > 0) {
            let supplierInvoices = this.currentDeclaration.SupplierInvoices[0];
            this.entityPM.ConsigneeAddress = !AppTool.IsNullOrEmpty(supplierInvoices.BuyerAddress) ? supplierInvoices.BuyerAddress : "";
            this.entityPM.ConsigneeCountry = !AppTool.IsNullOrEmpty(supplierInvoices.BuyerCountryCode) ? supplierInvoices.BuyerCountryCode : "";

            if (supplierInvoices.SupplierInvoiceItems.length > 0) {
                var fieldVal = supplierInvoices?.SupplierInvoiceItems[0]?.OriginCountryCode; // the first invoice from list
                if (fieldVal) {
                    this.entityPM.OriginCountry = fieldVal; // the first invoice item from list
                }
            }
        }

    }

    public selectedCertificateOfOriginItem: CertificateOfOriginItemPM;
    ShowSelectionComponent(currentDeclaration: DeclarationPM, customParam: boolean, isEntityDisplayOnly: boolean, item: CertificateOfOriginItemPM) {
        this.selectedCertificateOfOriginItem = item;
        var selectInvoicesOnly = customParam;
        var windowArgs: any = {};
        windowArgs.DeclarationPM = currentDeclaration;
        windowArgs.selectInvoicesOnly = selectInvoicesOnly;

        windowArgs.existInvoices = item.InvoiceConnect;

        windowArgs.CertificateOfOriginItem = item;
        windowArgs.IsEntityDisplayOnly = isEntityDisplayOnly;

        var logWindow = new LogitudeWindow();
        logWindow.Height = 700;
        logWindow.Width = 1000;
        logWindow.ShowCloseButton = true;
        logWindow.WindowArgs = windowArgs;
        logWindow.ComponentLoaded.subscribe(comp => {
            logWindow.WindowClosed.subscribe(ok => {
                if (ok) {
                    this.SelectionInvoicesCompleted(comp);
                }
            });
        });
        logWindow.Show('./CustomsModules/CustomsDeclarationModules/DeclarationOthers/Components/Documents/PointersFromInvoicesSelectionComponent');
    }

    public SelectionCompleted: EventEmitter<any> = new EventEmitter();
    SelectionInvoicesCompleted(args) {
        if (args.SelectedInvoices != null) {
            this.selectedCertificateOfOriginItem.InvoiceConnect = args.ConnectedInvoices;
            args.SelectedInvoices.Collection.forEach((invoice) => {
                var hasLineschosen = args.StaticSelectedInvoiceItems.Collection.filter(d => d.DeclarationId == invoice.DeclarationId && d.CounterKey == invoice.InvoiceCounterKey)[0];
                if (!hasLineschosen) {
                    // do somthing?
                }

            });
        }
        this.SelectionCompleted.emit(args);
    }

    OnChanged($event, item) {

        this.ErrorsList = [];
        var prevIsInvoicesForPrint = item.IsInvoicesForPrint;
        item.IsInvoicesForPrint = !item.IsInvoicesForPrint;

        var InvoicesForPrintList = this.CertificateOriginInvoiceItems.Collection.filter(x => x.IsInvoicesForPrint);

        if (this.IsUnitedInvoices && InvoicesForPrintList.length < 2) {
            this.IsUnitedInvoices = false;
        }
        else if (this.IsUnitedInvoices && InvoicesForPrintList.find(x => x.CurrencyTypeCode != item.CurrencyTypeCode)) {
            item.IsInvoicesForPrint = prevIsInvoicesForPrint == false ? null : false;
            this.ErrorsList = [TextCodeTranslator.Translate('Customs.CertificateOfOrigin.O.DifferentNotUnited')];

        }

    }



    // Edit Mode region:
    onCellSelected($event, logcelltemplate: LogCellTemplateComponent, Item: any) {
        logcelltemplate.IsDisplayMode = false;
        logcelltemplate.IsEditMode = true;
    }

    // SetLocalName(entity, fieldName, item) {
    //     if (!AppTool.IsNullOrEmpty(entity)) {
    //         if (fieldName == "OriginCriterionCodeName") {
    //             this.CertificateOriginItemItems.Collection.filter(x => x.ItemSerial == item.ItemSerial)[0][fieldName] = entity.OriginCriterionCode;
    //             return;
    //         }
    //         this.CertificateOriginItemItems.Collection.filter(x => x.ItemSerial == item.ItemSerial)[0][fieldName] = entity.LocalName;
    //     } else {
    //         this[fieldName] = null;
    //     }

    // }

    CheckMandatoryFields() {
        if (!this.entityPM.CooTypeCode && !this.entityPM.RequestReasonCode) {
            this.ErrorsList = [TextCodeTranslator.Translate('Customs.CertificateOfOrigin.O.MandatoryFields')];
            // this.ErrorsList = ["סוג תעודת מקור וסיבת בקשה הם שדות חובה"];
        }
        else if (!this.entityPM.CooTypeCode) {
            this.ErrorsList = [TextCodeTranslator.Translate('Customs.CertificateOfOrigin.O.TypeCodeMandatory')];
            // this.ErrorsList = ["סוג תעודת מקור הוא שדה חובה"];
        }
        else if (!this.entityPM.RequestReasonCode) {
            this.ErrorsList = [TextCodeTranslator.Translate('Customs.CertificateOfOrigin.O.RequestReasonMandatory')];
            // this.ErrorsList = ["סיבת בקשה הוא שדה חובה"];
        }
        else {
            this.ErrorsList = [];
        }
    }

    CheckMandatoryCustomsFields(ValidationErrors = []) {
        // debugger
        // // check:
        // let someName = "OriginCountry"
        // let field = this.entityPM[someName];
        // if (!field) {
        //     ValidationErrors.push(someName);
        // }
        this.mandatoryFielsList.forEach(item => {
            if(item){
                let field = this.entityPM[item];
                if (!field)
                    ValidationErrors.push(item);
            }
        });
    }


    //#region  CertificateOfOrigin properties
    public get Id(): string {
        return this.entityPM.Id;
    }
    public set Id(newValue: string) {
        this.entityPM.Id = newValue;
    }

    public get Tenant(): number {
        return this.entityPM.Tenant;
    }
    public set Tenant(newValue: number) {
        this.entityPM.Tenant = newValue;
    }

    public get SearchFields(): string {
        return this.entityPM.SearchFields;
    }
    public set SearchFields(newValue: string) {
        this.entityPM.SearchFields = newValue;
    }

    public get Counter(): string {
        return this.entityPM.Counter;
    }
    public set Counter(newValue: string) {
        this.entityPM.Counter = newValue;
    }

    public get CooTypeCode(): string {
        return this.entityPM.CooTypeCode;
    }
    public set CooTypeCode(newValue: string) {

        this.entityPM.CooTypeCode = newValue;
        if (this.ErrorsList?.length > 0 || this.IsNewOrEdit === StatusCertificateOfOrigin.IsEdit) {
            this.CheckMandatoryFields();
        }
        if (this.entityPM.CooTypeCode) {
            this.entityPM.CertificateOriginItemItems.forEach(item => {
                this.getOriginCriterionCodeNameFromCache(item.OriginCriterionCode, false, item);
            });
            this.SetWarningByCooTypeCode(this.entityPM.CooTypeCode);
        }
    }

    public get RequestReasonCode(): string {
        return this.entityPM.RequestReasonCode;
    }
    public set RequestReasonCode(newValue: string) {
        this.entityPM.RequestReasonCode = newValue;
        if (this.ErrorsList?.length > 0 || this.IsNewOrEdit === StatusCertificateOfOrigin.IsEdit) {
            this.CheckMandatoryFields();
        }
    }

    public get COONumber(): string {
        return this.entityPM.COONumber;
    }
    public set COONumber(newValue: string) {
        this.entityPM.COONumber = newValue;
    }

    public get COONumberToCancel(): string {
        return this.entityPM.COONumberToCancel;
    }
    public set COONumberToCancel(newValue: string) {
        this.entityPM.COONumberToCancel = newValue;
    }

    public get ReplacementReason(): string {
        return this.entityPM.ReplacementReason;
    }
    public set ReplacementReason(newValue: string) {
        this.entityPM.ReplacementReason = newValue;
    }

    public get DeclarationId(): string {
        return this.entityPM.DeclarationId;
    }
    public set DeclarationId(newValue: string) {
        this.entityPM.DeclarationId = newValue;
    }

    public get ExporterVat(): string {
        return this.entityPM.ExporterVat;
    }
    public set ExporterVat(newValue: string) {
        this.entityPM.ExporterVat = newValue;
    }


    public get ExporterName(): string {
        return this.entityPM.ExporterName;

    }
    public set ExporterName(newValue: string) {
        this.entityPM.ExporterName = newValue;
    }

    public get ExporterAddress(): string {
        return this.entityPM.ExporterAddress;
    }
    public set ExporterAddress(newValue: string) {
        this.entityPM.ExporterAddress = newValue;
    }

    public get ExporterCountry(): string {
        if (!this.entityPM.ExporterCountry) {
            const countryCode = "IL";
            this.entityPM.ExporterCountry = countryCode;
            return this.entityPM.ExporterCountry;
        }
        return this.entityPM.ExporterCountry;
    }
    public set ExporterCountry(newValue: string) {
        this.entityPM.ExporterCountry = newValue;
    }

    public get TradeAgreementCountry1(): string {
        return this.entityPM.TradeAgreementCountry1;
    }
    public set TradeAgreementCountry1(newValue: string) {
        this.entityPM.TradeAgreementCountry1 = newValue;
    }

    public get TradeAgreementCountry2(): string {
        return this.entityPM.TradeAgreementCountry2;
    }
    public set TradeAgreementCountry2(newValue: string) {
        this.entityPM.TradeAgreementCountry2 = newValue;
    }

    public get TradeAgreementGroupOfCountries(): string {
        return this.entityPM.TradeAgreementGroupOfCountries;
    }

    public set TradeAgreementGroupOfCountries(newValue: string) {
        this.entityPM.TradeAgreementGroupOfCountries = newValue;
    }

    public get ConsigneeName(): string {
        return this.entityPM.ConsigneeName;
    }
    public set ConsigneeName(newValue: string) {
        this.entityPM.ConsigneeName = newValue;
    }

    public get ConsigneeAddress(): string {
        return this.entityPM.ConsigneeAddress;
    }
    public set ConsigneeAddress(newValue: string) {
        this.entityPM.ConsigneeAddress = newValue;
    }

    public get ConsigneeCountry(): string {
        return this.entityPM.ConsigneeCountry;
    }
    public set ConsigneeCountry(newValue: string) {
        this.entityPM.ConsigneeCountry = newValue;
    }

    public get ConsigneeRemarks(): string {
        return this.entityPM.ConsigneeRemarks;
    }
    public set ConsigneeRemarks(newValue: string) {
        this.entityPM.ConsigneeRemarks = newValue;
    }

    public get IsConsigneeForPrint(): boolean {
        return this.entityPM.IsConsigneeForPrint;
    }
    public set IsConsigneeForPrint(newValue: boolean) {
        this.entityPM.IsConsigneeForPrint = newValue;
    }

    public get OriginCountry(): string {
        return this.entityPM.OriginCountry;
    }
    public set OriginCountry(newValue: string) {
        this.entityPM.OriginCountry = newValue;
    }

    public get OriginGroupOfCountry(): string {
        return this.entityPM.OriginGroupOfCountry;
    }
    public set OriginGroupOfCountry(newValue: string) {
        this.entityPM.OriginGroupOfCountry = newValue;
    }

    public get DestinationCountry(): string {
        return this.entityPM.DestinationCountry;
    }
    public set DestinationCountry(newValue: string) {
        this.entityPM.DestinationCountry = newValue;
    }

    public get DestinationGroupOfCountries(): string {
        return this.entityPM.DestinationGroupOfCountries;
    }
    public set DestinationGroupOfCountries(newValue: string) {
        this.entityPM.DestinationGroupOfCountries = newValue;
    }

    public get Transport(): string {
        return this.entityPM.Transport;
    }
    public set Transport(newValue: string) {
        this.entityPM.Transport = newValue;
    }

    public get PortOfShipment(): string {
        return this.entityPM.PortOfShipment;
    }
    public set PortOfShipment(newValue: string) {
        this.entityPM.PortOfShipment = newValue;
    }

    public get IsCumulation(): boolean {
        return this.entityPM.IsCumulation;
    }
    public set IsCumulation(newValue: boolean) {
        this.entityPM.IsCumulation = newValue;
    }

    public get CumulationCountry(): string {
        return this.entityPM.CumulationCountry;
    }
    public set CumulationCountry(newValue: string) {
        this.entityPM.CumulationCountry = newValue;
    }

    public get CumulationGroupOfCountries(): string {
        return this.entityPM.CumulationGroupOfCountries;
    }
    public set CumulationGroupOfCountries(newValue: string) {
        this.entityPM.CumulationGroupOfCountries = newValue;
    }


    public get PlaceOfManufacture(): string {
        return this.entityPM.PlaceOfManufacture;
    }
    public set PlaceOfManufacture(newValue: string) {
        this.entityPM.PlaceOfManufacture = newValue;
    }

    public get ZipCodeOfManufacture(): string {
        return this.entityPM.ZipCodeOfManufacture;
    }
    public set ZipCodeOfManufacture(newValue: string) {
        this.entityPM.ZipCodeOfManufacture = newValue;
    }

    public get Observations(): string {
        return this.entityPM.Observations;
    }
    public set Observations(newValue: string) {
        this.entityPM.Observations = newValue;
    }

    public get IsExportDecForPrint(): boolean {
        return this.entityPM.IsExportDecForPrint;
    }
    public set IsExportDecForPrint(newValue: boolean) {
        this.entityPM.IsExportDecForPrint = newValue;
    }

    public get IsUnitedInvoices(): boolean {
        return this.entityPM.IsUnitedInvoices;
    }
    public set IsUnitedInvoices(newValue: boolean) {
        this.ErrorsList = [];

        if (newValue)
            this.ValidateIsUnitedInvoices();

        if (this.ErrorsList.length > 0)
            this.entityPM.IsUnitedInvoices = this.entityPM.IsUnitedInvoices == false ? null : false;
        else
            this.entityPM.IsUnitedInvoices = newValue;
    }
    ValidateIsUnitedInvoices() {
        var InvoicesForPrintList = this.CertificateOriginInvoiceItems.Collection.filter(x => x.IsInvoicesForPrint);

        if (InvoicesForPrintList.length < 2)
            this.ErrorsList = [TextCodeTranslator.Translate('Customs.CertificateOfOrigin.O.OneNotUnited')];
        else if (InvoicesForPrintList.find(x => x.CurrencyTypeCode != InvoicesForPrintList[0].CurrencyTypeCode))
            this.ErrorsList = [TextCodeTranslator.Translate('Customs.CertificateOfOrigin.O.DifferentNotUnited')];


    }
    public get CustomsHouse(): string {
        return this.entityPM.CustomsHouse;
    }
    public set CustomsHouse(newValue: string) {
        this.entityPM.CustomsHouse = newValue;
    }

    public get IssuingCountry(): string {
        return this.entityPM.IssuingCountry;
    }
    public set IssuingCountry(newValue: string) {
        this.entityPM.IssuingCountry = newValue;
    }

    public get CityOfDeclaration(): string {
        return this.entityPM.CityOfDeclaration;
    }
    public set CityOfDeclaration(newValue: string) {
        this.entityPM.CityOfDeclaration = newValue;
    }

    public get CountryOfDeclaration(): string {
        return this.entityPM.CountryOfDeclaration;
    }
    public set CountryOfDeclaration(newValue: string) {
        this.entityPM.CountryOfDeclaration = newValue;
    }

    public get DateOfDeclaration(): Date {

        if (!this.entityPM.DateOfDeclaration) {
            var todayDate = DateTool.GetCurrentDateAsUtc();
            this.entityPM.DateOfDeclaration = todayDate;
            return this.entityPM.DateOfDeclaration;
        }
        return this.entityPM.DateOfDeclaration;
    }
    public set DateOfDeclaration(newValue: Date) {
        this.entityPM.DateOfDeclaration = newValue;
    }

    public get IsDeclaredByManufacture(): boolean {
        return this.entityPM.IsDeclaredByManufacture;
    }
    public set IsDeclaredByManufacture(newValue: boolean) {
        this.entityPM.IsDeclaredByManufacture = newValue;
    }

    public get IsDeclaredByExporter(): boolean {
        return this.entityPM.IsDeclaredByExporter;
    }
    public set IsDeclaredByExporter(newValue: boolean) {
        this.entityPM.IsDeclaredByExporter = newValue;
    }

    public get IsAttachedList(): boolean {
        return this.entityPM.IsAttachedList;
    }
    public set IsAttachedList(newValue: boolean) {
        this.entityPM.IsAttachedList = newValue;
    }

    public get InsufficentWorkingInd(): boolean {
        return this.entityPM.InsufficentWorkingInd;
    }
    public set InsufficentWorkingInd(newValue: boolean) {
        this.entityPM.InsufficentWorkingInd = newValue;
    }

    public get InsufficentWorkingText(): string {
        return this.entityPM.InsufficentWorkingText;
    }
    public set InsufficentWorkingText(newValue: string) {
        this.entityPM.InsufficentWorkingText = newValue;
    }

    public get NonExportDate(): Date {
        return this.entityPM.NonExportDate;
    }
    public set NonExportDate(newValue: Date) {
        this.entityPM.NonExportDate = newValue;
    }

    public get NonExportCountry(): string {
        return this.entityPM.NonExportCountry;
    }
    public set NonExportCountry(newValue: string) {
        this.entityPM.NonExportCountry = newValue;
    }

    public get NonImportBillOfLadingNum(): string {
        return this.entityPM.NonImportBillOfLadingNum;
    }
    public set NonImportBillOfLadingNum(newValue: string) {
        this.entityPM.NonImportBillOfLadingNum = newValue;
    }

    public get NonExportPort(): string {
        return this.entityPM.NonExportPort;
    }
    public set NonExportPort(newValue: string) {
        this.entityPM.NonExportPort = newValue;
    }

    public get NonImportDate(): Date {
        return this.entityPM.NonImportDate;
    }
    public set NonImportDate(newValue: Date) {
        this.entityPM.NonImportDate = newValue;
    }

    public get NonExportBillOfLadingNum(): string {
        return this.entityPM.NonExportBillOfLadingNum;
    }
    public set NonExportBillOfLadingNum(newValue: string) {
        this.entityPM.NonExportBillOfLadingNum = newValue;
    }

    public get NonTransirCountry(): string {
        return this.entityPM.NonTransirCountry;
    }
    public set NonTransirCountry(newValue: string) {
        this.entityPM.NonTransirCountry = newValue;
    }

    public get NonPortOfEntrance(): string {
        return this.entityPM.NonPortOfEntrance;
    }
    public set NonPortOfEntrance(newValue: string) {
        this.entityPM.NonPortOfEntrance = newValue;
    }

    public get NonExpectedExitDate(): Date {
        return this.entityPM.NonExpectedExitDate;
    }
    public set NonExpectedExitDate(newValue: Date) {
        this.entityPM.NonExpectedExitDate = newValue;
    }

    public get NonExitPort(): string {
        return this.entityPM.NonExitPort;
    }
    public set NonExitPort(newValue: string) {
        this.entityPM.NonExitPort = newValue;
    }

    public get NonGoodsDescription(): string {
        return this.entityPM.NonGoodsDescription;
    }
    public set NonGoodsDescription(newValue: string) {
        this.entityPM.NonGoodsDescription = newValue;
    }

    public get NonDeclaringCompany(): string {
        return this.entityPM.NonDeclaringCompany;
    }
    public set NonDeclaringCompany(newValue: string) {
        this.entityPM.NonDeclaringCompany = newValue;
    }

    public get NonDeclaringPerson(): string {
        return this.entityPM.NonDeclaringPerson;
    }
    public set NonDeclaringPerson(newValue: string) {
        this.entityPM.NonDeclaringPerson = newValue;
    }

    public get NonDeclaringPosition(): string {
        return this.entityPM.NonDeclaringPosition;
    }
    public set NonDeclaringPosition(newValue: string) {
        this.entityPM.NonDeclaringPosition = newValue;
    }

    public get NonManifestNum(): string {
        return this.entityPM.NonManifestNum;
    }
    public set NonManifestNum(newValue: string) {
        this.entityPM.NonManifestNum = newValue;
    }

    public get ErrXml(): string {
        return this.entityPM.ErrXml;
    }
    public set ErrXml(newValue: string) {
        this.entityPM.ErrXml = newValue;
    }

    public get CooStatusCode(): string {
        return this.entityPM.CooStatusCode;
    }
    public set CooStatusCode(newValue: string) {
        this.entityPM.CooStatusCode = newValue;
    }
    public get FeedbackRemark(): string {
        return this.entityPM.FeedbackRemark;
    }
    public set FeedbackRemark(newValue: string) {
        this.entityPM.FeedbackRemark = newValue;
    }

    public get RejectCancelReason(): string {
        return this.entityPM.RejectCancelReason;
    }
    public set RejectCancelReason(newValue: string) {
        this.entityPM.RejectCancelReason = newValue;
    }

    public get IssueDateIfReleased(): Date {
        return this.entityPM.IssueDateIfReleased;
    }
    public set IssueDateIfReleased(newValue: Date) {
        this.entityPM.IssueDateIfReleased = newValue;
    }

    public get QueryUrl(): string {
        return this.entityPM.QueryUrl;
    }
    public set QueryUrl(newValue: string) {
        this.entityPM.QueryUrl = newValue;
    }

    public get CooPdf(): string {
        return this.entityPM.CooPdf;
    }
    public set CooPdf(newValue: string) {
        this.entityPM.CooPdf = newValue;
    }

    public get CoodPdf1(): string {
        return this.entityPM.CoodPdf1;
    }
    public set CoodPdf1(newValue: string) {
        this.entityPM.CoodPdf1 = newValue;
    }

    public get OpenByUser(): string {
        return this.entityPM.OpenByUser;
    }
    public set OpenByUser(newValue: string) {
        this.entityPM.OpenByUser = newValue;
    }

    public get IsSubmitted(): boolean {
        return this.entityPM.IsSubmitted;
    }
    public set IsSubmitted(newValue: boolean) {
        this.entityPM.IsSubmitted = newValue;
    }
    //#endregion CertificateOfOrigin properties

}

export class CertificateOfOriginInvoiceLine extends BaseComponent {
    public entityPM: CertificateOfOriginInvoicePM;
    public ObjectTableName: string = "Customs.CertificateOfOriginInvoice";
    public DataContext = this;
    Parent: CertificateOfOriginGeneralTabComponent;
    constructor(EntityPM: CertificateOfOriginInvoicePM, parent: CertificateOfOriginGeneralTabComponent) {
        super();
        this.entityPM = EntityPM;
        this.Parent = parent;
    }

    public get InvoicesIdUry(): number {
        return this.entityPM.InvoicesIdUry;
    }
    public set InvoicesIdUry(newValue: number) {
        this.entityPM.InvoicesIdUry = newValue;
    }

    public get InvoiceNumber(): string {
        return this.entityPM.InvoiceNumber;
    }
    public set InvoiceNumber(newValue: string) {
        this.entityPM.InvoiceNumber = newValue;
    }

    public get InvoiceDate(): Date {
        return this.entityPM.InvoiceDate;
    }
    public set InvoiceDate(newValue: Date) {
        this.entityPM.InvoiceDate = newValue;
    }

    public get InvoiceSum(): string {
        return this.entityPM.InvoiceSum;
    }
    public set InvoiceSum(newValue: string) {
        this.entityPM.InvoiceSum = newValue;
    }

    public get CurrencyTypeCode(): string {
        return this.entityPM.CurrencyTypeCode;
    }
    public set CurrencyTypeCode(newValue: string) {
        this.entityPM.CurrencyTypeCode = newValue;
    }

    public get DescriptionOfInvoice(): string {
        return this.entityPM.DescriptionOfInvoice;
    }
    public set DescriptionOfInvoice(newValue: string) {
        this.entityPM.DescriptionOfInvoice = newValue;
    }

    public get IsInvoicesForPrint(): boolean {
        return this.entityPM.IsInvoicesForPrint;
    }
    public set IsInvoicesForPrint(newValue: boolean) {
        this.entityPM.IsInvoicesForPrint = newValue;
    }
}


export class CertificateOfOriginItemLine extends BaseComponent {
    public entityPM: CertificateOfOriginItemPM;
    public ObjectTableName: string = "Customs.CertificateOfOriginItem";
    public DataContext = this;
    Parent: CertificateOfOriginGeneralTabComponent;
    constructor(EntityPM: CertificateOfOriginItemPM, parent: CertificateOfOriginGeneralTabComponent) {
        super();
        this.entityPM = EntityPM;
        this.Parent = parent;
    }

    SetLocalName(entity, fieldName, item , CertificateOriginItemItems) {
        if (!AppTool.IsNullOrEmpty(entity)) {
            if (fieldName == "OriginCriterionCodeName") {
                CertificateOriginItemItems.Collection.filter(x => x.ItemSerial == item.ItemSerial)[0][fieldName] = entity.OriginCriterionCode;
                return;
            }
            CertificateOriginItemItems.Collection.filter(x => x.ItemSerial == item.ItemSerial)[0][fieldName] = entity.LocalName;
        } 
        else {
            this[fieldName] = null;
        }
    }
    public get InvoiceConnect(): string {
        return this.entityPM.InvoiceConnect;
    }
    public set InvoiceConnect(newValue: string) {
        this.entityPM.InvoiceConnect = newValue;
    }

    public get ItemSerial(): number {
        return this.entityPM.ItemSerial;
    }
    public set ItemSerial(newValue: number) {
        this.entityPM.ItemSerial = newValue;
    }

    public get ItemId(): string {
        return this.entityPM.ItemId;
    }
    public set ItemId(newValue: string) {
        this.entityPM.ItemId = newValue;
    }

    public get MeasureType(): string {
        return this.entityPM.MeasureType;
    }
    public set MeasureType(newValue: string) {
        this.entityPM.MeasureType = newValue;
    }

    public get MeasureTypeName(): string {
        return this.entityPM.MeasureTypeName;
    }
    public set MeasureTypeName(newValue: string) {
        this.entityPM.MeasureTypeName = newValue;
    }

    public get ContainerIsoCode(): string {
        return this.entityPM.ContainerIsoCode;
    }
    public set ContainerIsoCode(newValue: string) {
        this.entityPM.ContainerIsoCode = newValue;
    }

    public get MarksAndNumbers(): string {
        return this.entityPM.MarksAndNumbers;
    }
    public set MarksAndNumbers(newValue: string) {
        this.entityPM.MarksAndNumbers = newValue;
    }

    public get ItemDescription(): string {
        return this.entityPM.ItemDescription;
    }
    public set ItemDescription(newValue: string) {
        this.entityPM.ItemDescription = newValue;
    }

    public get PackageQuantity(): number {
        return this.entityPM.PackageQuantity;
    }
    public set PackageQuantity(newValue: number) {
        this.entityPM.PackageQuantity = newValue;
    }

    public get PackageType(): string {
        return this.entityPM.PackageType;
    }
    public set PackageType(newValue: string) {
        this.entityPM.PackageType = newValue;
    }

    public get PackingTypeName(): string {
        return this.entityPM.PackingTypeName;
    }
    public set PackingTypeName(newValue: string) {
        this.entityPM.PackingTypeName = newValue;
    }

    public get Weight(): number {
        return this.entityPM.Weight;
    }
    public set Weight(newValue: number) {
        this.entityPM.Weight = newValue;
    }
    public get OriginCriterionCode(): string {
        return this.entityPM.OriginCriterionCode;
    }
    public set OriginCriterionCode(newValue: string) {
        this.entityPM.OriginCriterionCode = newValue;
    }
    public get OriginCriterionCodeName(): string {
        return this.entityPM.OriginCriterionCodeName;
    }
    public set OriginCriterionCodeName(newValue: string) {
        this.entityPM.OriginCriterionCodeName = newValue;
    }
}
