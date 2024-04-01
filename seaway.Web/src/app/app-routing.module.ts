import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RoomListComponent } from './Components/room-list/room-list.component';
import { HomePageComponent } from './Components/home-page/home-page.component';
import { MainAdminPageComponent } from './Components/Admin/main-admin-page/main-admin-page.component';
import { RoomsComponent } from './Components/Admin/rooms/rooms.component';
import { ActivitiesComponent } from './Components/Admin/activities/activities.component';
import { AdminMainComponent } from './Components/Admin/admin-main/admin-main.component';

const routes: Routes = [
  {path: '', component: HomePageComponent},
  {path: "rooms", component: RoomListComponent},
  {path: 'Administration', component: AdminMainComponent,
   children: [
    {path: '', component: MainAdminPageComponent},
    {path: 'Rooms', component: RoomsComponent},
    {path: 'Activities', component: ActivitiesComponent}
   ]}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
