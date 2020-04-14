import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { ChangeprofilePageRoutingModule } from './changeprofile-routing.module';

import { ChangeprofilePage } from './changeprofile.page';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    ChangeprofilePageRoutingModule
  ],
  declarations: [ChangeprofilePage]
})
export class ChangeprofilePageModule {}
