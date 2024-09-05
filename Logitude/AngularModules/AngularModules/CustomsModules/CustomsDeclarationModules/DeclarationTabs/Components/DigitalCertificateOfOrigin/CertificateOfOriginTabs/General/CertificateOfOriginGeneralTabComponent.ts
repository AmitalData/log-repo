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
import { CertificateOfOriginWebService } from 'Customs/Services/WebServices/CertificateOfOriginWebService';
import { GroupByClass } from 'Infrastructure/DataContracts/Dashboard/GroupByClass';
import { QueryFilterItem } from 'Report/Components/Filters/QueryFilterItem';
import { ConfirmWindow } from 'Controls/Windows/ConfirmWindow';
import { DeclarationPMService } from 'Customs/Services/StandardPMs/DeclarationPMService';
import { AmitalGatewayUtil, UnifreightMessageM } from 'Infrastructure/Utilities/AmitalGatewayUtil';
import { SessionLocator } from 'Infrastructure/Utilities/SessionLocator';
import * as xmlbuilder from 'xmlbuilder';
 


class UpdateGeneralArgsParams {
    public UpdateField: string;
    public LookUpTableName: string;
    public IsMultiline: string;
    public QueryFilterItems;
    public ObjectTableName: string;
    public Validate: any;
    public Title: string;
    public IsItemsWithNoValue: boolean;
    public ItemsWithNoValueTitle: string;
    public SelectionCompletedMethod: any;
}

class UpdateGeneralParams {
    public Title: string;
    public Arguments: UpdateGeneralArgsParams;
}
 
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
    myDictionary: Record<string, string> = {};
    public currentDeclaration: DeclarationPM;
    public currentCard: CardPM;
    public CertificateOriginInvoiceItems: ObservableCollection // type <CertificateOfOriginInvoicePM[]>;
    public CertificateOriginItemItems: ObservableCollection // type <CertificateOfOriginItemPM[]>;
    public IsNewOrEdit: StatusCertificateOfOrigin;
    public IsDisplayMode: boolean = true;
    public IsDisplayMessage: boolean = false;
    public DisplayOnlyMessage: string = "";
    public IsEditMode: boolean = true;
    public isReady: boolean;
    controlEnabled: boolean;
    IsDisplayOnly: boolean = false;
     public IsActionButtonsEnabled: boolean = true;
    public originalItemSource: ObservableCollection = new ObservableCollection([]);
    public updateOptionsMap = new Map<string, UpdateGeneralParams>();
    public UpdateOptionParams = {
        ContainerIsoCode: { validate: null },
        ItemId: null,
        OriginCriterionCode: { LookUpTableName: 'Customs.OriginCriterion', ObjectTableName: 'Customs.CertificateOfOriginItem', QueryFilterItems: null },
        ItemDescription: { IsMultiline: true },
    }
     public ErrorsList: string[];
     public StatusCode:string = "4";
    private _declarationPMService: DeclarationPMService = new DeclarationPMService();
 
    constructor() {
        super();
        this.BuildUpdateParams();
    }

    cargoDescription: string = "";
    supplierInvoiceExtendedPMService: SupplierInvoiceExtendedPMService = new SupplierInvoiceExtendedPMService();
    InitTab(EntityPM: CertificateOfOriginPM, currentDeclaration: DeclarationPM, IsNewOrEdit: StatusCertificateOfOrigin, IsDisplayOnly: boolean) {
        this.entityPM = EntityPM;
        this.IsNewOrEdit = IsNewOrEdit;
        this.IsDisplayOnly = IsDisplayOnly;
        this.IsActionButtonsEnabled = !IsDisplayOnly;
        this.CertificateOriginInvoiceItems = new ObservableCollection([]);
        this.CertificateOriginItemItems = new ObservableCollection([]);
        this.currentDeclaration = currentDeclaration;
        this.cargoDescription = this.currentDeclaration.Consignments[0]?.CargoDescription;
        
        if (IsNewOrEdit === StatusCertificateOfOrigin.IsNew) {
           
            this.InitMoreDataScreenValues();
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
        
        this.InitUrls()
        this.SetPropertiesEnabled();
        this.SetWarning();
        this.SetWarningByCooTypeCode(EntityPM.CooTypeCode);
        this.initSelectionValueFields();
        this.controlEnabled = StatusCertificateOfOrigin.IsNew ? true : false;
        this.setDisplayMessage();

        
        
    }
    InitUrls() {
        for (const paramName in this.entityPM) {
            this.myDictionary[paramName]=localStorage.getItem(paramName+"_"+this.entityPM.CooTypeCode+".png");
        }
    }
    
    
    InitMoreDataScreenValues() {
        this.certificateOfOriginWebService.GetCityOfDeclarationByImporterID(this.currentDeclaration.ImporterId, this.currentDeclaration.Tenant).subscribe(myResult => {
            if (!myResult.HasError && myResult.Result != null) {
                this.PlaceOfManufacture = myResult.Result.LocalCityCode;
                this.ZipCodeOfManufacture = myResult.Result.LocalPostalCode;
            }
        });


    }


    setDisplayMessage() {
        if(this.entityPM.UpdateDeclaration == "A"){
            this.DisplayOnlyMessage = TextCodeTranslator.Translate('Customs.CertificateOfOrigin.O.DecNotSubmitted');
            this.IsDisplayMessage = true;
        }
    }
    initSelectionValueFields() {
         this.selectedValueOriginCountry = this.entityPM.OriginGroupOfCountry && !this.entityPM.OriginCountry ? this.fieldNameOriginGroupOfCountry : this.fieldNameOriginCountry;
        this.selectedValueDestinationCountry = this.entityPM.DestinationGroupOfCountries && !this.entityPM.DestinationCountry ? this.fieldNameDestinationGroupOfCountries : this.fieldNameDestinationCountry;
        this.selectedValueTradeAgreement = this.entityPM.TradeAgreementGroupOfCountries && !this.entityPM.TradeAgreementCountry2 ? this.fieldNameTradeAgreementGroupOfCountries : this.fieldNameTradeAgreementCountry2;
    }

    updateSelectedValueChange(selectedValue: string) {
        const mappings = {
            [this.fieldNameDestinationCountry]: () => this.DestinationGroupOfCountries = null,
            [this.fieldNameDestinationGroupOfCountries]: () => this.DestinationCountry = null,
            [this.fieldNameOriginCountry]: () => this.OriginGroupOfCountry = null,
            [this.fieldNameOriginGroupOfCountry]: () => this.OriginCountry = null,
            [this.fieldNameTradeAgreementCountry2]: () => this.TradeAgreementGroupOfCountries = null,
            [this.fieldNameTradeAgreementGroupOfCountries]: () => this.TradeAgreementCountry2 = null,
        };
        if (selectedValue in mappings) mappings[selectedValue]();
    }

    // TradeAgreementGroupOfCountries/TradeAgreementCountry2
    fieldNameTradeAgreementCountry2: string = TextCodeTranslator.Translate('Customs.CertificateOfOrigin.F.TradeAgreementCountry2');
    fieldNameTradeAgreementGroupOfCountries: string = TextCodeTranslator.Translate('Customs.CertificateOfOrigin.F.TradeAgreementGroupOfCountries');
    TradeAgreementSelectionList: string[] = [this.fieldNameTradeAgreementCountry2, this.fieldNameTradeAgreementGroupOfCountries];
    selectedValueTradeAgreement: string = this.fieldNameTradeAgreementCountry2;

    // OriginGroupOfCountry/OriginCountry
    fieldNameOriginCountry: string = TextCodeTranslator.Translate('Customs.CertificateOfOrigin.F.OriginCountry');
    fieldNameOriginGroupOfCountry: string = TextCodeTranslator.Translate('Customs.CertificateOfOrigin.F.OriginGroupOfCountry');
    OriginCountrySelectionList: string[] = [this.fieldNameOriginCountry, this.fieldNameOriginGroupOfCountry];
    selectedValueOriginCountry: string = this.fieldNameOriginCountry;

    // DestinationGroupOfCountries/DestinationCountry
    fieldNameDestinationCountry: string = TextCodeTranslator.Translate('Customs.CertificateOfOrigin.F.DestinationCountry');
    fieldNameDestinationGroupOfCountries: string = TextCodeTranslator.Translate('Customs.CertificateOfOrigin.F.DestinationGroupOfCountries');
    DestinationCountrySelectionList: string[] = [this.fieldNameDestinationCountry, this.fieldNameDestinationGroupOfCountries];
    selectedValueDestinationCountry: string = this.fieldNameDestinationCountry;

    InitNewCertificate(EntityPM: CertificateOfOriginPM) {
        this.entityPM.ExporterName = !AppTool.IsNullOrEmpty(this.currentCard.EnglishName) ? this.currentCard.EnglishName : "";
        this.entityPM.ExporterAddress = `${this.currentCard.Address1 ? this.currentCard.Address1 + " ," : ""}${this.currentCard.Address2 ? this.currentCard.Address2 : ""}`;
        this.InitializeRelatedDeclarationData();
        this.InitilizeNewCertificateWithSupplierInvoices(EntityPM);
        this.InitilizeNewCertificateWithConsignments(EntityPM);
    }

    // SupplierInvoices for CertificateOriginInvoiceItems:
    InitilizeNewCertificateWithSupplierInvoices(EntityPM: CertificateOfOriginPM) {
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
            mappedInvoice.DescriptionOfInvoice = !AppTool.IsNullOrEmpty(this.cargoDescription) ? this.cargoDescription : "";
            mappedInvoice.IsInvoicesForPrint = true;

            // add to collection
            this.CertificateOriginInvoiceItems.Insert(new CertificateOfOriginInvoiceLine(mappedInvoice, this));
            this.entityPM.CertificateOriginInvoiceItems.push(mappedInvoice);
        });
    }

    // Consignments for CertificateOriginItemItems:
    InitilizeNewCertificateWithConsignments(EntityPM: CertificateOfOriginPM) {
        // #108953 -init from unifreight
        if (!this.currentDeclaration.IsConnectedToUnifreight && AmitalGatewayUtil.Instance.AmitalBrowserInUse)
            this.operationalDataFromUnifreight(EntityPM);
        else // init from Declaration.Consignments
            this.initCertificateOriginItemItems(EntityPM);
    }



    operationalDataFromUnifreight(EntityPM: CertificateOfOriginPM) {
        SessionLocator.SelectedSession.StartBusyIndicatorLoading();
        let sub = AmitalGatewayUtil.Instance.UnifaceRequestArrived
            .subscribe(
                (mess: UnifreightMessageM) => {
                    var IsMatchUnifreightCallbackCommand = (
                        mess.LogitudeEntityNumber == this.currentDeclaration.Id &&
                        mess.LogitudeViewModel == "CertificateOfOriginGeneralTabComponent.ts");
                    if (IsMatchUnifreightCallbackCommand) {
                        sub.unsubscribe();
                        SessionLocator.SelectedSession.StopBusyIndicator();
                        let XMLOfConsignmentsDetailsToCertificateOfOriginOut = UnifreightMessageM.GetStringValue(mess, "XMLOfConsignmentsDetailsToCertificateOfOriginOut");
                        const xmlData = (xml: string) => xml.replace(/&lt;/g, '<').replace(/&gt;/g, '>').replace(/&amp;/g, '&');
                        const result = this.parseXml(xmlData(XMLOfConsignmentsDetailsToCertificateOfOriginOut));

                        if (AppTool.IsNullOrEmpty(result) || AppTool.IsNullOrEmpty(result?.certificateOfOriginItems) || result?.certificateOfOriginItems == 0) {
                            this.initCertificateOriginItemItems(EntityPM);
                        }
                        else { 
                            this.initCertificateOriginItemsFromUnifreight(result, EntityPM);
                        }
                    }
                }
            );

        SessionLocator.SelectedSession.StartBusyIndicator("");
        var unifreightMessageM =
            AmitalGatewayUtil.Instance.
                DeclarationMessaging.GetMessage(this.currentDeclaration.CustomFileNo, this.currentDeclaration.Id, "CertificateOfOriginGeneralTabComponent.ts", "BFIFILE");
        unifreightMessageM.Requset.push(["XMLOfConsignmentsDetailsToCertificateOfOrigin", this.buildXmlCertificateOfOriginPM(this.entityPM)]);

        AmitalGatewayUtil.Instance.SendRequestToUnifreightAsync(
            "AmitalGatewayUtil.ConsignmentsDetailsToCertificateOfOrigin",
            "BFIHMAIN.LogitudeTask",
            "ConsignmentsDetailsToCertificateOfOrigin",
            unifreightMessageM,
            "תקשורת של תעודת מקור לקבלת נתוני משגור");
    }

    initCertificateOriginItemsFromUnifreight(result, EntityPM: CertificateOfOriginPM) {
        this.CertificateOriginItemItems.Clear();
        this.originalItemSource.Clear();
        this.entityPM.CertificateOriginItemItems = [];
        result?.certificateOfOriginItems?.forEach((unifreightItem) => {
            const mappedConsignments = new CertificateOfOriginItemPM(EntityPM);
            mappedConsignments.Tenant = EntityPM.Tenant;

            // Initialize from Unifreight data if available
            mappedConsignments.ItemSerial = unifreightItem.itemSerial || '';
            mappedConsignments.MarksAndNumbers = unifreightItem.marksAndNumbers || '';
            mappedConsignments.Weight = unifreightItem.weight || '';
            mappedConsignments.ContainerIsoCode = unifreightItem.isoContainerType || '';

            // Find corresponding consignment item by serial or other identifier
            let consignment = this.currentDeclaration.Consignments.filter(c => c.SequenceNumeric == unifreightItem.itemSerial)[0];

            if (consignment) {
                const consignmentPackage = consignment.ConsignmentPackages[0];
                // Update fields if not set by Unifreight data:
                mappedConsignments.MarksAndNumbers = mappedConsignments.MarksAndNumbers || consignmentPackage?.MarksNumbers || '';
                mappedConsignments.PackageQuantity = consignmentPackage?.PackageQuantity || 0;
                mappedConsignments.Weight = mappedConsignments.Weight || consignmentPackage?.GrossMassMeasure || 0;
                mappedConsignments.MeasureType = consignmentPackage?.GrossMassMeasureTypeCode || '';
                mappedConsignments.PackageType = consignmentPackage?.PackageTypeCode || '';
                mappedConsignments.PackingTypeName = consignmentPackage?.PackageTypeName || '';
                mappedConsignments.MeasureTypeName = consignmentPackage?.GrossMassMeasureTypeName || '';
                mappedConsignments.ItemDescription = consignment.CargoDescription || '';
                mappedConsignments.ItemId = this.currentDeclaration.SupplierInvoices[0]?.SupplierInvoiceItems[0]?.ClassificationCode.substring(0, 6) || '';
                // Initialize ContainerTypeWCO field:
                this.getContainerTypeWCOData(consignment, mappedConsignments, unifreightItem.manifestNumber);
            }

            // Add to collections
            const certificateOfOriginItemLine = new CertificateOfOriginItemLine(mappedConsignments, this);
            this.CertificateOriginItemItems.Insert(certificateOfOriginItemLine);
            this.originalItemSource.Insert(certificateOfOriginItemLine);
            this.entityPM.CertificateOriginItemItems.push(mappedConsignments);
        });

        // add the items from this.currentDeclaration.Consignments are not exist in unifreight and exist in the currentDeclaration.Consignments
        this.currentDeclaration.Consignments.forEach((consignment) => {
            if (!result.certificateOfOriginItems.some(i => i.itemSerial == consignment.SequenceNumeric)) {
                const mappedConsignments = new CertificateOfOriginItemPM(EntityPM);
                mappedConsignments.Tenant = EntityPM.Tenant;

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
                const certificateOfOriginItemLine = new CertificateOfOriginItemLine(mappedConsignments, this);
                this.CertificateOriginItemItems.Insert(certificateOfOriginItemLine);
                this.originalItemSource.Insert(certificateOfOriginItemLine);
                this.entityPM.CertificateOriginItemItems.push(mappedConsignments);
            }
        });
    }

    parseXml(xmlString: string): any {
        // Parse the XML string into a DOM Document
        const parser = new DOMParser();
        const xmlDoc = parser.parseFromString(xmlString, 'application/xml');

        // Extract values from the XML
        const customsFile = xmlDoc.getElementsByTagName('LogitudeCustomsFileCertificate')[0];
        const customFileNo = customsFile.getElementsByTagName('CustomFileNo')[0]?.textContent || '';
        const id = customsFile.getElementsByTagName('Id')[0]?.textContent || '';
        const certificateType = customsFile.getElementsByTagName('CertificateType')[0]?.textContent || '';

        const items = customsFile.getElementsByTagName('CertificateOfOriginItem');
        const certificateOfOriginItems: any[] = [];

        for (let i = 0; i < items.length; i++) {
            const item = items[i];
            const itemSerial = item.getElementsByTagName('ItemSerial')[0]?.textContent || '';
            const manifestNumber = item.getElementsByTagName('ManifestNumber')[0]?.textContent || '';
            const description = item.getElementsByTagName('Description')[0]?.textContent || '';
            const marksAndNumbers = item.getElementsByTagName('MarksAndNumbers')[0]?.textContent || '';
            const weight = item.getElementsByTagName('Weight')[0]?.textContent || '';
            const isoContainerType = item.getElementsByTagName('IsoContainerType')[0]?.textContent || '';

            certificateOfOriginItems.push({
                itemSerial,
                manifestNumber,
                description,
                marksAndNumbers,
                weight,
                isoContainerType,
            });
        }

        return {
            customFileNo,
            id,
            certificateType,
            certificateOfOriginItems,
        };
    }

    buildXmlCertificateOfOriginPM(EntityPM: CertificateOfOriginPM) {
        const data = {
            CustomFileNo: this.currentDeclaration.CustomFileNo,
            Id: this.currentDeclaration.Id,
            CertificateType: EntityPM.CooTypeCode,
            // MAP CertificateOfOriginItems to Unifreight BY THIS STRUCTURE:
            CertificateOfOriginItems: this.currentDeclaration.Consignments.map(consignment => ({
                ItemSerial: consignment.ConsignmentNumber,
                ManifestNumber: consignment.ManifestNumber,
                // this fields will return full from Unifreight:
                Description: "",
                MarksAndNumbers: "",
                Weight: "",
                IsoContainerType: ""
            }))
        };
        const xmlDataString = this.convertToXML(data);
        console.log(xmlDataString);

        return this.convertToXML(data);
    }

    convertToXML(data) {
        const root = xmlbuilder.create('LOGICUSTCLOSEFILE', { encoding: 'UTF-8' }); // Ensure encoding is specified
        const certificate = root.ele('LogitudeCustomsFileCertificate');

        certificate.ele('CustomFileNo', data.CustomFileNo);
        certificate.ele('Id', data.Id);
        certificate.ele('CertificateType', data.CertificateType || ''); // Handle potentially undefined CertificateType

        const items = certificate.ele('CertificateOfOriginItems');

        data.CertificateOfOriginItems.forEach(item => {
            const itemElement = items.ele('CertificateOfOriginItem');
            itemElement.ele('ItemSerial', item.ItemSerial);
            itemElement.ele('ManifestNumber', item.ManifestNumber);
            // Explicitly add empty fields
            itemElement.ele('Description', item.Description || '');
            itemElement.ele('MarksAndNumbers', item.MarksAndNumbers || '');
            itemElement.ele('Weight', item.Weight || '');
            itemElement.ele('IsoContainerType', item.IsoContainerType || '');
        });

        const xmlString = root.end({ pretty: true });
        return xmlString;
    }



    initCertificateOriginItemItems(EntityPM: CertificateOfOriginPM) {
        this.CertificateOriginItemItems.Clear();
        this.originalItemSource.Clear();
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
            const certificateOfOriginItemLine = new CertificateOfOriginItemLine(mappedConsignments, this);
            this.CertificateOriginItemItems.Insert(certificateOfOriginItemLine);
            this.originalItemSource.Insert(certificateOfOriginItemLine);
            this.entityPM.CertificateOriginItemItems.push(mappedConsignments);

        });
    }


    exportStorageWebService = new ExportStorageWebService();
    getContainerTypeWCOData(consignment: ConsignmentPM, mappedConsignments: CertificateOfOriginItemPM, ManifestNumberFromUnifreight = null) {
        // Validate consignment data:
        if (!ManifestNumberFromUnifreight) consignment.ManifestNumber = consignment.ManifestNumber ? consignment.ManifestNumber : '';
        else consignment.ManifestNumber = ManifestNumberFromUnifreight;

        consignment.SecondCargoID = consignment.SecondCargoID ? consignment.SecondCargoID : '';
        consignment.ThirdCargoID = consignment.ThirdCargoID ? consignment.ThirdCargoID : '';
        consignment.CargoTypeCode = consignment.CargoTypeCode ? consignment.CargoTypeCode : '';

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
        this.InitializeCertificateOriginInvoiceItems(EntityPM.CertificateOriginInvoiceItems);
        this.InitializeCertificateOriginItemItems(EntityPM.CertificateOriginItemItems);
    }

    InitializeCertificateOriginInvoiceItems(certificateOriginInvoiceItems: CertificateOfOriginInvoicePM[]) {
        this.CertificateOriginInvoiceItems.Clear();

        // update CertificateOriginInvoice list:
        certificateOriginInvoiceItems.forEach((item) => {
            this.CertificateOriginInvoiceItems.Insert(new CertificateOfOriginInvoiceLine(item, this));
        });
    }

    InitializeCertificateOriginItemItems(certificateOriginItemItems: CertificateOfOriginItemPM[]) {
        this.CertificateOriginItemItems.Clear();
        this.originalItemSource.Clear();

        // update CertificateOriginItemItems list:
        certificateOriginItemItems.forEach((item) => {
            this.getMeasureNameFromCache(item.MeasureType, item);
            this.getPackageTypeNameFromCache(item.PackageType, item);
            this.getOriginCriterionCodeNameFromCache(item.OriginCriterionCode, true, item);


             const certificateOfOriginItemLine = new CertificateOfOriginItemLine(item, this);
            this.CertificateOriginItemItems.Insert(certificateOfOriginItemLine);
            this.originalItemSource.Insert(certificateOfOriginItemLine);
        });
    }

    RefreshCertificateOriginItemItemsClicked() {

        var confirmMsg: string = TextCodeTranslator.Translate('Customs.CertificateOfOriginItem.O.RefreshConfirmationQuestion');
        var confirmWindow = new ConfirmWindow();
        confirmWindow.Title = TextCodeTranslator.Translate("General.O.Confirm");
        confirmWindow.Width = 400;
        confirmWindow.Height = 180;
        confirmWindow.YesButtonText = TextCodeTranslator.Translate("Customs.General.B.OK");
        confirmWindow.NoButtonText = TextCodeTranslator.Translate("General.B.Cancel");
        confirmWindow.Show(confirmMsg);

        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {
                // this._declarationPMService.get(this.currentDeclaration.Id).subscribe(myResult => {
                //     var myResponse: ServiceResponse = myResult;
                //     if (!myResponse.HasError && myResponse.Result) {
                //         this.currentDeclaration = myResponse.Result;
                //     }
                this.supplierInvoiceExtendedPMService.GetSupplierInvoicesPMsForDeclaration(this.currentDeclaration.Id).subscribe((response: any) => {
                    if (!response.HasError && response.Result) {
                        this.currentDeclaration.SupplierInvoices = response.Result;
                    }
                    const oldItems = this.entityPM.CertificateOriginItemItems;
                    this.InitilizeNewCertificateWithConsignments(this.entityPM);
                    oldItems.forEach(item => {
                        item.ChangeSetOp = "Delete";
                        this.entityPM.CertificateOriginItemItems.push(item);
                    });
                    this.entityPM.IsChange = true;
                });
                // });
            }
        });
    }

    RefreshCertificateOriginInvoiceItemsClicked() {

        var confirmMsg: string = TextCodeTranslator.Translate('Customs.CertificateOfOriginInvoice.O.RefreshConfirmationQuestion');
        var confirmWindow = new ConfirmWindow();
        confirmWindow.Title = TextCodeTranslator.Translate("General.O.Confirm");
        confirmWindow.Width = 400;
        confirmWindow.Height = 180;
        confirmWindow.YesButtonText = TextCodeTranslator.Translate("Customs.General.B.OK");
        confirmWindow.NoButtonText = TextCodeTranslator.Translate("General.B.Cancel");
        confirmWindow.Show(confirmMsg);

        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {
                // this._declarationPMService.get(this.currentDeclaration.Id).subscribe(myResult => {
                //     var myResponse: ServiceResponse = myResult;
                //     if (!myResponse.HasError && myResponse.Result) {
                //         this.currentDeclaration = myResponse.Result;
                // this.entityPM.CertificateOriginInvoiceItems.forEach(item => this.entityPM.DeletedCertificateOriginInvoiceItems.push(item));
                const oldItems = this.entityPM.CertificateOriginInvoiceItems;

                this.InitilizeNewCertificateWithSupplierInvoices(this.entityPM);
                oldItems.forEach(item => {
                    item.ChangeSetOp = "Delete";
                    this.entityPM.CertificateOriginInvoiceItems.push(item);
                });
                this.entityPM.IsChange = true;
                //     }
                // });
            }
         });
    }

    updateEntity(EntityPM: CertificateOfOriginPM) {
        this.entityPM = EntityPM;
        this.InitilizeListsFromCertificateOfOrigin(EntityPM);
        this.setDisplayMessage();
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
            this.updateOptionsMap["OriginCriterionCode"].Arguments.QueryFilterItems = this.CriterionTypesFilterItems;
            // this.UpdateOptionParams.OriginCriterionCode.QueryFilterItems = this.CriterionTypesFilterItems;
        });
    }

    SetPropertiesEnabled() {
        var enabled = !this.IsDisplayOnly;
        this.UIProperties.SetEnabled("CooTypeCode", this.ObjectTableName, enabled);
        this.UIProperties.SetEnabled("RequestReasonCode", this.ObjectTableName, enabled);
        this.SetPropertiesEnabledAllFields(enabled);
    }

    SetPropertiesEnabledAllFields(enabled: boolean) {
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
        this.UIProperties.SetEnabled("Observations", this.ObjectTableName, enabled);

        // this.disableElementById("selectedValueTradeAgreementBox", enabled);
        // this.disableElementById("selectedValueOriginCountryBox", enabled);
        // this.disableElementById("selectedValueDestinationCountryBox", enabled);
    }

    // disableElementById(elementId: string, enabled: boolean): void {
    // const selectElement = document.getElementById(elementId) as HTMLSelectElement;
    // if (selectElement) {
    //     selectElement.disabled = !enabled;
    // }
    // }

    SetWarning() {
        this.UIProperties.SetWarning("CooTypeCode", this.ObjectTableName, true);
        this.UIProperties.SetWarning("RequestReasonCode", this.ObjectTableName, true);
    }

    SetDisableByCooTypeCodeEuro(enabled: boolean) { // if CooTypeCode = 1 or 2
        this.UIProperties.SetEnabled("TradeAgreementCountry1", this.ObjectTableName, enabled);
        this.UIProperties.SetEnabled("TradeAgreementCountry2", this.ObjectTableName, enabled);
        this.UIProperties.SetEnabled("TradeAgreementGroupOfCountries", this.ObjectTableName, enabled);
        this.UIProperties.SetEnabled("PlaceOfManufacture", this.ObjectTableName, enabled);
        this.UIProperties.SetEnabled("ZipCodeOfManufacture", this.ObjectTableName, enabled);
        // this.disableElementById("selectedValueTradeAgreementBox", enabled);
    }

    mandatoryFielsList = [];
    certificateOfOriginMandatoryFieldsList = [];
    tempCertificateOfOriginMandatoryFieldsList = [];
    certificateOfOriginWebService: CertificateOfOriginWebService = new CertificateOfOriginWebService();
    formSectionsCouples: FormSectionsCouples;
    //certificateOfOriginMandatoryCouples = [];
    SetWarningByCooTypeCode(CooTypeCode) {
        if (!CooTypeCode) {
            this.tempCertificateOfOriginMandatoryFieldsList.forEach(i => {
                this.UIProperties.SetWarning(i.MappedCertificateFieldsName, this.ObjectTableName, false);
            });

            this.IsDisplayOnly = true;
            this.SetPropertiesEnabledAllFields(!this.IsDisplayOnly);

            this.formSectionsCouples = new FormSectionsCouples(this.tempCertificateOfOriginMandatoryFieldsList);
            this.tempCertificateOfOriginMandatoryFieldsList = [];
            return;
        }
        else {
            this.IsDisplayOnly = false;
            this.SetPropertiesEnabledAllFields(!this.IsDisplayOnly);

            if (this.entityPM.CooTypeCode != "1" && this.entityPM.CooTypeCode != "2") {
                this.SetDisableByCooTypeCodeEuro(this.IsDisplayOnly);
            }
        }

        this.certificateOfOriginWebService.GetMandatoryFieldsByCooTypeCode(CooTypeCode, this.entityPM.Tenant).subscribe((myResponse: any) => {
            if (!myResponse.HasError) {
                if (this.tempCertificateOfOriginMandatoryFieldsList.length > 0) {
                    this.tempCertificateOfOriginMandatoryFieldsList.forEach(i => {
                        this.UIProperties.SetWarning(i.MappedCertificateFieldsName, this.ObjectTableName, false);
                    });
                }

                this.certificateOfOriginMandatoryFieldsList = myResponse?.Result;

                this.formSectionsCouples = new FormSectionsCouples(this.certificateOfOriginMandatoryFieldsList);

                if (this.certificateOfOriginMandatoryFieldsList.length > 0) {

                    this.certificateOfOriginMandatoryFieldsList.forEach(item => {
                        if (item.IsMandatory == FieldRequirement.Mandatory || item.IsMandatory == FieldRequirement.Condition) {


                            if (!AppTool.IsNullOrEmpty(item.MappedCertificateFieldsName)) {
                                this.UIProperties.SetWarning(item.MappedCertificateFieldsName, this.ObjectTableName, true);
                            }
                            this.setCouplesWarning(item.IsMandatory, true);
                        }
                    });
                    this.tempCertificateOfOriginMandatoryFieldsList = this.certificateOfOriginMandatoryFieldsList;
                }
                else {

                    this.tempCertificateOfOriginMandatoryFieldsList.forEach(i => {
                        this.UIProperties.SetWarning(i.MappedCertificateFieldsName, this.ObjectTableName, false);
                    });
                    this.tempCertificateOfOriginMandatoryFieldsList = [];
                }

            }
        });
    }

    isMandatorySelectedOriginCountry: boolean = false;
    isMandatorySelectedTradeAgreement: boolean = false;
    isMandatorySelectedDestinationCountry: boolean = false;
    private setCouplesWarning(isMandatory, enabled: boolean) {
        this.formSectionsCouples.groupOfCountriesList.forEach(group => {
            if (group.isMandatoryCouple && isMandatory == FieldRequirement.Condition) {
                this.UIProperties.SetWarning(group.fields[0], this.ObjectTableName, enabled);
                this.UIProperties.SetWarning(group.fields[1], this.ObjectTableName, enabled);
                this.updateSelectCouplesWarning(group.groupName, enabled);
            }
            else {
                this.updateSelectCouplesWarning(group.groupName, enabled);
            }
            this.checkWarningsCouples()
        });
    }

    updateSelectCouplesWarning(groupName: string = "", isMandatory: boolean = false) {
        if (groupName == "all") {
            this.isMandatorySelectedOriginCountry = isMandatory;
            this.isMandatorySelectedTradeAgreement = isMandatory;
            this.isMandatorySelectedDestinationCountry = isMandatory;
        }
        else {
            if (groupName == "OriginCountryCouple") this.isMandatorySelectedOriginCountry = isMandatory;
            if (groupName == "TradeAgreementCountryCouple") this.isMandatorySelectedTradeAgreement = isMandatory;
            if (groupName == "DestinationCountryCouple") this.isMandatorySelectedDestinationCountry = isMandatory;
        }
    }

    private checkWarningsCouples() {
        if (this.entityPM.RequestReasonCode != "10" && this.entityPM.RequestReasonCode != "13" && this.entityPM.RequestReasonCode != "14") {
            if (this.entityPM.OriginGroupOfCountry || this.entityPM.OriginCountry) {
                this.UIProperties.SetWarning(this.formSectionsCouples.originCountryCouple.fields[0], this.ObjectTableName, false);
                this.UIProperties.SetWarning(this.formSectionsCouples.originCountryCouple.fields[1], this.ObjectTableName, false);
                this.updateSelectCouplesWarning("OriginCountryCouple", false);
            }
            else {
                this.UIProperties.SetWarning(this.formSectionsCouples.originCountryCouple.fields[0], this.ObjectTableName, true);
                this.UIProperties.SetWarning(this.formSectionsCouples.originCountryCouple.fields[1], this.ObjectTableName, true);
                this.updateSelectCouplesWarning("OriginCountryCouple", true);
            }
            if (this.entityPM.DestinationGroupOfCountries || this.entityPM.DestinationCountry) {
                this.UIProperties.SetWarning(this.formSectionsCouples.destinationCountryCouple.fields[0], this.ObjectTableName, false);
                this.UIProperties.SetWarning(this.formSectionsCouples.destinationCountryCouple.fields[1], this.ObjectTableName, false);
                this.updateSelectCouplesWarning("DestinationCountryCouple", false);
            }
            else {
                this.UIProperties.SetWarning(this.formSectionsCouples.destinationCountryCouple.fields[0], this.ObjectTableName, true);
                this.UIProperties.SetWarning(this.formSectionsCouples.destinationCountryCouple.fields[1], this.ObjectTableName, true);
                this.updateSelectCouplesWarning("DestinationCountryCouple", true);
            }
            if (this.entityPM.TradeAgreementCountry2 || this.entityPM.TradeAgreementGroupOfCountries) {
                this.UIProperties.SetWarning(this.formSectionsCouples.tradeAgreementCountryCouple.fields[0], this.ObjectTableName, false);
                this.UIProperties.SetWarning(this.formSectionsCouples.tradeAgreementCountryCouple.fields[1], this.ObjectTableName, false);
                this.updateSelectCouplesWarning("TradeAgreementCountryCouple", false);
            }
            else {
                this.UIProperties.SetWarning(this.formSectionsCouples.tradeAgreementCountryCouple.fields[0], this.ObjectTableName, true);
                this.UIProperties.SetWarning(this.formSectionsCouples.tradeAgreementCountryCouple.fields[1], this.ObjectTableName, true);
                this.updateSelectCouplesWarning("TradeAgreementCountryCouple", true);
            }
        }
         else {
            this.setWarningFalseForCouples();
         }
    }


     private setWarningFalseForCouples() {
        this.UIProperties.SetWarning(this.formSectionsCouples.originCountryCouple.fields[0], this.ObjectTableName, false);
        this.UIProperties.SetWarning(this.formSectionsCouples.originCountryCouple.fields[1], this.ObjectTableName, false);
        this.UIProperties.SetWarning(this.formSectionsCouples.destinationCountryCouple.fields[0], this.ObjectTableName, false);
        this.UIProperties.SetWarning(this.formSectionsCouples.destinationCountryCouple.fields[1], this.ObjectTableName, false);
        this.UIProperties.SetWarning(this.formSectionsCouples.tradeAgreementCountryCouple.fields[0], this.ObjectTableName, false);
        this.UIProperties.SetWarning(this.formSectionsCouples.tradeAgreementCountryCouple.fields[1], this.ObjectTableName, false);
        this.updateSelectCouplesWarning("all", false);
 
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
            var consigneeName = this.currentDeclaration.DeclarationExportRecipients[0]?.RecipientName; // first from list
            if (consigneeName) {
                this.entityPM.ConsigneeName = consigneeName;
            }
            var consigneeAddress = this.currentDeclaration.DeclarationExportRecipients[0]?.RecipientAddress; // first from list
            if (consigneeAddress) {
                this.entityPM.ConsigneeAddress = consigneeAddress;
            }
            var consigneeCountry = this.currentDeclaration.DeclarationExportRecipients[0]?.RecipientIssueCountryCode; // first from list
            if (consigneeCountry) {
                this.entityPM.ConsigneeCountry = consigneeCountry;
            }
        }

        this.entityPM.DestinationCountry = !AppTool.IsNullOrEmpty(this.currentDeclaration.DestinationCountryCode) ? this.currentDeclaration.DestinationCountryCode : "";

        if (this.currentDeclaration.SupplierInvoices.length > 0) {
            let supplierInvoices = this.currentDeclaration.SupplierInvoices[0];

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



    CheckMandatoryFields() {
        if (!this.entityPM.CooTypeCode && !this.entityPM.RequestReasonCode) {
            this.ErrorsList = [TextCodeTranslator.Translate('Customs.CertificateOfOrigin.O.MandatoryFields')];
            // this.ErrorsList = ["סוג תעודת מקור וסיבת בקשה הם שדות חובה"];
        }
        else if (!this.entityPM.CooTypeCode) {
            this.ErrorsList = [TextCodeTranslator.Translate('Customs.CertificateOfOrigin.O.TypeCodeMandatory')];
            // this.ErrorsList = ["סוג תעודת מקור הום שדה חובה"];
        }
        else if (!this.entityPM.RequestReasonCode) {
            this.ErrorsList = [TextCodeTranslator.Translate('Customs.CertificateOfOrigin.O.RequestReasonMandatory')];
            // this.ErrorsList = ["סיבת בקשה הום שדה חובה"];
        }
        else {
            this.ErrorsList = [];
        }

        if (!(this.entityPM.RequestReasonCode != "10" && this.entityPM.RequestReasonCode != "13" && this.entityPM.RequestReasonCode != "14")) {
            this.setWarningFalseForCouples();
        } else {
            this.checkWarningsCouples();
        }


    }

    public CheckMandatoryCustomsFields(ValidationErrors = []) {
        this.tempCertificateOfOriginMandatoryFieldsList.forEach(item => {
            if (item) {
                let field = this.entityPM[item.MappedCertificateFieldsName];
                if (!field) {
                    var fieldName = TextCodeTranslator.Translate('Customs.CertificateOfOrigin.F.' + item.MappedCertificateFieldsName);
                     if (fieldName != "" &&
                        this.formSectionsCouples.originCountryCouple.fields.indexOf(item.MappedCertificateFieldsName) == -1 &&
                        this.formSectionsCouples.destinationCountryCouple.fields.indexOf(item.MappedCertificateFieldsName) == -1 &&
                        this.formSectionsCouples.tradeAgreementCountryCouple.fields.indexOf(item.MappedCertificateFieldsName) == -1) {
                         ValidationErrors.push(fieldName);
                    }
                }
            }
        });

        // handle couples error messages:
        ValidationErrors = this.checkCouplesErrorMessages(ValidationErrors);
        return ValidationErrors;
    }

    // Search(text: string) {
    //     var itemsSource: any = this.originalItemSource;
    //     if (AppTool.IsNullOrEmpty(text)) {
    //         this.CertificateOriginItemItems.InsertCollection(itemsSource.Collection);
    //     }
    //     else {

    //         var TempItemSource: CertificateOfOriginItemLine[] = [];
    //         if (text.length <= 2) {
    //             TempItemSource = itemsSource.Collection.filter(f => f.ItemSerial.toString().includes(text));
    //         }
    //         else {
    //             TempItemSource = itemsSource.Collection.filter(f => f.MarksAndNumbers.toUpperCase().includes(text.toUpperCase()));
    //         }
    //         this.CertificateOriginItemItems.InsertCollection(TempItemSource);
    //     }
    // }

    // #region actions button
    // private BuildUpdateParams() {

    //     for (var option in this.UpdateOptionParams) {
    //         let params = new UpdateGeneralParams();
    //         let fieldName = TextCodeTranslator.Translate("Customs.CertificateOfOrigin.O." + option);
    //         params.Title = TextCodeTranslator.Translate("Customs.Declaration.O.Update") + " " + fieldName;
    //         params.Arguments = new UpdateGeneralArgsParams();
    //         params.Arguments.UpdateField = option;
    //         params.Arguments.Title = TextCodeTranslator.Translate("Customs.CertificateOfOrigin.O.MultiUpdate");

    //         if (this.UpdateOptionParams[option]?.LookUpTableName && this.UpdateOptionParams[option]?.ObjectTableName) {
    //             params.Arguments.LookUpTableName = this.UpdateOptionParams[option].LookUpTableName;
    //             params.Arguments.ObjectTableName = this.UpdateOptionParams[option].ObjectTableName;
    //         }
    //         // if (this.UpdateOptionParams[option]?.Validate) {
    //         //     params.Arguments.Validate = SupplierInvoiceItemLine.validateClassificationCode;
    //         // }
    //         if (this.UpdateOptionParams[option]?.QueryFilterItems) {
    //             params.Arguments.QueryFilterItems = this.UpdateOptionParams[option].QueryFilterItems;
    //         }
    //         if (this.UpdateOptionParams[option]?.IsMultiline) {
    //             params.Arguments.IsMultiline = this.UpdateOptionParams[option].IsMultiline;
    //         }
    //         params.Arguments.ItemsWithNoValueTitle = TextCodeTranslator.Translate("Customs.CertificateOfOriginItem.O.ItemsWithNoValueTitle").replace("{field}", fieldName);
    //         params.Arguments.IsItemsWithNoValue = true;
    //         params.Arguments.SelectionCompletedMethod = (comp) => {
    //             this.SelectionOriginCompleted(comp);
    //         };
    //         params.Arguments.ObjectTableName = 'Customs.CertificateOfOriginItem';

    //         this.updateOptionsMap[option] = params;
    //     }
    // }

    // SelectionOriginCompleted(args) {
    //     if (args.ItemsSource != null) {
    //         if (args.UpdateAll) {
    //             for (let item of this.entityPM.CertificateOriginItemItems) {
    //                 if (item[args.UpdateField] != args) {
    //                     this.updateProcess(item, args);
    //                 }
    //             }
    //         } else {
    //             if (args.UpdateItemsWithNoValue) {
    //                 for (let item of this.entityPM.CertificateOriginItemItems) {
    //                     if (item[args.UpdateField] == "" || item[args.UpdateField] == null) {
    //                         this.updateProcess(item, args);
    //                     }
    //                 }
    //             } else {
    //                 if (args.ItemsSource) {
    //                     for (let item of this.entityPM.CertificateOriginItemItems) {
    //                         var number = args.ItemsSource.Collection.filter(d => d.Number == item.ItemSerial)[0];
    //                         if (number) {
    //                             this.updateProcess(item, args);
    //                         }
    //                     }
    //                 }
    //             }
    //         }
    //     }
    // }
    // updateProcess(item: CertificateOfOriginItemPM, args) {
    //     var updateField = args.UpdateField;

    //     switch (updateField) {
    //         case 'OriginCriterionCode': {
    //             item.OriginCriterionCode = args.FinalValue.Code;
    //             item.OriginCriterionCodeName = args.FinalValue.OriginCriterionCode;
    //             break;
    //         }

    //         default:
    //             item[updateField] = args.FinalValue;
    //     }

 
    //  }
 
 
    //#endregion
    checkCouplesErrorMessages(ValidationErrors) {
        if (this.isMandatorySelectedOriginCountry) {
            let originCountry = this.entityPM[this.formSectionsCouples.originCountryCouple.fields[0]];
            let originGroupOfCountry = this.entityPM[this.formSectionsCouples.originCountryCouple.fields[1]];
            if (this.selectedValueOriginCountry == this.fieldNameOriginCountry && !originCountry) {
                ValidationErrors.push(TextCodeTranslator.Translate('Customs.CertificateOfOrigin.F.' + this.formSectionsCouples.originCountryCouple.fields[0]))
            }
            else if (this.selectedValueOriginCountry == this.fieldNameOriginGroupOfCountry && !originGroupOfCountry) {
                ValidationErrors.push(TextCodeTranslator.Translate('Customs.CertificateOfOrigin.F.' + this.formSectionsCouples.originCountryCouple.fields[1]));
            }
        }
        if (this.isMandatorySelectedDestinationCountry) {
            let destinationCountry = this.entityPM[this.formSectionsCouples.destinationCountryCouple.fields[0]];
            let destinationGroupOfCountries = this.entityPM[this.formSectionsCouples.destinationCountryCouple.fields[1]];
            if (this.selectedValueDestinationCountry == this.fieldNameDestinationCountry && !destinationCountry) {
                ValidationErrors.push(TextCodeTranslator.Translate('Customs.CertificateOfOrigin.F.' + this.formSectionsCouples.destinationCountryCouple.fields[0]))
            }
            else if (this.selectedValueDestinationCountry == this.fieldNameDestinationGroupOfCountries && !destinationGroupOfCountries) {
                ValidationErrors.push(TextCodeTranslator.Translate('Customs.CertificateOfOrigin.F.' + this.formSectionsCouples.destinationCountryCouple.fields[1]));
            }
        }
        if (this.isMandatorySelectedTradeAgreement) {
            let tradeAgreementCountry2 = this.entityPM[this.formSectionsCouples.tradeAgreementCountryCouple.fields[0]];
            let tradeAgreementGroupOfCountries = this.entityPM[this.formSectionsCouples.tradeAgreementCountryCouple.fields[1]];
            if (this.selectedValueTradeAgreement == this.fieldNameTradeAgreementCountry2 && !tradeAgreementCountry2) {
                ValidationErrors.push(TextCodeTranslator.Translate('Customs.CertificateOfOrigin.F.' + this.formSectionsCouples.tradeAgreementCountryCouple.fields[0]))
            }
            else if (this.selectedValueTradeAgreement == this.fieldNameTradeAgreementGroupOfCountries && !tradeAgreementGroupOfCountries) {
                ValidationErrors.push(TextCodeTranslator.Translate('Customs.CertificateOfOrigin.F.' + this.formSectionsCouples.tradeAgreementCountryCouple.fields[1]));
            }
        }
        return ValidationErrors;
    }

    // public CheckMandatoryCustomsFields(ValidationErrors = []) {
    //     this.tempCertificateOfOriginMandatoryFieldsList.forEach(item => {
    //         if (item) {
    //             let field = this.entityPM[item.MappedCertificateFieldsName];
    //             if (!field) {
    //                 var fieldName = TextCodeTranslator.Translate('Customs.CertificateOfOrigin.F.' + item.MappedCertificateFieldsName);
    //                 if (fieldName != "" &&
    //                     this.formSectionsCouples.originCountryCouple.fields.indexOf(item.MappedCertificateFieldsName) == -1 &&
    //                     this.formSectionsCouples.destinationCountryCouple.fields.indexOf(item.MappedCertificateFieldsName) == -1 &&
    //                     this.formSectionsCouples.tradeAgreementCountryCouple.fields.indexOf(item.MappedCertificateFieldsName) == -1) {
    //                     ValidationErrors.push(fieldName);
    //                 }
    //             }
    //         }
    //     });
    //     this.formSectionsCouples.groupOfCountriesList.forEach(group => {
    //         let field1 = this.entityPM[group.fields[0]];
    //         let field2 = this.entityPM[group.fields[1]];
    //         if (!field1 && !field2) {
    //             var fieldName1 = TextCodeTranslator.Translate('Customs.CertificateOfOrigin.F.' + field1);
    //             var fieldName2 = TextCodeTranslator.Translate('Customs.CertificateOfOrigin.F.' + field2);
    //             ValidationErrors.push(fieldName1);
    //             ValidationErrors.push(fieldName2);
    //         }

    //         // find the field1 in the ValidationErrors:
    //         if (!field1 && field2) {
    //             var fieldName = TextCodeTranslator.Translate('Customs.CertificateOfOrigin.F.' + group.fields[0]);
    //             ValidationErrors = ValidationErrors.filter(i => i != fieldName);
    //         }
    //         else if (field1 && !field2) {
    //             var fieldName = TextCodeTranslator.Translate('Customs.CertificateOfOrigin.F.' + group.fields[1]);
    //             ValidationErrors = ValidationErrors.filter(i => i != fieldName);
    //         }
    //     });
    //     return ValidationErrors;
    // }

    Search(text: string) {
        var itemsSource: any = this.originalItemSource;
        if (AppTool.IsNullOrEmpty(text)) {
            this.CertificateOriginItemItems.InsertCollection(itemsSource.Collection);
        }
        else {

            var TempItemSource: CertificateOfOriginItemLine[] = [];
            if (text.length <= 2) {
                TempItemSource = itemsSource.Collection.filter(f => f.ItemSerial.toString().includes(text));
            }
            else {
                TempItemSource = itemsSource.Collection.filter(f => f.MarksAndNumbers.toUpperCase().includes(text.toUpperCase()));
            }
            this.CertificateOriginItemItems.InsertCollection(TempItemSource);
        }
    }

    // #region actions button
    private BuildUpdateParams() {

        for (var option in this.UpdateOptionParams) {
            let params = new UpdateGeneralParams();
            let fieldName = TextCodeTranslator.Translate("Customs.CertificateOfOrigin.O." + option);
            params.Title = TextCodeTranslator.Translate("Customs.Declaration.O.Update") + " " + fieldName;
            params.Arguments = new UpdateGeneralArgsParams();
            params.Arguments.UpdateField = option;
            params.Arguments.Title = TextCodeTranslator.Translate("Customs.CertificateOfOrigin.O.MultiUpdate");

            if (this.UpdateOptionParams[option]?.LookUpTableName && this.UpdateOptionParams[option]?.ObjectTableName) {
                params.Arguments.LookUpTableName = this.UpdateOptionParams[option].LookUpTableName;
                params.Arguments.ObjectTableName = this.UpdateOptionParams[option].ObjectTableName;
            }
            // if (this.UpdateOptionParams[option]?.Validate) {
            //     params.Arguments.Validate = SupplierInvoiceItemLine.validateClassificationCode;
            // }
            if (this.UpdateOptionParams[option]?.QueryFilterItems) {
                params.Arguments.QueryFilterItems = this.UpdateOptionParams[option].QueryFilterItems;
            }
            if (this.UpdateOptionParams[option]?.IsMultiline) {
                params.Arguments.IsMultiline = this.UpdateOptionParams[option].IsMultiline;
            }
            params.Arguments.ItemsWithNoValueTitle = TextCodeTranslator.Translate("Customs.CertificateOfOriginItem.O.ItemsWithNoValueTitle").replace("{field}", fieldName);
            params.Arguments.IsItemsWithNoValue = true;
            params.Arguments.SelectionCompletedMethod = (comp) => {
                this.SelectionOriginCompleted(comp);
            };
            params.Arguments.ObjectTableName = 'Customs.CertificateOfOriginItem';

            this.updateOptionsMap[option] = params;
        }
    }

    SelectionOriginCompleted(args) {
        if (args.ItemsSource != null) {
            if (args.UpdateAll) {
                for (let item of this.entityPM.CertificateOriginItemItems) {
                    if (item[args.UpdateField] != args) {
                        this.updateProcess(item, args);
                    }
                }
            } else {
                if (args.UpdateItemsWithNoValue) {
                    for (let item of this.entityPM.CertificateOriginItemItems) {
                        if (item[args.UpdateField] == "" || item[args.UpdateField] == null) {
                            this.updateProcess(item, args);
                        }
                    }
                } else {
                    if (args.ItemsSource) {
                        for (let item of this.entityPM.CertificateOriginItemItems) {
                            var number = args.ItemsSource.Collection.filter(d => d.Number == item.ItemSerial)[0];
                            if (number) {
                                this.updateProcess(item, args);
                            }
                        }
                    }
                }
            }
        }
    }
    updateProcess(item: CertificateOfOriginItemPM, args) {
        var updateField = args.UpdateField;

        switch (updateField) {
            case 'OriginCriterionCode': {
                item.OriginCriterionCode = args.FinalValue.Code;
                item.OriginCriterionCodeName = args.FinalValue.OriginCriterionCode;
                break;
            }

            default:
                item[updateField] = args.FinalValue;
        }
    }

    UpdateClicked(type){
        let selectedOptionsSettings=this.updateOptionsMap[type];

        let title = selectedOptionsSettings.Title;
        var args = selectedOptionsSettings.Arguments;
        this.UpdateCertificateOfOriginGeneralField(args, title);
    }

    UpdateCertificateOfOriginGeneralField(args: UpdateGeneralArgsParams, title) {
        var windowArgs: any = {};
        var logWindow = new LogitudeWindow();
        logWindow.Width = 700;
        logWindow.Height = 500;
        logWindow.ShowCloseButton = true;
        windowArgs.CertificateOfOriginPM = this.entityPM;
        windowArgs = Object.assign(windowArgs, args);
        logWindow.WindowArgs = windowArgs;
        logWindow.Title = title;
        logWindow.ComponentLoaded.subscribe(comp => {
            logWindow.WindowClosed.subscribe(s => {
                if (s) {
                    args.SelectionCompletedMethod(comp);
                }
            });
        });
        logWindow.Show('./CustomsModules/CustomsDeclarationModules/DeclarationTabs/Components/DigitalCertificateOfOrigin/CertificateOfOriginTabs/General/UpdateCertificateOfOriginGeneralFieldComponent');
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
        
        if(this.entityPM.CooTypeCode != newValue){
            this.entityPM.CooTypeCode = newValue;
            this.InitUrls();
        }
        this.entityPM.CooTypeCode = newValue;
         
        if (this.ErrorsList?.length > 0 || this.IsNewOrEdit === StatusCertificateOfOrigin.IsEdit) {
            this.CheckMandatoryFields();
        }
        if (this.entityPM.CooTypeCode) {
            this.entityPM.CertificateOriginItemItems.forEach(item => {
                this.getOriginCriterionCodeNameFromCache(item.OriginCriterionCode, false, item);
            });
        }
        this.SetWarningByCooTypeCode(this.entityPM.CooTypeCode);
        this.entityPM.IsDirty = true;
    }
    
    public get RequestReasonCode(): string {
        return this.entityPM.RequestReasonCode;
    }
    public set RequestReasonCode(newValue: string) {
        this.entityPM.RequestReasonCode = newValue;
        if (this.ErrorsList?.length > 0 || this.IsNewOrEdit === StatusCertificateOfOrigin.IsEdit) {
            this.CheckMandatoryFields();
        }
        this.entityPM.IsDirty = true;
    }

    public get COONumber(): string {
        return this.entityPM.COONumber;
    }
    public set COONumber(newValue: string) {
        this.entityPM.COONumber = newValue;
        this.entityPM.IsDirty = true;
    }

    public get COONumberToCancel(): string {
        return this.entityPM.COONumberToCancel;
    }
    public set COONumberToCancel(newValue: string) {
        this.entityPM.COONumberToCancel = newValue;
        this.entityPM.IsDirty = true;
    }

    public get ReplacementReason(): string {
        return this.entityPM.ReplacementReason;
    }
    public set ReplacementReason(newValue: string) {
        this.entityPM.ReplacementReason = newValue;
        this.entityPM.IsDirty = true;
    }

    public get DeclarationId(): string {
        return this.entityPM.DeclarationId;
    }
    public set DeclarationId(newValue: string) {
        this.entityPM.DeclarationId = newValue;
        this.entityPM.IsDirty = true;
    }

    public get ExporterVat(): string {
        return this.entityPM.ExporterVat;
    }
    public set ExporterVat(newValue: string) {
        this.entityPM.ExporterVat = newValue;
        this.entityPM.IsDirty = true;
    }


    public get ExporterName(): string {
        return this.entityPM.ExporterName;

    }
    public set ExporterName(newValue: string) {
        this.entityPM.ExporterName = newValue;
        this.entityPM.IsDirty = true;
    }

    public get ExporterAddress(): string {
        return this.entityPM.ExporterAddress;
    }
    public set ExporterAddress(newValue: string) {
        this.entityPM.ExporterAddress = newValue;
        this.entityPM.IsDirty = true;
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
        this.entityPM.IsDirty = true;
    }

    public get TradeAgreementCountry1(): string {
        if (AppTool.IsNullOrEmpty(this.entityPM.TradeAgreementCountry1)) {
            const countryCode = "IL";
            this.entityPM.TradeAgreementCountry1 = countryCode;
            return this.entityPM.TradeAgreementCountry1;
        }
        return this.entityPM.TradeAgreementCountry1;
    }
    public set TradeAgreementCountry1(newValue: string) {
        this.entityPM.TradeAgreementCountry1 = newValue;
        this.entityPM.IsDirty = true;
    }

    public get TradeAgreementCountry2(): string {
        return this.entityPM.TradeAgreementCountry2;
    }
    public set TradeAgreementCountry2(newValue: string) {
        this.entityPM.TradeAgreementCountry2 = newValue;
        this.entityPM.IsDirty = true;
        this.checkWarningsCouples();
    }

    public get TradeAgreementGroupOfCountries(): string {
        return this.entityPM.TradeAgreementGroupOfCountries;
    }

    public set TradeAgreementGroupOfCountries(newValue: string) {
        this.entityPM.TradeAgreementGroupOfCountries = newValue;
        this.entityPM.IsDirty = true;
        this.checkWarningsCouples();
    }

    public get ConsigneeName(): string {
        return this.entityPM.ConsigneeName;
    }
    public set ConsigneeName(newValue: string) {
        this.entityPM.ConsigneeName = newValue;
        this.entityPM.IsDirty = true;
    }

    public get ConsigneeAddress(): string {
        return this.entityPM.ConsigneeAddress;
    }
    public set ConsigneeAddress(newValue: string) {
        this.entityPM.ConsigneeAddress = newValue;
        this.entityPM.IsDirty = true;
    }

    public get ConsigneeCountry(): string {
        return this.entityPM.ConsigneeCountry;
    }
    public set ConsigneeCountry(newValue: string) {
        this.entityPM.ConsigneeCountry = newValue;
        this.entityPM.IsDirty = true;
    }

    public get ConsigneeRemarks(): string {
        return this.entityPM.ConsigneeRemarks;
    }
    public set ConsigneeRemarks(newValue: string) {
        this.entityPM.ConsigneeRemarks = newValue;
        this.entityPM.IsDirty = true;
    }

    public get IsConsigneeForPrint(): boolean {
        return this.entityPM.IsConsigneeForPrint;
    }
    public set IsConsigneeForPrint(newValue: boolean) {
        this.entityPM.IsConsigneeForPrint = newValue;
        this.entityPM.IsDirty = true;
    }

    public get OriginCountry(): string {
        return this.entityPM.OriginCountry;
    }
    public set OriginCountry(newValue: string) {
        this.entityPM.OriginCountry = newValue;
        this.entityPM.IsDirty = true;

        this.checkWarningsCouples();
    }

    public get OriginGroupOfCountry(): string {
        return this.entityPM.OriginGroupOfCountry;
    }
    public set OriginGroupOfCountry(newValue: string) {
        this.entityPM.OriginGroupOfCountry = newValue;
        this.entityPM.IsDirty = true;

        this.checkWarningsCouples();
    }

    public get DestinationCountry(): string {
        return this.entityPM.DestinationCountry;
    }
    public set DestinationCountry(newValue: string) {
        this.entityPM.DestinationCountry = newValue;
        this.entityPM.IsDirty = true;

        this.checkWarningsCouples();
    }

    public get DestinationGroupOfCountries(): string {
        return this.entityPM.DestinationGroupOfCountries;
    }
    public set DestinationGroupOfCountries(newValue: string) {
        this.entityPM.DestinationGroupOfCountries = newValue;
        this.entityPM.IsDirty = true;

        this.checkWarningsCouples();
    }

    public get Transport(): string {
        return this.entityPM.Transport;
    }
    public set Transport(newValue: string) {
        this.entityPM.Transport = newValue;
        this.entityPM.IsDirty = true;
    }

    public get PortOfShipment(): string {
        return this.entityPM.PortOfShipment;
    }
    public set PortOfShipment(newValue: string) {
        this.entityPM.PortOfShipment = newValue;
        this.entityPM.IsDirty = true;
    }

    public get IsCumulation(): boolean {
        return this.entityPM.IsCumulation;
    }
    public set IsCumulation(newValue: boolean) {
        this.entityPM.IsCumulation = newValue;
        this.entityPM.IsDirty = true;
    }

    public get CumulationCountry(): string {
        return this.entityPM.CumulationCountry;
    }
    public set CumulationCountry(newValue: string) {
        this.entityPM.CumulationCountry = newValue;
        this.entityPM.IsDirty = true;
    }

    public get CumulationGroupOfCountries(): string {
        return this.entityPM.CumulationGroupOfCountries;
    }
    public set CumulationGroupOfCountries(newValue: string) {
        this.entityPM.CumulationGroupOfCountries = newValue;
        this.entityPM.IsDirty = true;
    }


    public get PlaceOfManufacture(): string {
        return this.entityPM.PlaceOfManufacture;
    }
    public set PlaceOfManufacture(newValue: string) {
        this.entityPM.PlaceOfManufacture = newValue;
        this.entityPM.IsDirty = true;
    }

    public get ZipCodeOfManufacture(): string {
        return this.entityPM.ZipCodeOfManufacture;
    }
    public set ZipCodeOfManufacture(newValue: string) {
        this.entityPM.ZipCodeOfManufacture = newValue;
        this.entityPM.IsDirty = true;
    }

    public get Observations(): string {
        return this.entityPM.Observations;
    }
    public set Observations(newValue: string) {
        this.entityPM.Observations = newValue;
        this.entityPM.IsDirty = true;
    }

    public get IsExportDecForPrint(): boolean {
        return this.entityPM.IsExportDecForPrint;
    }
    public set IsExportDecForPrint(newValue: boolean) {
        this.entityPM.IsExportDecForPrint = newValue;
        this.entityPM.IsDirty = true;
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

        this.entityPM.IsDirty = true;
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
        this.entityPM.IsDirty = true;
    }

    public get IssuingCountry(): string {
        return this.entityPM.IssuingCountry;
    }
    public set IssuingCountry(newValue: string) {
        this.entityPM.IssuingCountry = newValue;
        this.entityPM.IsDirty = true;
    }

    public get CityOfDeclaration(): string {
        return this.entityPM.CityOfDeclaration;
    }
    public set CityOfDeclaration(newValue: string) {
        this.entityPM.CityOfDeclaration = newValue;
        this.entityPM.IsDirty = true;
    }

    public get CountryOfDeclaration(): string {
        return this.entityPM.CountryOfDeclaration;
    }
    public set CountryOfDeclaration(newValue: string) {
        this.entityPM.CountryOfDeclaration = newValue;
        this.entityPM.IsDirty = true;
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
        this.entityPM.IsDirty = true;
    }
   

    public get IsDeclaredByManufacture(): boolean {
        return this.entityPM.IsDeclaredByManufacture;
    }
    public set IsDeclaredByManufacture(newValue: boolean) {
        this.entityPM.IsDeclaredByManufacture = newValue;
        this.entityPM.IsDirty = true;
    }

    public get IsDeclaredByExporter(): boolean {
        return this.entityPM.IsDeclaredByExporter;
    }
    public set IsDeclaredByExporter(newValue: boolean) {
        this.entityPM.IsDeclaredByExporter = newValue;
        this.entityPM.IsDirty = true;
    }

    public get IsAttachedList(): boolean {
        return this.entityPM.IsAttachedList;
    }
    public set IsAttachedList(newValue: boolean) {
        this.entityPM.IsAttachedList = newValue;
        this.entityPM.IsDirty = true;
    }

    public get InsufficentWorkingInd(): boolean {
        return this.entityPM.InsufficentWorkingInd;
    }
    public set InsufficentWorkingInd(newValue: boolean) {
        this.entityPM.InsufficentWorkingInd = newValue;
        this.entityPM.IsDirty = true;
    }

    public get InsufficentWorkingText(): string {
        return this.entityPM.InsufficentWorkingText;
    }
    public set InsufficentWorkingText(newValue: string) {
        this.entityPM.InsufficentWorkingText = newValue;
        this.entityPM.IsDirty = true;
    }

    public get NonExportDate(): Date {
        return this.entityPM.NonExportDate;
    }
    public set NonExportDate(newValue: Date) {
        this.entityPM.NonExportDate = newValue;
        this.entityPM.IsDirty = true;
    }

    public get NonExportCountry(): string {
        return this.entityPM.NonExportCountry;
    }
    public set NonExportCountry(newValue: string) {
        this.entityPM.NonExportCountry = newValue;
        this.entityPM.IsDirty = true;
    }

    public get NonImportBillOfLadingNum(): string {
        return this.entityPM.NonImportBillOfLadingNum;
    }
    public set NonImportBillOfLadingNum(newValue: string) {
        this.entityPM.NonImportBillOfLadingNum = newValue;
        this.entityPM.IsDirty = true;
    }

    public get NonExportPort(): string {
        return this.entityPM.NonExportPort;
    }
    public set NonExportPort(newValue: string) {
        this.entityPM.NonExportPort = newValue;
        this.entityPM.IsDirty = true;
    }

    public get NonImportDate(): Date {
        return this.entityPM.NonImportDate;
    }
    public set NonImportDate(newValue: Date) {
        this.entityPM.NonImportDate = newValue;
        this.entityPM.IsDirty = true;
    }

    public get NonExportBillOfLadingNum(): string {
        return this.entityPM.NonExportBillOfLadingNum;
    }
    public set NonExportBillOfLadingNum(newValue: string) {
        this.entityPM.NonExportBillOfLadingNum = newValue;
        this.entityPM.IsDirty = true;
    }

    public get NonTransirCountry(): string {
        return this.entityPM.NonTransirCountry;
    }
    public set NonTransirCountry(newValue: string) {
        this.entityPM.NonTransirCountry = newValue;
        this.entityPM.IsDirty = true;
    }

    public get NonPortOfEntrance(): string {
        return this.entityPM.NonPortOfEntrance;
    }
    public set NonPortOfEntrance(newValue: string) {
        this.entityPM.NonPortOfEntrance = newValue;
        this.entityPM.IsDirty = true;
    }

    public get NonExpectedExitDate(): Date {
        return this.entityPM.NonExpectedExitDate;
    }
    public set NonExpectedExitDate(newValue: Date) {
        this.entityPM.NonExpectedExitDate = newValue;
        this.entityPM.IsDirty = true;
    }

    public get NonExitPort(): string {
        return this.entityPM.NonExitPort;
    }
    public set NonExitPort(newValue: string) {
        this.entityPM.NonExitPort = newValue;
        this.entityPM.IsDirty = true;
    }

    public get NonGoodsDescription(): string {
        return this.entityPM.NonGoodsDescription;
    }
    public set NonGoodsDescription(newValue: string) {
        this.entityPM.NonGoodsDescription = newValue;
        this.entityPM.IsDirty = true;
    }

    public get NonDeclaringCompany(): string {
        return this.entityPM.NonDeclaringCompany;
    }
    public set NonDeclaringCompany(newValue: string) {
        this.entityPM.NonDeclaringCompany = newValue;
        this.entityPM.IsDirty = true;
    }

    public get NonDeclaringPerson(): string {
        return this.entityPM.NonDeclaringPerson;
    }
    public set NonDeclaringPerson(newValue: string) {
        this.entityPM.NonDeclaringPerson = newValue;
        this.entityPM.IsDirty = true;
    }

    public get NonDeclaringPosition(): string {
        return this.entityPM.NonDeclaringPosition;
    }
    public set NonDeclaringPosition(newValue: string) {
        this.entityPM.NonDeclaringPosition = newValue;
        this.entityPM.IsDirty = true;
    }

    public get NonManifestNum(): string {
        return this.entityPM.NonManifestNum;
    }
    public set NonManifestNum(newValue: string) {
        this.entityPM.NonManifestNum = newValue;
        this.entityPM.IsDirty = true;
    }

    public get ErrXml(): string {
        return this.entityPM.ErrXml;
    }
    public set ErrXml(newValue: string) {
        this.entityPM.ErrXml = newValue;
        this.entityPM.IsDirty = true;
    }

    public get CooStatusCode(): string {
        return this.entityPM.CooStatusCode;
    }
    public set CooStatusCode(newValue: string) {
        this.entityPM.CooStatusCode = newValue;
        this.entityPM.IsDirty = true;
    }
    public get FeedbackRemark(): string {
        return this.entityPM.FeedbackRemark;
    }
    public set FeedbackRemark(newValue: string) {
        this.entityPM.FeedbackRemark = newValue;
        this.entityPM.IsDirty = true;
    }

    public get RejectCancelReason(): string {
        return this.entityPM.RejectCancelReason;
    }
    public set RejectCancelReason(newValue: string) {
        this.entityPM.RejectCancelReason = newValue;
        this.entityPM.IsDirty = true;
    }

    public get IssueDateIfReleased(): Date {
        return this.entityPM.IssueDateIfReleased;
    }
    public set IssueDateIfReleased(newValue: Date) {
        this.entityPM.IssueDateIfReleased = newValue;
        this.entityPM.IsDirty = true;
    }

    public get QueryUrl(): string {
        return this.entityPM.QueryUrl;
    }
    public set QueryUrl(newValue: string) {
        this.entityPM.QueryUrl = newValue;
        this.entityPM.IsDirty = true;
    }

    public get CooPdf(): string {
        return this.entityPM.CooPdf;
    }
    public set CooPdf(newValue: string) {
        this.entityPM.CooPdf = newValue;
        this.entityPM.IsDirty = true;
    }

    public get CoodPdf1(): string {
        return this.entityPM.CoodPdf1;
    }
    public set CoodPdf1(newValue: string) {
        this.entityPM.CoodPdf1 = newValue;
        this.entityPM.IsDirty = true;
    }

    public get OpenByUser(): string {
        return this.entityPM.OpenByUser;
    }
    public set OpenByUser(newValue: string) {
        this.entityPM.OpenByUser = newValue;
        this.entityPM.IsDirty = true;
    }

    public get IsSubmitted(): boolean {
        return this.entityPM.IsSubmitted;
    }
    public set IsSubmitted(newValue: boolean) {
        this.entityPM.IsSubmitted = newValue;
        this.entityPM.IsDirty = true;
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
        this.entityPM.IsDirty = true;
        this.Parent.entityPM.IsDirty = true;
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

    SetLocalName(entity, fieldName, item, CertificateOriginItemItems) {
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
        this.updateIsDirty();
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
        this.updateIsDirty();
    }

    public get MeasureTypeName(): string {
        return this.entityPM.MeasureTypeName;
    }
    public set MeasureTypeName(newValue: string) {
        this.entityPM.MeasureTypeName = newValue;
        this.updateIsDirty();
    }

    public get ContainerIsoCode(): string {
        return this.entityPM.ContainerIsoCode;
    }
    public set ContainerIsoCode(newValue: string) {
        this.entityPM.ContainerIsoCode = newValue;
        this.updateIsDirty();
    }

    public get MarksAndNumbers(): string {
        return this.entityPM.MarksAndNumbers;
    }
    public set MarksAndNumbers(newValue: string) {
        this.entityPM.MarksAndNumbers = newValue;
        this.updateIsDirty();
    }

    public get ItemDescription(): string {
        return this.entityPM.ItemDescription;
    }
    public set ItemDescription(newValue: string) {
        this.entityPM.ItemDescription = newValue;
        this.updateIsDirty();
    }

    public get PackageQuantity(): number {
        return this.entityPM.PackageQuantity;
    }
    public set PackageQuantity(newValue: number) {
        this.entityPM.PackageQuantity = newValue;
        this.updateIsDirty();
    }

    public get PackageType(): string {
        return this.entityPM.PackageType;
    }
    public set PackageType(newValue: string) {
        this.entityPM.PackageType = newValue;
        this.updateIsDirty();
    }

    public get PackingTypeName(): string {
        return this.entityPM.PackingTypeName;
    }
    public set PackingTypeName(newValue: string) {
        this.entityPM.PackingTypeName = newValue;
        this.updateIsDirty();
    }

    public get Weight(): number {
        return this.entityPM.Weight;
    }
    public set Weight(newValue: number) {
        this.entityPM.Weight = newValue;
        this.updateIsDirty();
    }
    public get OriginCriterionCode(): string {
        return this.entityPM.OriginCriterionCode;
    }
    public set OriginCriterionCode(newValue: string) {
        this.entityPM.OriginCriterionCode = newValue;
        this.updateIsDirty();
    }
    public get OriginCriterionCodeName(): string {
        return this.entityPM.OriginCriterionCodeName;
    }
    public set OriginCriterionCodeName(newValue: string) {
        this.entityPM.OriginCriterionCodeName = newValue;
        this.updateIsDirty();
    }

    updateIsDirty() {
        this.entityPM.IsDirty = true;
        this.Parent.entityPM.IsDirty = true;
    }
}

enum FieldRequirement {
    Mandatory = "Mandatory",
    Optional = "Optional",
    Condition = "Condition",
}


enum CouplesMandatoryFields {
    OriginCountryCouple = "OriginCountryCouple",
    DestinationCountryCouple = "DestinationCountryCouple",
    TradeAgreementCountryCouple = "TradeAgreementCountryCouple",
}
// Define the structure for the Country and GroupOfCountries to be used in different context

interface GroupOfCountries {
    groupName: string;
    fields: string[];
    isMandatoryCouple: boolean;
}
class FormSectionsCouples {
    originCountryCouple: GroupOfCountries = {
        groupName: CouplesMandatoryFields.OriginCountryCouple,
        fields: ["OriginCountry", "OriginGroupOfCountry"],
        isMandatoryCouple: false
    };
    destinationCountryCouple: GroupOfCountries = {
        groupName: CouplesMandatoryFields.DestinationCountryCouple,
        fields: ["DestinationCountry", "DestinationGroupOfCountries"],
        isMandatoryCouple: false
    };
    tradeAgreementCountryCouple: GroupOfCountries = {
        groupName: CouplesMandatoryFields.TradeAgreementCountryCouple,
        fields: ["TradeAgreementCountry2", "TradeAgreementGroupOfCountries"],
        isMandatoryCouple: false
    };

    certificateOfOriginMandatoryCouples = [];
    constructor(certificateOfOriginMandatory) {
        this.certificateOfOriginMandatoryCouples = certificateOfOriginMandatory;

        this.filterListCouples();
    }

    groupOfCountriesList = [this.originCountryCouple, this.destinationCountryCouple, this.tradeAgreementCountryCouple];

    filterListCouples() {
        let isMandatoryFieldExist = [];
        this.groupOfCountriesList.forEach(group => {
            group.fields.forEach(field => {
                if (group.isMandatoryCouple) return;
                isMandatoryFieldExist = this.certificateOfOriginMandatoryCouples.filter(item => !AppTool.IsNullOrEmpty(item.MappedCertificateFieldsName) && item.MappedCertificateFieldsName.toLocaleLowerCase() == field.toLocaleLowerCase() && item.IsMandatory == FieldRequirement.Condition);
                group.isMandatoryCouple = isMandatoryFieldExist.length > 0 ? true : false;
            });
        });
    }

   
}





