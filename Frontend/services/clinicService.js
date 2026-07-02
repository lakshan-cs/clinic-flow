import apiClient from './api';

export const getAllClinics = () => apiClient.get('/api/Clinic').then(r => r.data);
export const createClinic = (data) => apiClient.post('/api/Clinic', data).then(r => r.data);
export const updateClinic = (data) => apiClient.put(`/api/Clinic`, data).then(r => r.data);
export const deleteClinic = (id) => apiClient.delete(`/api/Clinic/${id}`).then(r => r.data);
