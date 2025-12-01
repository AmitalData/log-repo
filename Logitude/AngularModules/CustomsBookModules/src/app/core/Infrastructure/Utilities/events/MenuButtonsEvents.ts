import {Output, EventEmitter} from '@angular/core';
import {MenuButtonPM} from '../../EntityPMs/MenuButtonPM';

export class MenuButtonsEvents {
    @Output() static MenuButtonsStateChanged: EventEmitter<MenuButtonsStateChangedEventArgs> = new EventEmitter<MenuButtonsStateChangedEventArgs>();
}

export class MenuButtonsStateChangedEventArgs {
    MenuButtonsStates: { [EventCode: string]: boolean; } = {}
}