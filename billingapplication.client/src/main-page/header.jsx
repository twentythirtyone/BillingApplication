import logo from '../assets/img/logo.svg';
import { useUser } from '../user-context.jsx';
const Header = () => {
    const userData = useUser();
    const splittedUserName = userData.passportInfo.fullName.split(' ');
    return (

        <header className='main-page-header'>
            <div className='header-left'>
                <img src={logo} className='header-logo' />
                <span className='header-title'>Alfa-Telecom</span>
            </div>
            <div className='header-right'>
                <button className='profile-button'>{splittedUserName[0] + ' ' + splittedUserName[1]} <div className='profile-button-email'>{userData.email}</div></button>
            </div>
        </header>
    );
};

export default Header;