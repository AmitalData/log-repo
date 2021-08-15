declare var window: any;
import {Component, OnInit, ViewChild, ViewContainerRef} from '@angular/core';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {QuoteOPTemplateSectionPM} from '../../../QuoteOPM/EntityPMs/QuoteOPTemplateSectionPM';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {QuoteOPTemplateSectionPMService} from '../../../QuoteOPM/Services/StandardPMs/QuoteOPTemplateSectionPMService';
import {EditQuoteTemplateComponent} from './EditQuoteTemplateComponent';
import {FroalaEditorSetting} from '../../../InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/DocsOut/FroalaEditorSetting';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import {QuoteOPTemplateSettingPM} from '../../../QuoteOPM/EntityPMs/QuoteOPTemplateSettingPM';
import {Guid} from '../../../Infrastructure/Utilities/Guid';
import {AppTool} from '../../../Infrastructure/Tools';
import {ObservableCollection} from '../../../Infrastructure/Utilities/ObservableCollection';
import {QuoteOPTemplateTextCodePM} from '../../../QuoteOPM/EntityPMs/QuoteOPTemplateTextCodePM';
import {TextCodeData} from './QuoteTemplatePricingSettingComponent';
import {QuoteOPTemplateTextDesignPM} from '../../../QuoteOPM/EntityPMs/QuoteOPTemplateTextDesignPM';
import {QuoteOPTemplateTableDesignPM} from '../../../QuoteOPM/EntityPMs/QuoteOPTemplateTableDesignPM';
import {QuoteOPTemplateTableDesignPMService} from '../../../QuoteOPM/Services/StandardPMs/QuoteOPTemplateTableDesignPMService';
import {QuoteOPTemplateTextDesignPMService} from '../../../QuoteOPM/Services/StandardPMs/QuoteOPTemplateTextDesignPMService';
import {QuoteOPTemplateSettingPMService} from '../../../QuoteOPM/Services/StandardPMs/QuoteOPTemplateSettingPMService';
import {QuoteOPTemplateTextCodeExtendedPMService} from '../../../QuoteOPM/Services/ExtendedPMs/QuoteOPTemplateTextCodeExtendedPMService';
import {QuoteOPTemplateTextDesignExtendedPMService} from '../../../QuoteOPM/Services/ExtendedPMs/QuoteOPTemplateTextDesignExtendedPMService';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';


@Component({
    selector: 'QuoteTemplateTotalPerContainerSetting',
    
    templateUrl: 'QuoteTemplateTotalPerContainerSetting.html',
})

export class QuoteTemplateTotalPerContainerSetting extends BaseComponent implements OnInit {
    QuoteOPTemplateSettingPM: QuoteOPTemplateSettingPM;
    QuoteOPTemplateId: string;
    public ItemsSource: ObservableCollection;
    QuoteOPTemplateTextCodePMList: QuoteOPTemplateTextCodePM[] = [];

    QuoteOPTemplateTextDesignPMLists: QuoteOPTemplateTextDesignPM[] = [];
    IsLoadPage: boolean = false;
    QuoteOPTemplateSettingPMService: QuoteOPTemplateSettingPMService;
    QuoteOPTemplateTextDesignPMService: QuoteOPTemplateTextDesignPMService;
    QuoteOPTemplateTableDesignPMService: QuoteOPTemplateTableDesignPMService;
    QuoteOPTemplateTextCodeExtendedPMService: QuoteOPTemplateTextCodeExtendedPMService;
    QuoteOPTemplateTextDesignExtendedPMService: QuoteOPTemplateTextDesignExtendedPMService;
    TableDesignPM: QuoteOPTemplateTableDesignPM;
    HeaderTextDesignPM: QuoteOPTemplateTextDesignPM;
    RowTextDesignPM: QuoteOPTemplateTextDesignPM;
    TitleTextDesignPM: QuoteOPTemplateTextDesignPM;

    IsSaveQuoteOPTemplateTextDesignRuning: boolean = false;
    IsSaveQuoteOPTemplateTableDesignRuning: boolean = false;
    IsSaveQuoteOPTemplateTextCodeRuning: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.ItemsSource = new ObservableCollection([]);
        this.QuoteOPTemplateSettingPMService = new QuoteOPTemplateSettingPMService();
        this.QuoteOPTemplateTableDesignPMService = new QuoteOPTemplateTableDesignPMService();
        this.QuoteOPTemplateTextDesignPMService = new QuoteOPTemplateTextDesignPMService();
        this.QuoteOPTemplateTextDesignExtendedPMService = new QuoteOPTemplateTextDesignExtendedPMService();
        this.QuoteOPTemplateTextCodeExtendedPMService = new QuoteOPTemplateTextCodeExtendedPMService();

    }

    ngOnInit() {

    }

    SetWindowArgs(args: any) {

        if (args) {
            this.QuoteOPTemplateSettingPM = args.QuoteOPTemplateSettingPM;

            if (args.QuoteOPTemplateTextCodePMList) {
                this.QuoteOPTemplateTextCodePMList = args.QuoteOPTemplateTextCodePMList.filter(d => d.Area == "TotalPerContainers");
                this.BuildItemsSource();
            }

            
            if (!AppTool.IsNullOrEmpty(this.QuoteOPTemplateSettingPM.TotalPerContainersTableDesignId)){
                this.LoadData();
            }
            else this.IsLoadPage = false;

        }

    }


    //ShowTotalSplitToMultipleCurrencies
    get TotalPerContainersCurrencyType() {
        var totalPerContainersCurrencyType: string = "";
        if (this.QuoteOPTemplateSettingPM) totalPerContainersCurrencyType = this.QuoteOPTemplateSettingPM.TotalPerContainersCurrencyType;
        return totalPerContainersCurrencyType;
    }
    set TotalPerContainersCurrencyType(value: string) {
        if (this.QuoteOPTemplateSettingPM != null) {
            this.QuoteOPTemplateSettingPM.TotalPerContainersCurrencyType = value;
        }
    }


    get ShowTitleTotalPerContainersTable() {
        var showTitleTotalPerContainersTable: boolean = false;
        if (this.QuoteOPTemplateSettingPM) {
            showTitleTotalPerContainersTable = this.QuoteOPTemplateSettingPM.ShowTitleTotalPerContainersTable;
        }
        return showTitleTotalPerContainersTable;
    }
    set ShowTitleTotalPerContainersTable(value: boolean) {
        if (this.QuoteOPTemplateSettingPM != null) {
            this.QuoteOPTemplateSettingPM.ShowTitleTotalPerContainersTable = value;
        }
    }

    get ShowPageBreakBeforeTotalPerContainersTable() {
        var showPageBreakBeforeTotalPerContainersTable: boolean = false;
        if (this.QuoteOPTemplateSettingPM) {
            showPageBreakBeforeTotalPerContainersTable = this.QuoteOPTemplateSettingPM.ShowPageBreakBeforeTotalPerContainersTable;
        }
        return showPageBreakBeforeTotalPerContainersTable;
    }
    set ShowPageBreakBeforeTotalPerContainersTable(value: boolean) {
        if (this.QuoteOPTemplateSettingPM != null) {
            this.QuoteOPTemplateSettingPM.ShowPageBreakBeforeTotalPerContainersTable = value;
        }
    }




    get ShowIncludedChargesPerContainers() {
        var showIncludedChargesPerContainers: boolean = false;
        if (this.QuoteOPTemplateSettingPM) {
            showIncludedChargesPerContainers = this.QuoteOPTemplateSettingPM.ShowIncludedChargesPerContainers;
        }
        return showIncludedChargesPerContainers;
    }
    set ShowIncludedChargesPerContainers(value: boolean) {
        if (this.QuoteOPTemplateSettingPM != null) {
            this.QuoteOPTemplateSettingPM.ShowIncludedChargesPerContainers = value;
        }
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
    }


    LoadTableDesign() {

        this.QuoteOPTemplateTableDesignPMService.get(this.QuoteOPTemplateSettingPM.TotalPerContainersTableDesignId).subscribe((res:any) => {
            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError && pmResponse.Result) {
                this.TableDesignPM = pmResponse.Result;
            }
            this.LoadTextDesign();

        });
    }



    LoadTextDesign() {
       
        var titleTextDesignId: string = this.QuoteOPTemplateSettingPM.TotalPerContainersAdditionalTextDesignId;

        var ids: string = titleTextDesignId;
        if (this.TableDesignPM) {
            ids += ("," + this.TableDesignPM.HeaderDesignId);
            ids += ("," + this.TableDesignPM.LinesDesignId);
        }

        this.QuoteOPTemplateTextDesignExtendedPMService.GetQuoteOPTemplateTextDesignPMListByIds(ids, SessionLocator.Tenant).subscribe((res:any) => {
            var pmResponse: ServiceResponse = res;
            this.CurrentSession.StopBusyIndicator();
            if (!pmResponse.HasError && pmResponse.Result) {
                this.QuoteOPTemplateTextDesignPMLists = pmResponse.Result;

                if (this.TableDesignPM) {
                    this.HeaderTextDesignPM = this.QuoteOPTemplateTextDesignPMLists.filter(d => d.Id == this.TableDesignPM.HeaderDesignId)[0];
                    if (this.HeaderTextDesignPM) {
                        this.HeaderTextDesignPM.Title = "Header";
                    }

                    this.RowTextDesignPM = this.QuoteOPTemplateTextDesignPMLists.filter(d => d.Id == this.TableDesignPM.LinesDesignId)[0];
                    if (this.RowTextDesignPM) {
                        this.RowTextDesignPM.Title = "Rows";
                        this.RowTextDesignPM.HideAlignment = 'true';
                    }
                }
              



                this.TitleTextDesignPM = this.QuoteOPTemplateTextDesignPMLists.filter(d => d.Id == titleTextDesignId)[0];
                if (this.TitleTextDesignPM) {
                    this.TitleTextDesignPM.Title = "Title";
                }

            }

            this.IsLoadPage = true;
        });


    }


  
    ValidationErrorsList: any[] = [];
    SaveButtonClicked() {
        this.ValidationErrorsList = [];


        var textDesignPmLists = this.QuoteOPTemplateTextDesignPMLists.filter(d => d.IsDirty == true);
        if (textDesignPmLists.length > 0) this.IsSaveQuoteOPTemplateTextDesignRuning = true;
        if (this.TableDesignPM.IsDirty) this.IsSaveQuoteOPTemplateTableDesignRuning = true;



        var textCodeDataLists: TextCodeData[] = this.ItemsSource.Collection.filter(d => d.EntityPM.IsDirty == true);
        if (textCodeDataLists.length > 0) this.IsSaveQuoteOPTemplateTextCodeRuning = true;



        if (this.IsSaveQuoteOPTemplateTextDesignRuning || this.IsSaveQuoteOPTemplateTableDesignRuning || this.IsSaveQuoteOPTemplateTextCodeRuning) {

            this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator.Translate("QuoteOPTemplate.M.Saving"));

            if (this.QuoteOPTemplateSettingPM.IsDirty) {
                this.QuoteOPTemplateSettingPMService.update(this.QuoteOPTemplateSettingPM).subscribe((res:any) => {
                    this.QuoteOPTemplateSettingPM.IsDirty = false;
                    this.SaveOthers(textDesignPmLists, textCodeDataLists);
                });
            } else this.SaveOthers(textDesignPmLists, textCodeDataLists);



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

    SaveOthers(textDesignPmLists: any[], textCodeDataLists: any[]) {

        if (this.IsSaveQuoteOPTemplateTextDesignRuning) this.SaveQuoteOPTemplateTextDesign(textDesignPmLists);
        if (this.IsSaveQuoteOPTemplateTableDesignRuning) this.SaveQuoteOPTemplateTableDesign();
        if (this.IsSaveQuoteOPTemplateTextCodeRuning) this.SaveQuoteOPTemplateTextCode(textCodeDataLists);
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



    SaveCompleted() {
        if (!this.IsSaveQuoteOPTemplateTextDesignRuning && !this.IsSaveQuoteOPTemplateTableDesignRuning && !this.IsSaveQuoteOPTemplateTextCodeRuning) {
            this.CurrentSession.StopBusyIndicator();
            this.CurrentSession.CurrentWindow.Close("Refresh");

        }

    }

    CloseButtonClicked() {
        this.CurrentSession.StopBusyIndicator();
        this.CurrentSession.CurrentWindow.Close("");
    }
}


