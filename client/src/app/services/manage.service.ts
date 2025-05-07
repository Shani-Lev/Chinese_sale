import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ManagerTicket } from '../models/manager-ticket';

@Injectable({
  providedIn: 'root'
})
export class ManageService {

  BASE_URL = 'https://localhost:7069/api/Manage';

  http: HttpClient = inject(HttpClient);
  constructor() { }

  getRevenue(): Observable<ManagerTicket[]>{
    return this.http.get<ManagerTicket[]>(this.BASE_URL+"/revenue")
  }
}
