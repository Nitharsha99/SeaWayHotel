import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { NgxDropzoneModule } from 'ngx-dropzone';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { NgxPaginationModule } from 'ngx-pagination';
import { NgbDatepickerModule } from '@ng-bootstrap/ng-bootstrap';

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
import { ActivitiesComponent } from './Components/Admin/activities/activities.component';
import { AdminMainComponent } from './Components/Admin/admin-main/admin-main.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { LoginComponent } from './Components/Admin/login/login.component';
import { AddActivitiesComponent } from './Components/Admin/activities/add-activities/add-activities.component';
import { AllActivitiesComponent } from './Components/Admin/activities/all-activities/all-activities.component';
import { CommonFunctionComponent } from './commonFunction';
import { OffersComponent } from './Components/Admin/offers/offers.component';
import { AllOffersComponent } from './Components/Admin/offers/all-offers/all-offers.component';
import { AddOffersComponent } from './Components/Admin/offers/add-offers/add-offers.component';
import { ManagersComponent } from './Components/Admin/managers/managers.component';
import { ForgotPasswordComponent } from './Components/Admin/login/forgot-password/forgot-password.component';
import { NewPasswordComponent } from './Components/Admin/login/new-password/new-password.component';
import { RoomCategoriesComponent } from './Components/Admin/room-categories/room-categories.component';
import { AddRoomCategoryComponent } from './Components/Admin/room-categories/add-room-category/add-room-category.component';
import { AllRoomCategoiesComponent } from './Components/Admin/room-categories/all-room-categoies/all-room-categoies.component';
import { AllRoomsComponent } from './Components/Admin/rooms/all-rooms/all-rooms.component';
import { AddRoomComponent } from './Components/Admin/rooms/add-room/add-room.component';
import { RoomsComponent } from './Components/Admin/rooms/rooms.component';


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
    ActivitiesComponent,
    AdminMainComponent,
    LoginComponent,
    AddActivitiesComponent,
    AllActivitiesComponent,
    AllOffersComponent,
    AddOffersComponent,
    OffersComponent,
    ManagersComponent,
    ForgotPasswordComponent,
    NewPasswordComponent,
    RoomCategoriesComponent,
    AddRoomCategoryComponent,
    AllRoomCategoiesComponent,
    AllRoomsComponent,
    AddRoomComponent,
    RoomsComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    NgxDropzoneModule,
    ReactiveFormsModule,
    BrowserAnimationsModule,
    NgxPaginationModule,
    FormsModule,
    NgbDatepickerModule
  ],
  providers: [CommonFunctionComponent],
  bootstrap: [AppComponent]
})
export class AppModule { }
