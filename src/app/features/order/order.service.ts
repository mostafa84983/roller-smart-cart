import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { OrderDto } from './order.model';

@Injectable({
  providedIn: 'root'
})
export class OrderService {

  constructor(private http : HttpClient) { }

  GetOrdersofUser ()
  {
   return this.http.get<OrderDto[]>('https://localhost:7075/api/Order/OrdersOfUser') ;
  }

   GetOrdersByUser (userId : number)
  {
   return this.http.get<OrderDto[]>(`https://localhost:7075/api/Order/OrdersByUser?userid=${{userId}}`) ;
  }

}
