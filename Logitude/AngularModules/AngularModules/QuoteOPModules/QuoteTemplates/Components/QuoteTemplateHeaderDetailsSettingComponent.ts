declare var System: any;
declare var window: any;
import {Component, OnInit, ViewChild, ViewContainerRef} from '@angular/core';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {QuoteOPTemplatePM} from '../../../QuoteOPM/EntityPMs/QuoteOPTemplatePM';
import {QuoteOPTemplateSettingPM} from '../../../QuoteOPM/EntityPMs/QuoteOPTemplateSettingPM';
import {QuoteOPTemplateTextDesignPM} from '../../../QuoteOPM/EntityPMs/QuoteOPTemplateTextDesignPM';
import {QuoteOPTemplateTableDesignPM} from '../../../QuoteOPM/EntityPMs/QuoteOPTemplateTableDesignPM';
import {QuoteOPTemplateDetailsFieldPM} from '../../../QuoteOPM/EntityPMs/QuoteOPTemplateDetailsFieldPM';
import {QuoteOPTemplateHeaderFieldPM} from '../../../QuoteOPM/EntityPMs/QuoteOPTemplateHeaderFieldPM';
import {BorderType} from '../../../Infrastructure/Components/LogitudeCustomComponents/TextDesignComponent';
import {ObservableCollection} from '../../../Infrastructure/Utilities/ObservableCollection';
import {QuoteOPTemplateTextCodePM} from '../../../QuoteOPM/EntityPMs/QuoteOPTemplateTextCodePM';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {QuoteOPTemplateSettingPMService} from '../../../QuoteOPM/Services/StandardPMs/QuoteOPTemplateSettingPMService';
import {QuoteOPTemplateTableDesignPMService} from '../../../QuoteOPM/Services/StandardPMs/QuoteOPTemplateTableDesignPMService';
import {QuoteOPTemplateTextDesignPMService} from '../../../QuoteOPM/Services/StandardPMs/QuoteOPTemplateTextDesignPMService';
import {QuoteOPTemplateTextDesignExtendedPMService} from '../../../QuoteOPM/Services/ExtendedPMs/QuoteOPTemplateTextDesignExtendedPMService';
import {QuoteOPTemplateTextCodeExtendedPMService} from '../../../QuoteOPM/Services/ExtendedPMs/QuoteOPTemplateTextCodeExtendedPMService';
import {QuoteOPTemplateDetailsFieldExtendedPMService} from '../../../QuoteOPM/Services/ExtendedPMs/QuoteOPTemplateDetailsFieldExtendedPMService';
import {QuoteOPTemplateHeaderFieldExtendedPMService} from '../../../QuoteOPM/Services/ExtendedPMs/QuoteOPTemplateHeaderFieldExtendedPMService';
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
    QuoteOPTemplateSettingPMService: QuoteOPTemplateSettingPMService;
    QuoteOPTemplateDetailsFieldExtendedPMService: QuoteOPTemplateDetailsFieldExtendedPMService;
    QuoteOPTemplateHeaderFieldExtendedPMService: QuoteOPTemplateHeaderFieldExtendedPMService;

    QuoteOPTemplateTextDesignPMService: QuoteOPTemplateTextDesignPMService;
    QuoteOPTemplateTableDesignPMService: QuoteOPTemplateTableDesignPMService;
    QuoteOPTemplateTextDesignExtendedPMService: QuoteOPTemplateTextDesignExtendedPMService;
    QuoteOPTemplateTextCodeExtendedPMService: QuoteOPTemplateTextCodeExtendedPMService;
    QuoteOPTemplatePM: QuoteOPTemplatePM;
    IsLoadPage: boolean;

    Alignment: string[] = [];
    QuoteOPTemplateSettingPM: QuoteOPTemplateSettingPM;

    public ValidationErrorsList: string[];

    TableDesignPM: QuoteOPTemplateTableDesignPM;
    HeaderTextDesignPM: QuoteOPTemplateTextDesignPM;
    RowTextDesignPM: QuoteOPTemplateTextDesignPM;
    TitleTextDesignPM: QuoteOPTemplateTextDesignPM;
    IsLoadingTextDesign: boolean = true;
    IsLoadingQuoteField: boolean = true;
    QuoteOPTemplateDetailsFieldPMList: QuoteOPTemplateDetailsFieldPM[] = [];
    QuoteOPTemplateHeaderFieldPMList: QuoteOPTemplateHeaderFieldPM[] = [];


    QuoteOPTemplateTextDesignPMLists: QuoteOPTemplateTextDesignPM[] = [];
    QuoteOPTemplateTextCodePMList: QuoteOPTemplateTextCodePM[] = [];
    ObjectFieldTextList: ObjectFieldText[] = [];
    ObjectFieldTextListColum1: ObjectFieldText[] = [];
    ObjectFieldTextListColum2: ObjectFieldText[] = []; 

    AllObjectFieldTextList: ObjectFieldText[] = [];
    ObjectFieldTextListColum2ListSelected: ObjectFieldText;
    ObjectFieldTextListColum1ListSelected: ObjectFieldText;
    ObjectFieldTextListSelected: ObjectFieldText;
    ObjectFieldPMList: ObjectFieldPM[] = [];
    IsWindowOpened: boolean = true;

    IsSaveQuoteOPTemplateTextDesignRuning: boolean = false;
    IsSaveQuoteOPTemplateTableDesignRuning: boolean = false;
    IsSaveQuoteOPTemplateTextCodeRuning: boolean = false;
    IsSaveQuoteOPTemplateObjectField: boolean = false;
    BorderTypesSelected: BorderType;
    BorderTypes: BorderType[] = [];
    public ItemsSource: ObservableCollection;
    QuoteOPPM: any;
    QuoteOPTemplateSectionTypeName: string = "QuoteHeader";
    QuoteOPTemplateSectionTypeCode: string = "QH";
    @ViewChild('Child', { read: ViewContainerRef, static: false }) viewContainerRef: ViewContainerRef;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.QuoteOPTemplateSettingPMService = new QuoteOPTemplateSettingPMService();
        this.QuoteOPTemplateTableDesignPMService = new QuoteOPTemplateTableDesignPMService();
        this.QuoteOPTemplateTextDesignPMService = new QuoteOPTemplateTextDesignPMService();
        this.QuoteOPTemplateTextDesignExtendedPMService = new QuoteOPTemplateTextDesignExtendedPMService();
        this.QuoteOPTemplateTextCodeExtendedPMService = new QuoteOPTemplateTextCodeExtendedPMService();
        this.QuoteOPTemplateDetailsFieldExtendedPMService = new QuoteOPTemplateDetailsFieldExtendedPMService();
        this.QuoteOPTemplateHeaderFieldExtendedPMService = new QuoteOPTemplateHeaderFieldExtendedPMService();

        this.ItemsSource = new ObservableCollection([]);
    }

    ngOnInit() {

    }

    SelectedTabCode: string;
    SetWindowArgs(args: any) {
        this.SelectedTabCode = "TAC";
        this.QuoteOPTemplatePM = args.QuoteOPTemplatePM;
        this.QuoteOPTemplateSectionTypeName = args.QuoteOPTemplateSectionTypeName;
        this.QuoteOPTemplateSectionTypeCode = args.QuoteOPTemplateSectionTypeCode;
        this.QuoteOPTemplateSettingPM = args.QuoteOPTemplateSettingPM;
        this.QuoteOPPM = args.QuoteOPPM;


        this.QuoteOPTemplateDetailsFieldPMList = [];
        this.QuoteOPTemplateHeaderFieldPMList = [];
        this.QuoteOPTemplateTextDesignPMLists = [];
        this.QuoteOPTemplateTextCodePMList = [];
        this.ObjectFieldTextList = [];
        this.ObjectFieldTextListColum1= [];
        this.ObjectFieldTextListColum2 = []; 


        this.BorderTypes = [];
        this.BorderTypes.push(new BorderType("Auto", "AUTO"));
        this.BorderTypes.push(new BorderType("Fixed", "FIXED"));


        var selectedBorderCode = this.QuoteOPTemplateSectionTypeCode == "QH" ? this.QuoteOPTemplateSettingPM.HeaderTableColumWidthType : this.QuoteOPTemplateSettingPM.DetailsTableColumWidthType;


        this.BorderTypesSelected = this.BorderTypes.filter(d => d.Code == selectedBorderCode)[0];


        this.Alignment.push("Left"); this.Alignment.push("Center"); this.Alignment.push("Right");

        if (args.QuoteOPTemplateTextCodePMList) {
            this.QuoteOPTemplateTextCodePMList = args.QuoteOPTemplateTextCodePMList.filter(d => d.Area == this.QuoteOPTemplateSectionTypeName);
            this.CustomQuoteOPTemplateTextCodePMLists();
            this.BuildItemsSource();
        }

        this.LoadData();
    }



    BorderTypesSelectedChanged(border: BorderType) {
        if (this.QuoteOPTemplateSettingPM) {

            if (this.QuoteOPTemplateSectionTypeCode == "QH") {
                if (this.QuoteOPTemplateSettingPM.HeaderTableColumWidthType != border.Code) {
                    this.QuoteOPTemplateSettingPM.HeaderTableColumWidthType = border.Code;
               
                }
            } else {

                if (this.QuoteOPTemplateSettingPM.DetailsTableColumWidthType != border.Code) {
                    this.QuoteOPTemplateSettingPM.DetailsTableColumWidthType = border.Code;
          
                }

            }
        }
    }

    //Prop setting 
    ShowTitleQuoteDetailsKey: string = Guid.newGuid();
    get ShowTitleQuoteDetails() {
        var showTitleQuoteDetails: boolean = false;
        if (this.QuoteOPTemplateSettingPM) showTitleQuoteDetails = this.QuoteOPTemplateSettingPM.ShowTitleQuoteDetails;
        return showTitleQuoteDetails;
    }
    set ShowTitleQuoteDetails(value: boolean) {
        if (this.QuoteOPTemplateSettingPM != null) {
            this.QuoteOPTemplateSettingPM.ShowTitleQuoteDetails = value;
        }
    }

    get TableColumn1LabelWidth() {
        var tableColumn1LabelWidth: number = 0;
        if (this.QuoteOPTemplateSettingPM) tableColumn1LabelWidth = this.QuoteOPTemplateSectionTypeCode == "QH" ? this.QuoteOPTemplateSettingPM.HeaderTableColumn1LabelWidth : this.QuoteOPTemplateSettingPM.DetailsTableColumn1LabelWidth;
        return tableColumn1LabelWidth;
    }
    set TableColumn1LabelWidth(value: number) {
        if (this.QuoteOPTemplateSettingPM != null) {
            if (this.QuoteOPTemplateSectionTypeCode == "QH") this.QuoteOPTemplateSettingPM.HeaderTableColumn1LabelWidth = value;
            else this.QuoteOPTemplateSettingPM.DetailsTableColumn1LabelWidth = value;
        }
    }


    get TableColumn1ValueWidth() {
        var tableColumn1ValueWidth: number = 0;
        if (this.QuoteOPTemplateSettingPM) tableColumn1ValueWidth = this.QuoteOPTemplateSectionTypeCode == "QH" ? this.QuoteOPTemplateSettingPM.HeaderTableColumn1ValueWidth : this.QuoteOPTemplateSettingPM.DetailsTableColumn1ValueWidth;
        return tableColumn1ValueWidth;
    }
    set TableColumn1ValueWidth(value: number) {
        if (this.QuoteOPTemplateSettingPM != null) {
            if (this.QuoteOPTemplateSectionTypeCode == "QH") this.QuoteOPTemplateSettingPM.HeaderTableColumn1ValueWidth = value;
            else this.QuoteOPTemplateSettingPM.DetailsTableColumn1ValueWidth = value;
        }
    }



    get TableColumn2LabelWidth() {
        var tableColumn2LabelWidth: number = 0;
        if (this.QuoteOPTemplateSettingPM) tableColumn2LabelWidth = this.QuoteOPTemplateSectionTypeCode == "QH" ? this.QuoteOPTemplateSettingPM.HeaderTableColumn2LabelWidth : this.QuoteOPTemplateSettingPM.DetailsTableColumn2LabelWidth;
        return tableColumn2LabelWidth;
    }
    set TableColumn2LabelWidth(value: number) {
        if (this.QuoteOPTemplateSettingPM != null) {
            if (this.QuoteOPTemplateSectionTypeCode == "QH") this.QuoteOPTemplateSettingPM.HeaderTableColumn2LabelWidth = value;
            else this.QuoteOPTemplateSettingPM.DetailsTableColumn2LabelWidth = value;
        }
    }


    get TableColumn2ValueWidth() {
        var tableColumn2ValueWidth: number = 0;
        if (this.QuoteOPTemplateSettingPM) tableColumn2ValueWidth = this.QuoteOPTemplateSectionTypeCode == "QH" ? this.QuoteOPTemplateSettingPM.HeaderTableColumn2ValueWidth : this.QuoteOPTemplateSettingPM.DetailsTableColumn2ValueWidth;
        return tableColumn2ValueWidth;
    }
    set TableColumn2ValueWidth(value: number) {
        if (this.QuoteOPTemplateSettingPM != null) {
            if (this.QuoteOPTemplateSectionTypeCode == "QH") this.QuoteOPTemplateSettingPM.HeaderTableColumn2ValueWidth = value;
            else this.QuoteOPTemplateSettingPM.DetailsTableColumn2ValueWidth = value;
        }
    }

  



    CustomQuoteOPTemplateTextCodePMLists() {

        if (this.QuoteOPTemplateSectionTypeCode == "QD" && this.QuoteOPTemplateTextCodePMList) {

            var itemShipingLine: QuoteOPTemplateTextCodePM = this.QuoteOPTemplateTextCodePMList.filter(d => d.TextCode == "SHIPINFLINE")[0];
            var itemTruker: QuoteOPTemplateTextCodePM = this.QuoteOPTemplateTextCodePMList.filter(d => d.TextCode == "TRUCKER")[0];
            var itemAirLine: QuoteOPTemplateTextCodePM = this.QuoteOPTemplateTextCodePMList.filter(d => d.TextCode == "AIRLINE")[0];

            var fromport: QuoteOPTemplateTextCodePM = this.QuoteOPTemplateTextCodePMList.filter(d => d.TextCode == "FROMPORT")[0];
            var toport: QuoteOPTemplateTextCodePM = this.QuoteOPTemplateTextCodePMList.filter(d => d.TextCode == "TOPORT")[0];

            var fromLocation: QuoteOPTemplateTextCodePM = this.QuoteOPTemplateTextCodePMList.filter(d => d.TextCode == "FROMLOCATION")[0];
            var toLocation: QuoteOPTemplateTextCodePM = this.QuoteOPTemplateTextCodePMList.filter(d => d.TextCode == "TOLOCATION")[0];



            var NumberOfPackages: QuoteOPTemplateTextCodePM = this.QuoteOPTemplateTextCodePMList.filter(d => d.TextCode == "NUMBEROFPACKAGES")[0];
            var NumberOfContainers: QuoteOPTemplateTextCodePM = this.QuoteOPTemplateTextCodePMList.filter(d => d.TextCode == "NUMBEROFCONTAINERS")[0];



            if (this.QuoteOPPM != null) {
                if (this.QuoteOPPM.TransportModeName == "Air") {

                    this.QuoteOPTemplateTextCodePMList = this.QuoteOPTemplateTextCodePMList.filter(d => d.TextCode != itemShipingLine.TextCode && d.TextCode != itemTruker.TextCode);
                }
                else
                    if (this.QuoteOPPM.TransportModeName == "Ocean") {
     
                        this.QuoteOPTemplateTextCodePMList = this.QuoteOPTemplateTextCodePMList.filter(d => d.TextCode != itemAirLine.TextCode && d.TextCode != itemTruker.TextCode);
       
                    }
                    else
                        if (this.QuoteOPPM.TransportModeName == "Inland") {
                            this.QuoteOPTemplateTextCodePMList = this.QuoteOPTemplateTextCodePMList.filter(d => d.TextCode != itemAirLine.TextCode && d.TextCode != itemShipingLine.TextCode && d.TextCode != NumberOfPackages.TextCode);
                        }



                if (this.QuoteOPPM.TransportModeId == "A" || this.QuoteOPPM.ShipmentTypeId == "LCL" || this.QuoteOPPM.ShipmentTypeId == "LCLD" || this.QuoteOPPM.ShipmentTypeId == "LTL") {
                    this.QuoteOPTemplateTextCodePMList = this.QuoteOPTemplateTextCodePMList.filter(d => d.TextCode != NumberOfContainers.TextCode );
                }
                else if (this.QuoteOPPM.ShipmentTypeId == "FCL" || this.QuoteOPPM.ShipmentTypeId == "FTL" || this.QuoteOPPM.ShipmentTypeId == "FCLD") {
    
                    this.QuoteOPTemplateTextCodePMList = this.QuoteOPTemplateTextCodePMList.filter(d => d.TextCode != NumberOfPackages.TextCode);
                }


                if (this.QuoteOPPM.DirectionId == "D") {
                    this.QuoteOPTemplateTextCodePMList = this.QuoteOPTemplateTextCodePMList.filter(d => d.TextCode != toport.TextCode && d.TextCode != fromport.TextCode);
          
                }
                else {
                    this.QuoteOPTemplateTextCodePMList = this.QuoteOPTemplateTextCodePMList.filter(d => d.TextCode != toLocation.TextCode && d.TextCode != fromLocation.TextCode);

                }


            }
            else {

                this.QuoteOPTemplateTextCodePMList = this.QuoteOPTemplateTextCodePMList.filter(d => d.TextCode != itemShipingLine.TextCode && d.TextCode != itemTruker.TextCode && d.TextCode != NumberOfContainers.TextCode && d.TextCode != toport.TextCode && d.TextCode != fromport.TextCode);

            }

        }
    }


    CustomQuoteFieldList() {
        if (this.QuoteOPTemplateSectionTypeCode == "QD") {

            var QuoteFieldNameString = "Expiration Date, Expiration Days, Shipper Name, Shipper Address, Quote Number, Shipper Contact, Shipper References , Consignee Name, Consignee Address, Consignee Contact, Consignee References, Customer Name, Customer Address, Customer Contact, Customer References, Pickup From, Delivery To, Incoterms, Service, Salesman, Description of goods , Dangerous goods, Chargeable Weight, Gross Weight, Volume, Transit Time, Notify Name, Notify Address, Notify Contact ,Move Type, Departure Frequency"  ;


            var quoteFieldList = QuoteFieldNameString.split(',');


            if (this.QuoteOPPM != null) {
                if (this.QuoteOPPM.TransportModeName == "Air") {
                    quoteFieldList.push("AirLine");

                }
                else
                    if (this.QuoteOPPM.TransportModeName == "Inland") {
                        quoteFieldList.push("ShipingLine");


                    }
                    else
                        if (this.QuoteOPPM.TransportModeName == "Ocean") {
                            quoteFieldList.push("Trucker");

                        }


                if (this.QuoteOPPM.TransportModeId == "A" || this.QuoteOPPM.ShipmentTypeId == "LCL" || this.QuoteOPPM.ShipmentTypeId == "LCLD" || this.QuoteOPPM.ShipmentTypeId == "LTL") {
                    quoteFieldList.push("Number Of Packages");
                }
                else if (this.QuoteOPPM.ShipmentTypeId == "FCL" || this.QuoteOPPM.ShipmentTypeId == "FTL" || this.QuoteOPPM.ShipmentTypeId == "FCLD") {
                    quoteFieldList.push("Number Of Containers");
                }




                if (this.QuoteOPPM.DirectionId == "D") {
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


        if (this.QuoteOPTemplateSectionTypeCode == "QD") this.LoadQuoteOPTemplateDetailsFields();
        else this.LoadQuoteOPTemplateHeaderFields();
   

    }

    BuildItemsSource() {

        var itemsCollection: TextCodeData[] = [];

        this.QuoteOPTemplateTextCodePMList.forEach((item) => {
            itemsCollection.push(new TextCodeData(item));
        })
        this.ItemsSource.AppendCollection(itemsCollection);

    }



    LoadData() {
    
        this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator.Translate("QuoteOPTemplate.M.Loading"));
        this.QuoteOPTemplateTextDesignPMLists = [];
        this.LoadTableDesign();
        this.CustomQuoteFieldList();
    }


    LoadTextDesign() {

        var ids: string = "";
        if (this.QuoteOPTemplateSectionTypeCode == "QD") {
            ids = this.QuoteOPTemplateSettingPM.DetailsTitleDesignId;
        }

        if (this.TableDesignPM) {

            if (!AppTool.IsNullOrEmpty(ids)) ids += ",";
            ids += this.TableDesignPM.HeaderDesignId;
            ids += ("," + this.TableDesignPM.LinesDesignId);
        }

        this.QuoteOPTemplateTextDesignExtendedPMService.GetQuoteOPTemplateTextDesignPMListByIds(ids, SessionLocator.Tenant).subscribe((res:any) => {
            var pmResponse: ServiceResponse = res;
            this.IsLoadingTextDesign = false;
            this.LoadCompleted();
            if (!pmResponse.HasError && pmResponse.Result) {
                this.QuoteOPTemplateTextDesignPMLists = pmResponse.Result;


                if (this.TableDesignPM) {
                    this.HeaderTextDesignPM = this.QuoteOPTemplateTextDesignPMLists.filter(d => d.Id == this.TableDesignPM.HeaderDesignId)[0];
                    if (this.HeaderTextDesignPM) {
                        this.HeaderTextDesignPM.Title = "Label";
                    }

                    this.RowTextDesignPM = this.QuoteOPTemplateTextDesignPMLists.filter(d => d.Id == this.TableDesignPM.LinesDesignId)[0];
                    if (this.RowTextDesignPM) {
                        this.RowTextDesignPM.Title = "Value";
                       // this.RowTextDesignPM.HideAlignment = true;
                    }
                }
                if (this.QuoteOPTemplateSectionTypeCode == "QD") {
                    this.TitleTextDesignPM = this.QuoteOPTemplateTextDesignPMLists.filter(d => d.Id == this.QuoteOPTemplateSettingPM.DetailsTitleDesignId)[0];
                    if (this.TitleTextDesignPM) {
                        this.TitleTextDesignPM.Title = "Title Design";
                    }
                }
             
                
            }

      
        });


    }


    //Load Table Design

    LoadTableDesign() {

        var tableDesignId: string = this.QuoteOPTemplateSectionTypeCode == "QD" ? this.QuoteOPTemplateSettingPM.DetailsTableDesignId : this.QuoteOPTemplateSettingPM.HeaderTableDesignId;
        this.QuoteOPTemplateTableDesignPMService.get(tableDesignId).subscribe((res:any) => {
            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError && pmResponse.Result) {
                this.TableDesignPM = pmResponse.Result;
            }
            this.LoadTextDesign();

        });
    }

    LoadQuoteOPTemplateDetailsFields() {
        this.QuoteOPTemplateDetailsFieldExtendedPMService.GetQuoteTemplateDetailsFieldByQuoteTemplateId(this.QuoteOPTemplatePM.Id, SessionLocator.Tenant).subscribe((res:any) => {
            var pmResponse: ServiceResponse = res;


            this.IsLoadingQuoteField = false;
            this.LoadCompleted();
            this.ObjectFieldTextListColum2 = [];
            this.ObjectFieldTextListColum1 = [];
            this.AllObjectFieldTextList = [];

            if (!pmResponse.HasError && pmResponse.Result) {

                this.QuoteOPTemplateDetailsFieldPMList = pmResponse.Result;
                this.FillObjectFieldTextColumLists(0, this.QuoteOPTemplateDetailsFieldPMList,"Details");
                this.FillObjectFieldTextColumLists(1, this.QuoteOPTemplateDetailsFieldPMList,"Details");
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
    
    FillObjectFieldTextColumLists(column: number, QuoteOPTemplateFieldPMList: any[] , type:string) {
        QuoteOPTemplateFieldPMList.filter(d => d.Column == column).sort((a, b) => { return a.Row - b.Row }).forEach((item) => {
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


    LoadQuoteOPTemplateHeaderFields() {
        this.QuoteOPTemplateHeaderFieldExtendedPMService.GetQuoteOPTemplateHeaderFieldByQuoteOPTemplateId(this.QuoteOPTemplatePM.Id, SessionLocator.Tenant).subscribe((res:any) => {
            var pmResponse: ServiceResponse = res;
            this.IsLoadingQuoteField = false;
            this.LoadCompleted();
            if (!pmResponse.HasError && pmResponse.Result) {
                this.QuoteOPTemplateHeaderFieldPMList = pmResponse.Result;
                this.FillObjectFieldTextColumLists(0, this.QuoteOPTemplateHeaderFieldPMList,"Header");
                this.FillObjectFieldTextColumLists(1, this.QuoteOPTemplateHeaderFieldPMList,"Header");
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

        this.SaveObjectFieldTextListColum(this.QuoteOPTemplateSectionTypeCode, 0);
        this.SaveObjectFieldTextListColum(this.QuoteOPTemplateSectionTypeCode, 1);

        if (this.QuoteOPTemplateSectionTypeCode == "QH") {
            if (this.QuoteOPTemplateHeaderFieldPMList) {
                var headerField = this.QuoteOPTemplateHeaderFieldPMList.filter(d => d.IsDirty)[0];
                if (headerField) this.IsSaveQuoteOPTemplateObjectField = true;
            }
        }

       else if (this.QuoteOPTemplateSectionTypeCode == "QD") {
            if (this.QuoteOPTemplateDetailsFieldPMList) {
                var detailsField = this.QuoteOPTemplateDetailsFieldPMList.filter(d => d.IsDirty)[0];
                if (detailsField) this.IsSaveQuoteOPTemplateObjectField = true;
            }
        }

    }

    TotalOfPercentages():number {
        return this.TableColumn1LabelWidth +  this.TableColumn1ValueWidth + this.TableColumn2LabelWidth +  this.TableColumn2ValueWidth;
    }

    SaveButtonClicked() {

        this.SaveObjectFieldTextList();
        
        var QuoteOPTemplateObjectFieldLists = null;

        this.ValidationErrorsList = [];
        var textDesignPmLists = null;
        var textCodeDataLists: TextCodeData[] = null;
        
        if (this.TotalOfPercentages() > 100){
            this.ValidationErrorsList.push("Total of percentages is greater than 100");
        }
    
        if (this.ValidationErrorsList.length == 0) {
        if (this.QuoteOPTemplateTextDesignPMLists) {
            textDesignPmLists = this.QuoteOPTemplateTextDesignPMLists.filter(d => d.IsDirty == true);
            if (textDesignPmLists.length > 0) this.IsSaveQuoteOPTemplateTextDesignRuning = true;
            if (this.TableDesignPM.IsDirty) this.IsSaveQuoteOPTemplateTableDesignRuning = true;
        }
      

        if (this.ItemsSource) {
             textCodeDataLists = this.ItemsSource.Collection.filter(d => d.EntityPM.IsDirty == true);
            if (textCodeDataLists.length > 0) this.IsSaveQuoteOPTemplateTextCodeRuning = true;
        }


        if (this.IsSaveQuoteOPTemplateObjectField){
            if (this.QuoteOPTemplateSectionTypeCode == "QH") {
                if (this.QuoteOPTemplateHeaderFieldPMList) {
                    QuoteOPTemplateObjectFieldLists = this.QuoteOPTemplateHeaderFieldPMList.filter(d => d.IsDirty);
                }
            }
            else if (this.QuoteOPTemplateSectionTypeCode == "QD") {
                if (this.QuoteOPTemplateDetailsFieldPMList) {
                    QuoteOPTemplateObjectFieldLists = this.QuoteOPTemplateDetailsFieldPMList.filter(d => d.IsDirty);
           
                }
            }
        }


        if (this.ObjectFieldTextListColum2 && this.ObjectFieldTextListColum2.length > 0) {
            if (this.QuoteOPTemplateSectionTypeCode == "QH") {
                this.QuoteOPTemplateSettingPM.HeaderSectionHasTwoColumns = true;
            }
            else {
                this.QuoteOPTemplateSettingPM.DetailsSectionHasTwoColumns = true;

            }
        }
        else {

            if (this.QuoteOPTemplateSectionTypeCode == "QH") {
                this.QuoteOPTemplateSettingPM.HeaderSectionHasTwoColumns = false;
            }
            else {
                this.QuoteOPTemplateSettingPM.DetailsSectionHasTwoColumns = false;

            }

        }


        if (this.IsSaveQuoteOPTemplateTextDesignRuning || this.IsSaveQuoteOPTemplateTableDesignRuning || this.IsSaveQuoteOPTemplateTextCodeRuning || this.IsSaveQuoteOPTemplateObjectField) {

            this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator.Translate("QuoteOPTemplate.M.Saving"));

            if (this.QuoteOPTemplateSettingPM.IsDirty) {
                this.QuoteOPTemplateSettingPMService.update(this.QuoteOPTemplateSettingPM).subscribe((res:any) => {
                    this.QuoteOPTemplateSettingPM.IsDirty = false;
                    this.SaveOthers(textDesignPmLists, textCodeDataLists, QuoteOPTemplateObjectFieldLists);
                });
            } else this.SaveOthers(textDesignPmLists, textCodeDataLists, QuoteOPTemplateObjectFieldLists);



        } else {

            if (this.QuoteOPTemplateSettingPM.IsDirty) {
                this.SaveQuoteOPTemplateSetting();
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
        var QuoteOPTemplateFieldPMList: any[] = type == "QH" ? this.QuoteOPTemplateHeaderFieldPMList : this.QuoteOPTemplateDetailsFieldPMList;
        

        if (objectFieldTextListColum && objectFieldTextListColum.length > 0) {
            QuoteOPTemplateFieldPMList.forEach((item) => {
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
                var oldEntity = QuoteOPTemplateFieldPMList.filter(d => d.FieldCode == newItem.Code)[0];
                if (oldEntity != null) {
                    oldEntity.Row = i;
                    oldEntity.Column = column;
                    if (oldEntity.IsDirty) {
                      oldEntity.IsEdit = true
                      oldEntity.IsDelete = false;
                    };
               
                }
                else {

                    var newQuoteOPTemplateFieldPM: any = type == "QH" ? new QuoteOPTemplateHeaderFieldPM() : new QuoteOPTemplateDetailsFieldPM();
                    newQuoteOPTemplateFieldPM.Tenant = SessionLocator.Tenant;
                    newQuoteOPTemplateFieldPM.Id = 1 + i.toString();
                    newQuoteOPTemplateFieldPM.Row = i
                    newQuoteOPTemplateFieldPM.Column = column;
                    newQuoteOPTemplateFieldPM.FieldCode = newItem.Code;
                    newQuoteOPTemplateFieldPM.QuoteTemplateId = this.QuoteOPTemplatePM.Id,
                    newQuoteOPTemplateFieldPM.IsAdd = true;
                    QuoteOPTemplateFieldPMList.push(newQuoteOPTemplateFieldPM);
                }

            });

        }

        else {

            QuoteOPTemplateFieldPMList.forEach((item) => {

                if (item.Column == column && !item.IsEdit) item.IsDelete = true;
            });
        }


    }



    SaveOthers(textDesignPmLists: any[], textCodeDataLists: any[], QuoteOPTemplateObjectFieldLists:any[]) {

        if (this.IsSaveQuoteOPTemplateTextDesignRuning) this.SaveQuoteOPTemplateTextDesign(textDesignPmLists);
        if (this.IsSaveQuoteOPTemplateTableDesignRuning) this.SaveQuoteOPTemplateTableDesign();
        if (this.IsSaveQuoteOPTemplateTextCodeRuning) this.SaveQuoteOPTemplateTextCode(textCodeDataLists);
        if (this.IsSaveQuoteOPTemplateObjectField) this.SaveQuoteOPTemplateObjectFieldDB(QuoteOPTemplateObjectFieldLists);
    }


    SaveQuoteOPTemplateObjectFieldDB(items: any) {

        items.forEach((item) => { item.IsDirty = false; });

        if (this.QuoteOPTemplateSectionTypeCode == "QH") {
            this.QuoteOPTemplateHeaderFieldExtendedPMService.updateHeaderFields(items).subscribe((res:any) => {
                this.IsSaveQuoteOPTemplateObjectField = false;
                this.SaveCompleted();

            });
        }

        else {
            this.QuoteOPTemplateDetailsFieldExtendedPMService.updateDetailsFields(items).subscribe((res:any) => {
                this.IsSaveQuoteOPTemplateObjectField = false;
                this.SaveCompleted();

            });
        }

    }






    SaveQuoteOPTemplateTextDesign(items: any) {

        items.forEach((item) => { item.IsDirty = false; });


        this.QuoteOPTemplateTextDesignExtendedPMService.updateQuoteOPTemplateTextDesignPMs(items).subscribe((res:any) => {
            this.IsSaveQuoteOPTemplateTextDesignRuning = false;
            this.SaveCompleted();

        });

    }

    SaveQuoteOPTemplateTableDesign() {

        this.QuoteOPTemplateTableDesignPMService.update(this.TableDesignPM).subscribe((res:any) => {
            this.IsSaveQuoteOPTemplateTableDesignRuning = false;
            this.SaveCompleted();

        });
    }

    SaveQuoteOPTemplateSetting() {
        this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator.Translate("QuoteOPTemplate.M.Saving"));
        this.QuoteOPTemplateSettingPMService.update(this.QuoteOPTemplateSettingPM).subscribe((res:any) => {
            this.QuoteOPTemplateSettingPM.IsDirty = false;
            this.SaveCompleted();

        });

    }

    SaveQuoteOPTemplateTextCode(items: TextCodeData[]) {

        var QuoteOPTemplateTextCodePMLists: QuoteOPTemplateTextCodePM[] = [];
        items.forEach((item) => {
            if (item.EntityPM) {
                item.EntityPM.IsDirty = false;
                QuoteOPTemplateTextCodePMLists.push(item.EntityPM);
            }
        });


        this.QuoteOPTemplateTextCodeExtendedPMService.updateTextCodes(QuoteOPTemplateTextCodePMLists).subscribe((res:any) => {
            this.IsSaveQuoteOPTemplateTextCodeRuning = false;
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
        if (!this.IsSaveQuoteOPTemplateTextDesignRuning && !this.IsSaveQuoteOPTemplateTableDesignRuning && !this.IsSaveQuoteOPTemplateTextCodeRuning) {
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
