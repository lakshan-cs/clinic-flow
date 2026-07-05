import apiClient from './api';

export const getAllergysByPatientId = (patientId) =>
  apiClient.get(`/api/patientallergy/patient/${patientId}`).then(r => r.data);
