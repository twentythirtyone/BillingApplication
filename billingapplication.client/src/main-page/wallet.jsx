import { useEffect, useState } from 'react';
import { useUser } from '../user-context.jsx';

function Wallet() {
    const [transactionHistory, setTransactionHistory] = useState([]);
    const [error, setError] = useState(null);
    const [visibleCount, setVisibleCount] = useState(3);
    const [isPopupVisible, setPopupVisible] = useState(false); // Состояние для показа попапа
    const [amount, setAmount] = useState(''); // Поле для ввода суммы
    const [isLoading, setLoading] = useState(false); // Состояние загрузки

    const { userData, loading: userLoading, refreshUserData } = useUser();

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
                const filteredData = data.filter((transaction) => transaction.data.amount !== 0);
                setTransactionHistory(filteredData);
            } catch (err) {
                setError(err.message);
            }
        };

        fetchHistory();
    }, []);

    const handleShowMore = () => {
        setVisibleCount((prevCount) => prevCount + 3);
    };

    const handleTopUp = async () => {
        if (!amount || isNaN(amount)) {
            alert('Введите корректную сумму');
            return;
        }

        setLoading(true);

        const topUpData = {
            id: 0,
            phoneId: userData?.id || 0,
            senderInfo: userData?.number || 'string',
            amount: parseFloat(amount),
            date: new Date().toISOString(),
        };

        try {
            const token = localStorage.getItem('token');
            const response = await fetch('https://localhost:7262/topups/add', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    Authorization: `Bearer ${token}`,
                },
                body: JSON.stringify(topUpData),
            });

            if (!response.ok) {
                throw new Error('Ошибка при пополнении счета');
            }

            alert('Счет успешно пополнен');
            setPopupVisible(false);
            setAmount('');
            refreshUserData(); // Обновляем данные пользователя
        } catch (err) {
            alert(err.message);
        } finally {
            setLoading(false);
        }
    };

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

            {/* Ссылка на пополнение */}
            <button className="topup-button" onClick={() => setPopupVisible(true)}>
                Пополнить счет
            </button>

            {/* Попап */}
            {isPopupVisible && (
                <div className="popup-overlay">
                    <div className="popup">
                        <h3>Пополнение счета</h3>
                        <input
                            type="number"
                            placeholder="Введите сумму"
                            value={amount}
                            onChange={(e) => setAmount(e.target.value)}
                            className="topup-input"
                        />
                        <div className="popup-actions">
                            <button className="topup-submit" onClick={handleTopUp} disabled={isLoading}>
                                {isLoading ? 'Отправка...' : 'Отправить'}
                            </button>
                            <button className="topup-cancel" onClick={() => setPopupVisible(false)}>
                                Отмена
                            </button>
                        </div>
                    </div>
                </div>
            )}

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
                                            <span>{transaction.data.amount} ₽</span> ·{' '}
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