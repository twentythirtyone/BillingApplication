import { useEffect, useState } from 'react';
import axios from 'axios';

export const UserInfo = ({ userId }) => {
  const [userData, setUserData] = useState(null);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState(null);

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
        setError('Ошибка при загрузке данных пользователя.');
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
    <div>
      <h1>{userData?.passportInfo?.fullName || 'Неизвестный пользователь'}</h1>
      <p>Номер ID: <strong>{userData?.id || '—'}</strong></p>
      <p>Номер: <strong>{userData?.number || '—'}</strong></p>
      <p>Email: <strong>{userData?.email || '—'}</strong></p>
      <p>Баланс: <strong>{userData?.balance || 0} ₽</strong></p>
      <p>Тариф: <strong>«{userData?.tariff?.title || 'Нет данных'}»</strong></p>
    </div>
  );
};