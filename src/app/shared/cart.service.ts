import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CartService {
 private OrderIdSubject: BehaviorSubject<number>;
 orderid$;

 totalPriceSubject = new BehaviorSubject<number>(0);
 totalPrice$ = this.totalPriceSubject.asObservable()

  constructor() {
    
    const saved = localStorage.getItem('orderId');
    this.OrderIdSubject = new BehaviorSubject<number>(saved ? +saved : -1);
    this.orderid$ = this.OrderIdSubject.asObservable();
  }

  
  setOrderId(orderId: number) {
    localStorage.setItem('orderId', orderId.toString());
    this.OrderIdSubject.next(orderId);
  }

  getOrderId(): number {
    return this.OrderIdSubject.value;
  }

  setTotalPrice(price: number) {
    this.totalPriceSubject.next(price);
  }

  getTotalPrice(): number {
    return this.totalPriceSubject.value;
  }

  
}
