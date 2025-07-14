import { Injectable , inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { PaymentModel } from './payment.model';
import { environment } from '../../../environments/environment';


@Injectable({
  providedIn: 'root'
})
export class PaymentService {

    private http = inject(HttpClient);
    private baseUrl = `${environment.apiUrl}/Payment`;

    createCheckoutSession(orderId : number) : Observable<PaymentModel>
    {
    return this.http.post<PaymentModel>(`${this.baseUrl}/checkout`, { orderId });
    }
}
