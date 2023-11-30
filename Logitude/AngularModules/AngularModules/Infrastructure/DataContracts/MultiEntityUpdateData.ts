import { AutomationSetValue } from './AutomationSetValue';
import { MultiEntityUpdateDataEntity } from './MultiEntityUpdateDataEntity';

export class MultiEntityUpdateData {
    public ObjectTableId: string;
    public ObjectTableName: string;
    public UserId: string;
    public SetValueLists: AutomationSetValue[];
    public Entities: MultiEntityUpdateDataEntity[];
}

