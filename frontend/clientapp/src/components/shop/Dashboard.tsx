
import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { Card, CardContent, CardDescription, CardFooter, CardHeader, CardTitle } from '../ui/card';
import { Button } from '../ui/button';
import { Badge } from '../ui/badge';
import { Alert, AlertDescription, AlertTitle } from '../ui/alert';
import { Loader2 } from 'lucide-react';

interface ShopItem {
  id: string;
  name: string;
  description: string;
  price: number;
}

interface UserPoints {
  pointAmount: number;
  allTimePoints: number;
  items: ShopItem[];
}

interface User {
  id: string;
  firstname: string;
  lastname: string;
  email: string;
  points: {
    allTimePoints: number;
    pointAmount: number;
    items: ShopItem[];
  };
}

const PointShop = () => {
  const navigate = useNavigate();
  const [user, setUser] = useState<User | null>(null);
  const [userPoints, setUserPoints] = useState<UserPoints | null>(null);
  const [availableItems] = useState<ShopItem[]>([
    { id: '1', name: 'Dark Theme', description: 'Enable dark mode for a unique experience', price: 500 },
    { id: '2', name: 'Premium Badge', description: 'Show off your status with a unique badge', price: 1000 },
    { id: '3', name: 'Custom Role', description: 'Get a unique role in the community', price: 2000 },
    { id: '4', name: 'Day off', description: 'Get a day off - one time use!', price: 20000 },
  ]);
  const [loading, setLoading] = useState(true);
  const [purchasing, setPurchasing] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [purchaseStatus, setPurchaseStatus] = useState<{
    show: boolean;
    success: boolean;
    message: string;
  }>({ show: false, success: false, message: '' });

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
      
      const userData = await response.json();
      console.log('User data:', userData);
      
      setUser(userData);
      setUserPoints({
        pointAmount: userData.points.pointAmount,
        allTimePoints: userData.points.allTimePoints,
        items: userData.points.items
      });
    } catch (err) {
      console.error('Error:', err);
      setError('Failed to load user data');
      navigate('/');
    } finally {
      setLoading(false);
    }
  };

  const addPoints = async (amount: number, reason: string) => {
    if (!user) return;
  
    try {
      const token = localStorage.getItem('token');
      if (!token) {
        navigate('/');
        return;
      }
  
      const response = await fetch(`http://localhost:5001/api/points/${user.id}/add`, {
        method: 'POST',
        headers: {
          'Authorization': `Bearer ${token}`,
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({ amount, reason }), // Send amount and reason as an object
      });
  
      if (!response.ok) {
        const errorData = await response.json();
        throw new Error(errorData.title || 'Failed to add points');
      }
  
      console.log('Points added successfully');
      // Refresh user data to reflect the new points
      await fetchUserData();
    } catch (error) {
      console.error('Error adding points:', error);
    }
  };
  
  

  const handlePurchase = async (item: ShopItem) => {
    if (!user || !userPoints) return;

    if (userPoints.items.some(ownedItem => ownedItem.id === item.id || ownedItem.name === item.name)) {
      setPurchaseStatus({
        show: true,
        success: false,
        message: 'You already own this item!'
      });
      return;
    }

    setPurchasing(true);

    try {
      const token = localStorage.getItem('token');
      if (!token) {
        navigate('/');
        return;
      }

      if (userPoints.pointAmount < item.price) {
        throw new Error('Not enough points');
      }

      // First update points
      const newAmount = userPoints.pointAmount - item.price;
      const updatePointsResponse = await fetch(`http://localhost:5001/api/points/${user.id}/update`, {
        method: 'PUT',
        headers: {
          'Authorization': `Bearer ${token}`,
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(newAmount)
      });

      if (!updatePointsResponse.ok) {
        throw new Error('Failed to update points');
      }

      // Then save the purchased item
      const purchaseItemResponse = await fetch(`http://localhost:5001/api/points/${user.id}/purchase`, {
        method: 'POST',
        headers: {
          'Authorization': `Bearer ${token}`,
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({
          id: item.id,
          name: item.name,
          description: item.description,
          price: item.price
        })
      });

      if (!purchaseItemResponse.ok) {
        throw new Error('Failed to save purchase');
      }

      setPurchaseStatus({
        show: true,
        success: true,
        message: `Successfully purchased ${item.name}!`
      });

      // Refresh user data
      await fetchUserData();

    } catch (err) {
      console.error('Error during purchase:', err);
      setPurchaseStatus({
        show: true,
        success: false,
        message: err instanceof Error ? err.message : 'Purchase failed'
      });
      
      // Try to restore points if the purchase failed
      try {
        await fetchUserData();
      } catch (refreshError) {
        console.error('Failed to refresh user data:', refreshError);
      }
    } finally {
      setPurchasing(false);
      setTimeout(() => {
        setPurchaseStatus(prev => ({ ...prev, show: false }));
      }, 3000);
    }
  };

  useEffect(() => {
    fetchUserData();
  }, [navigate]);

  if (loading) {
    return (
      <div className="flex justify-center items-center min-h-screen">
        <Loader2 className="w-8 h-8 animate-spin" />
      </div>
    );
  }

  if (error) {
    return (
      <Alert variant="destructive">
        <AlertTitle>Error</AlertTitle>
        <AlertDescription>{error}</AlertDescription>
      </Alert>
    );
  }

  return (
    <div className="container mx-auto p-4">
      <Card className="mb-6">
        <CardHeader>
          <CardTitle>Your Points</CardTitle>
          <CardDescription>
            {user ? `Welcome, ${user.firstname} ${user.lastname}` : 'Loading...'}
          </CardDescription>
        </CardHeader>
        <CardContent>
          <div className="flex justify-between items-center">
            <p className="text-2xl font-bold">{userPoints?.pointAmount || 0} points</p>
            <p className="text-xl">All-time: {userPoints?.allTimePoints || 0}</p>
          </div>
          <Button 
            onClick={() => addPoints(100, 'cheat')} 
            className="mt-4"
            variant="outline"
          >
            Add 100 points (Test)
          </Button>
        </CardContent>
      </Card>

      {purchaseStatus.show && (
        <Alert 
          variant={purchaseStatus.success ? 'default' : 'destructive'}
          className="mb-4"
        >
          <AlertTitle>{purchaseStatus.success ? 'Success!' : 'Error'}</AlertTitle>
          <AlertDescription>{purchaseStatus.message}</AlertDescription>
        </Alert>
      )}

      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
        {availableItems.map((item) => {
          const isOwned = userPoints?.items?.some(ownedItem => 
            ownedItem.id === item.id || ownedItem.name === item.name
          );
          return (
            <Card 
              key={item.id}
              className={isOwned ? 'opacity-75 bg-gray-50' : ''}
            >
              <CardHeader>
                <div className="flex justify-between items-start">
                  <div>
                    <CardTitle>{item.name}</CardTitle>
                    <CardDescription>{item.description}</CardDescription>
                  </div>
                  {isOwned && (
                    <Badge variant="secondary" className="bg-gray-200">
                      Owned
                    </Badge>
                  )}
                </div>
              </CardHeader>
              <CardContent>
                <Badge 
                  variant={isOwned ? "outline" : "secondary"} 
                  className={`text-lg ${isOwned ? 'border-gray-300 text-gray-500' : ''}`}
                >
                  {item.price} points
                </Badge>
              </CardContent>
              <CardFooter>
                <Button 
                  onClick={() => handlePurchase(item)}
                  disabled={!userPoints || userPoints.pointAmount < item.price || purchasing || isOwned}
                  variant={isOwned ? "outline" : "default"}
                  className={`w-full ${isOwned ? 'bg-gray-100 text-gray-500 cursor-not-allowed' : ''}`}
                >
                  {purchasing ? (
                    <>
                      <Loader2 className="w-4 h-4 mr-2 animate-spin" />
                      Processing...
                    </>
                  ) : isOwned ? (
                    'Already in your inventory'
                  ) : !userPoints || userPoints.pointAmount < item.price ? (
                    'Not enough points' 
                  ) : (
                    'Purchase'
                  )}
                </Button>
              </CardFooter>
            </Card>
          );
        })}
      </div>
    </div>
  );
};

export default PointShop;
