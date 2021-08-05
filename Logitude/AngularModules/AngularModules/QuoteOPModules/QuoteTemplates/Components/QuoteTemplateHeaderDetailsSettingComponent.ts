declare var System: any;
declare var window: any;
import {Component, OnInit, ViewChild, ViewContainerRef} from '@angular/core';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {QuoteTemplatePM} from '../../../Quote/EntityPMs/QuoteTemplatePM';
import {QuoteTemplateSettingPM} from '../../../Quote/EntityPMs/QuoteTemplateSettingPM';
import {QuoteTemplateTextDesignPM} from '../../../Quote/EntityPMs/QuoteTemplateTextDesignPM';
import {QuoteTemplateTableDesignPM} from '../../../Quote/EntityPMs/QuoteTemplateTableDesignPM';
import {QuoteTemplateDetailsFieldPM} from '../../../Quote/EntityPMs/QuoteTemplateDetailsFieldPM';
import {QuoteTemplateHeaderFieldPM} from '../../../Quote/EntityPMs/QuoteTemplateHeaderFieldPM';
import {BorderType} from '../../../Infrastructure/Components/LogitudeCustomComponents/TextDesignComponent';
import {ObservableCollection} from '../../../Infrastructure/Utilities/ObservableCollection';
import {QuoteTemplateTextCodePM} from '../../../Quote/EntityPMs/QuoteTemplateTextCodePM';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {QuoteTemplateSettingPMService} from '../../../Quote/Services/StandardPMs/QuoteTemplateSettingPMService';
import {QuoteTemplateTableDesignPMService} from '../../../Quote/Services/StandardPMs/QuoteTemplateTableDesignPMService';
import {QuoteTemplateTextDesignPMService} from '../../../Quote/Services/StandardPMs/QuoteTemplateTextDesignPMService';
import {QuoteTemplateTextDesignExtendedPMService} from '../../../Quote/Services/ExtendedPMs/QuoteTemplateTextDesignExtendedPMService';
import {QuoteTemplateTextCodeExtendedPMService} from '../../../Quote/Services/ExtendedPMs/QuoteTemplateTextCodeExtendedPMService';
import {QuoteTemplateDetailsFieldExtendedPMService} from '../../../Quote/Services/ExtendedPMs/QuoteTemplateDetailsFieldExtendedPMService';
import {QuoteTemplateHeaderFieldExtendedPMService} from '../../../Quote/Services/ExtendedPMs/QuoteTemplateHeaderFieldExtendedPMService';
import {Guid} from '../../../Infrastructure/Utilities/Guid';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { ObjectFieldPM } from'../../../Infrastructure/entitypms/ObjectFieldPM';
import {TextCodeData} from'./QuoteTemplatePricingSettingComponent';
import {AppTool} from '../../../Infrastructure/Tools';

@Component({
    selector: 'QuoteTemplateHeaderDetailsSettingComponent',
    
    templateUrl: './QuoteTemplateHeaderDetailsSettingComponent.html',
})

export class QuoteTemplateHeaderDetailsSettingComponent extends BaseComponent implements OnInit {
    quoteTemplateSettingPMService: QuoteTemplateSettingPMService;
    quoteTemplateDetailsFieldExtendedPMService: QuoteTemplateDetailsFieldExtendedPMService;
    quoteTemplateHeaderFieldExtendedPMService: QuoteTemplateHeaderFieldExtendedPMService;

    quoteTemplateTextDesignPMService: QuoteTemplateTextDesignPMService;
    quoteTemplateTableDesignPMService: QuoteTemplateTableDesignPMService;
    quoteTemplateTextDesignExtendedPMService: QuoteTemplateTextDesignExtendedPMService;
    quoteTemplateTextCodeExtendedPMService: QuoteTemplateTextCodeExtendedPMService;
    QuoteTemplatePM: QuoteTemplatePM;
    IsLoadPage: boolean;

    Alignment: string[] = [];
    QuoteTemplateSettingPM: QuoteTemplateSettingPM;

    public ValidationErrorsList: string[];

    TableDesignPM: QuoteTemplateTableDesignPM;
    HeaderTextDesignPM: QuoteTemplateTextDesignPM;
    RowTextDesignPM: QuoteTemplateTextDesignPM;
    TitleTextDesignPM: QuoteTemplateTextDesignPM;
    IsLoadingTextDesign: boolean = true;
    IsLoadingQuoteField: boolean = true;
    QuoteTemplateDetailsFieldPMList: QuoteTemplateDetailsFieldPM[] = [];
    QuoteTemplateHeaderFieldPMList: QuoteTemplateHeaderFieldPM[] = [];


    QuoteTemplateTextDesignPMLists: QuoteTemplateTextDesignPM[] = [];
    QuoteTemplateTextCodePMList: QuoteTemplateTextCodePM[] = [];
    ObjectFieldTextList: ObjectFieldText[] = [];
    ObjectFieldTextListColum1: ObjectFieldText[] = [];
    ObjectFieldTextListColum2: ObjectFieldText[] = []; 

    AllObjectFieldTextList: ObjectFieldText[] = [];
    ObjectFieldTextListColum2ListSelected: ObjectFieldText;
    ObjectFieldTextListColum1ListSelected: ObjectFieldText;
    ObjectFieldTextListSelected: ObjectFieldText;
    ObjectFieldPMList: ObjectFieldPM[] = [];
    IsWindowOpened: boolean = true;

    IsSaveQuoteTemplateTextDesignRuning: boolean = false;
    IsSaveQuoteTemplateTableDesignRuning: boolean = false;
    IsSaveQuoteTemplateTextCodeRuning: boolean = false;
    IsSaveQuoteTemplateObjectField: boolean = false;
    BorderTypesSelected: BorderType;
    BorderTypes: BorderType[] = [];
    public ItemsSource: ObservableCollection;
    QuotePM: any;
    QuoteTemplateSectionTypeName: string = "QuoteHeader";
    QuoteTemplateSectionTypeCode: string = "QH";
    @ViewChild('Child', { read: ViewContainerRef, static: false }) viewContainerRef: ViewContainerRef;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.quoteTemplateSettingPMService = new QuoteTemplateSettingPMService();
        this.quoteTemplateTableDesignPMService = new QuoteTemplateTableDesignPMService();
        this.quoteTemplateTextDesignPMService = new QuoteTemplateTextDesignPMService();
        this.quoteTemplateTextDesignExtendedPMService = new QuoteTemplateTextDesignExtendedPMService();
        this.quoteTemplateTextCodeExtendedPMService = new QuoteTemplateTextCodeExtendedPMService();
        this.quoteTemplateDetailsFieldExtendedPMService = new QuoteTemplateDetailsFieldExtendedPMService();
        this.quoteTemplateHeaderFieldExtendedPMService = new QuoteTemplateHeaderFieldExtendedPMService();

        this.ItemsSource = new ObservableCollection([]);
    }

    ngOnInit() {

    }

    SelectedTabCode: string;
    SetWindowArgs(args: any) {
        this.SelectedTabCode = "TAC";
        this.QuoteTemplatePM = args.QuoteTemplatePM;
        this.QuoteTemplateSectionTypeName = args.QuoteTemplateSectionTypeName;
        this.QuoteTemplateSectionTypeCode = args.QuoteTemplateSectionTypeCode;
        this.QuoteTemplateSettingPM = args.QuoteTemplateSettingPM;
        this.QuotePM = args.QuotePM;


        this.QuoteTemplateDetailsFieldPMList = [];
        this.QuoteTemplateHeaderFieldPMList = [];
        this.QuoteTemplateTextDesignPMLists = [];
        this.QuoteTemplateTextCodePMList = [];
        this.ObjectFieldTextList = [];
        this.ObjectFieldTextListColum1= [];
        this.ObjectFieldTextListColum2 = []; 


        this.BorderTypes = [];
        this.BorderTypes.push(new BorderType("Auto", "AUTO"));
        this.BorderTypes.push(new BorderType("Fixed", "FIXED"));


        var selectedBorderCode = this.QuoteTemplateSectionTypeCode == "QH" ? this.QuoteTemplateSettingPM.HeaderTableColumWidthType : this.QuoteTemplateSettingPM.DetailsTableColumWidthType;


        this.BorderTypesSelected = this.BorderTypes.filter(d => d.Code == selectedBorderCode)[0];


        this.Alignment.push("Left"); this.Alignment.push("Center"); this.Alignment.push("Right");

        if (args.QuoteTemplateTextCodePMList) {
            this.QuoteTemplateTextCodePMList = args.QuoteTemplateTextCodePMList.filter(d => d.Area == this.QuoteTemplateSectionTypeName);
            this.CustomQuoteTemplateTextCodePMLists();
            this.BuildItemsSource();
        }

        this.LoadData();
    }



    BorderTypesSelectedChanged(border: BorderType) {
        if (this.QuoteTemplateSettingPM) {

            if (this.QuoteTemplateSectionTypeCode == "QH") {
                if (this.QuoteTemplateSettingPM.HeaderTableColumWidthType != border.Code) {
                    this.QuoteTemplateSettingPM.HeaderTableColumWidthType = border.Code;
               
                }
            } else {

                if (this.QuoteTemplateSettingPM.DetailsTableColumWidthType != border.Code) {
                    this.QuoteTemplateSettingPM.DetailsTableColumWidthType = border.Code;
          
                }

            }
        }
    }

    //Prop setting 
    ShowTitleQuoteDetailsKey: string = Guid.newGuid();
    get ShowTitleQuoteDetails() {
        var showTitleQuoteDetails: boolean = false;
        if (this.QuoteTemplateSettingPM) showTitleQuoteDetails = this.QuoteTemplateSettingPM.ShowTitleQuoteDetails;
        return showTitleQuoteDetails;
    }
    set ShowTitleQuoteDetails(value: boolean) {
        if (this.QuoteTemplateSettingPM != null) {
            this.QuoteTemplateSettingPM.ShowTitleQuoteDetails = value;
        }
    }

    get TableColumn1LabelWidth() {
        var tableColumn1LabelWidth: number = 0;
        if (this.QuoteTemplateSettingPM) tableColumn1LabelWidth = this.QuoteTemplateSectionTypeCode == "QH" ? this.QuoteTemplateSettingPM.HeaderTableColumn1LabelWidth : this.QuoteTemplateSettingPM.DetailsTableColumn1LabelWidth;
        return tableColumn1LabelWidth;
    }
    set TableColumn1LabelWidth(value: number) {
        if (this.QuoteTemplateSettingPM != null) {
            if (this.QuoteTemplateSectionTypeCode == "QH") this.QuoteTemplateSettingPM.HeaderTableColumn1LabelWidth = value;
            else this.QuoteTemplateSettingPM.DetailsTableColumn1LabelWidth = value;
        }
    }


    get TableColumn1ValueWidth() {
        var tableColumn1ValueWidth: number = 0;
        if (this.QuoteTemplateSettingPM) tableColumn1ValueWidth = this.QuoteTemplateSectionTypeCode == "QH" ? this.QuoteTemplateSettingPM.HeaderTableColumn1ValueWidth : this.QuoteTemplateSettingPM.DetailsTableColumn1ValueWidth;
        return tableColumn1ValueWidth;
    }
    set TableColumn1ValueWidth(value: number) {
        if (this.QuoteTemplateSettingPM != null) {
            if (this.QuoteTemplateSectionTypeCode == "QH") this.QuoteTemplateSettingPM.HeaderTableColumn1ValueWidth = value;
            else this.QuoteTemplateSettingPM.DetailsTableColumn1ValueWidth = value;
        }
    }



    get TableColumn2LabelWidth() {
        var tableColumn2LabelWidth: number = 0;
        if (this.QuoteTemplateSettingPM) tableColumn2LabelWidth = this.QuoteTemplateSectionTypeCode == "QH" ? this.QuoteTemplateSettingPM.HeaderTableColumn2LabelWidth : this.QuoteTemplateSettingPM.DetailsTableColumn2LabelWidth;
        return tableColumn2LabelWidth;
    }
    set TableColumn2LabelWidth(value: number) {
        if (this.QuoteTemplateSettingPM != null) {
            if (this.QuoteTemplateSectionTypeCode == "QH") this.QuoteTemplateSettingPM.HeaderTableColumn2LabelWidth = value;
            else this.QuoteTemplateSettingPM.DetailsTableColumn2LabelWidth = value;
        }
    }


    get TableColumn2ValueWidth() {
        var tableColumn2ValueWidth: number = 0;
        if (this.QuoteTemplateSettingPM) tableColumn2ValueWidth = this.QuoteTemplateSectionTypeCode == "QH" ? this.QuoteTemplateSettingPM.HeaderTableColumn2ValueWidth : this.QuoteTemplateSettingPM.DetailsTableColumn2ValueWidth;
        return tableColumn2ValueWidth;
    }
    set TableColumn2ValueWidth(value: number) {
        if (this.QuoteTemplateSettingPM != null) {
            if (this.QuoteTemplateSectionTypeCode == "QH") this.QuoteTemplateSettingPM.HeaderTableColumn2ValueWidth = value;
            else this.QuoteTemplateSettingPM.DetailsTableColumn2ValueWidth = value;
        }
    }

  



    CustomQuoteTemplateTextCodePMLists() {

        if (this.QuoteTemplateSectionTypeCode == "QD" && this.QuoteTemplateTextCodePMList) {

            var itemShipingLine: QuoteTemplateTextCodePM = this.QuoteTemplateTextCodePMList.filter(d => d.TextCode == "SHIPINFLINE")[0];
            var itemTruker: QuoteTemplateTextCodePM = this.QuoteTemplateTextCodePMList.filter(d => d.TextCode == "TRUCKER")[0];
            var itemAirLine: QuoteTemplateTextCodePM = this.QuoteTemplateTextCodePMList.filter(d => d.TextCode == "AIRLINE")[0];

            var fromport: QuoteTemplateTextCodePM = this.QuoteTemplateTextCodePMList.filter(d => d.TextCode == "FROMPORT")[0];
            var toport: QuoteTemplateTextCodePM = this.QuoteTemplateTextCodePMList.filter(d => d.TextCode == "TOPORT")[0];

            var fromLocation: QuoteTemplateTextCodePM = this.QuoteTemplateTextCodePMList.filter(d => d.TextCode == "FROMLOCATION")[0];
            var toLocation: QuoteTemplateTextCodePM = this.QuoteTemplateTextCodePMList.filter(d => d.TextCode == "TOLOCATION")[0];



            var NumberOfPackages: QuoteTemplateTextCodePM = this.QuoteTemplateTextCodePMList.filter(d => d.TextCode == "NUMBEROFPACKAGES")[0];
            var NumberOfContainers: QuoteTemplateTextCodePM = this.QuoteTemplateTextCodePMList.filter(d => d.TextCode == "NUMBEROFCONTAINERS")[0];



            if (this.QuotePM != null) {
                if (this.QuotePM.TransportModeName == "Air") {

                    this.QuoteTemplateTextCodePMList = this.QuoteTemplateTextCodePMList.filter(d => d.TextCode != itemShipingLine.TextCode && d.TextCode != itemTruker.TextCode);
                }
                else
                    if (this.QuotePM.TransportModeName == "Ocean") {
     
                        this.QuoteTemplateTextCodePMList = this.QuoteTemplateTextCodePMList.filter(d => d.TextCode != itemAirLine.TextCode && d.TextCode != itemTruker.TextCode);
       
                    }
                    else
                        if (this.QuotePM.TransportModeName == "Inland") {
                            this.QuoteTemplateTextCodePMList = this.QuoteTemplateTextCodePMList.filter(d => d.TextCode != itemAirLine.TextCode && d.TextCode != itemShipingLine.TextCode && d.TextCode != NumberOfPackages.TextCode);
                        }



                if (this.QuotePM.TransportModeId == "A" || this.QuotePM.ShipmentTypeId == "LCL" || this.QuotePM.ShipmentTypeId == "LCLD" || this.QuotePM.ShipmentTypeId == "LTL") {
                    this.QuoteTemplateTextCodePMList = this.QuoteTemplateTextCodePMList.filter(d => d.TextCode != NumberOfContainers.TextCode );
                }
                else if (this.QuotePM.ShipmentTypeId == "FCL" || this.QuotePM.ShipmentTypeId == "FTL" || this.QuotePM.ShipmentTypeId == "FCLD") {
    
                    this.QuoteTemplateTextCodePMList = this.QuoteTemplateTextCodePMList.filter(d => d.TextCode != NumberOfPackages.TextCode);
                }


                if (this.QuotePM.DirectionId == "D") {
                    this.QuoteTemplateTextCodePMList = this.QuoteTemplateTextCodePMList.filter(d => d.TextCode != toport.TextCode && d.TextCode != fromport.TextCode);
          
                }
                else {
                    this.QuoteTemplateTextCodePMList = this.QuoteTemplateTextCodePMList.filter(d => d.TextCode != toLocation.TextCode && d.TextCode != fromLocation.TextCode);

                }


            }
            else {

                this.QuoteTemplateTextCodePMList = this.QuoteTemplateTextCodePMList.filter(d => d.TextCode != itemShipingLine.TextCode && d.TextCode != itemTruker.TextCode && d.TextCode != NumberOfContainers.TextCode && d.TextCode != toport.TextCode && d.TextCode != fromport.TextCode);

            }

        }
    }


    CustomQuoteFieldList() {
        if (this.QuoteTemplateSectionTypeCode == "QD") {

            var QuoteFieldNameString = "Expiration Date, Expiration Days, Shipper Name, Shipper Address, Quote Number, Shipper Contact, Shipper References , Consignee Name, Consignee Address, Consignee Contact, Consignee References, Customer Name, Customer Address, Customer Contact, Customer References, Pickup From, Delivery To, Incoterms, Service, Salesman, Description of goods , Dangerous goods, Chargeable Weight, Gross Weight, Volume, Transit Time, Notify Name, Notify Address, Notify Contact ,Move Type, Departure Frequency"  ;


            var quoteFieldList = QuoteFieldNameString.split(',');


            if (this.QuotePM != null) {
                if (this.QuotePM.TransportModeName == "Air") {
                    quoteFieldList.push("AirLine");

                }
                else
                    if (this.QuotePM.TransportModeName == "Inland") {
                        quoteFieldList.push("ShipingLine");


                    }
                    else
                        if (this.QuotePM.TransportModeName == "Ocean") {
                            quoteFieldList.push("Trucker");

                        }


                if (this.QuotePM.TransportModeId == "A" || this.QuotePM.ShipmentTypeId == "LCL" || this.QuotePM.ShipmentTypeId == "LCLD" || this.QuotePM.ShipmentTypeId == "LTL") {
                    quoteFieldList.push("Number Of Packages");
                }
                else if (this.QuotePM.ShipmentTypeId == "FCL" || this.QuotePM.ShipmentTypeId == "FTL" || this.QuotePM.ShipmentTypeId == "FCLD") {
                    quoteFieldList.push("Number Of Containers");
                }




                if (this.QuotePM.DirectionId == "D") {
                    quoteFieldList.push("From Location");
                    quoteFieldList.push("To Location");

                }
                else {
                    quoteFieldList.push("From Port");
                    quoteFieldList.push("To Port");
                }
            }
            else {
                quoteFieldList.push("AirLine");
                quoteFieldList.push("From Location");
                quoteFieldList.push("To Location");
                quoteFieldList.push("Number Of Packages");
            }

            
            

        }

        else {
            var QuoteFieldNameString = "Quote Date, Expiration Date, Quote Number, Customer, ATTN";
            quoteFieldList = QuoteFieldNameString.split(',');
            
        }

        this.ObjectFieldTextList = [];

        quoteFieldList.forEach((qouteField) => {
            this.ObjectFieldTextList.push(new ObjectFieldText(qouteField, qouteField.trim().replace(" ", "").replace(/\s+/g, '').toUpperCase()));
        });

  
         var table = window.ObjectTables.filter(d => d.Name == "QuoteOP")[0];
         this.ObjectFieldPMList = window.ObjectFields.filter(d => d.ObjectTableId == table.Id && d.IsCustom);
         if (this.ObjectFieldPMList) {
             this.ObjectFieldPMList.forEach((objectFieldPM) => {
                 var defaultText = TextCodeTranslator.Translate(objectFieldPM.FullNameTextCodeCode);
                 this.ObjectFieldTextList.push(new ObjectFieldText(defaultText, objectFieldPM.FullNameTextCodeCode));
             });
         }


        if (this.QuoteTemplateSectionTypeCode == "QD") this.LoadQuoteTemplateDetailsFields();
        else this.LoadQuoteTemplateHeaderFields();
   

    }

    BuildItemsSource() {

        var itemsCollection: TextCodeData[] = [];

        this.QuoteTemplateTextCodePMList.forEach((item) => {
            itemsCollection.push(new TextCodeData(item));
        })
        this.ItemsSource.AppendCollection(itemsCollection);

    }



    LoadData() {
    
        this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator.Translate("QuoteTemplate.M.Loading"));
        this.QuoteTemplateTextDesignPMLists = [];
        this.LoadTableDesign();
        this.CustomQuoteFieldList();
    }


    LoadTextDesign() {

        var ids: string = "";
        if (this.QuoteTemplateSectionTypeCode == "QD") {
            ids = this.QuoteTemplateSettingPM.DetailsTitleDesignId;
        }

        if (this.TableDesignPM) {

            if (!AppTool.IsNullOrEmpty(ids)) ids += ",";
            ids += this.TableDesignPM.HeaderDesignId;
            ids += ("," + this.TableDesignPM.LinesDesignId);
        }

        this.quoteTemplateTextDesignExtendedPMService.GetQuoteTemplateTextDesignPMListByIds(ids, SessionLocator.Tenant).subscribe((res:any) => {
            var pmResponse: ServiceResponse = res;
            this.IsLoadingTextDesign = false;
            this.LoadCompleted();
            if (!pmResponse.HasError && pmResponse.Result) {
                this.QuoteTemplateTextDesignPMLists = pmResponse.Result;


                if (this.TableDesignPM) {
                    this.HeaderTextDesignPM = this.QuoteTemplateTextDesignPMLists.filter(d => d.Id == this.TableDesignPM.HeaderDesignId)[0];
                    if (this.HeaderTextDesignPM) {
                        this.HeaderTextDesignPM.Title = "Label";
                    }

                    this.RowTextDesignPM = this.QuoteTemplateTextDesignPMLists.filter(d => d.Id == this.TableDesignPM.LinesDesignId)[0];
                    if (this.RowTextDesignPM) {
                        this.RowTextDesignPM.Title = "Value";
                       // this.RowTextDesignPM.HideAlignment = true;
                    }
                }
                if (this.QuoteTemplateSectionTypeCode == "QD") {
                    this.TitleTextDesignPM = this.QuoteTemplateTextDesignPMLists.filter(d => d.Id == this.QuoteTemplateSettingPM.DetailsTitleDesignId)[0];
                    if (this.TitleTextDesignPM) {
                        this.TitleTextDesignPM.Title = "Title Design";
                    }
                }
             
                
            }

      
        });


    }


    //Load Table Design

    LoadTableDesign() {

        var tableDesignId: string = this.QuoteTemplateSectionTypeCode == "QD" ? this.QuoteTemplateSettingPM.DetailsTableDesignId : this.QuoteTemplateSettingPM.HeaderTableDesignId;
        this.quoteTemplateTableDesignPMService.get(tableDesignId).subscribe((res:any) => {
            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError && pmResponse.Result) {
                this.TableDesignPM = pmResponse.Result;
            }
            this.LoadTextDesign();

        });
    }

    LoadQuoteTemplateDetailsFields() {
        this.quoteTemplateDetailsFieldExtendedPMService.GetQuoteTemplateDetailsFieldByQuoteTemplateId(this.QuoteTemplatePM.Id, SessionLocator.Tenant).subscribe((res:any) => {
            var pmResponse: ServiceResponse = res;


            this.IsLoadingQuoteField = false;
            this.LoadCompleted();
            this.ObjectFieldTextListColum2 = [];
            this.ObjectFieldTextListColum1 = [];
            this.AllObjectFieldTextList = [];

            if (!pmResponse.HasError && pmResponse.Result) {

                this.QuoteTemplateDetailsFieldPMList = pmResponse.Result;
                this.FillObjectFieldTextColumLists(0, this.QuoteTemplateDetailsFieldPMList,"Details");
                this.FillObjectFieldTextColumLists(1, this.QuoteTemplateDetailsFieldPMList,"Details");
            }

            this.FillAllObjectFieldLists();
        });

    }




    FillAllObjectFieldLists() {
        if (this.ObjectFieldTextList) {
            this.ObjectFieldTextList.forEach((item) => {
                this.AllObjectFieldTextList.push(item);
            });
        }

        this.ObjectFieldTextListColum1.forEach((item) => {
            this.AllObjectFieldTextList.push(item);
        });

        this.ObjectFieldTextListColum2.forEach((item) => {
            this.AllObjectFieldTextList.push(item);
        });
    }
    
    FillObjectFieldTextColumLists(column: number, quoteTemplateFieldPMList: any[] , type:string) {
        quoteTemplateFieldPMList.filter(d => d.Column == column).sort((a, b) => { return a.Row - b.Row }).forEach((item) => {
            var fieldCode: string = item.FieldCode;
            var field: string = type == "Header" ? this.GetFieldNameQuoteHeader(fieldCode) : this.GetFieldNameQuoteDetails(fieldCode);

            if (AppTool.IsNullOrEmpty(field)) field = TextCodeTranslator.Translate(item.FieldCode);
            this.ObjectFieldTextList = this.ObjectFieldTextList.filter(d => d.Code != fieldCode);
            if (item.Column == 0) this.ObjectFieldTextListColum1.push(new ObjectFieldText(field, fieldCode));
            else this.ObjectFieldTextListColum2.push(new ObjectFieldText(field, fieldCode));
        });
    }

    
    LoadCompleted() {

        if (!this.IsLoadingTextDesign && !this.IsLoadingQuoteField) {
            this.CurrentSession.StopBusyIndicator();
            this.IsLoadPage = true;
        }
    }


    LoadQuoteTemplateHeaderFields() {
        this.quoteTemplateHeaderFieldExtendedPMService.GetQuoteTemplateHeaderFieldByQuoteTemplateId(this.QuoteTemplatePM.Id, SessionLocator.Tenant).subscribe((res:any) => {
            var pmResponse: ServiceResponse = res;
            this.IsLoadingQuoteField = false;
            this.LoadCompleted();
            if (!pmResponse.HasError && pmResponse.Result) {
                this.QuoteTemplateHeaderFieldPMList = pmResponse.Result;
                this.FillObjectFieldTextColumLists(0, this.QuoteTemplateHeaderFieldPMList,"Header");
                this.FillObjectFieldTextColumLists(1, this.QuoteTemplateHeaderFieldPMList,"Header");
            }
            this.FillAllObjectFieldLists();

        });
    }
    private GetFieldNameQuoteDetails(fieldname: string) {

        var Field: string = "";

        if (fieldname == "EXPIRATIONDAYS") {
            Field = "Expiration Days";
        }

        else if (fieldname == "EXPIRATIONDATE") {
            Field = "Expiration Date";
        }

        else if (fieldname == "QUOTENUMBER") {
            Field = "Quote Number";
        }

        if (fieldname == "SHIPPERNAME") {
            Field = "Shipper Name";
        }

        else if (fieldname == "SHIPPERADDRESS") {
            Field = "Shipper Address";
        }

        else if (fieldname == "SHIPPERCONTACT") {
            Field = "Shipper Contact";
        }

        else if (fieldname == "SHIPPERREFERENCES") {
            Field = "Shipper References";
        }

        else if (fieldname == "NOTIFYNAME") {
            Field = "Notify Name";
        }

        else if (fieldname == "NOTIFYADDRESS") {
            Field = "Notify Address";
        }

        else if (fieldname == "NOTIFYCONTACT") {
            Field = "Notify Contact";
        }

        if (fieldname == "CONSIGNEENAME") {
            Field = "Consignee Name";
        }

        else if (fieldname == "CONSIGNEEADDRESS") {
            Field = "Consignee Address";
        }

        else if (fieldname == "CONSIGNEECONTACT") {
            Field = "Consignee Contact";
        }

        else if (fieldname == "CONSIGNEEREFERENCES") {
            Field = "Consignee References";
        }
        
        if (fieldname == "CUSTOMERNAME") {
            Field = "Customer Name";
        }

        else if (fieldname == "CUSTOMERADDRESS") {
            Field = "Customer Address";
        }

        else if (fieldname == "CUSTOMERCONTACT") {
            Field = "Customer Contact";
        }

        else if (fieldname == "TRANSITTIME") {
            Field = "Transit Time";
        }     

        else if (fieldname == "MOVETYPE") {
            Field = "Move Type";
        }

        else if (fieldname == "DEPARTUREFREQUENCY") {
            Field = "Departure Frequency";
        }   

       else if (fieldname == "CUSTOMERREFERENCES") {
            Field = "Customer References";
        }
        
        else if (fieldname == "PICKUPFROM") {
            Field = "Pickup From";
        }

        else if (fieldname == "DELIVERYTO") {
            Field = "Delivery To";
        }

        else if (fieldname == "FROMPORT") {
                Field = "From Port";
            }
            else
                if (fieldname == "FROMLOCATION") {
                    Field = "From Location";
                }

                else

                    if (fieldname == "TOLOCATION") {
                        Field = "To Location";
                    }

                    else if (fieldname == "TOPORT") {
                        Field = "To Port";
                    }

                    else if (fieldname == "INCOTERMS") {
                        Field = "Incoterms";
                    }

                    else if (fieldname == "SERVICE") {
                        Field = "Service";
                    }

                    else if (fieldname == "SALESMAN") {
                        Field = "SALESMAN";
                    }

        if (fieldname == "DESCRIPTIONOFGOODS") {
            Field = "Description of goods";
        }

        else if (fieldname == "DANGEROUSGOODS") {
            Field = "Dangerous goods";
        }

        else if (fieldname == "AIRLINE") {
            Field = "Airline";
        }

        else if (fieldname == "SHIPINGLINE") {
            Field = "Shipingline";
        }

        else if (fieldname == "TRUCKER") {
            Field = "Trucker";
        }

        else if (fieldname == "CHARGEABLEWEIGHT") {
            Field = "Chargeable Weight";
        }

        else if (fieldname == "GROSSWEIGHT") {
            Field = "Gross Weight";
        }

        else if (fieldname == "VOLUME") {
            Field = "Volume";
        }

        //else if (fieldname == "VOLUMETRICWEIGHT") {
        //    Field = "Volumetric Weight";
        //}

        else if (fieldname == "NUMBEROFPACKAGES") {
            Field = "Number Of Packages";
        }

        else if (fieldname == "NUMBEROFCONTAINERS") {
            Field = "Number Of Containers";
        }

        return Field;
    }

    private GetFieldNameQuoteHeader(fieldname: string) {


        var Field: string = "";

        if (fieldname == "QUOTENUMBER") {
            Field = "Quote Number";
        }

        else if (fieldname == "QUOTEDATE") {
            Field = "Quote Date";
        }

        else if (fieldname == "EXPIRATIONDATE") {
            Field = "Expiration Date";
        }
        else
            if (fieldname == "CUSTOMER") {
                Field = "Customer";
            }

            else
                if (fieldname == "ATTN") {
                    Field = "ATTN";
                }
        return Field;
    }
    // End Prop setting 
    SaveObjectFieldTextList() {

        this.SaveObjectFieldTextListColum(this.QuoteTemplateSectionTypeCode, 0);
        this.SaveObjectFieldTextListColum(this.QuoteTemplateSectionTypeCode, 1);

        if (this.QuoteTemplateSectionTypeCode == "QH") {
            if (this.QuoteTemplateHeaderFieldPMList) {
                var headerField = this.QuoteTemplateHeaderFieldPMList.filter(d => d.IsDirty)[0];
                if (headerField) this.IsSaveQuoteTemplateObjectField = true;
            }
        }

       else if (this.QuoteTemplateSectionTypeCode == "QD") {
            if (this.QuoteTemplateDetailsFieldPMList) {
                var detailsField = this.QuoteTemplateDetailsFieldPMList.filter(d => d.IsDirty)[0];
                if (detailsField) this.IsSaveQuoteTemplateObjectField = true;
            }
        }

    }

    TotalOfPercentages():number {
        return this.TableColumn1LabelWidth +  this.TableColumn1ValueWidth + this.TableColumn2LabelWidth +  this.TableColumn2ValueWidth;
    }

    SaveButtonClicked() {

        this.SaveObjectFieldTextList();
        
        var quoteTemplateObjectFieldLists = null;

        this.ValidationErrorsList = [];
        var textDesignPmLists = null;
        var textCodeDataLists: TextCodeData[] = null;
        
        if (this.TotalOfPercentages() > 100){
            this.ValidationErrorsList.push("Total of percentages is greater than 100");
        }
    
        if (this.ValidationErrorsList.length == 0) {
        if (this.QuoteTemplateTextDesignPMLists) {
            textDesignPmLists = this.QuoteTemplateTextDesignPMLists.filter(d => d.IsDirty == true);
            if (textDesignPmLists.length > 0) this.IsSaveQuoteTemplateTextDesignRuning = true;
            if (this.TableDesignPM.IsDirty) this.IsSaveQuoteTemplateTableDesignRuning = true;
        }
      

        if (this.ItemsSource) {
             textCodeDataLists = this.ItemsSource.Collection.filter(d => d.EntityPM.IsDirty == true);
            if (textCodeDataLists.length > 0) this.IsSaveQuoteTemplateTextCodeRuning = true;
        }


        if (this.IsSaveQuoteTemplateObjectField){
            if (this.QuoteTemplateSectionTypeCode == "QH") {
                if (this.QuoteTemplateHeaderFieldPMList) {
                    quoteTemplateObjectFieldLists = this.QuoteTemplateHeaderFieldPMList.filter(d => d.IsDirty);
                }
            }
            else if (this.QuoteTemplateSectionTypeCode == "QD") {
                if (this.QuoteTemplateDetailsFieldPMList) {
                    quoteTemplateObjectFieldLists = this.QuoteTemplateDetailsFieldPMList.filter(d => d.IsDirty);
           
                }
            }
        }


        if (this.ObjectFieldTextListColum2 && this.ObjectFieldTextListColum2.length > 0) {
            if (this.QuoteTemplateSectionTypeCode == "QH") {
                this.QuoteTemplateSettingPM.HeaderSectionHasTwoColumns = true;
            }
            else {
                this.QuoteTemplateSettingPM.DetailsSectionHasTwoColumns = true;

            }
        }
        else {

            if (this.QuoteTemplateSectionTypeCode == "QH") {
                this.QuoteTemplateSettingPM.HeaderSectionHasTwoColumns = false;
            }
            else {
                this.QuoteTemplateSettingPM.DetailsSectionHasTwoColumns = false;

            }

        }


        if (this.IsSaveQuoteTemplateTextDesignRuning || this.IsSaveQuoteTemplateTableDesignRuning || this.IsSaveQuoteTemplateTextCodeRuning || this.IsSaveQuoteTemplateObjectField) {

            this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator.Translate("QuoteTemplate.M.Saving"));

            if (this.QuoteTemplateSettingPM.IsDirty) {
                this.quoteTemplateSettingPMService.update(this.QuoteTemplateSettingPM).subscribe((res:any) => {
                    this.QuoteTemplateSettingPM.IsDirty = false;
                    this.SaveOthers(textDesignPmLists, textCodeDataLists, quoteTemplateObjectFieldLists);
                });
            } else this.SaveOthers(textDesignPmLists, textCodeDataLists, quoteTemplateObjectFieldLists);



        } else {

            if (this.QuoteTemplateSettingPM.IsDirty) {
                this.SaveQuoteTemplateSetting();
            }
            else {
                this.CurrentSession.StopBusyIndicator();
                this.CurrentSession.CloseCurrentWindow();
            }



        }



}


    }



   

    SaveObjectFieldTextListColum(type: string, column:number) {

        var objectFieldTextListColum: ObjectFieldText[] = column == 0 ? this.ObjectFieldTextListColum1 : this.ObjectFieldTextListColum2;
        var quoteTemplateFieldPMList: any[] = type == "QH" ? this.QuoteTemplateHeaderFieldPMList : this.QuoteTemplateDetailsFieldPMList;
        

        if (objectFieldTextListColum && objectFieldTextListColum.length > 0) {
            quoteTemplateFieldPMList.forEach((item) => {
                var objectFieldText: ObjectFieldText = objectFieldTextListColum.filter(d => d.Code == item.FieldCode)[0];
                if (objectFieldText == null) {
                    if (item != null) {
                        if (item.Column == column && !item.IsEdit ) item.IsDelete = true;
                    }
                }

            });

            var i: number = -1;
            objectFieldTextListColum.forEach((newItem) => {
                ++i;
                var oldEntity = quoteTemplateFieldPMList.filter(d => d.FieldCode == newItem.Code)[0];
                if (oldEntity != null) {
                    oldEntity.Row = i;
                    oldEntity.Column = column;
                    if (oldEntity.IsDirty) {
                      oldEntity.IsEdit = true
                      oldEntity.IsDelete = false;
                    };
               
                }
                else {

                    var newQuoteTemplateFieldPM: any = type == "QH" ? new QuoteTemplateHeaderFieldPM() : new QuoteTemplateDetailsFieldPM();
                    newQuoteTemplateFieldPM.Tenant = SessionLocator.Tenant;
                    newQuoteTemplateFieldPM.Id = 1 + i.toString();
                    newQuoteTemplateFieldPM.Row = i
                    newQuoteTemplateFieldPM.Column = column;
                    newQuoteTemplateFieldPM.FieldCode = newItem.Code;
                    newQuoteTemplateFieldPM.QuoteTemplateId = this.QuoteTemplatePM.Id,
                    newQuoteTemplateFieldPM.IsAdd = true;
                    quoteTemplateFieldPMList.push(newQuoteTemplateFieldPM);
                }

            });

        }

        else {

            quoteTemplateFieldPMList.forEach((item) => {

                if (item.Column == column && !item.IsEdit) item.IsDelete = true;
            });
        }


    }



    SaveOthers(textDesignPmLists: any[], textCodeDataLists: any[], quoteTemplateObjectFieldLists:any[]) {

        if (this.IsSaveQuoteTemplateTextDesignRuning) this.SaveQuoteTemplateTextDesign(textDesignPmLists);
        if (this.IsSaveQuoteTemplateTableDesignRuning) this.SaveQuoteTemplateTableDesign();
        if (this.IsSaveQuoteTemplateTextCodeRuning) this.SaveQuoteTemplateTextCode(textCodeDataLists);
        if (this.IsSaveQuoteTemplateObjectField) this.SaveQuoteTemplateObjectFieldDB(quoteTemplateObjectFieldLists);
    }


    SaveQuoteTemplateObjectFieldDB(items: any) {

        items.forEach((item) => { item.IsDirty = false; });

        if (this.QuoteTemplateSectionTypeCode == "QH") {
            this.quoteTemplateHeaderFieldExtendedPMService.updateHeaderFields(items).subscribe((res:any) => {
                this.IsSaveQuoteTemplateObjectField = false;
                this.SaveCompleted();

            });
        }

        else {
            this.quoteTemplateDetailsFieldExtendedPMService.updateDetailsFields(items).subscribe((res:any) => {
                this.IsSaveQuoteTemplateObjectField = false;
                this.SaveCompleted();

            });
        }

    }






    SaveQuoteTemplateTextDesign(items: any) {

        items.forEach((item) => { item.IsDirty = false; });


        this.quoteTemplateTextDesignExtendedPMService.updateQuoteTemplateTextDesignPMs(items).subscribe((res:any) => {
            this.IsSaveQuoteTemplateTextDesignRuning = false;
            this.SaveCompleted();

        });

    }

    SaveQuoteTemplateTableDesign() {

        this.quoteTemplateTableDesignPMService.update(this.TableDesignPM).subscribe((res:any) => {
            this.IsSaveQuoteTemplateTableDesignRuning = false;
            this.SaveCompleted();

        });
    }

    SaveQuoteTemplateSetting() {
        this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator.Translate("QuoteTemplate.M.Saving"));
        this.quoteTemplateSettingPMService.update(this.QuoteTemplateSettingPM).subscribe((res:any) => {
            this.QuoteTemplateSettingPM.IsDirty = false;
            this.SaveCompleted();

        });

    }

    SaveQuoteTemplateTextCode(items: TextCodeData[]) {

        var quoteTemplateTextCodePMLists: QuoteTemplateTextCodePM[] = [];
        items.forEach((item) => {
            if (item.EntityPM) {
                item.EntityPM.IsDirty = false;
                quoteTemplateTextCodePMLists.push(item.EntityPM);
            }
        });


        this.quoteTemplateTextCodeExtendedPMService.updateTextCodes(quoteTemplateTextCodePMLists).subscribe((res:any) => {
            this.IsSaveQuoteTemplateTextCodeRuning = false;
            this.SaveCompleted();

        });

    }



    //Drag Drop ObjectFieldTextLis

    OnObjectFieldTextListDragStart(event: DragEvent, item: ObjectFieldText) {
        if (item) {
            event.dataTransfer.setData("Code", item.Code);
        }
    }

    allowDropObjectFieldTextList(event: DragEvent) {
        event.preventDefault();

    }


    DropItemToObjectFieldTextList(event: DragEvent) {

        var code = event.dataTransfer.getData("Code");

        var item: ObjectFieldText = this.AllObjectFieldTextList.filter(d => d.Code == code)[0];
        if (item) {
            this.DeleteItemFromLists(item);

            if (this.ObjectFieldTextList) {
                this.ObjectFieldTextList.push(item);
            }
        }
    }



    //Drag Drop ObjectFieldTextColumn1List

    OnObjectFieldTextColumn1ListDragStart(event: DragEvent, item: ObjectFieldText) {
        if (item) {
            event.dataTransfer.setData("Code", item.Code);
        }
    }



    allowDropObjectFieldTextColumn1List(event: DragEvent) {
        event.preventDefault();

    }


    DropItemToObjectFieldTextColumn1List(event: DragEvent) {


        var code = event.dataTransfer.getData("Code");

        var item: ObjectFieldText = this.AllObjectFieldTextList.filter(d => d.Code == code)[0];
        if (item) {
            this.DeleteItemFromLists(item);

            if (this.ObjectFieldTextListColum1) {
                this.ObjectFieldTextListColum1.push(item);
            }
        }

    }

   
    //Drag Drop ObjectFieldTextColumn2List

    OnObjectFieldTextColumn2ListDragStart(event: DragEvent, item: ObjectFieldText) {
        if (item) {
            event.dataTransfer.setData("Code", item.Code);
        }
    }



    allowDropObjectFieldTextColumn2List(event: DragEvent) {
        event.preventDefault();

    }


    DropItemToObjectFieldTextColumn2List(event: DragEvent) {


        var code = event.dataTransfer.getData("Code");

        var item: ObjectFieldText = this.AllObjectFieldTextList.filter(d => d.Code == code)[0];
        if (item) {
            this.DeleteItemFromLists(item);

            if (this.ObjectFieldTextListColum2) {
                this.ObjectFieldTextListColum2.push(item);
            }
        }

    }

    DeleteItemFromLists(item: ObjectFieldText) {

        if (this.ObjectFieldTextList && this.ObjectFieldTextList.length > 0) {
            var item1 = this.ObjectFieldTextList.filter(d => d.Code == item.Code)[0];
            if (item1) {
                var index = this.ObjectFieldTextList.indexOf(item1);
                if (index != -1) this.ObjectFieldTextList.splice(index, 1);

            }
        }


        if (this.ObjectFieldTextListColum1 && this.ObjectFieldTextListColum1.length > 0) {
            var item2 = this.ObjectFieldTextListColum1.filter(d => d.Code == item.Code)[0];
            if (item2) {
                var index = this.ObjectFieldTextListColum1.indexOf(item2);
                if (index != -1) this.ObjectFieldTextListColum1.splice(index, 1);

            }
        }

        if (this.ObjectFieldTextListColum2 && this.ObjectFieldTextListColum2.length > 0) {
            var item3 = this.ObjectFieldTextListColum2.filter(d => d.Code == item.Code)[0];
            if (item3) {
                var index = this.ObjectFieldTextListColum2.indexOf(item3);
                if (index != -1) this.ObjectFieldTextListColum2.splice(index, 1);

            }
        }

    }


   


    SaveCompleted() {
        if (!this.IsSaveQuoteTemplateTextDesignRuning && !this.IsSaveQuoteTemplateTableDesignRuning && !this.IsSaveQuoteTemplateTextCodeRuning) {
            this.CloseCurrentWindow();

        }

    }

    CloseCurrentWindow() {
        if (this.IsWindowOpened) {
            this.IsWindowOpened = false;
            this.CurrentSession.StopBusyIndicator();
            this.CurrentSession.CurrentWindow.Close("Refresh");
        }
    }

    CloseButtonClicked() {


        this.CurrentSession.CloseCurrentWindow();
    }
}

 class ObjectFieldText {
     public Name: string;
     public Code: string;
     constructor(name: string, code: string) {
         this.Name = name;
         this.Code = code;
     }
  

}
