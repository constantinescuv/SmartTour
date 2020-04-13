import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';
import { NavController, ToastController } from '@ionic/angular';

@Component({
  selector: 'app-register',
  templateUrl: './register.page.html',
  styleUrls: ['./register.page.scss'],
})
export class RegisterPage implements OnInit {

  postData = {
    username: '',
    email: '',
    passw: ''
    };
    
    constructor(private authservice: AuthService, private navController: NavController, private toastController: ToastController) {}
    
    ngOnInit() {
    }
    
    // validateInputs() {
    // console.log(this.postData);
    // let username = this.postData.username.trim();
    // let password = this.postData.passw.trim();
    // let email = this.postData.email.trim();
    // return (
    // this.postData.username &&
    // this.postData.passw &&
    // this.postData.email &&
    // username.length > 0 &&
    // email.length > 0 &&
    // password.length > 0
    // );
    // }
    
  async signupAction() {
    try { 
      const res = await this.authservice.register(this.postData);

      const toast = await this.toastController.create({
        message: 'Successfully registered!',
        color: 'success',
        duration: 2000
      });

      toast.present();

      this.navController.navigateForward(['login'], { animated: false });
    } catch {
      
      const toast = await this.toastController.create({
        message: 'Error while registering!',
        color: 'danger',
        duration: 2000
      });

      toast.present();

    }
  }

  

}
