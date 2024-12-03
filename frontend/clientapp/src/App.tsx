import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import PointShop from './components/shop/PointsShop'; 
import Login from './components/auth/login'; 

function App() {
  return (
    <Router> {/* Wrap the app with Router */}
      <div className="min-h-screen bg-gray-50">
        <Routes>
          <Route path="/" element={<Login />} /> 
          <Route path="/point-shop" element={<PointShop />} /> 
        </Routes>
      </div>
    </Router>
  );
}

export default App;
