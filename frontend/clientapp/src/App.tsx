import React from 'react';
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import PointShop from './components/shop/PointsShop';
import Login from './components/auth/login';
import Dashboard from './components/shop/Dashboard'; // Zorg dat dit pad correct is
import SignUp from './components/auth/Signup'; // Zorg dat dit pad correct is
import Settings from './components/auth/Settings'; // Zorg dat dit pad correct is


function App() {
  return (
    <Router>
      <div className="min-h-screen bg-gray-50">
        <Routes>
          <Route path="/" element={<Login />} />
          <Route path="/dashboard" element={<Dashboard />} />
          <Route path="/point-shop" element={<PointShop />} />
          <Route path="/signup" element={<SignUp />} />
          <Route path="/settings" element={<Settings />} />
        </Routes>
      </div>
    </Router>
  );
}

export default App;