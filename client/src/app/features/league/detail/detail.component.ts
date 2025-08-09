import { Component, inject, Input, OnInit } from '@angular/core';
import { LeagueService } from "../../../core/services/league.service";
import { Athlete, Game, League, Position, UserTeam } from "@models";
import { ActivatedRoute, Router, RouterLink } from "@angular/router";
import { NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { SelectPlayerComponent } from "../../player/select-player/select-player.component";
import {
  faAdd,
  faPlay,
  faRightToBracket,
  faShuffle,
  faCalendarAlt, faChevronCircleRight, faChevronCircleLeft
} from "@fortawesome/free-solid-svg-icons";
import { FaIconComponent } from "@fortawesome/angular-fontawesome";
import { SiteSettingsService } from "../../../core/services/site-settings.service";

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
  ngOnInit(): void {    this.getLeague();
  }
  
  private leagueService = inject(LeagueService);
  private route = inject(ActivatedRoute);
  private modalService = inject(NgbModal);
  private id = +this.route.snapshot.paramMap.get("id")!;
  
  siteSettings = inject(SiteSettingsService);
  currentWeek = this.siteSettings.currentWeek() ?? 1;
  weeklySchedule:Game[] = [];
  league?:League; 
  currentTeam? :UserTeam;
  
  getLeague() {
    return this.leagueService.getLeague(this.id).subscribe({
      next:response => {
        this.league = response;
        this.currentTeam = this.league?.teams[0];
        this.getWeeklySchedule();
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

  updateCurrentWeek(scale:number){
    this.currentWeek += scale;
    this.getWeeklySchedule();
  }

  getWeeklySchedule(){
    if (!this.league?.schedule) return;
    this.weeklySchedule = this.league.schedule.filter(s => s.week === this.currentWeek);
  }
  
  protected readonly faAdd = faAdd;
  protected readonly faShuffle = faShuffle;
  protected readonly faPlay = faPlay;
  protected readonly faRightToBracket = faRightToBracket;
  protected readonly faCalendarAlt = faCalendarAlt;
  protected readonly Position = Position;
  protected readonly faChevronCircleRight = faChevronCircleRight;
  protected readonly faChevronCircleLeft = faChevronCircleLeft;
}
