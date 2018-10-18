
import {ChangeField} from './ChangeField';
import {EntityChangeAutomation} from './EntityChangeAutomation';
export class EntityChangeAutomationsSummary {
    public Id: string;
    public ChangeFieldsList: ChangeField[];

    public EntityChangeAutomationList: EntityChangeAutomation[];
 
    public ObjectTableName: string;
}