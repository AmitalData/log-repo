export class CodeNameClass{


    private code: string; 
    private name: string;
    private integerCode: number;
    private additionalField: string;
    private dateTime: Date;
    private checked: boolean;
    public get Checked() { return this.checked; }
    public set Checked(value: boolean) { this.checked = value; }


    public get Code() { return this.code; }
    public set Code(value: string) { this.code = value; }

    public get Name() { return this.name; }
    public set Name(value: string) { this.name = value; }

    public get IntegerCode() { return this.integerCode; }
    public set IntegerCode(value: number) { this.integerCode = value; }

    public get AdditionalField() { return this.additionalField; }
    public set AdditionalField(value: string) { this.additionalField = value; }

    public get DateTime() { return this.dateTime; }
    public set DateTime(value: Date) { this.dateTime = value; }

    constructor(myCode: string = null, myName: string = null) {
        this.Code = myCode;
        this.Name = myName;
    }
}