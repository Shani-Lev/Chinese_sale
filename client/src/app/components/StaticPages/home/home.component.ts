import { Component } from '@angular/core';
import { ImportsModule } from '../../../imports';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-home',
  imports: [ImportsModule, RouterLink],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {

}
