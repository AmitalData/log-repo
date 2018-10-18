export class SendEmailArgs {
    public Ticket: any;
    public IsInternal: boolean;
    public InternalCorrespondenceLinesCount: number;
}

export class TicketStagesArgs {
    public Code: string;
    public Name: string;

    public IsEnabled = true;
    public ItemOpacity: number;
    //get IsEnabled() {
    //    return this.isEnabled;
    //}
    //set IsEnabled(value: boolean) {
    //    if (this.isEnabled != value) {
    //        this.isEnabled = value;
    //    }
    //}
}
export class TicketClosureArgs {
    public Ticket: any;
    public StageCode: string;
}

export class ActivityInputArgs {
    public Activity: any;
    public TypeCode: string;
    public IsAddCustomerAllowed: boolean;
    public IsEditMode: boolean;
    public IsEnabled: boolean;
    public CustomerId: string;
    public QuoteId: string;
    public Subject: string;
    public DueDate: Date;
    public OpportunityId: string;
    public CallWithId: string;
    public IsOpen: boolean;
    public IsMarkedCompleted: boolean;
    public TicketId: string;

}

export class OpportunityArgs {
    public Entity: any;
    public IsNew: boolean;
    public IsAddCustomerVisible: boolean;

}

export class NewTicketArgs {
    public TicketNumber: string;
    public CompanyId: string;
}

export class InviteeArgs {
    public Entity: any;
}