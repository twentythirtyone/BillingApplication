import { StrictMode } from 'react'
import { UserProvider } from './user-context.jsx';
import { createRoot } from 'react-dom/client'
import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom'
import WelcomePage from './welcome-page.jsx'
import LoginForm from './authorization/user-login.jsx'
import AdminLoginForm from './authorization/admin-login.jsx'
import MainPage from './main-page/main-page.jsx'
import Dashboard from './main-page/dashboard.jsx'
import Wallet from './main-page/wallet.jsx'
import './style.css'

createRoot(document.getElementById('root')).render(
    <StrictMode>
        <UserProvider>
            <BrowserRouter>
                <Routes>
                    <Route path='/' element={<WelcomePage />} />
                    <Route path='login' element={<LoginForm />} />
                    <Route path='operator' element={<AdminLoginForm />} />
                    <Route path="main" element={<MainPage />}>
                        <Route index element={<Navigate to="control" />} />
                        <Route path="control" element={<Dashboard />} />
                        <Route path="tariff" element={<Dashboard />} />
                        <Route path="wallet" element={<Wallet />} />
                        <Route path="add-services" element={<Dashboard />} />
                        <Route path="history" element={<Dashboard />} />
                        <Route path="analytics" element={<Dashboard />} />
                        <Route path="settings" element={<Dashboard />} />
                    </Route>
                </Routes>
            </BrowserRouter>
        </UserProvider>
    </StrictMode>
);