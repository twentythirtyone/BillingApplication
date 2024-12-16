import { useEffect, useState } from 'react';

function Wallet() {
    const [transactionHistory, setTransactionHistory] = useState([]);
    const [error, setError] = useState(null);
    const [visibleCount, setVisibleCount] = useState(3); // Количество видимых операций

    useEffect(() => {
        document.title = 'Кошелек';

        const fetchHistory = async () => {
            try {
                const token = localStorage.getItem('token');
                const response = await fetch('https://localhost:7262/subscribers/history', {
                    method: 'GET',
                    headers: {
                        'Content-Type': 'application/json',
                        Authorization: `Bearer ${token}`,
                    },
                });

                if (!response.ok) {
                    throw new Error(`Ошибка: ${response.status}`);
                }

                const data = await response.json();

                // Фильтрация: берем только те операции, где price !== 0
                const filteredData = data.filter((transaction) => transaction.data.price !== 0);
                setTransactionHistory(filteredData);
            } catch (err) {
                setError(err.message);
            }
        };

        fetchHistory();
    }, []);

    const handleShowMore = () => {
        setVisibleCount((prevCount) => prevCount + 5);
    };

    // Определение иконки для типа операции
    const getIcon = (type) => {
        switch (type) {
            case 'СМС':
                return '📨';
            case 'Интернет':
                return '🌐';
            case 'Оплата':
                return '💳';
            case 'Звонки':
                return '📞';
            default:
                return '❓';
        }
    };

    return (
        <div className="wallet">
            <h2>Кошелек</h2>

            <div className="transaction-history">
                <h3>История операций</h3>
                {error ? (
                    <p className="error">{error}</p>
                ) : transactionHistory.length === 0 ? (
                    <p>Загрузка или нет данных...</p>
                ) : (
                    <>
                        <ul className="transaction-list">
                            {transactionHistory.slice(0, visibleCount).map((transaction, index) => (
                                <li key={index} className="transaction-item">
                                    <div className="transaction-icon">{getIcon(transaction.type)}</div>
                                    <div className="transaction-details">
                                        <p className="transaction-type">
                                            <strong>{transaction.type}</strong>
                                        </p>
                                        <p className="transaction-name">{transaction.data.name}</p>
                                        <p className="transaction-amount-date">
                                            <span>{transaction.data.price} ₽</span> ·{' '}
                                            <span>{new Date(transaction.data.date).toLocaleString()}</span>
                                        </p>
                                    </div>
                                </li>
                            ))}
                        </ul>
                        {visibleCount < transactionHistory.length && (
                            <button className="show-more" onClick={handleShowMore}>
                                Показать еще
                            </button>
                        )}
                    </>
                )}
            </div>
        </div>
    );
}

export default Wallet;