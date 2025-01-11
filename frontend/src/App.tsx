import React from 'react';
import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import PointShop from './app/shop/PointsShop';
import Login from './app/auth/Login';
import Dashboard from './app/shop/Dashboard';
import SignUp from './app/auth/Signup';
import Settings from './app/auth/settings';
import Calendar from './app/calendar/calendar';
import Events from './app/calendar/events';

function App() {
  return (
    <BrowserRouter>
      <div className="min-h-screen bg-gray-50">
        <Routes>
          <Route path="/a" element={<Login />} />
          <Route path="/" element={<Events />} />
          <Route path="/dashboard" element={<Dashboard />} />
          <Route path="/point-shop" element={<PointShop />} />
          <Route path="/calendar" element={<Calendar />} />
          <Route path="/signup" element={<SignUp />} />
          <Route path="/settings" element={<Settings />} />
          <Route path="*" element={<Navigate to="/" />} />
        </Routes>
      </div>
    </BrowserRouter>
  );
}

export default App;