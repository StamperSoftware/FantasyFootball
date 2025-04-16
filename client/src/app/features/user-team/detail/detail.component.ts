import { Component, inject, OnInit } from '@angular/core';
import { UserTeamService } from "../../../core/services/user-team.service";
import { ActivatedRoute } from "@angular/router";
import { UserTeam } from "../../../models";
import { FaIconComponent } from "@fortawesome/angular-fontawesome";
import { faEdit } from "@fortawesome/free-solid-svg-icons";
import { NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { EditUserTeamComponent } from "../edit/edit.component";

@Component({
  selector: 'app-user-team-detail',
  standalone: true,
    imports: [
        FaIconComponent
    ],
  templateUrl: './detail.component.html',
  styleUrl: './detail.component.scss'
})
export class UserTeamDetailComponent implements OnInit {
    
    ngOnInit(): void {
      this.getTeam();
    }
    
    private teamService = inject(UserTeamService);
    private activatedRoute = inject(ActivatedRoute);
    private modalService = inject(NgbModal);
    
    team?:UserTeam;
    hasErrors = false;
    
    teamId = this.activatedRoute.snapshot.paramMap.get("id");
  
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

    protected readonly faEdit = faEdit;
}
