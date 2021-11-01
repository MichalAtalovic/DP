import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PublicationCategoryDialogComponent } from './publication-category-dialog.component';

describe('PublicationCategoryDialogComponent', () => {
  let component: PublicationCategoryDialogComponent;
  let fixture: ComponentFixture<PublicationCategoryDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PublicationCategoryDialogComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PublicationCategoryDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
