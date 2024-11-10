import { useEffect } from 'react'
import { Doughnut } from "react-chartjs-2";
import "chart.js/auto";

const Dashboard = () => {
    useEffect(() => {
        document.title = 'Панель управления';
    });

    const dataOptions = [
        { label: "Минуты", value: 235, max: 500 },
        { label: "Интернет", value: 15, max: 50, unit: "ГБ" },
        { label: "SMS", value: 45, max: 50 },
    ];

    const additionalServices = [
        { label: "+100 минут", price: "99₽" },
        { label: "+2 ГБ", price: "99₽" },
        { label: "+50 SMS", price: "199₽" },
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
          <span className="phone-number">+7 777 777-77-77</span>
          <div className="user-info">Мой номер</div>
        </div>
        <div className="balance-section">
          <h2>Баланс</h2>
          <div className="balance">
            <div>Мои средства: </div>
            <div>Расходы:</div>
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
                        
                        <span>{service.label}</span>
                        <button className="service-price">{service.price}</button>
                    </div>
                ))}
            </div>

      </div>
    );
  }
  
  export default Dashboard;