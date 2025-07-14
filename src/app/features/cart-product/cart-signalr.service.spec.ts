import { TestBed } from '@angular/core/testing';

import { CartSignalrService } from './cart-signalr.service';

describe('CartSignalrService', () => {
  let service: CartSignalrService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CartSignalrService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
