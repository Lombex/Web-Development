import React from 'react';
import { useNavigate } from 'react-router-dom';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '../ui/card';
import { Users, Settings, Tags, Calendar, LogOut } from 'lucide-react';
import { Button } from '../ui/button';

const AdminDashboard = () => {
  const navigate = useNavigate();

  const handleLogout = () => {
    localStorage.removeItem('token');
    navigate('/');
  };
// werkt 
  const adminNavigationCards = [
    {
      title: "Manage Users",
      description: "View, edit, or remove user accounts",
      icon: <Users className="h-6 w-6 text-white" />,
      color: "bg-purple-500 hover:bg-purple-600",
      path: "/admin/manage-users",
    },
    {
      title: "Calendar",
      description: "View and manage office attendance",
      icon: <Calendar className="h-6 w-6 text-white" />,
      color: "bg-blue-500 hover:bg-blue-600",
      path: "/calendar",
    },
    {
      title: "Events",
      description: "Manage events and preferences",
      icon: <Tags className="h-6 w-6 text-white" />,
      color: "bg-red-500 hover:bg-red-600",
      path: "/events",
    },
  ];

  return (
    <div className="container mx-auto p-4 grid grid-cols-1 lg:grid-cols-4 gap-6">
      {/* Sidebar with Admin Info */}
      <div className="lg:col-span-1">
        <Card className="bg-gradient-to-r from-purple-500 to-purple-600">
          <CardHeader>
            <div className="flex flex-col items-center space-y-4 text-white">
              <img
                src="/api/placeholder/64/64"
                alt="Admin Profile"
                className="rounded-full w-24 h-24 border-2 border-white"
              />
              <div>
                <h2 className="text-xl font-bold">Admin</h2>
              </div>
              <Button
                variant="secondary"
                onClick={handleLogout}
                className="flex items-center space-x-2"
              >
                <LogOut className="h-4 w-4" />
                <span>Log Out</span>
              </Button>
            </div>
          </CardHeader>
        </Card>
      </div>

      {/* Main Admin Dashboard Content */}
      <div className="lg:col-span-3 space-y-6">
        <Card>
          <CardHeader>
            <CardTitle>Welcome back, Admin!</CardTitle>
            <CardDescription>What would you like to change today?</CardDescription>
          </CardHeader>
        </Card>

        {/* Admin Navigation Cards */}
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-2 gap-4">
          {adminNavigationCards.map((card, index) => (
            <Card
              key={index}
              className={`cursor-pointer transition-shadow hover:shadow-lg ${card.color}`}
              onClick={() => navigate(card.path)}
            >
              <CardHeader>
                <div className="text-white p-3 rounded-full w-fit">
                  {card.icon}
                </div>
              </CardHeader>
              <CardContent>
                <h3 className="font-bold text-white">{card.title}</h3>
                <p className="text-sm text-white/80">{card.description}</p>
              </CardContent>
            </Card>
          ))}
        </div>
      </div>
    </div>
  );
};

export default AdminDashboard;
