import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { NgxDropzoneModule } from 'ngx-dropzone';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HeaderComponent } from './Components/header/header.component';
import { FooterComponent } from './Components/footer/footer.component';
import { HomePageComponent } from './Components/home-page/home-page.component';
import { SubHomePageComponent } from './Components/home-page/sub-home-page/sub-home-page.component';
import { NewActivityComponent } from './Components/new-activity/new-activity.component';
import { RoomListComponent } from './Components/room-list/room-list.component';
import { RoomViewComponent } from './Components/room-view/room-view.component';
import { MainAdminPageComponent } from './Components/Admin/main-admin-page/main-admin-page.component';
import { RoomsComponent } from './Components/Admin/rooms/rooms.component';
import { ActivitiesComponent } from './Components/Admin/activities/activities.component';
import { AdminMainComponent } from './Components/Admin/admin-main/admin-main.component';
import { AddRoomComponent } from './Components/Admin/rooms/add-room/add-room.component';
import { AllRoomsComponent } from './Components/Admin/rooms/all-rooms/all-rooms.component';

@NgModule({
  declarations: [
    AppComponent,
    HeaderComponent,
    FooterComponent,
    HomePageComponent,
    SubHomePageComponent,
    NewActivityComponent,
    RoomListComponent,
    RoomViewComponent,
    MainAdminPageComponent,
    RoomsComponent,
    ActivitiesComponent,
    AdminMainComponent,
    AddRoomComponent,
    AllRoomsComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    NgxDropzoneModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
