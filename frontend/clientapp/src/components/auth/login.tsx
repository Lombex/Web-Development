import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Button } from '../ui/button';
import { Loader2 } from 'lucide-react';

const Login: React.FC = () => {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState<string | null>(null);
  const [loading, setLoading] = useState(false);

  const navigate = useNavigate();

  const handleLoginSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    console.log('Login attempt with:', { email }); // Debug log

    setLoading(true);
    setError(null);

    try {
      // Eerst controleren of de server bereikbaar is
      const testResponse = await fetch('http://localhost:5001/api/user/Test');
      console.log('Test response:', await testResponse.text());

      // Login request
      const response = await fetch('http://localhost:5001/api/auth/login', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({
          Email: email,
          Password: password,
        }),
      });

      console.log('Response status:', response.status); // Debug log
      
      const data = await response.json();
      console.log('Response data:', data); // Debug log

      if (!response.ok) {
        throw new Error(data.message || 'Login failed');
      }

      // Sla token op en navigeer
      localStorage.setItem('token', data.token);
      navigate('/dashboard');

    } catch (err) {
      console.error('Full error:', err); // Debug log
      setError(err instanceof Error ? err.message : 'Login failed');
      localStorage.removeItem('token');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="flex items-center justify-center min-h-screen bg-gray-200">
      <div className="bg-white p-6 rounded-lg shadow-lg w-96">
        <form onSubmit={handleLoginSubmit} className="space-y-4">
          <div>
            <label htmlFor="email" className="block text-sm font-medium text-gray-700">
              Email:
            </label>
            <input
              type="email"
              id="email"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              required
              className="mt-1 p-2 w-full border border-gray-300 rounded-md"
            />
          </div>
          <div>
            <label htmlFor="password" className="block text-sm font-medium text-gray-700">
              Password:
            </label>
            <input
              type="password"
              id="password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              required
              className="mt-1 p-2 w-full border border-gray-300 rounded-md"
            />
          </div>
          <Button 
            type="submit"
            disabled={loading}
            variant="outline"
            className="w-full mt-4"
          >
            {loading && <Loader2 className="w-4 h-4 mr-2 animate-spin" />}
            {loading ? 'Logging in...' : 'Login'}
          </Button>

          {error && (
            <p className="text-red-500 mt-2">
              Error: {error}
            </p>
          )}
        </form>
      </div>
    </div>
  );
};

export default Login;