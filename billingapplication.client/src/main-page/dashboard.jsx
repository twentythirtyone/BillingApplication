import { useEffect, useState } from 'react';
import ReactLoading from 'react-loading';
import { AdditionalServices } from './additional-services.jsx';
import { TariffOptions } from './tariff-options.jsx';
import { fetchExpenses } from '../requests.jsx';
import axios from "axios";

const Dashboard = () => {
    const token = localStorage.getItem("token");
    const [userData, setUserData] = useState({
        number: "",
        email: "",
        balance: "",
        tariff: {
            title: "",
            bundle: {
                callTime: "",
            }
        }
    });
    const [userExpenses, setUserExpenses] = useState(null);

    // Функция для загрузки расходов
    const loadExpenses = async () => {
        try {
            const expenses = await fetchExpenses();
            setUserExpenses(expenses);
        } catch (err) {
            console.error("Ошибка при загрузке расходов:", err);
        }
    };

    const fetchUserData = async () => {
        try {
            const response = await axios.get("/billingapplication/subscribers/current", {
                headers: {
                    Authorization: `Bearer ${token}`,
                    Accept: "*/*",
                },
            });

            setUserData(response.data);
        } catch (error) {
            console.error("Ошибка при получении данных пользователя:", error);
        }
    };

    // Первичная загрузка данных
    useEffect(() => {
        document.title = "Панель управления";
        fetchUserData()
        loadExpenses();  
    }, [userData]);

    if (!userData || !userExpenses) {
        return (<div className="tariff">
            <ReactLoading type="cylon" color="#FF3B30" height={667} width={375} className='loading'/>;
        </div>)
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
            <AdditionalServices/>
        </div>
    );
};

export default Dashboard;