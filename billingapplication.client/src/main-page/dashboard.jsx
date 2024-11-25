import { useEffect, useState } from 'react'
import { useUser } from '../user-context.jsx'
import { AdditionalServices } from './additional-services.jsx'
import { TariffOptions } from './tariff-options.jsx'
import { fetchExpenses }  from '../requests.jsx'

const Dashboard = () => {
    const userData = useUser();
    const [userExpenses, setUserExpenses] = useState(null);

    useEffect(() => {
        const loadExpenses = async () => {
            const expenses = await fetchExpenses();
            setUserExpenses(expenses);
        };
        loadExpenses();
    }, []);

    useEffect(() => {
        document.title = 'Панель управления';
    }, []);

    if (!userData) {
        return <div>Загрузка данных...</div>;
    }

    const prettyNumber = userData.number.replace(/(\+7|8)(\d{3})(\d{3})(\d{2})(\d{2})/, "+7 $2 $3-$4-$5");

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
                    <div>Расходы: <p className='balance-sum'>{userExpenses}₽</p></div>
                </div>
            </div>
            <div className="tariff-section">
                <h2>Мой тариф</h2>
                <TariffOptions userData={ userData } />
            </div>
            <h2>Дополнительные услуги</h2>
            <AdditionalServices />
        </div>
    );
};

export default Dashboard;
