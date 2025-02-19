import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { SearchCustomsItemAutocomplateComponent } from '../page-top/search-customs-item-autocomplate/search-customs-item-autocomplate.component';
import { NgFor, NgIf } from '@angular/common';
import { CB_Preference, HierarchyLevel, PreferenceType, PreferencesService } from './PreferencesService';
import { API_MainService } from '../../../core/API_MainService';
import { SessionInfo } from '../../../core/Infrastructure/Utilities/SessionInfo';

@Component({
  selector: 'app-preference-menu',
  standalone: true,
  imports: [FormsModule, MatAutocompleteModule, SearchCustomsItemAutocomplateComponent, NgFor, NgIf],
  templateUrl: './preference-menu.html',
  styleUrl: './preference-menu.css',
})
export class PreferenceMenuComponent implements OnInit {
  hierarchyLevels: HierarchyLevel[] = [
    { level: 1, label: 'חלק' },
    { level: 2, label: 'פרק' },
    { level: 3, label: 'פרט' },
    { level: 4, label: 'סעיף' },
    { level: 5, label: 'פרט מכס' }
  ];

  preferences: CB_Preference[] = [];

  constructor(private preferencesService: PreferencesService) { }

  ngOnInit(): void {
    this.loadPreferences();
  }

  loadPreferences(): void {
    this.preferencesService.allPreferences.subscribe((data) => {
      this.preferences = this.hierarchyLevels.map(({ level }) =>
        data.find((p) => p.Level === level) ?? this.createDefaultPreference(level)
      );
    });
  }


  createDefaultPreference(level: number): CB_Preference {
    return {
      Id: '',
      Tenant: SessionInfo.LoggedUserTenant,
      BackgroundColor: '#F3F5F7',
      TextColor: '#1C1C1C',
      Level: level,
      UserId: SessionInfo.LoggedUserId
    };
  }

  savePreferences(): void {
    this.preferencesService.updateAllPreferences(this.preferencesService.allPreferences?.getValue());
  }

  clearPreferences(): void {
    if (this.preferencesService.allPreferences?.getValue()?.length > 0) {
      this.preferences.forEach((pref) => {
        pref.BackgroundColor = '#F3F5F7';
        pref.TextColor = '#1C1C1C';
      });
      this.preferencesService.updateAllPreferences(this.preferences);
    }
  }
}

