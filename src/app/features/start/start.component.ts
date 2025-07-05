import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../core/auth/auth.service';
import { UserComponent } from '../user/user.component';
import { HomeComponent } from '../home/home.component';

@Component({
  selector: 'app-start',
  standalone: true,
  imports: [UserComponent, HomeComponent],
  templateUrl: './start.component.html',
  styleUrl: './start.component.scss'
})
export class StartComponent implements OnInit {
 
  Role: string|null = '';

  constructor(private authService : AuthService)
  {}
 
  ngOnInit(): void {
    this.authService.isLoggedIn()
    this.authService.isAuthenticated$.subscribe(() => {
    this.Role = this.authService.getRole();
    console.log("admin") ;
  });
  }




}
