import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card"
import { Badge } from "@/components/ui/badge"
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from "@/components/ui/table"
import { Button } from "@/components/ui/button"
import { ChevronRight } from "lucide-react"

const logs = [
  {
    user_id: 1,
    action: "VIDEO_UPLOAD",
    date_action: "2025-01-08 14:32:15",
    ip: "192.168.1.45",
    description: "description log",
  },
  {
    user_id: 2,
    action: "VIDEO_READ",
    date_action: "2025-01-08 14:32:15",
    ip: "192.168.1.67",
    description: "description log",
  },
]

const actionLabels: Record<string, string> = {
  VIDEO_UPLOAD: "Upload Vidéo",
  VIDEO_READ: "Lecture Vidéo",
  KEY_ROTATION: "Rotation Clé",
  AUTH_LOGIN: "Connexion",
  AUTH_FAILED: "Échec Auth",
}

export function AuditLogs() {
  return (
    <Card className="bg-card border-border">
      <CardHeader className="flex flex-row items-center justify-between pb-3">
        <CardTitle className="text-lg font-semibold text-foreground">Journal d'Audit</CardTitle>
        <Button variant="ghost" size="sm" className="text-muted-foreground">
          Voir tout
          <ChevronRight className="ml-1 h-4 w-4" />
        </Button>
      </CardHeader>
      <CardContent>
        <Table>
          <TableHeader>
            <TableRow className="border-border hover:bg-transparent">
              <TableHead className="text-muted-foreground">User ID</TableHead>
              <TableHead className="text-muted-foreground">Action</TableHead>
              <TableHead className="text-muted-foreground">IP</TableHead>
              <TableHead className="text-muted-foreground">Date action</TableHead>
              <TableHead className="text-muted-foreground">Description</TableHead>
            </TableRow>
          </TableHeader>
          <TableBody>
            {logs.map((log) => (
              <TableRow key={log.user_id} className="border-border">
                {/* <TableCell>
                  <Badge
                    variant="outline"
                    className={`font-mono text-xs ${
                      log.action === "AUTH_FAILED"
                        ? "bg-destructive/10 text-destructive border-destructive/20"
                        : "bg-secondary text-foreground border-border"
                    }`}
                  >
                    {actionLabels[log.action] || log.action}
                  </Badge>
                </TableCell> */}
                <TableCell className="text-sm text-foreground">{log.user_id}</TableCell>
                <TableCell className="text-sm text-muted-foreground font-mono">{log.action}</TableCell>
                <TableCell className="text-sm text-muted-foreground font-mono">{log.ip}</TableCell>
                <TableCell className="text-sm text-muted-foreground">{log.date_action}</TableCell>
                <TableCell className="text-sm text-muted-foreground">{log.description}</TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </CardContent>
    </Card>
  )
}
