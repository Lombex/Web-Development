import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '../ui/card';
import { Button } from '../ui/button';
import { ShoppingBag, Settings, Users, BookOpen, LogOut, Loader2, Calendar } from 'lucide-react';

interface UserData {
  id: string;
  name: string;
  email: string;
  role: string;
  firstname: string;
  lastname: string;
}

const Dashboard = () => {
  const navigate = useNavigate();
  const [userData, setUserData] = useState<UserData | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchUserData = async () => {
      const token = localStorage.getItem('token');

      if (!token) {
        navigate('/');
        return;
      }

      try {
        const response = await fetch('http://localhost:5001/api/user/fromToken', {
          headers: {
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json',
          },
        });

        if (!response.ok) {
          throw new Error('Failed to fetch user data');
        }

        const data = await response.json();
        setUserData(data);
      } catch (error) {
        console.error('Error fetching user data:', error);
        setError('Failed to load user data');
        localStorage.removeItem('token');
        navigate('/');
      } finally {
        setLoading(false);
      }
    };

    fetchUserData();
  }, [navigate]);

  const handleLogout = () => {
    localStorage.removeItem('token');
    navigate('/');
  };

  const navigationCards = [
    {
      title: "Point Shop",
      description: "Besteed je punten aan beloningen",
      icon: <ShoppingBag className="h-6 w-6" />,
      color: "bg-blue-500 hover:bg-blue-600",
      path: "/point-shop",
    },
    {
      title: "Calendar",
      description: "View and manage office attendance",
      icon: <Calendar className="h-6 w-6" />,
      color: "bg-blue-500 hover:bg-blue-600",
      path: "/calendar",
    },
    {
      title: "Opdrachten",
      description: "Bekijk en lever je opdrachten in",
      icon: <BookOpen className="h-6 w-6" />,
      color: "bg-green-500 hover:bg-green-600",
      path: "/assignments",
    },
    {
      title: "Groepen",
      description: "Bekijk je projectgroepen",
      icon: <Users className="h-6 w-6" />,
      color: "bg-purple-500 hover:bg-purple-600",
      path: "/groups",
    },
    {
      title: "Instellingen",
      description: "Pas je voorkeuren aan",
      icon: <Settings className="h-6 w-6" />,
      color: "bg-gray-500 hover:bg-gray-600",
      path: "/settings",
    },
  ];

  if (loading) {
    return (
      <div className="flex items-center justify-center min-h-screen">
        <Loader2 className="w-8 h-8 animate-spin" />
      </div>
    );
  }

  if (error || !userData) {
    return (
      <div className="flex items-center justify-center min-h-screen">
        <p className="text-red-500">Er is een fout opgetreden bij het laden van je gegevens.</p>
      </div>
    );
  }

  return (
    <div className="container mx-auto p-4 grid grid-cols-1 lg:grid-cols-4 gap-6">
      {/* Sidebar with User Info */}
      <div className="lg:col-span-1">
        <Card className="bg-gradient-to-r from-blue-500 to-blue-600">
          <CardHeader>
            <div className="flex flex-col items-center space-y-4 text-white">
              <img
                src="/api/placeholder/64/64"
                alt="Profile"
                className="rounded-full w-24 h-24 border-2 border-white"
              />
              <div>
                <h2 className="text-xl font-bold">{userData.firstname} {userData.lastname}</h2>
                <p className="text-blue-100">{userData.role}</p>
                <p className="text-sm text-blue-100">{userData.email}</p>
              </div>
              <Button
                variant="secondary"
                onClick={handleLogout}
                className="flex items-center space-x-2"
              >
                <LogOut className="h-4 w-4" />
                <span>Uitloggen</span>
              </Button>
            </div>
          </CardHeader>
        </Card>
      </div>

      {/* Main Dashboard Content */}
      <div className="lg:col-span-3 space-y-6">
        <Card>
          <CardHeader>
            <CardTitle>Welkom terug, {userData.firstname}!</CardTitle>
            <CardDescription>
              Wat wil je vandaag doen?
            </CardDescription>
          </CardHeader>
        </Card>

        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-2 gap-4">
          {navigationCards.map((card, index) => (
            <Card
              key={index}
              className={`hover:shadow-lg transition-shadow cursor-pointer ${card.color}`}
              onClick={() => navigate(card.path)}
            >
              <CardHeader>
                <div className="p-3 rounded-full w-fit">
                  {card.icon}
                </div>
              </CardHeader>
              <CardContent>
                <h3 className="font-bold mb-1">{card.title}</h3>
                <p className="text-sm text-gray-500">{card.description}</p>
              </CardContent>
            </Card>
          ))}
        </div>
      </div>
    </div>
  );
};

export default Dashboard;
