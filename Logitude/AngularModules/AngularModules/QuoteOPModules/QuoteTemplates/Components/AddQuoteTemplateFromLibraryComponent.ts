declare var window: any;

import {Component, OnInit, ViewChild, ViewContainerRef} from '@angular/core';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {QuoteOPTemplatePM} from '../../../QuoteOPM/EntityPMs/QuoteOPTemplatePM';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {QuoteOPTemplateExtendedPMService} from '../../../QuoteOPM/Services/ExtendedPMs/QuoteOPTemplateExtendedPMService';
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
    QuoteOPTemplateExtendedPMService: QuoteOPTemplateExtendedPMService;
    public QuoteOPTemplateLists: any[];
    public FullQuoteOPTemplateLists: any[];
    IsShowMessageNoQuoteOPTemplate: boolean;
    public QuoteOPTemplateViewModelSelected: any;
    IsLoadTextCode: boolean;
    QuoteOPId: any;
    QuoteTypeCode: string = null;
    AreaName: string;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.QuoteOPTemplateExtendedPMService = new QuoteOPTemplateExtendedPMService();

    }

    ngOnInit() {

    }

    SetWindowArgs(args: any) {

        this.QuoteOPId = args.QuoteOPId;
        this.AreaName = args.AreaName;
        this.QuoteTypeCode = !AppTool.IsNullOrEmpty(args.QuoteTypeCode) ? args.QuoteTypeCode:"" ;
        
        this._entityResourceService.getEntityResourceByTableName("DocumentTypeTemplate").subscribe((response:any) => {
            this.IsLoadTextCode = true;
            this.QuoteOPTemplateLists = [];
            this.FullQuoteOPTemplateLists = [];
            this.LoadQuoteOPTemplateList();

        });

    }


    onSearchTextChangeEvent(search) {
        if (search) {
            if (search != "Search") {
                this.QuoteOPTemplateLists = this.FullQuoteOPTemplateLists.filter(d => d.Name.toUpperCase().indexOf(search.toUpperCase()) > -1 || d.Name.toUpperCase().indexOf(search.toUpperCase()) > -1);
            }
        }
        else {
            this.QuoteOPTemplateLists = this.FullQuoteOPTemplateLists;
        }

        if (this.QuoteOPTemplateLists && this.QuoteOPTemplateLists.length == 0) {
            this.IsShowMessageNoQuoteOPTemplate = true;
        } else this.IsShowMessageNoQuoteOPTemplate = false;

    }



    LoadQuoteOPTemplateList() {
        this.QuoteOPTemplateLists = [];
        this.FullQuoteOPTemplateLists = [];


        this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator.Translate("QuoteOPTemplate.M.Loading"));



        this.QuoteOPTemplateExtendedPMService.GetQuoteOPTemplateListsFromLibrary(this.QuoteTypeCode).subscribe((res:any) => {
            this.CurrentSession.StopBusyIndicator();
            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                pmResponse.Result.forEach((item) => {
                    this.QuoteOPTemplateLists.push(item);
                    this.FullQuoteOPTemplateLists.push(item);

                });

                if (this.QuoteOPTemplateLists.length == 0) {
                    this.IsShowMessageNoQuoteOPTemplate = true;
                } else this.IsShowMessageNoQuoteOPTemplate = false;
            }

        });

    }


    OnSelectedDocumentTypeTemplateLists(item: any) {
        this.QuoteOPTemplateViewModelSelected = item;

    }


    CloseButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    AddFromLibraryButtonClicked(item: any) {
        this.QuoteOPTemplateViewModelSelected = item;
        this.QuoteOPTemplateViewModelSelected.IsEnabledAddDocumentTemplate = false;
        this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator.Translate("QuoteOPTemplate.M.Saving"));




        this.QuoteOPTemplateExtendedPMService.GetCopyQuoteOPTemplateFromLibrary(item.Id, SessionLocator.LoggedUserId).subscribe((res:any) => {
            this.CurrentSession.StopBusyIndicator();
            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                this.CurrentSession.CurrentWindow.Close(pmResponse.Result);
 
            }

        });

    }

    OnSelectedQuoteOPTemplateLists(item:any) {
        this.QuoteOPTemplateViewModelSelected = item;
    }

    PreviewFromLibraryButtonClicked(item: any) {
        this.QuoteOPTemplateViewModelSelected = item;

        var windowArgs: any = {};
        var logWindow = new LogitudeWindow();
        windowArgs.QuoteTemplateId = item.Id;
        windowArgs.QuoteOPId = !AppTool.IsNullOrEmpty(this.QuoteOPId) ? this.QuoteOPId :"";
        windowArgs.AreaName = "FromLibrary";
        logWindow.Title = "Preview Template";
        logWindow.WindowArgs = windowArgs;
        logWindow.Width = 1000;
        logWindow.Height = (window.innerHeight - 130);
        logWindow.Show("./QuoteModules/QuoteTemplates/Components/PreviewQuoteTemplateReportComponent");
    }


}


