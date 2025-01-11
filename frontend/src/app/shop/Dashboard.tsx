import React from 'react';
import { Card, CardContent, CardDescription, CardFooter, CardHeader, CardTitle } from '../../components/ui/card';
import { Button } from '../../components/ui/button';
import { ShoppingBag, Settings, Users, BookOpen } from 'lucide-react';

interface DashboardProps {
  onNavigate: (page: string) => void;
}

const Dashboard = ({ onNavigate }: DashboardProps) => {
  const user = {
    name: "John Doe",
    email: "john@example.com",
    role: "Student",
    avatar: "/api/placeholder/32/32"
  };

  const navigationCards = [
    {
      title: "Point Shop",
      description: "Besteed je punten aan beloningen",
      icon: <ShoppingBag className="h-6 w-6" />,
      color: "bg-blue-100",
      page: "pointshop"
    },
    {
      title: "Opdrachten",
      description: "Bekijk en lever je opdrachten in",
      icon: <BookOpen className="h-6 w-6" />,
      color: "bg-green-100",
      page: "assignments"
    },
    {
      title: "Groepen",
      description: "Bekijk je projectgroepen",
      icon: <Users className="h-6 w-6" />,
      color: "bg-purple-100",
      page: "groups"
    },
    {
      title: "Instellingen",
      description: "Pas je voorkeuren aan",
      icon: <Settings className="h-6 w-6" />,
      color: "bg-gray-100",
      page: "settings"
    }
  ];

  return (
    <div className="container mx-auto p-4 space-y-6">
      {/* Gebruikersprofiel Sectie */}
      <Card className="bg-gradient-to-r from-blue-500 to-blue-600">
        <CardHeader>
          <div className="flex items-center space-x-4">
            <img
              src={user.avatar}
              alt="Profile"
              className="rounded-full w-16 h-16 border-2 border-white"
            />
            <div className="text-white">
              <h2 className="text-2xl font-bold">{user.name}</h2>
              <p className="text-blue-100">{user.role}</p>
              <p className="text-sm text-blue-100">{user.email}</p>
            </div>
          </div>
        </CardHeader>
      </Card>

      {/* Welkom Bericht */}
      <Card>
        <CardHeader>
          <CardTitle>Welkom terug, {user.name}!</CardTitle>
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
                onClick={() => onNavigate(card.page)}
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