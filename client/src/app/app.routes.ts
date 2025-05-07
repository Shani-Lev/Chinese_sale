import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AllGiftComponent } from './components/Manage/Gifts/all-gift/all-gift.component';
import { AddDonorComponent } from './components/Manage/Donors/add-donor/add-donor.component';
import { AddCategoryComponent } from './components/Manage/Categories/add-Category/add-Category.component';
import { AddGiftComponent } from './components/Manage/Gifts/add-gift/add-gift.component';
import { LoginComponent } from './components/Auth/login/login.component';
import { RegisterComponent } from './components/Auth/register/register.component';
import { HomeComponent } from './components/StaticPages/home/home.component';
import { AllDonorsComponent } from './components/Manage/Donors/all-donors/all-donors.component';
import { TicketsComponent } from './components/Manage/tickets/tickets.component';
import { ManagerTemplateComponent } from './components/Manage/manager-template/manager-template.component';
import { StatusComponent } from './components/Manage/status/status.component';
import { GiftsComponent } from './components/User/gifts/gifts.component';
import { CartComponent } from './components/User/cart/cart.component';
import { TicketsUserComponent } from './components/User/tickets-user/tickets-user.component';
import { AllCategoriesComponent } from './components/Manage/Categories/all-categories/all-categories.component';


export const routes: Routes = [

    { path: 'manage', component: ManagerTemplateComponent, children:[
        { path: '', redirectTo: 'status', pathMatch: 'full' }, 
        {
            path: 'gifts', component: AllGiftComponent, children: [
                {
                    path: 'add', component: AddGiftComponent, children: [
                        { path: 'addDonor', component: AddDonorComponent, outlet: 'addDonor' },
                        { path: 'addCategory', component: AddCategoryComponent, outlet: 'addCategory' },
                    ]
                },
            ]
        },
        {
            path: 'donors', component: AllDonorsComponent, children: [
                { path: 'addDonor', component: AddDonorComponent },
            ]
        },
        {
            path: 'categories', component: AllCategoriesComponent, children: [
                { path: 'addCategory', component: AddCategoryComponent },
            ]
        },
        { path: 'tickets', component: TicketsComponent },
        { path: 'status', component: StatusComponent },
    ] },    
    { path: 'gifts', component: GiftsComponent },
    { path: 'cart', component: CartComponent },
    { path: 'tickets', component: TicketsUserComponent },
    { path: 'login', component: LoginComponent },
    { path: 'register', component: RegisterComponent },
    { path: 'home', component: HomeComponent },
    { path: '', redirectTo: '/home', pathMatch: 'full' },
    { path: '**', component: HomeComponent }
];

