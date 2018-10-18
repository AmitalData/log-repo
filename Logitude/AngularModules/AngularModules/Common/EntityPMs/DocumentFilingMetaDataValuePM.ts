import {DocumentsFilingPM} from './DocumentsFilingPM';
import {UIProperties, UIProperty} from '../../Infrastructure/Components/LogitudeComponents/UIProperties';
export class DocumentFilingMetaDataValuePM
{

    public UIProperties: UIProperties;
    constructor( ) {
        this.UIProperties = new UIProperties;
        this.IsDirty = false;
    }
   
    private changeSetOp: string;
    public get ChangeSetOp() { return this.changeSetOp; }
    public set ChangeSetOp(newValue: string) { this.changeSetOp = newValue; }

    private id:string;
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

    private metaDataValue: string;
    public get MetaDataValue() { return this.metaDataValue; }
    public set MetaDataValue(newValue: string) { this.metaDataValue = newValue; }
   
    public IsDirty: boolean;
  
}