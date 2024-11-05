import { NavLink } from 'react-router-dom';
import controlIcon from '../assets/img/sidebar/control.svg';
import tariffIcon from '../assets/img/sidebar/tariff.svg';
import walletIcon from '../assets/img/sidebar/wallet.svg';
import addServicesIcon from '../assets/img/sidebar/add-services.svg';
import historyIcon from '../assets/img/sidebar/history.svg';
import analyticsIcon from '../assets/img/sidebar/analitics.svg';
import settingsIcon from '../assets/img/sidebar/settings.svg';

const Sidebar = () => {
    const menuItems = [
        { path: 'control', name: 'Панель управления', icon: controlIcon },
        { path: 'tariff', name: 'Тариф', icon: tariffIcon },
        { path: 'wallet', name: 'Кошелек', icon: walletIcon },
        { path: 'add-services', name: 'Доп услуги', icon: addServicesIcon },
        { path: 'history', name: 'История', icon: historyIcon },
        { path: 'analytics', name: 'Аналитика', icon: analyticsIcon },
        { path: 'settings', name: 'Настройки', icon: settingsIcon },
    ];

    return (
        <div className='sidebar'>
            <div className='sidebar-title'>Billing System</div>
            <div className='sidebar-menu'>
                {menuItems.map((item, index) => (
                    <NavLink to={item.path} key={index} className='sidebar-link'>
                        <img src={item.icon} className='navbar-icon' alt={item.name} />
                        <div className='navbar-link-text'>{item.name}</div>
                    </NavLink>
                ))}
            </div>
        </div>
    );
};

export default Sidebar;