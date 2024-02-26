import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SubHomePageComponent } from './sub-home-page.component';

describe('SubHomePageComponent', () => {
  let component: SubHomePageComponent;
  let fixture: ComponentFixture<SubHomePageComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SubHomePageComponent]
    });
    fixture = TestBed.createComponent(SubHomePageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
