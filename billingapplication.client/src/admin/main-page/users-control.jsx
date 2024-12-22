import { useState, useEffect } from 'react';
import axios from 'axios';

export const ClientRegisterForm = () => {
  const [formData, setFormData] = useState({
    user: {
      id: 0,
      email: '',
      password: '',
      salt: '',
      passportId: 0,
      tariffId: 0,
      number: '',
      balance: 0,
      paymentDate: '',
      callTime: '00:00:00',
      messages: 0,
      internet: 0,
    },
    passport: {
      id: 0,
      passportNumber: '',
      fullName: '',
      issueDate: '',
      expiryDate: '',
      issuedBy: '',
      registration: '',
    },
    tariffId: 0,
  });

  const [tariffs, setTariffs] = useState([]);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    const fetchTariffs = async () => {
      const TOKEN = localStorage.getItem('token');
      try {
        const response = await axios.get('https://localhost:7262/tariff/', {
          headers: { Authorization: `Bearer ${TOKEN}` },
        });
        setTariffs(response.data); // Ожидается, что API возвращает массив тарифов
        setIsLoading(false);
      } catch (error) {
        console.error('Ошибка при получении тарифов:', error);
        setIsLoading(false);
      }
    };

    fetchTariffs();
  }, []);

  const handleChange = (e) => {
    const { name, value } = e.target;

    if (Object.keys(formData.user).includes(name)) {
      setFormData((prevData) => ({
        ...prevData,
        user: { ...prevData.user, [name]: value },
      }));
    } else if (Object.keys(formData.passport).includes(name)) {
      setFormData((prevData) => ({
        ...prevData,
        passport: { ...prevData.passport, [name]: value },
      }));
    } else if (name === 'tariffId') {
      setFormData((prevData) => ({
        ...prevData,
        tariffId: Number(value),
      }));
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    const TOKEN = localStorage.getItem('token');

    const requestBody = {
      user: {
        ...formData.user,
        id: 0,
        salt: '',
        passportId: 0,
        paymentDate: new Date().toISOString(),
      },
      passport: {
        ...formData.passport,
        id: 0,
        issueDate: new Date(formData.passport.issueDate).toISOString(),
        expiryDate: new Date(formData.passport.expiryDate).toISOString(),
      },
      tariffId: formData.tariffId,
    };

    try {
      const response = await axios.post(
        'https://localhost:7262/auth/register/subscriber',
        requestBody,
        {
          headers: { Authorization: `Bearer ${TOKEN}` },
        }
      );
      console.log('Регистрация прошла успешно:', response.data);
    } catch (error) {
      console.error('Ошибка при регистрации:', error);
    }
  };

  return (
    <form onSubmit={handleSubmit}>
      <h2>Регистрация нового клиента</h2>

      <div>
        <label>Email:</label>
        <input
          type="email"
          name="email"
          value={formData.user.email}
          onChange={handleChange}
          required
        />
      </div>
      <div>
        <label>Пароль:</label>
        <input
          type="password"
          name="password"
          value={formData.user.password}
          onChange={handleChange}
          required
        />
      </div>
      <div>
        <label>Номер телефона:</label>
        <input
          type="text"
          name="number"
          value={formData.user.number}
          onChange={handleChange}
          required
        />
      </div>

      <h2>Данные паспорта</h2>
      <div>
        <label>Номер паспорта:</label>
        <input
          type="text"
          name="passportNumber"
          value={formData.passport.passportNumber}
          onChange={handleChange}
          required
        />
      </div>
      <div>
        <label>ФИО:</label>
        <input
          type="text"
          name="fullName"
          value={formData.passport.fullName}
          onChange={handleChange}
          required
        />
      </div>
      <div>
        <label>Дата выдачи:</label>
        <input
          type="date"
          name="issueDate"
          value={formData.passport.issueDate}
          onChange={handleChange}
          required
        />
      </div>
      <div>
        <label>Дата истечения срока действия:</label>
        <input
          type="date"
          name="expiryDate"
          value={formData.passport.expiryDate}
          onChange={handleChange}
          required
        />
      </div>
      <div>
        <label>Кем выдан:</label>
        <input
          type="text"
          name="issuedBy"
          value={formData.passport.issuedBy}
          onChange={handleChange}
          required
        />
      </div>
      <div>
        <label>Прописка:</label>
        <input
          type="text"
          name="registration"
          value={formData.passport.registration}
          onChange={handleChange}
          required
        />
      </div>

      <button type="submit">Зарегистрировать</button>
    </form>
  );
};