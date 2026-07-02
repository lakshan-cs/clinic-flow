import React, { useState, useEffect } from 'react';
import { X } from 'lucide-react';
import { toast } from 'react-toastify';
import { updateTask } from '../services/taskService';
import { getCurrentUser } from '../services/authService';
import styles from './EditTask.module.css';

const EditTask = ({ task, isOpen, onClose, onSave }) => {
  const [formData, setFormData] = useState({
    id: '',
    title: '',
    description: '',
    status: 'TODO',
    priority: 'MEDIUM',
    dueDate: ''
  });

  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState(null);

  // Update form data when task changes
  useEffect(() => {
    if (task) {
      setFormData({
        id: task.id,
        title: task.title,
        description: task.description,
        status: task.status || 'TODO',
        priority: task.priority || 'MEDIUM',
        dueDate: task.dueDate
      });
    }
  }, [task]);

  const handleInputChange = (e) => {
    const { name, value } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: value
    }));
    if (error) setError(null);
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setIsLoading(true);
    setError(null);

    try {
      const updatedTask = await updateTask(formData.id, {
        title: formData.title,
        description: formData.description,
        status: formData.status,
        priority: formData.priority,
        dueDate: formData.dueDate
      });

      toast.success('Task updated successfully!');
      onSave(updatedTask);
      onClose();
    } catch (err) {
      console.error('Error updating task:', err);
      const errorMessage = err.response?.data?.message || err.response?.data?.error || err.message || 'Failed to update task';
      setError(errorMessage);
      toast.error(errorMessage);
    } finally {
      setIsLoading(false);
    }
  };

  const handleClose = () => {
    onClose();
  };

  // Don't render if not open
  if (!isOpen) return null;

  return (
    <div className={styles.modalOverlay} onClick={handleClose}>
      <div className={styles.modalContent} onClick={(e) => e.stopPropagation()}>
        {/* Modal Header */}
        <div className={styles.modalHeader}>
          <h2 className={styles.modalTitle}>Edit Task</h2>
          <button className={styles.modalCloseBtn} onClick={handleClose}>
            <X size={24} />
          </button>
        </div>

        {/* Modal Body */}
        <form className={styles.modalForm} onSubmit={handleSubmit}>
          {/* Title Field */}
          <div className={styles.modalFormGroup}>
            <label htmlFor="edit-title" className={styles.modalFormLabel}>
              Task Title <span className={styles.required}>*</span>
            </label>
            <input
              type="text"
              id="edit-title"
              name="title"
              value={formData.title}
              onChange={handleInputChange}
              className={styles.modalFormInput}
              placeholder="Enter task title"
              required
            />
          </div>

          {/* Description Field */}
          <div className={styles.modalFormGroup}>
            <label htmlFor="edit-description" className={styles.modalFormLabel}>
              Description <span className={styles.required}>*</span>
            </label>
            <textarea
              id="edit-description"
              name="description"
              value={formData.description}
              onChange={handleInputChange}
              className={styles.modalFormTextarea}
              placeholder="Enter task description"
              rows="4"
              required
            />
          </div>

          {/* Status and Priority Row */}
          <div className={styles.modalFormRow}>
            {/* Status Field */}
            <div className={styles.modalFormGroup}>
              <label htmlFor="edit-status" className={styles.modalFormLabel}>
                Status <span className={styles.required}>*</span>
              </label>
              <select
                id="edit-status"
                name="status"
                value={formData.status}
                onChange={handleInputChange}
                className={styles.modalFormSelect}
                required
              >
                <option value="TODO">Pending (TODO)</option>
                <option value="IN_PROGRESS">In Progress</option>
                <option value="DONE">Done</option>
              </select>
            </div>

            {/* Priority Field */}
            <div className={styles.modalFormGroup}>
              <label htmlFor="edit-priority" className={styles.modalFormLabel}>
                Priority <span className={styles.required}>*</span>
              </label>
              <select
                id="edit-priority"
                name="priority"
                value={formData.priority}
                onChange={handleInputChange}
                className={styles.modalFormSelect}
                required
              >
                <option value="HIGH">High</option>
                <option value="MEDIUM">Medium</option>
                <option value="LOW">Low</option>
              </select>
            </div>
          </div>

          {/* Due Date Field */}
          <div className={styles.modalFormGroup}>
            <label htmlFor="edit-dueDate" className={styles.modalFormLabel}>
              Due Date <span className={styles.required}>*</span>
            </label>
            <input
              type="date"
              id="edit-dueDate"
              name="dueDate"
              value={formData.dueDate}
              onChange={handleInputChange}
              className={styles.modalFormInput}
              required
            />
          </div>

          {/* Modal Footer with Buttons */}
          <div className={styles.modalFooter}>
            {error && (
              <div className={styles.modalError}>
                {error}
              </div>
            )}
            <div>
              <button 
                type="button" 
                onClick={handleClose} 
                className={`${styles.modalBtn} ${styles.modalBtnClose}`}
                disabled={isLoading}
              >
                Cancel
              </button>
              <button 
                type="submit" 
                className={`${styles.modalBtn} ${styles.modalBtnSave}`}
                disabled={isLoading}
              >
                {isLoading ? 'Saving...' : 'Save Changes'}
              </button>
            </div>
          </div>
        </form>
      </div>
    </div>
  );
};

export default EditTask;
