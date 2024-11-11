import { useEffect } from 'react';
import { Doughnut } from "react-chartjs-2";
import "chart.js/auto";
import { useUser } from '../user-context.jsx';

const Dashboard = () => {
    const userData = useUser();

    // Проверка на наличие данных пользователя
    if (!userData) {
        return <div>Загрузка данных...</div>;
    }

    const prettyNumber = userData.number.replace(/(\+7)(\d{3})(\d{3})(\d{2})(\d{2})/, "$1 $2 $3-$4-$5");

    useEffect(() => {
        document.title = 'Панель управления';
    }, []);  // Добавляем пустой массив зависимостей для вызова один раз

    const timeToMinutes = (time) => {
        const [hours, minutes, seconds] = time.split(":").map(Number);
        return hours * 60 + minutes + Math.floor(seconds / 60);
    };

    const dataOptions = [
        { label: "Минуты", value: timeToMinutes(userData.callTime), max: timeToMinutes(userData.tariff.bundle.callTime) },
        { label: "Интернет", value: userData.internet, max: userData.tariff.bundle.internet, unit: "ГБ" },
        { label: "SMS", value: userData.messages, max: userData.tariff.bundle.messages },
    ];

    const additionalServices = [
        { label: '+100', unit: 'минут', price: "99" },
        { label: '+2', unit: 'ГБ', price: "99" },
        { label: '+50', unit: 'SMS', price: "199" },
    ];

    const generateChartData = (value, max) => ({
        datasets: [
            {
                data: [value, max - value],
                backgroundColor: ["#FF3B30", "#E0E0E0"],
                borderWidth: 0,
            },
        ],
    });

    return (
        <div className="dashboard">
            <div className="header">
                <span className="phone-number">{prettyNumber}</span>
                <div className="user-info">Мой номер</div>
            </div>
            <div className="balance-section">
                <h2>Баланс</h2>
                <div className="balance">
                    <div>Мои средства <p className='balance-sum'>{userData.balance}₽</p></div>
                    <div>Расходы: <p className='balance-sum'>{1000}₽</p></div>
                </div>
            </div>
            <div className="tariff-section">
                <h2>Мой тариф</h2>
                <div className="tariff-options">
                    {dataOptions.map((option, index) => (
                        <div className="tariff-card" key={index}>
                            <p className='chart-name'>{option.label}</p>
                            <Doughnut
                                data={generateChartData(option.value, option.max)}
                                options={{
                                    cutout: "90%",
                                    plugins: {
                                        tooltip: { enabled: false },
                                        legend: { display: false },
                                    },
                                }}
                                className="chart"
                            />
                            <div className="chart-value">
                                <div className='chart-value-top'>{option.value}{option.unit || ""}</div> из {option.max}{option.unit || ""}
                            </div>
                        </div>
                    ))}
                </div>
            </div>
            <h2>Дополнительные услуги</h2>
            <div className="additional-services">
                {additionalServices.map((service, index) => (
                    <div className="service-card" key={index}>
                        <span className="service-card-top">{service.label}</span>
                        <span className="service-card-bottom">{service.unit}</span>
                        <button className="service-price">{service.price}₽</button>
                    </div>
                ))}
            </div>
        </div>
    );
}

export default Dashboard;
