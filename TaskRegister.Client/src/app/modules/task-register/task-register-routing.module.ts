import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { CrudComponent } from "./crud/crud.component";

const routes: Routes = [
    {
      path: '',
      component: CrudComponent
    },
    {
      path: '**',
      redirectTo: '',
      pathMatch: 'full'
    }
  ];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})

export class TaskRegisterRoutingModule {}