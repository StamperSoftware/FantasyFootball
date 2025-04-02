import { Component, inject, OnInit } from '@angular/core';
import { LeagueService } from "../../../core/services/league.service";
import { League } from "../../../models";
import { ActivatedRoute, Router } from "@angular/router";

@Component({
  selector: 'app-league-detail',
  standalone: true,
  imports: [],
  templateUrl: './detail.component.html',
  styleUrl: './detail.component.scss'
})
export class LeagueDetailComponent implements OnInit {
  ngOnInit(): void {
    this.getLeague();
  }
  
  private leagueService = inject(LeagueService);
  private route:ActivatedRoute = inject(ActivatedRoute);
  private id :number = +this.route.snapshot.paramMap.get("id")!;
  league?:League; 
  
  getLeague() {
    this.leagueService.getLeague(this.id).subscribe({
      next:response => this.league = response,
    });
  }
}
