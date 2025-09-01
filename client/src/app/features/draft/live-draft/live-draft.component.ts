import { Component, inject, OnDestroy, signal } from '@angular/core';
import { ActivatedRoute, Router } from "@angular/router";
import { AccountService, LeagueService, LiveDraftService } from "@services";
import { Athlete, DraftHelper, League, Position, UserTeam, DraftStatus } from "@models";
import { PositionDropdownComponent } from "../../../ui/dropdowns/position/position-dropdown.component";
import { UserTeamDropdownComponent } from "../../../ui/dropdowns/user-team/user-team-dropdown.component";
import { NgStyle } from "@angular/common";
import { FaIconComponent } from "@fortawesome/angular-fontawesome";
import { faAdd, faCircleArrowLeft } from "@fortawesome/free-solid-svg-icons";

@Component({
    selector: 'app-live-draft',
    standalone: true,
    templateUrl: './live-draft.component.html',
    imports: [
        PositionDropdownComponent,
        UserTeamDropdownComponent,
        NgStyle,
        FaIconComponent
    ],
    styleUrl: './live-draft.component.scss'
})
export class LiveDraftComponent implements OnDestroy {
    
    private liveDraftService = inject(LiveDraftService);
    private accountService = inject(AccountService);
    private leagueService = inject(LeagueService);
    private activeRoute = inject(ActivatedRoute);
    private router = inject(Router);
    position = signal<Position|'all'>('all');
    leagueId = +this.activeRoute.snapshot.paramMap.get("league-id")!;
    league!:League;
    userTeam = signal<UserTeam>({} as UserTeam);
    
    isConnected = this.liveDraftService.isConnected    
    draftHelper = signal<DraftHelper|undefined>(undefined);
    
    currentTeamId = signal<number|undefined>(undefined);
    
    constructor () {
        
        window.onbeforeunload = async () => {
            await this.disconnect();
        };
        
        this.leagueService.getLeague(this.leagueId).subscribe({
            next: async (league) => {
                this.league = league;
                
                this.userTeam.set(league.teams.find(ut => ut.player.appUser.id == this.accountService.currentUser()?.id)!);
                
                this.liveDraftService.addEventListener("DraftedPlayer", (draftHelper:DraftHelper)=> this.draftHelper.set(draftHelper));
                this.liveDraftService.addEventListener("JoinedGroup", (draftHelper:DraftHelper)=> this.draftHelper.set(draftHelper));
                this.liveDraftService.addEventListener("LeftGroup", (draftHelper:DraftHelper)=> this.draftHelper.set(draftHelper));
                
                await this.liveDraftService.start();
                await this.joinRoom();
                this.currentTeamId.set(this.draftHelper()?.teams[0]?.id ?? 0);
            },
        });
    }
    
    getAthletes(){
        if(this.position() == 'all') return this.draftHelper()?.availableAthletes;
        return this.draftHelper()?.availableAthletes.filter(a => a.position == this.position());
    }
    
    getRoster(){
        return this.draftHelper()?.teams.find(t => t.id == this.currentTeamId())?.roster.bench;
    }

    async disconnect(){
        
        if (this.draftHelper()?.teams?.find(ut => ut.player.user?.id == this.accountService.currentUser()?.id)?.isOnline) {
            await this.leaveRoom();
        }
        
        if (this.isConnected()){
            await this.liveDraftService.stop();
        }
    }
    
    async ngOnDestroy() {
        await this.disconnect();
    }
    
    async joinRoom(){
        await this.liveDraftService.joinGroup(this.leagueId).then(dh => this.draftHelper.set(dh));
    }
    
    async leaveRoom(){
        await this.liveDraftService.leaveGroup(this.leagueId).then(() => this.router.navigateByUrl(`leagues/${this.leagueId}`));
    }
    
    async draftPlayer(athleteId:number){
        if (!this.draftHelper()) return;
        await this.liveDraftService.draftPlayer(this.leagueId, this.draftHelper()!.currentPick.id,athleteId).then(dh => this.draftHelper.set(dh));
    }
    
    async simulateDraft(){
        if (!this.draftHelper()) return;
        await this.liveDraftService.simulateDraft(this.leagueId).then(dh => this.draftHelper.set(dh));
    }
    
    protected readonly Position = Position;
    protected readonly DraftStatus = DraftStatus;
    protected readonly faAdd = faAdd;
    protected readonly faCircleArrowLeft = faCircleArrowLeft;
}
