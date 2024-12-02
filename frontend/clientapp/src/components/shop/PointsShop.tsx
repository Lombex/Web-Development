import React, { useState } from 'react';
import { Card, CardContent, CardDescription, CardFooter, CardHeader, CardTitle } from '../ui/card';
import { Button } from '../ui/button';
import { Badge } from '../ui/badge';
import { Alert, AlertDescription, AlertTitle } from '../ui/alert';
import { ArrowLeft } from 'lucide-react';

interface ShopItem {
  id: string;
  name: string;
  description: string;
  price: number;
}

interface PointShopProps {
  onNavigate: (page: string) => void;
}

const PointShop = ({ onNavigate }: PointShopProps) => {
  const [points, setPoints] = useState(1000);
  const [items] = useState<ShopItem[]>([
    { id: '1', name: 'Dark Theme', description: 'Enable dark mode', price: 500 },
    { id: '2', name: 'Premium Badge', description: 'Show off your status', price: 1000 },
    { id: '3', name: 'Custom Role', description: 'Get a unique role', price: 2000 },
  ]);
  const [purchaseStatus, setPurchaseStatus] = useState<{
    show: boolean;
    success: boolean;
    message: string;
  }>({ show: false, success: false, message: '' });

  const handlePurchase = (item: ShopItem) => {
    if (points >= item.price) {
      setPoints(prev => prev - item.price);
      setPurchaseStatus({
        show: true,
        success: true,
        message: `Successfully purchased ${item.name}!`
      });
    } else {
      setPurchaseStatus({
        show: true,
        success: false,
        message: 'Not enough points!'
      });
    }

    setTimeout(() => {
      setPurchaseStatus(prev => ({ ...prev, show: false }));
    }, 3000);
  };

  return (
    <div className="container mx-auto p-4">
      {/* Terug knop */}
      <Button 
        variant="outline" 
        className="mb-4"
        onClick={() => onNavigate('dashboard')}
      >
        <ArrowLeft className="mr-2 h-4 w-4" />
        Terug naar Dashboard
      </Button>

      <Card className="mb-6">
        <CardHeader>
          <CardTitle>Your Points</CardTitle>
          <CardDescription>Current Balance</CardDescription>
        </CardHeader>
        <CardContent>
          <p className="text-2xl font-bold">{points} points</p>
        </CardContent>
      </Card>

      {purchaseStatus.show && (
        <Alert className="mb-4" variant={purchaseStatus.success ? 'default' : 'destructive'}>
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
                disabled={points < item.price}
                className="w-full"
              >
                {points >= item.price ? 'Purchase' : 'Not enough points'}
              </Button>
            </CardFooter>
          </Card>
        ))}
      </div>
    </div>
  );
};

export default PointShop;