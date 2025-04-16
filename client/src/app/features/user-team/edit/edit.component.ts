import { Component, inject, Input, input } from '@angular/core';
import { FormsModule } from "@angular/forms";
import { UserTeamService } from "../../../core/services/user-team.service";
import { NgbActiveModal } from "@ng-bootstrap/ng-bootstrap";

@Component({
  selector: 'app-edit-user-team',
  standalone: true,
    imports: [
        FormsModule
    ],
  templateUrl: './edit.component.html',
  styleUrl: './edit.component.scss'
})
export class EditUserTeamComponent {
  
  private userTeamService = inject(UserTeamService);
  private activeModal = inject(NgbActiveModal);
  @Input() data : {name:string, id:number} = {name:"", id:0};
  
  handleEdit(){
    this.userTeamService.updateTeamName(this.data.name, this.data.id).subscribe({
      next:() => this.activeModal.close(),
    });  
  }
  
}
