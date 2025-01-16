import { Sidebar } from './admin-sidebar.jsx';
import { Header }  from './header.jsx'
import { Outlet }  from 'react-router-dom';
import axios from 'axios';
import { useEffect } from 'react';
import { useNavigate } from 'react-router-dom';

export const AdminMainPage = () => {
    const navigate = useNavigate();

    useEffect(() => {
      document.body.style.backgroundColor = '#1E2026';
      document.body.style.color = '#FFFFFF';
  
      return () => {
        document.body.style.backgroundColor = '';
        document.body.style.color = '';
      };
    }, []);

    const checkAuthorization = async () => {
        try {
          const token = localStorage.getItem('token');
          if (!token) {
            navigate('/operator-login');
            return;
          }
    
          const response = await axios.get('/billingapplication/auth/current', {
            headers: {
              Authorization: `Bearer ${token}`,
            },
          });
    
          const { roles } = response.data;
          if (!roles.includes('Admin') && !roles.includes('Operator')) {
            navigate('/operator-login');
        }
        } catch (error) {
          console.error('Ошибка при проверке авторизации:', error);
          navigate('/operator-login');
        }
      };
    
      useEffect(() => {
        checkAuthorization();
      }, [navigate]);

    return (
        <div className='admin-main-page'>
            <Header />
            <div className='admin-content-layout '>
            <Sidebar />
            <Outlet />
            </div>
        </div>
    );
};
