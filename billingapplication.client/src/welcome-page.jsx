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
                    <div className='logo-img'>20:31</div>
                    <div className='logo'> Alfa-Telecom</div>
                    <button className='login-btn' onClick={handleNavigation}>
                        Войти
                    </button>
                </header>

                <div className='content'>
                    <h1>Дисклеймер:</h1>
                    <p style={{ color: 'white' }}>Данный сайт является студенческим учебным проектом,
                        созданным исключительно в образовательных целях.
                        Он не является официальным проектом,
                        не связан с деятельностью и не одобрен АО "АЛЬФА-БАНК".
                        Вся представленная информация используется только для обучения,
                        и любой контент, относящийся к АО "АЛЬФА-БАНК",
                        использован в рамках добросовестного использования без умысла на коммерческую выгоду или нарушение прав.</p>
{/*                     <h1>Биллинговая система</h1>
                    <p>Эффективно управляйте своими счетами, платежами и данными клиентов</p> */}
                </div>
            </div>
        </div>
    );
};

export default WelcomePage;