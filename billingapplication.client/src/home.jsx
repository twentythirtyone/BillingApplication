import { useState, useRef, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';

const HomePage = () => {
    const [showRoleOptions, setShowRoleOptions] = useState(false);
    const roleOptionsRef = useRef(null);
    const navigate = useNavigate();

    const toggleRoleOptions = () => {
        setShowRoleOptions(!showRoleOptions);
    };

    useEffect(() => {
        document.title = 'Главная';

        const handleClickOutside = (event) => {
            if (roleOptionsRef.current && !roleOptionsRef.current.contains(event.target)) {
                setShowRoleOptions(false);
            }
        };

        document.addEventListener('mousedown', handleClickOutside);
        return () => {
            document.removeEventListener('mousedown', handleClickOutside);
        };
    }, []);

    return (
        <div className='home'>
            <div className='homepage'>
                <header className='navbar'>
                    <img className='logo-img' src='src/assets/img/logo.svg' alt='Alfa-Telecom' />
                    <div className='logo'>Alfa-Telecom</div>
                    <button className='login-btn' onClick={toggleRoleOptions}>
                        Войти
                    </button>
                    {showRoleOptions && (
                        <div className='role-options' ref={roleOptionsRef}>
                            <button className='role-option' onClick={() => navigate('Auth/operator', { replace: false })}>
                                Оператор
                            </button>
                            <button className='role-option' onClick={() => navigate('Auth/login', { replace: false })}>
                                Пользователь
                            </button>
                        </div>
                    )}
                </header>

                <div className='content'>
                    <h1>Биллинговая система</h1>
                    <p>Эффективно управляйте своими счетами, платежами и данными клиентов</p>
                </div>
            </div>
        </div >
    );
};


export default HomePage;