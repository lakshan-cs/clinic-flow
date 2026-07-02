import apiClient from './api';

export const getAllAllergies = () => apiClient.get('/api/allergy').then(r => r.data);
export const createAllergy = (data) => apiClient.post('/api/allergy', data).then(r => r.data);
export const updateAllergy = (data) => apiClient.put(`/api/allergy`, data).then(r => r.data);
export const deleteAllergy = (id) => apiClient.delete(`/api/allergy/${id}`).then(r => r.data);
