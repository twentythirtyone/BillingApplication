/* eslint-disable react/prop-types */

import { useEffect, useState } from 'react';
import axios from 'axios';
import { Line } from 'react-chartjs-2';

export const UserCharts = ({ userId }) => {
  const [chartsData, setChartsData] = useState({});
  const [selectedType, setSelectedType] = useState('calls');
  const [totalSum, setTotalSum] = useState(0);
  const [isLoading, setIsLoading] = useState(true);

  const token = localStorage.getItem('token');

  useEffect(() => {
    const fetchAnalytics = async () => {
      setIsLoading(true);
      try {
        const response = await axios.get(`/billingapplication/history/${userId}`, {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        });

        const groupedData = response.data.reduce((acc, item) => {
          const { type, data } = item;
          if (!acc[type]) acc[type] = [];
          acc[type].push(data);
          return acc;
        }, {});

        prepareChartData(groupedData);
      } catch (error) {
        console.error('Ошибка при загрузке аналитики:', error);
      } finally {
        setIsLoading(false);
      }
    };

    const prepareChartData = (data) => {
      const charts = {};
      const groupByDate = (items, valueKey) => {
        const grouped = items.reduce((acc, item) => {
          const date = new Date(item.date).toLocaleDateString();
          if (!acc[date]) acc[date] = 0;
          acc[date] += valueKey ? item[valueKey] || 0 : 1;
          return acc;
        }, {});

        return {
          labels: Object.keys(grouped),
          data: Object.values(grouped),
        };
      };

      if (data['Звонок']) {
        const { labels, data: durationData } = groupByDate(data['Звонок'], 'duration');
        charts.calls = {
          labels,
          datasets: [
            {
              label: 'Длительность звонков (минуты)',
              data: durationData,
              borderColor: 'rgba(0, 255, 191, 0.9)',
              borderWidth: 3,
              backgroundColor: 'none',
              tension: 0.4,
            },
          ],
        };
      }

      if (data['Интернет']) {
        const { labels, data: internetData } = groupByDate(data['Интернет'], 'spentInternet');
        charts.internet = {
          labels,
          datasets: [
            {
              label: 'Объем трафика (ГБ)',
              data: internetData.map((value) => value / 1024), // Преобразование в ГБ
              borderColor: 'rgba(0, 255, 255, 0.9)',
              borderWidth: 3,
              backgroundColor: 'none',
              tension: 0.4,
            },
          ],
        };
      }

      if (data['СМС']) {
        const { labels, data: smsData } = groupByDate(data['СМС'], null);
        charts.sms = {
          labels,
          datasets: [
            {
              label: 'Количество отправленных SMS',
              data: smsData,
              borderColor: 'rgba(0, 162, 255, 0.9)', // Неоновый фиолетовый
              borderWidth: 3,
              backgroundColor: 'none',
              tension: 0.4,
            },
          ],
        };
      }

      if (data['Оплата']) {
        const { labels, data: paymentData } = groupByDate(data['Оплата'], 'amount');
        charts.payments = {
          labels,
          datasets: [
            {
              label: 'Общая сумма платежей (₽)',
              data: paymentData,
              borderColor: 'rgba(168, 118, 248, 0.9)', // Неоновый жёлтый
              borderWidth: 3,
              backgroundColor: 'none',
              tension: 0.4,
            },
          ],
        };
      }

      setChartsData(charts);
    };

    fetchAnalytics();
  }, [userId]);

  const handleTypeChange = (event) => {
    setSelectedType(event.target.value);
    calculateTotalSum(event.target.value);
  };

  const calculateTotalSum = (type) => {
    if (!chartsData[type]) return;

    const { datasets } = chartsData[type];
    const sum = datasets[0].data.reduce((total, value) => total + value, 0);
    setTotalSum(sum);
  };

  if (isLoading) {
    return <p>Загрузка данных аналитики...</p>;
  }

  return (
    <div className="user-charts">
      <h2>Активность пользователя</h2>
      <label>
        Выберите тип данных:
        <select value={selectedType} onChange={handleTypeChange} className="user-chart-select">
          <option value="calls">Звонки</option>
          <option value="internet">Интернет</option>
          <option value="sms">СМС</option>
          <option value="payments">Платежи</option>
        </select>
      </label>
      <div className="user-analytics-graph">
        {chartsData[selectedType] && (
          <Line
            data={chartsData[selectedType]}
            options={{
              plugins: {
                legend: {
                  display: false,
                },
              },
              elements: {
                line: {
                  tension: 0.4, // Убедитесь, что интерполяция линии включена
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
                  display: false, // Убираем подписи оси Y
                },
              },
              responsive: true,
              maintainAspectRatio: false,
            }}
          />
        )}
      </div>
      <p>Потрачено за текущий период: {totalSum}</p>
    </div>
  );
};