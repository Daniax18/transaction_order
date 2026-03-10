"use client"
import {
  MoreHorizontal,
  Mail,
  Key,
  Trash2,
  Shield,
  ShieldCheck,
  ShieldAlert,
  Clock,
  CheckCircle,
  XCircle,
} from "lucide-react"
import { Button } from "@/components/ui/button"
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card"
import { Badge } from "@/components/ui/badge"
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuSeparator,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu"
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from "@/components/ui/table"
import { Avatar, AvatarFallback } from "@/components/ui/avatar"
import { cn } from "@/lib/utils"

interface UsersListProps {
  searchQuery: string
}

const users = [
  {
    id: "1",
    name: "Jean Dupont",
    email: "jean.dupont@barbichetz.fr",
    level: "Comptable",
  },
  {
    id: "2",
    name: "Marie Martin",
    email: "marie.martin@barbichetz.fr",
    level: "Assistante",
  },
]

const roleIcons = {
  Administrateur: ShieldCheck,
  Comptable: Shield,
  Assistante: ShieldAlert,
}

const statusConfig = {
  active: { label: "Actif", variant: "default" as const, className: "bg-primary/10 text-primary border-primary/20" },
  pending: {
    label: "En attente",
    variant: "secondary" as const,
    className: "bg-yellow-500/10 text-yellow-500 border-yellow-500/20",
  },
  inactive: {
    label: "Inactif",
    variant: "secondary" as const,
    className: "bg-muted text-muted-foreground border-muted",
  },
}

export function UsersList({ searchQuery }: UsersListProps) {
  const filteredUsers = users.filter(
    (user) =>
      user.name.toLowerCase().includes(searchQuery.toLowerCase()) ||
      user.email.toLowerCase().includes(searchQuery.toLowerCase()),
  )

  return (
    <Card className="bg-card border-border">
      <CardHeader>
        <CardTitle className="text-lg">Utilisateurs ({filteredUsers.length})</CardTitle>
      </CardHeader>
      <CardContent>
        <Table>
          <TableHeader>
            <TableRow className="border-border hover:bg-transparent">
              <TableHead>Utilisateur</TableHead>
              <TableHead className="w-12"></TableHead>
            </TableRow>
          </TableHeader>
          <TableBody>
            {filteredUsers.map((user) => {
              const RoleIcon = roleIcons[user.level as keyof typeof roleIcons] || Shield

              return (
                <TableRow key={user.id} className="border-border">
                  <TableCell>
                    <div className="flex items-center gap-3">
                      <Avatar className="h-9 w-9">
                        <AvatarFallback className="bg-secondary text-foreground text-xs">
                          {user.name
                            .split(" ")
                            .map((n) => n[0])
                            .join("")}
                        </AvatarFallback>
                      </Avatar>
                      <div>
                        <p className="font-medium text-foreground">{user.name}</p>
                        <p className="text-xs text-muted-foreground">{user.email}</p>
                      </div>
                    </div>
                  </TableCell>
                  <TableCell>
                    <div className="flex items-center gap-2">
                      <RoleIcon className="w-4 h-4 text-muted-foreground" />
                      <span className="text-sm">{user.level}</span>
                    </div>
                  </TableCell>
                  <TableCell>
                    <DropdownMenu>
                      <DropdownMenuTrigger asChild>
                        <Button variant="ghost" size="icon" className="h-8 w-8">
                          <MoreHorizontal className="w-4 h-4" />
                        </Button>
                      </DropdownMenuTrigger>
                      <DropdownMenuContent align="end" className="bg-card border-border">
                        <DropdownMenuItem>
                          <Mail className="w-4 h-4 mr-2" />
                          Envoyer un e-mail
                        </DropdownMenuItem>
                        <DropdownMenuItem>
                          <Key className="w-4 h-4 mr-2" />
                          Réinitialiser mot de passe
                        </DropdownMenuItem>
                        <DropdownMenuSeparator className="bg-border" />
                        <DropdownMenuItem className="text-destructive focus:text-destructive">
                          <Trash2 className="w-4 h-4 mr-2" />
                          Désactiver le compte
                        </DropdownMenuItem>
                      </DropdownMenuContent>
                    </DropdownMenu>
                  </TableCell>
                </TableRow>
              )
            })}
          </TableBody>
        </Table>
      </CardContent>
    </Card>
  )
}
