import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Card, CardContent, CardDescription, CardFooter, CardHeader, CardTitle } from '../ui/card';
import { Button } from '../ui/button';
import { ShoppingBag, Settings, Users, BookOpen, LogOut, Loader2 } from 'lucide-react';

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
        const response = await fetch('http://localhost:5001/api/user/all', {
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
        if (error instanceof Error && error.message.includes('401')) {
          localStorage.removeItem('token');
          navigate('/');
        }
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
      color: "bg-blue-100",
      path: "/point-shop"
    },
    {
      title: "Opdrachten",
      description: "Bekijk en lever je opdrachten in",
      icon: <BookOpen className="h-6 w-6" />,
      color: "bg-green-100",
      path: "/assignments"
    },
    {
      title: "Groepen",
      description: "Bekijk je projectgroepen",
      icon: <Users className="h-6 w-6" />,
      color: "bg-purple-100",
      path: "/groups"
    },
    {
      title: "Instellingen",
      description: "Pas je voorkeuren aan",
      icon: <Settings className="h-6 w-6" />,
      color: "bg-gray-100",
      path: "/settings"
    }
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
    <div className="container mx-auto p-4 space-y-6">
      {/* Gebruikersprofiel Sectie */}
      <Card className="bg-gradient-to-r from-blue-500 to-blue-600">
        <CardHeader>
          <div className="flex items-center justify-between">
            <div className="flex items-center space-x-4">
              <img
                src="/api/placeholder/32/32"
                alt="Profile"
                className="rounded-full w-16 h-16 border-2 border-white"
              />
              <div className="text-white">
                <h2 className="text-2xl font-bold">{userData.firstname} {userData.lastname}</h2>
                <p className="text-blue-100">{userData.role}</p>
                <p className="text-sm text-blue-100">{userData.email}</p>
              </div>
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

      {/* Welkom Bericht */}
      <Card>
        <CardHeader>
          <CardTitle>Welkom terug, {userData.firstname}!</CardTitle>
          <CardDescription>
            Wat wil je vandaag doen?
          </CardDescription>
        </CardHeader>
      </Card>

      {/* Navigatie Grid */}
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-4">
        {navigationCards.map((card, index) => (
          <Card 
            key={index} 
            className="hover:shadow-lg transition-shadow cursor-pointer"
          >
            <CardHeader>
              <div className={`${card.color} p-3 rounded-full w-fit`}>
                {card.icon}
              </div>
            </CardHeader>
            <CardContent>
              <h3 className="font-bold mb-1">{card.title}</h3>
              <p className="text-sm text-gray-500">{card.description}</p>
            </CardContent>
            <CardFooter>
              <Button 
                variant="secondary" 
                className="w-full justify-center"
                onClick={() => navigate(card.path)}
              >
                Ga naar {card.title}
              </Button>
            </CardFooter>
          </Card>
        ))}
      </div>

      {/* Recent Activiteit of Aankondigingen */}
      <Card>
        <CardHeader>
          <CardTitle>Recente Aankondigingen</CardTitle>
        </CardHeader>
        <CardContent>
          <div className="space-y-4">
            <div className="border-l-4 border-blue-500 pl-4">
              <h4 className="font-semibold">Nieuwe opdracht beschikbaar</h4>
              <p className="text-sm text-gray-500">Frontend Development - React Basics</p>
            </div>
            <div className="border-l-4 border-green-500 pl-4">
              <h4 className="font-semibold">Points Shop Update</h4>
              <p className="text-sm text-gray-500">Nieuwe beloningen toegevoegd!</p>
            </div>
          </div>
        </CardContent>
      </Card>
    </div>
  );
};

export default Dashboard;