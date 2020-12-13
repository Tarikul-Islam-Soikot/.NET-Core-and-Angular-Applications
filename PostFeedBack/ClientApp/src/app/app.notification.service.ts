import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';

@Injectable()
export class NotificationService {

  constructor(public toastr: ToastrService) {
  }
  showSuccess(message: string, title?: string) {
    this.showSuccessExtra(message, title);
  }
  showError(message: string, title?: string) {
    this.showErrorExtra(message, title);
  }
  showWarning(message: string, title?: string) {
    this.showWarningExtra(message, title);
  }

  showErrorExtra(message: string, title?: string,
    disableTimeOut: boolean = false, timeOut: number = 4000, maxOpened: number = 3, tapToDismiss: boolean = true,
    progressBar: boolean = true, closeButton: boolean = true, positionClass: string = 'toast-top-center') {

    this.toastr.toastrConfig.maxOpened = maxOpened;
    this.toastr.error(message, title, {
      disableTimeOut: disableTimeOut,
      timeOut: timeOut,
      tapToDismiss: tapToDismiss,
      progressBar: progressBar,
      closeButton: closeButton,
      positionClass: positionClass
    });
  }

  showSuccessExtra(message: string, title?: string,
    disableTimeOut: boolean = false, timeOut: number = 4000, maxOpened: number = 3, tapToDismiss: boolean = true,
    progressBar: boolean = true, closeButton: boolean = true, positionClass: string = 'toast-top-center') {

    this.toastr.toastrConfig.maxOpened = maxOpened;
    this.toastr.success(message, title, {
      disableTimeOut: disableTimeOut,
      timeOut: timeOut,
      tapToDismiss: tapToDismiss,
      progressBar: progressBar,
      closeButton: closeButton,
      positionClass: positionClass
    });
  }

  showWarningExtra(message: string, title?: string,
    disableTimeOut: boolean = false, timeOut: number = 4000, maxOpened: number = 3, tapToDismiss: boolean = true,
    progressBar: boolean = true, closeButton: boolean = true, positionClass: string = 'toast-top-center') {

    this.toastr.toastrConfig.maxOpened = maxOpened;
    this.toastr.warning(message, title, {
      disableTimeOut: disableTimeOut,
      timeOut: timeOut,
      tapToDismiss: tapToDismiss,
      progressBar: progressBar,
      closeButton: closeButton,
      positionClass: positionClass
    });
  }

}

