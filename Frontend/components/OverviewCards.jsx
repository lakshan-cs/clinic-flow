import React from 'react';
import { Building2, Stethoscope, CalendarCheck } from 'lucide-react';
import styles from './OverviewCards.module.css';

const OverviewCards = ({ mostActiveClinic = { name: 'N/A', count: 0 }, busiestProvider = { name: 'N/A', count: 0 }, upcomingAppointments = 0, totalAppointments = 0, isLoading = false }) => {
  const cardsData = [
    {
      id: 1,
      title: 'Most Active Clinic',
      value: mostActiveClinic.name,
      trend: `${mostActiveClinic.count} appointment${mostActiveClinic.count !== 1 ? 's' : ''}`,
      icon: Building2,
      colorClass: styles.cardBlue,
      smallValue: true
    },
    {
      id: 2,
      title: 'Busiest Provider',
      value: busiestProvider.name,
      trend: `${busiestProvider.count} appointment${busiestProvider.count !== 1 ? 's' : ''}`,
      icon: Stethoscope,
      colorClass: styles.cardGreen,
      smallValue: true
    },
    {
      id: 3,
      title: 'Upcoming Appointments',
      value: upcomingAppointments,
      trend: 'Next 7 days',
      icon: CalendarCheck,
      colorClass: styles.cardOrange
    },
    {
      id: 4,
      title: 'Total Appointments',
      value: totalAppointments,
      trend: 'All scheduled appointments',
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
              <h3 className={card.smallValue ? styles.cardValueSmall : styles.cardValue}>
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
