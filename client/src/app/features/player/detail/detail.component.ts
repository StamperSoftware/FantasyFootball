import { Component, inject, OnInit } from '@angular/core';
import { Player } from "@models";
import { ActivatedRoute } from "@angular/router";
import { PlayerService } from "../../../core/services/player.service";

@Component({
  selector: 'app-player-detail',
  standalone: true,
  imports: [],
  templateUrl: './detail.component.html',
  styleUrl: './detail.component.scss'
})
export class PlayerDetailComponent implements OnInit {
    ngOnInit(): void {
        this.getPlayer();
    }
  
    private playerService = inject(PlayerService); 
    private activatedRoute = inject(ActivatedRoute);
  
    player?:Player;
    hasErrors = false;
    id:string|null = this.activatedRoute.snapshot.paramMap.get("id");
        
    getPlayer(){
        if (!this.id) {
            this.hasErrors = true;
            return;
        }
      
        this.playerService.getPlayer(+this.id).subscribe({
          next: player => this.player = player,
          error : err =>  this.hasErrors = true,
        });
    }

}
