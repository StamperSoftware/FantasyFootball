import { Component, inject, Input, OnInit } from '@angular/core';
import { Athlete, League, Position } from "../../../models";
import { ActivatedRoute, Router } from "@angular/router";
import { LeagueService } from "../../../core/services/league.service";
import { FaIconComponent } from "@fortawesome/angular-fontawesome";
import { faAdd } from "@fortawesome/free-solid-svg-icons";
import { NgbActiveModal, NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { SelectPlayerComponent } from "../../player/select-player/select-player.component";
import { SelectAthleteListComponent } from "../../athlete/select-list/select-list.component";
import { AthleteService } from "../../../core/services/athlete.service";
import { NgStyle } from "@angular/common";

@Component({
  selector: 'app-draft',
  standalone: true,
  imports: [
    FaIconComponent,
    NgStyle
  ],
  templateUrl: './draft.component.html',
  styleUrl: './draft.component.scss'
})
export class DraftComponent implements OnInit {
  ngOnInit(): void {
    this.getAthletes();
    this.getLeague();
  }
  
  protected readonly faAdd = faAdd;
  protected readonly Position = Position;
  
  private router = inject(Router);
  private route = inject(ActivatedRoute);
  private leagueService = inject(LeagueService);
  private athleteService = inject(AthleteService);
  private modalService = inject(NgbModal);
  
  leagueId = this.route.snapshot.paramMap.get("league-id");
  league? :League;
  hasErrors = false;
  playersPerTeam = 10;
  currentPick = 0;
  draftOrder:DraftSlot[] = [];
  draftResults:Map<number,number[]> = new Map();
  athletes?:Athlete[];
  
  getLeague() {
    if (!this.leagueId) return;
    this.leagueService.getLeague(+this.leagueId).subscribe({
      next:league => {
        this.league = league;
        this.setDraftOrder();
      },
      error: err => this.hasErrors = true,
    })
  }
  
  getAthletes() {
    this.athleteService.getAthletes().subscribe({
      next: athletes => this.athletes = athletes,
      error : err => this.hasErrors = true,
    })
  }
  
  openSelectAthlete() {
    let modal = this.modalService.open(SelectAthleteListComponent);
    modal.componentInstance.data = {athletes : this.athletes};
    modal.result.then(athlete => this.selectAthlete(athlete));
  }
  
  selectAthlete(athlete:Athlete) {
    this.draftOrder[this.currentPick].athlete = athlete; 
    this.athletes = this.athletes?.filter(a => a.id !== athlete.id);
    
    if (this.draftResults.has(this.draftOrder[this.currentPick]?.teamId)) {
      this.draftResults.get(this.draftOrder[this.currentPick].teamId)!.push(athlete.id);
    } else {
      this.draftResults.set(this.draftOrder[this.currentPick].teamId, [athlete.id])
    }
    
    this.currentPick++;
  }
  
  setDraftOrder() {
    
    if (!this.league?.teams) return;
    this.draftOrder = [];
    const teamLength = this.league.teams.length;
    const numberPicks = teamLength * this.playersPerTeam;
    
    let team;
    let isIncreasing = true;
    
    for (let i = 0; i < numberPicks; i++) {
      let remainder = i % teamLength;
      if (i > 0 && !remainder) {
        isIncreasing = !isIncreasing;
      }
      let position = i;
      if (isIncreasing) {
        team = this.league.teams[remainder];
      } else {
        position = ((teamLength + (teamLength * Math.floor(i/teamLength))) - remainder)-1;
        team = this.league.teams[(teamLength - 1) - remainder]
      }
      this.draftOrder.push({teamId:team.id, position});
    }    
  }
  
  simulateDraft(){
    const interval = setInterval(() => {
      if (this.athletes?.[0]) this.selectAthlete(this.athletes?.[0]);
      if (this.currentPick == this.draftOrder.length || this.hasErrors) clearInterval(interval);
    }, 0);
  }
  
  submitDraft(){
    if (!this.leagueId) return;
    if (!this.draftResults.size) return;
    
    this.leagueService.submitDraft(this.draftResults).subscribe({
      next:() => this.router.navigateByUrl(`/leagues/${this.leagueId}`),
      error : err => this.hasErrors = true,
    });
  }
  
  restartDraft(){
    this.currentPick = 0;
    this.draftResults = new Map(); 
    this.getAthletes();
    this.setDraftOrder();
  }
}

type DraftSlot = {
  position : number,
  teamId : number,
  athlete? : Athlete
}
