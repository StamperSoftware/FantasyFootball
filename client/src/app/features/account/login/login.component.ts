import { Component, inject } from '@angular/core';
import { FormsModule } from "@angular/forms";
import { AccountService } from "../../../core/services/account.service";
import { NgbActiveModal } from "@ng-bootstrap/ng-bootstrap";

@Component({
  selector: 'app-login',
  standalone: true,
    imports: [
        FormsModule
    ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
  
    private accountService = inject(AccountService);
    private activeModal = inject(NgbActiveModal);
  
    hasErrors = false;
    email = "";
    password = "";
    
    handleLogin(){
      this.accountService.login({email:this.email, password:this.password}, () => this.activeModal.close(), () => this.hasErrors = true);
    }
}
