import logo from '../assets/img/logo.svg';
import { useState, useRef, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import axios from "axios";

const Header = () => {
    const token = localStorage.getItem("token");
    const [userData, setUserData] = useState({
        email: "...",
        passportInfo: {
            fullName: '... '
        }
    });
    const [isMenuOpen, setIsMenuOpen] = useState(false);
    const navigate = useNavigate();
    const menuRef = useRef(null);

    const toggleMenu = () => {
        setIsMenuOpen(!isMenuOpen);
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

    const handleLogout = async () => {
        try {
            const response = await fetch(`/billingapplication/auth/logout`, {
                method: 'POST',
                headers: {
                    'Accept': '*/*',
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${localStorage.getItem('token')}`,
                },
            });

            if (response.ok) {
                localStorage.removeItem('token');
                navigate('/');
                console.log('Вы вышли');
            } else {
                console.error('Ошибка при выполнении выхода', response.status);
            }
        } catch (error) {
            console.error('Ошибка сети или сервера:', error);
        }
    };

    useEffect(() => {
        fetchUserData();
        const handleClickOutside = (event) => {
            if (menuRef.current && !menuRef.current.contains(event.target)) {
                setIsMenuOpen(false);
            }
        };
        document.addEventListener('mousedown', handleClickOutside);
        return () => {
            document.removeEventListener('mousedown', handleClickOutside);
        };
    }, []);


    const userName = userData.passportInfo.fullName || 'Загрузка...'
    const splittedUserName = userName.split(' ') ;

    return (
        <header className="main-page-header">
            <div className="header-left">
                <img src={logo} className="header-logo" alt="Logo" />
                <span className="header-title">Alfa-Telecom</span>
            </div>

            <div className="header-right" ref={menuRef}>
                <button className="profile-button" onClick={toggleMenu}>
                    <img className="profile-pic" src="..\src\assets\img\avatar.svg" alt="Avatar" />
                    {splittedUserName[0] + ' ' + splittedUserName[1]}
                    <div className="profile-button-email">{userData.email || 'Загрузка...'}</div>
                </button>
                {isMenuOpen && (
                    <div className="profile-menu">
                        <button onClick={handleLogout} className="profile-menu-item">
                            Выйти
                        </button>
                    </div>
                )}
            </div>
        </header>
    );
};

export default Header;
