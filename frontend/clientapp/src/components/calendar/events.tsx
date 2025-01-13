import React, { useEffect, useState } from "react";
import { Loader2, CheckCircle, XCircle, ArrowUpDown, ChevronDown, MoreHorizontal, Pencil, Trash2 } from "lucide-react";
import { Dialog, DialogContent, DialogDescription, DialogFooter, DialogHeader, DialogTitle, DialogTrigger } from "../../components/ui/dialog"
import { ColumnDef, ColumnFiltersState, SortingState,
  VisibilityState, flexRender, getCoreRowModel,
  getFilteredRowModel, getPaginationRowModel, getSortedRowModel,
  useReactTable 
} from "@tanstack/react-table"
 
import { Label } from "../../components/ui/label"

import { Button } from "../../components/ui/button"
import { Input } from "../../components/ui/input"

import { DropdownMenu, DropdownMenuCheckboxItem,
  DropdownMenuContent, DropdownMenuItem, DropdownMenuLabel,
  DropdownMenuSeparator, DropdownMenuTrigger
} from "../../components/ui/dropdown-menu"

import { Table, TableBody, TableCell,
  TableHead, TableHeader, TableRow
} from "../../components/ui/table"

import { Textarea } from "../../components/ui/textarea"

import { Checkbox } from "../../components/ui/checkbox"

interface Event {
  id: string;
  title: string;
  description: string;
  startTime: string;
  endTime: string;
  location: string;
  approval: boolean;
}

const EventsDashboard: React.FC = () => {

  const [events, setEvents] = useState<Event[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [sorting, setSorting] = React.useState<SortingState>([])
  const [columnFilters, setColumnFilters] = React.useState<ColumnFiltersState>([])
  const [columnVisibility, setColumnVisibility] = React.useState<VisibilityState>({})
  const [rowSelection, setRowSelection] = React.useState({})

  const [id, setId] = useState("");
  const [title, setTitle] = useState("");
  const [description, setDescription] = useState("");
  const [startTime, setStartTime] = useState("");
  const [endTime, setEndTime] = useState("");
  const [location, setLocation] = useState("");
  const [approval, setApproval] = useState(false);

  const handleUpdateEvent = async (e: React.FormEvent) => {
    e.preventDefault();

    const updatedEvent = {
      Id: id,
      Title: title,
      Description: description,
      StartTime: startTime,
      EndTime: endTime,
      Location: location,
      Approval: approval,
    };

    try {
      const response = await fetch("http://localhost:5001/api/events/update", {
        method: "PUT",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(updatedEvent),
      });

      if (response.ok) {
        const updatedEventData = await response.json();
        alert("Event updated successfully!");
        setEvents((prevEvents) =>
          prevEvents.map((evt) => (evt.id === id ? updatedEventData : evt))
        );
      } else {
        throw new Error("Failed to update event.");
      }
    } catch (error) {
      alert(error);
    }
  };

  const handleCreateEvent = async (e: React.FormEvent) => { 
    e.preventDefault();

    const newEvent = {
      Title: title,
      Description: description,
      StartTime: startTime,
      EndTime: endTime,
      Location: location,
      Approval: approval,
    };

    try {
      const response = await fetch("http://localhost:5001/api/events/create", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(newEvent),
      });

      if (!response.ok) {
        throw new Error(`Failed to update event: ${response.statusText}`);
      }
  
      const contentType = response.headers.get("content-type");
      if (contentType && contentType.includes("application/json")) {
        const data = await response.json();
        alert("Event updated successfully!");
        console.log(data); // Debugging: Check the returned data
      } else {
        alert("Event updated successfully!");
      }
    } catch (error) {
      alert(error);
    }
  };

  const handleDeleteEvent = async (eventId: string) => {
    try {
      const response = await fetch(`http://localhost:5001/api/events/delete/${eventId}`, {
        method: "DELETE",
        headers: { "Content-Type": "application/json" },
      });
  
      if (!response.ok) {
        throw new Error(`Failed to delete event: ${response.statusText}`);
      }
  
      alert("Event deleted successfully!");
      setEvents((prevEvents) => prevEvents.filter((event) => event.id !== eventId));
    } catch (error) {
      alert(`Error: ${error}`);
    }
  };

  React.useEffect(() => {
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
    fetchEvents();
  }, []);

  const columns: ColumnDef<Event>[] = [   
    {
      accessorKey: "id",
      header: "ID",
      cell: ({ row }) => (<div>{row.getValue("id")}</div>)
    },
    {
      accessorKey: "title",
      header: ({ column }) => {
        return (
          <Button variant="ghost" onClick={() => column.toggleSorting(column.getIsSorted() === "asc")}>
            Title<ArrowUpDown />
          </Button>
        )
      },
      cell: ({ row }) => <div className="lowercase">{row.getValue("title")}</div>
    },
    {
      accessorKey: "description",
      header: "Description",
      cell: ({ row }) => <div>{row.getValue("description")}</div>
    },
    {
        accessorKey: "startTime",
        header: "Start Time",
        cell: ({ row }) => <div>{row.getValue("startTime")}</div>
    },
    {
        accessorKey: "endTime",
        header: "End Time",
        cell: ({ row }) => <div>{row.getValue("endTime")}</div>
    },
    {
        accessorKey: "location",
        header: "Location",
        cell: ({ row }) => <div>{row.getValue("location")}</div>
    },
    {
        accessorKey: "approval",
        header: "Approval",
        cell: ({ row }) => <div>{row.getValue("approval") ? <CheckCircle className="text-lime-400"/> : <XCircle className="text-red-400"/>}</div>
    },
    {
      id: "actions",
      enableHiding: false,
      cell: ({ row }) => {
        const event = row.original;
    
        const handleJoinEvent = async () => {
          try {
            const userId = localStorage.getItem("userId");
            if (!userId) {
              alert("User not logged in.");
              return;
            }
    
            const response = await fetch("http://localhost:5001/api/eventattendance/join", {
              method: "POST",
              headers: {
                "Content-Type": "application/json",
              },
              body: JSON.stringify({
                EventId: event.id,
                UserId: userId,
              }),
            });
    
            if (!response.ok) {
              throw new Error("Failed to join the event.");
            }
    
            alert("Successfully joined the event!");
          } catch (error) {
            alert(`Error: ${(error as Error).message}`);
          }
        };
    
        return (
          <DropdownMenu>
            <DropdownMenuTrigger asChild>
              <Button variant="ghost" className="h-8 w-8 p-0">
                <span className="sr-only">Open menu</span>
                <MoreHorizontal />
              </Button>
            </DropdownMenuTrigger>
            <DropdownMenuContent align="end">
              <DropdownMenuLabel>Actions</DropdownMenuLabel>
              <DropdownMenuItem onClick={() => navigator.clipboard.writeText(event.id)}>Copy ID</DropdownMenuItem>
              <DropdownMenuSeparator />
              <DropdownMenuItem onClick={handleJoinEvent}>
                <CheckCircle className="text-green-400" /> Join Event
              </DropdownMenuItem>
              <DropdownMenuItem onClick={() => handleDeleteEvent(event.id)}>
                <Trash2 className="text-red-400" /> Delete Event
              </DropdownMenuItem>
            </DropdownMenuContent>
          </DropdownMenu>
        );
      },
    }    
  ]

  const table = useReactTable({
    data: events,
    columns,
    onSortingChange: setSorting,
    onColumnFiltersChange: setColumnFilters,
    getCoreRowModel: getCoreRowModel(),
    getPaginationRowModel: getPaginationRowModel(),
    getSortedRowModel: getSortedRowModel(),
    getFilteredRowModel: getFilteredRowModel(),
    onColumnVisibilityChange: setColumnVisibility,
    onRowSelectionChange: setRowSelection,
    state: {
      sorting,
      columnFilters,
      columnVisibility,
      rowSelection,
    },
  })

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
        <Input placeholder="Filter by title..." value={(table.getColumn("title")?.getFilterValue() as string) ?? ""}
          onChange={(event) => table.getColumn("title")?.setFilterValue(event.target.value)} className="max-w-sm"/>
        
        <Dialog>
          <DialogTrigger asChild>
            <Button>Create Event</Button>
          </DialogTrigger>
          <DialogContent className="sm:max-w-[425px]">
            <DialogHeader>
              <DialogTitle>Create event</DialogTitle>
              <DialogDescription>Make changes to your event here. Click save when you're done.</DialogDescription>
            </DialogHeader>
            <form onSubmit={handleCreateEvent}>
              <div className="grid gap-4 py-4">

                <div className="grid grid-cols-1 items-center gap-4">
                  <Label htmlFor="title">Title</Label>
                  <Input id="title" placeholder="Your title"
                    value={title} onChange={(e) => setTitle(e.target.value)} required/>
                </div>

                <div className="grid grid-cols-1 items-center gap-4">
                  <Label htmlFor="description">Description</Label>
                  <Textarea id="description" placeholder="Type your message here."
                    value={description} onChange={(e) => setDescription(e.target.value)} required/>
                </div>

                <div className="grid grid-cols-1 items-center gap-4">
                  <Label htmlFor="starttime">Start Time</Label>
                  <Input id="starttime" placeholder="2024-12-01T12:00:00" 
                    value={startTime} onChange={(e) => setStartTime(e.target.value)} required/>
                </div>

                <div className="grid grid-cols-1 items-center gap-4">
                  <Label htmlFor="endtime">End Time</Label>
                  <Input id="endtime" placeholder="2024-12-01T14:00:00"
                    value={endTime} onChange={(e) => setEndTime(e.target.value)} required/>
                </div>

                <div className="grid grid-cols-1 items-center gap-4">
                  <Label htmlFor="location">Location</Label>
                  <Input id="location" placeholder="Amsterdam" value={location}
                    onChange={(e) => setLocation(e.target.value)} required/>
                </div>

                <div className="flex items-center space-x-2">
                  <Checkbox id="approval" checked={approval} onCheckedChange={(value: boolean) => setApproval(!!value)}/>
                  <label htmlFor="approval" className="text-sm font-medium leading-none">
                    Approval
                  </label>
                </div>
                
              </div>
              <DialogFooter>
                <Button type="submit">Save changes</Button>
              </DialogFooter>
            </form>
          </DialogContent>
        </Dialog>
        
        <Dialog>
          <DialogTrigger asChild>
            <Button>Update Event</Button>
          </DialogTrigger>
          <DialogContent className="sm:max-w-[425px]">
            <DialogHeader>
              <DialogTitle>Update event</DialogTitle>
              <DialogDescription>Make changes to your event here. Click save when you're done.</DialogDescription>
            </DialogHeader>
            <form onSubmit={(handleUpdateEvent)}>
              <div className="grid gap-4 py-4">

                <div className="grid grid-cols-1 items-center gap-4">
                    <Label htmlFor="id">Id</Label>
                    <Input id="id" placeholder="Event Id.."
                      value={id} onChange={(e) => setId(e.target.value)} required/>
                </div>

                <div className="grid grid-cols-1 items-center gap-4">
                  <Label htmlFor="title">Title</Label>
                  <Input id="title" placeholder="Your title"
                    value={title} onChange={(e) => setTitle(e.target.value)} required/>
                </div>

                <div className="grid grid-cols-1 items-center gap-4">
                  <Label htmlFor="description">Description</Label>
                  <Textarea id="description" placeholder="Type your message here."
                    value={description} onChange={(e) => setDescription(e.target.value)} required/>
                </div>

                <div className="grid grid-cols-1 items-center gap-4">
                  <Label htmlFor="starttime">Start Time</Label>
                  <Input id="starttime" placeholder="2024-12-01T12:00:00" 
                    value={startTime} onChange={(e) => setStartTime(e.target.value)} required/>
                </div>

                <div className="grid grid-cols-1 items-center gap-4">
                  <Label htmlFor="endtime">End Time</Label>
                  <Input id="endtime" placeholder="2024-12-01T14:00:00"
                    value={endTime} onChange={(e) => setEndTime(e.target.value)} required/>
                </div>

                <div className="grid grid-cols-1 items-center gap-4">
                  <Label htmlFor="location">Location</Label>
                  <Input id="location" placeholder="Amsterdam" value={location}
                    onChange={(e) => setLocation(e.target.value)} required/>
                </div>

                <div className="flex items-center space-x-2">
                  <Checkbox id="approval" checked={approval} onCheckedChange={(value: boolean) => setApproval(!!value)}/>
                  <label htmlFor="approval" className="text-sm font-medium leading-none">
                    Approval
                  </label>
                </div>
                
              </div>
              <DialogFooter>
                <Button type="submit">Save changes</Button>
              </DialogFooter>
            </form>
          </DialogContent>
        </Dialog>

        <DropdownMenu>
          <DropdownMenuTrigger asChild>
            <Button variant="outline" className="ml-auto">Columns<ChevronDown /></Button>
          </DropdownMenuTrigger>
          <DropdownMenuContent align="end">
            {table.getAllColumns().filter((column) => column.getCanHide()).map((column) => {
                return (<DropdownMenuCheckboxItem key={column.id} className="capitalize"
                    checked={column.getIsVisible()} onCheckedChange={(value: boolean) => column.toggleVisibility(!!value)}>{column.id}</DropdownMenuCheckboxItem>)
              })
            }
          </DropdownMenuContent>
        </DropdownMenu>
      </div>
      <div className="rounded-md border">
        <Table>
          <TableHeader>
            {table.getHeaderGroups().map((headerGroup) => (<TableRow key={headerGroup.id}>
                {headerGroup.headers.map((header) => {
                  return (
                    <TableHead key={header.id}>
                        {header.isPlaceholder ? null : flexRender(header.column.columnDef.header, header.getContext())}
                    </TableHead>
                  )
                })}
              </TableRow>
            ))}
          </TableHeader>
          <TableBody>
            {table.getRowModel().rows?.length ? (
              table.getRowModel().rows.map((row) => (
                <TableRow key={row.id} data-state={row.getIsSelected() && "selected"}>
                  {row.getVisibleCells().map((cell) => (
                    <TableCell key={cell.id}>
                      {flexRender(cell.column.columnDef.cell, cell.getContext())}
                    </TableCell>
                  ))}
                </TableRow>
              ))
            ) : (
              <TableRow>
                <TableCell colSpan={columns.length} className="h-24 text-center">No results.</TableCell>
              </TableRow>
            )}
          </TableBody>
        </Table>
      </div>
      <div className="flex items-center justify-end space-x-2 py-4">
        
        <div className="flex-1 text-sm text-muted-foreground">
          {table.getFilteredSelectedRowModel().rows.length} of{" "}
          {table.getFilteredRowModel().rows.length} row(s) selected.
        </div>

        <div className="space-x-2">
          <Button variant="outline" onClick={() => table.previousPage()} disabled={!table.getCanPreviousPage()}>Previous</Button>
          <Button variant="outline" onClick={() => table.nextPage()} disabled={!table.getCanNextPage()}>Next</Button>
        </div>

      </div>
    </div>
  );
};

export default EventsDashboard;