import { Component, inject, OnInit } from '@angular/core';
import { UserTeam } from "@models";
import { UserTeamService } from "../../../core/services/user-team.service";
import { RouterLink } from "@angular/router";

@Component({
  selector: 'app-user-team-list',
  standalone: true,
    imports: [
        RouterLink
    ],
  templateUrl: './list.component.html',
  styleUrl: './list.component.scss'
})
export class UserTeamListComponent implements OnInit {
    ngOnInit(): void {
      this.getTeams();
    }
    
    private teamService = inject(UserTeamService);
    
    teams?:UserTeam[];
    hasErrors=false;
    
    getTeams(){
      this.teamService.getTeams().subscribe({
        next : response => this.teams = response,
        error : err => this.hasErrors = true,
      })
    }
}
