import apiClient from './api';

export const getAllProviders = () => apiClient.get('/api/Provider').then(r => r.data);
export const createProvider = (data) => apiClient.post('/api/Provider', data).then(r => r.data);
export const updateProvider = (data) => apiClient.put(`/api/Provider`, data).then(r => r.data);
export const deleteProvider = (id) => apiClient.delete(`/api/Provider/${id}`).then(r => r.data);
