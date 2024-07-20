import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AllOffersComponent } from './all-offers.component';

describe('AllOffersComponent', () => {
  let component: AllOffersComponent;
  let fixture: ComponentFixture<AllOffersComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AllOffersComponent]
    });
    fixture = TestBed.createComponent(AllOffersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
