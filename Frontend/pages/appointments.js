import React, { useState, useEffect } from 'react';
import { useRouter } from 'next/router';
import { Plus, Menu, Search } from 'lucide-react';
import { toast } from 'react-toastify';
import Sidebar from '../components/Sidebar';
import { getAllAppointments, createAppointment } from '../services/appointmentService';
import { getAllPatients } from '../services/patientService';
import { getAllClinics } from '../services/clinicService';
import { getAllProviders } from '../services/providerService';
import styles from '../styles/Appointments.module.css';

const emptyForm = () => ({
  patientId: '',
  clinicId: '',
  providerId: '',
  date: '',
  time: '',
  reason: '',
});

export default function Appointments() {
  const router = useRouter();
  const [isSidebarOpen, setIsSidebarOpen] = useState(true);
  const [appointments, setAppointments] = useState([]);
  const [patients, setPatients] = useState([]);
  const [clinics, setClinics] = useState([]);
  const [providers, setProviders] = useState([]);
  const [isLoading, setIsLoading] = useState(true);

  const [showModal, setShowModal] = useState(false);
  const [formData, setFormData] = useState(emptyForm());
  const [isSaving, setIsSaving] = useState(false);
  const [searchQuery, setSearchQuery] = useState('');

  useEffect(() => {
    fetchAll();
  }, []);

  const fetchAll = async () => {
    try {
      setIsLoading(true);
      const [appts, pts, cls, prvs] = await Promise.all([
        getAllAppointments(),
        getAllPatients(),
        getAllClinics(),
        getAllProviders(),
      ]);
      setAppointments(appts);
      setPatients(pts);
      setClinics(cls);
      setProviders(prvs);
    } catch (err) {
      toast.error('Failed to load data');
    } finally {
      setIsLoading(false);
    }
  };

  const handleLogout = () => {
    localStorage.removeItem('user');
    localStorage.removeItem('token');
    router.push('/login');
  };

  const openAddModal = () => {
    setFormData(emptyForm());
    setShowModal(true);
  };

  const handleFieldChange = (field, value) => {
    setFormData(f => ({ ...f, [field]: value }));
  };

  const validate = () => {
    if (!formData.patientId) { toast.warning('Patient is required'); return false; }
    if (!formData.clinicId) { toast.warning('Clinic is required'); return false; }
    if (!formData.providerId) { toast.warning('Provider is required'); return false; }
    if (!formData.date) { toast.warning('Appointment date is required'); return false; }
    if (!formData.time) { toast.warning('Appointment time is required'); return false; }
    return true;
  };

  const handleSave = async () => {
    if (!validate()) return;
    try {
      setIsSaving(true);
      const dateTime = `${formData.date}T${formData.time}:00`;
      const payload = {
        patientId: parseInt(formData.patientId, 10),
        clinicId: parseInt(formData.clinicId, 10),
        providerId: parseInt(formData.providerId, 10),
        dateTime,
        reason: formData.reason || null,
      };
      await createAppointment(payload);
      toast.success('Appointment created successfully');
      setShowModal(false);
      const appts = await getAllAppointments();
      setAppointments(appts);
    } catch (err) {
      const message = err?.response?.data?.error || 'Failed to create appointment';
      toast.error(message);
    } finally {
      setIsSaving(false);
    }
  };

  // ── Lookup helpers ──────────────────────────────────
  const getPatientName = (id) => {
    const p = patients.find(p => p.id === id);
    return p ? p.fullName : `Patient #${id}`;
  };

  const getClinicName = (id) => {
    const c = clinics.find(c => c.id === id);
    return c ? c.name : `Clinic #${id}`;
  };

  const getProviderName = (id) => {
    const p = providers.find(p => p.id === id);
    return p ? p.name : `Provider #${id}`;
  };

  const formatDateTime = (dt) => {
    if (!dt) return '';
    const d = new Date(dt);
    return d.toLocaleString(undefined, {
      year: 'numeric', month: 'short', day: 'numeric',
      hour: '2-digit', minute: '2-digit',
    });
  };

  const filteredAppointments = appointments.filter(appt => {
    const q = searchQuery.toLowerCase();
    if (!q) return true;
    return (
      getPatientName(appt.patientId).toLowerCase().includes(q) ||
      getClinicName(appt.clinicId).toLowerCase().includes(q) ||
      getProviderName(appt.providerId).toLowerCase().includes(q) ||
      (appt.reason || '').toLowerCase().includes(q) ||
      formatDateTime(appt.dateTime).toLowerCase().includes(q)
    );
  });

  return (
    <div className={styles.pageContainer}>
      <Sidebar isOpen={isSidebarOpen} activePage="appointments" onLogout={handleLogout} />

      <main className={styles.main}>
        {/* Header */}
        <div className={styles.header}>
          <div className={styles.headerLeft}>
            <button className={styles.menuBtn} onClick={() => setIsSidebarOpen(o => !o)}>
              <Menu size={20} />
            </button>
            <div>
              <h1 className={styles.pageTitle}>Appointments</h1>
              <p className={styles.pageSubtitle}>View and schedule appointments</p>
            </div>
          </div>
          <div className={styles.headerRight}>
            <div className={styles.searchBar}>
              <Search size={16} className={styles.searchIcon} />
              <input
                type="text"
                placeholder="Search appointments..."
                value={searchQuery}
                onChange={e => setSearchQuery(e.target.value)}
                className={styles.searchInput}
              />
            </div>
            <button className={styles.addBtn} onClick={openAddModal}>
              <Plus size={18} />
              Add Appointment
            </button>
          </div>
        </div>

        {/* Table */}
        <div className={styles.tableCard}>
          {isLoading ? (
            <div className={styles.emptyState}>Loading appointments...</div>
          ) : filteredAppointments.length === 0 ? (
            <div className={styles.emptyState}>{appointments.length === 0 ? 'No appointments found. Schedule one to get started.' : 'No appointments match your search.'}</div>
          ) : (
            <table className={styles.table}>
              <thead>
                <tr>
                  <th>Patient</th>
                  <th>Clinic</th>
                  <th>Provider</th>
                  <th>Date &amp; Time</th>
                  <th>Reason</th>
                </tr>
              </thead>
              <tbody>
                {filteredAppointments.map((appt) => (
                  <tr key={appt.id}>
                    <td>{getPatientName(appt.patientId)}</td>
                    <td>{getClinicName(appt.clinicId)}</td>
                    <td>{getProviderName(appt.providerId)}</td>
                    <td>{formatDateTime(appt.dateTime)}</td>
                    <td className={appt.reason ? styles.reasonCell : styles.noReason}>
                      {appt.reason || '—'}
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          )}
        </div>
      </main>

      {/* Add Appointment Modal */}
      {showModal && (
        <div className={styles.modalOverlay} onClick={() => setShowModal(false)}>
          <div className={styles.modal} onClick={e => e.stopPropagation()}>
            <h2 className={styles.modalTitle}>Add Appointment</h2>

            <div className={styles.formGroup}>
              <label>Patient *</label>
              <select
                value={formData.patientId}
                onChange={e => handleFieldChange('patientId', e.target.value)}
              >
                <option value="">-- Select a patient --</option>
                {patients.map(p => (
                  <option key={p.id} value={p.id}>{p.fullName}</option>
                ))}
              </select>
            </div>

            <div className={styles.formGroup}>
              <label>Clinic *</label>
              <select
                value={formData.clinicId}
                onChange={e => handleFieldChange('clinicId', e.target.value)}
              >
                <option value="">-- Select a clinic --</option>
                {clinics.map(c => (
                  <option key={c.id} value={c.id}>{c.name}</option>
                ))}
              </select>
            </div>

            <div className={styles.formGroup}>
              <label>Provider *</label>
              <select
                value={formData.providerId}
                onChange={e => handleFieldChange('providerId', e.target.value)}
              >
                <option value="">-- Select a provider --</option>
                {providers.map(p => (
                  <option key={p.id} value={p.id}>{p.name}</option>
                ))}
              </select>
            </div>

            <div className={styles.formGroup}>
              <label>Appointment Date *</label>
              <input
                type="date"
                value={formData.date}
                min={new Date().toISOString().split('T')[0]}
                onChange={e => handleFieldChange('date', e.target.value)}
              />
            </div>

            <div className={styles.formGroup}>
              <label>Appointment Time *</label>
              <input
                type="time"
                value={formData.time}
                onChange={e => handleFieldChange('time', e.target.value)}
              />
            </div>

            <div className={styles.formGroup}>
              <label>Reason for Visit</label>
              <textarea
                value={formData.reason}
                onChange={e => handleFieldChange('reason', e.target.value)}
                placeholder="Enter reason for visit (optional)"
              />
            </div>

            <div className={styles.modalFooter}>
              <button className={styles.cancelBtn} onClick={() => setShowModal(false)}>
                Cancel
              </button>
              <button className={styles.saveBtn} onClick={handleSave} disabled={isSaving}>
                {isSaving ? 'Saving...' : 'Save'}
              </button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
}
