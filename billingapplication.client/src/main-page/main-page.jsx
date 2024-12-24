import Sidebar from './sidebar.jsx';
import Header from './header.jsx'
import { Outlet } from 'react-router-dom';

const MainPage = () => {

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