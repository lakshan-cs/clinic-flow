import React, { useState, useEffect } from 'react';
import { useRouter } from 'next/router';
import { Pencil, Trash2, Plus, Menu } from 'lucide-react';
import { toast } from 'react-toastify';
import Sidebar from '../components/Sidebar';
import { getAllClinics, createClinic, updateClinic, deleteClinic } from '../services/clinicService';
import styles from '../styles/Clinics.module.css';

export default function Clinics() {
  const router = useRouter();
  const [isSidebarOpen, setIsSidebarOpen] = useState(true);
  const [clinics, setClinics] = useState([]);
  const [isLoading, setIsLoading] = useState(true);

  // Modal state
  const [showModal, setShowModal] = useState(false);
  const [isEditing, setIsEditing] = useState(false);
  const [selectedClinic, setSelectedClinic] = useState(null);
  const [formData, setFormData] = useState({ name: '', location: '' });
  const [isSaving, setIsSaving] = useState(false);

  // Delete confirm state
  const [showDeleteModal, setShowDeleteModal] = useState(false);
  const [clinicToDelete, setClinicToDelete] = useState(null);

  useEffect(() => {
    fetchClinics();
  }, []);

  const fetchClinics = async () => {
    try {
      setIsLoading(true);
      const data = await getAllClinics();
      setClinics(data);
    } catch (err) {
      toast.error('Failed to load clinics');
    } finally {
      setIsLoading(false);
    }
  };

  const handleLogout = () => {
    localStorage.removeItem('user');
    localStorage.removeItem('token');
    router.push('/login');
  };

  // ── Add / Edit ──────────────────────────────────────────────
  const openAddModal = () => {
    setIsEditing(false);
    setSelectedClinic(null);
    setFormData({ name: '', location: '' });
    setShowModal(true);
  };

  const openEditModal = (clinic) => {
    setIsEditing(true);
    setSelectedClinic(clinic);
    setFormData({ name: clinic.name, location: clinic.location });
    setShowModal(true);
  };

  const handleSave = async () => {
    if (!formData.name.trim() || !formData.location.trim()) {
      toast.warning('Name and location are required');
      return;
    }
    try {
      setIsSaving(true);
      if (isEditing) {
        await updateClinic({ ...formData, id: selectedClinic.id });
        toast.success('Clinic updated successfully');
      } else {
        await createClinic(formData);
        toast.success('Clinic added successfully');
      }
      setShowModal(false);
      fetchClinics();
    } catch (err) {
      toast.error(isEditing ? 'Failed to update clinic' : 'Failed to add clinic');
    } finally {
      setIsSaving(false);
    }
  };

  // ── Delete ───────────────────────────────────────────────────
  const openDeleteModal = (clinic) => {
    setClinicToDelete(clinic);
    setShowDeleteModal(true);
  };

  const handleDelete = async () => {
    try {
      await deleteClinic(clinicToDelete.id);
      toast.success('Clinic deleted successfully');
      setShowDeleteModal(false);
      fetchClinics();
    } catch (err) {
      toast.error('Failed to delete clinic');
    }
  };

  return (
    <div className={styles.pageContainer}>
      <Sidebar isOpen={isSidebarOpen} activePage="clinics" onLogout={handleLogout} />

      <main className={styles.main}>
        {/* Header */}
        <div className={styles.header}>
          <div className={styles.headerLeft}>
            <button className={styles.menuBtn} onClick={() => setIsSidebarOpen(o => !o)}>
              <Menu size={20} />
            </button>
            <div>
              <h1 className={styles.pageTitle}>Clinics</h1>
              <p className={styles.pageSubtitle}>Manage clinic locations</p>
            </div>
          </div>
          <button className={styles.addBtn} onClick={openAddModal}>
            <Plus size={18} />
            Add Clinic
          </button>
        </div>

        {/* Table */}
        <div className={styles.tableCard}>
          {isLoading ? (
            <div className={styles.emptyState}>Loading clinics...</div>
          ) : clinics.length === 0 ? (
            <div className={styles.emptyState}>No clinics found. Add one to get started.</div>
          ) : (
            <table className={styles.table}>
              <thead>
                <tr>
                  <th>Name</th>
                  <th>Location</th>
                  <th className={styles.actionsCol}>Actions</th>
                </tr>
              </thead>
              <tbody>
                {clinics.map((clinic) => (
                  <tr key={clinic.id}>
                    <td>{clinic.name}</td>
                    <td>{clinic.location}</td>
                    <td className={styles.actionsCell}>
                      <button className={styles.editBtn} onClick={() => openEditModal(clinic)} title="Edit">
                        <Pencil size={15} />
                        Edit
                      </button>
                      <button className={styles.deleteBtn} onClick={() => openDeleteModal(clinic)} title="Delete">
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
            <h2 className={styles.modalTitle}>{isEditing ? 'Edit Clinic' : 'Add Clinic'}</h2>
            {isEditing && (
              <div className={styles.formGroup}>
                <label>Clinic ID</label>
                <input
                  type="text"
                  value={selectedClinic?.id ?? ''}
                  readOnly
                  className={styles.readOnlyInput}
                />
              </div>
            )}
            <div className={styles.formGroup}>
              <label>Name</label>
              <input
                type="text"
                value={formData.name}
                onChange={e => setFormData(f => ({ ...f, name: e.target.value }))}
                placeholder="Enter clinic name"
              />
            </div>
            <div className={styles.formGroup}>
              <label>Location (Optional)</label>
              <input
                type="text"
                value={formData.location}
                onChange={e => setFormData(f => ({ ...f, location: e.target.value }))}
                placeholder="Enter clinic location"
              />
            </div>
            <div className={styles.modalActions}>
              <button className={styles.cancelBtn} onClick={() => setShowModal(false)}>Cancel</button>
              <button className={styles.saveBtn} onClick={handleSave} disabled={isSaving}>
                {isSaving ? 'Saving...' : isEditing ? 'Update' : 'Add Clinic'}
              </button>
            </div>
          </div>
        </div>
      )}

      {/* Delete Confirm Modal */}
      {showDeleteModal && (
        <div className={styles.modalOverlay} onClick={() => setShowDeleteModal(false)}>
          <div className={styles.modal} onClick={e => e.stopPropagation()}>
            <h2 className={styles.modalTitle}>Delete Clinic</h2>
            <p className={styles.deleteMsg}>
              Are you sure you want to delete <strong>{clinicToDelete?.name}</strong>? This action cannot be undone.
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
