import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MyLibraryActionPanelComponent } from './my-library-action-panel.component';

describe('MyLibraryActionPanelComponent', () => {
  let component: MyLibraryActionPanelComponent;
  let fixture: ComponentFixture<MyLibraryActionPanelComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MyLibraryActionPanelComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(MyLibraryActionPanelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
