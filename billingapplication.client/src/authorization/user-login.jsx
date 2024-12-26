import { useNavigate } from 'react-router-dom';
import { useState, useEffect } from 'react';
import logo from '../assets/img/logo.svg';

const LoginForm = () => {
    useEffect(() => {
        document.title = 'Войти';
    });
    const navigate = useNavigate();
    const [phoneNumber, setPhoneNumber] = useState('');
    const [password, setPassword] = useState('');
    const [errorMessage, setErrorMessage] = useState('');
    const [isLoading, setIsLoading] = useState(false);

    const handlePhoneNumberChange = (e) => {
        setPhoneNumber(e.target.value);
    };

    const handlePasswordChange = (e) => {
        setPassword(e.target.value);
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        setIsLoading(true);
        setErrorMessage('');

        try {
            const response = await fetch('/billingapplication/Auth/login', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    phoneNumber,
                    password,
                }),
            });

            if (response.status === 401 || response.status === 400) {
                throw new Error('Неверный логин или пароль');
            }

            if (!response.ok) {
                throw new Error(response.status);
            }

            const data = await response.json();
            console.log('Авторизация успешна:', data);
            const token = data.token;
            localStorage.setItem('token', token);
            navigate('/main', { state: { token } });
            window.location.reload();
        } catch (error) {
            setErrorMessage(error.message);
        } finally {
            setIsLoading(false);
        }
    };

    return (
        <div className='login'>
            <div className='center-logo'>
                <img className='logo-img1' src={logo} alt="Логотип" />
                <div className='logo-text1'>Alfa-Telecom</div>
            </div>
            <form className='log-form' onSubmit={handleSubmit}>
                <input className='phone-input'
                    type="text"
                    id="phoneNumber"
                    value={phoneNumber}
                    onChange={handlePhoneNumberChange}
                    placeholder="Номер телефона"
                    required
                />
                <input className='password-input'
                    type="password"
                    id="password"
                    value={password}
                    onChange={handlePasswordChange}
                    placeholder="Пароль"
                    required
                />
                <a className='forget-pass' href='#'>Забыли пароль?</a>
                <button className='confirm' type="submit" disabled={isLoading}>
                    {isLoading ? 'Загрузка...' : 'Войти'}
                </button>
                {errorMessage && <p className="error">{errorMessage}</p>}
            </form>
        </div>
    );
};

export default LoginForm;