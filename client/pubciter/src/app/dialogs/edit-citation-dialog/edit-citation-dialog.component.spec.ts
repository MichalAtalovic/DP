import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditCitationDialogComponent } from './edit-citation-dialog.component';

describe('EditCitationDialogComponent', () => {
  let component: EditCitationDialogComponent;
  let fixture: ComponentFixture<EditCitationDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EditCitationDialogComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(EditCitationDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
