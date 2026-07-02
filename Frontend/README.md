# TaskFlow - Task Management Application

A modern task management application built with Next.js, featuring task tracking, analytics, and user profiles.

## Features

- 📝 Create and manage tasks
- 📊 Dashboard with analytics and charts
- ✅ Track task completion
- 🔍 Search and filter tasks
- 👤 User profile management
- 🎨 Clean, responsive UI

## Tech Stack

- **Framework**: Next.js 14
- **UI**: React 18
- **Styling**: CSS Modules
- **HTTP Client**: Axios
- **Charts**: Recharts
- **Notifications**: React Toastify
- **Icons**: Lucide React

## Getting Started

### Prerequisites

- Node.js 18+ installed
- npm or yarn package manager

### Installation

```bash
# Install dependencies
npm install

# Run development server
npm run dev
```

Open [http://localhost:3000](http://localhost:3000) to view the application.

### Environment Variables

Create a `.env.local` file in the root directory:

```env
NEXT_PUBLIC_API_URL=http://44.217.52.15:8080
```

## Project Structure

```
Frontend/
├── pages/              # Next.js pages (file-based routing)
├── components/         # Reusable React components
├── services/          # API service layer
├── styles/            # CSS Modules
└── public/            # Static assets
```

## Available Scripts

- `npm run dev` - Start development server
- `npm run build` - Build for production
- `npm start` - Start production server
- `npm run lint` - Run ESLint

## Features Overview

### Authentication
- User login and signup
- Session management with localStorage

### Task Management
- Create, read, update, delete tasks
- Set priority levels (High, Medium, Low)
- Set due dates
- Mark tasks as completed

### Dashboard
- Overview cards with task statistics
- Daily completion trend chart
- Task distribution pie chart

### Filtering & Search
- Filter by priority
- Filter by date (Overdue, Today, Upcoming)
- Search tasks by title or description

## License

This project is private and proprietary.

## Expanding the ESLint configuration

If you are developing a production application, we recommend using TypeScript with type-aware lint rules enabled. Check out the [TS template](https://github.com/vitejs/vite/tree/main/packages/create-vite/template-react-ts) for information on how to integrate TypeScript and [`typescript-eslint`](https://typescript-eslint.io) in your project.
