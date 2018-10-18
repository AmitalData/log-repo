import {DocumentTypeCustomFieldPM} from '../../../../../../Common/EntityPMs/DocumentTypeCustomFieldPM';
import {EditDocumentComponent} from '../../EditDocumentComponent';
import {DocumentCustomFieldsComponent} from '../../DocumentCustomFieldsComponent';
import {GeneratedDocumentCustomFieldComponent} from '../../GeneratedDocumentCustomFieldComponent';

export class DocumentCustomFieldsArgs {
    public Tenant: number;
    public DocumentTypeId: string;
    public  EntityId: string;
    public ObjectTableId: string;
    IsChangeCustomField: boolean;
    IsEditCustomField: boolean;
    public DocumentCustomFields: any;
    public EditCustomField: boolean;
    public editDocumentComponent: EditDocumentComponent;
    public DocumentTypeCustomFieldLists: DocumentTypeCustomFieldPM[];
    public ObjectTableName: string;
    public EntityPM: any;
    public ScreenCode: string;
    GeneratedDocumentCustomFieldComponent: GeneratedDocumentCustomFieldComponent;

    constructor() {

    }
}