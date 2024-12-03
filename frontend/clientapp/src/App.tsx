import React from 'react';
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import PointShop from './components/shop/PointsShop';
import Login from './components/auth/login';
import Dashboard from './components/shop/Dashboard'; // Zorg dat dit pad correct is

function App() {
  return (
    <Router>
      <div className="min-h-screen bg-gray-50">
        <Routes>
          <Route path="/" element={<Login />} />
          <Route path="/dashboard" element={<Dashboard />} />
          <Route path="/point-shop" element={<PointShop />} />
        </Routes>
      </div>
    </Router>
  );
}

export default App;