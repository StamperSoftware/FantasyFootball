import { Component, inject } from '@angular/core';
import { FormsModule } from "@angular/forms";
import { AccountService } from "../../../core/services/account.service";
import { RegisterDto } from "../../../models";
import { NgbActiveModal } from "@ng-bootstrap/ng-bootstrap";
import { Router } from "@angular/router";

@Component({
  selector: 'app-register',
  standalone: true,
    imports: [
        FormsModule
    ],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss'
})
export class RegisterComponent {
  private accountService = inject(AccountService);
  private activeModal = inject(NgbActiveModal);
  
  firstName = "";
  lastName = "";
  email = "";
  userName = "";
  password = "";
  
  handleRegister(){
    const registerDto : RegisterDto = {
      firstName : this.firstName,
      lastName : this.lastName,
      email : this.email,
      userName : this.userName,
      password : this.password,
    }
    this.accountService.registerUser(registerDto).subscribe({
      next: () => {
        this.activeModal.close();
      }
    })
  }
}
