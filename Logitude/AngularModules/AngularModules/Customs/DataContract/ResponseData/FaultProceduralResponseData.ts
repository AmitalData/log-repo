import { INF_MSG_GenericResponseData } from './INF_MSG_GenericResponseData';
import { ObservableCollection } from '../../../Infrastructure/Utilities/ObservableCollection';

export class FaultProceduralResponseData extends INF_MSG_GenericResponseData {

    public ApplicationID: string;
    public ResponseStatusXML: string;
    public FaultGeneralDetailList: Array<FaultGeneralDetailResult>;
}

export class FaultGeneralDetailResult {

    public ProceduralFaultID: string;
    public CustomsHouse: string;
    public CustomsHouseName: string;
    public DeclarationId: string;
    public ProceduralFaultCode: string;
    public ProceduralFaultCodeName: string;
    public CreateDate: string;
    public AgentExternalID: string;
    public ExternalID: string;
    public ProceduralFaultStatus: string;
    public ProceduralFaultStatusName: string;
    public ProceduralFaultInputProcessName: string;
    public ImporterExporterResponsibilityName: string;
    public FaultAdittionalInformationList: Array<FaultAdittionalInformationResult>;
    public FaultAdittionalInformationListObs: ObservableCollection;
    public FieldsPathtoFaultList: Array<FieldsPathtoFaultResult>;

}

export class FaultAdittionalInformationResult {

    public AgentInDeclarationName: string;
    public AgentResponsibilityID: string;
    public AgentResponsibilityName: string;
    public ResponsibilityID: string;
    public ResponsibilityName: string;
    public ProceduralFaultInputProcess: string;
    public ProceduralFaultInputProcessName: string;
    public RansomViolationType: string;
    public RansomViolationTypeName: string;
    public RansomViolationSum: string;
    public FelonyType: string;
    public FelonyTypeName: string;
    public Severity: string;
    public SeverityName: string;
    public Scoring: string;
    public ExporterImporterInDeclarationName: string;

}

export class FieldsPathtoFaultResult {

    public SequenceNumber: string;
    public DisplayPath: string;

}