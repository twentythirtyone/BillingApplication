import { useEffect, useState } from 'react';
import { fetchExtras, deleteExtra } from './extras-api.jsx';
import { ExtrasFormModal } from './extras-modal';
import deleteIcon from '../../../assets/img/delete.svg';
import editIcon from '../../../assets/img/edit.svg';
import ConfirmModal from './confirm-modal';

export const ExtrasTable = () => {
  const [extras, setExtras] = useState([]);
  const [selectedExtra, setSelectedExtra] = useState(null);
  const [showModal, setShowModal] = useState(false);
  const [showConfirm, setShowConfirm] = useState(false);
  const [deleteId, setDeleteId] = useState(null);
  const [loading, setLoading] = useState(true);


  const authToken = localStorage.getItem('token');
  useEffect(() => {
    if (!authToken) return; // Ждем, пока появится токен

    const fetchData = async () => {
      setLoading(true); // Включаем индикатор загрузки
      try {
        const response = await fetchExtras(authToken);
        setExtras(response.data);
      } catch (error) {
        console.error('Error fetching extras:', error);
      } finally {
        setLoading(false); // Выключаем индикатор загрузки
      }
    };

    fetchData(); // Первичная загрузка данных
    const intervalId = setInterval(fetchData, 5000); // Обновление каждые 5 секунд

    return () => clearInterval(intervalId); // Очистка
  }, [authToken]);

  const handleDelete = () => {
    deleteExtra(deleteId)
      .then(() => {
        setExtras((prev) => prev.filter((extra) => extra.id !== deleteId));
        setShowConfirm(false);
      })
      .catch((error) => console.error('Error deleting extra:', error));
  };

  const handleAddClick = () => {
    setShowModal(true);
  };

  return (
    <div>
      <h1>Дополнительные услуги</h1>
      <table>
        <thead className="tariffs-heading">
          <tr>
            <th>Название</th>
            <th>Цена</th>
            <th>Интернет (МБ)</th>
            <th>Звонки</th>
            <th>SMS</th>
            <th style={{ color: '#8596AC' }}>Редактировать</th>
            <th style={{ color: '#8596AC' }}>Удалить</th>
          </tr>
        </thead>
        <tbody style={{ color: '#8596AC' }}>
          {extras.map((extra) => (
            <tr key={extra.id}>
              <td style={{ color: '#fff' }}>{extra.title || '—'}</td>
              <td>{extra.price ? `${extra.price}₽` : '0'}</td>
              {extra.title !== 'Стандартный' && (
                <>
                  <td>
                    <button className="table-buttons">
                      <img
                        src={editIcon}
                        onClick={() => {
                          setSelectedExtra(extra);
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
                          setDeleteId(extra.id);
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
        <ExtraFormModal
          tariff={selectedExtra}
          onClose={() => setShowModal(false)}
          onSave={(updatedTariff) => {
            if (selectedExtra) {
              setExtras((prev) =>
                prev.map((t) => (t.id === updatedTariff.id ? updatedTariff : t))
              );
            } else {
              setExtras((prev) => [...prev, updatedTariff]);
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
    </div>
  );
};