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
    
  async validateInputs() {
    let firstName = this.postData.firstName.trim();
    let lastName = this.postData.lastName.trim();
    let password = this.postData.passw.trim();
    let email = this.postData.email.trim();

    var valid = true;

    if (!/^[a-zA-Z]+$/.test(firstName) || firstName.length <= 0 || firstName.length >= 100){
      const toast = await this.toastController.create({
        message: 'First name must not contain digits or symbols!',
        color: 'danger',
        duration: 2000
      });
      valid = false;

      setTimeout(function () {
        toast.present();
      }, 4000);
    }
    if (!/^[a-zA-Z]+$/.test(lastName) || lastName.length <= 0 || lastName.length >= 100){
      const toast = await this.toastController.create({
        message: 'Last name must not contain digits or symbols!',
        color: 'danger',
        duration: 2000
      });
      valid = false;

      setTimeout(function () {
        toast.present();
      }, 6000);
    }
    if (!email.includes("@") || !email.includes(".") || email.length < 5 || email.length >= 100){
      const toast = await this.toastController.create({
        message: 'Enter a valid email!',
        color: 'danger',
        duration: 2000
      });
      valid = false;

      setTimeout(function () {
        toast.present();
      }, 8000);
    }
    if (password.length < 6 || password.length >= 100 || !/\d/.test(password)){
      const toast = await this.toastController.create({
        message: 'Password must be at least 6 characters long and contain at least one number!',
        color: 'danger',
        duration: 2000
      });
      valid = false;

      setTimeout(function () {
        toast.present();
      }, 3000);
    }
    
    return valid;
  }
    
  async signupAction() {
    var resp = await this.validateInputs();
    console.log(resp);
    if (!resp) {
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
