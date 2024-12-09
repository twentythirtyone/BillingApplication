import { useState } from 'react';
import { addTariff, updateTariff } from './tariff-api.jsx';
import TimeField from 'react-simple-timefield';

export const TariffFormModal = ({ tariff, onClose, onSave }) => {
    const [formData, setFormData] = useState({
      id: tariff?.id || 0,
      title: tariff?.title || '',
      description: tariff?.description || '',
      price: tariff?.price || 0,
      internet: tariff?.bundle?.internet || 0,
      callTime: tariff?.bundle?.callTime || '',
      messages: tariff?.bundle?.messages || 0,
      bundleId: tariff?.bundle?.id || 0,
    });
  
    const handleCallTimeChange = (value) => {
      setFormData({ ...formData, callTime: value });
    };
  
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
  
      const apiCall = tariff ? updateTariff : addTariff;
      apiCall(updatedTariff)
        .then((response) => {
          onSave(response.data);
          onClose();
        })
        .catch((error) => console.error('Error saving tariff:', error));
    };
  
    return (
      <div className="modal">
        <form onSubmit={handleSubmit}>
            <label>
            ID:
            <input
              type="number"
              value={formData.id}
              onChange={(e) => setFormData({ ...formData, id: e.target.value })}
            />
          </label>
          <label>
            Название:
            <input
              type="text"
              value={formData.title}
              onChange={(e) => setFormData({ ...formData, title: e.target.value })}
            />
          </label>
          <label>
            Цена:
            <input
              type="number"
              value={formData.price}
              onChange={(e) => setFormData({ ...formData, price: +e.target.value })}
            />
          </label>
          <label>
            Интернет:
            <input
              type="number"
              value={formData.internet}
              onChange={(e) => setFormData({ ...formData, internet: e.target.value })}
            />
          </label>
          <label>
            Звонки:
            <TimeField
              value={formData.callTime}
              onChange={(e) => handleCallTimeChange(e.target.value)}
              style={{
                width: '390px',
                padding: '5px',
                border: 'none',
                borderRadius: '4px',
                fontSize: '14px',
              }}
              showSeconds
            />
          </label>
          <label>
            SMS:
            <input
              type="number"
              value={formData.messages}
              onChange={(e) => setFormData({ ...formData, messages: +e.target.value })}
            />
          </label>
          <div className="tariff-modal-buttons">
            <button type="submit">Сохранить</button>
            <button type="button" onClick={onClose}>Отмена</button>
          </div>
        </form>
      </div>
    );
  };
