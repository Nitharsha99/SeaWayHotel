import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HeaderComponent } from './Components/header/header.component';
import { FooterComponent } from './Components/footer/footer.component';
import { HomePageComponent } from './Components/home-page/home-page.component';
import { SubHomePageComponent } from './Components/home-page/sub-home-page/sub-home-page.component';
import { NewActivityComponent } from './Components/new-activity/new-activity.component';
import { RoomListComponent } from './Components/room-list/room-list.component';
import { RoomViewComponent } from './Components/room-view/room-view.component';

@NgModule({
  declarations: [
    AppComponent,
    HeaderComponent,
    FooterComponent,
    HomePageComponent,
    SubHomePageComponent,
    NewActivityComponent,
    RoomListComponent,
    RoomViewComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
