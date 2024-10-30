import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import { BrowserRouter, Routes, Route } from 'react-router-dom'
import Home from './home.jsx'
import LoginForm from './user-login.jsx'
import AdminLoginForm from './admin-login.jsx'
import './style.css'

createRoot(document.getElementById('root')).render(
    <StrictMode>
     <BrowserRouter>
        <Routes>
           <Route path='*' element={<Home />} />
           <Route path='Auth/login' element={<LoginForm />} />
                <Route path='Auth/operator' element={<AdminLoginForm />} />
        </Routes>
     </BrowserRouter>
  </StrictMode>,
)
