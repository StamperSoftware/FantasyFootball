import { Component, inject, OnInit } from '@angular/core';
import { LeagueService } from "../../core/services/league.service";
import { League } from "../../models";
import { FaIconComponent } from "@fortawesome/angular-fontawesome";
import { faAdd } from "@fortawesome/free-solid-svg-icons";
import { RouterLink } from "@angular/router";

@Component({
  selector: 'app-leagues',
  standalone: true,
    imports: [
        FaIconComponent,
        RouterLink
    ],
  templateUrl: './leagues.component.html',
  styleUrl: './leagues.component.scss'
})
export class LeaguesComponent implements OnInit{
  
    protected readonly faAdd = faAdd;
    private leagueService = inject(LeagueService);
    leagues?:League[];
    hasErrors:boolean = false;
    
    ngOnInit(): void {
      this.getLeagues();
    }

    createLeague() {
      this.leagueService.createLeague({Name:"newLeague"}).subscribe({
        next: () => this.getLeagues(),
      })
    }
    
    getLeagues(){
      this.leagueService.getLeagues().subscribe(
          {
            next: (response) => this.leagues = response.data,
            error: err => this.hasErrors = true
          }
      )
    }
}
