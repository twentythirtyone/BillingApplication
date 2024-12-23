import { useState } from 'react';
import axios from 'axios';

export const OperatorRegistrationForm = () => {
    const [formData, setFormData] = useState({
        email: '',
        nickname: '',
        password: '',
        salt: "string",
        isAdmin: false,
    });

    const [message, setMessage] = useState('');

    const handleChange = (e) => {
        const { name, value, type, checked } = e.target;
        setFormData((prevData) => ({
            ...prevData,
            [name]: type === 'checkbox' ? checked : value,
        }));
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        try {
            const response = await axios.post('https://localhost:7262/auth/register/operator', {
                id: 0,
                email: formData.email,
                nickname: formData.nickname,
                password: formData.password,
                salt: '',
                isAdmin: formData.isAdmin,
            });

            setMessage(`Оператор успешно зарегистрирован: ${response.data}`);
        } catch (error) {
            setMessage(
                error.response?.data?.message || 'При регистрации произошла ошибка'
            );
        }
    };

    return (
        <div>
            <h1>Регистрация нового оператора</h1>
            <form onSubmit={handleSubmit}>
                <div className='clientRegister'>
                    <div>
                        <label htmlFor="email">Email:</label>
                        <input
                            type="email"
                            id="email"
                            name="email"
                            value={formData.email}
                            onChange={handleChange}
                            required
                        />
                    </div>
                    <div>
                        <label htmlFor="nickname">Имя:</label>
                        <input
                            type="text"
                            id="nickname"
                            name="nickname"
                            value={formData.nickname}
                            onChange={handleChange}
                            required
                        />
                    </div>
                    <div>
                        <label htmlFor="password">Пароль:</label>
                        <input
                            type="password"
                            id="password"
                            name="password"
                            value={formData.password}
                            onChange={handleChange}
                            required
                        />
                    </div>
                    </div>
                    <div className='admin-checkbox'>
                        <label htmlFor="isAdmin">
                        Роль админа:
                            <input
                                type="checkbox"
                                id="isAdmin"
                                name="isAdmin"
                                checked={formData.isAdmin}
                                onChange={handleChange}
                            />
                        </label>
                    </div>
                <button className='registerButton' type="submit">Зарегистрировать</button>
            </form>
            {message && <p>{message}</p>}
        </div>
    );
};