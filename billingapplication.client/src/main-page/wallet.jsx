import { useEffect, useState } from 'react';
import axios from 'axios';
import cardIcon from '../assets/img/wallet/card.svg';
import simIcon from '../assets/img/wallet/sim.svg';
import smsIcon from '../assets/img/wallet/sms.svg';
import internetIcon from '../assets/img/wallet/internet.svg';
import dollarIcon from '../assets/img/wallet/dollar.svg';
import ReactLoading from 'react-loading';
import { ExpensesChart } from './expences-graph.jsx';

export const Wallet = () => {
    const [transactionHistory, setTransactionHistory] = useState(null);
    const [error, setError] = useState(null);
    const [isPopupVisible, setPopupVisible] = useState(false);
    const [amount, setAmount] = useState('');
    const [isLoading, setLoading] = useState(false);
    const [expenses, setExpenses] = useState(0);
    const [filterType, setFilterType] = useState('payments');
    const [startDate, setStartDate] = useState('');
    const [endDate, setEndDate] = useState('');

    const token = localStorage.getItem("token");
    const [userData, setUserData] = useState({
        number: "Нет данных",
        email: "Нет данных",
        balance: 0,
        tariff: {
            title: "Нет данных",
            bundle: {
                callTime: "Нет данных",
            }
        }
    });

    const fetchUserData = async () => {
        try {
            const response = await axios.get("/billingapplication/subscribers/current", {
                headers: {
                    Authorization: `Bearer ${token}`,
                },
            });
            setUserData(response.data);
        } catch (error) {
            console.error("Ошибка при получении данных пользователя:", error);
        }
    };

    const fetchHistory = async () => {
        try {
            const response = await axios.get('/billingapplication/subscribers/wallet/history', {
                headers: {
                    Authorization: `Bearer ${token}`,
                },
            });
            setTransactionHistory(response.data);
        } catch (err) {
            setError(err.message);
        }
    };

    const getExpenses = async () => {
        try {
            const response = await axios.get('/billingapplication/subscribers/expenses/month/current', {
                headers: {
                    Authorization: `Bearer ${token}`,
                },
            });
            setExpenses(response.data);
        } catch (error) {
            console.error("Ошибка получения данных расходов:", error);
        }
    };

    useEffect(() => {
        document.title = 'Кошелек';
        fetchUserData();
        fetchHistory();
        getExpenses();
    }, []);

    const handleTopUp = async () => {
        if (!amount || isNaN(amount)) {
            alert('Введите корректную сумму');
            return;
        }
        setLoading(true);

        const topUpData = {
            phoneId: userData.id,
            senderInfo: userData?.number || '',
            amount: parseFloat(amount),
            date: new Date().toISOString(),
        };

        try {
            const response = await axios.post('/billingapplication/topups/add', topUpData, {
                headers: {
                    Authorization: `Bearer ${token}`,
                },
            });
            alert('Счет успешно пополнен');
            setPopupVisible(false);
            setAmount('');
            fetchUserData();
        } catch (err) {
            alert(err.response?.data?.message || "Ошибка пополнения");
        } finally {
            setLoading(false);
        }
    };

    const getIcon = (string) => {
        if (string.includes('звонок')) return simIcon;
        if (string.toLowerCase().includes('смс')) return smsIcon;
        if (string.toLowerCase().includes('гб') || string.toLowerCase().includes('гига')) return internetIcon;
        if (string.toLowerCase().includes('пополнение')) return dollarIcon;
        return cardIcon;
    };

    const handleFilterChange = (event) => {
        setFilterType(event.target.value);
    };

    const handleDateFilter = () => {
        const currentHistory = getFilteredData();
        if (!Array.isArray(currentHistory)) return [];
        const filtered = currentHistory.filter((transaction) => {
            const transactionDate = new Date(transaction.date);
            const start = startDate ? new Date(startDate) : null;
            const end = endDate ? new Date(endDate) : null;
            return (!start || transactionDate >= start) && (!end || transactionDate <= end);
        });
        return filtered;
    };

    const getFilteredData = () => {
        if (!transactionHistory || typeof transactionHistory !== 'object') return [];
        return filterType === 'payments' ? transactionHistory.payments || [] : transactionHistory.topUps || [];
    };

    const filteredData = handleDateFilter();

    if (!transactionHistory || !userData || !expenses) {
        return (
            <div className="tariff">
                <ReactLoading type="cylon" color="#FF3B30" height={667} width={375} className='loading' />;
            </div>
        );
    }

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

                <div className='history-filters'>
                    <select value={filterType} onChange={handleFilterChange} className='history-select'>
                        <option value="payments">Списания</option>
                        <option value="topUps">Пополнения</option>
                    </select>
                    <div className='history-filter-date'>
                        <div >c </div>
                        <input
                            type="date"
                            value={startDate}
                            onChange={(e) => setStartDate(e.target.value)}
                            className='date-filter'
                            placeholder='Начальная дата'
                        />
                        <div className='history-filter-label'>по</div>
                        <input
                            type="date"
                            value={endDate}
                            onChange={(e) => setEndDate(e.target.value)}
                            className='date-filter'
                            placeholder='Конечная дата'
                        />
                        </div>
                </div>

                {error ? (
                    <p className="error">{error}</p>
                ) : filteredData.length === 0 ? (
                    <ul className="transaction-list"><li className="topup-button transaction">Нет данных</li></ul>
                ) : (
                    <>
                        <ul className="transaction-list">
                            {filteredData.map((transaction, index) => (
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
};