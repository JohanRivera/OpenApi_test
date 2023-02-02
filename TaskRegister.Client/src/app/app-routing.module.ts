import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { CrudComponent } from "./modules/task-register/crud/crud.component";

const routes: Routes = [
    {
        path: "",
        component: CrudComponent,
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