import { Component } from '@angular/core';
import { FormBuilder, FormGroup, FormGroupDirective, ReactiveFormsModule, Validators } from '@angular/forms';
import {MatError, MatFormFieldModule} from '@angular/material/form-field';
import {MatIconModule} from '@angular/material/icon';
import {MatInputModule} from '@angular/material/input';
import { AuthService } from '../../auth.service';
import { Router, RouterModule } from '@angular/router';


@Component({
  selector: 'app-login',
  standalone: true,
  imports: [MatFormFieldModule , MatInputModule , MatIconModule, ReactiveFormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent  {

  errorMessage : string = "" ;
  isloggedIn = true;
  form : FormGroup = new FormGroup({}) ;

  constructor(private formBuilder : FormBuilder , private authService : AuthService , private router : Router)
  {

  }

  ngOnInit()
  {
    this.form = this.formBuilder.group({
      email : ['',[Validators.required , Validators.email]],
      password : ['' , [Validators.required , Validators.minLength(8)]]
    })
  }

  onSubmit()
  {
    this.authService.login(this.form.value).subscribe({
      next : response => 
      {
        this.isloggedIn= true;
        this.authService.startLogin();
        this.authService.storeToken(response.token , response.expiration , response.role)
        this.router.navigate(['/home']) ;
      } ,
      error : err => 
      {
        this.isloggedIn = false;
       this.errorMessage= err.error ;
      }
    })


  }


}
