import { Injectable } from '@angular/core';
import { jwtDecode } from 'jwt-decode';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private userTypeSubject = new BehaviorSubject<string>(this.getUserTypeFromLocalStorage());
  userType$ = this.userTypeSubject.asObservable();

  constructor() {
    window.addEventListener('storage', () => {
      this.userTypeSubject.next(this.getUserTypeFromLocalStorage());
    });
  }

  private getUserTypeFromLocalStorage(): string {
    const token = localStorage.getItem('token') || "";
    if (token != "") {
      const decoded: any = jwtDecode(token);
      return decoded.role;
    }
    return ""
  }

  updateUserType(userType: string) {
    localStorage.setItem('role', userType);
    this.userTypeSubject.next(userType);
  }
}