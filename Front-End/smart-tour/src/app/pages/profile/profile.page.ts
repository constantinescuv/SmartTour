import { Component } from '@angular/core';
import { NavController, ToastController, PopoverController } from '@ionic/angular';
import { PopoverComponent } from 'src/app/components/popover/popover.component';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.page.html',
  styleUrls: ['./profile.page.scss'],
})
export class ProfilePage {

  constructor(private navController: NavController, 
    private toastController: ToastController,
    private popoverController: PopoverController) {}

    image=JSON.parse(localStorage.getItem('user')).image;
    firstName=JSON.parse(localStorage.getItem('user')).firstName;
    lastName=JSON.parse(localStorage.getItem('user')).lastName;
    placesVisited=JSON.parse(localStorage.getItem('user')).placesVisited;
    toursCompleted=JSON.parse(localStorage.getItem('user')).toursCompleted;

  ngOnInit() {
    if(!localStorage.getItem('user')) {
      this.navController.navigateForward(['login'], { animated: false });
      this.notLoggedInToast();
    }
  }

  async notLoggedInToast() {
    const toast = await this.toastController.create({
      message: 'You are not logged in!',
      color: 'danger',
      duration: 2000
    });

    toast.present();
  }

  async presentPopover(ev: any) {
    const popover = await this.popoverController.create({
      component: PopoverComponent,
      event: ev,
      translucent: true
    });
    return await popover.present();
  }
}

