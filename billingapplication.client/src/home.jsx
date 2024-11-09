const HomePage = () => {

    return (
        <div className='homepage'>
            <header className='navbar'>
                <img className ='logo-img' src='src/assets/img/logo.svg' alt='Alfa-Telecom'></img>
                <div className='logo'>Alfa-Telecom</div>
                <button className='login-btn'>Войти</button>
            </header>
            <div className='content'>
                <h1>Биллинговая система</h1>
                <p>Эффективно управляйте своими счетами, платежами и данными клиентов</p>
            </div>
        </div>
    );
};

export default HomePage;