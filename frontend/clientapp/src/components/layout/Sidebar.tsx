
import React from 'react';
import { useNavigate, useLocation } from 'react-router-dom';
import { 
  LayoutDashboard,
  ShoppingBag,
  Calendar,
  Trophy,
  Settings,
  LogOut,
  Menu,
  X
} from 'lucide-react';
import { Button } from '../ui/button';

interface SidebarProps {
  isMobile: boolean;
  isOpen: boolean;
  onToggle: () => void;
}

const Sidebar: React.FC<SidebarProps> = ({ isMobile, isOpen, onToggle }) => {
  const navigate = useNavigate();
  const location = useLocation();

  const menuItems = [
    { path: '/dashboard', name: 'Dashboard', icon: LayoutDashboard },
    { path: '/calendar', name: 'Calendar', icon: Calendar }, // Add this line
    { path: '/point-shop', name: 'Point Shop', icon: ShoppingBag },
    { path: '/events', name: 'Events', icon: Calendar },
    { path: '/achievements', name: 'Achievements', icon: Trophy },
    { path: '/settings', name: 'Settings', icon: Settings },
  ];
  const handleLogout = () => {
    localStorage.removeItem('token');
    navigate('/');
  };

  const sidebarContent = (
    <>
      <div className="p-4">
        <h1 className="text-xl font-bold text-white mb-4">Calenderfy</h1>
      </div>
      <nav className="flex-1">
        {menuItems.map((item) => {
          const Icon = item.icon;
          const isActive = location.pathname === item.path;
          return (
            <button
              key={item.path}
              onClick={() => {
                navigate(item.path);
                if (isMobile) onToggle();
              }}
              className={`w-full flex items-center space-x-2 px-6 py-3 text-left
                ${isActive 
                  ? 'bg-blue-700 text-white' 
                  : 'text-blue-100 hover:bg-blue-700 hover:text-white'
                } transition-colors duration-200`}
            >
              <Icon className="h-5 w-5" />
              <span>{item.name}</span>
            </button>
          );
        })}
      </nav>
      <div className="p-4">
        <Button
          variant="secondary"
          className="w-full flex items-center justify-center space-x-2"
          onClick={handleLogout}
        >
          <LogOut className="h-4 w-4" />
          <span>Logout</span>
        </Button>
      </div>
    </>
  );

  if (isMobile) {
    return (
      <>
        <button
          onClick={onToggle}
          className="fixed top-4 left-4 z-50 p-2 bg-blue-600 rounded-md text-white"
        >
          {isOpen ? <X /> : <Menu />}
        </button>

        {isOpen && (
          <div className="fixed inset-0 z-40 bg-black bg-opacity-50" onClick={onToggle} />
        )}

        <div
          className={`fixed inset-y-0 left-0 w-64 bg-blue-600 transform transition-transform duration-300 ease-in-out z-50
            ${isOpen ? 'translate-x-0' : '-translate-x-full'}`}
        >
          {sidebarContent}
        </div>
      </>
    );
  }

  return (
    <div className="w-64 bg-blue-600 h-screen flex flex-col fixed left-0 top-0">
      {sidebarContent}
    </div>
  );
};

export default Sidebar;