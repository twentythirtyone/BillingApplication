import { useEffect, useState } from 'react';
import { fetchExtras, deleteExtra } from './managment-api.jsx';
import { BaseExtrasFormModal } from './extras-modal';
import deleteIcon from '../../../assets/img/delete.svg';
import editIcon from '../../../assets/img/edit.svg';
import ConfirmModal from './confirm-modal';
import axios from 'axios';

export const ExtrasTable = () => {
  const [extras, setExtras] = useState([]);
  const [selectedExtra, setSelectedExtra] = useState(null);
  const [showEditModal, setShowEditModal] = useState(false);
  const [showCreateModal, setShowCreateModal] = useState(false);
  const [showConfirm, setShowConfirm] = useState(false);
  const [deleteId, setDeleteId] = useState(null);
  const [isAdmin, setIsAdmin] = useState(false);

  const authToken = localStorage.getItem('token');

  const checkUserRole = async () => {
    try {
      const response = await axios.get('/billingapplication/auth/current', {
        headers: {
          Authorization: `Bearer ${authToken}`,
        },
      });

      const { roles } = response.data;
      setIsAdmin(roles.includes('Admin'));
    } catch (error) {
      console.error('Ошибка при проверке роли:', error);
    }
  };

  const fetchData = async () => {
    try {
      const response = await fetchExtras(authToken);
      const sortedData = response.data.sort((a, b) => a.id - b.id);
      setExtras(sortedData);
    } catch (error) {
      console.error('Error fetching extras:', error);
    }
  };

  useEffect(() => {
    fetchData();
    checkUserRole();
    const intervalId = setInterval(fetchData, 5000);

    return () => clearInterval(intervalId);
  }, [authToken]);

  const handleDelete = () => {
    deleteExtra(deleteId)
      .then(() => {
        setExtras((prev) => prev.filter((extra) => extra.id !== deleteId));
        setShowConfirm(false);
      })
      .catch((error) => console.error('Error deleting extra:', error));
  };

  const handleCreate = (newExtra) => {
    setExtras((prevExtras) => [...prevExtras, newExtra]);
    setShowCreateModal(false);
  };

  const handleEdit = (updatedExtra) => {
    setExtras((prevExtras) =>
      prevExtras.map((extra) =>
        extra.id === updatedExtra.id ? updatedExtra : extra
      )
    );
    setShowEditModal(false);
  };

  return (
    <div>
      <div className="custom-table-wrapper">
        <table className="custom-table">
          <thead>
            <tr>
              <th style={{ width: '50px' }}>ID</th>
              <th style={{ width: '190px' }}>Название</th>
              <th style={{ width: '140px' }}>Цена</th>
              <th style={{ width: '140px' }}>Услуга</th>
              <th style={{ width: '143px' }}>Объем</th>
              {isAdmin && <th style={{ color: '#8596AC' }}>Редактировать</th>}
              {isAdmin && <th style={{ color: '#8596AC' }}>Удалить</th>}
            </tr>
          </thead>
          <tbody style={{ color: '#8596AC' }}>
            {extras.map((extra) => {
              let serviceType = '—';
              let serviceVolume = '—';

              if (extra.bundle?.callTime && extra.bundle.callTime !== '00:00:00') {
                serviceType = 'Связь';
                serviceVolume = extra.bundle.callTime;
              } else if (extra.bundle?.internet) {
                serviceType = 'Интернет';
                serviceVolume = `${extra.bundle.internet / 1024} ГБ`;
              } else if (extra.bundle?.messages && extra.bundle.messages !== 0) {
                serviceType = 'SMS';
                serviceVolume = `${extra.bundle.messages} SMS`;
              }

              return (
                <tr key={extra.id}>
                  <td>{extra.id}</td>
                  <td style={{ color: '#fff' }}>{extra.title || '—'}</td>
                  <td>{extra.price ? `${extra.price}₽` : '0'}</td>
                  <td>{serviceType}</td>
                  <td>{serviceVolume}</td>
                  {isAdmin && (
                    <>
                      <td>
                        <button className="table-buttons">
                          <img
                            src={editIcon}
                            onClick={() => {
                              setSelectedExtra(extra);
                              setShowEditModal(true);
                            }}
                          />
                        </button>
                      </td>
                      <td>
                        <button className="table-buttons">
                          <img
                            src={deleteIcon}
                            onClick={() => {
                              setDeleteId(extra.id);
                              setShowConfirm(true);
                            }}
                          />
                        </button>
                      </td>
                    </>
                  )}
                </tr>
              );
            })}
          </tbody>
        </table>
      </div>

      {isAdmin && (
        <button
          className="add-new-tariff"
          onClick={() => setShowCreateModal(true)}
        >
          Добавить новую услугу...
        </button>
      )}

      {showCreateModal && (
        <BaseExtrasFormModal
          initialData={null}
          onClose={() => setShowCreateModal(false)}
          onSave={handleCreate}
          isEditing={false}
        />
      )}

      {showEditModal && selectedExtra && (
        <BaseExtrasFormModal
          initialData={selectedExtra}
          onClose={() => setShowEditModal(false)}
          onSave={handleEdit}
          isEditing={true}
        />
      )}

      {showConfirm && (
        <ConfirmModal
          message="Вы уверены, что хотите удалить услугу?"
          onConfirm={handleDelete}
          onCancel={() => setShowConfirm(false)}
        />
      )}
    </div>
  );
};