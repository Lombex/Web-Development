import React, { useEffect, useState } from "react";
import {
  Loader2,
  CheckCircle,
  XCircle,
  ArrowUpDown,
  ChevronDown,
  MoreHorizontal,
  Trash2,
  Clipboard,
  Plus,
  Users,
} from "lucide-react";
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from "../../components/ui/dialog";
import { Button } from "../../components/ui/button";
import { Input } from "../../components/ui/input";
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from "../../components/ui/table";
import { Checkbox } from "../../components/ui/checkbox";
import { DropdownMenu, DropdownMenuTrigger, DropdownMenuContent, DropdownMenuItem } from "../../components/ui/dropdown-menu";

interface Event {
  id: string;
  title: string;
  description: string;
  startTime: string;
  endTime: string;
  location: string;
  approval: boolean;
}

interface User {
  id: string;
  role: string; // To check if user is admin
}

const EventsDashboard: React.FC = () => {
  const [events, setEvents] = useState<Event[]>([]);
  const [user, setUser] = useState<User | null>(null);
  const [loading, setLoading] = useState(true);
  const [isAdmin, setIsAdmin] = useState(false); // Check if the user is an admin
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchUserData = async () => {
      const token = localStorage.getItem("token");
      if (!token) {
        alert("User is not logged in");
        return;
      }

      try {
        // Fetch user data
        const userResponse = await fetch("http://localhost:5001/api/user/fromToken", {
          headers: {
            Authorization: `Bearer ${token}`,
            "Content-Type": "application/json",
          },
        });

        if (!userResponse.ok) throw new Error("Failed to fetch user data");

        const userData: User = await userResponse.json();
        setUser(userData);
        setIsAdmin(userData.role === "Admin"); // Check if user is admin
      } catch (err) {
        setError("Failed to load user data");
      }
    };

    const fetchEvents = async () => {
      try {
        const response = await fetch("http://localhost:5001/api/events/all");
        if (!response.ok) throw new Error("Failed to fetch events.");
        const data: Event[] = await response.json();
        setEvents(data);
      } catch (err) {
        setError("Something went wrong.");
      } finally {
        setLoading(false);
      }
    };

    fetchUserData();
    fetchEvents();
  }, []);

  const handleJoinEvent = (eventId: string) => {
    alert(`You joined event with ID: ${eventId}`);
    // Implement join logic here
  };

  const handleCopyEventId = (eventId: string) => {
    navigator.clipboard.writeText(eventId);
    alert(`Copied Event ID: ${eventId}`);
  };

  const handleDeleteEvent = async (eventId: string) => {
    if (!isAdmin) {
      alert("Only admins can delete events.");
      return;
    }

    try {
      const response = await fetch(`http://localhost:5001/api/events/delete/${eventId}`, {
        method: "DELETE",
        headers: { "Content-Type": "application/json" },
      });

      if (!response.ok) {
        throw new Error("Failed to delete event.");
      }

      alert("Event deleted successfully!");
      setEvents((prevEvents) => prevEvents.filter((event) => event.id !== eventId));
    } catch (error) {
      alert(`Error: ${error}`);
    }
  };

  if (loading) {
    return (
      <div className="flex items-center justify-center min-h-screen">
        <Loader2 className="w-8 h-8 animate-spin" />
      </div>
    );
  }

  if (error) {
    return (
      <div className="flex items-center justify-center min-h-screen">
        <p className="text-red-500">{error}</p>
      </div>
    );
  }

  return (
    <div className="w-full p-4">
      <div className="flex gap-2 items-center py-4">
        <Input placeholder="Filter by title..." className="max-w-sm" />

        {/* Only show "Create Event" button if user is admin */}
        {isAdmin && (
          <Dialog>
            <DialogTrigger asChild>
              <Button>Create Event</Button>
            </DialogTrigger>
            <DialogContent className="sm:max-w-[425px]">
              <DialogHeader>
                <DialogTitle>Create event</DialogTitle>
                <DialogDescription>
                  Make changes to your event here. Click save when you're done.
                </DialogDescription>
              </DialogHeader>
              {/* Add form logic here */}
              <DialogFooter>
                <Button type="submit">Save changes</Button>
              </DialogFooter>
            </DialogContent>
          </Dialog>
        )}
      </div>
      <div className="rounded-md border">
        <Table>
          <TableHeader>
            <TableRow>
              <TableHead>Title</TableHead>
              <TableHead>Description</TableHead>
              <TableHead>Start Time</TableHead>
              <TableHead>End Time</TableHead>
              <TableHead>Location</TableHead>
              <TableHead>Approval</TableHead>
              <TableHead>Actions</TableHead>
            </TableRow>
          </TableHeader>
          <TableBody>
            {events.length ? (
              events.map((event) => (
                <TableRow key={event.id}>
                  <TableCell>
                    <DropdownMenu>
                      <DropdownMenuTrigger asChild>
                        <Button variant="ghost">{event.title}</Button>
                      </DropdownMenuTrigger>
                      <DropdownMenuContent>
                        <DropdownMenuItem>View Details</DropdownMenuItem>
                        <DropdownMenuItem>Share Event</DropdownMenuItem>
                      </DropdownMenuContent>
                    </DropdownMenu>
                  </TableCell>
                  <TableCell>{event.description}</TableCell>
                  <TableCell>{new Date(event.startTime).toLocaleString()}</TableCell>
                  <TableCell>{new Date(event.endTime).toLocaleString()}</TableCell>
                  <TableCell>{event.location}</TableCell>
                  <TableCell>
                    {event.approval ? <CheckCircle className="text-lime-400" /> : <XCircle className="text-red-400" />}
                  </TableCell>
                  <TableCell>
                    <DropdownMenu>
                      <DropdownMenuTrigger asChild>
                        <Button variant="ghost">
                          <MoreHorizontal />
                        </Button>
                      </DropdownMenuTrigger>
                      <DropdownMenuContent>
                        <DropdownMenuItem onClick={() => handleJoinEvent(event.id)}>
                          <Users className="mr-2" />
                          Join Event
                        </DropdownMenuItem>
                        <DropdownMenuItem onClick={() => handleCopyEventId(event.id)}>
                          <Clipboard className="mr-2" />
                          Copy Event ID
                        </DropdownMenuItem>
                        {isAdmin && (
                          <DropdownMenuItem onClick={() => handleDeleteEvent(event.id)}>
                            <Trash2 className="mr-2 text-red-400" />
                            Delete Event
                          </DropdownMenuItem>
                        )}
                      </DropdownMenuContent>
                    </DropdownMenu>
                  </TableCell>
                </TableRow>
              ))
            ) : (
              <TableRow>
                <TableCell colSpan={7} className="text-center">
                  No events found.
                </TableCell>
              </TableRow>
            )}
          </TableBody>
        </Table>
      </div>
    </div>
  );
};

export default EventsDashboard;
