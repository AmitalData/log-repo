declare var System: any;
declare var window: any;
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {UserLoginLogList} from '../../../../Common/EntityLists/UserLoginLogList';
import {Component, OnInit}  from '@angular/core';
import {UserExtendedPMService} from '../../../../Common/Services/ExtendedPMs/UserExtendedPMService';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {DocumentTypePM} from '../../../../Common/EntityPMs/DocumentTypePM';
import {ObjectTablePM} from '../../../../Infrastructure/EntityPMs/ObjectTablePM';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {ClassLevelValidator} from '../../../../Infrastructure/Validators/ClassLevelValidator';
import {AppTool} from '../../../../Infrastructure/Tools';
import {DocumentTypePMService} from '../../../../Common/Services/StandardPMs/DocumentTypePMService';
import {DocumentTypePMExtendedService} from '../../../../Common/Services/ExtendedPMs/DocumentTypePMExtendedService';
import {SessionInfo} from '../../../../Infrastructure/Utilities/SessionInfo';
import {InfraSettings} from '../../../../Infrastructure/Utilities/InfraSettings';
import {FeatureLocator} from '../../../../Infrastructure/Utilities/FeatureLocator';
import {UIProperty, UIProperties}  from '../../../../Infrastructure/Components/LogitudeComponents/UIProperties';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import { FormGroup, FormBuilder} from '@angular/forms';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { CachedDataManager } from '../../../../Infrastructure/Utilities/CachedDataManager';

declare var insertAtSubject: any;

@Component({
    moduleId: module.id,
    selector: 'NewDocumentType',
    templateUrl: './NewDocumentTypeComponent.html',
    providers: [DocumentTypePMService, DocumentTypePMExtendedService]
})

export class NewDocumentTypeComponent extends BaseComponent implements OnInit {
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    public myForm: FormGroup;
    validator: ClassLevelValidator;
    NewDocumentTypePM: DocumentTypePM = new DocumentTypePM();
    public ObjectTablesList: ObjectTablePM[];
    public FormatList: string[];
    TemplateFormatCode: string = "";
    IsEnableFormat: boolean = false;
    ShowFeildTenant0: boolean = true;
    public IsNewEntityCall: boolean = true;

    SelectedObjectTable: ObjectTablePM;
    private documentTypePMService: DocumentTypePMService;
    constructor(fb: FormBuilder,  public _documentTypePMExtendedService: DocumentTypePMExtendedService) {
        super();
 
        if (this.documentTypePMService == null) {
            this.documentTypePMService = new DocumentTypePMService();

        }

        this.myForm = fb.group({});
        this.validator = new ClassLevelValidator();
    }



    ngOnInit(


    ) {
        this.Run();
    }


    IsShowAdvanceLink: boolean = false;
    RolesAreaVisibility: boolean;
    SetWindowArgs(args: any) {

    }

    Run() {


        this.NewDocumentTypePM.DocumentTypeCategoryCode = "O";
        this.NewDocumentTypePM.Tenant = InfraSettings.TenantPM.Id;
        //this.NewDocumentTypePM.TemplateFormatCode = "P";
        var tempList: ObjectTablePM[] = [];

        window.ObjectTables.forEach(item => {
            switch (item.Name) {
                case "Shipment":
                case "Master":
                case "Quote":
                case "Opportunity":
                case "Ticket":
                case "APInvoice":
                case "ARInvoice":
                case "APPayment":
                case "ARPayment":
                case "Agent":
                case "Customer":
                case "Customs.Declaration":
                case "Customs.CheckRepresentativeType":
                case "LogitudeMessagesTransmissionLog":
                case "SharedLogistics":
                case "ShipmentPickUpDelivery":
                case "Journal":
                case "BankDeposit":
                case "GLAccount":
                case "WarehouseEntry":
                case "PaymentCheque":
                case "WarehouseRelease":
                case "TaxReport":
                case"TaxDeductionReport":
                case "Airline":
                case "CustomAgent":
                case "Participant":
                case "ShippingAgent":
                case "ShippingLine":
                case "Trucker":
                case "Vendor":
                case "Warehouse":
                case "OpenFormatReport":
                {                        
                    if (tempList.filter(f => f.Name == item.Name).length == 0) {
                        tempList.push(item);
                    }

                    break;
                }

            }
        });

        this.ObjectTablesList = tempList.sort((a, b) => { return (a.Name === b.Name) ? 0 : (a.Name < b.Name) ? -1 : 1 });

        //this.ObjectTablesList.forEach((item) => {

        //    if (item.Name == "WarehouseEntry" || item.Name == "WarehouseRelease") {
        //        item.DisplayName = item.Name == "WarehouseEntry" ? "CrossDockEntry" : "CrossDockRelease"; 
        //    }
        //   else item.DisplayName = item.Name;
        //});


        this.SelectedObjectTable = this.ObjectTablesList[0]
        this.NewDocumentTypePM.ObjectTableId = this.SelectedObjectTable.Id;
        this.NewDocumentTypePM.ObjectTableName = this.SelectedObjectTable.Name;
        this.FormatList = [];
        this.FormatList.push("Print");
        this.FormatList.push("Message");
       


        if (FeatureLocator.HasFeaturePermession("DocumentType", "DOCUMENTTYPEPROPERTIES")) this.ShowFeildTenant0 = true;
        else this.ShowFeildTenant0 = false;


    }


    FormatValueChanged(format) {
        if (format == "Print") this.NewDocumentTypePM.TemplateFormatCode = "P";
        else this.NewDocumentTypePM.TemplateFormatCode = "M";

    }






    ExsitCode:string="";
    CodeLostFocusMethod(code: string) {

        if (code && this.ExsitCode != code) {
            this.ExsitCode = code;
            this._documentTypePMExtendedService.GetDoesDocumentTypeCodeExist(code, SessionLocator.Tenant).subscribe(res => {

                var pmResponse: ServiceResponse = res;
                if (!pmResponse.HasError) {
                    var myResult = pmResponse.Result;
                    if (myResult == true) {
                        this.ValidationErrorsList = [];
                        this.ValidationErrorsList.push("The code " + code + " already exists");
                    } else this.ValidationErrorsList = [];
                }



              
            });

    

        }

    }

    ObjectTableValueChanged(table: any) {
        if (table) {
            this.NewDocumentTypePM.ObjectTableName = table.Name;
            this.NewDocumentTypePM.ObjectTableId = table.Id;
            if (table.Name == "Shipment" || table.Name == "Quote"  ) {
                this.IsShowAdvanceLink = true;
            }
            else this.IsShowAdvanceLink = false;
        }
        else {
            this.NewDocumentTypePM.ObjectTableId = null;
            this.SelectedObjectTable = null;
        }
    }



    public IsDocOutChange(isdoc) {

        if (isdoc) this.IsEnableFormat = true;
        else this.IsEnableFormat = false;

    }




    public ValidationErrorsList: string[];
    SaveButtonClicked() {
        
      
             this.ValidationErrorsList = [];
             var errorsArray = this.validator.Validate("DocumentType", this.NewDocumentTypePM);
             if (errorsArray.length > 0) {
                 errorsArray.forEach((item) => {
                     this.ValidationErrorsList.push(item);
                 });
             }


             if (!this.NewDocumentTypePM.IsDocIn && !this.NewDocumentTypePM.IsDocOut) {
                 this.ValidationErrorsList.push("Please chose Doc in or Doc out");
             }

             if (this.NewDocumentTypePM.ObjectTableName == "Shipment" || this.NewDocumentTypePM.ObjectTableName == "Quote") {

                 var valid = ((this.NewDocumentTypePM.IsAir) || (this.NewDocumentTypePM.IsOcean) || (this.NewDocumentTypePM.IsInland));

                 if (!valid) {
                     this.ValidationErrorsList.push("Please choose the transportation method of the document type");
                 }
             }

             if (this.ValidationErrorsList.length == 0) {

                 SessionLocator.CurrentSession.CurrentWindow.StartBusyIndicator("Saving...");

                 this.documentTypePMService.insert(this.NewDocumentTypePM).subscribe(res=> {
                     SessionLocator.CurrentSession.CurrentWindow.StopBusyIndicator();

                     var pmResponse: ServiceResponse = res;
                     if (!pmResponse.HasError) {
                         var myResult = pmResponse.Result;
                         if (myResult) {
                             SessionLocator.CurrentSession.CloseCurrentWindow();
                             CachedDataManager.RefreshTableData("DocumentType", true);
                         }
                     }
                     else {

                         pmResponse.ErrorsArray.forEach((item) => {
                             this.ValidationErrorsList.push(item);
                         });

                     }



                 })
             }

    }

    AdvanceLinkMethod() {


        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Width = 400;
        logitudeWindow.Height = 380;
        logitudeWindow.Title = "Advance";
        logitudeWindow.DataContext = this.NewDocumentTypePM;

        logitudeWindow.Show('./InfrastructureModules/InfrastructureDocuments/Components/DocumentType/AdvanceDocumentTypeComponent');



    }


    InputFileNameId: string = "";
    InputFileNameIdGenerated(id: string) {
        this.InputFileNameId = id;
    }

    AddDataField() {
        if (!AppTool.IsNullOrEmpty(this.NewDocumentTypePM.ObjectTableId)) {


            var tableId: string = "";
            var table = window.ObjectTables.filter(d => d.Id == this.NewDocumentTypePM.ObjectTableId)[0];
            if (table) tableId = table.Id;

            this._entityResourceService.getEntityResourceByTableName(table.Name).subscribe(response => {
                var windowArgs: any = {};
                windowArgs.ObjectTypeField = "DocuemntFileName";
                windowArgs.HideSystemDataTab = true;
                windowArgs.ObjectTableId = tableId;
                var logWindow = new LogitudeWindow();
                logWindow.Width = 500;
                logWindow.Height = 600;
                logWindow.Title = "Insert Data Field";
                logWindow.WindowArgs = windowArgs;
                logWindow.Show('./InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/DocumentObjectFieldsComponent');
                logWindow.WindowClosed.subscribe(($event: any) => {
                    if ($event) {
                        this.NewDocumentTypePM.FileName = insertAtSubject(this.InputFileNameId, $event);

                    }

                });
            });

        }
    }

    CloseButtonClicked() {

       SessionLocator.CurrentSession.CloseCurrentWindow();

    }



    

}


