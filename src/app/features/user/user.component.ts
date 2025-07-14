import { Component, OnInit } from '@angular/core';
import { UserService } from './user.service';
import { UserModel } from './user.model';

@Component({
  selector: 'app-user',
  standalone: true,
  imports: [],
  templateUrl: './user.component.html',
  styleUrl: './user.component.scss'
})
export class UserComponent implements OnInit{

  users : UserModel[]= [] ;
  errorMessage = '' ;
  

  constructor(private userService : UserService){}

  ngOnInit(): void {
   this.userService.GetAllUsers().subscribe({
      next : data => {
      this.users = data ;
      },
      error : err => {
      this.errorMessage = err.error;
      }
    });
}

}
