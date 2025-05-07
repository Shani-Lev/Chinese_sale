import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { UserTicket } from '../models/user-ticket';

@Injectable({
  providedIn: 'root'
})
export class UserTicketService {

  BASE_URL = 'https://localhost:7069/api/TicketUser';

  http: HttpClient = inject(HttpClient);
  constructor() { }

  getInBasket(): Observable<UserTicket[]>{
    return this.http.get<UserTicket[]>(this.BASE_URL+"/basket");
  }
  getNotInBasket(): Observable<UserTicket[]>{
    return this.http.get<UserTicket[]>(this.BASE_URL)
  }
  Buy(giftId : number){
    return this.http.put(this.BASE_URL+"/"+giftId, {})
  }
  BuyAll(){
    return this.http.put(this.BASE_URL, {})
  }
  remove(giftId : number, amount: number){
    return this.http.delete(this.BASE_URL+"?giftId="+giftId+"&amount="+amount)
    //https://localhost:7069/api/TicketUser?giftId=12&amount=1
  }
  add(giftId : number, amount : number,toBasket: boolean){
    return this.http.post(this.BASE_URL+"?giftId="+giftId+"&Amount="+amount+"&toBasket="+toBasket, {})
    //https://localhost:7069/api/TicketUser?giftId=14&Amount=5
  }
}
