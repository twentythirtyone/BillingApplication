import { useEffect, useState } from 'react';
import axios from 'axios';
import { Line } from 'react-chartjs-2';
import { Chart as ChartJS } from 'chart.js/auto';

export const UserCharts = ({ userId }) => {
  const [chartsData, setChartsData] = useState({});
  const [selectedType, setSelectedType] = useState('calls');
  const [totalSum, setTotalSum] = useState(0);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState(null);

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
        setError('Ошибка при загрузке аналитики.');
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
              backgroundColor: 'rgba(75, 192, 192, 0.2)',
              borderColor: 'rgba(75, 192, 192, 1)',
              borderWidth: 1,
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
              label: 'Объем данных (ГБ)',
              data: internetData.map((value) => value / 1024), // Преобразование в ГБ
              backgroundColor: 'rgba(153, 102, 255, 0.2)',
              borderColor: 'rgba(153, 102, 255, 1)',
              borderWidth: 1,
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
              label: 'Количество SMS',
              data: smsData,
              backgroundColor: 'rgba(255, 159, 64, 0.2)',
              borderColor: 'rgba(255, 159, 64, 1)',
              borderWidth: 1,
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
              label: 'Сумма оплаты (₽)',
              data: paymentData,
              backgroundColor: 'rgba(54, 162, 235, 0.2)',
              borderColor: 'rgba(54, 162, 235, 1)',
              borderWidth: 1,
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
      <div>
        <label>
          Выберите тип данных:
          <select value={selectedType} onChange={handleTypeChange} className='user-chart-select'>
            <option value="calls">Звонки</option>
            <option value="internet">Интернет</option>
            <option value="sms">СМС</option>
            <option value="payments">Платежи</option>
          </select>
        </label>
      </div>
      <div className='user-analytics-graph'>
        {chartsData[selectedType] && (
            <>
            <Line data={chartsData[selectedType]} />
            <p>Потрачено за текущий период: {totalSum}</p>
            </>
        )}
      </div>
    </div>
  );
};