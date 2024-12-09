import { createContext, useContext, useState, useEffect } from 'react';
import ReactLoading from 'react-loading';
import axios from 'axios';

const UserContext = createContext();

export const useUser = () => {
    const context = useContext(UserContext);
    if (!context) {
        throw new Error("useUser must be used within a UserProvider");
    }
    return context;
};

export const UserProvider = ({ children }) => {
    const [userData, setUserData] = useState(null);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const token = localStorage.getItem('token');
        if (token) {
            fetchUserData(token).catch(() => setLoading(false)); // Обработка ошибки
        } else {
            setLoading(false); // Нет токена — завершаем загрузку
        }
    }, []);

    const fetchUserData = async (token) => {
        try {
            const response = await axios.get('https://localhost:7262/subscribers/current', {
                headers: {
                    'Authorization': `Bearer ${token}`,
                },
            });
            setUserData(response.data);
        } catch (error) {
            console.error("Ошибка при получении данных пользователя:", error);
            setUserData(null); // Обнуляем данные пользователя при ошибке
        } finally {
            setLoading(false); // Устанавливаем `loading` в `false`, даже при ошибке
        }
    };

    const refreshUserData = async () => {
        const token = localStorage.getItem('token');
        if (token) {
            setLoading(true); // Обновление данных тоже может занять время
            await fetchUserData(token);
        }
    };

    if (loading) {
        return (
            <div style={{ display: 'flex', justifyContent: 'center', alignItems: 'center', height: '100vh' }}>
                <ReactLoading type="cylon" color="#FF3B30" height={667} width={375} className="loading" />
            </div>
        );
    }

    return (
        <UserContext.Provider value={{ userData, refreshUserData, loading }}>
            {children}
        </UserContext.Provider>
    );
};