import { useState } from 'react';

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

  const [serverResponse, setServerResponse] = useState(''); // Новое состояние для ответа от сервера

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
      subscriberModel: {
        ...formData.user,
        id: 0,
        salt: '',
        passportId: 0,
        paymentDate: new Date().toISOString(),
        callTime: '00:00:00',
        creationDate: new Date().toISOString(),
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
      const response = await fetch('/billingapplication/auth/register/subscriber', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          Authorization: `Bearer ${TOKEN}`,
        },
        body: JSON.stringify(requestBody),
      });

      if (response.ok) {
        const data = await response.json();
        setServerResponse(`Пользователь зарегистрирован с ID: ${data}`);
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
      } else {
        const errorText = await response.text();
        setServerResponse(`Ошибка при регистрации: ${errorText}`);
      }
    } catch (error) {
      setServerResponse(`Ошибка при регистрации: ${error.message}`);
    }
  };

  return (
    <div>
      <form onSubmit={handleSubmit}>
        <h1>Регистрация нового клиента</h1>
        <div className="clientRegister">
          <div>
            <input
              placeholder="Email"
              type="email"
              name="email"
              value={formData.user.email}
              onChange={handleChange}
              required
            />
          </div>
          <div>
            <input
              placeholder="Пароль"
              type="password"
              name="password"
              value={formData.user.password}
              onChange={handleChange}
              required
            />
          </div>
          <div>
            <input
              placeholder="Номер телефона"
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
              placeholder="Номер паспорта"
              type="text"
              name="passportNumber"
              value={formData.passport.passportNumber}
              onChange={handleChange}
              required
            />
          </div>
          <div>
            <input
              placeholder="ФИО"
              type="text"
              name="fullName"
              value={formData.passport.fullName}
              onChange={handleChange}
              required
            />
          </div>
          <div>
            <input
              type="text"
              placeholder="Дата выдачи"
              name="issueDate"
              value={formData.passport.issueDate}
              onFocus={(e) => (e.target.type = 'date')}
              onBlur={(e) => {
                if (!e.target.value) e.target.type = 'text';
              }}
              onChange={handleChange}
              required
            />
          </div>
          <div>
            <input
              placeholder="Дата истечения срока действия"
              type="text"
              name="expiryDate"
              value={formData.passport.expiryDate}
              onFocus={(e) => (e.target.type = 'date')}
              onBlur={(e) => {
                if (!e.target.value) e.target.type = 'text';
              }}
              onChange={handleChange}
              required
            />
          </div>
          <div>
            <input
              placeholder="Кем выдан"
              type="text"
              name="issuedBy"
              value={formData.passport.issuedBy}
              onChange={handleChange}
              required
            />
          </div>
          <div>
            <input
              placeholder="Прописка"
              type="text"
              name="registration"
              value={formData.passport.registration}
              onChange={handleChange}
              required
            />
          </div>
        </div>
        <button className="registerButton" type="submit">
          Зарегистрировать
        </button>
      </form>
      {serverResponse && <p>{serverResponse}</p>}
    </div>
  );
};