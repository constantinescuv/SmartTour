import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { RouteReuseStrategy } from '@angular/router';

import { IonicModule, IonicRouteStrategy } from '@ionic/angular';
import { SplashScreen } from '@ionic-native/splash-screen/ngx';
import { StatusBar } from '@ionic-native/status-bar/ngx';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ModalPageModule } from './modal/modal.module';
import { PopoverComponent } from 'src/app/components/popover/popover.component';
import { DetailComponent } from './components/detail/detail.component';
import { DescriptionComponent } from './components/description/description.component';
import { CallNumber } from '@ionic-native/call-number/ngx';
import { EmailComposer } from '@ionic-native/email-composer/ngx';


@NgModule({
  declarations: [AppComponent, PopoverComponent, DetailComponent, DescriptionComponent],
  entryComponents: [PopoverComponent, DetailComponent, DescriptionComponent],
  imports: [BrowserModule, IonicModule.forRoot(), AppRoutingModule, ModalPageModule],
  providers: [
    StatusBar,
    SplashScreen,
    { provide: RouteReuseStrategy, useClass: IonicRouteStrategy },
    CallNumber,
    EmailComposer
  ],
  bootstrap: [AppComponent]
})
export class AppModule {}
