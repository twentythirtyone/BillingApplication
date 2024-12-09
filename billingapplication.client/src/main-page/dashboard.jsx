import { useEffect, useState } from 'react';
import ReactLoading from 'react-loading';
import { useUser } from '../user-context.jsx';
import { AdditionalServices } from './additional-services.jsx';
import { TariffOptions } from './tariff-options.jsx';
import { fetchExpenses } from '../requests.jsx';

const Dashboard = () => {
    const { userData, loading: userLoading, refreshUserData } = useUser(); // Достаем метод обновления из контекста
    const [userExpenses, setUserExpenses] = useState(null);
    const [loading, setLoading] = useState(true);

    // Функция для загрузки расходов
    const loadExpenses = async () => {
        setLoading(true);
        try {
            const expenses = await fetchExpenses();
            setUserExpenses(expenses);
        } catch (err) {
            console.error("Ошибка при загрузке расходов:", err);
        } finally {
            setLoading(false);
        }
    };

    // Первичная загрузка данных
    useEffect(() => {
        const initialize = async () => {
            if (userData) {
                await loadExpenses();
            }
            setLoading(false); // Завершаем первичную загрузку
        };
        initialize();
    }, [userData]);

    useEffect(() => {
        document.title = "Панель управления";
    }, []);

    // Если данные пользователя или расходы загружаются, показываем индикатор
    if (userLoading || loading) {
        return (
            <ReactLoading
                type="cylon"
                color="#FF3B30"
                height={667}
                width={375}
                className="loading"
            />
        );
    }

    // Если пользователь не авторизован
    if (!userData) {
        return null; // Вы можете добавить здесь логику перенаправления, если нужно
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
                    <div>
                        Мои средства <p className="balance-sum">{userData.balance}₽</p>
                    </div>
                    <div>
                        Расходы: <p className="balance-sum">{userExpenses}₽</p>
                    </div>
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