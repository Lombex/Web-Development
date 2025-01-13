import React, { ReactNode } from 'react';
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
import Achievements from './components/achievements/Achievements';
import Layout from './components/layout/Layout';

interface ProtectedRouteProps {
  children: ReactNode;
  adminRequired?: boolean;
}

const ProtectedRoute: React.FC<ProtectedRouteProps> = ({ children, adminRequired = false }) => {
  const token = localStorage.getItem('token');
  const userRole = localStorage.getItem('userRole');

  if (!token) {
    return <Navigate to="/" />;
  }

  if (adminRequired && userRole !== 'Admin') {
    return <Navigate to="/dashboard" />;
  }

  return <Layout>{children}</Layout>;
};

function App() {
  return (
    <BrowserRouter>
      <div className="min-h-screen bg-gray-50">
        <Routes>
          {/* Public Routes */}
          <Route path="/" element={<Login />} />
          <Route path="/signup" element={<SignUp />} />

          {/* Protected User Routes */}
          <Route path="/dashboard" element={
            <ProtectedRoute>
              <Dashboard />
            </ProtectedRoute>
          } />
          
          <Route path="/calendar" element={
            <ProtectedRoute>
              <Calendar />
            </ProtectedRoute>
          } />
          
          <Route path="/point-shop" element={
            <ProtectedRoute>
              <PointShop />
            </ProtectedRoute>
          } />
          
          <Route path="/settings" element={
            <ProtectedRoute>
              <Settings />
            </ProtectedRoute>
          } />
          
          <Route path="/events" element={
            <ProtectedRoute>
              <Events />
            </ProtectedRoute>
          } />
          
          <Route path="/achievements" element={
            <ProtectedRoute>
              <Achievements />
            </ProtectedRoute>
          } />

          {/* Protected Admin Routes */}
          <Route path="/admin" element={
            <ProtectedRoute adminRequired>
              <AdminDashboard />
            </ProtectedRoute>
          } />
          
          <Route path="/admin/manage-users" element={
            <ProtectedRoute adminRequired>
              <UserDetails />
            </ProtectedRoute>
          } />

          {/* Catch all route */}
          <Route path="*" element={<Navigate to="/" />} />
        </Routes>
      </div>
    </BrowserRouter>
  );
}

export default App;