import {EditDocumentComponent} from './EditDocumentComponent';
import {StimulsoftViewerComponent} from '../../../../Infrastructure/Components/StimulsoftComponent/StimulsoftViewerComponent';
import {ReportFliter} from '../../../../Report/Components/Filters/ReportFliter';
import {ReportsTemplateList} from '../../../../Common/EntityLists/ReportsTemplateList';
import {EntityPartner} from '../../../../Infrastructure/DataContracts/EntityPartner';
import {BuildStimulReportResult} from './DocsOut/Filters/BuildStimulReportResult';

export class StimulsoftArg {
    public DocumenttypeCode: string;
    public Tenant: number;
    public DocumenttypetemplateId: string;
    DoucmentTemplateByte: any;
    public CurrentDocumentOutId: string;
    public DocumenttypecopyId: string;
    public EntityId: string;
    EditDocumentComponent: EditDocumentComponent;
    public DocumentTypeId: string;
    public ReportsPreviewComponent: any;
    public BuildStimulReportResult: BuildStimulReportResult;



    public StimulsoftViewerComponent: StimulsoftViewerComponent;
    IsShowShiftToolbar: boolean;
    IsShowExportPrinttoPDF: boolean = false;
    IsShowExportMicrosoftExcel: boolean = false;
    IsShowSendButton: boolean = false;
    ReportKey: string;
    NumberOfPage: number;
    PagesCount: number;
    TypePage: string;
    ScreenHeight: number;
    ScreenWidth: number;
    PartnersObslist: EntityPartner[];
    ReportsTemplateLists: ReportsTemplateList[] = [];
    DefaultTemplateId: string;
    IsManageStimul: boolean;
    ReportFliter: ReportFliter;
    ShowStimulHeader: boolean;
    ShowStimulFooter: boolean;
    ShowReportsTemlatesLists: boolean = false;
    ReportFilterConmponent: any;
    TemplateDescription: string;
    IsReset: boolean;
    constructor() {

    }
}