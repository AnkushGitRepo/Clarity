# Frontend Architecture & Workspace Guide
Welcome to the Clarity Frontend! This is a modern Single Page Application (SPA) built with **Angular 17+** using **Standalone Components** and native CSS variables for dynamic theming.

This document explains the overarching workspace structure and build configurations.

## Workspace Structure

```text
frontend/
├── angular.json       # The Master Configuration File
├── Dockerfile         # Nginx Deployment Blueprint
├── package.json       # NPM Dependencies & Scripts
├── nginx.conf         # Web Server Routing Rules
└── src/               # Application Source Code
```

---

## 1. `angular.json`
This is the command center for the entire Angular CLI. It defines how the application is compiled, served, and tested.
*   **Builder Engine:** By default, it uses the modern `@angular/build:application` which leverages Esbuild for lightning-fast compilation compared to older Webpack builds.
*   **Assets Array:** Dictates which static folders (like `public/` and `src/assets/`) should be physically copied into the final production deployment. This is how our `logo.png` makes it to the live server!
*   **Styles Layout:** Defines `src/styles.css` as the global stylesheet injected into the root HTML before any component loads.

## 2. Component Architecture (`src/app/`)
We strictly follow a modular, scalable folder convention to keep the codebase clean:

*   **`core/`**: Contains singleton services (HTTP calls, Timers), data models, and interceptors (JWT injection). These are instantiated ONCE when the app loads.
*   **`shared/`**: Presentational "dumb" components (Navbar, Footer, Habit Cards). They don't fetch their own data; they rely on `@Input()` and `@Output()` to communicate with the features that host them.
*   **`features/`**: The distinct "pages" of the application (Dashboard, Tasks, Pomodoro, Analytics). These are "smart" components that inject Core services, fetch data, and arrange Shared components.

## 3. Styling & Theming (`src/styles.css`)
We use **Vanilla CSS with CSS Variables (`--var`)** to manage our Design System. 
By defining variables on the `:root` pseudo-selector, we establish a Dark Mode default. The light mode is dynamically achieved by the `ThemeService` attaching a `[data-theme='light']` attribute to the `<body>` tag, which instantly overrides the global variables and cascades new colors to every component in the DOM without writing duplicate CSS!

## 4. `nginx.conf` & `Dockerfile`
Since Angular is a Single Page Application, it only consists of one actual HTML file (`index.html`). 
*   If a user directly navigates to `clarity.com/tasks`, the browser asks the server for a folder named `/tasks`, which doesn't exist!
*   The `nginx.conf` contains a crucial `try_files $uri $uri/ /index.html;` directive. It tells Nginx: "If you can't find the file the user asked for, just serve `index.html` and let Angular's internal JavaScript router handle showing the correct component."
*   The `Dockerfile` handles a multi-stage build: compiling the Typescript down to raw JS/HTML, then copying only those lightweight output files into a hardened Nginx alpine image for serving.
