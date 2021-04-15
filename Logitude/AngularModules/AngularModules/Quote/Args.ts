import {QuotePM} from './EntityPMs/QuotePM';

export class NewQuoteComponentArgs {
    public Quote: any = null;
    public IsCopyFromQuote: boolean = false; 
    public DefaultCustomerId: string = null;
    public OpportunityId: string = null;
    public IsCreatedFromTicket: boolean = false;
    public TicketCreateDate: Date;
    public ConvertTransportMode: boolean = false;  
}

export class QuoteEventNotesArgs {
    public EntityPM: any;
    public NotesHeader: string = "Notes";
    public ShowClosingReason: boolean = false;
    public IsConvertQuoteType: boolean = false;
}
