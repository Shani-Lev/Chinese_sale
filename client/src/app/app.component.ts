import { Component, inject } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { ButtonModule } from 'primeng/button';
import { PasswordModule } from 'primeng/password';
import { FormsModule } from '@angular/forms';
import { FloatLabelModule } from 'primeng/floatlabel';
import { AllGiftComponent } from './components/Manage/Gifts/all-gift/all-gift.component';
import { AddGiftComponent } from './components/Manage/Gifts/add-gift/add-gift.component';
import { AddDonorComponent } from './components/Manage/Donors/add-donor/add-donor.component';
import { TemplateComponent } from './components/template/template.component';
import { ImportsModule } from './imports';
import { StatusService } from './services/status.service';

@Component({
    selector: 'app-root',
    imports: [RouterOutlet, TemplateComponent, ImportsModule],
    templateUrl: './app.component.html',
    styleUrl: './app.component.css'
})
export class AppComponent {
  ststuse = inject(StatusService)

  ngOnInit(){
    this.ststuse.setOnStorage()
  }

  title = 'client';
  value1!: string;

  value2!: string;

  value3!: string;

}
