import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { SearchCustomsItemAutocomplateComponent } from '../page-top/search-customs-item-autocomplate/search-customs-item-autocomplate.component';
import { NgFor } from '@angular/common';
import { CB_Preference, PreferenceType, PreferencesService } from './PreferencesService';
import { API_MainService } from '../../../core/API_MainService';
import { SessionInfo } from '../../../core/Infrastructure/Utilities/SessionInfo';

@Component({
  selector: 'app-preference-menu',
  standalone: true,
  imports: [FormsModule, MatAutocompleteModule, SearchCustomsItemAutocomplateComponent, NgFor],
  templateUrl: './preference-menu.html',
  styleUrl: './preference-menu.css',
})
export class PreferenceMenuComponent implements OnInit {
  preferenceTypeBackground: PreferenceType = PreferenceType.Background;
  preferenceTypeText: PreferenceType = PreferenceType.Text;

  levels = [1, 2, 3, 4, 5, 6, 7];
  selectedType: PreferenceType = PreferenceType.Background;
  preferences = {
    [PreferenceType.Background]: Array(7).fill('#ffffff'),
    [PreferenceType.Text]: Array(7).fill('#000000')
  };

  constructor(private preferencesService: PreferencesService, private apiService: API_MainService) {
    this.levels.forEach(level => {
      this.preferences[PreferenceType.Background][level - 1] = this.preferencesService.getPreference(level, PreferenceType.Background);
      this.preferences[PreferenceType.Text][level - 1] = this.preferencesService.getPreference(level, PreferenceType.Text);
    });


  }

  tenant = 6;
  ngOnInit(): void {
    // this.addNewPreference();
    // this.updatePreference();
    this.deletePreference();

    // this.getPreferencesByUserId('1-9', 0);
  }

  updatePreferences(level: number) {
    this.preferencesService.setPreference(level, this.preferences[this.selectedType][level - 1], this.selectedType);
  }

  // 1. build function are get by user id and tenant:
  getPreferencesByUserId(userId: string, tenant: number) {

    // init preferences by user id
    this.apiService.GetCB_PreferenceByUserIdAndTenant(userId, tenant).subscribe((data: any) => {
      let PreferencesList: CB_Preference[] = data?.body;
      console.log(PreferencesList);

    });
  }
  data: CB_Preference = {
    Id: '',
    Tenant: SessionInfo.LoggedUserTenant,
    BackgroundColor: '#F3F5F7',
    TextColor: '#1C1C1C',
    UserId: SessionInfo.LoggedUserId,
    Level: 1
  };
  // 2. build function are add new preference:
  addNewPreference() {


    // data.UserId = SessionInfo.LoggedUserEmail;
    // this.data.Tenant = 6;
    // init preferences by user id
    this.apiService.AddNewCB_Preference(this.data).subscribe((preferences) => {
      console.log(preferences);

    });
  }

  updatePreference() {
    this.data.Id = '1-1';
    this.data.BackgroundColor = '#F3F5F3';
    // init preferences by user id
    this.apiService.EditCB_Preference(this.data).subscribe((preferences) => {
      console.log(preferences);

    });
  }
  deletePreference() {
    this.data.Id = '1-3';
    // init preferences by user id
    this.apiService.DeleteCB_Preference(this.data).subscribe((preferences) => {
      console.log(preferences);
    });
  }

}
