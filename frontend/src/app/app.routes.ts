import { Routes } from '@angular/router';
import { LoginComponent } from './features/auth/login/login.component';
import { SignupComponent } from './features/auth/signup/signup.component';
import { authGuard } from './core/guards/auth.guard';

export const routes: Routes = [
    { path: '', redirectTo: '/login', pathMatch: 'full' },
    { path: 'login', component: LoginComponent },
    { path: 'signup', component: SignupComponent },
    {
        path: 'dashboard',
        loadComponent: () => import('./features/dashboard/dashboard.component').then(m => m.DashboardComponent),
        canActivate: [authGuard]
    },
    {
        path: 'pomodoro',
        loadComponent: () => import('./features/pomodoro-page/pomodoro-page').then(m => m.PomodoroPage),
        canActivate: [authGuard]
    },    
    {
        path: 'analytics',
        loadComponent: () => import('./features/analytics/analytics.component').then(m => m.AnalyticsComponent),
        canActivate: [authGuard]
    },
    {
        path: 'tasks',
        loadComponent: () => import('./features/tasks-page/tasks-page').then(m => m.TasksPageComponent),
        canActivate: [authGuard]
    },    
    { path: '**', redirectTo: '/login' }
];
