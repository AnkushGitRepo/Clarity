# Shared Module (`src/app/shared/`)

The Shared folder contains highly reusable UI components that are entirely "dumb". They do not fetch data from the backend; they only know how to look pretty and rely on inputs and outputs to function.

By keeping these components isolated, we enforce UI consistency across the entire app and make maintenance a breeze.

## 1. `components/navbar/`
*   **Role:** The global top navigation header.
*   **Logic:** It subscribes to the `AuthService.isAuthenticated$` stream to automatically hide itself on the `/login` page and show itself on the `/dashboard`. 
*   **Theming:** Houses the main toggle button that triggers the `ThemeService` to switch between Dark and Light mode.

## 2. `components/footer/`
*   **Role:** The global pinned footer.
*   **Logic:** Utilizes CSS Flexbox to anchor itself to the bottom of the `main-content` viewport. It handles strict external routing (`target="_blank" rel="noopener noreferrer"`) for security when linking to social profiles and portfolios.

## 3. `components/habit-card/`
*   **Role:** A highly modular component designed strictly to display a single habit.
*   **Inputs:** Accepts a `@Input() habit` object from the parent Dashboard. By dynamically accessing properties like `habit.color`, it binds custom CSS attributes directly to the HTML template (e.g., `[style.backgroundColor]="habit.color"`).
*   **Outputs:** When a user clicks "Complete Habit", the card uses `EventEmitter` (`@Output()`) to shout "Hey parent! I was clicked!" This ensures the Dashboard handles the actual database request, keeping the Habit Card perfectly reusable anywhere in the app without tying it to the `HabitService`.

## 4. `components/pomodoro/`
*   **Role:** The circular timer display component.
*   **Logic:** While the `pomodoro-page` manages the API logging, this shared component manages the aesthetic visualization of the ticking clock, allowing it to easily be embedded in a sidebar or widget down the line simply by importing the selector `<app-pomodoro>`.
