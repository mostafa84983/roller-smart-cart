import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from './auth.service';

export const authGuard: CanActivateFn = (route, state) => {
  
  const service = inject(AuthService);
  const router = inject(Router) ;

  if(!service.isLoggedIn())
  {
    service.logout();
    router.navigate(['/login']);
    return false;
  }

  return true ;
    


};
