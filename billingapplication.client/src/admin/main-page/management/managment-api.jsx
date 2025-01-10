import axios from 'axios';

const Tariff_API = '/billingapplication/tariff';
const EXTRA_API = '/billingapplication/extras';
const TOKEN = localStorage.getItem('token');

const tariffInstance = axios.create({
  baseURL: Tariff_API,
  headers: { Authorization: `Bearer ${TOKEN}` },
});

const extraInstance = axios.create({
  baseURL: EXTRA_API,
  headers: { Authorization: `Bearer ${TOKEN}` },
});


export const fetchTariffs = () => tariffInstance.get('/');
export const addTariff = (tariff) => tariffInstance.post('/add', tariff);
export const updateTariff = (tariff) => tariffInstance.patch('/update', tariff);
export const deleteTariff = (id) => tariffInstance.delete(`/id/${id}`);

export const fetchExtras = () => extraInstance.get('/');
export const addExtra = (extra) => extraInstance.post('/new', extra);
export const updateExtra = (extra) => extraInstance.patch('/update', extra);
export const deleteExtra = (id) => extraInstance.delete(`/${id}`);