import { Component, inject } from '@angular/core';
import { Router, RouterLink } from "@angular/router";
import { AccountService } from "../../core/services/account.service";
import { NgbDropdown, NgbDropdownItem, NgbDropdownMenu, NgbDropdownToggle, NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { LoginComponent } from "../../features/account/login/login.component";
import { RegisterComponent } from "../../features/account/register/register.component";

@Component({
  selector: 'app-header',
  standalone: true,
    imports: [
        RouterLink,
        NgbDropdown,
        NgbDropdownMenu,
        NgbDropdownItem,
        NgbDropdownToggle
    ],
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss'
})
export class HeaderComponent {
    
    accountService = inject(AccountService);
    private modalService = inject(NgbModal);
    
    handleLogout(){
      this.accountService.logout();
    }
    openLoginModal(){
        this.modalService.open(LoginComponent).result.then();
    }
    
    openRegisterModal(){
        this.modalService.open(RegisterComponent).result.then();
    }
}
