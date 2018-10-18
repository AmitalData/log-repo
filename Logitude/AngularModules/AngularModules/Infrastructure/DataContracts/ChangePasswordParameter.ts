

import {Injectable} from '@angular/core';

@Injectable()

export class ChangePasswordParameter {

    public Email: string;
    public CurrentPassword: string;
    public ContactId: string;
    public NewPassword: string;

}

