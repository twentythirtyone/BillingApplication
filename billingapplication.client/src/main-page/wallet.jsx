import {useEffect } from 'react'


function Wallet() {
    useEffect(() => {
        document.title = 'Кошелек';
    });

    return (
        <div className="wallet">
            <h2>Кошелек</h2>
            <div className="wallet-balance">
                <h3>Пополнить счет</h3>
            </div>

            <div className="transaction-history">
                <h3>История операций</h3>
                <ul className="transaction-list">
                </ul>
            </div>

            <div className="monthly-expenses">
                <h3>Месячные расходы</h3>
                <div className="expenses-chart">
                    {/* Здесь можно использовать Chart.js или другую библиотеку для графика */}
                </div>
            </div>
        </div>
    );
}

export default Wallet;