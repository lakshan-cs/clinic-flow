import apiClient from './api';

export const getAllAppointments = () => apiClient.get('/api/Appointment').then(r => r.data);
export const createAppointment = (data) => apiClient.post('/api/Appointment', data).then(r => r.data);
