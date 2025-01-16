/* eslint-disable react/prop-types */
import { useState } from 'react';
import { addTariff, updateTariff } from './managment-api.jsx';
import TimeField from 'react-simple-timefield';

// Компонент для добавления нового тарифа
export const AddTariffModal = ({ onClose, onSave }) => {
    const [formData, setFormData] = useState({
        title: '',
        description: '',
        price: '',
        internet: '',
        callTime: '',
        messages: '',
    });

    const handleCallTimeChange = (value) => {
        setFormData({ ...formData, callTime: value });
    };

    const handleSubmit = (e) => {
        e.preventDefault();
        const newTariff = {
            tariff: {
                title: formData.title,
                description: formData.description,
                price: formData.price,
                bundle: {
                    internet: formData.internet * 1024,
                    callTime: formData.callTime,
                    messages: formData.messages,
                },
            },
        };

        addTariff(newTariff)
            .then((response) => {
                onSave(response.data);
                onClose();
            })
            .catch((error) => console.error('Error adding tariff:', error));
    };

    return (
        <div className="modal">
            <form onSubmit={handleSubmit}>
                <input
                    placeholder="Название"
                    type="text"
                    value={formData.title}
                    onChange={(e) => setFormData({ ...formData, title: e.target.value })}
                />
                <input
                    placeholder="Описание"
                    type="text"
                    value={formData.description}
                    onChange={(e) => setFormData({ ...formData, description: e.target.value })}
                />
                <input
                    placeholder="Цена"
                    type="number"
                    value={formData.price}
                    onChange={(e) => setFormData({ ...formData, price: +e.target.value })}
                />
                <input
                    placeholder="Интернет (ГБ)"
                    type="number"
                    value={formData.internet}
                    onChange={(e) => setFormData({ ...formData, internet: e.target.value })}
                />
                <TimeField
                    value={formData.callTime || ''}
                    onChange={(e) => handleCallTimeChange(e.target.value)}
                    input={
                        <input
                            placeholder="Звонки"
                            style={{
                                width: '389px',
                                padding: '7px',
                                border: 'none',
                                borderRadius: '12px',
                                fontSize: '14px',
                            }}
                        />
                    }
                    showSeconds
                />
                <input
                    placeholder="SMS"
                    type="number"
                    value={formData.messages}
                    onChange={(e) => setFormData({ ...formData, messages: +e.target.value })}
                />
                <div className="tariff-modal-buttons">
                    <button type="submit">Сохранить</button>
                    <button type="button" onClick={onClose}>Отмена</button>
                </div>
            </form>
        </div>
    );
};

// Компонент для редактирования тарифа
export const EditTariffModal = ({ tariff, onClose, onSave }) => {
    const [formData, setFormData] = useState({
        id: tariff.id,
        title: tariff.title,
        description: tariff.description,
        price: tariff.price,
        internet: tariff.bundle?.internet / 1024 || '',
        callTime: tariff.bundle?.callTime || '',
        messages: tariff.bundle?.messages || '',
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
                    internet: formData.internet * 1024,
                    callTime: formData.callTime,
                    messages: formData.messages,
                },
            },
        };

        updateTariff(updatedTariff)
            .then((response) => {
                onSave(response.data);
                onClose();
            })
            .catch((error) => console.error('Error updating tariff:', error));
    };

    return (
        <div className="modal modal-extra">
            <form onSubmit={handleSubmit}>
              <label>Название</label>
                <input
                    placeholder="Название"
                    type="text"
                    value={formData.title}
                    onChange={(e) => setFormData({ ...formData, title: e.target.value })}
                />
                 <label>Описание</label>
                <input
                    placeholder="Описание"
                    type="text"
                    value={formData.description}
                    onChange={(e) => setFormData({ ...formData, description: e.target.value })}
                />
                 <label>Цена</label>
                <input
                    placeholder="Цена"
                    type="number"
                    value={formData.price}
                    onChange={(e) => setFormData({ ...formData, price: +e.target.value })}
                />
                 <label>Интернет (ГБ)</label>
                <input
                    placeholder="Интернет (ГБ)"
                    type="number"
                    value={formData.internet}
                    onChange={(e) => setFormData({ ...formData, internet: e.target.value })}
                />
                 <label>Звонки</label>
                <TimeField
                    value={formData.callTime || ''}
                    onChange={(e) => handleCallTimeChange(e.target.value)}
                    input={
                        <input
                            placeholder="Звонки"
                            style={{
                                width: '389px',
                                padding: '7px',
                                border: 'none',
                                borderRadius: '12px',
                                fontSize: '14px',
                            }}
                        />
                    }
                    showSeconds
                />
                 <label>SMS</label>
                <input
                    placeholder="SMS"
                    type="number"
                    value={formData.messages}
                    onChange={(e) => setFormData({ ...formData, messages: +e.target.value })}
                />
                <div className="tariff-modal-buttons">
                    <button type="submit">Сохранить</button>
                    <button type="button" onClick={onClose}>Отмена</button>
                </div>
            </form>
        </div>
    );
};