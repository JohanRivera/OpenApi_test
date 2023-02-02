import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { UserRegister } from 'src/app/core/models/user.model';
import { AuthService } from 'src/app/core/services/auth.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit{

  public user: UserRegister = { userName: '', password: ''};
  public fieldTextType: boolean = false;

  constructor(private authService: AuthService, private router: Router) {}

  ngOnInit(): void {}

  // Metodo que valida el form del login
  onSubmit(form: NgForm)
  {
    if (form.invalid) {
      return;
    }
    
    this.register(this.user); // Llamado de consumo de método de logeo
  }

  // Metodo para autenticar al usuario
  register(data: UserRegister)
  {
    // this.authService.doLogin(data).subscribe(response => {
    //   if(response.accessToken !== '')
    //     this.router.navigate(['/'])
      
    //   else
    //     Swal.fire({
    //       icon: 'info',
    //       text: 'Usuario y/o contraseña invalida',
    //     })
    // },(error) =>{
    //   Swal.fire({
    //     icon: 'error',
    //     text: 'Ha ocurrido un error al iniciar sesión',
    //   })
    // });
  }

  login(){
    return this.authService.doLogin();
  }
}
