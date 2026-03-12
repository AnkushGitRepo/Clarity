# ✨ Clarity - The Ultimate Habit Tracker 🚀

Welcome to **Clarity**, a beautiful, modern, and open-source habit tracking experience built with cutting-edge technologies. Clarity is designed to help you stay focused, measure your progress deeply across multiple views, and seamlessly break down monumental goals into daily milestones.

![Clarity Preview](https://i.imgur.com/your-preview-image.png) *(Imagine a beautiful UI screenshot here)*

## 🌟 Features

*   🎯 **Intelligent Habit Management:** Create, color-code, and track repeating daily habits with ease.
*   🔥 **Streak Tracking:** Computes current consecutive days and all-time longest streaks automatically to keep you motivated.
*   📊 **Deep Work Analytics:** Visualize your consistency through GitHub-style heatmap contribution graphs.
*   🍅 **Integrated Pomodoro & Stopwatch:** Uncluttered focus! Includes both traditional 25/5 Pomodoro flows or open-ended Stopwatch logs that map specific times to your distinct habits.
*   📋 **Hierarchical Kanban Tasks:** A Trello-style Drag-and-Drop board segmenting Monthly Vision, Weekly Goals, and Daily Execution. Create Parent-Child nested task relationships easily!
*   🌓 **Dark / Light Mode & Theming:** Crisp styling mapped gracefully via CSS variables.

## 🛠️ Technology Stack

Clarity is built with a decoupled, containerized Microservices architecture:

*   **Frontend:** Angular 17+ (Standalone Components), HTML5, Vanilla CSS System, `@angular/cdk/drag-drop`
*   **Backend:** ASP.NET Core 8 Web API (C#), JWT Authentication, BCrypt Hashing, MVC Pattern
*   **Database:** MongoDB
*   **Deployment:** Docker & `docker-compose`

## 🚀 Setup & Installation (Dockerized)

Running Clarity is as easy as breathing, thanks to fully configured Docker composition.

**Prerequisites:** 
* Install [Docker](https://docs.docker.com/get-docker/) & `docker-compose`.

```bash
# 1. Clone the repository
git clone git@github.com:AnkushGitRepo/Clarity.git
cd Clarity

# 2. Spin up the containers (MongoDB, .NET Backend, Angular Frontend)
docker-compose up -d --build

# 3. Access the web app
Open http://localhost:4200 in your browser!
```

*Note: The backend API runs implicitly inside the docker-network on port `5239`, communicating securely with the Frontend Nginx proxy.*

## 🙋‍♂️ About the Developer

Built with passion for productivity. 

🔗 **GitHub:** [AnkushGitRepo](https://github.com/AnkushGitRepo)
💼 **Portfolio:** [ankushg.vercel.app](https://ankushg.vercel.app/)
👔 **LinkedIn:** [Ankush Gupta](https://www.linkedin.com/in/ankushgupta18/)
📸 **Instagram:** [@_ankushg](https://www.instagram.com/_ankushg/)

---
*Happy tracking! Stay focused, stay consistent.* 📈
