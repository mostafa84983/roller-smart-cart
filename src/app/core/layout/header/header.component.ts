import { Component } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../auth/auth.service';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [RouterModule],
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss'
})
export class HeaderComponent  {

  isLoggedIn : boolean = false ;
  constructor(private router : Router , private authservice : AuthService) {}

  ngOnInit(){
    this.authservice.isLoggedIn();
    
    this.authservice.isAuthenticated$.subscribe(value => {
      this.isLoggedIn = value ;
    }) ;
  }
  
  Logout(){
  this.authservice.logout();
  }
}
