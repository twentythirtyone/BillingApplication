import axios from 'axios';

const API_URL = 'https://localhost:7262/extras';
const TOKEN = localStorage.getItem('token');

const axiosInstance = axios.create({
  baseURL: API_URL,
  headers: { Authorization: `Bearer ${TOKEN}` },
});

export const fetchExtras = () => axiosInstance.get('/');
export const addExtra = (extra) => axiosInstance.post('/new', extra);
export const updateExtra = (extra) => axiosInstance.patch('/update', extra);
export const deleteExtra = (id) => axiosInstance.delete(`/id/${id}/delete`);