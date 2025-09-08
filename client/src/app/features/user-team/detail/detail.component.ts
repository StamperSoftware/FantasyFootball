import { Component, inject, OnInit, signal } from '@angular/core';
import { UserTeamService } from "../../../core/services/user-team.service";
import { ActivatedRoute, RouterLink } from "@angular/router";
import { Athlete, Position, UserTeam } from "@models";
import { FaIconComponent } from "@fortawesome/angular-fontawesome";
import {
    faArrowDown,
    faArrowUp,
    faChevronDown,
    faCircleLeft,
    faEdit,
    faPlus, faShuffle,
    faTrashCan
} from "@fortawesome/free-solid-svg-icons";
import { NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { EditUserTeamComponent } from "../edit/edit.component";
import { SelectAthleteListComponent } from "../../athlete/select-list/select-list.component";
import { LeagueService } from "../../../core/services/league.service";
import { AccountService } from "@services";

@Component({
  selector: 'app-user-team-detail',
  standalone: true,
    imports: [
        FaIconComponent,
        RouterLink
    ],
  templateUrl: './detail.component.html',
  styleUrl: './detail.component.scss'
})
export class UserTeamDetailComponent implements OnInit {
    
    ngOnInit(): void {
        this.getTeam();
        if(this.accountService.currentUser()){
            this.leagueService.checkIfUserIsInLeague(this.accountService.currentUser()!.id, this.leagueId).subscribe({
                next:response => this.isUserInLeague.set(response),
            });
        }
    }
    
    private teamService = inject(UserTeamService);
    private leagueService = inject(LeagueService);
    private activatedRoute = inject(ActivatedRoute);
    private modalService = inject(NgbModal);
    accountService = inject(AccountService);
    isUserInLeague = signal(false); 
    
    protected readonly faEdit = faEdit;
    protected readonly faPlus = faPlus;
    
    team?:UserTeam;
    hasErrors = false;
    
    teamId = this.activatedRoute.snapshot.paramMap.get("user-team-id");
    leagueId = +this.activatedRoute.snapshot.paramMap.get("league-id")!;
    getTeam(){
      if(!this.teamId) {
        this.hasErrors = true;
        return;
      }
      
      this.teamService.getTeam(+this.teamId).subscribe({
        next: response => this.team = response,
        error: err => this.hasErrors = true
      })
    }
    
    openEditModal(){
        const modal = this.modalService.open(EditUserTeamComponent);
        modal.componentInstance.data = {name:this.team?.name, id :this.teamId};
        modal.result.then(()=>this.getTeam());
    }

    openAddAthleteModal() {
        if(!this.leagueId) return;
        this.leagueService.getAvailableAthletes(+this.leagueId).subscribe({
            next: athletes => {
                const modal = this.modalService.open(SelectAthleteListComponent);
                modal.componentInstance.data = {athletes};
                modal.result.then((athlete:Athlete) => this.addAthleteToTeam(athlete.id));
            }
        })
    }
    
    addAthleteToTeam(athleteId:number){
        if (this.teamId == null || this.leagueId == null) return;
        
        this.leagueService.addAthleteToTeam(+this.leagueId,athleteId, +this.teamId).subscribe({
            next: () => this.getTeam(),
            error : () => this.hasErrors = true,
        });
    }
    
    dropAthlete(athleteId:number) {
        if (!this.teamId) return;

        this.teamService.dropAthlete(athleteId, +this.teamId).subscribe({
            next: () => this.getTeam(),
            error : () => this.hasErrors = true,
        });
    }
    
    moveToBench(athleteId:number) {
        if (!this.teamId) return;

        this.teamService.moveToBench(athleteId, +this.teamId).subscribe({
            next: () => this.getTeam(),
            error : () => this.hasErrors = true,
        });
    }
    
    moveToStarters(athleteId:number) {
        if (!this.teamId) return;

        this.teamService.moveToStarters(athleteId, +this.teamId).subscribe({
            next: () => this.getTeam(),
            error : () => this.hasErrors = true,
        });
    }
    
    protected readonly faCircleLeft = faCircleLeft;
    protected readonly Position = Position;
    protected readonly faTrashCan = faTrashCan;
    protected readonly faArrowUp = faArrowUp;
    protected readonly faArrowDown = faArrowDown;
    protected readonly faShuffle = faShuffle;

    tradeForAthlete (id: number) {
        
    }
}
