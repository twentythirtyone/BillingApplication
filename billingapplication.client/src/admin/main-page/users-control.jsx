import { useNavigate } from 'react-router-dom';
import axios from 'axios';
import { useEffect, useState } from 'react';

export const UserControl = () => {
    const [isAdmin, setIsAdmin] = useState(false);
    const navigate = useNavigate();
    const token = localStorage.getItem('token');

    const goToUserRegister = () => {
        navigate(`/operator/user-control/client-registration`);
    };

    const goToOperatorRegister = () => {
        navigate(`/operator/user-control/operator-registration`);
    };

    const checkUserRole = async () => {
        try {
          const response = await axios.get('/billingapplication/auth/current', {
            headers: {
              Authorization: `Bearer ${token}`,
            },
          });
    
          const { roles } = response.data;
          setIsAdmin(roles.includes('Admin'));
        } catch (error) {
          console.error('Ошибка при проверке роли:', error);
        }
      };
      useEffect(() => {
        document.title = 'Управление пользователями';

        checkUserRole();
      })

    return (
        <div className="user-analytics">
            <h1>Регистрация пользователей</h1>
            <div className='register-buttons'>
                <button className='register-client' onClick={goToUserRegister}>Зарегистрировать нового клиента</button>
                {isAdmin && <button className='register-client' onClick={goToOperatorRegister}>Зарегистрировать нового оператора</button>}
            </div>
        </div>
    );
};