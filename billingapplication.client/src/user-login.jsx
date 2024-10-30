import { useState } from 'react';
import logo from './assets/img/logo.svg';

const LoginForm = () => {
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
            const response = await fetch('https://localhost:7262/Auth/login', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    phoneNumber,
                    password,
                }),
            });

            if (!response.ok) {
                throw new Error('Неправильный номер телефона или пароль');
            }

            const data = await response.json();
            console.log('Авторизация успешна:', data);

            // Дополнительные действия при успешной авторизации
        } catch (error) {
            setErrorMessage(error.message);
        } finally {
            setIsLoading(false);
        }
    };

    return (
        <div className='login'>
            <div className='center-logo'>
                <img className='logo-img1' src={logo} />
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
                {errorMessage && <p className="error">{"Не удалось отправить запрос на сервер"}</p>}
            </form>
        </div>
    );
}; 

export default LoginForm;