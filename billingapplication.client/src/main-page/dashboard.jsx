import { useEffect, useState } from 'react'
import { useNavigate } from "react-router-dom";
import { useUser } from '../user-context.jsx'
import { AdditionalServices } from './additional-services.jsx'
import { TariffOptions } from './tariff-options.jsx'
import { fetchExpenses }  from '../requests.jsx'

const Dashboard = () => {
    const navigate = useNavigate();
    const userData = useUser();
    const [userExpenses, setUserExpenses] = useState(null);
    const [loading, setLoading] = useState(true); // Для отслеживания загрузки данных
    const [error, setError] = useState(null); // Для обработки ошибок

    useEffect(() => {
        const loadExpenses = async () => {
            try {
                const expenses = await fetchExpenses();
                setUserExpenses(expenses);
            } catch (err) {
                console.error("Ошибка при загрузке расходов:", err);
                setError("Не удалось загрузить данные расходов.");
            } finally {
                setLoading(false);
            }
        };

        if (userData) {
            loadExpenses();
        } else {
            setLoading(false);
        }
    }, [userData]);

    useEffect(() => {
        document.title = "Панель управления";
    }, []);

    useEffect(() => {
        if (!userData || error) {
            navigate("/"); // Перенаправление на главную страницу
        }
    }, [userData, error, navigate]);

    if (loading) {
        return <div>Загрузка данных...</div>;
    }

    if (!userData) {
        return null; // Пользователь не авторизован, уже перенаправлен на главную
    }

    const prettyNumber = userData.number.replace(
        /(\+7|8)(\d{3})(\d{3})(\d{2})(\d{2})/,
        "+7 $2 $3-$4-$5"
    );

    return (
        <div className="dashboard">
            <div className="header">
                <span className="phone-number">{prettyNumber}</span>
                <div className="user-info">Мой номер</div>
            </div>
            <div className="balance-section">
                <h2>Баланс</h2>
                <div className="balance">
                    <div>Мои средства <p className="balance-sum">{userData.balance}₽</p></div>
                    <div>Расходы: <p className="balance-sum">{userExpenses}₽</p></div>
                </div>
            </div>
            <div className="tariff-section">
                <h2>Тариф: {userData.tariff.title}</h2>
                <TariffOptions userData={userData} />
            </div>
            <h2>Дополнительные услуги</h2>
            <AdditionalServices />
        </div>
    );
};

export default Dashboard;
