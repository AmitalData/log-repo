declare var window: any;

import {Component, OnInit, ViewChild, ViewContainerRef} from '@angular/core';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {QuoteTemplatePM} from '../../../Quote/EntityPMs/QuoteTemplatePM';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {QuoteTemplateExtendedPMService} from '../../../Quote/Services/ExtendedPMs/QuoteTemplateExtendedPMService';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import {EntityResourceService} from '../../../Infrastructure/Services/EntityResourceService';
import {Guid} from '../../../Infrastructure/Utilities/Guid';
declare var StringToBase64, Base64ToString: any;
import {AppTool} from '../../../Infrastructure/Tools';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';
@Component({
    selector: 'AddQuoteTemplateFromLibraryComponent',
    
    templateUrl: './AddQuoteTemplateFromLibraryComponent.html',
})

export class AddQuoteTemplateFromLibraryComponent extends BaseComponent implements OnInit {
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    quoteTemplateExtendedPMService: QuoteTemplateExtendedPMService;
    public QuoteTemplateLists: any[];
    public FullQuoteTemplateLists: any[];
    IsShowMessageNoQuoteTemplate: boolean;
    public QuoteTemplateViewModelSelected: any;
    IsLoadTextCode: boolean;
    QuoteId: any;
    QuoteTypeCode: string = null;
    AreaName: string;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.quoteTemplateExtendedPMService = new QuoteTemplateExtendedPMService();

    }

    ngOnInit() {

    }

    SetWindowArgs(args: any) {

        this.QuoteId = args.QuoteId;
        this.AreaName = args.AreaName;
        this.QuoteTypeCode = !AppTool.IsNullOrEmpty(args.QuoteTypeCode) ? args.QuoteTypeCode:"" ;
        
        this._entityResourceService.getEntityResourceByTableName("DocumentTypeTemplate").subscribe((response:any) => {
            this.IsLoadTextCode = true;
            this.QuoteTemplateLists = [];
            this.FullQuoteTemplateLists = [];
            this.LoadQuoteTemplateList();

        });

    }


    onSearchTextChangeEvent(search) {
        if (search) {
            if (search != "Search") {
                this.QuoteTemplateLists = this.FullQuoteTemplateLists.filter(d => d.Name.toUpperCase().indexOf(search.toUpperCase()) > -1 || d.Name.toUpperCase().indexOf(search.toUpperCase()) > -1);
            }
        }
        else {
            this.QuoteTemplateLists = this.FullQuoteTemplateLists;
        }

        if (this.QuoteTemplateLists && this.QuoteTemplateLists.length == 0) {
            this.IsShowMessageNoQuoteTemplate = true;
        } else this.IsShowMessageNoQuoteTemplate = false;

    }



    LoadQuoteTemplateList() {
        this.QuoteTemplateLists = [];
        this.FullQuoteTemplateLists = [];


        this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator.Translate("QuoteTemplate.M.Loading"));



        this.quoteTemplateExtendedPMService.GetQuoteTemplateListsFromLibrary(this.QuoteTypeCode).subscribe((res:any) => {
            this.CurrentSession.StopBusyIndicator();
            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                pmResponse.Result.forEach((item) => {
                    this.QuoteTemplateLists.push(item);
                    this.FullQuoteTemplateLists.push(item);

                });

                if (this.QuoteTemplateLists.length == 0) {
                    this.IsShowMessageNoQuoteTemplate = true;
                } else this.IsShowMessageNoQuoteTemplate = false;
            }

        });

    }


    OnSelectedDocumentTypeTemplateLists(item: any) {
        this.QuoteTemplateViewModelSelected = item;

    }


    CloseButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    AddFromLibraryButtonClicked(item: any) {
        this.QuoteTemplateViewModelSelected = item;
        this.QuoteTemplateViewModelSelected.IsEnabledAddDocumentTemplate = false;
        this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator.Translate("QuoteTemplate.M.Saving"));




        this.quoteTemplateExtendedPMService.GetCopyQuoteTemplateFromLibrary(item.Id, SessionLocator.LoggedUserId).subscribe((res:any) => {
            this.CurrentSession.StopBusyIndicator();
            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                this.CurrentSession.CurrentWindow.Close(pmResponse.Result);
 
            }

        });

    }

    OnSelectedQuoteTemplateLists(item:any) {
        this.QuoteTemplateViewModelSelected = item;
    }

    PreviewFromLibraryButtonClicked(item: any) {
        this.QuoteTemplateViewModelSelected = item;

        var windowArgs: any = {};
        var logWindow = new LogitudeWindow();
        windowArgs.QuoteTemplateId = item.Id;
        windowArgs.QuoteId = !AppTool.IsNullOrEmpty(this.QuoteId) ? this.QuoteId :"";
        windowArgs.AreaName = "FromLibrary";
        logWindow.Title = "Preview Template";
        logWindow.WindowArgs = windowArgs;
        logWindow.Width = 1000;
        logWindow.Height = (window.innerHeight - 130);
        logWindow.Show("./QuoteModules/QuoteTemplates/Components/PreviewQuoteTemplateReportComponent");
    }


}


