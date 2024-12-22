import { useState } from 'react';
import { addExtra, updateExtra } from './managment-api.jsx';
import TimeField from 'react-simple-timefield';

export const ExtrasFormModal = ({ extra, onClose, onSave }) => {
    const [formData, setFormData] = useState({
      id: extra?.id || 0,
      title: extra?.title || '',
      description: extra?.description || '',
      package: extra?.package || '',
      price: extra?.price || 0,
      bundle: extra?.bundle || 0,
    });

    const handleSubmit = (e) => {
        e.preventDefault();

        const updatedTariff = {
            tariff: {
              id: formData.id,
              title: formData.title,
              description: formData.description,
              price: formData.price,
              bundle: {
                id: formData.bundleId,
                internet: formData.internet,
                callTime: formData.callTime,
                messages: formData.messages,
              },
            },
            bundleId: formData.bundleId,
          };

      const updatedExtra = {
        extra: {
          id: formData.id,
          title: formData.title,
          description: formData.description,
          package: formData.package,
          price: formData.price,
          bundle: {
                id: formData.bundleId,
                callTime: formData.callTime,
                messages: formData.messages,
                internet: formData.internet,
            }
        }
      };
  
      const apiCall = extra ? updateExtra : addExtra;
      apiCall(updatedExtra)
        .then((response) => {
          onSave(response.data);
          onClose();
        })
        .catch((error) => console.error('ошикба сохранения доп.услуги:', error));
    };
  
    return (
      <div className="modal">
        <form onSubmit={handleSubmit}>
          <label>
            Название:
          </label>
            <input
              type="text"
              value={formData.title}
              onChange={(e) => setFormData({ ...formData, title: e.target.value })}
            />
          <label>
            Описание:
          </label>
            <input
              type="text"
              value={formData.description}
              onChange={(e) => setFormData({ ...formData, description: e.target.value })}
            />
          <label>
            Цена:
          </label>
            <input
              type="number"
              value={formData.price}
              onChange={(e) => setFormData({ ...formData, price: +e.target.value })}
            />
          <div className="tariff-modal-buttons">
            <button type="submit">Сохранить</button>
            <button type="button" onClick={onClose}>Отмена</button>
          </div>
        </form>
      </div>
    );
  };