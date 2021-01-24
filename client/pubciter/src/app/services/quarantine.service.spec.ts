import { TestBed } from '@angular/core/testing';

import { QuarantineService } from './quarantine.service';

describe('QuarantineService', () => {
  let service: QuarantineService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(QuarantineService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
