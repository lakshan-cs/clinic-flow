import apiClient from './api';

export const getAllPatients = () => apiClient.get('/api/Patient').then(r => r.data);
export const createPatient = (data) => apiClient.post('/api/Patient', data).then(r => r.data);
export const updatePatient = (data) => apiClient.put('/api/Patient', data).then(r => r.data);
export const deletePatient = (id) => apiClient.delete(`/api/Patient/${id}`).then(r => r.data);
export const getPatientAllergies = (patientId) =>
  apiClient.get(`/api/PatientAllergy/patient/${patientId}`).then(r => r.data);
