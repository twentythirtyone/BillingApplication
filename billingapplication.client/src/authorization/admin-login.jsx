import { useState, useEffect } from 'react';
import logo from '../assets/img/logo.svg';

const AdminLoginForm = () => {
    useEffect(() => {
        document.title = 'AdminLogin';
    });

    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [token, setToken] = useState('');

    const handleLogin = async (e) => {
        e.preventDefault();

        const response = await fetch('https://localhost:7262/Auth/operator', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Accept': '*/*',
            },
            body: JSON.stringify({ email, password }),
        });

        if (response.ok) {
            const data = await response.json();
            setToken(data.token);
            localStorage.setItem('token', data.token);
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
                    value={email}
                    onChange={(e) => setEmail(e.target.value)}
                    placeholder="Логин или почта"
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

export default AdminLoginForm;