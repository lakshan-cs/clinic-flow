import React, { useState, useEffect } from 'react';
import { useRouter } from 'next/router';
import { Search, Calendar, Flag, CheckCircle, Clock, Edit2, Trash2, Filter } from 'lucide-react';
import { toast } from 'react-toastify';
import Sidebar from '../components/Sidebar';
import EditTask from '../components/EditTask';
import { getAllTasks, deleteTask } from '../services/taskService';
import { getCurrentUser, removeUser } from '../services/authService';
import styles from '../styles/AllTasks.module.css';

export default function AllTasks() {
  const router = useRouter();
  const [isSidebarOpen, setIsSidebarOpen] = useState(true);
  const [activePage, setActivePage] = useState('all-tasks');
  const [searchQuery, setSearchQuery] = useState('');
  const [filterPriority, setFilterPriority] = useState('all');
  const [filterStatus, setFilterStatus] = useState('all');
  const [sortBy, setSortBy] = useState('dueDate');
  const [sortOrder, setSortOrder] = useState('asc');
  const [currentPage, setCurrentPage] = useState(1);
  const [tasksPerPage] = useState(9);
  
  // Modal state
  const [isEditModalOpen, setIsEditModalOpen] = useState(false);
  const [selectedTask, setSelectedTask] = useState(null);

  // Tasks data and loading state
  const [tasks, setTasks] = useState([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState(null);

  // Fetch all tasks on component mount
  useEffect(() => {
    fetchTasks();
  }, []);

  const fetchTasks = async () => {
    const user = getCurrentUser();

    if (!user) {
      router.push('/login');
      return;
    }

    try {
      setIsLoading(true);
      setError(null);
      // Backend API automatically returns all tasks for ADMIN users based on token
      const data = await getAllTasks();
      setTasks(data);
    } catch (err) {
      console.error('Error fetching tasks:', err);
      const errorMessage = err.response?.data?.message || 'Failed to fetch tasks';
      setError(errorMessage);
    } finally {
      setIsLoading(false);
    }
  };

  const handleLogout = () => {
    removeUser();
    toast.success('Logged out successfully!');
    router.push('/login');
  };

  const handleEdit = (task) => {
    setSelectedTask(task);
    setIsEditModalOpen(true);
  };

  const handleSaveEdit = async (updatedTask) => {
    // Refetch all tasks to ensure we have the latest data from the backend
    await fetchTasks();
  };

  const handleCloseModal = () => {
    setIsEditModalOpen(false);
    setSelectedTask(null);
  };

  const handleDelete = async (taskId) => {
    if (window.confirm('Are you sure you want to delete this task?')) {
      try {
        await deleteTask(taskId);
        setTasks(tasks.filter(task => task.id !== taskId));
        toast.success('Task deleted successfully!');
      } catch (err) {
        console.error('Error deleting task:', err);
        const errorMessage = err.response?.data?.message || 'Failed to delete task';
        setError(errorMessage);
        toast.error("Error deleting task");
      }
    }
  };

  // Count tasks by status
  const getStatusCounts = () => {
    const counts = {
      todo: 0,
      in_progress: 0,
      done: 0
    };
    
    tasks.forEach(task => {
      const status = task.status?.toLowerCase();
      if (status === 'todo') counts.todo++;
      else if (status === 'in_progress') counts.in_progress++;
      else if (status === 'done' || status === 'completed') counts.done++;
    });
    
    return counts;
  };

  const statusCounts = getStatusCounts();

  // Count tasks by priority
  const getPriorityCounts = () => {
    const counts = {
      high: 0,
      medium: 0,
      low: 0
    };
    
    tasks.forEach(task => {
      const priority = task.priority?.toLowerCase();
      if (priority === 'high') counts.high++;
      else if (priority === 'medium') counts.medium++;
      else if (priority === 'low') counts.low++;
    });
    
    return counts;
  };

  const priorityCounts = getPriorityCounts();

  // Filter tasks by priority
  const priorityFilteredTasks = tasks.filter(task => {
    if (filterPriority === 'all') return true;
    return task.priority?.toUpperCase() === filterPriority;
  });

  // Filter by status
  const statusFilteredTasks = priorityFilteredTasks.filter(task => {
    if (filterStatus === 'all') return true;
    const taskStatus = task.status?.toLowerCase();
    if (filterStatus === 'DONE') {
      return taskStatus === 'done' || taskStatus === 'completed';
    }
    return task.status?.toUpperCase() === filterStatus;
  });

  // Filter by search query
  const searchedTasks = statusFilteredTasks.filter(task => 
    task.title.toLowerCase().includes(searchQuery.toLowerCase()) ||
    task.description.toLowerCase().includes(searchQuery.toLowerCase())
  );

  // Sort tasks
  const sortedTasks = [...searchedTasks].sort((a, b) => {
    if (sortBy === 'dueDate') {
      const dateA = new Date(a.dueDate);
      const dateB = new Date(b.dueDate);
      return sortOrder === 'asc' ? dateA - dateB : dateB - dateA;
    } else if (sortBy === 'priority') {
      const priorityOrder = { HIGH: 3, MEDIUM: 2, LOW: 1 };
      const priorityA = priorityOrder[a.priority?.toUpperCase()] || 0;
      const priorityB = priorityOrder[b.priority?.toUpperCase()] || 0;
      return sortOrder === 'asc' ? priorityA - priorityB : priorityB - priorityA;
    }
    return 0;
  });

  // Pagination
  const totalPages = Math.ceil(sortedTasks.length / tasksPerPage);
  const indexOfLastTask = currentPage * tasksPerPage;
  const indexOfFirstTask = indexOfLastTask - tasksPerPage;
  const currentTasks = sortedTasks.slice(indexOfFirstTask, indexOfLastTask);

  // Reset to page 1 when filters change
  useEffect(() => {
    setCurrentPage(1);
  }, [filterPriority, filterStatus, searchQuery, sortBy, sortOrder]);

  const getPriorityColor = (priority) => {
    const priorityLower = priority?.toLowerCase();
    switch (priorityLower) {
      case 'high': return styles.priorityHigh;
      case 'medium': return styles.priorityMedium;
      case 'low': return styles.priorityLow;
      default: return styles.priorityMedium;
    }
  };

  const getStatusColor = (status) => {
    const statusLower = status?.toLowerCase();
    return (statusLower === 'done' || statusLower === 'completed') ? styles.statusCompleted : styles.statusPending;
  };

  const formatDate = (dateString) => {
    const date = new Date(dateString);
    return date.toLocaleDateString('en-US', { 
      month: 'short', 
      day: 'numeric', 
      year: 'numeric' 
    });
  };

  const isOverdue = (dueDate, status) => {
    const statusLower = status?.toLowerCase();
    if (statusLower === 'done' || statusLower === 'completed') return false;
    const today = new Date();
    today.setHours(0, 0, 0, 0);
    const taskDate = new Date(dueDate);
    taskDate.setHours(0, 0, 0, 0);
    return taskDate < today;
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
            <h1 className={styles.pageTitle}>All Tasks</h1>
          </div>
          
          <div className={styles.headerRight}>
            <div className={styles.searchBar}>
              <Search size={20} className={styles.searchIcon} />
              <input
                type="text"
                placeholder="Search tasks..."
                value={searchQuery}
                onChange={(e) => setSearchQuery(e.target.value)}
                className={styles.searchInput}
              />
            </div>
          </div>
        </header>

        {/* Sorting Controls */}
        <div className={styles.sortingContainer}>
          <div className={styles.sortingLabel}>
            <span>Sort by:</span>
          </div>
          <div className={styles.sortingButtons}>
            <button 
              className={`${styles.sortBtn} ${sortBy === 'dueDate' ? styles.sortActive : ''}`}
              onClick={() => {
                if (sortBy === 'dueDate') {
                  setSortOrder(sortOrder === 'asc' ? 'desc' : 'asc');
                } else {
                  setSortBy('dueDate');
                  setSortOrder('asc');
                }
              }}
            >
              <Calendar size={16} />
              Due Date {sortBy === 'dueDate' && (sortOrder === 'asc' ? '↑' : '↓')}
            </button>
            <button 
              className={`${styles.sortBtn} ${sortBy === 'priority' ? styles.sortActive : ''}`}
              onClick={() => {
                if (sortBy === 'priority') {
                  setSortOrder(sortOrder === 'asc' ? 'desc' : 'asc');
                } else {
                  setSortBy('priority');
                  setSortOrder('desc');
                }
              }}
            >
              <Flag size={16} />
              Priority {sortBy === 'priority' && (sortOrder === 'asc' ? '↑' : '↓')}
            </button>
          </div>
          <div className={styles.resultCount}>
            Showing {indexOfFirstTask + 1}-{Math.min(indexOfLastTask, sortedTasks.length)} of {sortedTasks.length} tasks
          </div>
        </div>

        {/* Filter Sections */}
        <div className={styles.filtersContainer}>
          {/* Status Filter */}
          <div className={styles.filterSection}>
            <div className={styles.filterLabel}>
              <Filter size={18} />
              Filter by Status:
            </div>
            <div className={styles.filterButtons}>
              <button 
                className={`${styles.filterBtn} ${filterStatus === 'all' ? styles.filterActive : ''}`}
                onClick={() => setFilterStatus('all')}
              >
                All
              </button>
              <button 
                className={`${styles.filterBtn} ${filterStatus === 'TODO' ? styles.filterActive : ''}`}
                onClick={() => setFilterStatus('TODO')}
              >
                To Do {statusCounts.todo > 0 && <span className={styles.countBadge}>{statusCounts.todo}</span>}
              </button>
              <button 
                className={`${styles.filterBtn} ${filterStatus === 'IN_PROGRESS' ? styles.filterActive : ''}`}
                onClick={() => setFilterStatus('IN_PROGRESS')}
              >
                In Progress {statusCounts.in_progress > 0 && <span className={styles.countBadge}>{statusCounts.in_progress}</span>}
              </button>
              <button 
                className={`${styles.filterBtn} ${filterStatus === 'DONE' ? styles.filterActive : ''}`}
                onClick={() => setFilterStatus('DONE')}
              >
                Done {statusCounts.done > 0 && <span className={styles.countBadge}>{statusCounts.done}</span>}
              </button>
            </div>
          </div>

          {/* Priority Filter */}
          <div className={`${styles.filterSection} ${styles.priorityFilterSection}`}>
            <div className={styles.filterLabel}>
              <Flag size={18} />
              Filter by Priority:
            </div>
            <div className={styles.filterButtons}>
              <button 
                className={`${styles.filterBtn} ${filterPriority === 'all' ? styles.filterActive : ''}`}
                onClick={() => setFilterPriority('all')}
              >
                All
              </button>
              <button 
                className={`${styles.filterBtn} ${filterPriority === 'HIGH' ? styles.filterActive : ''}`}
                onClick={() => setFilterPriority('HIGH')}
              >
                High {priorityCounts.high > 0 && <span className={styles.countBadge}>{priorityCounts.high}</span>}
              </button>
              <button 
                className={`${styles.filterBtn} ${filterPriority === 'MEDIUM' ? styles.filterActive : ''}`}
                onClick={() => setFilterPriority('MEDIUM')}
              >
                Medium {priorityCounts.medium > 0 && <span className={styles.countBadge}>{priorityCounts.medium}</span>}
              </button>
              <button 
                className={`${styles.filterBtn} ${filterPriority === 'LOW' ? styles.filterActive : ''}`}
                onClick={() => setFilterPriority('LOW')}
              >
                Low {priorityCounts.low > 0 && <span className={styles.countBadge}>{priorityCounts.low}</span>}
              </button>
            </div>
          </div>

        </div>

        {/* Tasks Grid */}
        <div className={styles.tasksGrid}>
          {isLoading ? (
            <div className={styles.loadingContainer}>
              <div className={styles.spinner}></div>
              <p>Loading tasks...</p>
            </div>
          ) : error ? (
            <div className={styles.errorContainer}>
              <p className={styles.errorMessage}>{error}</p>
              <button className={styles.retryBtn} onClick={fetchTasks}>Retry</button>
            </div>
          ) : searchedTasks.length === 0 ? (
            <div className={styles.noTasks}>
              <p>No tasks found</p>
            </div>
          ) : (
            currentTasks.map(task => (
              <div key={task.id} className={styles.taskCard}>
                <div className={styles.taskCardHeader}>
                  <h3 className={styles.taskTitle}>{task.title}</h3>
                </div>

                <div className={styles.priorityBadgeContainer}>
                  <span className={`${styles.priorityBadge} ${getPriorityColor(task.priority || 'medium')}`}>
                    <Flag size={14} />
                    <span style={{ textTransform: 'capitalize' }}>{task.priority?.toLowerCase() || 'medium'}</span>
                  </span>
                </div>
                
                <p className={styles.taskDescription}>{task.description}</p>
                
                <div className={styles.taskFooter}>
                  <div className={styles.taskMeta}>
                    <span className={`${styles.statusBadge} ${getStatusColor(task.status)}`}>
                      {(task.status?.toLowerCase() === 'done' || task.status?.toLowerCase() === 'completed') ? (
                        <CheckCircle size={16} />
                      ) : (
                        <Clock size={16} />
                      )}
                      <span style={{ textTransform: 'capitalize' }}>{task.status?.toLowerCase()}</span>
                    </span>
                    
                    <span className={`${styles.dueDate} ${isOverdue(task.dueDate, task.status) ? styles.overdueDate : ''}`}>
                      <Calendar size={16} />
                      {formatDate(task.dueDate)}
                      {isOverdue(task.dueDate, task.status) && <span className={styles.overdueLabel}>Overdue</span>}
                    </span>
                  </div>
                </div>

                <div className={styles.taskActionsBottom}>
                  <button 
                    className={`${styles.actionBtnBottom} ${styles.editBtnBottom}`}
                    onClick={() => handleEdit(task)}
                  >
                    <Edit2 size={16} />
                    Edit
                  </button>
                  <button 
                    className={`${styles.actionBtnBottom} ${styles.deleteBtnBottom}`}
                    onClick={() => handleDelete(task.id)}
                  >
                    <Trash2 size={16} />
                    Delete
                  </button>
                </div>
              </div>
            ))
          )}
        </div>

        {/* Pagination Controls */}
        {!isLoading && !error && sortedTasks.length > 0 && (
          <div className={styles.paginationContainer}>
            <button 
              className={styles.paginationBtn}
              onClick={() => setCurrentPage(prev => Math.max(prev - 1, 1))}
              disabled={currentPage === 1}
            >
              Previous
            </button>
            
            <div className={styles.paginationNumbers}>
              {Array.from({ length: totalPages }, (_, i) => i + 1).map(page => (
                <button
                  key={page}
                  className={`${styles.paginationNumber} ${currentPage === page ? styles.paginationActive : ''}`}
                  onClick={() => setCurrentPage(page)}
                >
                  {page}
                </button>
              ))}
            </div>
            
            <button 
              className={styles.paginationBtn}
              onClick={() => setCurrentPage(prev => Math.min(prev + 1, totalPages))}
              disabled={currentPage === totalPages}
            >
              Next
            </button>
          </div>
        )}
      </main>

      <EditTask 
        task={selectedTask}
        isOpen={isEditModalOpen}
        onClose={handleCloseModal}
        onSave={handleSaveEdit}
      />
    </div>
  );
}
