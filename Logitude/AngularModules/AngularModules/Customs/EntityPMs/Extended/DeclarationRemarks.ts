export class DeclarationRemarks {
    StatusId: string;
    StatusName: string;
    StatusDate: string;
    StatusComment: string;
    StatuseTime?: string;


}
export class Remarks {
    id: string;
    StatusItemlist: DeclarationRemarks[];
    ErrMessage: string;
}

