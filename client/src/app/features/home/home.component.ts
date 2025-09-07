import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { HomeService } from "../../core/services/home.service";
import { AccountService, UserTeamService } from "@services";
import { UserTeam } from "@models";
import { RouterLink } from "@angular/router";
import { NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { LoginComponent } from "../account/login/login.component";
import { RegisterComponent } from "../account/register/register.component";
import { map } from "rxjs";

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [
    RouterLink
  ],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss'
})
export class HomeComponent implements OnInit {
  
  accountService = inject(AccountService);
  userTeamService = inject(UserTeamService);
  userTeams=signal<UserTeam[]>([]);
  
  private modalService = inject(NgbModal);

  openLoginModal(){
    this.modalService.open(LoginComponent).result.then();
  }

  openRegisterModal(){
    this.modalService.open(RegisterComponent).result.then();
  }
  
  ngOnInit(): void {
    let user = this.accountService.currentUser();
    if (user) {
      this.userTeamService.getUserTeams(user.id).subscribe({
        next:teams=>this.userTeams.set(teams),
      });    
    }
  }
  
}
