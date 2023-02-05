import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { ProjectDto } from 'src/app/core/models/projects.model';
import { SearchTimeslipDto, TaskRegisterDto } from 'src/app/core/models/task-register.model';
import { AuthService } from 'src/app/core/services/auth.service';
import { ProjectsService } from 'src/app/core/services/projects.service';
import { TaskRegisterService } from 'src/app/core/services/task-register.service';
import { ColumnMode } from '@swimlane/ngx-datatable';
import Swal from 'sweetalert2';
import { UserDataResult } from 'angular-auth-oidc-client';

@Component({
  selector: 'app-crud',
  templateUrl: './crud.component.html',
  styleUrls: ['./crud.component.css']
})
export class CrudComponent implements OnInit {

  //#region Definición de variables

  /*Variables para visualizar la tabla con opciones inicial*/
  @ViewChild('tableTimelips') table: any;

  ColumnMode = ColumnMode;
  userData!: UserDataResult;
  funder = [];
  calculated = [];
  pending = [];
  groups = [];

  editing = {};

  // Lista de proyectos
  project: ProjectDto[] = [];
  selectProject: ProjectDto = {id: '', subject: '', projectName: '', active: true};

  // Lista timeslips
  tasksRegister: TaskRegisterDto[] = [];

  // Timeslips para actualizar o crear
  timeslips: TaskRegisterDto = { subjectUserId: '', projectId: '', projectName: '', subject: '', taskDescription: '',
    comment: '', assignedDate: '', assignedTo: '', time: 0 }; 

  // Timeslip para busqueda especifica
  searchTimes: SearchTimeslipDto = { subjectUserId: '', projectId: '', dateLimitDown: '', dateLimitUp: '', isAll: true };

  // Dropdown Projects
  public dropdownSettings = {};

  //#endregion

  constructor(private authService: AuthService, 
    private projectService: ProjectsService,
    private taskService: TaskRegisterService) {}

  ngOnInit(): void {
    // 1. Obtener data del usuario
    this.authService.userData.subscribe(response => {
      this.userData = response; 
    });

    // 2. Obtener lista de proyectos
    this.projectService.GetAllProjects().subscribe(response => {
      this.project = response.content;
      // this.authService.userData.subscribe(res => {
      //   this.timeslips = { subjectUserId: res.userData['sub'], 
      //     projectId: this.project[0].id, 
      //     projectName: this.project[0].projectName, 
      //     subject: '', 
      //     taskDescription: 'Desarrollo interfaces de usuario.',
      //     comment: '', 
      //     assignedDate:  new Date().toISOString().replace('T',' ').replace('Z', ''), 
      //     assignedTo: `${res.userData['given_name']} ${res.userData['family_name']}`, 
      //     time: 10
      //   };
      //   this.taskService.CreateTimeslip(this.timeslips).subscribe(create => {
      //     console.log(create);
      //   });
      // }); creación
    });

    // 3. Obtener consulta de timeslip
    this.authService.userData.subscribe(res => {
      this.searchTimes = {
        subjectUserId: res.userData['sub'],
        projectId: '',
        isAll: true,
        dateLimitDown: '2023-02-04',
        dateLimitUp: '2023-02-05'
      }
      this.taskService.GetTimeslips(this.searchTimes).subscribe(res => {
        this.tasksRegister = res.content;
      })
    });

    this.configDropDown();
  }

  //#region Configuración y funciones para el dropdown

  configDropDown() {
    this.dropdownSettings = {
        singleSelection: true,
        defaultOpen: false,
        idField: 'id',
        textField: 'projectName',
        searchPlaceholderText: "Buscar",
        noDataAvailablePlaceholderText: "Datos no encontrados",
        itemsShowLimit: 1,
        allowSearchFilter: true
    };
  }

  onDeselect() { // Función para los items seleccionados en la busqueda
    //this.selectedItems = [];
  }

  onItemSelect() {

  }
  
  //#endregion
  
  //#region Configuración y funciones para la tabla

  getGroupRowHeight(group: any[], rowHeight: any) {
    let style = {};

    style = {
      height: group.length * 30 + 'px',
      width: '100%'
    };

    return style;
  }

  toggleExpandGroup(group: any[]) {
    console.log('Toggled Expand Group!', group);
    this.table.groupHeader.toggleExpandGroup(group);
  }

  onSelect(row: any){

  }

  //#endregion

  downloadReport(){
    Swal.fire({
      icon: 'success',
      text: 'Reporte descargado con exito.'
    })
  }
}
