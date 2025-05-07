import { Component, inject } from '@angular/core';
import { ImportsModule } from '../../../imports';
import { Router, RouterModule } from '@angular/router';
import { User } from '../../../models/user';
import { PasswordModule } from 'primeng/password';
import { AuthService } from '../../../services/auth.service';

@Component({
  selector: 'app-register',
  imports: [ImportsModule, RouterModule, PasswordModule ],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  router = inject(Router)
  authService = inject(AuthService)

  user : User = new User("", "", "", "")
  checkPassword : string = ""
  register(){
      this.authService.register(this.user).subscribe()
      this.router.navigate(['login'])  
  }
  login(){
    this.router.navigate(['login']);
  }
}
