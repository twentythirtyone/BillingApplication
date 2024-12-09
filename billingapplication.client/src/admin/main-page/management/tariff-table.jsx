import { useEffect, useState } from 'react';
import { fetchTariffs, deleteTariff } from './tariff-api.jsx';
import { TariffFormModal } from './tariff-modal';
import deleteIcon from '../../../assets/img/delete.svg';
import editIcon from '../../../assets/img/edit.svg';
import ConfirmModal from './confirm-modal';

export const TariffTable = () => {
  const [tariffs, setTariffs] = useState([]);
  const [selectedTariff, setSelectedTariff] = useState(null);
  const [showModal, setShowModal] = useState(false);
  const [showConfirm, setShowConfirm] = useState(false);
  const [deleteId, setDeleteId] = useState(null);

  useEffect(() => {
    fetchTariffs()
      .then((response) => setTariffs(response.data))
      .catch((error) => console.error('Error fetching tariffs:', error));
  }, []);

  const handleDelete = () => {
    deleteTariff(deleteId)
      .then(() => {
        setTariffs(tariffs.filter((tariff) => tariff.id !== deleteId));
        setShowConfirm(false);
      })
      .catch((error) => console.error('Error deleting tariff:', error));
  };

  const handleAddClick = () => {
    setShowModal(true);
  };

  const handleCancel = () => {
    setShowModal(false);
  };

  return (
    <div>
        <h1>Тарифы</h1>
      <table>
        <thead className='tariffs-heading'>
          <tr>
            <th>ID</th>
            <th>Название</th>
            <th>Цена</th>
            <th>Интернет</th>
            <th>Звонки</th>
            <th>SMS</th>
            <th style={{color: '#8596AC'}}>Редактировать</th>
            <th style={{color: '#8596AC'}}>Удалить</th>
          </tr>
        </thead>
        <tbody style={{color: '#8596AC'}}>
          {tariffs.map((tariff) => (
            <tr key={tariff.id}>
              <td>{tariff.id}</td>
              <td style={{color: '#fff'}}>{tariff.title}</td>
              <td>{tariff.price}₽</td>
              <td>{tariff.bundle.internet}</td>
              <td>{tariff.bundle.callTime}</td>
              <td>{tariff.bundle.messages}</td>
              <td>
                <button className='table-buttons'> <img src={editIcon} onClick={() => {
                  setSelectedTariff(tariff);
                  setShowModal(true);
                }} /></button>
              </td>
              <td>
                <button className='table-buttons'> <img src={deleteIcon} onClick={() => {
                  setDeleteId(tariff.id);
                  setShowConfirm(true);
                }} /></button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>

      {!showModal && (<button className="add-new-tariff" onClick={handleAddClick}>
          Добавить новый план...
        </button>
      )}

      {showModal && (
        <TariffFormModal
          tariff={selectedTariff}
          onClose={() => setShowModal(false)}
          onSave={(updatedTariff) => {
            if (selectedTariff) {
              setTariffs(tariffs.map((t) => t.id === updatedTariff.id ? updatedTariff : t));
            } else {
              setTariffs([...tariffs, updatedTariff]);
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