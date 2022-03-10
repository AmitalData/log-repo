import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BulkFeedPendingComponent } from './BulkFeedPendingComponent';

describe('BulkFeedPendingComponent', () => {
  let component: BulkFeedPendingComponent;
  let fixture: ComponentFixture<BulkFeedPendingComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ BulkFeedPendingComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(BulkFeedPendingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
