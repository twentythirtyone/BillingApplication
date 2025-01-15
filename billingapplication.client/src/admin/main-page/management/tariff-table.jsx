import { useEffect, useState } from 'react';
import { fetchTariffs, deleteTariff } from './managment-api.jsx';
import { AddTariffModal, EditTariffModal } from './tariff-modal';
import deleteIcon from '../../../assets/img/delete.svg';
import editIcon from '../../../assets/img/edit.svg';
import ConfirmModal from './confirm-modal';
import { ExtrasTable } from './extras-table.jsx';
import axios from 'axios';

export const TariffTable = () => {
  const [tariffs, setTariffs] = useState([]);
  const [selectedTariff, setSelectedTariff] = useState(null);
  const [showAddModal, setShowAddModal] = useState(false); 
const [showEditModal, setShowEditModal] = useState(false); 
  const [showConfirm, setShowConfirm] = useState(false);
  const [deleteId, setDeleteId] = useState(null);
  const [view, setView] = useState('tariffs');
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
      const response = await fetchTariffs(authToken);
      const sortedData = response.data.sort((a, b) => a.id - b.id);
  
      setTariffs(sortedData);
    } catch (error) {
      console.error('Error fetching tariffs:', error);
    }
  };

  useEffect(() => {
    document.title = "Управление услугами";
    fetchData();
    checkUserRole();
    const intervalId = setInterval(fetchData, 5000);

    return () => clearInterval(intervalId);
  }, [authToken]);

  const handleDelete = () => {
    deleteTariff(deleteId)
      .then(() => {
        setTariffs((prev) => prev.filter((tariff) => tariff.id !== deleteId));
        setShowConfirm(false);
      })
      .catch((error) => console.error('Error deleting tariff:', error));
  };


  const handleAddClick = () => {
    setShowAddModal(true);
  };

  // Обработчик для редактирования
  const handleEditClick = (tariff) => {
      setSelectedTariff(tariff);
      setShowEditModal(true);
  };

  return (
    <div style={{width:'1000px'}}>
      <h1>Управление услугами</h1>
      <select
        value={view}
        onChange={(e) => setView(e.target.value)}
        className='tariff-table-select'
      >
        <option value="tariffs">Тарифы</option>
        <option value="extras">Дополнительные услуги</option>
      </select>
  
      {view === 'tariffs' ? (
        <div>
          <div className="custom-table-wrapper">
            <table className="custom-table">
              <thead>
                <tr>
                  <th style={{ width: '50px' }}>ID</th>
                  <th style={{ width: '180px' }}>Название</th>
                  <th style={{ width: '90px' }}>Цена</th>
                  <th style={{ width: '100px' }}>Интернет</th>
                  <th style={{ width: '120px' }}>Звонки</th>
                  <th style={{ width: '90px' }}>SMS</th>
                  {isAdmin && <th style={{ color: '#8596AC' }}>Редактировать</th>}
                  {isAdmin && <th style={{ color: '#8596AC' }}>Удалить</th>}
                </tr>
              </thead>
              <tbody style={{ color: '#8596AC' }}>
                {tariffs.map((tariff) => (
                  <tr key={tariff.id}>
                    <td>{tariff.id}</td>
                    <td style={{ color: '#fff' }}>{tariff.title || '—'}</td>
                    <td>{tariff.price ? `${tariff.price}₽` : '0'}</td>
                    <td>{tariff.bundle?.internet / 1024 || '0'}</td>
                    <td>{tariff.bundle?.callTime || '0'}</td>
                    <td>{tariff.bundle?.messages || '0'}</td>
                    {isAdmin && tariff.title !== 'Стандартный' && (
                      <>
                        <td>
                            <button className="table-buttons">
                                <img
                                    src={editIcon}
                                    onClick={() => handleEditClick(tariff)}
                                />
                            </button>
                        </td>
                        <td>
                          <button className="table-buttons">
                            <img
                              src={deleteIcon}
                              onClick={() => {
                                setDeleteId(tariff.id);
                                setShowConfirm(true);
                              }}
                            />
                          </button>
                        </td>
                      </>
                    )}
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
  
          { isAdmin && !showAddModal && (
            <button className="add-new-tariff" onClick={handleAddClick}>
              Добавить новый план...
            </button>
          )}
  
          {showAddModal && (
            <AddTariffModal
                onClose={() => setShowAddModal(false)}
                onSave={(newTariff) => {
                    setTariffs((prev) => [...prev, newTariff]);
                    fetchData();
                }}
            />
          )}

          {showEditModal && selectedTariff && (
              <EditTariffModal
                  tariff={selectedTariff}
                  onClose={() => {
                      setShowEditModal(false);
                      setSelectedTariff(null); // Очистка выбранного тарифа
                  }}
                  onSave={(updatedTariff) => {
                      setTariffs((prev) =>
                          prev.map((t) => (t.id === updatedTariff.id ? updatedTariff : t))
                      );
                      fetchData();
                  }}
              />
          )}
  
          {showConfirm && (
            <ConfirmModal
              message="Вы уверены, что хотите удалить тариф?"
              onConfirm={handleDelete}
              onCancel={() => setShowConfirm(false)}
            />
          )}
        </div>
      ) : (
        <>
          <ExtrasTable />
        </>
      )}
    </div>
  );
};