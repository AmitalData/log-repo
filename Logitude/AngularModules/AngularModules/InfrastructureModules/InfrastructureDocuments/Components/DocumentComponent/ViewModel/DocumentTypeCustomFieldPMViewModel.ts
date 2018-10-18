import {DocumentTypeCustomFieldPM} from '../../../../../Common/EntityPMs/DocumentTypeCustomFieldPM';
import {BaseComponent} from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {Guid} from '../../../../../Infrastructure/Utilities/Guid';

export class DocumentTypeCustomFieldPMViewModel extends BaseComponent{
    FieldDataTypeCode: string;
    Name: string

    EntityPM: DocumentTypeCustomFieldPM;
    FieldCode: string;
    Id: string;
    MultiLine: boolean;
    public IsLoad: boolean;
    Tiger: any;
    IsFirstTime: boolean = true;


    keyYes: string;
    keyNo: string;
    private fieldValue: any;
    public get FieldValue() { return this.fieldValue; }
    public set FieldValue(newValue: any) {

        if (this.fieldValue != newValue) {
            this.fieldValue = newValue;
            if (this.Tiger && (this.FieldDataTypeCode == "Date" || this.FieldDataTypeCode == "DateTime") && !this.IsFirstTime) {
                this.Tiger.EditCustomField(this);
            }
            else this.IsFirstTime = false;
        }


    }



   
  public  constructor(documentTypeCustomFieldPM: DocumentTypeCustomFieldPM , tiger:any) {
        super();
    
        this.EntityPM = documentTypeCustomFieldPM;
        this.Tiger = tiger;
        this.MultiLine = documentTypeCustomFieldPM.MultiLine;
        this.keyNo = Guid.newGuid();
        this.keyYes = Guid.newGuid();


        this.Id = documentTypeCustomFieldPM.Id;
        this.FieldCode = documentTypeCustomFieldPM.FieldCode;
        this.FieldDataTypeCode = documentTypeCustomFieldPM.FieldDataTypeCode;
        this.Name = documentTypeCustomFieldPM.Name;
    }




}