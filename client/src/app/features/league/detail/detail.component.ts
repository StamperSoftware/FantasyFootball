import { Component, inject, OnInit } from '@angular/core';
import { LeagueService, SiteSettingsService, AccountService, ToastService } from "@services";
import { Game, League, Position, UserTeam } from "@models";
import { ActivatedRoute, RouterLink } from "@angular/router";
import { NgbModal } from "@ng-bootstrap/ng-bootstrap";
import {
  faAdd,
  faPlay,
  faRightToBracket,
  faShuffle,
  faCalendarAlt, faChevronCircleRight, faChevronCircleLeft, faGear
} from "@fortawesome/free-solid-svg-icons";
import { FaIconComponent } from "@fortawesome/angular-fontawesome";
import { SelectUserComponent } from "../../../ui/modals/select-user/select-user.component";

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
  private id = +this.route.snapshot.paramMap.get("league-id")!;
  accountService = inject(AccountService);
  private toastService = inject(ToastService);
  siteSettings = inject(SiteSettingsService);
  
  currentWeek = this.siteSettings.currentWeek() ?? 1;
  weeklySchedule:Game[] = [];
  league?:League; 
  currentTeam? :UserTeam;
  
  getLeague() {
    return this.leagueService.getLeague(this.id).subscribe({
      next:response => {
        this.league = response;
        this.currentTeam = this.league?.teams.find(t => t.userId == this.accountService.currentUser()?.id) ?? this.league?.teams[0];
        this.getWeeklySchedule();
      },
    });
  }
  
  addTeam(){
    
    const addUser = (userId:string) => this.leagueService.addUser(this.id, userId).subscribe({
      next: () => this.getLeague(),
      error: (err) => this.toastService.addToast({header:"Error", content:err.error}),
    });
    const modalRef = this.modalService.open(SelectUserComponent);
    modalRef.componentInstance.leagueId = this.id;
    modalRef.result.then(addUser,() => {});
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
  protected readonly faGear = faGear;
}
