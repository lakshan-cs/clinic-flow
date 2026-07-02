import React, { useState, useEffect } from 'react';
import { useRouter } from 'next/router';
import { Pencil, Trash2, Plus, Menu } from 'lucide-react';
import { toast } from 'react-toastify';
import Sidebar from '../components/Sidebar';
import { getAllProviders, createProvider, updateProvider, deleteProvider } from '../services/providerService';
import { getAllClinics } from '../services/clinicService';
import styles from '../styles/Providers.module.css';

export default function Providers() {
  const router = useRouter();
  const [isSidebarOpen, setIsSidebarOpen] = useState(true);
  const [providers, setProviders] = useState([]);
  const [clinics, setClinics] = useState([]);
  const [isLoading, setIsLoading] = useState(true);

  // Modal state
  const [showModal, setShowModal] = useState(false);
  const [isEditing, setIsEditing] = useState(false);
  const [selectedProvider, setSelectedProvider] = useState(null);
  const [formData, setFormData] = useState({ name: '', speciality: '', clinicId: '' });
  const [isSaving, setIsSaving] = useState(false);

  // Delete confirm state
  const [showDeleteModal, setShowDeleteModal] = useState(false);
  const [providerToDelete, setProviderToDelete] = useState(null);

  useEffect(() => {
    fetchProviders();
    fetchClinics();
  }, []);

  const fetchProviders = async () => {
    try {
      setIsLoading(true);
      const data = await getAllProviders();
      setProviders(data);
    } catch (err) {
      toast.error('Failed to load providers');
    } finally {
      setIsLoading(false);
    }
  };

  const fetchClinics = async () => {
    try {
      const data = await getAllClinics();
      setClinics(data);
    } catch (err) {
      toast.error('Failed to load clinics');
    }
  };

  const getClinicName = (clinicId) => {
    const clinic = clinics.find(c => c.id === clinicId);
    return clinic ? clinic.name : '—';
  };

  const handleLogout = () => {
    localStorage.removeItem('user');
    localStorage.removeItem('token');
    router.push('/login');
  };

  // ── Add / Edit ──────────────────────────────────────────────
  const openAddModal = () => {
    setIsEditing(false);
    setSelectedProvider(null);
    setFormData({ name: '', speciality: '', clinicId: '' });
    setShowModal(true);
  };

  const openEditModal = (provider) => {
    setIsEditing(true);
    setSelectedProvider(provider);
    setFormData({ name: provider.name, speciality: provider.speciality, clinicId: provider.clinicId });
    setShowModal(true);
  };

  const handleSave = async () => {
    if (!formData.name.trim() || !formData.speciality.trim() || !formData.clinicId) {
      toast.warning('Name, speciality and clinic are required');
      return;
    }
    try {
      setIsSaving(true);
      const payload = { name: formData.name, speciality: formData.speciality, clinicId: Number(formData.clinicId) };
      if (isEditing) {
        await updateProvider({ ...payload, id: selectedProvider.id });
        toast.success('Provider updated successfully');
      } else {
        await createProvider(payload);
        toast.success('Provider added successfully');
      }
      setShowModal(false);
      fetchProviders();
    } catch (err) {
      toast.error(isEditing ? 'Failed to update provider' : 'Failed to add provider');
    } finally {
      setIsSaving(false);
    }
  };

  // ── Delete ───────────────────────────────────────────────────
  const openDeleteModal = (provider) => {
    setProviderToDelete(provider);
    setShowDeleteModal(true);
  };

  const handleDelete = async () => {
    try {
      await deleteProvider(providerToDelete.id);
      toast.success('Provider deleted successfully');
      setShowDeleteModal(false);
      fetchProviders();
    } catch (err) {
      toast.error('Failed to delete provider');
    }
  };

  return (
    <div className={styles.pageContainer}>
      <Sidebar isOpen={isSidebarOpen} activePage="providers" onLogout={handleLogout} />

      <main className={styles.main}>
        {/* Header */}
        <div className={styles.header}>
          <div className={styles.headerLeft}>
            <button className={styles.menuBtn} onClick={() => setIsSidebarOpen(o => !o)}>
              <Menu size={20} />
            </button>
            <div>
              <h1 className={styles.pageTitle}>Healthcare Providers</h1>
              <p className={styles.pageSubtitle}>Manage healthcare providers</p>
            </div>
          </div>
          <button className={styles.addBtn} onClick={openAddModal}>
            <Plus size={18} />
            Add Provider
          </button>
        </div>

        {/* Table */}
        <div className={styles.tableCard}>
          {isLoading ? (
            <div className={styles.emptyState}>Loading providers...</div>
          ) : providers.length === 0 ? (
            <div className={styles.emptyState}>No providers found. Add one to get started.</div>
          ) : (
            <table className={styles.table}>
              <thead>
                <tr>
                  <th>Name</th>
                  <th>Speciality</th>
                  <th>Assigned Clinic</th>
                  <th className={styles.actionsCol}>Actions</th>
                </tr>
              </thead>
              <tbody>
                {providers.map((provider) => (
                  <tr key={provider.id}>
                    <td>{provider.name}</td>
                    <td>{provider.speciality}</td>
                    <td>{getClinicName(provider.clinicId)}</td>
                    <td className={styles.actionsCell}>
                      <button className={styles.editBtn} onClick={() => openEditModal(provider)} title="Edit">
                        <Pencil size={15} />
                        Edit
                      </button>
                      <button className={styles.deleteBtn} onClick={() => openDeleteModal(provider)} title="Delete">
                        <Trash2 size={15} />
                        Delete
                      </button>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          )}
        </div>
      </main>

      {/* Add / Edit Modal */}
      {showModal && (
        <div className={styles.modalOverlay} onClick={() => setShowModal(false)}>
          <div className={styles.modal} onClick={e => e.stopPropagation()}>
            <h2 className={styles.modalTitle}>{isEditing ? 'Edit Provider' : 'Add Provider'}</h2>
            {isEditing && (
              <div className={styles.formGroup}>
                <label>Provider ID</label>
                <input
                  type="text"
                  value={selectedProvider?.id ?? ''}
                  readOnly
                  className={styles.readOnlyInput}
                />
              </div>
            )}
            <div className={styles.formGroup}>
              <label>Name *</label>
              <input
                type="text"
                value={formData.name}
                onChange={e => setFormData(f => ({ ...f, name: e.target.value }))}
                placeholder="e.g. Dr. Saman Perera"
              />
            </div>
            <div className={styles.formGroup}>
              <label>Speciality *</label>
              <input
                type="text"
                value={formData.speciality}
                onChange={e => setFormData(f => ({ ...f, speciality: e.target.value }))}
                placeholder="e.g. Cardiology, General Medicine"
              />
            </div>
            <div className={styles.formGroup}>
              <label>Assigned Clinic *</label>
              <select
                value={formData.clinicId}
                onChange={e => setFormData(f => ({ ...f, clinicId: e.target.value }))}
                className={styles.selectInput}
              >
                <option value="">Select a clinic</option>
                {clinics.map(clinic => (
                  <option key={clinic.id} value={clinic.id}>{clinic.name}</option>
                ))}
              </select>
            </div>
            <div className={styles.modalActions}>
              <button className={styles.cancelBtn} onClick={() => setShowModal(false)}>Cancel</button>
              <button className={styles.saveBtn} onClick={handleSave} disabled={isSaving}>
                {isSaving ? 'Saving...' : isEditing ? 'Update' : 'Add Provider'}
              </button>
            </div>
          </div>
        </div>
      )}

      {/* Delete Confirm Modal */}
      {showDeleteModal && (
        <div className={styles.modalOverlay} onClick={() => setShowDeleteModal(false)}>
          <div className={styles.modal} onClick={e => e.stopPropagation()}>
            <h2 className={styles.modalTitle}>Delete Provider</h2>
            <p className={styles.deleteMessage}>
              Are you sure you want to delete <strong>{providerToDelete?.name}</strong>? This action cannot be undone.
            </p>
            <div className={styles.modalActions}>
              <button className={styles.cancelBtn} onClick={() => setShowDeleteModal(false)}>Cancel</button>
              <button className={styles.confirmDeleteBtn} onClick={handleDelete}>Delete</button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
}
