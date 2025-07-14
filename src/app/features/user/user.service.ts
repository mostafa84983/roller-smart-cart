import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import {UserModel } from './user.model';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private http : HttpClient) { }

  GetAllUsers()
  {
    return this.http.get<UserModel[]>(`${environment.apiUrl}/User?page=1&pageSize=4`);
  }


}
