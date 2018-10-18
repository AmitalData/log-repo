export class AmendmentView {
    ParentLine: number;
    ParentEntityName: string;
    ErrorType: string;
    Field: string;
    Line: number;
    EntityName: string;
    FieldNameTextCode: string;
    TableNameTextCode: string;
    LineNumber: string;
    Code: string;
    MessageError: string;
    OldValue: string;
    NewValue: string;
    FieldErrors: field[];
}

export class error {
    Code: string;
    ListVersionID: string;
    MessageError : string;
    ConstraintID : string;
    OldValue     : string;
    NewValue     : string;
}

export class field extends error
{
    Fieldcode    : string;
    Code         : string;
    MessageError : string;
    OldValue     : string;
    NewValue: string;
}