import { Component, inject, OnInit } from '@angular/core';
import { LeagueService, UserService } from "@services";
import { NgbActiveModal } from "@ng-bootstrap/ng-bootstrap";
import { AppUser } from "@models";

@Component({
  selector: 'app-select-user',
  standalone: true,
  imports: [],
  templateUrl: './select-user.component.html',
  styleUrl: './select-user.component.scss'
})
export class SelectUserComponent implements OnInit {
  
  ngOnInit(): void {
    this.Users();
  }

  private userService = inject(UserService)
  private leagueService = inject(LeagueService)
  private activeModal = inject(NgbActiveModal);
  
  users?:AppUser[];
  hasErrors = false;
  leagueId:number|undefined;

  Users(){
    if (this.leagueId) {

      this.leagueService.getUsersNotInLeague(this.leagueId).subscribe({
        next: response => this.users = response,
        error : err => this.hasErrors = true,
      })
    } else {
      this.userService.getUsers().subscribe({
        next: response => this.users = response,
        error : err => this.hasErrors = true,
      })
    }

  }

  handleSelectUser(id:string){
    this.activeModal.close(id);
  }
}
