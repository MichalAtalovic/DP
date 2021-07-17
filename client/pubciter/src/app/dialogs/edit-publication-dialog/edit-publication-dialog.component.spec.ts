import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditPublicationDialogComponent } from './edit-publication-dialog.component';

describe('EditPublicationDialogComponent', () => {
  let component: EditPublicationDialogComponent;
  let fixture: ComponentFixture<EditPublicationDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EditPublicationDialogComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(EditPublicationDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
