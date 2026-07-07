import React from 'react';
import { Users, Building2, Stethoscope, CalendarCheck } from 'lucide-react';
import styles from './OverviewCards.module.css';

const OverviewCards = ({ totalPatients = 0, totalClinics = 0, totalProviders = 0, totalAppointments = 0, isLoading = false }) => {
  const cardsData = [
    {
      id: 1,
      title: 'Total Patients',
      value: totalPatients,
      trend: 'Registered patients',
      icon: Users,
      colorClass: styles.cardBlue
    },
    {
      id: 2,
      title: 'Total Clinics',
      value: totalClinics,
      trend: 'Active clinics',
      icon: Building2,
      colorClass: styles.cardGreen
    },
    {
      id: 3,
      title: 'Total Providers',
      value: totalProviders,
      trend: 'Healthcare providers',
      icon: Stethoscope,
      colorClass: styles.cardOrange
    },
    {
      id: 4,
      title: 'Total Appointments',
      value: totalAppointments,
      trend: 'Scheduled appointments',
      icon: CalendarCheck,
      colorClass: styles.cardPurple
    }
  ];

  return (
    <div className={styles.cardsGrid}>
      {cardsData.map((card) => {
        const Icon = card.icon;
        return (
          <div key={card.id} className={`${styles.card} ${card.colorClass}`}>
            <div className={styles.cardIcon}>
              <Icon size={28} />
            </div>
            <div className={styles.cardContent}>
              <p className={styles.cardLabel}>{card.title}</p>
              <h3 className={styles.cardValue}>
                {isLoading ? '...' : card.value}
              </h3>
              <p className={styles.cardTrend}>{card.trend}</p>
            </div>
          </div>
        );
      })}
    </div>
  );
};

export default OverviewCards;
