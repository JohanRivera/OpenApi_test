import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CrudComponent } from './crud/crud.component';
import { TaskRegisterRoutingModule } from './task-register-routing.module';



@NgModule({
  declarations: [
    CrudComponent
  ],
  imports: [
    CommonModule,
    TaskRegisterRoutingModule
  ]
})
export class TaskRegisterModule { }
