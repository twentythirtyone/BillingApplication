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
            fetchUserData(token).catch(() => setLoading(false));
        } else {
            setLoading(false);
        }
    }, []);

    const fetchUserData = async (token) => {
        const apiUrl = 
        (process.env.BACKEND_HOST && process.env.BACKEND_PORT)
            ? `${process.env.BACKEND_HOST}:${process.env.BACKEND_PORT}`
            : 'http://localhost:5183';
        try {
            const response = await axios.get(`${apiUrl}/subscribers/current`, {
                headers: {
                    'Authorization': `Bearer ${token}`,
                },
            });
            setUserData(response.data);
        } catch (error) {
            console.error("Не удалось получить данные пользователя (клиента):", error);
            setUserData(null); 
        } finally {
            setLoading(false);
        }
    };

    const refreshUserData = async () => {
        const token = localStorage.getItem('token');
        if (token) {
            setLoading(true); // ���������� ������ ���� ����� ������ �����
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