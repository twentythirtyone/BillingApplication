import axios from 'axios';

const API_URL = 'https://localhost:7262/tariff';
const TOKEN = localStorage.getItem('token');

const axiosInstance = axios.create({
  baseURL: API_URL,
  headers: { Authorization: `Bearer ${TOKEN}` },
});

export const fetchTariffs = () => axiosInstance.get('/');
export const addTariff = (tariff) => axiosInstance.post('/add', tariff);
export const updateTariff = (tariff) => axiosInstance.patch('/update', tariff);
export const deleteTariff = (id) => axiosInstance.delete(`/${id}/delete`);