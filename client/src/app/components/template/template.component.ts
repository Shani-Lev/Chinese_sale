import { Component, inject } from '@angular/core';
import { ImportsModule } from '../../imports';
import { Router, RouterModule } from '@angular/router';
import { jwtDecode } from 'jwt-decode';
import { UserService } from '../../services/user.service';
import { StatusService } from '../../services/status.service';
import { Status } from '../../models/status';

interface TokenPayload {
  exp: number;
}

@Component({
  selector: 'app-template',
  imports: [ImportsModule, RouterModule],
  templateUrl: './template.component.html',
  styleUrl: './template.component.css'
})
export class TemplateComponent {
  router = inject(Router)
  userService = inject(UserService)
  statuseService = inject(StatusService)
  items: any;
  user_items: any;
  user: any
  userType: string = "";
  letter? : string; 
  statuse? : Status

  timeLeft: string = '';
  interval: any;

  ngOnInit() {
    
    this.getUser()
    this.getStatus()

    this.userService.userType$.subscribe(userType => {
      this.userType = userType;
      this.updateMenu();
      this.getUser()
    });
  
    this.user_items = [
      {
        label: 'כניסה',
        icon: 'pi pi-sign-in',
        route: '/login'
      },
      {
        label: 'הרשמה',
        icon: 'pi pi-user-plus',
        route: '/register'
      },
    ];
    if (this.userType != ""){
      this.user_items = [...this.user_items, {
        label: 'התנתקות',
        icon: 'pi pi-sign-out',
        command: () => {
          this.logout();
      },
        route: '/home'
      }]
    }
  }

  getUser(){
    let token = localStorage.getItem("token") || "";
    
    if (this.isTokenActive(token)) {
      this.user = this.getDecodedAccessToken(token);
      // console.log(this.user);
      this.letter = this.user.unique_name.charAt(0).toUpperCase()
      this.userType = this.user.role; 
      this.userService.updateUserType(this.userType);
    } else {
      //localStorage.setItem("token", "");
      this.user = null;
    }
  }

  getStatus(){

    this.statuseService.get().subscribe(
      data => {this.statuse = data; console.log(this.statuse); 
        if (this.statuse.text === 'ON')  this.timmer()
      }
    )
  }

  ngOnDestroy() {
    clearInterval(this.interval);
  }

  timmer(){
    this.interval = setInterval(() => {
      if (this.statuse?.lottoryEnd) {
        const lottoryEnd = new Date(this.statuse.lottoryEnd);
        const now = new Date();
        const diff = lottoryEnd.getTime() - now.getTime();
  
        if (diff > 0) {
          const hours = Math.floor((diff % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
          const minutes = Math.floor((diff % (1000 * 60 * 60)) / (1000 * 60));
          const seconds = Math.floor((diff % (1000 * 60)) / 1000);
          this.timeLeft = `${hours}h ${minutes}m ${seconds}s`;
        } 
        else{
          this.timeLeft = 'ההגרלה התבצעה!';
          clearInterval(this.interval);
        }
      } else {
        this.timeLeft = 'Lottory end time is not set.';
        clearInterval(this.interval);
      }
    }, 1000);

  }
  updateMenu() {
    // this.getUser()
    if (this.userType === 'MANAGER') {
      this.items = [
        {
          label: 'דף הבית',
          icon: 'pi pi-home',
          route: '/home'
        },
        {
          label: 'ניהול',
          icon: 'pi pi-cog',
          route: '/manage'
        },
      ];
    } else {
      this.items = [
        {
          label: 'דף הבית',
          icon: 'pi pi-home',
          route: '/home'
        },
        {
          label: 'קניית כרטיסים',
          icon: 'pi pi-gift',
          route: '/gifts'
        },
        {
          label: 'סל הכרטיסים שלי',
          icon: 'pi pi-shopping-cart',
          route: '/cart'
        },
        ,
        {
          label: 'הכרטיסים שרכשתי',
          icon: 'pi pi-tags',
          route: '/tickets'
        },
      ];
    }
  }

  getDecodedAccessToken(token: string): any {
    try {
      return jwtDecode(token);
    } catch (Error) {
      return null;
    }
  }

  isTokenActive(token: string): boolean {
    try {
      const decoded: TokenPayload = jwtDecode(token);
      const currentTime = Math.floor(Date.now() / 1000);
      return decoded.exp > currentTime;
    } catch (error) {
      return false;
    }
  }

  logout() {
    localStorage.removeItem('token');
    this.userService.updateUserType(''); 
  }
}
