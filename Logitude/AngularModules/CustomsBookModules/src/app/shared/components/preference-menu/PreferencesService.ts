
import { Injectable } from '@angular/core';
import { API_MainService } from '../../../core/API_MainService';

@Injectable({
    providedIn: 'root'
})

export class PreferencesService {
    constructor(private apiService: API_MainService) {
    }

    private preferences = {
        [PreferenceType.Background]: Array(7).fill('#F3F5F7'), // default colors
        [PreferenceType.Text]: Array(7).fill('#1C1C1C') // default colors
    };

    setPreference(level: number, color: string, type: PreferenceType) {
        this.preferences[type][level - 1] = color; // level is 1-based, array is 0-based
    }

    getPreference(level: number, type: PreferenceType): string {
        return this.preferences[type][level - 1] || (type === PreferenceType.Background ? '#F3F5F7' : '#1C1C1C');
    }

  

}

export class CB_Preference {
    Id: string;
    Tenant: number;
    BackgroundColor: string;
    TextColor: string;
    Level: number;
    UserId: string;
}

export enum PreferenceType {
    Background = 'background',
    Text = 'text'
}