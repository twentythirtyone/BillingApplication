import { useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
const WelcomePage = () => {
    const navigate = useNavigate();
    useEffect(() => {
        document.title = 'Добро пожаловать!';
    }, []);

    const handleNavigation = () => {
        const targetRoute = '/login';
        navigate(targetRoute, { replace: false });
    };

    return (
        <div className='home'>
            <div className='homepage'>
                <header className='navbar'>
                    <img className='logo-img' src='src/assets/img/logo.svg' alt='Alfa-Telecom' />
                    <div className='logo'>Alfa-Telecom</div>
                    <button className='login-btn' onClick={handleNavigation}>
                        Войти
                    </button>
                </header>

                <div className='content'>
                    <h1>Биллинговая система</h1>
                    <p>Эффективно управляйте своими счетами, платежами и данными клиентов</p>
                </div>
            </div>
        </div>
    );
};

export default WelcomePage;