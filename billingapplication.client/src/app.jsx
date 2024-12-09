import { StrictMode } from 'react'
import { UserProvider } from './user-context.jsx';
import { createRoot } from 'react-dom/client'
import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom'
import WelcomePage from './welcome-page.jsx'
import LoginForm from './authorization/user-login.jsx'
import AdminLoginForm from './admin/admin-login.jsx'
import MainPage from './main-page/main-page.jsx'
import Dashboard from './main-page/dashboard.jsx'
import Tariff from './main-page/tariff.jsx'
import Wallet from './main-page/wallet.jsx'

import { AdminMainPage } from './admin/main-page/admin-main-page.jsx'
import { Control } from './admin/main-page/control.jsx'
import { TariffTable } from './admin/main-page/management/tariff-table.jsx'
import './style.css'

createRoot(document.getElementById('root')).render(
    <StrictMode>
        <UserProvider>
            <BrowserRouter>
                <Routes>
                    <Route path='/' element={<WelcomePage />} />
                    <Route path='login' element={<LoginForm />} />
                    <Route path='operator-login' element={<AdminLoginForm />} />
                    <Route path='operator' element={<AdminMainPage />}>
                        <Route index element={<Navigate to="control" />} />
                        <Route path="control" element={<Control />} />
                        <Route path="monitoring" element={<Control />} />
                        <Route path="management" element={<TariffTable />} />
                        <Route path="user-control" element={<Control />} />
                        <Route path="analytics" element={<Control />} />
                        <Route path="settings" element={<Control />} />
                    </Route>

                    <Route path="main" element={<MainPage />}>
                        <Route index element={<Navigate to="control" />} />
                        <Route path="control" element={<Dashboard />} />
                        <Route path="tariff" element={<Tariff />} />
                        <Route path="wallet" element={<Wallet />} />
                        <Route path="add-services" element={<Dashboard />} />
                        <Route path="history" element={<Dashboard />} />
                        <Route path="settings" element={<Dashboard />} />
                    </Route>
                </Routes>
            </BrowserRouter>
        </UserProvider>
    </StrictMode>
);