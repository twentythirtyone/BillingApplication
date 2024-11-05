import {useState, useEffect } from 'react'
import Sidebar from './sidebar.jsx';
import Header from './header.jsx'
import { Outlet } from 'react-router-dom';

const MainPage = () => {
    const [userData, setUserData] = useState(null);
    const token = localStorage.getItem('token');

    useEffect(() => {
        if (token) {
            fetchUserData();
        }
    }, [token]);

    const fetchUserData = async () => {
        try {
            const response = await fetch("https://localhost:7262/Subscriber/getcurrentuser", {
                method: 'GET',
                headers: {
                    'Authorization': `Bearer ${token}`,
                    'Content-Type': 'application/json'
                }
            });

            if (response.ok) {
                const data = await response.json();
                setUserData(data);
                localStorage.setItem('userData', JSON.stringify(userData));
            } else {
                console.error("Failed to fetch user data");
            }
        } catch (error) {
            console.error("Error:", error);
        }
    };

    return (
        <div className='main-page'>
            <Header />
            <div className='content-layout'>
                <Sidebar />
                <div className='main-page-content'>
                    <Outlet />
                </div>
            </div>
        </div>
    );
};

export default MainPage;