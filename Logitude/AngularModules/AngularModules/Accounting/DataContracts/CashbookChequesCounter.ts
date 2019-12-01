export class CashbookChequesCounter
{
    CashChequesCount: number = 0;
    PostdatedChequesCount: number = 0;

    public get AllChequesCount() : number {
        return this.CashChequesCount + this.PostdatedChequesCount;
    }

}
