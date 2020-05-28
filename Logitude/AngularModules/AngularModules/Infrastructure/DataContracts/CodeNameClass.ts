export class CodeNameClass {
    public Code: string;
    public Name: string;
    public LocalName: string;
    public AdditionalField: string;
    public Code_Int: number;
    public DisplyText: string;
    public FromDate: Date;
    public ToDate: Date;
    constructor(myCode: string = null, myName: string = null, myLocalName: string = null, additionalField: string = null) {
        this.Code = myCode;
        this.Name = myName;
        this.LocalName = myLocalName;
        this.AdditionalField = additionalField;
    }
}
