import logo from '../assets/img/logo.svg';

const Header = () => {
    return (
        <header className='main-page-header'>
            <div className='header-left'>
                <img src={logo} className='header-logo' />
                <span className='header-title'>Alfa-Telecom</span>
            </div>
            <div className='header-right'>
                <button className='profile-button'>Иванов Иван</button>
            </div>
        </header>
    );
};

export default Header;