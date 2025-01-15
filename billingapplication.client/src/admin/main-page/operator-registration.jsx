import { useState } from 'react';

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
            const token = localStorage.getItem('token');

            const requestBody = {
                id: 0,
                email: formData.email,
                nickname: formData.nickname,
                password: formData.password,
                salt: 'string',
                isAdmin: formData.isAdmin,
            }

            const response = await fetch(
                '/billingapplication/auth/register/operator',
                {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                        Authorization: `Bearer ${token}`,
                    },
                    body: JSON.stringify(requestBody),
                }
            );

            if (!response.ok) {
                const errorData = await response.text();
                throw new Error(errorData.message || 'Произошла ошибка при регистрации');
            }

            const responseData = await response.json();
            setMessage(`Оператор успешно зарегистрирован: ${responseData}`);
        } catch (error) {
            console.error('Ошибка запроса:', error.message);
            setMessage(error.message || 'При регистрации произошла ошибка');
        }
    };

    return (
        <div>
            <h1>Регистрация нового оператора</h1>
            <form onSubmit={handleSubmit}>
                <div className='clientRegister'>
                    <div>
                        <input
                            placeholder='email'
                            type="email"
                            id="email"
                            name="email"
                            value={formData.email}
                            onChange={handleChange}
                            required
                        />
                    </div>
                    <div>
                        <input
                        placeholder='email'
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
                <button className='registerButton' type="submit">
                    Зарегистрировать
                </button>
            </form>
            {message && <p>{message}</p>}
        </div>
    );
};