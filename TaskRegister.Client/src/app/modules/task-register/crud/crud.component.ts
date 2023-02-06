import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { CreateProject, ProjectDto, SelectProject } from 'src/app/core/models/projects.model';
import { DeleteTimeslipDto, SearchTimeslipDto, TaskRegisterDto, TimelipsDto } from 'src/app/core/models/task-register.model';
import { AuthService } from 'src/app/core/services/auth.service';
import { ProjectsService } from 'src/app/core/services/projects.service';
import { TaskRegisterService } from 'src/app/core/services/task-register.service';
import { ColumnMode, isNullOrUndefined } from '@swimlane/ngx-datatable';
import Swal from 'sweetalert2';
import { UserDataResult } from 'angular-auth-oidc-client';
import { FormControl, FormGroup, NgForm } from '@angular/forms';

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
  
  // Variables para los proyectos
  project: ProjectDto[] = [];
  projectFilter: ProjectDto[] = [];
  projectToAddTask: ProjectDto = {id: '', subject: '', projectName: '', active: true};
  addProject: CreateProject = {projectName: ''};
  projectSelectDrop: SelectProject[] = []; 
  updateSelectDrop: SelectProject[] = []; 

  //Variables para timeslips
  startTime: boolean = false;
  pauseTime: boolean = false;
  timer: string = '0';
  secondsTimer: number = 0;
  interval: any;

  // Tarea eliminar
  timesDelete: TimelipsDto = { subjectUserId: '', projectId: '', projectName: '', subject: '', taskDescription: '',
    comment: '', assignedDate: '', assignedTo: '', time: '' }; 
  taskDelete: DeleteTimeslipDto = { subject: '' }

  // actualizar
  taksUpdate: TaskRegisterDto = { subjectUserId: '', projectId: '', projectName: '', subject: '', taskDescription: '',
    comment: '', assignedDate: '', assignedTo: '', time: 0 }; 

  // Lista timeslips
  tasksRegister: TimelipsDto[] = [];

  // Timeslips crear
  timeslips: TaskRegisterDto = { subjectUserId: '', projectId: '', projectName: '', subject: '', taskDescription: '',
    comment: '', assignedDate: '', assignedTo: '', time: 0 }; 

  // Timeslip para busqueda especifica
  searchTimes: SearchTimeslipDto = { subjectUserId: '', projectId: '', dateLimitDown: '', dateLimitUp: '', isAll: true };

  // Dropdown Projects
  public dropdownSettings = {};
  public dropdownSettingsFilter = {};
  public dropdownSettingsUpdate = {};

  range = new FormGroup({
    start: new FormControl<Date>(new Date()),
    end: new FormControl<Date>(new Date()),
  });
  isAllSearch: boolean = true;

  dateGlobal: any;

  //#endregion

  constructor(private authService: AuthService, 
    private projectService: ProjectsService,
    private taskService: TaskRegisterService) {}

  ngOnInit(): void {
    this.dateGlobal = new FormControl(new Date());

    // 1. Obtener data del usuario
    this.authService.userData.subscribe(response => {
      this.userData = response;
    });

    // 2. Obtener lista de proyectos
    this.projectService.GetAllProjects().subscribe(response => {
      this.project = response.content;
      this.projectFilter = response.content;
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

    this.dropdownSettingsFilter = {
      singleSelection: false,
      defaultOpen: false,
      idField: 'id',
      textField: 'projectName',
      selectAllText: 'Seleccionar todo',
      unSelectAllText: 'Deseleccionar todo',
      searchPlaceholderText: "Buscar",
      noDataAvailablePlaceholderText: "Datos no encontrados",
      itemsShowLimit: 1,
      allowSearchFilter: true,
      enableCheckAll: true
    };

    this.dropdownSettingsUpdate = {
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
    this.isAllSearch = false;
  }

  onSelectAll(){
    this.isAllSearch = true;
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
    this.table.groupHeader.toggleExpandGroup(group);
  }

  getData(){
    this.authService.userData.subscribe(res => {
      this.searchTimes = {
        subjectUserId: res.userData['sub'],
        projectId: this.isAllSearch ? '' : this.projectFilter[0].id,
        isAll: this.isAllSearch,
        dateLimitDown: this.range.get(['start'])?.value.toISOString(),
        dateLimitUp: this.range.get(['end'])?.value.toISOString().replace('T',' ').replace('Z', '')
      }

      var index = this.searchTimes.dateLimitDown.indexOf('T');
      this.searchTimes.dateLimitDown = this.searchTimes.dateLimitDown.substring(0, index).trimEnd();

      this.taskService.GetTimeslips(this.searchTimes).subscribe(res => {
        this.tasksRegister = res.content;
      })
    });
  }

  obteinData(row: any){
    this.timesDelete = row;
    
    this.updateSelectDrop = [{id:row.projectId, projectName:row.projectName}]
  }

  deleteData(subject: string){
    this.taskDelete.subject = subject;
    this.taskService.DeleteTimeslip(this.taskDelete).subscribe(response => {
      if(response.isSuccessful){
        this.tasksRegister = this.tasksRegister.filter((item: { subject: string }) => item.subject !== this.taskDelete.subject)
        Swal.fire({
          icon: 'success',
          text: response.message
        })
      }else
        Swal.fire({
          icon: 'error',
          text: response.message
        })

      this.taskDelete = { subject: '' }
    })
  }

  updateData(form: NgForm){
    
    if (form.invalid) {
      Object.values(form.controls).forEach(control => {
        control.markAsTouched();
      })
      Swal.fire({
        icon: 'info',
        text: 'Debe diligenciar los campos obligatorios marcados con un asterísco (*) en el formulario',
      })
      return;
    }
    
    if(this.timesDelete.time.includes('h') && this.timesDelete.time.includes('m') && this.timesDelete.time.includes(':')){
      
      let minutes = Number(this.timesDelete.time.substring(0, this.timesDelete.time.indexOf('h'))) * 60 
                + Number(this.timesDelete.time.substring(this.timesDelete.time.indexOf(':') + 1, this.timesDelete.time.indexOf('m')));

      this.taksUpdate = {
        //timesDelete
        subjectUserId: this.timesDelete.subjectUserId, 
        projectId: this.updateSelectDrop[0].id, 
        projectName: this.updateSelectDrop[0].projectName, 
        subject: this.timesDelete.subject, 
        taskDescription: this.timesDelete.taskDescription,
        comment: this.timesDelete.comment, 
        assignedDate: this.timesDelete.assignedDate, 
        assignedTo: this.timesDelete.assignedTo, 
        time: minutes
      }

      this.taskService.UpdateTimeslip(this.taksUpdate).subscribe(response =>{
        if(response.isSuccessful)
          Swal.fire({
            icon: 'success',
            text: response.message,
          })
        else
          Swal.fire({
            icon: 'error',
            text: response.message,
          })
      })
      
    }
    else
      Swal.fire({
        icon: 'info',
        text: 'Debe diligenciar correctamente los campos marcados con un asterísco (*) en el formulario',
      })
  }

  //#endregion

  //#region Agregar proyectos 

  createProject(isAdd: boolean){
    if(!isAdd){
      this.addProject.projectName = ''
    }
    else{
      this.projectService.CreateProject(this.addProject).subscribe(response => {
        if(response.isSuccessful)
          Swal.fire({ icon: 'success', text: response.message })
        else
          Swal.fire({ icon: 'error', text: response.message })
      })
    }
  }

  //#endregion

  //#region Funciones para agregar tareas

  InitTask(form: NgForm){
    if (form.invalid || this.projectSelectDrop.length === 0) {
      Object.values(form.controls).forEach(control => {
        control.markAsTouched();
      })
      Swal.fire({
        icon: 'info',
        text: 'Debe diligenciar los campos obligatorios marcados',
      })
      return;
    }
    this.initTimer();
  }

  initTimer(){
    this.startTime = true;
    this.pauseTime = false;
    this.interval = setInterval(() => {
      this.timerMethod();
    }, 1000);
  }

  timerMethod(){
    this.secondsTimer++;
    let minutes = Math.floor(this.secondsTimer / 60);
    let hours = Math.floor(minutes / 60);
    let extraSeconds = this.secondsTimer % 60;

    this.timer = `0${hours}h ` 
              + (minutes < 10 ? "0" + minutes : minutes) + 'm '
              + (extraSeconds < 10 ? "0" + extraSeconds : extraSeconds) + 's';
  }

  endTimer(isPause: boolean){
    this.pauseTime = true; // Para seguir en el estado dado el caso que falle el envio del timer.
    clearInterval(this.interval);

    // Creación de tarea en base de datos con el tiempo puesto
    if(!isPause)
      this.addTask(true);
  }

  addTask(isSaveTime: boolean){
    this.authService.userData.subscribe(res => {
      this.timeslips.subjectUserId = res.userData['sub'];
      this.timeslips.projectId = this.projectSelectDrop[0].id;
      this.timeslips.projectName = this.projectSelectDrop[0].projectName;
      this.timeslips.assignedDate = this.dateGlobal['value'].toISOString().replace('T',' ').replace('Z', '');
      this.timeslips.assignedTo = `${res.userData['given_name']} ${res.userData['family_name']}`;
      
      if(!isSaveTime){
        let index = this.timeslips.time.toString().indexOf('.');
        let hours = this.timeslips.time.toString().substring(0, index);
        let minutes = this.timeslips.time.toString().substring(index + 1);
        this.timeslips.time = (Number(hours) * 60) + Number(!this.timeslips.time.toString().substring(index + 1).startsWith('0') ? minutes + '0' : minutes);
      }

      this.timeslips.time = isSaveTime 
        ? Math.floor(this.secondsTimer / 60) 
        : this.timeslips.time;

      this.taskService.CreateTimeslip(this.timeslips).subscribe(create => {
        if(create.isSuccessful){
          this.startTime = false;
          this.pauseTime = false;
          this.timer = '0';
          this.secondsTimer = 0;
          this.timeslips = { subjectUserId: '', projectId: '', projectName: '', subject: '', taskDescription: '',
                              comment: '', assignedDate: '', assignedTo: '', time: 0 }; 
          this.projectSelectDrop = [];
          Swal.fire({
            icon: 'success',
            text: 'Tarea finalizada exitosamente'
          });
        }
        else
          Swal.fire({
            icon: 'error',
            text: 'Ocurrio un error en el guardado de la tarea'
          });
      });
    }); 
  }
  
  //#endregion

  downloadReport(){
    Swal.fire({
      icon: 'success',
      text: 'Reporte descargado con exito.'
    })
  }
}
