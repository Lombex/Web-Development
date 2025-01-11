import React, { useState } from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';

import Login from './app/auth/Login'; 
import PointShop from './app/shop/PointsShop';
import Dashboard from './app/shop/Dashboard';


function App() {
  const [currentPage, setCurrentPage] = useState('dashboard');

  return (
    <div>
      <div className="min-h-screen bg-gray-50">
        {currentPage === 'dashboard' && <Dashboard onNavigate={setCurrentPage} />}
        {currentPage === 'pointshop' && <PointShop onNavigate={setCurrentPage} />}
      </div>

      <Router> {/* Wrap the app with Router */}
        <div className="min-h-screen bg-gray-50">
          <Routes>
            <Route path="/" element={<Login />} /> 
            <Route path="/point-shop" element={<PointShop onNavigate={setCurrentPage} />} /> 
          </Routes>
        </div>
      </Router>
    </div>
  );
}

export default App;