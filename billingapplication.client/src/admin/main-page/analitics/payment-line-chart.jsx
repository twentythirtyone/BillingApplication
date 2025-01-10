import { useEffect, useState } from 'react';
import axios from 'axios';
import { Line } from 'react-chartjs-2';
import {
  Chart as ChartJS,
  CategoryScale,
  LinearScale,
  PointElement,
  LineElement,
  Title,
  Tooltip,
  Legend,
} from 'chart.js';

ChartJS.register(CategoryScale, LinearScale, PointElement, LineElement, Title, Tooltip, Legend);

export const PaymentLineChart = () => {
  const [chartData, setChartData] = useState(null);
  const token = localStorage.getItem('token');

  useEffect(() => {
    const fetchData = async () => {
      try {
        const response = await axios.get('/billingapplication/payment/month', {
          headers: {
            Authorization: `Bearer ${token}`,
            accept: '*/*',
          },
        });

        const payments = response.data;

        const groupedByDate = payments.reduce((acc, payment) => {
          const date = new Date(payment.date).toISOString().split('T')[0];
          acc[date] = (acc[date] || 0) + payment.amount;
          return acc;
        }, {});

        const sortedDates = Object.keys(groupedByDate).sort();
        const formattedDates = sortedDates.map((date) => {
          const [year, month, day] = date.split('-');
          return `${day}.${month}.${year}`;
        });
        const dailySums = sortedDates.map((date) => groupedByDate[date]);

        setChartData({
          labels: formattedDates,
          datasets: [
            {
              label: 'Потраченная сумма за день',
              data: dailySums,
              borderColor: 'rgba(54, 162, 235, 1)',
              backgroundColor: 'rgba(54, 162, 235, 0.2)',
              tension: 0.4,
              pointRadius: 5,
              pointBackgroundColor: 'rgba(54, 162, 235, 1)',
            },
          ],
        });
      } catch (error) {
        console.error('Error fetching payment data:', error);
      }
    };

    fetchData();
  }, []);

  if (!chartData) return <div>Загрузка...</div>;

  return (
    <div style={{ width: '560px', margin: 'auto' }}>
      <Line
        data={chartData}
        options={{
          responsive: true,
          plugins: {
            legend: {
              display: false,
            },
          },
          scales: {
            x: {
              ticks: {
                color: '#FFFFFF',
              },
              grid: {
                color: 'rgba(255, 255, 255, 0.2)',
              },
            },
            y: {
                ticks: {
                    color: '#FFFFFF',
                    stepSize: 500, // Задаёт шаг между метками
                  },
              grid: {
                color: 'rgba(255, 255, 255, 0.2)',
              },
              title: {
                display: true,
                text: 'Сумма (₽)',
                color: '#FFFFFF',
              },
            },
          },
        }}
      />
    </div>
  );
};