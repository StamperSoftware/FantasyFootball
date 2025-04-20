import { Component, inject, Input, OnInit } from '@angular/core';
import { LeagueService } from "../../../core/services/league.service";
import { League } from "../../../models";
import { ActivatedRoute, Router, RouterLink } from "@angular/router";
import { NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { SelectPlayerComponent } from "../../player/select-player/select-player.component";
import { faAdd, faPlay, faRightToBracket, faShuffle } from "@fortawesome/free-solid-svg-icons";
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
  private router = inject(Router);
  league?:League; 
  
  getLeague() {
    this.leagueService.getLeague(this.id).subscribe({
      next:response => this.league = response,
    });
  }
  
  
  addPlayer(){
    
    const addPlayer = (playerId:number) => this.leagueService.addPlayer(this.id, playerId).subscribe({
      next: () => this.getLeague()
    });
    
    this.modalService.open(SelectPlayerComponent).result.then(addPlayer,() => {});
  }
  startLeague(){
    
  }
  
  protected readonly faAdd = faAdd;
  protected readonly faShuffle = faShuffle;
  protected readonly faPlay = faPlay;
  protected readonly faRightToBracket = faRightToBracket;
}
