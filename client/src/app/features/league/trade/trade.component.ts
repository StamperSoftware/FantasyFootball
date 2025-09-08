import { Component, inject, OnInit } from '@angular/core';
import { ActivatedRoute } from "@angular/router";
import { Athlete, League, Position, UserTeam } from "@models";
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
  
  protected readonly Position = Position;
  
  private route = inject(ActivatedRoute);
  private leagueService = inject(LeagueService);
  private userTeamService = inject(UserTeamService);
  
  leagueId = this.route.snapshot.paramMap.get("league-id");
  league? : League;
  sendingTeam? : TradeTeam;
  receivingTeam? : TradeTeam;
  options? :UserTeam[];
  hasErrors = false;
  
  getLeague() {
    
    if (!this.leagueId) return;
    
    this.leagueService.getLeague(+this.leagueId).subscribe({
      next: league => {
        this.league = league;
        const sendingId = this.sendingTeam?.id ?? this.league?.teams[0]?.id;
        const receivingId = this.receivingTeam?.id ?? this.league?.teams[1]?.id;
        
        this.sendingTeam = {
          id : sendingId,
          team : this.league?.teams.find(t => t.id == sendingId)!,
          offer : new Set<Athlete>(),
        }
        
        this.receivingTeam = {
          id: receivingId,
          team : this.league?.teams.find(t => t.id == receivingId)!,
          offer: new Set<Athlete>(),
        }
        
        this.updateSelectOptions();
      },
      error : err => this.hasErrors = true,
    })  
  }
  
  updateSelectOptions() {
    this.options = this.league?.teams.filter(t => t.id != this.receivingTeam?.id && t.id != this.sendingTeam?.id);
  }
  
  updateTeam(e:any, team?:TradeTeam) {
    
    if (!team) return;
    
    const id = e.target.value;
    
    team.id = id;
    team.offer = new Set<Athlete>();
    team.team = this.league?.teams.find(t => t.id == id)!;
    
    this.updateSelectOptions();
  }
  
  updateOffer(id:number, team?:Set<Athlete>){
    
    if (!this.sendingTeam?.team || !this.receivingTeam?.team) return;
    if (!team) return;
    
    const athlete = [...this.sendingTeam?.team?.roster?.starters, ...this.sendingTeam?.team?.roster?.bench, 
      ...this.receivingTeam?.team?.roster?.starters, ...this.receivingTeam?.team?.roster?.bench].find(a => a.id == id);
    if (!athlete) return;
    
    if (team.has(athlete)) {
      team.delete(athlete);
    } else {
      team.add(athlete);
    }
  }
  
  submitTrade() {
    
    if (!this.sendingTeam?.id || !this.sendingTeam?.offer.size) return;
    if (!this.receivingTeam?.id || !this.receivingTeam.offer.size) return;
    this.userTeamService.createTradeRequest(this.sendingTeam.id, this.receivingTeam.id, Array.from(this.sendingTeam.offer).map(t => t.id), Array.from(this.receivingTeam.offer).map(t => t.id)).subscribe({
      next: () => this.getLeague(),
      error: err => this.hasErrors = true,
    })
  }

}
type TradeTeam = {
  id : number,
  team : UserTeam,
  offer : Set<Athlete>
}