import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { User } from '../models/user';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  BASE_URL = 'https://localhost:7069/api/Auth';

  http: HttpClient = inject(HttpClient);
  constructor() { }

  login (username: string, password: string) : Observable<string>{
    return this.http.post<string>(this.BASE_URL+'/login',{'username': username, 'password' : password});
  }

  register(user: User){
    return this.http.post(this.BASE_URL+'/register',user);
  }
}
