import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import logo from '../assets/img/logo.svg';

const AdminLoginForm = () => {
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [isLoading, setIsLoading] = useState(false);
    const [errorMessage, setErrorMessage] = useState('');
    const navigate = useNavigate();

    const apiUrl = 
        (process.env.BACKEND_HOST && process.env.BACKEND_PORT)
            ? `${process.env.BACKEND_HOST}:${process.env.BACKEND_PORT}`
            : 'http://localhost:5183';

    useEffect(() => {
        document.title = 'Войти';
    }, []);

    const handleLogin = async (e) => {
        e.preventDefault();
        setIsLoading(true);
        setErrorMessage('');

        try {
            
            const response = await fetch(`${apiUrl}/auth/login/operator`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    Accept: '*/*',
                },
                body: JSON.stringify({ email, password }),
            });

            if (response.status === 401) {
                throw new Error('Неверный логин или пароль');
            }

            if (!response.ok) {
                throw new Error('Не удалось отправить запрос на сервер');
            }

            const data = await response.json();
            console.log('Авторизация успешна:', data);
            const token = data.token;

            localStorage.setItem('token', token);
            navigate('/operator', { state: { token } });
            window.location.reload();
        } catch (error) {
            setErrorMessage(error.message);
        } finally {
            setIsLoading(false);
        }
    };

    return (
        <div className="login">
            <div className="center-logo">
                <img className="logo-img1" src={logo} alt="Логотип" />
                <div className="logo-text1">Администратор</div>
            </div>
            <form className="log-form" onSubmit={handleLogin}>
                <input
                    className="phone-input"
                    type="email"
                    id="email"
                    value={email}
                    onChange={(e) => setEmail(e.target.value)}
                    placeholder="Email"
                    required
                />
                <input
                    className="password-input"
                    type="password"
                    id="password"
                    value={password}
                    onChange={(e) => setPassword(e.target.value)}
                    placeholder="Пароль"
                    required
                />
                <a className="forget-pass" href="#">
                    Забыли пароль?
                </a>
                <button className="confirm" type="submit" disabled={isLoading}>
                    {isLoading ? 'Загрузка...' : 'Войти'}
                </button>
                {errorMessage && <p className="error">{errorMessage}</p>}
            </form>
        </div>
    );
};

export default AdminLoginForm;