import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject } from 'rxjs';
import { Router } from '@angular/router';
import { RegisterDto } from './register.model';
import { environment } from '../../../environments/environment';



@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private token = 'auth-token' ;
  private expriration = 'expiration-date' ;
  private role = 'role';

  constructor(private http : HttpClient, private router : Router) 
  { }

  login(credintials:{email:string , password:string}) 
  {
    return this.http.post<any>(`${environment.apiUrl}/User/Login` , credintials) ;
  }

 register(register:RegisterDto) 
 {
  return this.http.post<any>(`${environment.apiUrl}/User/Register`, register) ;
 }
  

  storeToken (token:string , expiration:string , role:string)
  {
    localStorage.setItem(this.token , token) ;
    localStorage.setItem(this.expriration , expiration);
    localStorage.setItem(this.role,role);
  }

  getRole () : string | null
  {
    return localStorage.getItem(this.role) ;
  }

  getToken () : string | null
  {
    return localStorage.getItem(this.token) ;
  }
  getExpiration()
  {
    return localStorage.getItem(this.expriration);
  }

  private isAuthenticated = new BehaviorSubject<boolean>(false);
  isAuthenticated$ = this.isAuthenticated.asObservable() ;


  startLogin()
  {
    this.isAuthenticated.next(true) ;
  }

  isLoggedIn()
  {
    const token = this.getToken();
    const expiration = this.getExpiration();

    if(!token || !expiration)
    {
      this.isAuthenticated.next(false);
      return false ;
    }

    const now = new Date() ;
    const isAuth = now < new Date(expiration);
    this.isAuthenticated.next(isAuth) ;

    return isAuth;
  }

  logout() 
  {
     this.isAuthenticated.next(false) ;
     if(this.getToken() || this.getRole() || this.getExpiration())
     {
        localStorage.removeItem(this.token);
        localStorage.removeItem(this.expriration);
        localStorage.removeItem(this.role);
      }

    if(localStorage.getItem('orderId'))
      localStorage.removeItem('orderId') ;
     
      this.router.navigate(['/login']) ;
  }

}
