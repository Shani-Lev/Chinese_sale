import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Donor } from '../models/donor';

@Injectable({
  providedIn: 'root'
})
export class DonorService {
  BASE_URL = 'https://localhost:7069/api/Donor';

  http: HttpClient = inject(HttpClient);
  constructor() { }

  getAll(): Observable<Donor[]>{
    return this.http.get<Donor[]>(this.BASE_URL);
  }
  add(donor : Donor){
    console.log("try add donor");
    
    return this.http.post(this.BASE_URL,donor);
  }
  update(donor: Donor){
    return this.http.put(this.BASE_URL+"/"+donor.id, donor);
  }
  delete(donor: Donor){
    return this.http.delete(this.BASE_URL+"/"+donor.id);
  }
  deleteAll(){
    return this.http.delete(this.BASE_URL);
  }
  search(text: string): Observable<Donor[]>{
    return this.http.get<Donor[]>(this.BASE_URL+"/search/"+text);
  }
}
