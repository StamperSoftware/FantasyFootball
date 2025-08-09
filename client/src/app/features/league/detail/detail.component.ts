import { Component, inject, Input, OnInit } from '@angular/core';
import { LeagueService } from "../../../core/services/league.service";
import { Athlete, League, Position, UserTeam } from "@models";
import { ActivatedRoute, Router, RouterLink } from "@angular/router";
import { NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { SelectPlayerComponent } from "../../player/select-player/select-player.component";
import {
  faAdd,
  faPlay,
  faRightToBracket,
  faShuffle,
  faCalendarAlt
} from "@fortawesome/free-solid-svg-icons";
import { FaIconComponent } from "@fortawesome/angular-fontawesome";

@Component({
  selector: 'app-league-detail',
  standalone: true,
  imports: [
    RouterLink,
    FaIconComponent
  ],
  templateUrl: './detail.component.html',
  styleUrl: './detail.component.scss'
})
export class LeagueDetailComponent implements OnInit {
  ngOnInit(): void {
    this.getLeague();
  }
  
  private leagueService = inject(LeagueService);
  private route:ActivatedRoute = inject(ActivatedRoute);
  private modalService = inject(NgbModal);
  private id :number = +this.route.snapshot.paramMap.get("id")!;
  
  league?:League; 
  currentTeam? :UserTeam;
  
  getLeague() {
    return this.leagueService.getLeague(this.id).subscribe({
      next:response => {
        this.league = response;
        this.currentTeam = this.league?.teams[0];
      },
    });
  }
  
  addTeam(){
    
    const addPlayer = (playerId:number) => this.leagueService.addPlayer(this.id, playerId).subscribe({
      next: () => this.getLeague()
    });
    
    this.modalService.open(SelectPlayerComponent).result.then(addPlayer,() => {});
  }
  
  startLeague() {
    
  }
  
  createSchedule(){
    if (!this.league) return;
    this.leagueService.createSchedule(this.league?.id).subscribe({
      next:() => this.getLeague()
    });
  }
  
  handleUpdateTeams(e:any) {
    this.currentTeam = this.league?.teams.find(t => t.id == e.target.value);
  }
  
  protected readonly faAdd = faAdd;
  protected readonly faShuffle = faShuffle;
  protected readonly faPlay = faPlay;
  protected readonly faRightToBracket = faRightToBracket;
  protected readonly faCalendarAlt = faCalendarAlt;
  protected readonly Position = Position;
}
