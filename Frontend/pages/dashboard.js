import React, { useState, useEffect } from 'react';
import { useRouter } from 'next/router';
import { toast } from 'react-toastify';
import Sidebar from '../components/Sidebar';
import OverviewCards from '../components/OverviewCards';
import Charts from '../components/Charts';
import { getCurrentUser, removeUser } from '../services/authService';
import { getAllPatients } from '../services/patientService';
import { getAllClinics } from '../services/clinicService';
import { getAllProviders } from '../services/providerService';
import { getAllAppointments } from '../services/appointmentService';
import { useMemo } from 'react';
import styles from '../styles/Dashboard.module.css';

export default function Dashboard() {
  const router = useRouter();
  const [isSidebarOpen, setIsSidebarOpen] = useState(true);
  const [activePage, setActivePage] = useState('dashboard');
  const [currentUser] = useState(() => getCurrentUser());

  const [stats, setStats] = useState({ totalPatients: 0, totalClinics: 0, totalProviders: 0, totalAppointments: 0 });
  const [chartData, setChartData] = useState({ appointments: [], clinics: [], providers: [] });
  const [patients, setPatients] = useState([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    fetchStats();
  }, []);

  const fetchStats = async () => {
    const user = getCurrentUser();

    if (!user) {
      router.push('/login');
      return;
    }

    try {
      setIsLoading(true);
      setError(null);
      const [patients, clinics, providers, appointments] = await Promise.all([
        getAllPatients(),
        getAllClinics(),
        getAllProviders(),
        getAllAppointments(),
      ]);
      setStats({
        totalPatients: patients.length,
        totalClinics: clinics.length,
        totalProviders: providers.length,
        totalAppointments: appointments.length,
      });
      setChartData({ appointments, clinics, providers });
      setPatients(patients);
    } catch (err) {
      console.error('Error fetching dashboard stats:', err);
      setError(err.response?.data?.message || 'Failed to fetch dashboard data');
    } finally {
      setIsLoading(false);
    }
  };

  const todaysAppointments = useMemo(() => {
    const today = new Date();
    const patientMap = {};
    patients.forEach(p => { patientMap[p.id] = p.fullName || p.FullName || `Patient ${p.id}`; });
    const clinicMap = {};
    chartData.clinics.forEach(c => { clinicMap[c.id] = c.name || c.Name || `Clinic ${c.id}`; });
    const providerMap = {};
    chartData.providers.forEach(p => { providerMap[p.id] = p.name || p.Name || `Provider ${p.id}`; });
    return chartData.appointments
      .filter(a => {
        const d = new Date(a.dateTime || a.DateTime);
        return d.getFullYear() === today.getFullYear() &&
          d.getMonth() === today.getMonth() &&
          d.getDate() === today.getDate();
      })
      .sort((a, b) => new Date(a.dateTime || a.DateTime) - new Date(b.dateTime || b.DateTime))
      .map(a => ({
        id: a.id || a.Id,
        time: new Date(a.dateTime || a.DateTime).toLocaleTimeString('en-US', { hour: '2-digit', minute: '2-digit' }),
        patient: patientMap[a.patientId || a.PatientId] || `Patient ${a.patientId || a.PatientId}`,
        clinic: clinicMap[a.clinicId || a.ClinicId] || `Clinic ${a.clinicId || a.ClinicId}`,
        provider: providerMap[a.providerId || a.ProviderId] || `Provider ${a.providerId || a.ProviderId}`,
        reason: a.reason || a.Reason || '—',
      }));
  }, [chartData, patients]);

  const handleLogout = () => {
    removeUser();
    toast.success('Logged out successfully!');
    router.push('/login');
  };

  return (
    <div className={styles.dashboardContainer}>
      <Sidebar 
        isOpen={isSidebarOpen}
        activePage={activePage}
        onLogout={handleLogout}
      />

      <main className={`${styles.main} ${!isSidebarOpen ? styles.mainExpanded : ''}`}>
        <header className={styles.header}>
          <div className={styles.headerLeft}>
            <button 
              onClick={() => setIsSidebarOpen(!isSidebarOpen)}
              className={styles.menuBtn}
            >
              <div className={styles.hamburgerIcon}>  
                <span></span>
                <span></span>
                <span></span>
              </div>  
            </button>
            <h1 className={styles.pageTitle}>Dashboard</h1>
          </div>
          
          <div className={styles.headerRight}>
            <div className={styles.profileArea}>
              <div className={styles.profileAvatar}>
                {currentUser?.username?.charAt(0)?.toUpperCase() || 'A'}
              </div>
              <div className={styles.profileInfo}>
                <span className={styles.profileName}>Admin</span>
                <span className={styles.profileRole}>Administrator</span>
              </div>
            </div>
          </div>
        </header>

        <div className={styles.content}>
          <OverviewCards
            totalPatients={stats.totalPatients}
            totalClinics={stats.totalClinics}
            totalProviders={stats.totalProviders}
            totalAppointments={stats.totalAppointments}
            isLoading={isLoading}
          />

          <Charts
            appointments={chartData.appointments}
            clinics={chartData.clinics}
            providers={chartData.providers}
            isLoading={isLoading}
          />

          <div className={styles.tableCard}>
            <h3 className={styles.tableTitle}>Today's Appointments</h3>
            {isLoading ? (
              <p className={styles.tableEmpty}>Loading...</p>
            ) : todaysAppointments.length === 0 ? (
              <p className={styles.tableEmpty}>No appointments scheduled for today.</p>
            ) : (
              <div className={styles.tableWrapper}>
                <table className={styles.table}>
                  <thead>
                    <tr>
                      <th>Time</th>
                      <th>Patient</th>
                      <th>Clinic</th>
                      <th>Provider</th>
                      <th>Reason</th>
                    </tr>
                  </thead>
                  <tbody>
                    {todaysAppointments.map(appt => (
                      <tr key={appt.id}>
                        <td>{appt.time}</td>
                        <td>{appt.patient}</td>
                        <td>{appt.clinic}</td>
                        <td>{appt.provider}</td>
                        <td>{appt.reason}</td>
                      </tr>
                    ))}
                  </tbody>
                </table>
              </div>
            )}
          </div>

          {error && (
            <div className={styles.errorContainer}>
              <p className={styles.errorMessage}>{error}</p>
              <button onClick={fetchStats} className={styles.retryBtn}>
                Retry
              </button>
            </div>
          )}
        </div>
      </main>
    </div>
  );
}
