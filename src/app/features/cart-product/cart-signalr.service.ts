import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';

@Injectable({
  providedIn: 'root'
})
export class CartSignalrService {
  private hubConnection!: signalR.HubConnection;

  startConnection(cartId: string) {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('https://localhost:7075/hubs/cart') 
      .withAutomaticReconnect()
      .build();

    this.hubConnection.start()
      .then(() => {
        console.log('SignalR Connected');
        this.joinCartGroup(cartId);
      })
      .catch(err => console.error('SignalR connection error:', err));
  }

  joinCartGroup(cartId: string) {
    this.hubConnection.invoke('JoinCartGroup', cartId)
      .catch(err => console.error('Failed to join cart group:', err));
  }

  onProductAdded(callback: (data: any) => void) {
  this.hubConnection.on('ProductAdded', callback);
   }

 onProductRemoved(callback: (data: any) => void) {
    this.hubConnection.on('ProductRemoved', callback);
  }

 onOrderCompleted(callback: (data: any) => void){
  this.hubConnection.on('CompleteProduct', callback);
}


}
