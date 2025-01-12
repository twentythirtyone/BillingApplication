import { TariffOptions } from './tariff-options.jsx'
import {useState, useEffect} from 'react'
import {getTypeExtra} from './functions.js'
import callsIcon from '../assets/img/extras/callsIcon.svg';
import internetIcon from '../assets/img/extras/internetIcon.svg';
import smsIcon from '../assets/img/extras/smsIcon.svg';
import axios from 'axios'

export const AddServicesPage = () => {
    const token = localStorage.getItem('token');
    const [userData, setUserData] = useState({
        callTime: "00:00:00",
        messages: 0,
        internet: 0,
        tariff: {
          title: "",
          bundle: {
            callTime: "00:00:00",
            messages: 0,
            internet: 0
          }
        },
      });

    const [addServs, setAddServs] = useState([]);

    const fetchExpenses = async () => {
        try {
            const response = await axios.get(
                '/billingapplication/subscribers/current',
                {
                    headers: {
                        Authorization: `Bearer ${token}`,
                        Accept: '*/*',
                    },
                });

            setUserData(response.data);
        } catch (error) {
            console.error('Ошибка при получении данных пользователя:', error);
        }
    };

    const getAddServs = async() => {
        try {
            const response = await axios.get(
                '/billingapplication/extras',
                {
                    headers: {
                        Authorization: `Bearer ${token}`,
                        Accept: '*/*',
                    },
                });

            setAddServs(response.data);
        } catch (e) {
            console.error(e)
        }
    }

    useEffect(() => {
        document.title='Доп услуги'

        fetchExpenses();
        getAddServs();
    }, []);

        const getIcon = (string) => {
            if (string.includes('минут')) {
                return callsIcon;
            } else if (string.toLowerCase().includes('смс')) {
                return smsIcon;
            } else if (string.toLowerCase().includes('гб')) {
                    return internetIcon;
            } else {
                return callsIcon;
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
                if (response.ok) {
                    alert(`Услуга "${serviceTitle}" успешно подключена!`);
                    fetchExpenses();
                }
            } catch (error) {
                alert('Ошибка при подключении услуги:', error);
            }
        };

    return (
        <div className="add-serv-page"> 
            <div className="add-serv-tariff">
                <h2>Тариф: {userData.tariff.title}</h2>
                <TariffOptions userData={userData} />
            </div>
            <h1>Дополнительные услуги</h1>
            <div className="additional-services-list">
                {addServs.map((service) => (
                    <div key={service.id} className="service-item">
                        <button onClick={() => handleServicePurchase(service.id, service.title)}>
                            <div className='extra-servise-title'>
                                <div className='service-item-icon'>
                                    <img src={getIcon(getTypeExtra(service.bundle)) }></img>
                                </div>
                                <div className='service-info'>
                                    <span className="service-title-line">{service.title}</span>
                                    <span className="service-desc-line">{getTypeExtra(service.bundle)}</span>
                                </div>
                            </div>
                            <p className="service-price-line">{service.price} ₽</p>
                        </button>
                    </div>
                ))}
            </div>
        </div>
    );
}