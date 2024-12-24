import getUserExpenses from './main-page/expences.jsx'


import axios from 'axios';

// Настройка базового клиента
const apiClient = axios.create({
    baseURL: 'http://billing-app-server:5183',
    headers: {
        'Content-Type': 'application/json',
    },
});

// Перехватчик для добавления токена авторизации
apiClient.interceptors.request.use((config) => {
    const token = localStorage.getItem('token');
    if (token) {
        config.headers['Authorization'] = `Bearer ${token}`;
    }
    return config;
}, (error) => Promise.reject(error));

export const getTariff = async () => {
    try {
        const response = await apiClient.get('/tariff');
        return response.data; // Возвращаем только данные
    } catch (error) {
        console.error('Ошибка при получении тарифов:', error);
        throw error; // Генерируем ошибку для обработки
    }
};

// Получение расходов
export const fetchExpenses = async () => {
    try {
        const response = await apiClient.get('/subscribers/expenses/month/current'); // Предполагаем, что это путь
        return response.data; // Возвращаем только данные
    } catch (error) {
        console.error('Ошибка при загрузке расходов:', error);
        throw error; // Генерируем ошибку для обработки
    }
};

// Смена тарифа
export const changeTariff = async (tariffId) => {
    try {
        const response = await apiClient.post(`/subscribers/tariff/change/${tariffId}`);
        return response.data;
    } catch (error) {
        console.error('Ошибка при смене тарифа:', error);
        throw error; // Генерируем ошибку для обработки
    }
};

// Оплата тарифа
export const payTariff = async (tariffId) => {
    try {
        const response = await apiClient.post('/subscribers/tariff/pay', { tariffId });
        return response.data;
    } catch (error) {
        console.error('Ошибка при оплате тарифа:', error);
        throw error; // Генерируем ошибку для обработки
    }
};