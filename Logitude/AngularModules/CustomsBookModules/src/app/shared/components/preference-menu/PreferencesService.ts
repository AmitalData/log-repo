
import { Injectable } from '@angular/core';
import { API_MainService } from '../../../core/API_MainService';
import { BehaviorSubject } from 'rxjs';
import { SessionInfo } from '../../../core/Infrastructure/Utilities/SessionInfo';

@Injectable({
    providedIn: 'root'
})

export class PreferencesService {
    constructor(private apiService: API_MainService) {
    }
    allPreferences: BehaviorSubject<CB_Preference[]> = new BehaviorSubject<CB_Preference[]>([]);

    private preferences = {
        [PreferenceType.Background]: Array(5).fill('#F3F5F7'), // default colors
        [PreferenceType.Text]: Array(5).fill('#1C1C1C') // default colors
    };

    getPreference(level: number, type: PreferenceType): string {
        if (this.allPreferences.getValue()?.length > 0) {
            if (type == PreferenceType.Background) return this.allPreferences.getValue().filter(item => item.Level == level)[0]?.BackgroundColor;
            else return this.allPreferences.getValue().filter(item => item.Level == level)[0]?.TextColor;
        }
        // return this.preferences[type][level - 1] || (type === PreferenceType.Background ? '#F3F5F7' : '#1C1C1C');
    }

    addNewPreference(preference: CB_Preference) {
        this.apiService.AddNewCB_Preference(preference).subscribe((preferences) => {
            console.log(preferences);
        });
    }

    AddAllCB_Preferences(preferences: CB_Preference[]) {
        this.apiService.AddNewAllCB_Preferences(preferences).subscribe((preferences) => {
            console.log(preferences);
        });
    }

    updatePreference(preference: CB_Preference) {
        this.apiService.EditCB_Preference(preference).subscribe((preferences) => {
            console.log(preferences);
        });
    }

    updateAllPreferences(preferences: CB_Preference[]) {
        this.apiService.EditAllCB_Preferences(preferences).subscribe((preferences) => {
            console.log(preferences);
        });
    }

    deletePreference(preference: CB_Preference) {
        this.apiService.DeleteCB_Preference(preference).subscribe((preferences) => {
            console.log(preferences);
        });
    }
    DeleteAllCB_Preferences(preferences: CB_Preference[]) {
        this.apiService.DeleteAllCB_Preferences(preferences).subscribe((preferences) => {
            console.log(preferences);
        });
    }

    getPreferencesByUserId(userId: string, tenant: number) {
        this.apiService.GetCB_PreferenceByUserIdAndTenant(userId, tenant).subscribe((data: any) => {
            let PreferencesList: CB_Preference[] = data?.body;
            this.allPreferences.next(PreferencesList);
            // console.log(PreferencesList);
        });
    }

    defualtDataPreferences: CB_Preference[] = [
        { Id: '', Tenant: SessionInfo.LoggedUserTenant, BackgroundColor: '#F3F5F7', TextColor: '#1C1C1C', Level: 1, UserId: SessionInfo.LoggedUserId },
        { Id: '', Tenant: SessionInfo.LoggedUserTenant, BackgroundColor: '#F3F5F7', TextColor: '#1C1C1C', Level: 2, UserId: SessionInfo.LoggedUserId },
        { Id: '', Tenant: SessionInfo.LoggedUserTenant, BackgroundColor: '#F3F5F7', TextColor: '#1C1C1C', Level: 3, UserId: SessionInfo.LoggedUserId },
        { Id: '', Tenant: SessionInfo.LoggedUserTenant, BackgroundColor: '#F3F5F7', TextColor: '#1C1C1C', Level: 4, UserId: SessionInfo.LoggedUserId },
        { Id: '', Tenant: SessionInfo.LoggedUserTenant, BackgroundColor: '#F3F5F7', TextColor: '#1C1C1C', Level: 5, UserId: SessionInfo.LoggedUserId }
    ];

    
}

export class CB_Preference {
    Id: string;
    Tenant: number;
    BackgroundColor: string;
    TextColor: string;
    Level: number;
    UserId: string;
}
export interface HierarchyLevel {
    level: number;
    label: string
};

export enum PreferenceType {
    Background = 'background',
    Text = 'text'
}