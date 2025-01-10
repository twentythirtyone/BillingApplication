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
        bundleId: extra?.bundle?.id || 0,
        callTime: extra?.bundle?.callTime || '',
        messages: extra?.bundle?.messages || 0,
        internet: extra?.bundle?.internet / 1024 || 0,
        bundleId: 0,
    });

    const [serviceType, setServiceType] = useState('internet');

    const handleServiceChange = (e) => {
        setServiceType(e.target.value);
        // Сброс значений при смене типа
        setFormData((prevData) => ({
            ...prevData,
            callTime: '',
            messages: 0,
            internet: 0,
        }));
    };

    const handleSubmit = (e) => {
        e.preventDefault();

        const updatedExtra = {
          tariff: {
              id: formData.id,
              title: formData.title,
              description: formData.description,
              price: formData.price,
              bundle: {
                  id: formData.bundleId,
                  callTime: serviceType === 'callTime' ? formData.callTime : '',
                  messages: serviceType === 'messages' ? formData.messages : 0,
                  internet: serviceType === 'internet' ? formData.internet * 1024 : 0,
              },
          },
          bundleId: formData.bundleId,
      };

      const apiCall = extra ? updateExtra : addExtra;
      apiCall(updatedExtra)
          .then((response) => {
              onSave(response.data);
              onClose();
          })
          .catch((error) => console.error('Ошибка сохранения доп.услуги:', error));
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
                <label>
                    Тип услуги:
                </label>
                <select value={serviceType} onChange={handleServiceChange}>
                    <option value="internet">Интернет</option>
                    <option value="callTime">Минуты</option>
                    <option value="messages">СМС</option>
                </select>
                {serviceType === 'internet' && (
                    <label>
                        Интернет (ГБ):
                        <input
                            type="number"
                            value={formData.internet}
                            onChange={(e) => setFormData({ ...formData, internet: +e.target.value })}
                        />
                    </label>
                )}
                {serviceType === 'callTime' && (
                    <label>
                        Звонки:
                        <TimeField
                            value={formData.callTime}
                            onChange={(e) => setFormData({ ...formData, callTime: e.target.value })}
                            showSeconds
                        />
                    </label>
                )}
                {serviceType === 'messages' && (
                    <label>
                        СМС:
                        <input
                            type="number"
                            value={formData.messages}
                            onChange={(e) => setFormData({ ...formData, messages: +e.target.value })}
                        />
                    </label>
                )}
                <div className="tariff-modal-buttons">
                    <button type="submit">Сохранить</button>
                    <button type="button" onClick={onClose}>Отмена</button>
                </div>
            </form>
        </div>
    );
};