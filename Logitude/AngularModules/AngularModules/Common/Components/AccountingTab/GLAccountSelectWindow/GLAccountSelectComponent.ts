import { ServiceResponse } from "./../../../../Infrastructure/DataContracts/ServiceResponse";
import { GLAccountExtendedPMService } from "./../../../../Accounting/Services/ExtendedPMs/GLAccountExtendedPMService";
import { EntityResourceService } from "./../../../../Infrastructure/Services/EntityResourceService";
import { Component, OnInit, Output, EventEmitter } from "@angular/core";
import { BaseComponent } from "../../../../Infrastructure/Components/LogitudeComponents/BaseComponent";
import { SessionLocator } from "../../../../Infrastructure/Utilities/SessionLocator";
import { ApiQueryFilters } from "../../../../Infrastructure/DataContracts/ApiQueryFilters";
import { EntityListService } from "../../../../Infrastructure/Services/EntityListService";
import { TextCodeTranslator } from "../../../../Infrastructure/Utilities/TextCodeTranslator";
import { MessageWindow } from "../../../../Controls/Windows/MessageWindow";
import { ObjectsLocator } from "../../../../Infrastructure/Locators/ObjectsLocator";
import { ConfirmWindow } from "../../../../Controls/Windows/ConfirmWindow";

@Component({
    templateUrl: "./GLAccountSelectComponent.html"
})
export class GLAccountSelectComponent extends BaseComponent implements OnInit
{
    // @Output() ItemSelected = new EventEmitter();
    @Output() SearchFieldchangeevent = new EventEmitter();
    @Output() onQueryChangeEvent = new EventEmitter();
    public DataContext: GLAccountSelectComponent = this;
    public CurrentSession = SessionLocator.SelectedSession;
    public ValidationErrorsList: string[] = [];
    public isRTL: boolean = false;
    public warningMessageShown: boolean = false;
    entityResourceService: EntityResourceService = new EntityResourceService();
    entityListService: EntityListService = new EntityListService();
    gLAccountExtendedPMService: GLAccountExtendedPMService = new GLAccountExtendedPMService();

    chartOfAccountTypeCode: string;
    cardId: string;

    constructor()
    {
        super();
        if (ObjectsLocator.GlobalSetting)
            this.isRTL = ObjectsLocator.GlobalSetting.LayoutDirection == "rtl";
    }

    ngOnInit()
    {
        this.entityResourceService
            .getEntityResourceByTableName("GLAccount")
            .subscribe(() =>
            {
                this.BuildColumns();
                this.ReloadData();
            });
    }
    PartnerId: string;
    SetWindowArgs(args)
    {
        this.chartOfAccountTypeCode = args.AccountTypeCode;
        this.cardId = args.CardId;
        this.PartnerId = args.PartnerId;
    }

    public columns: any[] = null;
    BuildColumns()
    {
        this.columns = [];
        this.columns.push({
            FieldName: "DisplayNumber",
            DataTypeCode: "String",
            Display: TextCodeTranslator.Translate("GLAccount.F.DisplayNumber"),
            Styles: { width: "150px" },
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: "LocalName",
            DataTypeCode: "String",
            Display: TextCodeTranslator.Translate("GLAccount.F.LocalName"),
            Styles: { width: "200px" },
            IsCustomTemplate: true
        });
        // this.columns.push({
        //     FieldName: 'EnglishName',
        //     DataTypeCode: 'String',
        //     Display: TextCodeTranslator.Translate("GLAccount.F.EnglishName"),
        //     Styles: { width: '200px' },
        //     IsCustomTemplate: true
        // });
        this.columns.push({
            FieldName: "InternalNumber",
            DataTypeCode: "String",
            Display: TextCodeTranslator.Translate("GLAccount.F.InternalNumber"),
            Styles: { width: "150px" },
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: "CurrencyCode",
            DataTypeCode: "String",
            Display: TextCodeTranslator.Translate("GLAccount.F.CurrencyCode"),
            Styles: { width: "100px" },
            IsCustomTemplate: true
        });
    }

    DataSource = {
        pageSize: 30,
        rowCount: null,
        sortingDir: "Ascending",
        getRows: (
            skip: number,
            take: number,
            sortingCol: string,
            sortingDir: string,
            getCount: boolean,
            searchFields?: string,
            filters: ApiQueryFilters = null
        ) =>
        {
            var tempo = this.GetRows(filters);
            return tempo;
        }
    };

    ReloadData()
    {
        this.onQueryChangeEvent.emit({ Filters: new ApiQueryFilters() });
    }

    GetRows(filters: ApiQueryFilters = null)
    {
        var filters = new ApiQueryFilters();

        filters.PageSize = 50;
        filters.PageIndex = 0;
        filters.GetCount = true;

        filters.SortBy = "DisplayNumber";
        filters.SortDirection = "Ascending";

        filters.addAdditionalFilter(
            "ChartOfAccountsTypeCode",
            this.chartOfAccountTypeCode,
            null,
            null,
            "Equals",
            false,
            false,
            false,
            "string"
        );
        filters.addAdditionalFilter(
            "SearchFields",
            this.searchFields,
            null,
            null,
            "Contains",
            false,
            true,
            false,
            "String"
        );

        return this.entityListService.getByFilters("GLAccount", filters);
    }

    CloseButtonClicked()
    {
        this.CurrentSession.CloseCurrentWindowEmit(null);
    }

    searchFields: string;
    TextChanged(searchtext)
    {
        if (searchtext != null && searchtext != undefined) {
            this.searchFields = searchtext;
            this.searchFields = this.searchFields.trim();
            if (this.searchFields != null && this.searchFields != undefined)
                this.SearchFieldchangeevent.emit(this.searchFields);
            else this.SearchFieldchangeevent.emit("");
        } else {
            this.searchFields = "";
            this.SearchFieldchangeevent.emit("");
        }
    }
    onRowSelected($event)
    {
        var glaccount = $event.rowData;
        this.chartOfAccountTypeCode = glaccount.ChartOfAccountsTypeCode;
        var glaccountId = $event.rowData["Id"];

        if (glaccountId) {
            this.connectCard(glaccountId);
        }
        // this.CurrentSession.CloseCurrentWindowEmit(glaccountId);
    }

    lastSelectedGLAccount: string;
    private connectCard(glaccountId: any, skipConnectedCardsValidation: boolean = false)
    {
        this.CurrentSession.StartBusyIndicatorSaving();
        this.ValidationErrorsList = [];
        this.lastSelectedGLAccount = glaccountId;

        this.gLAccountExtendedPMService
            .ConnectCardToGLAccount(glaccountId, this.cardId, skipConnectedCardsValidation)
            .subscribe((response: ServiceResponse) =>
            {
                this.CurrentSession.StopBusyIndicator();

                var result = response.Result;
                if (response.HasError) {
                    // this.ValidationErrorsList = response.ErrorsArray;
                    var errorsString = response.ErrorsArray.join(", ");

                    this.handleError(errorsString);
                } else {
                    if (result == "ok")
                        this.CurrentSession.CloseCurrentWindowEmit(glaccountId);
                }
            });
    }

    private handleError(errorsString: string)
    {
        if (this.chartOfAccountTypeCode == "4" || this.chartOfAccountTypeCode == "3" || this.PartnerId =="AC") // 4- Vendor 3- Customer
        {
            if(!this.warningMessageShown)
            {
                this.showWarningMessage(errorsString);
            }
        }
        else {
            this.showErrorMessage(errorsString);
        }

    }


    private showErrorMessage(errorsString: string)
    {
        let errorMsg = new MessageWindow();
        errorMsg.RTL = this.isRTL;
        errorMsg.Width = 400;
        errorMsg.Show(errorsString);
    }

    private showWarningMessage(errorsString: string)
    {
        let warningMsg = new ConfirmWindow();
        warningMsg.ShowNoButton = false;
        warningMsg.ShowCancelButton = true;
        warningMsg.YesButtonText = TextCodeTranslator.Translate("General.B.Ok");
        warningMsg.Width = 400;
        warningMsg.Show(errorsString);
        this.warningMessageShown = true;
        warningMsg.WindowClosed.subscribe((result: any) =>
        {
            if (warningMsg.Yes) {
                this.connectCard(this.lastSelectedGLAccount, true);
            }
        });
    }
}
