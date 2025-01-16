/* eslint-disable react/prop-types */
import { useState } from 'react'; 
import { addExtra, updateExtra } from './managment-api.jsx';
import TimeField from 'react-simple-timefield';

 export const BaseExtrasFormModal = ({ initialData, onClose, onSave, isEditing }) => {
    const [formData, setFormData] = useState({
        id: initialData?.id || 0,
        title: initialData?.title || '',
        description: initialData?.description || '',
        price: initialData?.price || 0,
        bundleId: initialData?.bundle?.id || 0,
        callTime: initialData?.bundle?.callTime || '00:00:00',
        messages: initialData?.bundle?.messages || 0,
        internet: initialData?.bundle?.internet / 1024 || 0,
    });

    const [serviceType, setServiceType] = useState(
        initialData?.bundle?.internet ? 'internet' : 
        initialData?.bundle?.callTime !== '00:00:00' ? 'callTime' : 
        'messages'
    );

    const handleServiceChange = (e) => {
        setServiceType(e.target.value);
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
            id: formData.id,
            title: formData.title,
            description: formData.description,
            price: formData.price,
            bundle: {
                id: formData.bundleId,
                callTime: serviceType === 'callTime' ? formData.callTime : '00:00:00',
                messages: serviceType === 'messages' ? formData.messages : 0,
                internet: serviceType === 'internet' ? formData.internet * 1024 : 0,
            },
        };

        const apiCall = isEditing ? updateExtra : addExtra;
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
                <label>Название:</label>
                    <input
                        type="text"
                        value={formData.title}
                        onChange={(e) => setFormData({ ...formData, title: e.target.value })}
                    />

                <label>Описание:</label>
                    <input
                        type="text"
                        value={formData.description}
                        onChange={(e) => setFormData({ ...formData, description: e.target.value })}
                    />

                <label>Цена:</label>
                    <input
                        type="number"
                        value={formData.price}
                        onChange={(e) => setFormData({ ...formData, price: +e.target.value })}
                    /> 
                <div>
                    <label>Тип услуги:</label>
                        <select value={serviceType} onChange={handleServiceChange}>
                            <option value="internet">Интернет</option>
                            <option value="callTime">Минуты</option>
                            <option value="messages">СМС</option>
                        </select>
                </div>
                {serviceType === 'internet' && (
                    <>
                    <label>Интернет (ГБ):</label>
                        <input
                            type="number"
                            value={formData.internet}
                            onChange={(e) => setFormData({ ...formData, internet: +e.target.value })}
                        /></>
                )}
                {serviceType === 'callTime' && (
                    <>
                    <label>Звонки:</label>
                        <TimeField
                            value={formData.callTime}
                            onChange={(e) => setFormData({ ...formData, callTime: e.target.value })}
                            showSeconds
                        />
                    </>
                )}
                {serviceType === 'messages' && (
                    <>
                    <label>СМС:</label>
                        <input
                            type="number"
                            value={formData.messages}
                            onChange={(e) => setFormData({ ...formData, messages: +e.target.value })}
                        />
                    </>
                )}
                <div className="tariff-modal-buttons">
                    <button type="submit">Сохранить</button>
                    <button type="button" onClick={onClose}>Отмена</button>
                </div>
            </form>
        </div>
    );
};