import { EventEmitter} from '@angular/core';
import {EditComponent} from '../Components/EditComponent/EditComponent';

import {Injectable} from '@angular/core';

@Injectable()

export class EntityArgs {
    public EntityPM: any;
    public OriginEntity: any;
    public EditComponent: EditComponent;
    public ObjectTableName: string;
    public EntityParentPM: any;
    public IsNewEntity: boolean = false;
    public SkipCtor: boolean = false;
    public IsFromStandAloneScreen: boolean = false;
    public PreSelectedTabCode: string = null;
    public EditComponentArgument: any = null;
    public EntityArgEventEmitter: EventEmitter<any> = new EventEmitter<any>();
    public SendMessage(token: any) {//itzik test 
        this.EntityArgEventEmitter.emit(token);
    }
}

