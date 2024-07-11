import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RoomListComponent } from './Components/room-list/room-list.component';
import { HomePageComponent } from './Components/home-page/home-page.component';
import { MainAdminPageComponent } from './Components/Admin/main-admin-page/main-admin-page.component';
import { RoomsComponent } from './Components/Admin/roomCategory/rooms.component';
import { ActivitiesComponent } from './Components/Admin/activities/activities.component';
import { AdminMainComponent } from './Components/Admin/admin-main/admin-main.component';
import { AddRoomComponent } from './Components/Admin/roomCategory/add-room/add-room.component';
import { AllRoomsComponent } from './Components/Admin/roomCategory/all-rooms/all-rooms.component';
import { LoginComponent } from './Components/Admin/login/login.component';
import { AllActivitiesComponent } from './Components/Admin/activities/all-activities/all-activities.component';
import { AddActivitiesComponent } from './Components/Admin/activities/add-activities/add-activities.component';

const routes: Routes = [
  {path: '', component: HomePageComponent},
  {path: "rooms", component: RoomListComponent},
  {path: 'Administration', component: AdminMainComponent,
   children: [
    {path: '', component: LoginComponent},
    {path: 'Home', component: MainAdminPageComponent},
    {path: 'Rooms', component: RoomsComponent,
    children: [
      {path: '', component: AllRoomsComponent},
      {path: 'addRoom', component:AddRoomComponent},
      {path: 'editRoom/:id', component:AddRoomComponent}
    ]
    },
    {path: 'Activities', component: ActivitiesComponent,
      children: [
        {path: '', component: AllActivitiesComponent},
        {path: 'addActivity', component: AddActivitiesComponent},
        {path: 'editActivity/:id', component: AddActivitiesComponent}
      ]
    }
        
    ]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
