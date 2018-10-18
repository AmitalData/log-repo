import { INF_MSG_GenericResponseData } from './INF_MSG_GenericResponseData';
import { ObservableCollection } from '../../../Infrastructure/Utilities/ObservableCollection';

export class RequiredDocumentResponseData extends INF_MSG_GenericResponseData {

    public ApplicationID: string;
    public Title: string;
    public VerificationDecisionType: string;
    public DocumentType: string;
    public DocumentTypeName: string;
    public DocumentTypeVisibility: string;
    public DocumentNumber: string;
    public DocumentWorkerName: string;
    public Remarks: string;
    public ReplacingDocumentId: string;
    public ReplacingDocumentIdVisibility: string;

    public DocumentConnectedEntitiesList: Array<DocumentConnectedEntitiesResult>;
}

export class DocumentConnectedEntitiesResult {
    public EntityType: number;
    public EntityTypeName: string;
    public EntityNumber: string;
}

