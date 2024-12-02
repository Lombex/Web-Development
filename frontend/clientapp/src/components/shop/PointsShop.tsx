import React, { useState, useEffect } from 'react';
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
}

interface User {
    id: string;
    firstname: string;
    lastname: string;
    email: string;
    password: string;
    recuringDays: number;
    role: number;
    points: {
      allTimePoints: number;
      pointAmount: number;
      items: any[];
    };
  }


  const PointShop = () => {
    const [user, setUser] = useState<User | null>(null);
    const [userPoints, setUserPoints] = useState<UserPoints | null>(null);
    const [items] = useState<ShopItem[]>([
      { id: '1', name: 'Dark Theme', description: 'Enable dark mode', price: 500 },
      { id: '2', name: 'Premium Badge', description: 'Show off your status', price: 1000 },
      { id: '3', name: 'Custom Role', description: 'Get a unique role', price: 2000 },
    ]);
    const [loading, setLoading] = useState(true);
    const [purchasing, setPurchasing] = useState(false);
    const [error, setError] = useState<string | null>(null);
    const [purchaseStatus, setPurchaseStatus] = useState<{
      show: boolean;
      success: boolean;
      message: string;
    }>({ show: false, success: false, message: '' });
  
    const fetchTestUser = async () => {
      try {
        const response = await fetch('http://localhost:5001/api/user/all');
        console.log('Users response:', response);
        
        if (!response.ok) throw new Error('Failed to fetch users');
        
        const users = await response.json();
        console.log('Users data:', users);
        
        const testUser = users.find((u: User) => u.email === 'john.doe@example.com');
        console.log('Test user:', testUser);
        
        if (testUser) {
          setUser(testUser);
          await fetchUserPoints(testUser.id);
        } else {
          throw new Error('Test user not found');
        }
      } catch (err) {
        console.error('Error:', err);
        setError('Failed to load user data');
      }
    };
  
    const fetchUserPoints = async (userId: string) => {
      try {
        const response = await fetch(`http://localhost:5001/api/points/${userId}`);
        console.log('Points response:', response);
        
        if (!response.ok) throw new Error('Failed to fetch points');
        
        const pointsData = await response.json();
        console.log('Points data:', pointsData);
        
        setUserPoints({
          pointAmount: pointsData,
          allTimePoints: pointsData
        });
      } catch (err) {
        console.error('Error fetching points:', err);
        setError('Failed to load points data');
      } finally {
        setLoading(false);
      }
    };
  
    const addPoints = async (amount: number) => {
      if (!user) return;
  
      try {
        console.log('Adding points:', amount);
        const response = await fetch(`http://localhost:5001/api/points/${user.id}/add`, {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json',
          },
          body: JSON.stringify(amount)
        });
        
        console.log('Add points response:', response);
        
        if (!response.ok) throw new Error('Failed to add points');
        
        await fetchUserPoints(user.id);
      } catch (err) {
        console.error('Error:', err);
        setError('Failed to add points');
      }
    };
  
    const handlePurchase = async (item: ShopItem) => {
      if (!user || !userPoints) return;
  
      try {
        setPurchasing(true);
        if (userPoints.pointAmount < item.price) {
          throw new Error('Not enough points');
        }
  
        const newAmount = userPoints.pointAmount - item.price;
        console.log('Updating points to:', newAmount);
        
        const response = await fetch(`http://localhost:5001/api/points/${user.id}/update`, {
          method: 'PUT',
          headers: {
            'Content-Type': 'application/json',
          },
          body: JSON.stringify(newAmount)
        });
  
        console.log('Purchase response:', response);
        
        if (!response.ok) throw new Error('Purchase failed');
  
        setPurchaseStatus({
          show: true,
          success: true,
          message: `Successfully purchased ${item.name}!`
        });
  
        await fetchUserPoints(user.id);
  
      } catch (err) {
        console.error('Error:', err);
        setPurchaseStatus({
          show: true,
          success: false,
          message: err instanceof Error ? err.message : 'Purchase failed'
        });
      } finally {
        setPurchasing(false);
        setTimeout(() => {
          setPurchaseStatus(prev => ({ ...prev, show: false }));
        }, 3000);
      }
    };
  
    useEffect(() => {
      fetchTestUser();
    }, []);
  
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
              onClick={() => addPoints(100)} 
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
          {items.map((item) => (
            <Card key={item.id}>
              <CardHeader>
                <CardTitle>{item.name}</CardTitle>
                <CardDescription>{item.description}</CardDescription>
              </CardHeader>
              <CardContent>
                <Badge variant="secondary" className="text-lg">
                  {item.price} points
                </Badge>
              </CardContent>
              <CardFooter>
                <Button 
                  onClick={() => handlePurchase(item)}
                  disabled={!userPoints || userPoints.pointAmount < item.price || purchasing}
                  className="w-full"
                >
                  {purchasing ? (
                    <Loader2 className="w-4 h-4 mr-2 animate-spin" />
                  ) : null}
                  {!userPoints || userPoints.pointAmount < item.price 
                    ? 'Not enough points' 
                    : 'Purchase'}
                </Button>
              </CardFooter>
            </Card>
          ))}
        </div>
      </div>
    );
  };
  
  export default PointShop;