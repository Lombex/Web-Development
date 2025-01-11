import React from 'react';
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import Login from './components/auth/login';
import Dashboard from './components/shop/Dashboard';
import Calendar from './components/calendar/calendar';
import PointShop from './components/shop/PointsShop';
import SignUp from './components/auth/Signup';
import Settings from './components/auth/Settings';

function App() {
  return (
    <Router>
      <div className="min-h-screen bg-white"> {/* Changed to bg-white */}
        <Routes>
          <Route path="/" element={<Login />} />
          <Route path="/dashboard" element={<Dashboard />} />
          <Route path="/calendar" element={<Calendar />} />
          <Route path="/point-shop" element={<PointShop />} />
          <Route path="/signup" element={<SignUp />} />
          <Route path="/settings" element={<Settings />} />
          <Route path="*" element={<Navigate to="/" />} />
        </Routes>
      </div>
    </Router>
  );
}

export default App;