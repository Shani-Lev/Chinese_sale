import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Status } from '../models/status';

@Injectable({
  providedIn: 'root'
})
export class StatusService {
  BASE_URL = 'https://localhost:7069/api/Manage/status';

  http: HttpClient = inject(HttpClient);
  constructor() { }

  get() : Observable<Status>{
    console.log("aaa"+(1+1)%3);
    
    console.log("get: service");  
   return this.http.get<Status>(this.BASE_URL);
  }
  set(nextTime : Date){
    return this.http.post<Status>(this.BASE_URL, nextTime);
  }
  put(nextTime : Date){
    console.log(nextTime);
    
    return this.http.put<Status>(this.BASE_URL, nextTime);
  }
  setOnStorage(){
    this.get().subscribe(data=>localStorage.setItem("status", data.text || "")) 
  }
}
