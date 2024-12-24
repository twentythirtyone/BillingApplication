import { useEffect, useState } from 'react';
import { fetchTariffs, deleteTariff } from './managment-api.jsx';
import { TariffFormModal } from './tariff-modal';
import deleteIcon from '../../../assets/img/delete.svg';
import editIcon from '../../../assets/img/edit.svg';
import ConfirmModal from './confirm-modal';
import { ExtrasTable } from './extras-table.jsx';

export const TariffTable = () => {
  const [tariffs, setTariffs] = useState([]);
  const [selectedTariff, setSelectedTariff] = useState(null);
  const [showModal, setShowModal] = useState(false);
  const [showConfirm, setShowConfirm] = useState(false);
  const [deleteId, setDeleteId] = useState(null);
  const [loading, setLoading] = useState(true);


  const authToken = localStorage.getItem('token');
  useEffect(() => {
    document.title = "Управление услугами";
    if (!authToken) return; // Ждем, пока появится токен

    const fetchData = async () => {
      setLoading(true); // Включаем индикатор загрузки
      try {
        const response = await fetchTariffs(authToken);
        setTariffs(response.data);
      } catch (error) {
        console.error('Error fetching tariffs:', error);
      } finally {
        setLoading(false); // Выключаем индикатор загрузки
      }
    };

    fetchData(); // Первичная загрузка данных
    const intervalId = setInterval(fetchData, 5000); // Обновление каждые 5 секунд

    return () => clearInterval(intervalId); // Очистка
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
    setShowModal(true);
  };

  return (
    <div>
      <h1>Тарифы</h1>
      <table>
        <thead className="tariffs-heading">
          <tr>
            <th>Название</th>
            <th>Цена</th>
            <th>Интернет</th>
            <th>Звонки</th>
            <th>SMS</th>
            <th style={{ color: '#8596AC' }}>Редактировать</th>
            <th style={{ color: '#8596AC' }}>Удалить</th>
          </tr>
        </thead>
        <tbody style={{ color: '#8596AC' }}>
          {tariffs.map((tariff) => (
            <tr key={tariff.id}>
              <td style={{ color: '#fff' }}>{tariff.title || '—'}</td>
              <td>{tariff.price ? `${tariff.price}₽` : '0'}</td>
              <td>{tariff.bundle?.internet / 1024 || '0'}</td>
              <td>{tariff.bundle?.callTime || '0'}</td>
              <td>{tariff.bundle?.messages || '0'}</td>
              {tariff.title !== 'Стандартный' && (
                <>
                  <td>
                    <button className="table-buttons">
                      <img
                        src={editIcon}
                        onClick={() => {
                          setSelectedTariff(tariff);
                          setShowModal(true);
                        }}
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

      {!showModal && (
        <button className="add-new-tariff" onClick={handleAddClick}>
          Добавить новый план...
        </button>
      )}

      {showModal && (
        <TariffFormModal
          tariff={selectedTariff}
          onClose={() => setShowModal(false)}
          onSave={(updatedTariff) => {
            if (selectedTariff) {
              setTariffs((prev) =>
                prev.map((t) => (t.id === updatedTariff.id ? updatedTariff : t))
              );
            } else {
              setTariffs((prev) => [...prev, updatedTariff]);
            }
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
        <ExtrasTable />
    </div>
  );
};