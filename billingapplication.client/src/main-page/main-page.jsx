import { useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import axios from 'axios';
import Sidebar from './sidebar.jsx'; 
import Header from './header.jsx';
import { Outlet } from 'react-router-dom';

const MainPage = () => {
  const navigate = useNavigate();

  useEffect(() => {
    const checkAuthorization = async () => {
      try {
        const token = localStorage.getItem('token');
        if (!token) {
          navigate('/not-authorised');
          return;
        }

        const response = await axios.get('/billingapplication/auth/current', {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        });

        const { roles } = response.data;
        if (!roles.includes('User')) {
          navigate('/not-authorised');
        }
      } catch (error) {
        console.error('Ошибка при проверке авторизации:', error);
        navigate('/not-authorised');
      }
    };

    checkAuthorization();
  }, [navigate]);

  return (
    <div className='main-page'>
      <Header />
      <div className='content-layout'>
        <Sidebar />
        <Outlet />
      </div>
    </div>
  );
};

export default MainPage;