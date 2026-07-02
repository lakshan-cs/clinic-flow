import apiClient from './api';

const AUTH_ENDPOINTS = {
  LOGIN: '/api/user/login',
};

/**
 * Login user
 * @param {Object} credentials - { email, password }
 * @returns {Promise} User data
 */
export const loginUser = async (credentials) => {
  const response = await apiClient.post(AUTH_ENDPOINTS.LOGIN, credentials);
  return response.data;
};

/**
 * Register new user
 * @param {Object} userData - User registration data
 * @returns {Promise} User data
 */
export const registerUser = async (userData) => {
  const response = await apiClient.post(AUTH_ENDPOINTS.REGISTER, userData);
  return response.data;
};

/**
 * Get user from localStorage
 * @returns {Object|null} User object or null
 */
export const getCurrentUser = () => {
  if (typeof window !== 'undefined') {
    const user = localStorage.getItem('user');
    return user ? JSON.parse(user) : null;
  }
  return null;
};

/**
 * Get token from localStorage
 * @returns {string|null} Token or null
 */
export const getToken = () => {
  if (typeof window !== 'undefined') {
    return localStorage.getItem('token');
  }
  return null;
};

/**
 * Save user and token to localStorage
 * @param {Object} data - Response data containing user and token
 */
export const saveUser = (data) => {
  if (typeof window !== 'undefined') {
    if (data.token) {
      localStorage.setItem('token', data.token);
    }
    localStorage.setItem('user', JSON.stringify(data));
  }
};

export const removeUser = () => {
  if (typeof window !== 'undefined') {
    localStorage.removeItem('user');
    localStorage.removeItem('token');
  }
};

/**
 * Check if user is authenticated
 * @returns {boolean}
 */
export const isAuthenticated = () => {
  return getToken() !== null;
};

export const logout = () => {
  removeUser();
};
