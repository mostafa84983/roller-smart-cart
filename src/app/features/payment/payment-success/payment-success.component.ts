import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CartService } from '../../../shared/cart.service';

@Component({
  selector: 'app-payment-success',
  standalone: true,
  imports: [],
  templateUrl: './payment-success.component.html',
  styleUrl: './payment-success.component.scss'
})
export class PaymentSuccessComponent implements OnInit{

  sessionId? : string;
  countDown : number = 5;
  private countdownInterval: any;
  
  constructor(private activatedRoute : ActivatedRoute, private cartSerivce : CartService, private route : Router) {}

  ngOnInit(): void {
    
    this.sessionId = this.activatedRoute.snapshot.queryParamMap.get('sessionId') ?? undefined;
    console.log('session id : ' ,this.sessionId);
    
    if(this.sessionId)
    {
      this.cartSerivce.setOrderId(-1);
      
    }

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
