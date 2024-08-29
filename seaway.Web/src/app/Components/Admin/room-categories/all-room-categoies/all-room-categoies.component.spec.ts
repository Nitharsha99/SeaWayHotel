import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AllRoomCategoiesComponent } from './all-room-categoies.component';

describe('AllRoomCategoiesComponent', () => {
  let component: AllRoomCategoiesComponent;
  let fixture: ComponentFixture<AllRoomCategoiesComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AllRoomCategoiesComponent]
    });
    fixture = TestBed.createComponent(AllRoomCategoiesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
