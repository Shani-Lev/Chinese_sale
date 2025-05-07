import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Category } from '../models/Category';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {

  BASE_URL = 'https://localhost:7069/api/Category';

  http: HttpClient = inject(HttpClient);
  constructor() { }

  getAll(): Observable<Category[]>{
    return this.http.get<Category[]>(this.BASE_URL);
  }

  add(category : Category){
    console.log("try add category");
    
    return this.http.post(this.BASE_URL,category);
  }
  delete(category : Category){
    console.log("try delete category");
    
    return this.http.delete(this.BASE_URL+"/"+category.id);
  }
  deleteAll(){
    
    return this.http.delete(this.BASE_URL);
  }
}
