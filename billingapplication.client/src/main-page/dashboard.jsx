import { useEffect } from 'react'

const Dashboard = () => {
    useEffect(() => {
        document.title = 'Панель управления';
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
        <div className="monthly-expenses">
          <h2>Месячные расходы</h2>
          <div>10 000Р</div>
          <div className="expenses-chart">
            {/* Здесь можно использовать Chart.js или другую библиотеку для графика! */}
          </div>
        </div>
        <div className="tariff-section">
          <h2>Мой тариф</h2>
          <div className="tariff-options">
            <div>Минуты: 235 мин</div>
            <div>Интернет: 15 ГБ</div>
            <div>SMS: 45 шт</div>
          </div>
        </div>
        <div className="additional-services">
          <h2>Дополнительные услуги</h2>
          <div className="service-card">+100 минут за 99Р</div>
          <div className="service-card">+2 ГБ за 99Р</div>
          <div className="service-card">+50 SMS за 199Р</div>
        </div>
      </div>
    );
  }
  
  export default Dashboard;