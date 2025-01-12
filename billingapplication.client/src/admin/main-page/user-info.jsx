/* eslint-disable react/prop-types */

import { useEffect, useState } from 'react';
import axios from 'axios';

export const UserInfo = ({ userId }) => {
  const [userData, setUserData] = useState(null);
  const [isLoading, setIsLoading] = useState(true);

  const token = localStorage.getItem('token');

  useEffect(() => {
    const fetchUserData = async () => {
      setIsLoading(true);
      try {
        const response = await axios.get(`/billingapplication/subscribers/${userId}`, {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        });
        setUserData(response.data);
      } catch (error) {
        console.error(error);
      } finally {
        setIsLoading(false);
      }
    };

    fetchUserData();
  }, [userId]);

  if (isLoading) {
    return <p>Загрузка данных пользователя...</p>;
  }

  return (
    <div className='user-info-container'>
      <h1>{userData?.passportInfo?.fullName || 'Неизвестный пользователь'}</h1>
      <p><span>ID</span> <strong>{userData?.id || '—'}</strong></p>
      <p><span>Номер</span> <strong>{userData?.number || '—'}</strong></p>
      <p><span>Email</span> <strong>{userData?.email || '—'}</strong></p>
      <p><span>Баланс</span> <strong>{userData?.balance || 0} ₽</strong></p>
      <p><span>Тариф</span> <strong>«{userData?.tariff?.title || 'Нет данных'}»</strong></p>
  </div>
  );
};