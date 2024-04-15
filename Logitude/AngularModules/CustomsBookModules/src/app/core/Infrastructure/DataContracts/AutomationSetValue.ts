

import {Injectable} from '@angular/core';

@Injectable()

export class AutomationSetValue {
    public ObjectFieldCode: string;
    public OperatorCode: string;
    public Value: string;
    public FieldName: string;
    public DataTypeCode: string;
    public IsCustomField: boolean;
}
