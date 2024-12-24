import { useNavigate } from 'react-router-dom';

export const UserControl = () => {
    const navigate = useNavigate();

    const goToUserRegister = () => {
        navigate(`/operator/user-control/client-registration`);
    };

    const goToOperatorRegister = () => {
        navigate(`/operator/user-control/operator-registration`);
    };

    return (
        <div className="user-analytics">
            <h1>Регистрация пользователей</h1>
            <div className='register-buttons'>
                <button className='register-client' onClick={goToUserRegister}>Зарегистрировать нового клиента</button>
                <button className='register-client' onClick={goToOperatorRegister}>Зарегистрировать нового оператора</button>
            </div>
        </div>
    );
};