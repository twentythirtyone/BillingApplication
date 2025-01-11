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

export const TariffBarChart = ({urlAPI, title}) => {
    const token = localStorage.getItem('token');
  const [chartData, setChartData] = useState(null);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const response = await axios.get(urlAPI, {
          headers: {
            'Authorization': `Bearer ${token}`,
            'accept': '*/*',
          },
        });

        // Extract data for the chart
        const labels = Object.keys(response.data);
        const values = Object.values(response.data);

        setChartData({
          labels,
          datasets: [
            {
              label: title,
              data: values,
              backgroundColor: [
                'rgba(54, 162, 235, 0.6)',
                'rgba(75, 192, 192, 0.6)',
                'rgba(153, 102, 255, 0.6)',
                'rgba(201, 203, 207, 0.6)',
                'rgba(99, 132, 255, 0.6)'
              ],
              borderColor: [
                'rgba(54, 162, 235, 1)',
                'rgba(75, 192, 192, 1)',
                'rgba(153, 102, 255, 1)',
                'rgba(201, 203, 207, 1)',
                'rgba(99, 132, 255, 1)'
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