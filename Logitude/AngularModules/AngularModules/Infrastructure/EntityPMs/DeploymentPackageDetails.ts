import { ObjectFieldPM } from "./ObjectFieldPM";

export class DeploymentPackageDetails {

    public Name: string;
    public Code: string;
    public Description: string;
    public CustomFields: Array<CustomFields>;
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
    public DataTypeName: string;
    public ObjectTableName: string;
    public LookUpTableName: string;
    public HelpText: string;
    public SearchFields: string;
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
        this.DataTypeName = objectField.DataTypeName;
        this.ObjectTableName = objectField.ObjectTableName;
        this.LookUpTableName = objectField.ObjectTable_LookUpTableName;
        this.HelpText = objectField.HelpTextCodeDefaultText;
        this.SearchFields = objectField.SearchFields;

        this.NumberOfDigits = objectField.NumberOfDigits;
        this.DigitsAfterPoint = objectField.DigitsAfterPoint;
        this.MaxLength = objectField.MaxLength;
        this.MinLength = objectField.MinLength;

        this.IsRequiered = objectField.IsRequiered;
        this.DisplayOnly = objectField.DisplayOnly;
        this.MultiLine = objectField.MultiLine;
    }
}
