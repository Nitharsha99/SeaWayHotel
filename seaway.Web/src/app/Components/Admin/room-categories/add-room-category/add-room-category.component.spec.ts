import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddRoomCategoryComponent } from './add-room-category.component';

describe('AddRoomCategoryComponent', () => {
  let component: AddRoomCategoryComponent;
  let fixture: ComponentFixture<AddRoomCategoryComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AddRoomCategoryComponent]
    });
    fixture = TestBed.createComponent(AddRoomCategoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
