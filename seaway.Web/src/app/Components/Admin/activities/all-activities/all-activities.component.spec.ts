import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AllActivitiesComponent } from './all-activities.component';

describe('AllActivitiesComponent', () => {
  let component: AllActivitiesComponent;
  let fixture: ComponentFixture<AllActivitiesComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AllActivitiesComponent]
    });
    fixture = TestBed.createComponent(AllActivitiesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
