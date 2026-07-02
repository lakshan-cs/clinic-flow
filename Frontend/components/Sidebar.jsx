import React from 'react';
import { useRouter } from 'next/router';
import { LayoutDashboard, Users, Building2, Stethoscope, Leaf, CalendarDays, LogOut } from 'lucide-react';
import styles from './Sidebar.module.css';

const Sidebar = ({ isOpen, activePage, onLogout }) => {
  const router = useRouter();

  const navItems = [
    { id: 'dashboard',   name: 'Dashboard',          icon: LayoutDashboard, path: '/dashboard' },
    { id: 'patients',    name: 'Patients',            icon: Users,           path: '/patients' },
    { id: 'clinics',     name: 'Clinics',             icon: Building2,       path: '/clinics' },
    { id: 'providers',   name: 'Healthcare Providers',icon: Stethoscope,     path: '/providers' },
    { id: 'allergies',   name: 'Allergies',           icon: Leaf,            path: '/allergies' },
    { id: 'appointments',name: 'Appointments',        icon: CalendarDays,    path: '/appointments' },
  ];

  return (
    <aside className={`${styles.sidebar} ${!isOpen ? styles.sidebarClosed : ''}`}>
      <div className={styles.sidebarHeader}>
        <h2 className={styles.logo}>ClinicFlow</h2>
      </div>
      
      <nav className={styles.nav}>
        {navItems.map((item) => {
          const Icon = item.icon;
          return (
            <button
              key={item.id}
              onClick={() => router.push(item.path)}
              className={`${styles.navItem} ${activePage === item.id ? styles.navItemActive : ''}`}
            >
              <Icon size={20} />
              <span className={styles.navText}>{item.name}</span>
            </button>
          );
        })}
      </nav>

      <button className={styles.logoutBtn} onClick={onLogout}>
        <LogOut size={20} />
        <span className={styles.navText}>Logout</span>
      </button>
    </aside>
  );
};

export default Sidebar;
