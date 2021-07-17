import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RemovePublicationDialogComponent } from './remove-publication-dialog.component';

describe('RemovePublicationDialogComponent', () => {
  let component: RemovePublicationDialogComponent;
  let fixture: ComponentFixture<RemovePublicationDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RemovePublicationDialogComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(RemovePublicationDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
