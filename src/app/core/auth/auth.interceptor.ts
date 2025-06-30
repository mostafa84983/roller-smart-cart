import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { catchError } from 'rxjs';

export const authInterceptor: HttpInterceptorFn = (req, next) => {

  const token = localStorage.getItem("authToken");
  const router = inject(Router);

  let modifiedRequest = req ;
  if(token)
  {
    modifiedRequest = req.clone({
      setHeaders : {
       Authorization: `Bearer ${token}`
      }
    })
  }

  return next(modifiedRequest).pipe(
    catchError((error: HttpErrorResponse) => {
      if (error.status === 401 ) {
        router.navigate(['/login']);
      }
      throw error; 
    })
 );
};
