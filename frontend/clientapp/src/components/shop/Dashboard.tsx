import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '../ui/card';
import { Button } from '../ui/button';
import { Badge } from '../ui/badge';
import { Alert, AlertDescription, AlertTitle } from '../ui/alert';
import { 
  ShoppingBag, 
  Settings, 
  Users, 
  BookOpen, 
  LogOut, 
  Loader2, 
  Award, 
  Trophy,
  TrendingUp,
  Calendar
} from 'lucide-react';

interface UserData {
  id: string;
  firstname: string;
  lastname: string;
  email: string;
  role: string;
  points: {
    pointAmount: number;
    allTimePoints: number;
    currentStreak: number;
  };
  badges: Array<{
    id: string;
    name: string;
    description: string;
  }>;
  achievements: Array<{
    id: string;
    name: string;
    description: string;
  }>;
  currentLevel: number;
}

interface PointHistoryItem {
  amount: number;
  description: string;
  timestamp: string;
}

const Dashboard = () => {
  const navigate = useNavigate();
  const [userData, setUserData] = useState<UserData | null>(null);
  const [pointHistory, setPointHistory] = useState<PointHistoryItem[]>([]);
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

        if (!response.ok) throw new Error('Failed to fetch user data');
        const data = await response.json();
        setUserData(data);

        // Fetch point history
        const historyResponse = await fetch(`http://localhost:5001/api/points/${data.id}/history`, {
          headers: {
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json',
          },
        });

        if (historyResponse.ok) {
          const historyData = await historyResponse.json();
          setPointHistory(historyData);
        }

      } catch (error) {
        console.error('Error:', error);
        setError('Failed to load user data');
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
        <Alert variant="destructive">
          <AlertTitle>Error</AlertTitle>
          <AlertDescription>{error || 'Failed to load user data'}</AlertDescription>
        </Alert>
      </div>
    );
  }

  return (
    <div className="container mx-auto p-4 grid grid-cols-1 lg:grid-cols-4 gap-6">
      {/* User Profile Card */}
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
                <p className="text-blue-100">Level {userData.currentLevel}</p>
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

        {/* Points & Streak Card */}
        <Card className="mt-4">
          <CardHeader>
            <CardTitle>Progress</CardTitle>
          </CardHeader>
          <CardContent>
            <div className="space-y-4">
              <div className="flex justify-between items-center">
                <span>Current Points:</span>
                <Badge variant="secondary">{userData.points.pointAmount}</Badge>
              </div>
              <div className="flex justify-between items-center">
                <span>All-time Points:</span>
                <Badge variant="secondary">{userData.points.allTimePoints}</Badge>
              </div>
              <div className="flex justify-between items-center">
                <span>Current Streak:</span>
                <Badge variant="secondary">{userData.points.currentStreak} days</Badge>
              </div>
            </div>
          </CardContent>
        </Card>
      </div>

      {/* Main Dashboard Content */}
      <div className="lg:col-span-3 space-y-6">
        {/* Recent Activity */}
        <Card>
          <CardHeader>
            <CardTitle>Recent Activity</CardTitle>
            <CardDescription>Your latest point transactions</CardDescription>
          </CardHeader>
          <CardContent>
            <div className="space-y-4">
              {pointHistory.slice(0, 5).map((item, index) => (
                <div key={index} className="flex justify-between items-center border-b pb-2">
                  <div>
                    <p className="font-medium">{item.description}</p>
                    <p className="text-sm text-gray-500">{new Date(item.timestamp).toLocaleDateString()}</p>
                  </div>
                  <Badge variant={item.amount >= 0 ? "default" : "destructive"}>
                    {item.amount >= 0 ? `+${item.amount}` : item.amount}
                  </Badge>
                </div>
              ))}
            </div>
          </CardContent>
        </Card>

        {/* Navigation Cards */}
        <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
          {[
            {
              title: "Point Shop",
              description: "Spend your points on rewards",
              icon: <ShoppingBag className="h-6 w-6 text-white" />,
              color: "bg-blue-500 hover:bg-blue-600",
              path: "/point-shop"
            },
            {
              title: "Events",
              description: "View upcoming events",
              icon: <Calendar className="h-6 w-6 text-white" />,
              color: "bg-green-500 hover:bg-green-600",
              path: "/events"
            },
            {
              title: "Achievements",
              description: "View your achievements",
              icon: <Trophy className="h-6 w-6 text-white" />,
              color: "bg-purple-500 hover:bg-purple-600",
              path: "/achievements"
            },
            {
              title: "Settings",
              description: "Manage your account",
              icon: <Settings className="h-6 w-6 text-white" />,
              color: "bg-gray-500 hover:bg-gray-600",
              path: "/settings"
            },
            {
              title: "Calendar",
              description: "View and manage your calendar",
              icon: <Calendar className="h-6 w-6 text-white" />,
              color: "bg-orange-500 hover:bg-orange-600",
              path: "/calendar"
            },
          ].map((card, index) => (
            <Card
              key={index}
              className={`${card.color} cursor-pointer transition-transform hover:scale-105`}
              onClick={() => navigate(card.path)}
            >
              <CardHeader>
                <div className="text-white">{card.icon}</div>
              </CardHeader>
              <CardContent>
                <h3 className="font-bold text-white">{card.title}</h3>
                <p className="text-sm text-white/80">{card.description}</p>
              </CardContent>
            </Card>
          ))}
        </div>

        {/* Achievements Showcase */}
        <Card>
          <CardHeader>
            <CardTitle>Recent Achievements</CardTitle>
          </CardHeader>
          <CardContent>
            <div className="flex flex-wrap gap-2">
              {userData.achievements?.slice(0, 3).map((achievement) => (
                <Badge key={achievement.id} variant="secondary" className="p-2">
                  <Award className="w-4 h-4 mr-1 inline" />
                  {achievement.name}
                </Badge>
              ))}
            </div>
          </CardContent>
        </Card>
      </div>
    </div>
  );
};

export default Dashboard;