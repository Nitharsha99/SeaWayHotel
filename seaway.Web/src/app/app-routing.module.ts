import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RoomListComponent } from './Components/room-list/room-list.component';
import { HomePageComponent } from './Components/home-page/home-page.component';
import { MainAdminPageComponent } from './Components/Admin/main-admin-page/main-admin-page.component';
import { ActivitiesComponent } from './Components/Admin/activities/activities.component';
import { AdminMainComponent } from './Components/Admin/admin-main/admin-main.component';
import { LoginComponent } from './Components/Admin/login/login.component';
import { AllActivitiesComponent } from './Components/Admin/activities/all-activities/all-activities.component';
import { AddActivitiesComponent } from './Components/Admin/activities/add-activities/add-activities.component';
import { OffersComponent } from './Components/Admin/offers/offers.component';
import { AllOffersComponent } from './Components/Admin/offers/all-offers/all-offers.component';
import { AddOffersComponent } from './Components/Admin/offers/add-offers/add-offers.component';
import { ManagersComponent } from './Components/Admin/managers/managers.component';
import { ForgotPasswordComponent } from './Components/Admin/login/forgot-password/forgot-password.component';
import { NewPasswordComponent } from './Components/Admin/login/new-password/new-password.component';
import { RoomsComponent } from './Components/Admin/rooms/rooms.component';
import { AllRoomsComponent } from './Components/Admin/rooms/all-rooms/all-rooms.component';
import { AllRoomCategoiesComponent } from './Components/Admin/room-categories/all-room-categoies/all-room-categoies.component';
import { AddRoomComponent } from './Components/Admin/rooms/add-room/add-room.component';
import { AddRoomCategoryComponent } from './Components/Admin/room-categories/add-room-category/add-room-category.component';

const routes: Routes = [
  {path: '', component: HomePageComponent},
  {path: "rooms", component: RoomListComponent},
  {path: 'Administration', component: AdminMainComponent,
   children: [
    {path: '', component: LoginComponent},
    {path: 'password', component: ForgotPasswordComponent},
    {path: 'reset', component: NewPasswordComponent},
    {path: 'Home', component: MainAdminPageComponent},
    {path: 'Rooms', component: RoomsComponent,
     children: [
      {path: '', component: AllRoomsComponent},
      {path: 'addRoom', component: AddRoomComponent},
      {path: 'editRoom/:id', component: AddRoomComponent},
      {path: 'types', component: AllRoomCategoiesComponent,
        children: [
          {path: 'add', component: AddRoomCategoryComponent},
          {path: 'edit/:id', component: AddRoomCategoryComponent},
        ]
      }
    ]
    },
    {path: 'Activities', component: ActivitiesComponent,
      children: [
        {path: '', component: AllActivitiesComponent},
        {path: 'addActivity', component: AddActivitiesComponent},
        {path: 'editActivity/:id', component: AddActivitiesComponent}
      ]
    },
    {path: 'Offers', component: OffersComponent,
      children: [
        {path: '', component: AllOffersComponent},
        {path: 'addOffer', component: AddOffersComponent},
        {path: 'editOffer/:id', component: AddOffersComponent}
      ]
    },
    // {path: 'Packages', component: PackagesComponent,
     
    // },
    {path: 'Managers', component: ManagersComponent}   
    ]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
