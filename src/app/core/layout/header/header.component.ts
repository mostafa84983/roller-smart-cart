import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../auth/auth.service';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [],
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss'
})
export class HeaderComponent  {

  isLoggedIn : boolean = false ;
  constructor(private router : Router , private authservice : AuthService) {}

  ngOnInit(){
    this.authservice.isAuthenticated$.subscribe(value => {
      this.isLoggedIn = value ;
    }) ;
  }
  
  goToCategories(){
    this.router.navigate(['/categories']);
  }

  goToOffers(){
    this.router.navigate(['/offers']);
  }
  Logout(){
  this.authservice.logout();
  }
}
