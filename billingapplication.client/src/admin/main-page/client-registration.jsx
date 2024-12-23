import { useState } from 'react';
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
    };

    try {
      const response = await axios.post(
        'https://localhost:7262/auth/register/subscriber',
        requestBody,
        {
          headers: { Authorization: `Bearer ${TOKEN}` },
        }
      );
      alert(`Пользователь зарегистрирован с ID: ${response.data}`);

      // Сбрасываем состояние формы после успешной отправки
      setFormData({
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
    } catch (error) {
      console.error('Ошибка при регистрации:', error);
    }
  };

  return (
    <div>
      <form onSubmit={handleSubmit}>
        <h1>Регистрация нового клиента</h1>
        <div className='clientRegister'>
        <div>
          <input
            placeholder='Email'
            type="email"
            name="email"
            value={formData.user.email}
            onChange={handleChange}
            required
          />
        </div>
        <div>
          <input
            placeholder='Пароль'
            type="password"
            name="password"
            value={formData.user.password}
            onChange={handleChange}
            required
          />
        </div>
        <div>
          <input
            placeholder='Номер телефона'
            type="text"
            name="number"
            value={formData.user.number}
            onChange={handleChange}
            required
          />
        </div>
        
        <h2>Данные паспорта</h2>
        <div>
          <input
            placeholder='Номер паспорта'
            type="text"
            name="passportNumber"
            value={formData.passport.passportNumber}
            onChange={handleChange}
            required
          />
        </div>
        <div>
          <input
            placeholder='ФИО'
            type="text"
            name="fullName"
            value={formData.passport.fullName}
            onChange={handleChange}
            required
          />
        </div>
        <div>
          <label>Дата выдачи</label>
          <input
            placeholder='Дата выдачи'
            type="date"
            name="issueDate"
            value={formData.passport.issueDate}
            onChange={handleChange}
            required
          />
        </div>
        <div>
          <label>Дата истечения срока действия</label>
          <input
            placeholder='Дата истечения срока действия'
            type="date"
            name="expiryDate"
            value={formData.passport.expiryDate}
            onChange={handleChange}
            required
          />
        </div>
        <div>
          <input
            placeholder='Кем выдан'
            type="text"
            name="issuedBy"
            value={formData.passport.issuedBy}
            onChange={handleChange}
            required
          />
        </div>
        <div>
          <input
            placeholder='Прописка'
            type="text"
            name="registration"
            value={formData.passport.registration}
            onChange={handleChange}
            required
          />
        </div>
        </div>
        <button className='registerButton' type="submit">Зарегистрировать</button>
      </form>
    </div>
  );
};