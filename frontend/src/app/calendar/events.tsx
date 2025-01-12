import React, { useEffect, useState } from "react";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "../../components/ui/card";
import { Loader2, CheckCircle, XCircle, ArrowUpDown, ChevronDown, MoreHorizontal } from "lucide-react";
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

export const columns: ColumnDef<Event>[] = [   
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
        const payment = row.original

        return (
          <DropdownMenu>
            <DropdownMenuTrigger asChild>
              <Button variant="ghost" className="h-8 w-8 p-0">
                <span className="sr-only">Open menu</span><MoreHorizontal />
              </Button>
            </DropdownMenuTrigger>
            <DropdownMenuContent align="end">
              <DropdownMenuLabel>Actions</DropdownMenuLabel>
              <DropdownMenuItem onClick={() => navigator.clipboard.writeText(payment.id)}>Copy ID</DropdownMenuItem>
              <DropdownMenuSeparator />
              <DropdownMenuItem>View customer</DropdownMenuItem>
              <DropdownMenuItem>View payment details</DropdownMenuItem>
            </DropdownMenuContent>
          </DropdownMenu>
        )
      },
    },
]

const EventsDashboard: React.FC = () => {

  const [events, setEvents] = useState<Event[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [sorting, setSorting] = React.useState<SortingState>([])
  const [columnFilters, setColumnFilters] = React.useState<ColumnFiltersState>([])
  const [columnVisibility, setColumnVisibility] = React.useState<VisibilityState>({})
  const [rowSelection, setRowSelection] = React.useState({})

  const [title, setTitle] = useState("");
  const [description, setDescription] = useState("");
  const [startTime, setStartTime] = useState("");
  const [endTime, setEndTime] = useState("");
  const [location, setLocation] = useState("");
  const [approval, setApproval] = useState(false);

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

      if (response.ok) {
        const contentType = response.headers.get("content-type");
        if (contentType && contentType.includes("application/json")) {
          const createdEvent = await response.json();
          setEvents((prevEvents) => [...prevEvents, createdEvent]);
          alert("Event created successfully!");
        } else alert("Event created successfully!");
      } else throw new Error("Failed to create event.");
    } catch (error) {
      alert(error);
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
                  <Checkbox id="approval" checked={approval} onCheckedChange={(value) => setApproval(!!value)}/>
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
                    checked={column.getIsVisible()} onCheckedChange={(value) => column.toggleVisibility(!!value)}>{column.id}</DropdownMenuCheckboxItem>)
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