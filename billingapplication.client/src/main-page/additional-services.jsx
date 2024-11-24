import { useEffect, useState } from 'react';

export const AdditionalServices = ({cutValue}) => {
    const [additionalServices, setAdditionalServices] = useState([]);

    const manualOverrides = {
        4: { title: '+30', description: 'минут' },
        5: { title: '+50', description: 'SMS' },
        6: { title: '+100 +30', description: 'SMS и ГБ' },
    };

    const fetchServices = async () => {
        try {
            const response = await fetch('https://localhost:7262/extras');
            if (!response.ok) {
                throw new Error('Ошибка при загрузке данных');
            }
            const data = await response.json();

            const updatedServices = data.map((service) => {
                if (manualOverrides[service.id]) {
                    return { ...service, ...manualOverrides[service.id] };
                }
                return service;
            });

            setAdditionalServices(updatedServices);
        } catch (error) {
            console.error('Ошибка:', error);
        }
    };

    const handleServicePurchase = async (serviceId) => {
        const token = localStorage.getItem('token');
        try {
            const response = await fetch(`https://localhost:7262/subscriber/add/extra/${serviceId}`, {
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
            alert(`Услуга "${result.title}" успешно подключена!`);
            console.log(result);
        } catch (error) {
            console.error('Ошибка при подключении услуги:', error);
        }
    };

    useEffect(() => {
        fetchServices();
    }, []);

    const latestServices = additionalServices.slice(-cutValue);

    return (
        <div className="additional-services">
            {latestServices.map((service) => (
                <div className="service-card" key={service.id}>
                    <span className="service-card-top">{service.title}</span>
                    <span className="service-card-bottom">{service.description}</span>
                    <button
                        className="service-price"
                        onClick={() => handleServicePurchase(service.id)}>
                        {service.price}₽
                    </button>
                </div>
            ))}
        </div>
    );
};