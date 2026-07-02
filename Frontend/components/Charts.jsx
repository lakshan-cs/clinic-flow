import React, { useMemo } from 'react';
import { LineChart, Line, PieChart, Pie, Cell, XAxis, YAxis, CartesianGrid, Tooltip, Legend, ResponsiveContainer } from 'recharts';
import styles from './Charts.module.css';

const Charts = ({ tasks = [], pendingTasks = 0, inProgressTasks = 0, completedTasks = 0, isLoading = false }) => {
  const lineData = useMemo(() => {
    const monthlyTrend = [];
    const now = new Date();

    for (let i = 5; i >= 0; i--) {
      const monthDate = new Date(now.getFullYear(), now.getMonth() - i, 1);
      const monthLabel = monthDate.toLocaleDateString('en-US', { month: 'short' });
      const month = monthDate.getMonth();
      const year = monthDate.getFullYear();

      const completedInMonth = tasks.filter(task => {
        const status = task.status?.toLowerCase();
        if (status !== 'done' && status !== 'completed') return false;

        const taskDate = new Date(task.dueDate);
        if (Number.isNaN(taskDate.getTime())) return false;

        return taskDate.getMonth() === month && taskDate.getFullYear() === year;
      }).length;

      monthlyTrend.push({
        month: monthLabel,
        completed: completedInMonth
      });
    }

    return monthlyTrend;
  }, [tasks]);

  const pieData = useMemo(() => {
    const data = [
      { name: 'Pending', value: pendingTasks, color: '#f59e0b' },
      { name: 'In Progress', value: inProgressTasks, color: '#3b82f6' },
      { name: 'Completed', value: completedTasks, color: '#10b981' }
    ];
    return data.filter(item => item.value > 0);
  }, [pendingTasks, inProgressTasks, completedTasks]);

  const hasTaskData = pieData.length > 0;

  if (isLoading) {
    return (
      <div className={styles.chartsGrid}>
        <div className={styles.chartCard}>
          <h3 className={styles.chartTitle}>Monthly Completion Trend</h3>
          <div style={{ display: 'flex', justifyContent: 'center', alignItems: 'center', height: '300px' }}>
            <p>Loading chart data...</p>
          </div>
        </div>
        <div className={styles.chartCard}>
          <h3 className={styles.chartTitle}>Task Distribution</h3>
          <div style={{ display: 'flex', justifyContent: 'center', alignItems: 'center', height: '300px' }}>
            <p>Loading chart data...</p>
          </div>
        </div>
      </div>
    );
  }

  return (
    <div className={styles.chartsGrid}>
      <div className={styles.chartCard}>
        <h3 className={styles.chartTitle}>Monthly Completion Trend</h3>
        <ResponsiveContainer width="100%" height={300}>
          <LineChart data={lineData}>
            <CartesianGrid strokeDasharray="3 3" stroke="#e5e7eb" />
            <XAxis dataKey="month" stroke="#6b7280" />
            <YAxis stroke="#6b7280" />
            <Tooltip 
              contentStyle={{
                backgroundColor: '#ffffff',
                border: '1px solid #e5e7eb',
                borderRadius: '8px'
              }}
            />
            <Legend />
            <Line 
              type="monotone" 
              dataKey="completed" 
              stroke="#10b981" 
              strokeWidth={2}
              dot={{ fill: '#10b981', r: 4 }}
              activeDot={{ r: 6 }}
            />
          </LineChart>
        </ResponsiveContainer>
      </div>

      <div className={styles.chartCard}>
        <h3 className={styles.chartTitle}>Task Distribution</h3>
        {hasTaskData ? (
          <ResponsiveContainer width="100%" height={300}>
            <PieChart>
              <Pie
                data={pieData}
                cx="50%"
                cy="50%"
                labelLine={true}
                label={({ name, percent }) => `${name}: ${(percent * 100).toFixed(0)}%`}
                outerRadius={90}
                fill="#8884d8"
                dataKey="value"
              >
                {pieData.map((entry, index) => (
                  <Cell key={`cell-${index}`} fill={entry.color} />
                ))}
              </Pie>
              <Tooltip />
              <Legend />
            </PieChart>
          </ResponsiveContainer>
        ) : (
          <div style={{ display: 'flex', justifyContent: 'center', alignItems: 'center', height: '300px', color: '#6b7280' }}>
            <p>No task data available</p>
          </div>
        )}
      </div>
    </div>
  );
};

export default Charts;
