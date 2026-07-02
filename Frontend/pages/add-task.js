import React, { useState } from 'react';
import { useRouter } from 'next/router';
import { toast } from 'react-toastify';
import Sidebar from '../components/Sidebar';
import { createTask } from '../services/taskService';
import { getCurrentUser, removeUser } from '../services/authService';
import styles from '../styles/AddTask.module.css';

export default function AddTask() {
  const router = useRouter();
  const [isSidebarOpen, setIsSidebarOpen] = useState(true);
  const [activePage, setActivePage] = useState('add-task');
  const [formData, setFormData] = useState({
    title: '',
    description: '',
    status: 'TODO',
    priority: 'MEDIUM',
    dueDate: ''
  });

  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState(null);
  const [successMessage, setSuccessMessage] = useState(null);

  const handleLogout = () => {
    removeUser();
    toast.success('Logged out successfully!');
    router.push('/login');
  };

  const handleInputChange = (e) => {
    const { name, value } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: value
    }));
    
    if (error) setError(null);
    if (successMessage) setSuccessMessage(null);
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setIsLoading(true);
    setError(null);
    setSuccessMessage(null);

    try {
      const user = getCurrentUser();

      if (!user) {
        router.push('/login');
        return;
      }

      const taskData = {
        title: formData.title,
        description: formData.description,
        status: formData.status,
        priority: formData.priority,
        dueDate: formData.dueDate
      };

      await createTask(taskData);

      setSuccessMessage('Task added successfully!');
      toast.success('Task added successfully!');

      handleReset();

      setTimeout(() => {
        router.push('/all-tasks');
      }, 1500);

    } catch (err) {
      console.error('Error adding task:', err);
      const errorMessage = err.response?.data?.message || err.response?.data?.error || err.message || 'An error occurred while adding the task';
      setError(errorMessage);
      toast.error(errorMessage);
    } finally {
      setIsLoading(false);
    }
  };

  const handleReset = () => {
    setFormData({
      title: '',
      description: '',
      status: 'TODO',
      priority: 'MEDIUM',
      dueDate: ''
    });
    setError(null);
    setSuccessMessage(null);
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
            <h1 className={styles.pageTitle}>Add New Task</h1>
          </div>
        </header>

        <div className={styles.content}>
          <div className={styles.formContainer}>
            <form onSubmit={handleSubmit} className={styles.taskForm}>
              <div className={styles.formGroup}>
                <label htmlFor="title" className={styles.formLabel}>
                  Task Title <span className={styles.required}>*</span>
                </label>
                <input
                  type="text"
                  id="title"
                  name="title"
                  value={formData.title}
                  onChange={handleInputChange}
                  className={styles.formInput}
                  placeholder="Enter task title"
                  required
                  disabled={isLoading}
                />
              </div>

              <div className={`${styles.formGroup} ${styles.descriptionGroup}`}>
                <label htmlFor="description" className={styles.formLabel}>
                  Description <span className={styles.required}>*</span>
                </label>
                <textarea
                  id="description"
                  name="description"
                  value={formData.description}
                  onChange={handleInputChange}
                  className={styles.formTextarea}
                  placeholder="Enter task description"
                  rows="5"
                  required
                  disabled={isLoading}
                />
              </div>

              <div className={styles.formRow}>
                <div className={styles.formGroup}>
                  <label htmlFor="status" className={styles.formLabel}>
                    Status <span className={styles.required}>*</span>
                  </label>
                  <select
                    id="status"
                    name="status"
                    value={formData.status}
                    onChange={handleInputChange}
                    className={styles.formSelect}
                    required
                    disabled={isLoading}
                  >
                    <option value="TODO">Pending (TODO)</option>
                    <option value="IN_PROGRESS">In Progress</option>
                    <option value="DONE">Done</option>
                  </select>
                </div>

                <div className={styles.formGroup}>
                  <label htmlFor="priority" className={styles.formLabel}>
                    Priority <span className={styles.required}>*</span>
                  </label>
                  <select
                    id="priority"
                    name="priority"
                    value={formData.priority}
                    onChange={handleInputChange}
                    className={styles.formSelect}
                    required
                    disabled={isLoading}
                  >
                    <option value="HIGH">High</option>
                    <option value="MEDIUM">Medium</option>
                    <option value="LOW">Low</option>
                  </select>
                </div>
              </div>

              <div className={styles.formGroup}>
                <label htmlFor="dueDate" className={styles.formLabel}>
                  Due Date <span className={styles.required}>*</span>
                </label>
                <input
                  type="date"
                  id="dueDate"
                  name="dueDate"
                  value={formData.dueDate}
                  onChange={handleInputChange}
                  className={styles.formInput}
                  required
                  disabled={isLoading}
                />
              </div>

              {successMessage && (
                <div className={styles.successMessage}>
                  {successMessage}
                </div>
              )}
              {error && (
                <div className={styles.errorMessage}>
                  {error}
                </div>
              )}

              <button
                type="submit"
                className={`${styles.btn} ${styles.btnPrimary}`}
                disabled={isLoading}
              >
                {isLoading ? 'Adding Task...' : 'Submit'}
              </button>
            </form>
          </div>
        </div>
      </main>
    </div>
  );
}
