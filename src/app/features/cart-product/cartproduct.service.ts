import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { productCartDto } from './productCartDto.model';

@Injectable({
  providedIn: 'root'
})
export class CartproductService {

  constructor(private http : HttpClient) { }

  getProductsOfCart (page : number , pageSize : number , orderId: number)
  {
    return this.http.get<productCartDto[]>(`https://localhost:7075/api/Product/order/${orderId}/products?page=${page}&pageSize=${pageSize}`) ;
  }

}
