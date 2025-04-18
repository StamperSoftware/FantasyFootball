import { Component, inject, OnInit } from '@angular/core';
import { ActivatedRoute } from "@angular/router";
import { Athlete, League, UserTeam } from "../../../models";
import { LeagueService } from "../../../core/services/league.service";
import { UserTeamService } from "../../../core/services/user-team.service";

@Component({
  selector: 'app-trade',
  standalone: true,
  imports: [],
  templateUrl: './trade.component.html',
  styleUrl: './trade.component.scss'
})
export class TradeComponent implements OnInit{
  ngOnInit(): void {
    this.getLeague();
  }
  
  private route = inject(ActivatedRoute);
  private leagueService = inject(LeagueService);
  private userTeamService = inject(UserTeamService);
  
  teamOneId? :number;
  teamTwoId? :number;

  teamOneSelectOptions? :UserTeam[];
  teamTwoSelectOptions? :UserTeam[];
  
  teamOne? : UserTeam;
  teamTwo? : UserTeam;
  
  teamOneOffer : Set<Athlete> = new Set();
  teamTwoOffer : Set<Athlete> = new Set();
  
  hasErrors = false;
  
  leagueId = this.route.snapshot.paramMap.get("league-id");
  league? : League;
  
  getLeague() {
    
    if (!this.leagueId) return;
    
    this.leagueService.getLeague(+this.leagueId).subscribe({
      next: league => {
        this.league = league;
        this.updateTeams();
      },
      error : err => this.hasErrors = true,
    })  
  }
  
  updateTeams() {
      this.teamOneId = this.teamOneId ?? this.league?.teams[0]?.id;
      this.teamTwoId = this.teamTwoId ?? this.league?.teams[1]?.id;
      
      this.teamOne = this.league?.teams.find(t => t.id == this.teamOneId);
      this.teamTwo = this.league?.teams.find(t => t.id == this.teamTwoId);
      
      this.teamOneSelectOptions = this.league?.teams.filter(t => t.id != this.teamTwoId && t.id != this.teamOneId);
      this.teamTwoSelectOptions = this.league?.teams.filter(t => t.id != this.teamOneId && t.id != this.teamTwoId);
      
      this.teamOneOffer.clear();
      this.teamTwoOffer.clear();
  }
  
  updateTeamOne(e:any){
    this.teamOneId = e.target.value;
    this.updateTeams();
  }
  updateTeamTwo(e:any){
    this.teamTwoId = e.target.value;
    this.updateTeams();
  }
  
  addToTeamOneOffer(id:number){
    const athlete = this.teamOne?.athletes.find(a => a.id == id);

    if(!athlete) return;

    if (this.teamOneOffer.has(athlete)) {
        this.teamOneOffer.delete(athlete);
      } else {
        this.teamOneOffer.add(athlete);
      }
  }
  addToTeamTwoOffer(id:number){
    const athlete = this.teamTwo?.athletes.find(a => a.id == id);
    
    if(!athlete) return;
    
    if (this.teamTwoOffer.has(athlete)) {
      this.teamTwoOffer.delete(athlete);
    } else {
      this.teamTwoOffer.add(athlete);
    }
  }
  
  submitTrade() {
    
    if (!this.teamOneId || !this.teamTwoId) return;
    if (!this.teamOneOffer.size || !this.teamTwoOffer.size) return;
    
    this.userTeamService.tradeAthletes(this.teamOneId, this.teamTwoId, Array.from(this.teamOneOffer).map(t => t.id), Array.from(this.teamTwoOffer).map(t => t.id)).subscribe({
      next: ()=> this.getLeague(),
      error: err => this.hasErrors = true,
    })
  }
  
}
