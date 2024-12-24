import { useEffect, useState } from 'react';

export const AdditionalServices = ({cutValue}) => {
    const [additionalServices, setAdditionalServices] = useState([]);

    const fetchServices = async () => {
        try {
            const response = await fetch('http://billing-app-server:5183/extras');
            if (!response.ok) {
                throw new Error('Ошибка при загрузке данных');
            }
            const data = await response.json();
            setAdditionalServices(data);
        } catch (error) {
            console.error('Ошибка:', error);
        }
    };

    const handleServicePurchase = async (serviceId, serviceTitle) => {
        const token = localStorage.getItem('token');
        try {
            const response = await fetch(`http://billing-app-server:5183/subscribers/extras/add/${serviceId}`, {
                method: 'POST',
                headers: {
                    'Accept': '*/*',
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${token}`
                }
            });
            if (!response.ok) {
                throw new Error('Ошибка при покупке услуги');
            }
            const result = await response.json();
            alert(`Услуга "${serviceTitle}" успешно подключена!`);
            console.log(result);
        } catch (error) {
            console.error('Ошибка при подключении услуги:', error);
        }
    };

    const parseAdditionalDesc = (description) => {
        return description.split(' ');
    };

    useEffect(() => {
        fetchServices();
    }, []);

    const latestServices = additionalServices.slice(-cutValue);

    return (
        <div className="additional-services">
            {latestServices.map((service) => (
                <div className="service-card" key={service.id}>
                    <span className="service-card-title"> +{parseAdditionalDesc(service.description)[0]}</span>
                    <span className="service-card-desc">{parseAdditionalDesc(service.description)[1] }</span>
                    <button
                        className="service-price"
                        onClick={() => handleServicePurchase(service.id, service.title)}>
                        {service.price}₽
                    </button>
                </div>
            ))}
        </div>
    );
};