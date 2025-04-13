// src/app/app-routing.module.ts
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CsvImportComponent } from './pages/csv-import/csv-import.component';
import { EmployeeProjectsComponent } from './pages/employee-projects/employee-projects.component';

const routes: Routes = [
  { path: 'csv-import', component: CsvImportComponent },
  { path: 'employee-projects', component: EmployeeProjectsComponent },
  { path: '', redirectTo: 'employee-projects', pathMatch: 'full' },
  { path: '**', redirectTo: 'employee-projects' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {}
