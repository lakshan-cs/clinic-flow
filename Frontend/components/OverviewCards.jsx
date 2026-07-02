import React from 'react';
import { Clock, ListTodo, CheckCircle } from 'lucide-react';
import styles from './OverviewCards.module.css';

const OverviewCards = ({ pendingTasks = 0, inProgressTasks = 0, completedTasks = 0, isLoading = false }) => {
  const cardsData = [
    {
      id: 1,
      title: 'Pending Tasks',
      value: pendingTasks,
      trend: 'Waiting to start',
      icon: Clock,
      colorClass: styles.cardOrange
    },
    {
      id: 2,
      title: 'In Progress Tasks',
      value: inProgressTasks,
      trend: 'Currently working',
      icon: ListTodo,
      colorClass: styles.cardBlue
    },
    {
      id: 3,
      title: 'Completed Tasks',
      value: completedTasks,
      trend: 'Successfully finished',
      icon: CheckCircle,
      colorClass: styles.cardGreen
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
