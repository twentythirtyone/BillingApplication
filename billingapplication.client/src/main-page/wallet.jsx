import { useEffect, useState } from 'react';
import { useUser } from '../user-context.jsx';
import cardIcon from '../assets/img/wallet/card.svg';
import simIcon from '../assets/img/wallet/sim.svg';
import smsIcon from '../assets/img/wallet/sms.svg';
import internetIcon from '../assets/img/wallet/internet.svg';
import dollarIcon from '../assets/img/wallet/dollar.svg';
import { ExpensesChart } from './expences-graph.jsx';

const token = localStorage.getItem('token');

function Wallet() {
    const [transactionHistory, setTransactionHistory] = useState([]);
    const [error, setError] = useState(null);
    const [visibleCount, setVisibleCount] = useState(3);
    const [isPopupVisible, setPopupVisible] = useState(false);
    const [amount, setAmount] = useState('');
    const [isLoading, setLoading] = useState(false);
    const [expenses, setExpenses] = useState(0);
    const [filterType, setFilterType] = useState('payments');

    const { userData, loading: userLoading, refreshUserData } = useUser();

    useEffect(() => {
        document.title = 'Кошелек';

        const fetchHistory = async () => {
            try {
                const response = await fetch('/billingapplication/subscribers/wallet/history', {
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
                setTransactionHistory(data);
            } catch (err) {
                setError(err.message);
            }
        };

        const getExpenses = async () => {
            try {
                const response = await fetch('/billingapplication/subscribers/expenses/month/current', {
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
            const response = await fetch('/billingapplication/topups/add', {
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
        } else if (string.toLowerCase().includes('гб') || string.toLowerCase().includes('гига')) {
            return internetIcon;
        } else if (string.toLowerCase().includes('пополнение')) {
            return dollarIcon;
        } else {
            return cardIcon;
        }
    };

    const handleFilterChange = (event) => {
        setFilterType(event.target.value);
    };

    const getFilteredData = () => {
        if (filterType === 'payments') {
            return transactionHistory.payments || [];
        } else if (filterType === 'topUps') {
            return transactionHistory.topUps || [];
        }
        return [];
    };

    const filteredData = transactionHistory ? getFilteredData() : [];

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

                <select value={filterType} onChange={handleFilterChange} className='history-select'>
                    <option value="payments">Списания</option>
                    <option value="topUps">Пополнения</option>
                </select>
                {error ? (
                    <p className="error">{error}</p>
                ) : filteredData.length === 0 ? (
                    <p>Загрузка или нет данных...</p>
                ) : (
                    <>
                        <ul className="transaction-list">
                            {filteredData.slice(0, visibleCount).map((transaction, index) => (
                                <li key={index} className="topup-button transaction">
                                    <div className="topup-button-img transaction-img">
                                        <img src={getIcon(transaction.name || 'Пополнение')} alt="icon" />
                                    </div>
                                    <div className="topup-button-text">
                                        {transaction.name || 'Пополнение счёта'}
                                        <div className='transaction-sum'>{transaction.amount} ₽</div>
                                        <p>{new Date(transaction.date).toLocaleString()}</p>
                                    </div>
                                </li>
                            ))}
                        </ul>
                        {visibleCount < filteredData.length && (
                            <button className="show-more-transactions" onClick={() => setVisibleCount(visibleCount + 5)}>
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
            <ExpensesChart />
        </div>
    );
}

export default Wallet;