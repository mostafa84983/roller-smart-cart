// import { HttpInterceptorFn } from '@angular/common/http';

// export const authInterceptor: HttpInterceptorFn = (req, next) => {

//   const localToken = localStorage.getItem("authToken");

//   if (req.url.includes('/User/login') || (!localToken && req.url.includes('/User/details'))) {
//     return next(req);
//   }

//   // Add Authorization header if token exists
//   if (localToken) {
//     const newRequest = req.clone({
//       setHeaders: {
//         Authorization: `Bearer ${localToken}`,
//       },
//     });
//     return next(newRequest);
//   }

//   return next(req);
// };

import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { catchError } from 'rxjs';

export const authInterceptor: HttpInterceptorFn = (req, next) => {

  const localToken = localStorage.getItem("auth-token");
  const router = inject(Router);

  const clonedRequest = localToken
    ? req.clone({
        setHeaders: {
          Authorization: `Bearer ${localToken}`,
        },
      })
    : req;

  return next(clonedRequest).pipe(
    catchError((error: HttpErrorResponse) => {
      if (error.status === 401 ) {
        //router.navigate(['/login']);
      }
      throw error; 
    })
  );
};