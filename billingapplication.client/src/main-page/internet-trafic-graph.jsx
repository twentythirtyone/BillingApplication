import { useEffect, useState } from 'react';
import { Bar } from 'react-chartjs-2';

export const TrafficChart = () => {
  const [trafficData, setTrafficData] = useState([]);
  const [currentMonthTraffic, setCurrentMonthTraffic] = useState(0);

  const fetchTrafficData = async () => {
    const currentMonth = new Date().getMonth() + 1;
    const months = [currentMonth - 2, currentMonth - 1, currentMonth].map(
      (month) => (month <= 0 ? month + 12 : month)
    );
  
    const token = localStorage.getItem('token'); // Получаем токен из localStorage
  
    const data = await Promise.all(
      months.map(async (month) => {
        const response = await fetch(`/billingapplication/traffic/internet/month/${month}/count`, {
          method: 'GET', // Используем метод GET
          headers: {
            'accept': '*/*', // Добавляем accept-заголовок
            'Authorization': `Bearer ${token}`, // Добавляем токен в заголовок Authorization
          },
        });
  
        if (!response.ok) {
          console.error(`Ошибка при запросе данных за месяц ${month}: ${response.statusText}`);
          return { month, count: 0 };
        }
  
        const count = await response.json();
        return { month, count };
      })
    );
  
    setTrafficData(data);
    setCurrentMonthTraffic(data.find((d) => d.month === currentMonth)?.count || 0);
  };

  useEffect(() => {
    fetchTrafficData();
  }, []);

  const getMonthName = (month) => {
    const monthNames = ['Январь', 'Февраль', 'Март', 'Апрель', 'Май', 'Июнь', 'Июль', 'Август', 'Сентябрь', 'Октябрь', 'Ноябрь', 'Декабрь'];
    return monthNames[month - 1];
  };


  const chartData = {
    labels: trafficData.map((data) => getMonthName(data.month)),
    datasets: [
      {
        label: 'Потрачено (ГБ)',
        data: trafficData.map((data) => data.count / 1024),
        backgroundColor: '#EDEDED',
        borderColor: '#737373',
        borderWidth: { top: 3 },
        borderSkipped: 'bottom',
      },
    ],
  };
  
  const chartOptions = {
    responsive: true,
    plugins: {
      legend: {
        display: false,
      },
    },
    scales: {
      x: {
        border:{
            display:false
          },
        ticks: {
          color: '#737373',
          font: {
            size: 14,
            weight: 700,
          },
        },
        grid: {
          color: '#FFFFFF',
          borderColor: '#FFFFFF',
          display: false,
        },
      },
      y: {
        border:{
            display:false
          },
        grid: {
          display: false,
          borderColor: '#FFFFFF',
          drawBorder: false
        },
        beginAtZero: true,
        ticks: {
          display: false,
        },
      },
    },
  };
  return (
    <div className='internet-trafic-sect' >
      <h3>Интернет</h3>
      <h1>{currentMonthTraffic / 1024} ГБ</h1>
      <p>За { new Date().toLocaleString('default', { month: 'long' })}</p>
      <Bar data={chartData} options={chartOptions} />
    </div>
  );
};