import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CategoryModel } from './models/category.model';
import { PaginatedResult } from '../../shared/paginated-result.model';
import { UpdateCategoryModel } from './models/update-category.model';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {

  private http = inject(HttpClient);
  private baseUrl = 'https://localhost:7075/api/Category';

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

  updateCategory(dto : UpdateCategoryModel) : Observable<void>
  {
    return this.http.patch<void>(this.baseUrl,dto);
  }

}
