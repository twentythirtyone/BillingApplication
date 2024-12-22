import { Sidebar } from './admin-sidebar.jsx';
import { Header }  from './header.jsx'
import { Outlet }  from 'react-router-dom';

export const AdminMainPage = () => {

    return (
        <div className='main-page admin'>
            <Header />
            <div className='content-layout admin'>
            <Sidebar />
            <Outlet />
            </div>
        </div>
    );
};
