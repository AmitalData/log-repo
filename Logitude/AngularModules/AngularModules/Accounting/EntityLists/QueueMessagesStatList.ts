
export class QueueMessagesStatList {   
    Id: number;
    QueueDefinitionCode: string;
    CreateDateTime: Date;
    Status: number;
    MessageBody: string;
    NextRunDateTime: Date;
    ProcessingDateTime: Date | null;
    CompleteDateTime: Date | null;
    RetryNumber: number;
    Tenant: number;
    HashCode: string;
	
}
 