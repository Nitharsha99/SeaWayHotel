import { TestBed } from '@angular/core/testing';

import { PackageServiceService } from './package-service.service';

describe('PackageServiceService', () => {
  let service: PackageServiceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(PackageServiceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
