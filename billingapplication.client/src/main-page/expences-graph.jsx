import { useEffect, useState } from 'react';
import axios from 'axios';
import { Bar } from 'react-chartjs-2';

export const ExpensesChart = () => {
    const [chartData, setChartData] = useState({
        labels: [],
        datasets: [],
    });

    const monthMap = {
        January: 'Янв',
        February: 'Фев',
        March: 'Мар',
        April: 'Апр',
        May: 'Май',
        June: 'Июн',
        July: 'Июл',
        August: 'Авг',
        September: 'Сен',
        October: 'Окт',
        November: 'Ноя',
        December: 'Дек',
    };

    const getLastSixMonths = () => {
        const today = new Date();
        const months = [];

        for (let i = 5; i >= 0; i--) {
            const date = new Date(today.getFullYear(), today.getMonth() - i, 1);
            const monthName = date.toLocaleString('en-US', { month: 'long' }); 
            months.push(monthMap[monthName]); 
        }

        return months;
    };

    useEffect(() => {
        const fetchExpenses = async () => {
            try {
                const token = localStorage.getItem('token');
                const response = await axios.get(
                    '/billingapplication/subscribers/expenses/monthes/current',
                    {
                        headers: {
                            Authorization: `Bearer ${token}`,
                            Accept: '*/*',
                        },
                    });

                const serverData = response.data;

                const lastSixMonths = getLastSixMonths();

                const expenses = lastSixMonths.map(
                    (month) =>
                        serverData[
                            Object.keys(monthMap).find((key) => monthMap[key] === month) // Находим соответствие английского месяца
                        ] || 0
                );

                setChartData({
                    labels: lastSixMonths,
                    datasets: [
                        {
                            label: 'Расходы ₽',
                            data: expenses,
                            backgroundColor: '#F5DEDB',
                        },
                    ],
                });
                console.log(serverData)
            } catch (error) {
                console.error('Ошибка при получении данных:', error);
            }
        };

        fetchExpenses();
    }, []);

    return (
        <div style={{ width: '500px', margin: '0 auto', marginTop: '0', marginLeft: '100px' }}>
            <Bar
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
                            border:{
                                display:false
                              },
                            grid: {
                                display: false,
                            },
                            ticks: {
                                font: {
                                    size: 14,
                                    weight: 700,
                                },
                                color: '#AB3D36',
                            },
                        },
                        y: {
                            border:{
                                display:false
                              },
                            grid: {
                                display: false,
                            },
                            ticks: {
                                display: false,
                            },
                        },
                    },
                }}
            />
        </div>
    );
};