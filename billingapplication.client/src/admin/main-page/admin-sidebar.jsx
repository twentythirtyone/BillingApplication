import { NavLink } from 'react-router-dom';
//import controlIcon from '../../assets/img/sidebar/control.svg';
import tariffIcon from '../../assets/img/sidebar/tariff.svg';
import addServicesIcon from '../../assets/img/sidebar/add-services.svg';
import historyIcon from '../../assets/img/sidebar/history.svg';
import analyticsIcon from '../../assets/img/sidebar/analitics.svg';
//import settingsIcon from '../../assets/img/sidebar/settings.svg';

export const Sidebar = () => {
    const menuItems = [
        //{ path: 'control', name: 'Панель управления', icon: controlIcon },
        { path: 'monitoring', name: 'Клиенты', icon: tariffIcon },
        { path: 'management', name: 'Управление услугами', icon: addServicesIcon },
        { path: 'user-control', name: 'Управление пользователями', icon: historyIcon },
        { path: 'analytics', name: 'Статистика', icon: analyticsIcon },
        //{ path: 'settings', name: 'Настройки', icon: settingsIcon },
    ];

    return (
        <div className='sidebar admin-sidebar'>
            <div className='sidebar-title' style={{color:'white'}}>Billing System</div>
            <div className='sidebar-menu'>
                {menuItems.map((item, index) => (
                    <NavLink to={item.path} key={index} className='sidebar-link admin-link'>
                        <img src={item.icon} className='navbar-icon admin-icon' alt={item.name} />
                        <div className='navbar-link-text admin-sidebar-text'>{item.name}</div>
                    </NavLink>
                ))}
            </div>
        </div>
    );
};