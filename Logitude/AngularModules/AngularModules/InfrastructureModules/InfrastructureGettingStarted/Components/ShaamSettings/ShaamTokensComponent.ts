import { ChangeDetectorRef, Component, EventEmitter, Output } from '@angular/core';
import { CommunicationLogListService } from 'Common/Services/StandardLists/CommunicationLogListService';
import { BaseComponent } from 'Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ApiQueryFilters } from 'Infrastructure/DataContracts/ApiQueryFilters';
import { EntityResourceService } from 'Infrastructure/Services/EntityResourceService';
import { AmitalGatewayUtil } from 'Infrastructure/Utilities/AmitalGatewayUtil';
import { SessionLocator } from 'Infrastructure/Utilities/SessionLocator';
import { TextCodeTranslator } from 'Infrastructure/Utilities/TextCodeTranslator';
import { ShaamWebService } from 'Shipment/Services/ShaamWebService';

@Component({
    selector: 'ShaamTokensComponent',
    templateUrl: './ShaamTokensComponent.html',
    styles: [`
        :host() { 
            height: 100%; 
            display: flex;
            flex-flow: column;
            align-items: stretch;
        }

        .buttons {
            flex: 0 1 auto;
        }
        
        .grid-container {
            flex: 1 1 auto;
            position: relative;
            padding:5px;
        }
    `]
})
export class ShaamTokensComponent extends BaseComponent {
    DataContext: ShaamTokensComponent = this;
    ObjectTableName: string = "Customs.ConfirmationNumberTokenLog";
    columns: any[] = null;
    finishBuildColumns: boolean = false
    loggedUserCode: string = SessionLocator.LoggedUserPM.Code;
    linkToCodeForToken: string = "";

    constructor(
        private _entityResourceService: EntityResourceService,
        private cd: ChangeDetectorRef,
    ) {
        super();
    }

    ngOnInit() {
        this.BuildColumns()
        this.initLinkToCodeForToken();
    }

    @Output() MenuHeaderchangeevent = new EventEmitter();
    @Output() onQueryChangeEvent = new EventEmitter();

    DataSource = {
        pageSize: 30,
        rowCount: null,
        sortingCol: "CreateDate",
        sortingDir: "Descending",
        getRows: (skip: number, take: number, sortingCol: string, sortingDir: string, getCount: boolean, searchFields?: string, filters: ApiQueryFilters = null) => {
            const res = this.getRows(skip, take, sortingCol, sortingDir, getCount, searchFields, filters)
            return res;
        }
    };

    getRows(skip, take, sortingCol, sortingDir, getCount: boolean, searchfields?: string, filters: ApiQueryFilters = null) {
        const pageSize: number = take;
        const page: number = pageSize == 0 ? 1 : (skip / pageSize) + 1;

        return new Promise(resolve => resolve(new ShaamWebService().getTokens(pageSize, page)));
    }

    BuildColumns() {
        const columnsList: GridColumn[] = [
            { FieldName: 'userCode', Display: 'User Code', Styles: { width: '100px' } },
            { FieldName: 'createDate', Display: 'Create Date', Styles: { width: '140px' }, isTemplate: true },
            { FieldName: 'refreshExpierDate', Display: 'Refresh Expier Date', Styles: { width: '140px' }, isTemplate: true },
            { FieldName: 'refreshToken', Display: 'Refresh Token', Styles: { width: '800px' } },
            { FieldName: 'accessExpireDate', Display: 'Access Expire Date', Styles: { width: '140px' }, isTemplate: true },
            { FieldName: 'accessToken', Display: 'Access Token', Styles: { width: '460px' } },
            { FieldName: 'isActive', Display: 'IsActive', Styles: { width: '60px' }, isTemplate: true },
        ]

        this.columns = [];
        columnsList.forEach(col => {
            const gridCol: any = {
                FieldName: col.FieldName,
                DataTypeCode: col.DataTypeCode || 'string',
                Display: col.Display || col.FieldName,
                Styles: col.Styles || { width: '220px' },
                IsCustomTemplate: col.IsCustomTemplate || true,
                ServerSideSortable: col.ServerSideSortable || true,
                SortByName: col.SortByName || col.FieldName,
            }

            if (col.isTemplate) {
                gridCol.HtmlListComponentName = 'ShaamTokenTemplate';
                gridCol.HtmlListComponentUrl = './CustomsModules/CustomsListTemplates/Components/ShaamTokenTemplate';
            }

            this.columns.push(gridCol);
        })

        this.finishBuildColumns = true;
    }

    async openLinkToCodeForToken() {
        var t = new Date();
        t.setSeconds(t.getSeconds() + 30);

        while (!this.linkToCodeForToken && t > (new Date()))
            await new Promise(res => setTimeout(() => { res('') }, 1000));

        if (!this.linkToCodeForToken)
            throw TextCodeTranslator.Translate('General.B.Erroroccured')

        localStorage.setItem('shaamTokenRedirect', location.href);

        if (!AmitalGatewayUtil.Instance.AmitalBrowserInUse)
            location.href = this.linkToCodeForToken;
        else {
            AmitalGatewayUtil.Instance.SendRequestToUnifreightAsync(
                "",
                "",
                'redirect',
                AmitalGatewayUtil.Instance.GetDefaultUnifreightMessageM(),
                'this.linkToCodeForToken',
                false);
        }
    }

    async initLinkToCodeForToken() {
        this.linkToCodeForToken = await new ShaamWebService().getLinkToCodeForToken(SessionLocator.LoggedUserPM.EnglishName).toPromise();
    }

    refreshTable() {
        this.finishBuildColumns = false;
        this.cd.detectChanges()
        this.finishBuildColumns = true;
        this.cd.detectChanges()
    }
}


type GridColumn = {
    FieldName: string;
    DataTypeCode?: string;
    Display?: string;
    Styles?: any;
    IsCustomTemplate?: boolean;
    ServerSideSortable?: boolean;
    SortByName?: string;
    isTemplate?: boolean;
};
