import React, { useState, useEffect } from 'react';
import { useRouter } from 'next/router';
import { Pencil, Trash2, Plus, Menu } from 'lucide-react';
import { toast } from 'react-toastify';
import Sidebar from '../components/Sidebar';
import { getAllAllergies, createAllergy, updateAllergy, deleteAllergy } from '../services/allergyService';
import styles from '../styles/Allergies.module.css';

export default function Allergies() {
  const router = useRouter();
  const [isSidebarOpen, setIsSidebarOpen] = useState(true);
  const [allergies, setAllergies] = useState([]);
  const [isLoading, setIsLoading] = useState(true);

  // Modal state
  const [showModal, setShowModal] = useState(false);
  const [isEditing, setIsEditing] = useState(false);
  const [selectedAllergy, setSelectedAllergy] = useState(null);
  const [formData, setFormData] = useState({ allergyType: '' });
  const [isSaving, setIsSaving] = useState(false);

  // Delete confirm state
  const [showDeleteModal, setShowDeleteModal] = useState(false);
  const [allergyToDelete, setAllergyToDelete] = useState(null);

  useEffect(() => {
    fetchAllergies();
  }, []);

  const fetchAllergies = async () => {
    try {
      setIsLoading(true);
      const data = await getAllAllergies();
      setAllergies(data);
    } catch (err) {
      toast.error('Failed to load allergies');
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
    setSelectedAllergy(null);
    setFormData({ allergyType: '' });
    setShowModal(true);
  };

  const openEditModal = (allergy) => {
    setIsEditing(true);
    setSelectedAllergy(allergy);
    setFormData({ allergyType: allergy.allergyType });
    setShowModal(true);
  };

  const handleSave = async () => {
    if (!formData.allergyType.trim()) {
      toast.warning('Allergy type is required');
      return;
    }
    try {
      setIsSaving(true);
      if (isEditing) {
        await updateAllergy({ allergyType: formData.allergyType, id: selectedAllergy.id });
        toast.success('Allergy updated successfully');
      } else {
        await createAllergy({ allergyType: formData.allergyType });
        toast.success('Allergy added successfully');
      }
      setShowModal(false);
      fetchAllergies();
    } catch (err) {
      toast.error(isEditing ? 'Failed to update allergy' : 'Failed to add allergy');
    } finally {
      setIsSaving(false);
    }
  };

  // ── Delete ───────────────────────────────────────────────────
  const openDeleteModal = (allergy) => {
    setAllergyToDelete(allergy);
    setShowDeleteModal(true);
  };

  const handleDelete = async () => {
    try {
      await deleteAllergy(allergyToDelete.id);
      toast.success('Allergy deleted successfully');
      setShowDeleteModal(false);
      fetchAllergies();
    } catch (err) {
      toast.error('Failed to delete allergy');
    }
  };

  return (
    <div className={styles.pageContainer}>
      <Sidebar isOpen={isSidebarOpen} activePage="allergies" onLogout={handleLogout} />

      <main className={styles.main}>
        {/* Header */}
        <div className={styles.header}>
          <div className={styles.headerLeft}>
            <button className={styles.menuBtn} onClick={() => setIsSidebarOpen(o => !o)}>
              <Menu size={20} />
            </button>
            <div>
              <h1 className={styles.pageTitle}>Allergies</h1>
              <p className={styles.pageSubtitle}>Manage allergy types</p>
            </div>
          </div>
          <button className={styles.addBtn} onClick={openAddModal}>
            <Plus size={18} />
            Add Allergy
          </button>
        </div>

        {/* Table */}
        <div className={styles.tableCard}>
          {isLoading ? (
            <div className={styles.emptyState}>Loading allergies...</div>
          ) : allergies.length === 0 ? (
            <div className={styles.emptyState}>No allergies found. Add one to get started.</div>
          ) : (
            <table className={styles.table}>
              <thead>
                <tr>
                  <th>Allergy Type</th>
                  <th className={styles.actionsCol}>Actions</th>
                </tr>
              </thead>
              <tbody>
                {allergies.map((allergy) => (
                  <tr key={allergy.id}>
                    <td>{allergy.allergyType}</td>
                    <td className={styles.actionsCell}>
                      <button className={styles.editBtn} onClick={() => openEditModal(allergy)} title="Edit">
                        <Pencil size={15} />
                        Edit
                      </button>
                      <button className={styles.deleteBtn} onClick={() => openDeleteModal(allergy)} title="Delete">
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
            <h2 className={styles.modalTitle}>{isEditing ? 'Edit Allergy' : 'Add Allergy'}</h2>
            {isEditing && (
              <div className={styles.formGroup}>
                <label>Allergy ID</label>
                <input
                  type="text"
                  value={selectedAllergy?.id ?? ''}
                  readOnly
                  className={styles.readOnlyInput}
                />
              </div>
            )}
            <div className={styles.formGroup}>
              <label>Allergy Type *</label>
              <input
                type="text"
                value={formData.allergyType}
                onChange={e => setFormData({ allergyType: e.target.value })}
                placeholder="e.g. Penicillin, Peanuts, Latex"
              />
            </div>
            <div className={styles.modalActions}>
              <button className={styles.cancelBtn} onClick={() => setShowModal(false)}>Cancel</button>
              <button className={styles.saveBtn} onClick={handleSave} disabled={isSaving}>
                {isSaving ? 'Saving...' : isEditing ? 'Update' : 'Add Allergy'}
              </button>
            </div>
          </div>
        </div>
      )}

      {/* Delete Confirm Modal */}
      {showDeleteModal && (
        <div className={styles.modalOverlay} onClick={() => setShowDeleteModal(false)}>
          <div className={styles.modal} onClick={e => e.stopPropagation()}>
            <h2 className={styles.modalTitle}>Delete Allergy</h2>
            <p className={styles.deleteMessage}>
              Are you sure you want to delete <strong>{allergyToDelete?.allergyType}</strong>? This action cannot be undone.
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
