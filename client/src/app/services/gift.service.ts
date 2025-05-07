import { inject, Injectable } from '@angular/core';
import { Gift } from '../models/gift';
import { Donor } from '../models/donor';
import { Category } from '../models/Category';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class GiftService {
  BASE_URL = 'https://localhost:7069/api/Gift';

  http: HttpClient = inject(HttpClient);
  constructor() { }

  getAll(): Observable<Gift[]>{
    return this.http.get<Gift[]>(this.BASE_URL);
  }
  add(gift : Gift){
    console.log("try add gift");   
    return this.http.post(this.BASE_URL,gift);
  }
  update(gift : Gift){
    console.log("try add gift"); 
    let id = gift.id
    delete gift.id  
    return this.http.put(this.BASE_URL+"/"+id,gift);
  }
  delete(id : number){
    console.log("try delete gift"); 
    return this.http.delete(this.BASE_URL+"/"+id);
  }
  deleteAll(){
    console.log("try delete gift"); 
    return this.http.delete(this.BASE_URL);
  }
  newGift(){
    const gift = new Gift("", 10, [], [],  1, "", "" );
    return gift   
  }
  search(name: string, donor: string, price: number = 0, minSales: number = 0): Observable<Gift[]>{
    console.log(name, donor, minSales, price);
    
    console.log("try search gift"); 
    //https://localhost:7069/search?name=nn&donor=nn&minSales=0&price=10
    return this.http.get<Gift[]>(this.BASE_URL+"/search?name="+name+"&donor="+donor+"&minSales="+minSales+"&price="+price);
  }
  sortPrice(): Observable<Gift[]>{
    return this.http.get<Gift[]>(this.BASE_URL+"/sort");
  }
}
