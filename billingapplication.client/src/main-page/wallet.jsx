function Wallet() {
    return (
        <div className="wallet">
            <h2>Кошелек</h2>
            <div className="wallet-balance">
                <h3>Пополнить счет</h3>
                <div className="wallet-topup">
                    <div className="topup-option">
                        <span>Bank card</span>
                        <span>from 10 to 30000 ₽</span>
                    </div>
                    <div className="topup-icon">→</div>
                </div>
            </div>

            <div className="transaction-history">
                <h3>История операций</h3>
                <ul className="transaction-list">
                    <li>
                        <div className="transaction-detail">
                            <span className="transaction-type">Оплата за услугу</span>
                            <span className="transaction-description">“Дополнительные минуты” № 12344</span>
                            <span className="transaction-date">30.06.2024 • 20:01</span>
                        </div>
                        <div className="transaction-amount">-99₽</div>
                    </li>
                    <li>
                        <div className="transaction-detail">
                            <span className="transaction-type">Тариф</span>
                            <span className="transaction-description">Ежемесячное списание средств по тарифу</span>
                            <span className="transaction-date">29.06.2024 • 18:43</span>
                        </div>
                        <div className="transaction-amount">-599₽</div>
                    </li>
                    <li>
                        <div className="transaction-detail">
                            <span className="transaction-type">Пополнение</span>
                            <span className="transaction-description">Пополнение счета №101254</span>
                            <span className="transaction-date">28.06.2024 • 08:22</span>
                        </div>
                        <div className="transaction-amount">+200₽</div>
                    </li>
                    {}
                </ul>
            </div>

            <div className="monthly-expenses">
                <h3>Месячные расходы</h3>
                <div className="expenses-amount">10 000₽</div>
                <div className="expenses-chart">
                    {/* Здесь можно использовать Chart.js или другую библиотеку для графика */}
                </div>
            </div>
        </div>
    );
}

export default Wallet;