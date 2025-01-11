import React, { useState, useEffect } from 'react';
import { ChevronLeft, ChevronRight } from 'lucide-react';
import { Dialog, DialogContent, DialogHeader, DialogTitle } from '../../components/ui/dialog';
import { Button } from '../../components/ui/button';
import { useNavigate } from 'react-router-dom';

interface CalendarEvent {
  id: string;
  title: string;
  description: string;
  startTime: string;
  endTime: string;
  location: string;
  approval: boolean;
  attendees?: number;
  isAttending?: boolean;
  rating?: number;
}

interface UserData {
  id: string;
  firstname: string;
  lastname: string;
  email: string;
  role: string;
}

interface Attendance {
  id: string;
  userId: string;
  date: string;
}

type ViewType = 'month' | 'week';

type SelectedDay = {
  date: Date;
  events: CalendarEvent[];
} | null;

const Calendar = () => {
  const [userData, setUserData] = useState<UserData | null>(null);
  const [attendances, setAttendances] = useState<Attendance[]>([]);
  const [isAdmin, setIsAdmin] = useState(false);
  const [currentDate, setCurrentDate] = useState(new Date());
  const [viewType, setViewType] = useState<ViewType>('month');
  const [selectedDay, setSelectedDay] = useState<SelectedDay>(null);
  const [events, setEvents] = useState<CalendarEvent[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const navigate = useNavigate();
  
  const timeSlots = Array.from({ length: 24 }, (_, i) => i);
  const monthNames = [
    "January", "February", "March", "April", "May", "June",
    "July", "August", "September", "October", "November", "December"
  ];
  const days = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"];

  useEffect(() => {
    const token = localStorage.getItem('token');
    if (!token) {
      navigate('/');
      return;
    }

    // Fetch user data
    const fetchUserData = async () => {
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
        setIsAdmin(data.role === 'Admin');
      } catch (err) {
        console.error('Error fetching user data:', err);
        navigate('/');
      }
    };

    // Fetch attendance data
    const fetchAttendances = async () => {
      try {
        const response = await fetch('http://localhost:5001/api/attendance/user/me', {
          headers: {
            'Authorization': `Bearer ${token}`,
          },
        });

        if (!response.ok) throw new Error('Failed to fetch attendances');
        const data = await response.json();
        setAttendances(data);
      } catch (err) {
        console.error('Error fetching attendances:', err);
      }
    };

    fetchUserData();
    fetchEvents();
    fetchAttendances();
  }, [navigate]);

  const fetchEvents = async () => {
    try {
      const token = localStorage.getItem('token');
      if (!token) {
        navigate('/');
        return;
      }

      const response = await fetch('http://localhost:5001/api/events/all', {
        headers: {
          'Authorization': `Bearer ${token}`,
          'Content-Type': 'application/json',
        },
      });

      if (!response.ok) throw new Error('Failed to fetch events');
      const data = await response.json();
      setEvents(data);
    } catch (err) {
      setError('Failed to load events');
      console.error('Error fetching events:', err);
    } finally {
      setLoading(false);
    }
  };

  const getWeekDates = (date: Date) => {
    const week = [];
    const start = new Date(date);
    start.setDate(date.getDate() - date.getDay());
    
    for (let i = 0; i < 7; i++) {
      const newDate = new Date(start);
      newDate.setDate(start.getDate() + i);
      week.push(newDate);
    }
    return week;
  };

  const getMonthDates = () => {
    const daysInMonth = new Date(
      currentDate.getFullYear(),
      currentDate.getMonth() + 1,
      0
    ).getDate();
    
    const firstDay = new Date(
      currentDate.getFullYear(),
      currentDate.getMonth(),
      1
    ).getDay();
    
    const blanks = Array(firstDay).fill(null);
    return [...blanks, ...Array.from({ length: daysInMonth }, (_, i) => i + 1)];
  };

  const navigateCalendar = (direction: 'prev' | 'next') => {
    const newDate = new Date(currentDate);
    if (viewType === 'month') {
      newDate.setMonth(currentDate.getMonth() + (direction === 'next' ? 1 : -1));
    } else {
      newDate.setDate(currentDate.getDate() + (direction === 'next' ? 7 : -7));
    }
    setCurrentDate(newDate);
  };

  const isToday = (date: Date | number) => {
    const today = new Date();
    if (typeof date === 'number') {
      return date === today.getDate() &&
        currentDate.getMonth() === today.getMonth() &&
        currentDate.getFullYear() === today.getFullYear();
    }
    return date.getDate() === today.getDate() &&
           date.getMonth() === today.getMonth() &&
           date.getFullYear() === today.getFullYear();
  };

  const formatTime = (time: string) => {
    return new Date(time).toLocaleTimeString([], { 
      hour: '2-digit', 
      minute: '2-digit'
    });
  };

  const formatDate = (date: Date) => {
    return `${days[date.getDay()]}, ${monthNames[date.getMonth()]} ${date.getDate()}, ${date.getFullYear()}`;
  };

  const getEventsForDay = (date: Date | number) => {
    return events.filter(event => {
      const eventDate = new Date(event.startTime);
      if (typeof date === 'number') {
        return eventDate.getDate() === date &&
               eventDate.getMonth() === currentDate.getMonth() &&
               eventDate.getFullYear() === currentDate.getFullYear();
      }
      return eventDate.getDate() === date.getDate() &&
             eventDate.getMonth() === date.getMonth() &&
             eventDate.getFullYear() === date.getFullYear();
    });
  };

  const calculateEventPosition = (event: CalendarEvent) => {
    const start = new Date(event.startTime);
    const end = new Date(event.endTime);
    const startHour = start.getHours() + (start.getMinutes() / 60);
    const endHour = end.getHours() + (end.getMinutes() / 60);
    const duration = endHour - startHour;
    
    return {
      top: `${startHour * 4}rem`,
      height: `${duration * 4}rem`
    };
  };

  const getEventColor = (event: CalendarEvent) => {
    return event.approval ? 'bg-green-200' : 'bg-yellow-200';
  };

  const handleDayClick = (date: number | Date) => {
    const selectedDate = typeof date === 'number' 
      ? new Date(currentDate.getFullYear(), currentDate.getMonth(), date)
      : date;
    
    const dayEvents = getEventsForDay(date);
    setSelectedDay({ date: selectedDate, events: dayEvents });
  };

  // Component rendering code continues...
  return (
    <div className="min-h-screen bg-gray-50 p-4">
      <div className="max-w-8xl mx-auto bg-white rounded-lg shadow-xl">
        {/* Calendar Header */}
        <div className="flex items-center justify-between p-4 border-b">
          <div className="flex items-center space-x-4">
            <h1 className="text-2xl font-bold">Calendar</h1>
            <div className="flex items-center space-x-2">
              <Button
                variant="ghost" 
                onClick={() => navigate('prev')}
                className="p-2"
              >
                <ChevronLeft className="w-5 h-5" />
              </Button>
              <h2 className="text-xl font-semibold">
                {monthNames[currentDate.getMonth()]} {currentDate.getFullYear()}
              </h2>
              <Button
                variant="ghost"
                onClick={() => navigate('next')}
                className="p-2"
              >
                <ChevronRight className="w-5 h-5" />
              </Button>
            </div>
          </div>
          
          <div className="flex items-center space-x-2">
            <Button
              onClick={() => setViewType('month')}
              variant={viewType === 'month' ? 'default' : 'outline'}
            >
              Month
            </Button>
            <Button
              onClick={() => setViewType('week')}
              variant={viewType === 'week' ? 'default' : 'outline'}
            >
              Week
            </Button>
          </div>
        </div>

        {loading ? (
          <div className="flex justify-center items-center h-96">
            <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-500" />
          </div>
        ) : error ? (
          <div className="flex justify-center items-center h-96 text-red-500">
            {error}
          </div>
        ) : (
          <>
            {/* Calendar Grid */}
            <div className="grid grid-cols-7 border-b">
              {days.map(day => (
                <div key={day} className="p-2 text-center border-r font-medium">
                  {viewType === 'week' ? day : day.slice(0, 3)}
                </div>
              ))}
            </div>

            <div className="grid grid-cols-7 divide-x">
              {viewType === 'month' ? (
                getMonthDates().map((date, index) => (
                  <div key={index} className="border-b">
                    {date === null ? (
                      <div className="bg-gray-50 p-2 min-h-[8rem]" />
                    ) : (
                      <div 
                        className={`p-2 min-h-[8rem] cursor-pointer hover:bg-gray-50 
                          ${isToday(date) ? 'bg-blue-50 hover:bg-blue-100' : ''}`}
                        onClick={() => handleDayClick(date)}
                      >
                        <span className={`inline-flex items-center justify-center w-6 h-6 rounded-full text-sm
                          ${isToday(date) ? 'bg-blue-500 text-white' : ''}`}>
                          {date}
                        </span>
                        <div className="mt-1 space-y-1">
                          {getEventsForDay(date).map(event => (
                            <div
                              key={event.id}
                              className={`${getEventColor(event)} p-1 rounded text-xs`}
                            >
                              <div className="font-medium truncate">{event.title}</div>
                              <div className="text-gray-600">
                                {formatTime(event.startTime)}
                              </div>
                            </div>
                          ))}
                        </div>
                      </div>
                    )}
                  </div>
                ))
              ) : (
                getWeekDates(currentDate).map((date, index) => (
                  <div key={index} className="border-b">
                    <div className="sticky top-0 p-2 bg-white border-b">
                      <span className={`inline-flex items-center justify-center w-8 h-8 rounded-full
                        ${isToday(date) ? 'bg-blue-500 text-white' : ''}`}>
                        {date.getDate()}
                      </span>
                    </div>
                    <div className="relative min-h-[40rem]">
                      {timeSlots.map(hour => (
                        <div key={hour} className="border-t h-16 relative">
                          <span className="absolute -top-3 left-1 text-xs text-gray-500">
                            {hour % 12 || 12}{hour < 12 ? 'am' : 'pm'}
                          </span>
                        </div>
                      ))}
                      {getEventsForDay(date).map(event => (
                        <div
                          key={event.id}
                          style={calculateEventPosition(event)}
                          className={`absolute left-6 right-1 p-1 rounded ${getEventColor(event)} 
                            text-sm overflow-hidden`}
                        >
                          <div className="font-semibold">{event.title}</div>
                          <div className="text-xs">
                            {formatTime(event.startTime)} - {formatTime(event.endTime)}
                          </div>
                        </div>
                      ))}
                    </div>
                  </div>
                ))
              )}
            </div>
          </>
        )}
      </div>

      {/* Event Details Dialog */}
      <Dialog open={selectedDay !== null} onOpenChange={() => setSelectedDay(null)}>
        <DialogContent className="max-w-2xl">
          <DialogHeader>
            <DialogTitle>
              {selectedDay && formatDate(selectedDay.date)}
            </DialogTitle>
          </DialogHeader>
          <div className="space-y-4">
            {selectedDay?.events.length === 0 ? (
              <p className="text-gray-500">No events scheduled for this day.</p>
            ) : (
              selectedDay?.events.map(event => (
                <div 
                  key={event.id}
                  className={`${getEventColor(event)} p-3 rounded-lg`}
                >
                  <div className="font-semibold text-lg">{event.title}</div>
                  <div className="text-sm text-gray-700">
                    {formatTime(event.startTime)} - {formatTime(event.endTime)}
                  </div>
                  <div className="text-sm mt-2">{event.description}</div>
                  <div className="text-sm text-gray-600 mt-1">
                    Location: {event.location}
                  </div>
                  <div className="text-sm mt-1">
                    Status: {event.approval ? 'Approved' : 'Pending Approval'}
                  </div>
                </div>
              ))
            )}
          </div>
        </DialogContent>
      </Dialog>
    </div>
  );
};

export default Calendar;