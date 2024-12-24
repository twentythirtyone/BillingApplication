import { useEffect, useState } from 'react';
import { useUser } from '../user-context.jsx';
import cardIcon from '../assets/img/wallet/card.svg';
import simIcon from '../assets/img/wallet/sim.svg';
import smsIcon from '../assets/img/wallet/sms.svg';
import internetIcon from '../assets/img/wallet/internet.svg';

const token = localStorage.getItem('token');

function Wallet() {
    const [transactionHistory, setTransactionHistory] = useState([]);
    const [error, setError] = useState(null);
    const [visibleCount, setVisibleCount] = useState(3);
    const [isPopupVisible, setPopupVisible] = useState(false);
    const [amount, setAmount] = useState('');
    const [isLoading, setLoading] = useState(false);
    const [expenses, setExpenses] = useState(0);

    const { userData, loading: userLoading, refreshUserData } = useUser();

    useEffect(() => {
        document.title = 'Кошелек';

        const fetchHistory = async () => {
            try { 
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
                const filteredData = data.filter((transaction) => transaction.type === 'Оплата');
                setTransactionHistory(filteredData);
            } catch (err) {
                setError(err.message);
            }
        };

        const getExpenses = async () => {
            try {
                const response = await fetch('https://localhost:7262/subscribers/expenses/month/current', {
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
                setExpenses(data);
            } catch (error) {
                console.log(error.message);
            }
        };

        fetchHistory();
        getExpenses();
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
            phoneId: userData.id,
            senderInfo: userData?.number || '',
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
            refreshUserData();
        } catch (err) {
            alert(err.message);
        } finally {
            setLoading(false);
        }
    };

    const getIcon = (string) => {
        if (string.includes('звонок')) {
            return simIcon;
        } else if (string.toLowerCase().includes('смс')) {
            return smsIcon;
        } else if (string.toLowerCase().includes('гб')) {
            return internetIcon;
        } else {
            return cardIcon;
        }
    };

    return (
        <div className="wallet">
            <h1>Кошелек</h1>
            <h3>Пополнить счет</h3>

            {!isPopupVisible ? (
                <button className="topup-button" onClick={() => setPopupVisible(true)}>
                    <div className="topup-button-img">
                        <img src={cardIcon} alt="Иконка карты" />
                    </div>
                    <div className="topup-button-text">
                        Банковская карта
                        <p>от 10 до 30000₽</p>
                    </div>
                </button>
            ) : (
                <div className="popup-overlay">
                    <div className="popup-topup">
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
                                <li key={index} className="topup-button transaction">
                                    <div className="topup-button-img transaction-img">
                                        <img src={getIcon(transaction.data.name)}></img>
                                    </div>
                                    <div className="topup-button-text">
                                        {transaction.data.name}
                                        <div className='transaction-sum'>{transaction.data.amount} ₽</div>
                                        <p>{new Date(transaction.data.date).toLocaleString()}</p>
                                    </div>
                                </li>
                            ))}
                        </ul>
                        {visibleCount < transactionHistory.length && (
                            <button className="show-more-transactions" onClick={handleShowMore}>
                                Показать еще
                            </button>
                        )}
                    </>
                )}
            </div>
            <div>
                <h2 className='expenses'>Месячные расходы</h2>
                <div className='expenses-sum'>{expenses} ₽</div>
            </div>
        </div>
    );
}

export default Wallet;