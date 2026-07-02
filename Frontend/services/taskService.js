import apiClient from './api';

const TASK_ENDPOINTS = {
  BASE: '/tasks',
};

/**
 * Get all tasks for the authenticated user
 * Token is automatically added via apiClient interceptor
 * Backend returns all tasks for ADMIN users, user-specific tasks for regular users
 * @returns {Promise} Array of tasks
 */
export const getAllTasks = async () => {
  const response = await apiClient.get(TASK_ENDPOINTS.BASE);
  return response.data;
};

/**
 * Get a single task by ID
 * @param {number} taskId - Task ID
 * @returns {Promise} Task object
 */
export const getTaskById = async (taskId) => {
  const response = await apiClient.get(`${TASK_ENDPOINTS.BASE}/${taskId}`);
  return response.data;
};

/**
 * Create a new task
 * @param {Object} taskData - Task data including user object
 * @returns {Promise} Created task
 */
export const createTask = async (taskData) => {
  const response = await apiClient.post(TASK_ENDPOINTS.BASE, taskData);
  return response.data;
};

/**
 * Update an existing task
 * @param {number} taskId - Task ID
 * @param {Object} taskData - Updated task data
 * @returns {Promise} Updated task
 */
export const updateTask = async (taskId, taskData) => {
  const response = await apiClient.put(TASK_ENDPOINTS.BASE, { 
    ...taskData, 
    id: taskId 
  });
  return response.data;
};

/**
 * Delete a task
 * @param {number} taskId - Task ID
 * @returns {Promise}
 */
export const deleteTask = async (taskId) => {
  const response = await apiClient.delete(`${TASK_ENDPOINTS.BASE}/${taskId}`);
  return response.data;
};

/**
 * Get tasks filtered by status
 * @param {string} status - Task status ('pending', 'done', etc.)
 * @returns {Promise} Array of filtered tasks
 */
export const getTasksByStatus = async (status) => {
  const allTasks = await getAllTasks();
  const statusLower = status.toLowerCase();
  return allTasks.filter(task => {
    const taskStatus = task.status?.toLowerCase();
    if (statusLower === 'completed' || statusLower === 'done') {
      return taskStatus === 'done' || taskStatus === 'completed';
    }
    return taskStatus === statusLower;
  });
};

/**
 * Get completed tasks for the authenticated user
 * @returns {Promise} Array of completed tasks
 */
export const getCompletedTasks = async () => {
  return getTasksByStatus('done');
};

/**
 * Get pending tasks for the authenticated user
 * @returns {Promise} Array of pending tasks
 */
export const getPendingTasks = async () => {
  return getTasksByStatus('pending');
};
