import { Component, OnInit } from '@angular/core';
import { PreferencesService } from '../main-display/main-display.component';
import { FormsModule } from '@angular/forms';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { SearchCustomsItemAutocomplateComponent } from '../page-top/search-customs-item-autocomplate/search-customs-item-autocomplate.component';
import { NgFor } from '@angular/common';

@Component({
  selector: 'app-preference-menu',
  standalone: true,
  imports: [FormsModule, MatAutocompleteModule, SearchCustomsItemAutocomplateComponent, NgFor],
  templateUrl: './preference-menu.html',
  styleUrl: './preference-menu.css',
})
export class PreferenceMenuComponent {

  levels = [1, 2, 3, 4, 5, 6, 7];
  preferences = {};

  constructor(private preferencesService: PreferencesService) {
    this.levels.forEach(level => {
      this.preferences[level] = this.preferencesService.getPreference(level);
    });
  }



  updatePreferences() {
    this.levels.forEach(level => {
      console.log( this.preferences[level]);
      this.preferencesService.setPreference(level, this.preferences[level]);
      
    });
  }
}