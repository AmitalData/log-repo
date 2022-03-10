import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FeedbackToStorageTabComponent } from './FeedbackToStorageTabComponent.component';

describe('FeedbackToStorageTabComponent', () => {
  let component: FeedbackToStorageTabComponent;
  let fixture: ComponentFixture<FeedbackToStorageTabComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ FeedbackToStorageTabComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(FeedbackToStorageTabComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
