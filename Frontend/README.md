# ClinicFlow - Frontend

A clinic management web application built with Next.js for managing patients, appointments, providers, clinics, and allergies.

## Tech Stack

- **Framework**: Next.js
- **UI**: React
- **Styling**: CSS Modules
- **HTTP Client**: Axios
- **Charts**: Recharts
- **Notifications**: React Toastify
- **Icons**: Lucide React

## Getting Started

### Prerequisites

- Node.js 18+ installed
- npm package manager

### Installation

```bash
# Install dependencies
npm install

# Run development server
npm run dev
```

Open [http://localhost:3000](http://localhost:3000) to view the application.

### Environment Variables

Create a `.env.local` file in the `Frontend/` directory:

```env
NEXT_PUBLIC_API_URL=http://localhost:5000
```

## Project Structure

```
Frontend/
├── pages/              # Next.js pages (file-based routing)
├── components/         # Reusable React components
├── services/           # API service layer
├── styles/             # CSS Modules
└── public/             # Static assets
```

## Available Scripts

- `npm run dev` - Start development server
- `npm run build` - Build for production
- `npm start` - Start production server

## Pages

- `/login` - User authentication
- `/dashboard` - Overview with charts and statistics
- `/patients` - Manage patient records
- `/appointments` - View and schedule appointments
- `/providers` - Manage healthcare providers
- `/clinics` - Manage clinic locations
- `/allergies` - Manage allergy types

## License

This project is private and proprietary.
