import { Component, inject, OnInit } from '@angular/core';
import { LeagueService } from "../../../core/services/league.service";
import { League } from "../../../models";
import { ActivatedRoute, RouterLink } from "@angular/router";
import { NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { PlayerListComponent } from "../../player/list/list.component";
import { SelectPlayerComponent } from "../../player/select-player/select-player.component";

@Component({
  selector: 'app-league-detail',
  standalone: true,
  imports: [
    RouterLink
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
}
