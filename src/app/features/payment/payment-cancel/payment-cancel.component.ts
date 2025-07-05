import { Component, OnInit } from '@angular/core';
import {  Router } from '@angular/router';

@Component({
  selector: 'app-payment-cancel',
  standalone: true,
  imports: [],
  templateUrl: './payment-cancel.component.html',
  styleUrl: './payment-cancel.component.scss'
})
export class PaymentCancelComponent implements OnInit{

  countDown : number = 5;
  private countdownInterval: any;

  constructor(private route : Router) {}
  ngOnInit(): void {
  this.countDownButton();
  }

  goToHome()
  {
     this.route.navigate(['/home']);
  }

  countDownButton()
  {
        this.countdownInterval = setInterval(() => {
        this.countDown--;

        if (this.countDown === 0) 
        {
          clearInterval(this.countdownInterval);
          this.goToHome();
        }
      }, 1000);
  }
}
