import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AutocomplateTableComponent } from './autocomplate-table.component';

describe('AutocomplateTableComponent', () => {
  let component: AutocomplateTableComponent;
  let fixture: ComponentFixture<AutocomplateTableComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AutocomplateTableComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AutocomplateTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
