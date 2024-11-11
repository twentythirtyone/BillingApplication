import logo from '../assets/img/logo.svg';
import { useUser } from '../user-context.jsx';
import { useState, useRef, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';

const Header = () => {
    const userData = useUser();  // Получаем данные пользователя через хук
    if (!userData) {
        return <div>Загрузка...</div>;  // Пока данные не загружены, показываем индикатор загрузки
    }
    const splittedUserName = userData.passportInfo.fullName.split(' ');
    const [isMenuOpen, setIsMenuOpen] = useState(false);
    const navigate = useNavigate();
    const menuRef = useRef(null);

    const toggleMenu = () => {
        setIsMenuOpen(!isMenuOpen);
    };

    const handleEditProfile = () => {
        // Функция редактирования профиля
    };

    const handleLogout = async () => {
        try {
            const response = await fetch('https://localhost:7262/auth/logout', {
                method: 'POST',
                headers: {
                    'Accept': '*/*',
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${ localStorage.getItem('token') }`
                }
            });

    if (response.ok) {
        localStorage.removeItem('token');
        navigate('/');
        console.log('Вы вышли')
    } else {
        console.error("Ошибка при выполнении выхода", response.status);
    }
} catch (error) {
    console.error("Ошибка сети или сервера:", error);
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

return (
    <header className='main-page-header'>
        <div className='header-left'>
            <img src={logo} className='header-logo' alt="Logo" />
            <span className='header-title'>Alfa-Telecom</span>
        </div>
        <div className='header-right' ref={menuRef}>
            <button className='profile-button' onClick={toggleMenu}>
                {splittedUserName[0] + ' ' + splittedUserName[1]}
                <div className='profile-button-email'>{userData.email}</div>
            </button>
            {isMenuOpen && (
                <div className='profile-menu'>
                    <button onClick={handleEditProfile} className='profile-menu-item'>Редактировать профиль</button>
                    <button onClick={handleLogout} className='profile-menu-item'>Выйти</button>
                </div>
            )}
        </div>
    </header>
);
};

export default Header