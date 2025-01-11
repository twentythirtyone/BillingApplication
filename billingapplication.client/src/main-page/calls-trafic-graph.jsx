import { useState, useEffect } from "react";
import { Line } from "react-chartjs-2";

export const CallsChart = () => {
  const [callsData, setCallsData] = useState([]);
  const currentMonth = new Date().getMonth() + 1;

  const fetchCallsData = async () => {
    try {
      const months = [currentMonth - 2, currentMonth - 1, currentMonth].map(
        (month) => (month <= 0 ? 12 + month : month)
      );
      const responses = await Promise.all(
        months.map((month) =>
          fetch(`/billingapplication/traffic/calls/month/${month}/duration`, {
            headers: {
              Authorization: `Bearer ${localStorage.getItem("token")}`,
            },
          }).then((res) => res.json())
        )
      );
      setCallsData(
        responses.map((data, index) => ({
          month: months[index],
          duration: data || 0,
        }))
      );
    } catch (error) {
      console.error("Ошибка загрузки данных:", error);
    }
  };

  useEffect(() => {
    fetchCallsData();
  }, []);

  
  const getMonthName = (month) => {
    const monthNames = ['Январь', 'Февраль', 'Март', 'Апрель', 'Май', 'Июнь', 'Июль', 'Август', 'Сентябрь', 'Октябрь', 'Ноябрь', 'Декабрь'];
    return monthNames[month - 1];
  };

  const currentMonthData = callsData.find((data) => data.month === currentMonth);
  const currentMonthName = getMonthName(currentMonth);
  const currentMonthDuration = currentMonthData?.duration || 0;

  const chartData = {
    labels: callsData.map((data) => getMonthName(data.month)),
    datasets: [
        {
          label: "Длительность звонков (минуты)",
          data: callsData.map((data) => data.duration),
          borderColor: "#737373",
          borderWidth: 3,
          fill: true,
          backgroundColor: (context) => {
            const chart = context.chart;
            const { ctx, chartArea } = chart;
            if (!chartArea) {
              return null;
            }
            const gradient = ctx.createLinearGradient(0, chartArea.top, 0, chartArea.bottom);
            gradient.addColorStop(0, "rgba(115, 115, 115, 0.15)");
            gradient.addColorStop(1, "rgba(115, 115, 115, 0)");
            return gradient;
          },
          tension: 0.4,
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
    <div className='internet-trafic-sect'>
      <h3>Звонки</h3>
      <h1>{currentMonthDuration} мин</h1>
      <p>За {currentMonthName}</p>
      <Line data={chartData} options={chartOptions} />
    </div>
  );
};
