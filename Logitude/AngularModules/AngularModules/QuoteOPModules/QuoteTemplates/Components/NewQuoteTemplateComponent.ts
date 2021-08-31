import {Component, OnInit, ViewChild, ViewContainerRef} from '@angular/core';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {QuoteOPTemplatePM} from '../../../QuoteOPM/EntityPMs/QuoteOPTemplatePM';
import {QuoteOPTemplateList} from '../../../QuoteOPM/EntityLists/QuoteOPTemplateList';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';
import {AppTool, DateTool} from '../../../Infrastructure/Tools';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import {CitySelectionArgs} from '../../../Common/Args';
import {Validator} from '../../../Infrastructure/Validators/Validator';
import {NewEntityArgs} from '../../../Infrastructure/Args';
import {EntityResourceService} from '../../../Infrastructure/Services/EntityResourceService';
import {QuoteOPTemplateExtendedPMService} from '../../../QuoteOPM/Services/ExtendedPMs/QuoteOPTemplateExtendedPMService';
import {Guid} from '../../../Infrastructure/Utilities/Guid';
import {MessageWindow} from '../../../Controls/Windows/MessageWindow';
import { ServiceLocator } from '../../../Infrastructure/Locators/ServiceLocator';

@Component({
    selector: 'NewQuoteTemplateComponent',
    
    templateUrl: './NewQuoteTemplateComponent.html',
})

export class NewQuoteTemplateComponent extends BaseComponent implements OnInit {

    public DataContext: NewQuoteTemplateComponent = this;
    EntityPM: QuoteOPTemplatePM;
    QuoteOPTemplateLists: QuoteOPTemplateList[] = [];
    AllQuoteOPTemplateLists: QuoteOPTemplateList[] = [];

    SelectedQuoteOPTemplate: QuoteOPTemplateList;
    VisibilityRadioFromTenant: boolean = false;
    IsNewEntityCall: boolean = true;
    public ValidationErrorsList: string[];
    QuoteOPTemplateExtendedPMService: QuoteOPTemplateExtendedPMService;

    FromAllTenantRadioButton: string;
    CopyRadioButton: string;
    NewRadioButton: string;
    IsReady: boolean = false;


    @ViewChild('Child', { read: ViewContainerRef, static: false }) viewContainerRef: ViewContainerRef;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();


        var _entityResourceService: EntityResourceService = new EntityResourceService();
        _entityResourceService.getEntityResourceByTableName("QuoteOPTemplate").subscribe((response:any) => {
            this.IsReady = true;
            this.EntityPM = this.GetNewInstance();
            this.QuoteOPTemplateExtendedPMService = new QuoteOPTemplateExtendedPMService();
            this.FromAllTenantRadioButton = Guid.newGuid();
            this.CopyRadioButton = Guid.newGuid();
            this.NewRadioButton = Guid.newGuid();

            if (SessionLocator.LoggedUserPM.IsCustomerCare) {
                this.VisibilityRadioFromTenant = true;
            }
  
        });

        ServiceLocator.SendTotangoUserActivity("Quotation", "Create Quote Template");
        
    }

    ngOnInit() {


    }

    GetNewInstance() {

        var newEntity = new QuoteOPTemplatePM();
        newEntity.Tenant = SessionLocator.Tenant;
        newEntity.CreatedByUserId = SessionLocator.LoggedUserId;
        newEntity.CreateDate = DateTool.GetCurrentDateTimeAsUtc();
        newEntity.UpdateDate = DateTool.GetCurrentDateTimeAsUtc();
        newEntity.TemplateTypeCode = "A";
        newEntity.IsTemplate = true;
        return newEntity;
    }



    private templateTypeCode: string;
    public get TemplateTypeCode() {
        if (this.EntityPM) {
             this.templateTypeCode = this.EntityPM.TemplateTypeCode;;
        }
        return this.templateTypeCode;
    }
    public set TemplateTypeCode(newValue: string) {
        if (this.templateTypeCode != newValue) {
            if (this.EntityPM) {
                this.EntityPM.TemplateTypeCode = newValue;
                if (this.AllQuoteOPTemplateLists) {
                    this.QuoteOPTemplateLists = this.AllQuoteOPTemplateLists.filter(d => d.TemplateTypeCode == this.EntityPM.TemplateTypeCode);
                }
            }
        }
    }




    SetWindowArgs(args: any) {

    }

    AddType: string = "New";
    RadioButtonChoice(choose: string) {

        switch (choose) {
            case "New":
                {
                    this.AddType = "New";
                    break;
                }

            case "Copy":
                {
                    this.AddType = "Copy";
                    this.LoadQuoteOPTemplateList();
                    break;
                }



            case "FromAllTenant":
                {
                    this.AddType = "FromAllTenant";
                    this.LoadQuoteOPTemplateList();
                    break;
                }



        }

    }
    OnSelectQuoteOPTemplateChange(item: QuoteOPTemplateList) {
        this.SelectedQuoteOPTemplate = item;
    }

    LoadQuoteOPTemplateList() {
        this.QuoteOPTemplateLists = [];
        this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator.Translate("QuoteOPTemplate.M.Loading"));
        this.QuoteOPTemplateExtendedPMService.GetQuoteOPTemplateLists(this.AddType).subscribe((res:any) => {
            this.CurrentSession.StopBusyIndicator();
            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                this.AllQuoteOPTemplateLists = pmResponse.Result;
                this.QuoteOPTemplateLists = this.AllQuoteOPTemplateLists.filter(d => d.TemplateTypeCode == this.EntityPM.TemplateTypeCode);
            }
         
        });
        
    }

    //New 

    NextButtonClicked() {
        this.ValidationErrorsList = [];

        if (AppTool.IsNullOrEmpty(this.EntityPM.Name)) {
            this.ValidationErrorsList.push("Name field is required");
        }

        if (AppTool.IsNullOrEmpty(this.EntityPM.TemplateTypeCode)) {
            this.ValidationErrorsList.push("Please Select QuoteOPTemplate");
        }


        if (this.ValidationErrorsList.length == 0) {
            if (this.AddType == "New") {

                this.CreateNewQuoteOPTemplate();
            }
            else {
                if (this.SelectedQuoteOPTemplate) {
                    this.CopyQuoteOPTemplatePM();
                }
              
            }
        }
    }

    CreateNewQuoteOPTemplate() {
        this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator.Translate("QuoteOPTemplate.M.Saving"));
        this.QuoteOPTemplateExtendedPMService.insert(this.EntityPM).subscribe((res:any) => {
            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                this.EntityPM = pmResponse.Result;
                this.OpenEditQuoteTemplateComponent();

            }
            else {
                this.CurrentSession.StopBusyIndicator();

                if (pmResponse.ErrorsArray && pmResponse.ErrorsArray.length > 0) {
                    var window = new MessageWindow();
                    window.Show(pmResponse.ErrorsArray[0]);
                }
          
            }

        });

    }

 
    CopyQuoteOPTemplatePM() {
     
        this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator.Translate("QuoteOPTemplate.M.Saving"));
        this.QuoteOPTemplateExtendedPMService.GetCopyQuoteOPTemplate(this.SelectedQuoteOPTemplate.Id, this.EntityPM.Name, SessionLocator.LoggedUserId, SessionLocator.Tenant).subscribe((res:any) => {
           
            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                if (pmResponse.Result) {
                    this.EntityPM = pmResponse.Result;
                    this.OpenEditQuoteTemplateComponent();
                }
                else this.CurrentSession.StopBusyIndicator();
            } else {
                this.CurrentSession.StopBusyIndicator();

                if (pmResponse.ErrorsArray && pmResponse.ErrorsArray.length > 0) {
                    var window = new MessageWindow();
                    window.Show(pmResponse.ErrorsArray[0]);
                }
            }

        });

    }


    //OpenEditQuoteOPTemplate
    OpenEditQuoteTemplateComponent() {
        this.CloseButtonClicked();
        var windowArgs: any = {};
        var logWindow = new LogitudeWindow();
        windowArgs.IsNewEntityCall = true;
        windowArgs.EntityPM = this.EntityPM;
        logWindow.Title = this.EntityPM.Name; 
        logWindow.WindowArgs = windowArgs;
        logWindow.Width = window.innerWidth - 150;
        logWindow.Height = window.innerHeight - 150;
        logWindow.IsShowCloseButton = true;
        logWindow.DataContext = this;

        logWindow.StartBusyIndicator(TextCodeTranslator.Translate("QuoteOPTemplate.M.Loading"));
        logWindow.Show("./QuoteOPModules/QuoteTemplates/Components/EditQuoteTemplateComponent");
       
    }



    CloseButtonClicked() {
        this.CurrentSession.CurrentWindow.Close(this.EntityPM.Id);

    }
}
