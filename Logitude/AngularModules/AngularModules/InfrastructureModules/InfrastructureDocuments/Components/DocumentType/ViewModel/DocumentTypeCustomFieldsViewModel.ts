
import {DocumentTypeCustomFieldPM} from '../../../../../Common/EntityPMs/DocumentTypeCustomFieldPM';

export class DocumentTypeCustomFieldsViewModel {
    public Id: string;


    public DivSelectBackgroud: string;
    public FieldCode: string;
    public FieldDataTypeCode: string;
    public DefaultValue: string;
    public EntityPM: DocumentTypeCustomFieldPM;
    
    public IsRequired: boolean;
    public InActive: boolean;

    public KeyInActive: string;
    public KeyInRequired: string;

    //private inActive: boolean = false;
    //get InActive() { return this.inActive; }
    //set InActive(newValue: boolean) {
    //    if (this.inActive != newValue) {
    //        this.inActive = newValue;
    //        this.EntityPM.InActive = newValue;
    //    }
    //}



    //private isRequired: boolean = false;
    //get IsRequired() { return this.isRequired; }
    //set IsRequired(newValue: boolean) {
    //    if (this.IsRequired != newValue) {
    //        this.IsRequired = newValue;
    //        this.EntityPM.IsRequired = newValue;
    //    }
    //}
    Name: string;

    public constructor(entityPM: any) {

        this.EntityPM = entityPM;
        this.Id = entityPM.Id;
        this.KeyInActive = entityPM.Id + "Active";
        this.KeyInRequired = entityPM.Id + "Required";
        this.DefaultValue = entityPM.DefaultValue;
        this.FieldCode = entityPM.FieldCode;
        this.FieldDataTypeCode = entityPM.FieldDataTypeCode;
        this.Name = entityPM.Name;
        this.IsRequired = entityPM.IsRequired;
        this.InActive = entityPM.InActive;
    }


}