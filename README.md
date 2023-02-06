# Open API Test

API con ASP.NET Core 6 protegida por un Identity Provider OIDC, realizado con ASP.NET Core 6, el cual se encarga de administrar la autenticación para un cliente, en este caso Angular versión 15.1.2, al cual se implementando OAuth2 y Open ID Connect (OIDC) y un consumo hacía el Identity Provider y el Servidor.

Caracteristicas:

    1. Open Api de registro de tareas diarias en NET6
    2. Identity Provider OIDC ASP.NET Core 6 - OpenApi 
    3. Front para registro de tareas diarias en Angular versión 15.1.2

## Instalación

Clone el repositorio, dirigete a la ruta del proyecto IdentityProvider `/TaskRegister.IDP` y ejecute el comando

```
dotnet run
```

Esto anterior realizarlo para el API que se encuentra en la ruta `/TaskRegister.API`. 

Luego dirijase al FrontEnd que se encuentra en la ruta `/TaskRegister.Client` y ejecute para instalar las dependencias del proyecto

```
npm install
```

y luego 

```
ng serve
```

Entonces se puede abrir localhost:4200 en el navegador.