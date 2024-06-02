import { ChangeDetectorRef, Component, EventEmitter, Output } from '@angular/core';
import { GridColumn } from 'Common/Components/Maintenance/AmitalAPI/components/LogitudeGridSimpleComponent';
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
    public static readonly shaamTokenRedirect = 'shaamTokenRedirect'

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
            { FieldName: 'accessExpireDate', Display: 'Access Expire Date', Styles: { width: '140px' }, isTemplate: true },
            { FieldName: 'isActive', Display: 'IsActive', Styles: { width: '60px' }, isTemplate: true },
            { FieldName: 'refreshToken', Display: 'Refresh Token', Styles: { width: '200px' } },
            { FieldName: 'accessToken', Display: 'Access Token', Styles: { width: '200px' } },
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

        localStorage.setItem(ShaamTokensComponent.shaamTokenRedirect, location.href);

        if (AmitalGatewayUtil.Instance.isUnifreightHost)
            AmitalGatewayUtil.Instance.SendRequestToUnifreightAsync(
                "",
                "",
                AmitalGatewayUtil.Instance.DeclarationMessaging.OpenNewBrowser,
                AmitalGatewayUtil.Instance.GetDefaultUnifreightMessageM(),
                '"' + this.linkToCodeForToken + '"',
                false);
        else
            open(this.linkToCodeForToken);
   		
    }
    async initLinkToCodeForToken() {
        const user: string =  new URLSearchParams(window.location.search).get('userCode') || SessionLocator.LoggedUserPM.Code || SessionLocator.LoggedUserPM.EnglishName;
        this.linkToCodeForToken = await new ShaamWebService().getLinkToCodeForToken(user).toPromise();
    }
    refreshTable() {
        this.finishBuildColumns = false;
        this.cd.detectChanges()
        this.finishBuildColumns = true;
        this.cd.detectChanges()
    }
}

