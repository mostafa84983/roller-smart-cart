import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CartService {

  constructor() { }

  private OrderIdSubject = new BehaviorSubject<number>(-1);
  orderid$ = this.OrderIdSubject.asObservable() ;
  
  setOrderId(orderId : number)
  {
    this.OrderIdSubject.next(orderId) ;
  }
  getOrderId()
  {
    return this.OrderIdSubject.value ;
  }

}
