import { Injectable } from '@angular/core';
import { ToastController } from '@ionic/angular';

@Injectable()
export class CustomToastController {
    
    private readonly defaultErrorMessage: string = "Something went wrong, please try again!";

    constructor(private toastController: ToastController) {
    }

    async showNormalToast(message:string) {
        const toast = await this.toastController.create({
            message: message,
            color: 'primary',
            duration: 2000
        });

        toast.present();
    }

    async showErrorToast(message:string = this.defaultErrorMessage) {
        const toast = await this.toastController.create({
            message: message,
            color: 'danger',
            duration: 2000
        });

        toast.present();
    }
}