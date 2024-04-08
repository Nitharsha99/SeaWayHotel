import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AllRoomsComponent } from './all-rooms.component';

describe('AllRoomsComponent', () => {
  let component: AllRoomsComponent;
  let fixture: ComponentFixture<AllRoomsComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AllRoomsComponent]
    });
    fixture = TestBed.createComponent(AllRoomsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
