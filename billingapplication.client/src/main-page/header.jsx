import logo from '../assets/img/logo.svg';
import { useUser } from '../user-context.jsx';
import { useState, useRef, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';

const Header = () => {
    const { userData, loading } = useUser(); // Деструктурируем данные и статус загрузки
    const [isMenuOpen, setIsMenuOpen] = useState(false);
    const navigate = useNavigate();
    const menuRef = useRef(null);
    const apiUrl = 
        (process.env.BACKEND_HOST && process.env.BACKEND_PORT)
            ? `${process.env.BACKEND_HOST}:${process.env.BACKEND_PORT}`
            : 'http://localhost:5183';

    const toggleMenu = () => {
        setIsMenuOpen(!isMenuOpen);
    };

    const handleLogout = async () => {
        try {
            const response = await fetch(`${apiUrl}/auth/logout`, {
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

    // Обработка загрузки данных пользователя
    if (loading) {
        return <div>Загрузка...</div>;
    }

    // Если данных пользователя нет (например, не авторизован)
    if (!userData) {
        return null; // Или можно перенаправить пользователя
    }

    const splittedUserName = userData.passportInfo.fullName.split(' ');

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
                    <div className="profile-button-email">{userData.email}</div>
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
