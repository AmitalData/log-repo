import {DocumentsFilingPM} from './DocumentsFilingPM';
import {UIProperties, UIProperty} from '../../Infrastructure/Components/LogitudeComponents/UIProperties';
export class DocumentsFilingMetaDataValuePM
{

    public UIProperties: UIProperties;
    constructor( entityParentPM: DocumentsFilingPM) {
        this.UIProperties = new UIProperties;
        this.IsDirty = false;
    }

    private changeSetOp: string;
    public get ChangeSetOp() { return this.changeSetOp; }
    public set ChangeSetOp(newValue: string) { this.changeSetOp = newValue; this.MarkAsDirty(); }
    private entityParentPM;
    public get EntityParentPM() { return this.entityParentPM; }
    public set EntityParentPM(newValue: any) { this.entityParentPM = newValue; this.MarkAsDirty(); }
    public OldEntityPM: DocumentsFilingMetaDataValuePM;

    private id: string;
    public get Id() { return this.id; }
    public set Id(newValue: string) { this.id = newValue; }

    private tenant: number;
    public get Tenant() { return this.tenant; }
    public set Tenant(newValue: number) { this.tenant = newValue; }

    private documentsFilingId: string;
    public get DocumentsFilingId() { return this.documentsFilingId; }
    public set DocumentsFilingId(newValue: string) { this.documentsFilingId = newValue; }

    private documentsMetaDataTypeId: string;
    public get DocumentsMetaDataTypeId() { return this.documentsMetaDataTypeId; }
    public set DocumentsMetaDataTypeId(newValue: string) { this.documentsMetaDataTypeId = newValue; }

    private documentsMetaDataTypeCode: string;
    public get DocumentsMetaDataTypeCode() { return this.documentsMetaDataTypeCode; }
    public set DocumentsMetaDataTypeCode(newValue: string) { this.documentsMetaDataTypeCode = newValue; }

    private metaDataValue: string;
    public get MetaDataValue() { return this.metaDataValue; }
    public set MetaDataValue(newValue: string) { this.metaDataValue = newValue; }

    public UniqueKey: string;

    public IsDirty: boolean;
    MarkAsDirty() {
        this.IsDirty = true;
        if (this.entityParentPM) {
            this.entityParentPM.MarkAsDirty();
        }
    }
}