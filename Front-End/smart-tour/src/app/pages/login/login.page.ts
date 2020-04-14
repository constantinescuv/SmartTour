import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';
import { ToastController, NavController } from '@ionic/angular';

@Component({
  selector: 'app-login',
  templateUrl: './login.page.html',
  styleUrls: ['./login.page.scss'],
})
export class LoginPage implements OnInit {

  postData = {
    email: '',
    passw: ''
    };
    
  constructor(
  private authService: AuthService,
  private navController: NavController,
  private toastController: ToastController
  ) {}
    
  ngOnInit() {
    if(!!localStorage.getItem('user')) {
      this.navController.navigateForward(['home'], { animated: false });
      this.loggedInToast(JSON.parse(localStorage.getItem('user')).username);
    }
  }

  async loggedInToast(username: string) {
    const toast = await this.toastController.create({
      message: 'Logged in as ' + username,
      color: 'primary',
      duration: 2000
    });

    toast.present();
  }
    
  async loginAction() {
    try { 
      const res = await this.authService.login(this.postData);

      const toast = await this.toastController.create({
        message: 'Login succesfull!',
        color: 'success',
        duration: 2000
      });

      toast.present();

      localStorage.setItem('user', JSON.stringify(res.body));

      this.navController.navigateForward(['home'], { animated: false });
    } catch {

      const toast = await this.toastController.create({
        message: 'Error! Check credentials..',
        color: 'danger',
        duration: 2000
      });

      toast.present();
    }
  }
}
