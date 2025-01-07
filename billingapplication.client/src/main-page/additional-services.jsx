import { useEffect, useState } from 'react';
import {getTypeExtra} from './functions.js';

export const AdditionalServices = ({cutValue}) => {
    const [additionalServices, setAdditionalServices] = useState([]);
    const [currentPage, setCurrentPage] = useState(0);
    const servicesPerPage = 3;


    const fetchServices = async () => {
        try {
            const response = await fetch('/billingapplication/extras');
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
            const response = await fetch(`/billingapplication/subscribers/extras/add/${serviceId}`, {
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

    const totalPages = Math.ceil(latestServices.length / servicesPerPage);

    const handleNextPage = () => {
        if (currentPage < totalPages - 1) {
            setCurrentPage(currentPage + 1);
        }
    };

    const handlePrevPage = () => {
        if (currentPage > 0) {
            setCurrentPage(currentPage - 1);
        }
    };

    const startIndex = currentPage * servicesPerPage;
    const visibleServices = latestServices.slice(startIndex, startIndex + servicesPerPage);

    return (
        <div className="additional-services">
            {currentPage > 0 && (
                <button className="prev-page" onClick={handlePrevPage}>
                    ‹
                </button>
            )}
            {visibleServices.map((service) => (
            <div className="service-card" key={service.id}>
                <span className="service-card-title"> +{parseAdditionalDesc(getTypeExtra(service.bundle))[0]}</span>
                <span className="service-card-desc">{parseAdditionalDesc(getTypeExtra(service.bundle))[1]}</span>
                <button
                    className="service-price"
                    onClick={() => handleServicePurchase(service.id, service.title)}>
                    {service.price}₽
                </button>
            </div>
        ))}
            
            {currentPage < totalPages - 1 && (
                <button className="next-page" onClick={handleNextPage}>
                    ›
                </button>
            )}
        </div>
    );
};