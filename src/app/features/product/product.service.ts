import {inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { PaginatedResult } from '../../shared/paginated-result.model';
import { productModel } from './models/product.model';
import { AddOfferModel } from './models/add-offer.model';
import { RemoveOfferModel } from './models/remove-offer.model';

@Injectable({
  providedIn: 'root'
})

export class ProductService {

  private http = inject(HttpClient);
  private baseUrl = 'https://localhost:7075/api/Product';


  getPaginatedProductsInCategory(categoryId : number , page : number , pageSize : number) : Observable<PaginatedResult<productModel>>
  {
    return this.http.get<PaginatedResult<productModel>>(
      `${this.baseUrl}/category/${categoryId}?page=${page}&pageSize=${pageSize}`); 
  }


getPaginatedProductsWithOfferInCategory(categoryId : number , page : number , pageSize : number) : Observable<PaginatedResult<productModel>>
{
    return this.http.get<PaginatedResult<productModel>>(
      `${this.baseUrl}/category/${categoryId}/offers?page=${page}&pageSize=${pageSize}`); 
}

getProductByCode(productCode : number) : Observable<productModel>
{
  return this.http.get<productModel>( `${this.baseUrl}/code/${productCode}`);
}

getProductById(productId : number) : Observable<productModel>
{
  return this.http.get<productModel>( `${this.baseUrl}/${productId}`);
}

addOfferToProduct(dto : AddOfferModel) : Observable<void>
{
 return this.http.put<void>(`${this.baseUrl}/add-offer`, dto);
}
removeOfferFromProduct(dto : RemoveOfferModel) : Observable<void>
{
 return this.http.put<void>(`${this.baseUrl}/remove-offer`, dto);
}

softDeleteProduct(productId : number) : Observable<void>
{
  return this.http.delete<void>( `${this.baseUrl}/${productId}`);
}

restoreProduct(productId : number) : Observable<void>
{
  return this.http.put<void>(`${this.baseUrl}/${productId}/restore` , {} );
}

createProduct(formData : FormData) : Observable<void>
{
 return this.http.post<void>(this.baseUrl, formData);
}

updateProduct(formData : FormData) : Observable<void>
{
 return this.http.patch<void>(this.baseUrl, formData);
}

}