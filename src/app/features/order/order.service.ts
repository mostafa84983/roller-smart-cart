import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { OrderDto } from './order.model';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class OrderService {

  constructor(private http : HttpClient) { }

  GetOrdersofUser ()
  {
   return this.http.get<OrderDto[]>(`${environment.apiUrl}/Order/OrdersOfUser`) ;
  }

   GetOrdersByUser (userId : number)
  {
   return this.http.get<OrderDto[]>(`${environment.apiUrl}/Order/OrdersByUser?userid=${{userId}}`) ;
  }

}
