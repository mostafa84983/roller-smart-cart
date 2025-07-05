import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CategoryModel } from './models/category.model';
import { PaginatedResult } from '../../shared/paginated-result.model';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {

  private http = inject(HttpClient);
  private baseUrl = `${environment.apiUrl}/Category`;

  getPaginatedCategories(page : number , pageSize : number) : Observable<PaginatedResult<CategoryModel>> 
  {
    return this.http.get<PaginatedResult<CategoryModel>>(
      `${this.baseUrl}/paginated?page=${page}&pageSize=${pageSize}`);  
  }

  getCategoriesWithOffers(page : number , pageSize : number) : Observable<PaginatedResult<CategoryModel>>
  {
    return this.http.get<PaginatedResult<CategoryModel>>(`${this.baseUrl}/offers?page=${page}&pageSize=${pageSize}`);
  }

  createCategory(formData : FormData) : Observable<void>
  {
    return this.http.post<void>(this.baseUrl, formData);
  }

  updateCategory(formData : FormData) : Observable<void>
  {
    return this.http.patch<void>(this.baseUrl,formData);
  }

}
