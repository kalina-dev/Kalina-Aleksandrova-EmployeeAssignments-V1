import { TestBed } from '@angular/core/testing';

import { EmployeeProjectsService } from './employee-projects.service';

describe('EmployeeProjectsService', () => {
  let service: EmployeeProjectsService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(EmployeeProjectsService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
