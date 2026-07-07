import React, { useMemo } from 'react';
import {
  PieChart, Pie, Cell, Tooltip, Legend, ResponsiveContainer,
  BarChart, Bar, XAxis, YAxis, CartesianGrid
} from 'recharts';
import styles from './Charts.module.css';

const PIE_COLORS = ['#3b82f6', '#10b981', '#f59e0b', '#ef4444', '#8b5cf6', '#06b6d4', '#f97316', '#84cc16'];

const Charts = ({ appointments = [], clinics = [], providers = [], isLoading = false }) => {
  const pieData = useMemo(() => {
    const clinicMap = {};
    clinics.forEach(c => { clinicMap[c.id] = c.name || `Clinic ${c.id}`; });
    const counts = {};
    appointments.forEach(a => {
      const name = clinicMap[a.clinicId] || `Clinic ${a.clinicId}`;
      counts[name] = (counts[name] || 0) + 1;
    });
    return Object.entries(counts).map(([name, value]) => ({ name, value }));
  }, [appointments, clinics]);

  const barData = useMemo(() => {
    const providerMap = {};
    providers.forEach(p => { providerMap[p.id] = p.name || `Provider ${p.id}`; });
    const counts = {};
    appointments.forEach(a => {
      const name = providerMap[a.providerId] || `Provider ${a.providerId}`;
      counts[name] = (counts[name] || 0) + 1;
    });
    return Object.entries(counts).map(([name, appointments]) => ({ name, appointments }));
  }, [appointments, providers]);

  const loadingPlaceholder = (title) => (
    <div className={styles.chartCard}>
      <h3 className={styles.chartTitle}>{title}</h3>
      <div style={{ display: 'flex', justifyContent: 'center', alignItems: 'center', height: '300px' }}>
        <p>Loading chart data...</p>
      </div>
    </div>
  );

  const emptyPlaceholder = (msg) => (
    <div style={{ display: 'flex', justifyContent: 'center', alignItems: 'center', height: '300px', color: '#6b7280' }}>
      <p>{msg}</p>
    </div>
  );

  if (isLoading) {
    return (
      <div className={styles.chartsGrid}>
        {loadingPlaceholder('Appointments by Clinic')}
        {loadingPlaceholder('Provider Workload')}
      </div>
    );
  }

  return (
    <div className={styles.chartsGrid}>
      <div className={styles.chartCard}>
        <h3 className={styles.chartTitle}>Appointments by Clinic</h3>
        {pieData.length > 0 ? (
          <ResponsiveContainer width="100%" height={300}>
            <PieChart>
              <Pie
                data={pieData}
                cx="50%"
                cy="50%"
                outerRadius={100}
                dataKey="value"
                label={({ name, percent }) => `${name}: ${(percent * 100).toFixed(0)}%`}
                labelLine
              >
                {pieData.map((_, index) => (
                  <Cell key={`cell-${index}`} fill={PIE_COLORS[index % PIE_COLORS.length]} />
                ))}
              </Pie>
              <Tooltip formatter={(value) => [value, 'Appointments']} />
              <Legend />
            </PieChart>
          </ResponsiveContainer>
        ) : emptyPlaceholder('No appointment data available')}
      </div>

      <div className={styles.chartCard}>
        <h3 className={styles.chartTitle}>Provider Workload</h3>
        {barData.length > 0 ? (
          <ResponsiveContainer width="100%" height={300}>
            <BarChart data={barData} margin={{ top: 5, right: 20, left: 0, bottom: 60 }}>
              <CartesianGrid strokeDasharray="3 3" stroke="#e5e7eb" />
              <XAxis dataKey="name" stroke="#6b7280" angle={-35} textAnchor="end" interval={0} tick={{ fontSize: 12 }} />
              <YAxis stroke="#6b7280" allowDecimals={false} />
              <Tooltip formatter={(value) => [value, 'Appointments']} />
              <Bar dataKey="appointments" fill="#6366f1" radius={[4, 4, 0, 0]} />
            </BarChart>
          </ResponsiveContainer>
        ) : emptyPlaceholder('No provider data available')}
      </div>
    </div>
  );
};

export default Charts;
