import {BrowserModule} from '@angular/platform-browser';
import {NgModule} from '@angular/core';

import {AppRoutingModule} from './app-routing.module';
import {AppComponent} from './app.component';
import {BrowserAnimationsModule} from "@angular/platform-browser/animations";
import {MatMenuModule} from '@angular/material/menu';
import { MatButtonModule} from '@angular/material/button'
import {MatIconModule} from '@angular/material/icon';
import {MatCardModule} from '@angular/material/card';
import {MatTabsModule} from '@angular/material/tabs';
import { MatDatepickerModule } from "@angular/material/datepicker";
import {  MatDialogModule } from "@angular/material/dialog";
import {  MatInputModule } from "@angular/material/input";
import {  MatListModule } from "@angular/material/list";
import {  MatPaginatorModule } from "@angular/material/paginator";
import {  MatProgressSpinnerModule } from "@angular/material/progress-spinner";
import {  MatSelectModule } from "@angular/material/select";
import { MatSidenavModule } from "@angular/material/sidenav";
import { MatSortModule } from "@angular/material/sort";
import {  MatTableModule } from "@angular/material/table";
import { MatToolbarModule } from "@angular/material/toolbar";
import { provideHttpClient, withInterceptorsFromDi } from "@angular/common/http";
import { ReactiveFormsModule} from "@angular/forms";
import {MatStepperModule} from '@angular/material/stepper';
import { MatCheckboxModule} from '@angular/material/checkbox';
import { MatRadioModule} from '@angular/material/radio';
import {MatNativeDateModule} from '@angular/material/core';
import { MatSliderModule} from '@angular/material/slider';
import { MatTooltipModule} from '@angular/material/tooltip';
import {DragDropModule} from '@angular/cdk/drag-drop';

import {MatGridListModule} from '@angular/material/grid-list';
import {MatTreeModule} from '@angular/material/tree';
import {ScrollingModule} from '@angular/cdk/scrolling';
import { HeaderComponent } from './layout/header/header.component';
import { FooterComponent } from './layout/footer/footer.component';
import { CsvImportComponent } from './pages/csv-import/csv-import.component';
import { EmployeeProjectsComponent } from './pages/employee-projects/employee-projects.component';

@NgModule({ declarations: [
        AppComponent,
        HeaderComponent,
        FooterComponent,
        CsvImportComponent,
        EmployeeProjectsComponent,
    ],
    bootstrap: [AppComponent], imports: [BrowserModule,
        BrowserAnimationsModule,
        MatMenuModule,
        MatButtonModule,
        MatIconModule,
        MatCardModule,
        MatTabsModule,
        MatSidenavModule,
        MatListModule,
        MatToolbarModule,
        MatTooltipModule,
        MatInputModule,
        MatTableModule,
        MatPaginatorModule,
        MatSortModule,
        MatProgressSpinnerModule,
        MatStepperModule,
        MatDialogModule,
        AppRoutingModule,
        MatSelectModule,
        MatCheckboxModule,
        DragDropModule,
        MatRadioModule,
        MatSliderModule,
        MatDatepickerModule,
        MatNativeDateModule,
        ReactiveFormsModule,
        MatGridListModule,
        MatTreeModule,
        ScrollingModule], providers: [
        provideHttpClient(withInterceptorsFromDi())
    ] })
export class AppModule {
}
