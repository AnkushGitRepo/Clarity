import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';

@Component({
    selector: 'app-signup',
    standalone: true,
    imports: [CommonModule, FormsModule, RouterLink],
    templateUrl: './signup.component.html',
    styleUrls: ['./signup.component.css']
})
export class SignupComponent {
    authService = inject(AuthService);
    router = inject(Router);

    username = '';
    email = '';
    password = '';
    errorMessage = '';

    onSubmit() {
        this.authService.register({ username: this.username, email: this.email, password: this.password }).subscribe({
            next: () => {
                // Log user in automatically after register
                this.authService.login({ email: this.email, password: this.password }).subscribe(() => {
                    this.router.navigate(['/dashboard']);
                });
            },
            error: (err) => {
                this.errorMessage = err.error?.message || 'Registration failed';
            }
        });
    }
}
