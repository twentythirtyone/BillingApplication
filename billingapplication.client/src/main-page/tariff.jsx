import { AdditionalServices } from './additional-services.jsx';
import { useEffect, useState } from 'react';

const Tariff = () => {
    const [tariffs, setTariffs] = useState([]);

    useEffect(() => {
        const token = localStorage.getItem('token');
        if (token) {
            getTariff(token);
        }
    }, []);

    const getTariff = async (token) => {
        try {
            const response = await fetch('https://localhost:7262/tariff/get', {
                method: 'GET',
                headers: {
                    'Accept': '*/*',
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${token}`,
                },
            });

            if (!response.ok) {
                throw new Error(`Ошибка: ${response.status}`);
            }
            const data = await response.json();
            setTariffs(data);
        } catch (error) {
            console.error('Failed to get tariff data', error);
        }
    };

    return (
        <div className="tariff">
            <h1>Оптимальный трафик для вас</h1>
            <div className="tariff-services">
                <AdditionalServices />
            </div>
            <div className='tariff-cards'>
                {tariffs.map((tariff) => (
                    <div className='tariff-card' key={tariff.id}>
                        <h3 className='tariff-title'>{tariff.title}</h3>
                        <p className='tariff-price'>
                            {tariff.price}₽ <span>в месяц</span>
                        </p>
                        <p className='tariff-description'>{tariff.description}</p>
                        <button className='tariff-button'>Выбрать тариф</button>
                    </div>
                ))}
            </div>
        </div>
    );
};

export default Tariff;