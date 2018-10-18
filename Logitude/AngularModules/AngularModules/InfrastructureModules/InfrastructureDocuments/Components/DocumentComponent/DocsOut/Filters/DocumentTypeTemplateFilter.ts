import {DocumentTypeTemplatePM} from '../../../../../../Common/EntityPMs/DocumentTypeTemplatePM';
import {EditableFieldPosition} from './EditableFieldPosition';

export class DocumentTypeTemplateFilter {
    Tenant: number;
    Id: string;
    Body: string;
    TemplateType: string;
    InActive: boolean;
    DocumentOutId: string;
    Processtype: string;
    Subject: string;
    TemplatePM: DocumentTypeTemplatePM;
    PageIndex: number;
    EditableFieldLists: EditableFieldPosition[];
    ReportKey: String;
    constructor() {

    }
}