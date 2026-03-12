# Core Module (`src/app/core/`)

The Core module acts as the central nervous system of the Angular application. It houses singleton services, route guards, and HTTP interceptors that enforce application-wide logic. 

**Rule of Thumb:** If it manages state, talks to the backend API, or protects routes, it belongs here.

## 1. Services (`core/services/`)
Services manage data retrieval and state application-wide. They are injected into components using Angular's Dependency Injection (`inject()`).

*   **`auth.service.ts`**: Manages the user session. It sends login/signup credentials to the backend, stores the returned JWT token securely in LocalStorage, and exposes `Observable` streams (`isAuthenticated$`, `currentUser$`) so the Navbar can instantly react when a user logs in or out.
*   **`habit.service.ts` & `task.service.ts`**: The data brokers. They format HTTP GET/POST/PUT/DELETE requests using Angular's `HttpClient` to communicate with our .NET Controllers. They define TypeScript `Interfaces` (like `TaskItem`) to ensure we have strict strong-typing of the data we receive.
*   **`theme.service.ts`**: Listens to the user's OS color scheme preferences (`prefers-color-scheme: dark`) and manages the toggle switch in the navbar, persisting the user's manual choice to LocalStorage.
*   **`timer.service.ts`**: Powers the Pomodoro clock. It maintains the countdown interval globally. This means if a user starts the Pomodoro timer and navigates to the Analytics page, the timer continues ticking in the background without resetting, because the Service exists outside the component lifecycle!

## 2. Guards (`core/guards/`)
*   **`auth.guard.ts`**: An Angular Router Guard function. Before the router navigates to protected pages like `/dashboard` or `/tasks`, this guard checks the `AuthService` to verify if a valid JWT token exists. If not, it forcefully redirects the user back to the `/login` page to ensure unauthenticated users cannot view the app internals.

## 3. Interceptors (`core/interceptors/`)
*   **`jwt.interceptor.ts`**: A middleware function that intercepts *every single outgoing HTTP request* from the Angular application. It seamlessly reads the JWT token from LocalStorage and attaches it to the HTTP Headers (`Authorization: Bearer <token>`). This is why our individual Services don't need to manually handle tokens on every API call—the interceptor automates the authentication handshake with the .NET backend globally.
