import { TextCodeTranslationPipe } from "../../Controls/Pipes/TextCodeTranslationPipe";
import { CustomPickListList } from "../EntityLists/CustomPickListList";
import { ObjectFieldPM } from "./ObjectFieldPM";

export class DeploymentPackageDetails {

    public Name: string;
    public Code: string;
    public Description: string;
    public CustomFields: Array<CustomFields>;
    public CustomPickLists: Array<CustomPickListItem>;
    constructor() {
        this.Name = "";
        this.Code = "";
        this.Description = "";
        this.CustomFields = [];
    }

}
export class CustomFields {
    public Code: string;
    public FieldCode: string;
    public Name: string;
    public DefaultText: string;
    public DataTypeName: string;
    public ObjectTableName: string;
    public LookUpTableName: string;
    public HelpText: string;
    public SearchFields: string;
    public CustomPickListCode: string;
    public NumberOfDigits: number;
    public DigitsAfterPoint: number;
    public MaxLength: number;
    public MinLength: number;
    public IsRequiered: boolean;
    public DisplayOnly: boolean;
    public MultiLine: boolean;

    constructor(objectField: ObjectFieldPM) {
        this.Code = objectField.Code;
        this.FieldCode = objectField.FieldCode;
        this.Name = objectField.FieldName;
        this.DefaultText = new TextCodeTranslationPipe().transform(objectField.FullNameTextCodeCode);
        this.DataTypeName = objectField.DataTypeName;
        this.ObjectTableName = objectField.ObjectTableName;
        this.LookUpTableName = objectField.ObjectTable_LookUpTableName;
        this.HelpText = objectField.HelpTextCodeDefaultText;
        this.SearchFields = objectField.SearchFields;
        this.CustomPickListCode = objectField.CustomPickListCode;

        this.NumberOfDigits = objectField.NumberOfDigits;
        this.DigitsAfterPoint = objectField.DigitsAfterPoint;
        this.MaxLength = objectField.MaxLength;
        this.MinLength = objectField.MinLength;

        this.IsRequiered = objectField.IsRequiered;
        this.DisplayOnly = objectField.DisplayOnly;
        this.MultiLine = objectField.MultiLine;
    }
  
}
export class CustomPickListItem{
    public Code: string;
    public Value: string;
    public IsMultipleChoice: boolean;

    constructor(customPickList: CustomPickListList) {
        this.Code = customPickList.Code;
        this.Value = customPickList.Value;
        this.IsMultipleChoice = customPickList.IsMultipleChoice;
    }
}
