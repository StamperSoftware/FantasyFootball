import { Component, inject, model, OnInit, signal } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { Team } from "@models";
import { TeamsService } from "../../../core/services/teams.service";

@Component({
  selector: 'app-team-dropdown',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    FormsModule
  ],
  templateUrl: './team.component.html',
  styleUrl: './team.component.scss'
})
export class TeamComponent implements OnInit {
  
    private teamService = inject(TeamsService);
  
    ngOnInit(): void {
      this.teamService.getTeams().subscribe({
        next:teams => this.teams.set(teams)
      })  
    }
    teams = signal<Team[]>([]);
    team = model<number|'all'>();
}
