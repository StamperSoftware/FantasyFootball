import { Component, inject, OnInit } from '@angular/core';
import { GameService } from "../../../core/services/game.service";
import { Game, Position } from "@models";
import { ActivatedRoute } from "@angular/router";

@Component({
  selector: 'app-game-detail',
  standalone: true,
  imports: [],
  templateUrl: './detail.component.html',
  styleUrl: './detail.component.scss'
})
export class GameDetailComponent implements OnInit {
    
    private gameService = inject(GameService);
    private route = inject(ActivatedRoute);
    gameId = this.route.snapshot.paramMap.get("id");
    game?:Game;
    hasErrors = false;
    ngOnInit(): void {
         this.getGame(); 
    }

    
    getGame(){
        
        if(!this.gameId) return;
        
        this.gameService.getGame(+this.gameId).subscribe({
            next: response => this.game = response,
            error : err => this.hasErrors = true,
        });
    }

    getAthleteStats(athleteId:number) {
        if (!this.game) return;
        
        return this.game.weeklyStats.find(ws => ws.athleteId == athleteId);
    }
    
    protected readonly Position = Position;
}
