import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '../ui/card';
import { Badge } from '../ui/badge';
import { Alert, AlertDescription, AlertTitle } from '../ui/alert';
import { Loader2, Award, Trophy, Star, Lock } from 'lucide-react';

interface Achievement {
  id: string;
  name: string;
  description: string;
  pointsRequired: number;
}

interface UserBadge {
  id: string;
  name: string;
  description: string;
  imageUrl: string;
  requiredPoints: number;
}

interface UserData {
  id: string;
  points: {
    allTimePoints: number;
  };
  achievements: Achievement[];
  badges: UserBadge[];
}

const Achievements = () => {
  const navigate = useNavigate();
  const [userData, setUserData] = useState<UserData | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [allAchievements, setAllAchievements] = useState<Achievement[]>([]);
  const [allBadges, setAllBadges] = useState<UserBadge[]>([]);

  useEffect(() => {
    const fetchData = async () => {
      const token = localStorage.getItem('token');
      if (!token) {
        navigate('/');
        return;
      }

      try {
        // Fetch user data
        const userResponse = await fetch('http://localhost:5001/api/user/fromToken', {
          headers: {
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json',
          },
        });

        if (!userResponse.ok) throw new Error('Failed to fetch user data');
        const userData = await userResponse.json();
        setUserData(userData);

        // Fetch all achievements and badges
        const [achievementsResponse, badgesResponse] = await Promise.all([
          fetch(`http://localhost:5001/api/achievements/user/${userData.id}`, {
            headers: { 'Authorization': `Bearer ${token}` },
          }),
          fetch(`http://localhost:5001/api/achievements/badges/user/${userData.id}`, {
            headers: { 'Authorization': `Bearer ${token}` },
          }),
        ]);

        if (achievementsResponse.ok) {
          const achievementsData = await achievementsResponse.json();
          setAllAchievements(achievementsData);
        }

        if (badgesResponse.ok) {
          const badgesData = await badgesResponse.json();
          setAllBadges(badgesData);
        }

      } catch (error) {
        console.error('Error:', error);
        setError('Failed to load achievements data');
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, [navigate]);

  if (loading) {
    return (
      <div className="flex items-center justify-center min-h-screen">
        <Loader2 className="w-8 h-8 animate-spin" />
      </div>
    );
  }

  if (error || !userData) {
    return (
      <Alert variant="destructive">
        <AlertTitle>Error</AlertTitle>
        <AlertDescription>{error}</AlertDescription>
      </Alert>
    );
  }

  const calculateProgress = (required: number) => {
    return Math.min(100, Math.round((userData.points.allTimePoints / required) * 100));
  };

  return (
    <div className="container mx-auto p-4 space-y-6">
      {/* Header */}
      <div className="flex justify-between items-center">
        <div>
          <h1 className="text-2xl font-bold">Achievements & Badges</h1>
          <p className="text-gray-500">Track your progress and unlock rewards</p>
        </div>
        <Badge variant="secondary" className="text-lg">
          {userData.points.allTimePoints} Points
        </Badge>
      </div>

      {/* Achievements Section */}
      <Card>
        <CardHeader>
          <CardTitle className="flex items-center gap-2">
            <Trophy className="h-6 w-6 text-yellow-500" />
            Achievements
          </CardTitle>
          <CardDescription>Complete tasks to earn achievements</CardDescription>
        </CardHeader>
        <CardContent>
          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
            {allAchievements.map((achievement) => {
              const isUnlocked = userData.achievements.some(a => a.id === achievement.id);
              const progress = calculateProgress(achievement.pointsRequired);

              return (
                <Card key={achievement.id} className={`relative ${isUnlocked ? 'bg-green-50' : 'bg-gray-50'}`}>
                  <CardHeader>
                    <div className="flex justify-between items-start">
                      <CardTitle className="text-lg">{achievement.name}</CardTitle>
                      {isUnlocked ? (
                        <Award className="h-6 w-6 text-green-500" />
                      ) : (
                        <Lock className="h-6 w-6 text-gray-400" />
                      )}
                    </div>
                    <CardDescription>{achievement.description}</CardDescription>
                  </CardHeader>
                  <CardContent>
                    <div className="space-y-2">
                      <div className="flex justify-between text-sm">
                        <span>Progress</span>
                        <span>{progress}%</span>
                      </div>
                      <div className="w-full bg-gray-200 rounded-full h-2">
                        <div
                          className={`h-2 rounded-full ${
                            isUnlocked ? 'bg-green-500' : 'bg-blue-500'
                          }`}
                          style={{ width: `${progress}%` }}
                        />
                      </div>
                      <div className="text-sm text-gray-500">
                        {achievement.pointsRequired} points required
                      </div>
                    </div>
                  </CardContent>
                </Card>
              );
            })}
          </div>
        </CardContent>
      </Card>

      {/* Badges Section */}
      <Card>
        <CardHeader>
          <CardTitle className="flex items-center gap-2">
            <Star className="h-6 w-6 text-purple-500" />
            Badges
          </CardTitle>
          <CardDescription>Collect badges by reaching milestones</CardDescription>
        </CardHeader>
        <CardContent>
          <div className="grid grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-4">
            {allBadges.map((badge) => {
              const isUnlocked = userData.badges.some(b => b.id === badge.id);
              const progress = calculateProgress(badge.requiredPoints);

              return (
                <Card 
                  key={badge.id} 
                  className={`text-center ${isUnlocked ? 'bg-purple-50' : 'bg-gray-50'}`}
                >
                  <CardHeader>
                    <div className="flex justify-center">
                      {isUnlocked ? (
                        <img
                          src={badge.imageUrl || "/api/placeholder/64/64"}
                          alt={badge.name}
                          className="w-16 h-16 rounded-full"
                        />
                      ) : (
                        <div className="w-16 h-16 rounded-full bg-gray-200 flex items-center justify-center">
                          <Lock className="h-8 w-8 text-gray-400" />
                        </div>
                      )}
                    </div>
                    <CardTitle className="text-base">{badge.name}</CardTitle>
                    <CardDescription className="text-sm">{badge.description}</CardDescription>
                  </CardHeader>
                  <CardContent>
                    <div className="w-full bg-gray-200 rounded-full h-2 mb-2">
                      <div
                        className={`h-2 rounded-full ${
                          isUnlocked ? 'bg-purple-500' : 'bg-blue-500'
                        }`}
                        style={{ width: `${progress}%` }}
                      />
                    </div>
                    <span className="text-sm text-gray-500">
                      {isUnlocked ? 'Unlocked!' : `${badge.requiredPoints} points needed`}
                    </span>
                  </CardContent>
                </Card>
              );
            })}
          </div>
        </CardContent>
      </Card>
    </div>
  );
};

export default Achievements;