import { useState } from 'react';
import logo from './assets/img/logo.svg';

const LoginForm = () => {
    const [phoneNumber, setPhoneNumber] = useState('');
    const [password, setPassword] = useState('');
    const [token, setToken] = useState('');

    const handleLogin = async (e) => {
        e.preventDefault();

        const response = await fetch('https://localhost:7262/Auth/login', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Accept': '*/*',
            },
            body: JSON.stringify({ phoneNumber, password }),
        });

        if (response.ok) {
            const data = await response.json();
            setToken(data.token);
            localStorage.setItem('token', data.token); // ��������� ����� � localStorage
        } else {
            console.error('Login failed');
        }
    };

    return (
        <div className='login'>
            <div className='center-logo'>
                <img className='logo-img1' src={logo} />
                <div className='logo-text1'>Alfa-Telecom</div>
            </div>
            <form className='log-form' onSubmit={handleLogin}>
                <input className='phone-input'
                    type="text"
                    value={phoneNumber}
                    onChange={(e) => setPhoneNumber(e.target.value)}
                    placeholder="Номер телефона"
                />
                <input className='password-input'
                    type="password"
                    value={password}
                    onChange={(e) => setPassword(e.target.value)}
                    placeholder="Пароль"
                />
                <a className='forget-pass' href='#'>Забыли пароль?</a>
                <button className='confirm' type="submit">Войти</button>
            </form>
        </div>
    );
};

export default LoginForm;