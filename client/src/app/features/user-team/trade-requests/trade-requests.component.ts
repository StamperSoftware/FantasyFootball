import { Component, inject, OnInit } from '@angular/core';
import { UserTeamService } from "@services";
import { TradeRequestTeamDto } from "@models";
import { FaIconComponent } from "@fortawesome/angular-fontawesome";
import { faThumbsDown, faThumbsUp } from "@fortawesome/free-solid-svg-icons";
import { ActivatedRoute } from "@angular/router";

@Component({
  selector: 'app-trade-requests',
  standalone: true,
  imports: [
    FaIconComponent
  ],
  templateUrl: './trade-requests.component.html',
  styleUrl: './trade-requests.component.scss'
})
export class TradeRequestsComponent implements OnInit {
  
    ngOnInit(): void {
      this.getRequests();
    }

    private activatedRoute = inject(ActivatedRoute);
    private userTeamService = inject(UserTeamService);
    private userId = +this.activatedRoute.snapshot.paramMap.get("user-team-id")!;
    
    initiatedRequests:TradeRequestTeamDto[] = [];
    receivedRequests:TradeRequestTeamDto[] = [];
  
    confirmTradeRequest(requestId:string){
      this.userTeamService.confirmTradeRequest(requestId).subscribe({
        next:() => this.getRequests()
      });
    }
    declineTradeRequest(requestId:string){
      this.userTeamService.declineTradeRequest(requestId).subscribe({
        next:() => this.getRequests()
      });
    }
    
    getRequests(){
        this.userTeamService.getReceivedTradeRequests(this.userId).subscribe({
            next: requests => this.receivedRequests = requests,
        })
        this.userTeamService.getInitiatedTradeRequests(this.userId).subscribe({
            next: requests => this.initiatedRequests = requests,
        })
    }
    
    protected readonly faThumbsUp = faThumbsUp;
    protected readonly faThumbsDown = faThumbsDown;
}
