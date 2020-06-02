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
    firstName: '',
    lastName: '',
    email: '',
    passw: ''
    };
    
  constructor(private authservice: AuthService, private navController: NavController, private toastController: ToastController) {}
    
  ngOnInit() {
    if(!!localStorage.getItem('user')) {
      this.navController.navigateForward(['home/tutorial'], { animated: false });
      this.loggedInToast(JSON.parse(localStorage.getItem('user')).firstName);
    }
  }

  async loggedInToast(firstName: string) {
    const toast = await this.toastController.create({
      message: 'Logged in as ' + firstName,
      color: 'primary',
      duration: 2000
    });

    toast.present();
  }
    
  validateInputs() {
    let firstName = this.postData.firstName.trim();
    let lastName = this.postData.lastName.trim();
    let password = this.postData.passw.trim();
    let email = this.postData.email.trim();

    return (
    firstName.length > 0 &&
    firstName.length < 100 &&
    lastName.length > 0 &&
    lastName.length < 100 &&
    email.length > 0 &&
    email.length < 100 &&
    password.length > 0 &&
    password.length < 100 &&
    email.includes("@") &&
    email.includes(".")
    );
  }
    
  async signupAction() {
    if (!this.validateInputs()) {
      const toast = await this.toastController.create({
        message: 'Invalid inputs!',
        color: 'danger',
        duration: 2000
      });

      toast.present();
    }
    else {
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

  

}
