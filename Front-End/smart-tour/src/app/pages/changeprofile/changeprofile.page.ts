import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { NavController, ToastController } from '@ionic/angular';
import { stringify } from 'querystring';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-changeprofile',
  templateUrl: './changeprofile.page.html',
  styleUrls: ['./changeprofile.page.scss'],
})
export class ChangeprofilePage implements OnInit {

  constructor(private navController: NavController, private toastController: ToastController, private authService: AuthService) { }

  firstName: string;
  lastName: string;
  image: string;
  resetPlaces: boolean;
  resetTours: boolean;

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

  async editInfo() {
    var postData = {}
    if (!(this.firstName === undefined)) {postData["FirstName"]=this.firstName}
    else {postData["FirstName"]=JSON.parse(localStorage.getItem('user')).firstName}
    if (!(this.lastName === undefined)) {postData["LastName"]=this.lastName}
    else {postData["LastName"]=JSON.parse(localStorage.getItem('user')).lastName}
    if (!(this.image === undefined)) {postData["Image"]=this.image}
    else {postData["Image"]=JSON.parse(localStorage.getItem('user')).image}
    if (!(this.resetPlaces === undefined)) {postData["ResetPlaces"]= this.resetPlaces ? 1 : 0}
    if (!(this.resetTours === undefined)) {postData["ResetTours"]=this.resetTours ? 1 : 0}
    postData["Email"]=JSON.parse(localStorage.getItem('user')).email;
    
    try {
      const res = await this.authService.edit(postData);
      const toast = await this.toastController.create({
        message: 'Changes Saved!',
        color: 'success',
        duration: 2000
      });

      toast.present();

      localStorage.setItem('user', JSON.stringify(res.body));

      this.navController.navigateForward(['home/profile'], { animated: false });
    } catch {

      const toast = await this.toastController.create({
        message: 'Error!',
        color: 'danger',
        duration: 2000
      });

      toast.present();
    }
  }

}
