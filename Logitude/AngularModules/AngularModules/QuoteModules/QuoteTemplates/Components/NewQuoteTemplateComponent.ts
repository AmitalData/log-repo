import {Component, OnInit, ViewChild, ViewContainerRef} from '@angular/core';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {QuoteTemplatePM} from '../../../Quote/EntityPMs/QuoteTemplatePM';
import {QuoteTemplateList} from '../../../Quote/EntityLists/QuoteTemplateList';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';
import {AppTool, DateTool} from '../../../Infrastructure/Tools';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import {CitySelectionArgs} from '../../../Common/Args';
import {Validator} from '../../../Infrastructure/Validators/Validator';
import {NewEntityArgs} from '../../../Infrastructure/Args';
import {EntityResourceService} from '../../../Infrastructure/Services/EntityResourceService';
import {QuoteTemplateExtendedPMService} from '../../../Quote/Services/ExtendedPMs/QuoteTemplateExtendedPMService';
import {Guid} from '../../../Infrastructure/Utilities/Guid';
import {MessageWindow} from '../../../Controls/Windows/MessageWindow';

@Component({
    selector: 'NewQuoteTemplateComponent',
    moduleId: module.id,
    templateUrl: './NewQuoteTemplateComponent.html',
})

export class NewQuoteTemplateComponent extends BaseComponent implements OnInit {

    public DataContext: NewQuoteTemplateComponent = this;
    EntityPM: QuoteTemplatePM;
    QuoteTemplateLists: QuoteTemplateList[] = [];
    SelectedQuoteTemplate: QuoteTemplateList;
    VisibilityRadioFromTenant: boolean = false;
    IsNewEntityCall: boolean = true;
    public ValidationErrorsList: string[];
    quoteTemplateExtendedPMService: QuoteTemplateExtendedPMService;

    FromAllTenantRadioButton: string;
    CopyRadioButton: string;
    NewRadioButton: string;
    IsReady: boolean = false;


    @ViewChild('Child', { read: ViewContainerRef }) viewContainerRef: ViewContainerRef;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();


        var _entityResourceService: EntityResourceService = new EntityResourceService();
        _entityResourceService.getEntityResourceByTableName("QuoteTemplate").subscribe(response => {
            this.IsReady = true;
            this.EntityPM = this.GetNewInstance();
            this.quoteTemplateExtendedPMService = new QuoteTemplateExtendedPMService();
            this.FromAllTenantRadioButton = Guid.newGuid();
            this.CopyRadioButton = Guid.newGuid();
            this.NewRadioButton = Guid.newGuid();

            if (SessionLocator.LoggedUserPM.IsCustomerCare) {
                this.VisibilityRadioFromTenant = true;
            }
  
        });


        
    }

    ngOnInit() {


    }

    GetNewInstance() {

        var newEntity = new QuoteTemplatePM();
        newEntity.Tenant = SessionLocator.Tenant;
        newEntity.CreatedByUserId = SessionLocator.LoggedUserId;
        newEntity.CreateDate = DateTool.GetCurrentDateTimeAsUtc();
        newEntity.UpdateDate = DateTool.GetCurrentDateTimeAsUtc();
        newEntity.IsTemplate = true;
        return newEntity;
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
                    this.LoadQuoteTemplateList();
                    break;
                }



            case "FromAllTenant":
                {
                    this.AddType = "FromAllTenant";
                    this.LoadQuoteTemplateList();
                    break;
                }



        }

    }
    OnSelectQuoteTemplateChange(item: QuoteTemplateList) {
        this.SelectedQuoteTemplate = item;
    }

    LoadQuoteTemplateList() {
        this.QuoteTemplateLists = [];
        this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator.Translate("QuoteTemplate.M.Loading"));
        this.quoteTemplateExtendedPMService.GetQuoteTemplateLists(this.AddType, SessionLocator.LoggedUserPM.IsCustomerCare, SessionLocator.Tenant).subscribe(res => {
            this.CurrentSession.StopBusyIndicator();
            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
       
                this.QuoteTemplateLists = pmResponse.Result;
            }
         
        });
        
    }

    //New 

    NextButtonClicked() {
        this.ValidationErrorsList = [];

        if (AppTool.IsNullOrEmpty(this.EntityPM.Name)) {
            this.ValidationErrorsList.push("Name field is required");
        }

        if (AppTool.IsNullOrEmpty(this.EntityPM.TemplateTypeCode) && this.AddType == "New") {
            this.ValidationErrorsList.push("Please Select QuoteTemplate");
        }


        if (this.ValidationErrorsList.length == 0) {
            if (this.AddType == "New") {

                this.CreateNewQuoteTemplate();
            }
            else {
                if (this.SelectedQuoteTemplate) {
                    this.CopyQuoteTemplatePM();
                }
              
            }
        }
    }

    CreateNewQuoteTemplate() {
        this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator.Translate("QuoteTemplate.M.Saving"));
        this.quoteTemplateExtendedPMService.insert(this.EntityPM).subscribe(res => {
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

 
    CopyQuoteTemplatePM() {
     
        this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator.Translate("QuoteTemplate.M.Saving"));
        this.quoteTemplateExtendedPMService.GetCopyQuoteTemplate(this.SelectedQuoteTemplate.Id, this.EntityPM.Name, SessionLocator.LoggedUserId, SessionLocator.Tenant).subscribe(res => {
           
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


    //OpenEditQuoteTemplate
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

        logWindow.StartBusyIndicator(TextCodeTranslator.Translate("QuoteTemplate.M.Loading"));
        logWindow.Show("./QuoteModules/QuoteTemplates/Components/EditQuoteTemplateComponent");
       
    }



    CloseButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
}
