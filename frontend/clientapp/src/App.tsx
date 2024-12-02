import React, { useState } from 'react';
import PointShop from './components/shop/PointsShop';
import Dashboard from './components/shop/Dashboard';

function App() {
  const [currentPage, setCurrentPage] = useState('dashboard');

  return (
    <div className="min-h-screen bg-gray-50">
      {currentPage === 'dashboard' && <Dashboard onNavigate={setCurrentPage} />}
      {currentPage === 'pointshop' && <PointShop onNavigate={setCurrentPage} />}
    </div>
  );
}

export default App;