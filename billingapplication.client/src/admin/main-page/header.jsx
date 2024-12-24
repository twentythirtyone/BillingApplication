import logo from '../../assets/img/logo.svg'
import  {useNavigate} from 'react-router-dom';

export const Header = () => {
    const navigate = useNavigate();
    const handleLogout = async () => {
        try {
            const response = await fetch('http://billing-app-server:5183/auth/logout', {
                method: 'POST',
                headers: {
                    'Accept': '*/*',
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${localStorage.getItem('token')}`,
                },
            });

            if (response.ok) {
                localStorage.removeItem('token');
                navigate('/operator-login');
            } else {
                console.error('Ошибка при выполнении выхода', response.status);
            }
        } catch (error) {
            console.error('Ошибка сети или сервера:', error);
        }
    };

    return (
        <header className="admin-header">
            <div className='header-left'>
                <img src={logo} className="header-logo" alt="Logo" />
                <span className="header-title">Alfa-Telecom</span>
            </div>

            <div className="header-right">
                <button className="admin-profile-button" onClick={handleLogout}>
                Выйти
                </button>
            </div>
        </header>
    );
};