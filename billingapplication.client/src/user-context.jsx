import { createContext, useContext, useState, useEffect } from 'react';

const UserContext = createContext();

export const useUser = () => useContext(UserContext);

export const UserProvider = ({ children }) => {
    const [userData, setUserData] = useState(null);

    useEffect(() => {
        // Загрузка данных пользователя при первой загрузке контекста
        const token = localStorage.getItem('token');
        if (token) {
            fetchUserData(token);
        }
    }, []);

    const fetchUserData = async () => {
        try {
            const response = await fetch('https://localhost:7262/Subscriber/getcurrentuser', {
                method: 'POST',
                headers: {
                    'Accept': '*/*',
                    'Content-Type': 'application/json',  // Укажите нужный тип контента, если требуется
                    'Authorization': `Bearer ${localStorage.getItem('token')}` // Добавьте токен, если он необходим для авторизации
                },
                body: JSON.stringify({}) // Пустое тело, если сервер не требует данных
            });

            if (!response.ok) {
                throw new Error(`Ошибка: ${response.status}`);
            }

            const data = await response.json();
            setUserData(data);
        } catch (error) {
            console.error("Failed to fetch user data:", error);
        }
    };

    return (
        <UserContext.Provider value={userData}>
            {children}
        </UserContext.Provider>
    );
};