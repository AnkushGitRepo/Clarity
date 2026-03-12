import { Injectable, signal } from '@angular/core';

@Injectable({
    providedIn: 'root'
})
export class ThemeService {
    isDarkMode = signal<boolean>(true);

    constructor() {
        this.initTheme();
    }

    private initTheme() {
        const savedTheme = localStorage.getItem('theme');
        if (savedTheme === 'light') {
            this.isDarkMode.set(false);
            document.documentElement.setAttribute('data-theme', 'light');
        } else {
            this.isDarkMode.set(true);
            document.documentElement.removeAttribute('data-theme');
        }
    }

    toggleTheme() {
        const newMode = !this.isDarkMode();
        this.isDarkMode.set(newMode);

        if (newMode) {
            document.documentElement.removeAttribute('data-theme');
            localStorage.setItem('theme', 'dark');
        } else {
            document.documentElement.setAttribute('data-theme', 'light');
            localStorage.setItem('theme', 'light');
        }
    }
}
