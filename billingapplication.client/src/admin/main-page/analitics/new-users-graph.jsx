import { useEffect, useState } from 'react';
import axios from 'axios';
import { Bar } from 'react-chartjs-2';
import {
  Chart as ChartJS,
  CategoryScale,
  LinearScale,
  BarElement,
  Title,
  Tooltip,
  Legend,
} from 'chart.js';

ChartJS.register(
  CategoryScale,
  LinearScale,
  BarElement,
  Title,
  Tooltip,
  Legend
);

export const TariffPieChart = ({ urlAPI, title }) => {
  const token = localStorage.getItem('token');
  const [chartData, setChartData] = useState(null);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const response = await axios.get(urlAPI, {
          headers: {
            Authorization: `Bearer ${token}`,
            accept: '*/*',
          },
        });

        const responseData = response.data;

        const extendedData = {
          Октябрь: 0,
          Ноябрь: 0,
          Декабрь: 0,
          ...responseData,
        };

        // Извлекаем метки и значения для диаграммы
        const labels = Object.keys(extendedData);
        const values = Object.values(extendedData);

        setChartData({
          labels,
          datasets: [
            {
              data: values,
              backgroundColor: [   
                'rgba(75, 192, 192, 0.6)',
                'rgba(153, 102, 255, 0.6)',
                'rgba(54, 162, 235, 0.6)',
                'rgba(99, 132, 255, 0.6)',
                'rgba(54, 235, 162, 0.6)', 
                'rgba(201, 203, 207, 0.6)',
              ],
              borderColor: [
                'rgba(75, 192, 192, 1)',
                'rgba(153, 102, 255, 1)',
                'rgba(54, 162, 235, 1)',
                'rgba(99, 132, 255, 1)',
                'rgba(54, 235, 162, 1)',
                'rgba(201, 203, 207, 1)',
              ],
              borderWidth: 1,
            },
          ],
        });
      } catch (error) {
        console.error('Error fetching tariff data:', error);
      }
    };

    fetchData();
  }, [urlAPI, title]);

  if (!chartData) return <div>Загрузка...</div>;

  return (
    <div style={{ width: '560px' }}>
      <Bar
        data={chartData}
        options={{
          responsive: true,
          plugins: {
            legend: {
              position: 'top',
              display: false,
              labels: {
                color: '#FFFFFF',
              },
            },
          },
          scales: {
            x: {
              ticks: {
                color: '#FFFFFF',
                font: {
                    size: 14, // Размер шрифта в пикселях
                    
                  },
              },
              grid: {
                color: 'rgba(255, 255, 255, 0.2)',
              },
            },
            y: {
              ticks: {
                color: '#FFFFFF',
                callback: function (value) {
                  return Number.isInteger(value) ? value : null;
                },
              },
              grid: {
                color: 'rgba(255, 255, 255, 0.2)',
              },
            },
          },
        }}
      />
    </div>
  );
};