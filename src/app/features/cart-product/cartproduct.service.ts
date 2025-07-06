import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { productCartDto } from './productCartDto.model';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class CartproductService {

  constructor(private http : HttpClient) { }

  getProductsOfCart (page : number , pageSize : number , orderId: number)
  {
    return this.http.get<productCartDto[]>(`${environment.apiUrl}/Product/order/${orderId}/products?page=${page}&pageSize=${pageSize}`) ;
  }

  OpenOCR(cartId: string)
  {
   return this.http.post<any>(`${environment.apiUrl}/Product/REDO?cartId=${cartId}`, {});
  }

   REDO(cartId: string)
  {
   return this.http.post<any>(`${environment.apiUrl}/Product/REDO?cartId=${cartId}`, {});
  }

}
