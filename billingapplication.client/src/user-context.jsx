import { createContext, useContext, useState, useEffect } from 'react';
import ReactLoading from 'react-loading';
import axios from 'axios';

const UserContext = createContext();

export const useUser = () => {
    const context = useContext(UserContext);
    return context;
};

export const UserProvider = ({ children }) => {
    const [userData, setUserData] = useState(null);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const token = localStorage.getItem('token');
        if (token) {
            fetchUserData(token);
        } else {
            setLoading(false);
        }
    }, []);

    const fetchUserData = async (token) => {
        setLoading(true);
        try {
            const response = await axios.get('https://localhost:7262/subscribers/current', {
                headers: {
                    'Authorization': `Bearer ${token}`,
                },
            });

            setUserData(response.data);
        } catch (error) {
            console.error("Failed to fetch user data:", error);
        } finally {
            setLoading(false);
        }
    };

    const refreshUserData = async () => {
        const token = localStorage.getItem('token');
        if (token) {
            await fetchUserData(token);
        }
    };

    return (
        <UserContext.Provider value={{ userData, refreshUserData, loading }}>
            {loading ? (
                <div style={{ display: 'flex', justifyContent: 'center', alignItems: 'center', height: '100vh' }}>
                    return (<ReactLoading type="cylon" color="#FF3B30" height={667} width={375} className="loading" />);
                </div>
            ) : (
                children
            )}
        </UserContext.Provider>
    );
};