import React from 'react';
import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import PointShop from './components/shop/PointsShop';
import Login from './components/auth/login';
import Dashboard from './components/shop/Dashboard';
import SignUp from './components/auth/Signup';
import Settings from './components/auth/Settings';
import Calendar from './components/calendar/calendar';
import Events from './components/calendar/events';
import AdminDashboard from './components/shop/Admindashboard';
import UserDetails from './components/auth/Userdetails';

function App() {
  return (
    <BrowserRouter>
      <div className="min-h-screen bg-gray-50">
        <Routes>
          <Route path="/" element={<Login />} />
          <Route path="/dashboard" element={<Dashboard />} />
          <Route path="/calendar" element={<Calendar />} />
          <Route path="/point-shop" element={<PointShop />} />
          <Route path="/signup" element={<SignUp />} />
          <Route path="/settings" element={<Settings />} />
          <Route path="/events" element={<Events />} />
          <Route path="/admin" element={<AdminDashboard />} />
          <Route path="/admin/manage-users" element={<UserDetails />} />
          <Route path="*" element={<Navigate to="/" />} />
        </Routes>
      </div>
    </BrowserRouter>
  );
}

export default App;