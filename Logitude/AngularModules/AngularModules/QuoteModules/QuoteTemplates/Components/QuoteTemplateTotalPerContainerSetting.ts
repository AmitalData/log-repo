declare var window: any;
import {Component, OnInit, ViewChild, ViewContainerRef} from '@angular/core';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {QuoteTemplateSectionPM} from '../../../Quote/EntityPMs/QuoteTemplateSectionPM';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {QuoteTemplateSectionPMService} from '../../../Quote/Services/StandardPMs/QuoteTemplateSectionPMService';
import {EditQuoteTemplateComponent} from './EditQuoteTemplateComponent';
import {FroalaEditorSetting} from '../../../InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/DocsOut/FroalaEditorSetting';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import {QuoteTemplateSettingPM} from '../../../Quote/EntityPMs/QuoteTemplateSettingPM';
import {Guid} from '../../../Infrastructure/Utilities/Guid';
import {AppTool} from '../../../Infrastructure/Tools';
import {ObservableCollection} from '../../../Infrastructure/Utilities/ObservableCollection';
import {QuoteTemplateTextCodePM} from '../../../Quote/EntityPMs/QuoteTemplateTextCodePM';
import {TextCodeData} from './QuoteTemplatePricingSettingComponent';
import {QuoteTemplateTextDesignPM} from '../../../Quote/EntityPMs/QuoteTemplateTextDesignPM';
import {QuoteTemplateTableDesignPM} from '../../../Quote/EntityPMs/QuoteTemplateTableDesignPM';
import {QuoteTemplateTableDesignPMService} from '../../../Quote/Services/StandardPMs/QuoteTemplateTableDesignPMService';
import {QuoteTemplateTextDesignPMService} from '../../../Quote/Services/StandardPMs/QuoteTemplateTextDesignPMService';
import {QuoteTemplateSettingPMService} from '../../../Quote/Services/StandardPMs/QuoteTemplateSettingPMService';
import {QuoteTemplateTextCodeExtendedPMService} from '../../../Quote/Services/ExtendedPMs/QuoteTemplateTextCodeExtendedPMService';
import {QuoteTemplateTextDesignExtendedPMService} from '../../../Quote/Services/ExtendedPMs/QuoteTemplateTextDesignExtendedPMService';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';


@Component({
    selector: 'QuoteTemplateTotalPerContainerSetting',
    
    templateUrl: 'QuoteTemplateTotalPerContainerSetting.html',
})

export class QuoteTemplateTotalPerContainerSetting extends BaseComponent implements OnInit {
    QuoteTemplateSettingPM: QuoteTemplateSettingPM;
    QuoteTemplateId: string;
    public ItemsSource: ObservableCollection;
    QuoteTemplateTextCodePMList: QuoteTemplateTextCodePM[] = [];

    QuoteTemplateTextDesignPMLists: QuoteTemplateTextDesignPM[] = [];
    IsLoadPage: boolean = false;
    quoteTemplateSettingPMService: QuoteTemplateSettingPMService;
    quoteTemplateTextDesignPMService: QuoteTemplateTextDesignPMService;
    quoteTemplateTableDesignPMService: QuoteTemplateTableDesignPMService;
    quoteTemplateTextCodeExtendedPMService: QuoteTemplateTextCodeExtendedPMService;
    quoteTemplateTextDesignExtendedPMService: QuoteTemplateTextDesignExtendedPMService;
    TableDesignPM: QuoteTemplateTableDesignPM;
    HeaderTextDesignPM: QuoteTemplateTextDesignPM;
    RowTextDesignPM: QuoteTemplateTextDesignPM;
    TitleTextDesignPM: QuoteTemplateTextDesignPM;

    IsSaveQuoteTemplateTextDesignRuning: boolean = false;
    IsSaveQuoteTemplateTableDesignRuning: boolean = false;
    IsSaveQuoteTemplateTextCodeRuning: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    public IsUsingVirtuallization: boolean = false;

    constructor() {
        super();
        this.ItemsSource = new ObservableCollection([]);
        this.quoteTemplateSettingPMService = new QuoteTemplateSettingPMService();
        this.quoteTemplateTableDesignPMService = new QuoteTemplateTableDesignPMService();
        this.quoteTemplateTextDesignPMService = new QuoteTemplateTextDesignPMService();
        this.quoteTemplateTextDesignExtendedPMService = new QuoteTemplateTextDesignExtendedPMService();
        this.quoteTemplateTextCodeExtendedPMService = new QuoteTemplateTextCodeExtendedPMService();

    }

    ngOnInit() {

    }

    SetWindowArgs(args: any) {
        this.SetIsUsingVirtuallization();
        if (args) {
            this.QuoteTemplateSettingPM = args.QuoteTemplateSettingPM;

            if (args.QuoteTemplateTextCodePMList) {
                this.QuoteTemplateTextCodePMList = args.QuoteTemplateTextCodePMList.filter(d => d.Area == "TotalPerContainers");
                this.BuildItemsSource();
            }

            
            if (!AppTool.IsNullOrEmpty(this.QuoteTemplateSettingPM.TotalPerContainersTableDesignId)){
                this.LoadData();
            }
            else this.IsLoadPage = false;

        }

    }

    SetIsUsingVirtuallization() {
        var hasGridVirtuallizationToggleFeature = SessionLocator.FeatureToggles.filter(d => d.ToggleCode == "EVG")[0]
        if (hasGridVirtuallizationToggleFeature) {
            this.IsUsingVirtuallization = true;
        }
    }

    //ShowTotalSplitToMultipleCurrencies
    get TotalPerContainersCurrencyType() {
        var totalPerContainersCurrencyType: string = "";
        if (this.QuoteTemplateSettingPM) totalPerContainersCurrencyType = this.QuoteTemplateSettingPM.TotalPerContainersCurrencyType;
        return totalPerContainersCurrencyType;
    }
    set TotalPerContainersCurrencyType(value: string) {
        if (this.QuoteTemplateSettingPM != null) {
            this.QuoteTemplateSettingPM.TotalPerContainersCurrencyType = value;
        }
    }


    get ShowTitleTotalPerContainersTable() {
        var showTitleTotalPerContainersTable: boolean = false;
        if (this.QuoteTemplateSettingPM) {
            showTitleTotalPerContainersTable = this.QuoteTemplateSettingPM.ShowTitleTotalPerContainersTable;
        }
        return showTitleTotalPerContainersTable;
    }
    set ShowTitleTotalPerContainersTable(value: boolean) {
        if (this.QuoteTemplateSettingPM != null) {
            this.QuoteTemplateSettingPM.ShowTitleTotalPerContainersTable = value;
        }
    }

    get ShowPageBreakBeforeTotalPerContainersTable() {
        var showPageBreakBeforeTotalPerContainersTable: boolean = false;
        if (this.QuoteTemplateSettingPM) {
            showPageBreakBeforeTotalPerContainersTable = this.QuoteTemplateSettingPM.ShowPageBreakBeforeTotalPerContainersTable;
        }
        return showPageBreakBeforeTotalPerContainersTable;
    }
    set ShowPageBreakBeforeTotalPerContainersTable(value: boolean) {
        if (this.QuoteTemplateSettingPM != null) {
            this.QuoteTemplateSettingPM.ShowPageBreakBeforeTotalPerContainersTable = value;
        }
    }




    get ShowIncludedChargesPerContainers() {
        var showIncludedChargesPerContainers: boolean = false;
        if (this.QuoteTemplateSettingPM) {
            showIncludedChargesPerContainers = this.QuoteTemplateSettingPM.ShowIncludedChargesPerContainers;
        }
        return showIncludedChargesPerContainers;
    }
    set ShowIncludedChargesPerContainers(value: boolean) {
        if (this.QuoteTemplateSettingPM != null) {
            this.QuoteTemplateSettingPM.ShowIncludedChargesPerContainers = value;
        }
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
    }


    LoadTableDesign() {

        this.quoteTemplateTableDesignPMService.get(this.QuoteTemplateSettingPM.TotalPerContainersTableDesignId).subscribe((res:any) => {
            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError && pmResponse.Result) {
                this.TableDesignPM = pmResponse.Result;
            }
            this.LoadTextDesign();

        });
    }



    LoadTextDesign() {
       
        var titleTextDesignId: string = this.QuoteTemplateSettingPM.TotalPerContainersAdditionalTextDesignId;

        var ids: string = titleTextDesignId;
        if (this.TableDesignPM) {
            ids += ("," + this.TableDesignPM.HeaderDesignId);
            ids += ("," + this.TableDesignPM.LinesDesignId);
        }

        this.quoteTemplateTextDesignExtendedPMService.GetQuoteTemplateTextDesignPMListByIds(ids, SessionLocator.Tenant).subscribe((res:any) => {
            var pmResponse: ServiceResponse = res;
            this.CurrentSession.StopBusyIndicator();
            if (!pmResponse.HasError && pmResponse.Result) {
                this.QuoteTemplateTextDesignPMLists = pmResponse.Result;

                if (this.TableDesignPM) {
                    this.HeaderTextDesignPM = this.QuoteTemplateTextDesignPMLists.filter(d => d.Id == this.TableDesignPM.HeaderDesignId)[0];
                    if (this.HeaderTextDesignPM) {
                        this.HeaderTextDesignPM.Title = "Header";
                    }

                    this.RowTextDesignPM = this.QuoteTemplateTextDesignPMLists.filter(d => d.Id == this.TableDesignPM.LinesDesignId)[0];
                    if (this.RowTextDesignPM) {
                        this.RowTextDesignPM.Title = "Rows";
                        this.RowTextDesignPM.HideAlignment = true;
                    }
                }
              



                this.TitleTextDesignPM = this.QuoteTemplateTextDesignPMLists.filter(d => d.Id == titleTextDesignId)[0];
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


        var textDesignPmLists = this.QuoteTemplateTextDesignPMLists.filter(d => d.IsDirty == true);
        if (textDesignPmLists.length > 0) this.IsSaveQuoteTemplateTextDesignRuning = true;
        if (this.TableDesignPM.IsDirty) this.IsSaveQuoteTemplateTableDesignRuning = true;



        var textCodeDataLists: TextCodeData[] = this.ItemsSource.Collection.filter(d => d.EntityPM.IsDirty == true);
        if (textCodeDataLists.length > 0) this.IsSaveQuoteTemplateTextCodeRuning = true;



        if (this.IsSaveQuoteTemplateTextDesignRuning || this.IsSaveQuoteTemplateTableDesignRuning || this.IsSaveQuoteTemplateTextCodeRuning) {

            this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator.Translate("QuoteTemplate.M.Saving"));

            if (this.QuoteTemplateSettingPM.IsDirty) {
                this.quoteTemplateSettingPMService.update(this.QuoteTemplateSettingPM).subscribe((res:any) => {
                    this.QuoteTemplateSettingPM.IsDirty = false;
                    this.SaveOthers(textDesignPmLists, textCodeDataLists);
                });
            } else this.SaveOthers(textDesignPmLists, textCodeDataLists);



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

    SaveOthers(textDesignPmLists: any[], textCodeDataLists: any[]) {

        if (this.IsSaveQuoteTemplateTextDesignRuning) this.SaveQuoteTemplateTextDesign(textDesignPmLists);
        if (this.IsSaveQuoteTemplateTableDesignRuning) this.SaveQuoteTemplateTableDesign();
        if (this.IsSaveQuoteTemplateTextCodeRuning) this.SaveQuoteTemplateTextCode(textCodeDataLists);
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



    SaveCompleted() {
        if (!this.IsSaveQuoteTemplateTextDesignRuning && !this.IsSaveQuoteTemplateTableDesignRuning && !this.IsSaveQuoteTemplateTextCodeRuning) {
            this.CurrentSession.StopBusyIndicator();
            this.CurrentSession.CurrentWindow.Close("Refresh");

        }

    }

    CloseButtonClicked() {
        this.CurrentSession.StopBusyIndicator();
        this.CurrentSession.CurrentWindow.Close("");
    }
}


