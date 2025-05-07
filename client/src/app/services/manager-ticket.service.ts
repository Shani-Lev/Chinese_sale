import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ManagerTicket } from '../models/manager-ticket';
import { User } from '../models/user';

@Injectable({
  providedIn: 'root'
})
export class ManagerTicketService {

  BASE_URL = 'https://localhost:7069/api/TicketManager';

  http: HttpClient = inject(HttpClient);
  constructor() { }

  getAll(): Observable<ManagerTicket[]>{
    return this.http.get<ManagerTicket[]>(this.BASE_URL);
  }
  getUser(id : number): Observable<User[]>{
    return this.http.get<User[]>(this.BASE_URL+"/users/"+id)
  }
  orderBySales(): Observable<ManagerTicket[]>{
    return this.http.get<ManagerTicket[]>(this.BASE_URL+"/orderBySales");
  }
  orderByPrice(): Observable<ManagerTicket[]>{
    return this.http.get<ManagerTicket[]>(this.BASE_URL+"/orderByPrice");
  }
}
