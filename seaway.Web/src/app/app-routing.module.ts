import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RoomListComponent } from './Components/room-list/room-list.component';
import { HomePageComponent } from './Components/home-page/home-page.component';
import { MainAdminPageComponent } from './Components/Admin/main-admin-page/main-admin-page.component';
import { RoomsComponent } from './Components/Admin/rooms/rooms.component';
import { ActivitiesComponent } from './Components/Admin/activities/activities.component';
import { AdminMainComponent } from './Components/Admin/admin-main/admin-main.component';
import { AddRoomComponent } from './Components/Admin/rooms/add-room/add-room.component';
import { AllRoomsComponent } from './Components/Admin/rooms/all-rooms/all-rooms.component';

const routes: Routes = [
  {path: '', component: HomePageComponent},
  {path: "rooms", component: RoomListComponent},
  {path: 'Administration', component: AdminMainComponent,
   children: [
    {path: '', component: MainAdminPageComponent},
    {path: 'Rooms', component: RoomsComponent,
    children: [
      {path: '', component: AllRoomsComponent},
      {path: 'addRoom', component:AddRoomComponent}
    ]
  },
    {path: 'Activities', component: ActivitiesComponent}
   ]}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
