import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom'
import WelcomePage from './welcome-page.jsx'
import LoginForm from './authorization/user-login.jsx'
import AdminLoginForm from './admin/admin-login.jsx'
import MainPage from './main-page/main-page.jsx'
import Dashboard from './main-page/dashboard.jsx'
import { TariffPage} from './main-page/tariff-page.jsx'
import { Wallet } from './main-page/wallet.jsx'
import { AddServicesPage } from './main-page/add-services-page.jsx'
import { UserSettings } from './main-page/user-settings.jsx'

import { AdminMainPage } from './admin/main-page/admin-main-page.jsx'
import { TariffTable } from './admin/main-page/management/tariff-table.jsx'
import { UserControl } from './admin/main-page/users-control.jsx'
import { UserTable } from './admin/main-page/users-table.jsx'
import { UserAnalytics } from './admin/main-page/user-analytics.jsx'
import { ClientRegisterForm } from './admin/main-page/client-registration.jsx'
import { OperatorRegistrationForm } from './admin/main-page/operator-registration.jsx'
import { AnaliticsPage } from './admin/main-page/analitics/analitics-page.jsx'

import {PleaseAuthorise} from './not-authorized-page.jsx'

import './style.css'

createRoot(document.getElementById('root')).render(
    <StrictMode>
            <BrowserRouter>
                <Routes>
                    <Route path='/' element={<WelcomePage />} />
                    <Route path='/not-authorised' element={<PleaseAuthorise />} />
                    <Route path='login' element={<LoginForm />} />
                    <Route path='operator-login' element={<AdminLoginForm />} />
                    <Route path='operator' element={<AdminMainPage />}>
                        <Route index element={<Navigate to="monitoring" />} />
                        <Route path="management" element={<TariffTable />} />
                        <Route path="monitoring" element={<UserTable/>} />
                        <Route path="user-control" element={<UserControl/>}/>
                        <Route path="user-control/client-registration" element={<ClientRegisterForm/>}/>
                        <Route path="user-control/operator-registration" element={<OperatorRegistrationForm/>}/>
                        <Route path="monitoring/user-analytics/:id" element={<UserAnalytics />} />
                        <Route path="analytics" element={<AnaliticsPage />} />
                    </Route>

                    <Route path="main" element={<MainPage />}>
                        <Route index element={<Navigate to="control" />} />
                        <Route path="control" element={<Dashboard />} />
                        <Route path="tariff" element={<TariffPage />} />
                        <Route path="wallet" element={<Wallet />} />
                        <Route path="add-services" element={<AddServicesPage />} />
                        <Route path="settings" element={<UserSettings />} />
                    </Route>
                </Routes>
            </BrowserRouter>
    </StrictMode>
);