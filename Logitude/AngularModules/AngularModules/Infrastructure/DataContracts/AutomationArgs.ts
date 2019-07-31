import {Injectable} from '@angular/core';


@Injectable()

export class AutomationArgs {

    public Id: string;
    public Tenant: number;
    public Order: number;
    public RecipientType: string;
    public RecipientValue: string;
    public AutomationsId: string;

}
