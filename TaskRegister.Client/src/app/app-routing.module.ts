import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { AuthGuard } from "./core/guards/auth.guard";
import { LayoutAuthComponent } from "./layouts/layout-auth/layout-auth.component";
import { LayoutComponent } from "./layouts/layout/layout.component";

const routes: Routes = [
    {
        path: "login",
        component: LayoutAuthComponent,
        children: [
            {
                path: "",
                loadChildren: () => import("./modules/auth/authentication.module").then((m) => m.AuthenticationModule)
            }
        ]
    },
    {
        path: "",
        canActivate: [AuthGuard],
        component: LayoutComponent,
        children: [
            {
                path: "",
                loadChildren: () => import("./modules/task-register/task-register.module").then((m) => m.TaskRegisterModule)
            }
        ]
    },
    {   path: "**", redirectTo: ""  }
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
})

export class AppRoutingModule { }