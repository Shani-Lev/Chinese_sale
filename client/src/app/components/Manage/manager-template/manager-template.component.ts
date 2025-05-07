import { Component, inject } from '@angular/core';
import { Router, RouterModule, RouterOutlet } from '@angular/router';
import { ImportsModule } from '../../../imports';
import { jwtDecode } from 'jwt-decode';

@Component({
  selector: 'app-manager-template',
  imports: [RouterOutlet, ImportsModule, RouterModule],
  templateUrl: './manager-template.component.html',
  styleUrl: './manager-template.component.css'
})
export class ManagerTemplateComponent {
  router = inject(Router)
  items: any[] | undefined;

  ngOnInit() {
      this.items = [
          {
              label: 'ניהול',
              items: [
                  {
                      label: 'מכירה סינית',
                      icon: 'pi pi-chart-line',
                      route: 'status'
                  },
                  {
                      label: 'מכירות',
                      icon: 'pi pi-tags',
                      route: 'tickets'
                  }
              ]
          },
          {
              label: 'נתונים',
              items: [
                  {
                      label: 'מתנות',
                      icon: 'pi pi-gift',
                      route: 'gifts'
                  },
                  {
                      label: 'תורמים',
                      icon: 'pi pi-users',
                      route: 'donors'
                  },
                  {
                      label: 'קטגוריות',
                      icon: 'pi pi-list-check',
                      route: 'categories'
                  }
              ]
          }
      ];
      let token = localStorage.getItem("token") || ""
      let user = this.getDecodedAccessToken(token)
      if (user.role != "MANAGER")
        this.router.navigate(['home']);
  }
  getDecodedAccessToken(token: string): any {
    try {
      return jwtDecode(token);
    } catch(Error) {
      return null;
    }
  }
}
