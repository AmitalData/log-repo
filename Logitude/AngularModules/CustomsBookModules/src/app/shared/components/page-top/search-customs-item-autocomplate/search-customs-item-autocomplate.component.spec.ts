import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SearchCustomsItemAutocomplateComponent } from './search-customs-item-autocomplate.component';

describe('SearchCustomsItemAutocomplateComponent', () => {
  let component: SearchCustomsItemAutocomplateComponent;
  let fixture: ComponentFixture<SearchCustomsItemAutocomplateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SearchCustomsItemAutocomplateComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(SearchCustomsItemAutocomplateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
