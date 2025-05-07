import { Component, inject } from '@angular/core';
import { ImportsModule } from '../../../imports';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../../services/auth.service';
import { jwtDecode } from 'jwt-decode';
import { UserService } from '../../../services/user.service';

@Component({
  selector: 'app-login',
  imports: [ImportsModule, RouterModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  router = inject(Router)
  authService = inject(AuthService)
  userService = inject(UserService)

  username: string = ""
  password: string = ""

  login() {
    let token = ""
    this.authService.login(this.username, this.password).subscribe((data: any) => {
      token = data.token;
      localStorage.setItem('token', token);

      const decoded: any = jwtDecode(token);
      this.userService.updateUserType(decoded.role);
      this.router.navigate(['home']);
    });

  }

  register() {
    this.router.navigate(['register']);
  }

}
