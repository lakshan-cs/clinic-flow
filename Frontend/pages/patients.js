import React, { useState, useEffect } from 'react';
import { useRouter } from 'next/router';
import { Pencil, Trash2, Plus, Menu, X, Search } from 'lucide-react';
import { toast } from 'react-toastify';
import Sidebar from '../components/Sidebar';
import { getAllPatients, createPatient, updatePatient, deletePatient } from '../services/patientService';
import { getAllAllergies } from '../services/allergyService';
import { getAllergysByPatientId } from '../services/patientAllergyService';
import styles from '../styles/Patients.module.css';

const SEVERITY_OPTIONS = ['MILD', 'MODERATE', 'SEVERE'];

const emptyForm = () => ({
  fullName: '',
  dateOfBirth: '',
  email: '',
  phoneNumber: '',
  allergies: [],
});

export default function Patients() {
  const router = useRouter();
  const [isSidebarOpen, setIsSidebarOpen] = useState(true);
  const [patients, setPatients] = useState([]);
  const [allAllergies, setAllAllergies] = useState([]);
  const [isLoading, setIsLoading] = useState(true);

  // Modal state
  const [showModal, setShowModal] = useState(false);
  const [isEditing, setIsEditing] = useState(false);
  const [selectedPatient, setSelectedPatient] = useState(null);
  const [formData, setFormData] = useState(emptyForm());
  const [isSaving, setIsSaving] = useState(false);

  // Delete confirm state
  const [showDeleteModal, setShowDeleteModal] = useState(false);
  const [patientToDelete, setPatientToDelete] = useState(null);
  const [isDeleting, setIsDeleting] = useState(false);
  const [searchQuery, setSearchQuery] = useState('');

  useEffect(() => {
    fetchPatients();
    fetchAllergies();
  }, []);

  const fetchPatients = async () => {
    try {
      setIsLoading(true);
      const data = await getAllPatients();
      setPatients(data);
    } catch (err) {
      toast.error('Failed to load patients');
    } finally {
      setIsLoading(false);
    }
  };

  const fetchAllergies = async () => {
    try {
      const data = await getAllAllergies();
      setAllAllergies(data);
    } catch (err) {
      toast.error('Failed to load allergy list');
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
    setSelectedPatient(null);
    setFormData(emptyForm());
    setShowModal(true);
  };

  const openEditModal = async (patient) => {
    setIsEditing(true);
    setSelectedPatient(patient);
    let existingAllergies = [];
    try {
      const data = await getAllergysByPatientId(patient.id);
      existingAllergies = data.map(pa => ({
        allergyId: pa.allergyId,
        severity: pa.severity || 'MILD',
        notes: pa.notes || '',
      }));
    } catch (err) {
      toast.error('Failed to load patient allergies');
    }
    setFormData({
      fullName: patient.fullName || '',
      dateOfBirth: patient.dateOfBirth ? patient.dateOfBirth.split('T')[0] : '',
      email: patient.email || '',
      phoneNumber: patient.phoneNumber || '',
      allergies: existingAllergies,
    });
    setShowModal(true);
  };

  const handleFieldChange = (field, value) => {
    setFormData(f => ({ ...f, [field]: value }));
  };

  // ── Allergy management inside form ─────────────────────────
  const handleAddAllergy = (allergyId) => {
    const id = parseInt(allergyId, 10);
    if (!id) return;
    if (formData.allergies.find(a => a.allergyId === id)) return; // already selected
    setFormData(f => ({
      ...f,
      allergies: [...f.allergies, { allergyId: id, severity: 'MILD', notes: '' }],
    }));
  };

  const handleRemoveAllergy = (allergyId) => {
    setFormData(f => ({
      ...f,
      allergies: f.allergies.filter(a => a.allergyId !== allergyId),
    }));
  };

  const handleAllergyFieldChange = (allergyId, field, value) => {
    setFormData(f => ({
      ...f,
      allergies: f.allergies.map(a =>
        a.allergyId === allergyId ? { ...a, [field]: value } : a
      ),
    }));
  };

  const getAllergyName = (id) => {
    const found = allAllergies.find(a => a.id === id);
    return found ? found.allergyType : `Allergy #${id}`;
  };

  // ── Validation ──────────────────────────────────────────────
  const validate = () => {
    if (!formData.fullName.trim()) { toast.warning('Full Name is required'); return false; }
    if (!formData.dateOfBirth) { toast.warning('Date of Birth is required'); return false; }
    if (!formData.email.trim()) { toast.warning('Email is required'); return false; }
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (!emailRegex.test(formData.email)) { toast.warning('Please enter a valid email address'); return false; }
    if (!formData.phoneNumber.trim()) { toast.warning('Phone Number is required'); return false; }
    const phoneRegex = /^\+?[\d\s\-().]{7,15}$/;
    if (!phoneRegex.test(formData.phoneNumber.trim())) { toast.warning('Please enter a valid phone number'); return false; }
    return true;
  };

  const handleSave = async () => {
    if (!validate()) return;
    try {
      setIsSaving(true);
      const payload = {
        fullName: formData.fullName,
        dateOfBirth: formData.dateOfBirth,
        email: formData.email,
        phoneNumber: formData.phoneNumber,
        allergies: formData.allergies,
      };
      if (isEditing) {
        await updatePatient({ ...payload, id: selectedPatient.id });
        toast.success('Patient updated successfully');
      } else {
        await createPatient(payload);
        toast.success('Patient added successfully');
      }
      setShowModal(false);
      fetchPatients();
    } catch (err) {
      const message = err.response?.data?.error || 'Failed to save patient';
      toast.error(message);
    } finally {
      setIsSaving(false);
    }
  };

  // ── Delete ───────────────────────────────────────────────────
  const openDeleteModal = (patient) => {
    setPatientToDelete(patient);
    setShowDeleteModal(true);
  };

  const handleDelete = async () => {
    try {
      setIsDeleting(true);
      await deletePatient(patientToDelete.id);
      toast.success('Patient deleted successfully');
      setShowDeleteModal(false);
      fetchPatients();
    } catch (err) {
      const message = err.response?.data?.error || 'Failed to delete patient';
      toast.error(message);
    } finally {
      setIsDeleting(false);
    }
  };

  // ── Available allergies for dropdown (not yet selected) ─────
  const availableAllergies = allAllergies.filter(
    a => !formData.allergies.find(sel => sel.allergyId === a.id)
  );

  const filteredPatients = patients.filter(p => {
    const q = searchQuery.toLowerCase();
    if (!q) return true;
    return (
      (p.fullName || '').toLowerCase().includes(q) ||
      (p.email || '').toLowerCase().includes(q) ||
      (p.phoneNumber || '').toLowerCase().includes(q) ||
      (p.dateOfBirth || '').toLowerCase().includes(q)
    );
  });

  return (
    <div className={styles.pageContainer}>
      <Sidebar isOpen={isSidebarOpen} activePage="patients" onLogout={handleLogout} />

      <main className={styles.main}>
        {/* Header */}
        <div className={styles.header}>
          <div className={styles.headerLeft}>
            <button className={styles.menuBtn} onClick={() => setIsSidebarOpen(o => !o)}>
              <Menu size={20} />
            </button>
            <div>
              <h1 className={styles.pageTitle}>Patients</h1>
              <p className={styles.pageSubtitle}>Manage patient records</p>
            </div>
          </div>
          <div className={styles.headerRight}>
            <div className={styles.searchBar}>
              <Search size={16} className={styles.searchIcon} />
              <input
                type="text"
                placeholder="Search patients..."
                value={searchQuery}
                onChange={e => setSearchQuery(e.target.value)}
                className={styles.searchInput}
              />
            </div>
            <button className={styles.addBtn} onClick={openAddModal}>
              <Plus size={18} />
              Add Patient
            </button>
          </div>
        </div>

        {/* Table */}
        <div className={styles.tableCard}>
          {isLoading ? (
            <div className={styles.emptyState}>Loading patients...</div>
          ) : filteredPatients.length === 0 ? (
            <div className={styles.emptyState}>{patients.length === 0 ? 'No patients found. Add one to get started.' : 'No patients match your search.'}</div>
          ) : (
            <table className={styles.table}>
              <thead>
                <tr>
                  <th>Full Name</th>
                  <th>Date of Birth</th>
                  <th>Email</th>
                  <th>Phone Number</th>
                  <th className={styles.actionsCol}>Actions</th>
                </tr>
              </thead>
              <tbody>
                {filteredPatients.map((patient) => (
                  <tr key={patient.id}>
                    <td>{patient.fullName}</td>
                    <td>{patient.dateOfBirth ? patient.dateOfBirth.split('T')[0] : ''}</td>
                    <td>{patient.email}</td>
                    <td>{patient.phoneNumber}</td>
                    <td className={styles.actionsCell}>
                      <button className={styles.editBtn} onClick={() => openEditModal(patient)} title="Edit">
                        <Pencil size={15} />
                        Edit
                      </button>
                      <button className={styles.deleteBtn} onClick={() => openDeleteModal(patient)} title="Delete">
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
            <h2 className={styles.modalTitle}>{isEditing ? 'Edit Patient' : 'Add Patient'}</h2>

            {isEditing && (
              <div className={styles.formGroup}>
                <label>Patient ID</label>
                <input
                  type="text"
                  value={selectedPatient?.id ?? ''}
                  readOnly
                  className={styles.readOnlyInput}
                />
              </div>
            )}

            <div className={styles.formGroup}>
              <label>Full Name *</label>
              <input
                type="text"
                value={formData.fullName}
                onChange={e => handleFieldChange('fullName', e.target.value)}
                placeholder="Enter full name"
              />
            </div>

            <div className={styles.formGroup}>
              <label>Date of Birth *</label>
              <input
                type="date"
                value={formData.dateOfBirth}
                onChange={e => handleFieldChange('dateOfBirth', e.target.value)}
              />
            </div>

            <div className={styles.formGroup}>
              <label>Email *</label>
              <input
                type="email"
                value={formData.email}
                onChange={e => handleFieldChange('email', e.target.value)}
                placeholder="Enter email address"
              />
            </div>

            <div className={styles.formGroup}>
              <label>Phone Number *</label>
              <input
                type="tel"
                value={formData.phoneNumber}
                onChange={e => handleFieldChange('phoneNumber', e.target.value)}
                placeholder="Enter phone number"
              />
            </div>

            {/* Allergies Section */}
            <h3 className={styles.sectionTitle}>Allergies</h3>

            {availableAllergies.length > 0 && (
              <div className={styles.formGroup}>
                <label>Add Allergy</label>
                <select
                  className={styles.allergyDropdown}
                  value=""
                  onChange={e => handleAddAllergy(e.target.value)}
                >
                  <option value="">-- Select an allergy --</option>
                  {availableAllergies.map(a => (
                    <option key={a.id} value={a.id}>{a.allergyType}</option>
                  ))}
                </select>
              </div>
            )}

            {formData.allergies.length > 0 && (
              <div className={styles.selectedAllergiesList}>
                {formData.allergies.map(selected => (
                  <div key={selected.allergyId} className={styles.allergyCard}>
                    <div className={styles.allergyCardHeader}>
                      <span className={styles.allergyCardName}>
                        {getAllergyName(selected.allergyId)}
                      </span>
                      <button
                        className={styles.removeAllergyBtn}
                        onClick={() => handleRemoveAllergy(selected.allergyId)}
                        title="Remove allergy"
                      >
                        <X size={16} />
                      </button>
                    </div>
                    <div className={styles.allergyCardFields}>
                      <div className={styles.allergyCardField}>
                        <label>Severity</label>
                        <select
                          value={selected.severity}
                          onChange={e => handleAllergyFieldChange(selected.allergyId, 'severity', e.target.value)}
                        >
                          {SEVERITY_OPTIONS.map(s => (
                            <option key={s} value={s}>{s}</option>
                          ))}
                        </select>
                      </div>
                      <div className={styles.allergyCardField}>
                        <label>Notes</label>
                        <input
                          type="text"
                          value={selected.notes}
                          onChange={e => handleAllergyFieldChange(selected.allergyId, 'notes', e.target.value)}
                          placeholder="Optional notes"
                        />
                      </div>
                    </div>
                  </div>
                ))}
              </div>
            )}

            <div className={styles.modalActions}>
              <button className={styles.cancelBtn} onClick={() => setShowModal(false)}>Cancel</button>
              <button className={styles.saveBtn} onClick={handleSave} disabled={isSaving}>
                {isSaving ? 'Saving...' : isEditing ? 'Update' : 'Add Patient'}
              </button>
            </div>
          </div>
        </div>
      )}

      {/* Delete Confirm Modal */}
      {showDeleteModal && (
        <div className={styles.modalOverlay} onClick={() => setShowDeleteModal(false)}>
          <div className={styles.modal} onClick={e => e.stopPropagation()}>
            <h2 className={styles.modalTitle}>Delete Patient</h2>
            <p className={styles.deleteMsg}>
              Are you sure you want to delete <strong>{patientToDelete?.fullName}</strong>?
              This action cannot be undone.
            </p>
            <div className={styles.modalActions}>
              <button className={styles.cancelBtn} onClick={() => setShowDeleteModal(false)}>Cancel</button>
              <button className={styles.confirmDeleteBtn} onClick={handleDelete} disabled={isDeleting}>
                {isDeleting ? 'Deleting...' : 'Delete'}
              </button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
}
