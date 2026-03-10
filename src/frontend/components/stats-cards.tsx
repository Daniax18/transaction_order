import { Card, CardContent } from "@/components/ui/card"
import { Video, Users, Shield, Clock } from "lucide-react"

const stats = [
  {
    name: "Messages Vidéo",
    value: "1,284",
    change: "+12%",
    changeType: "positive",
    icon: Video,
  },
  {
    name: "Signatures Vérifiées",
    value: "99.8%",
    change: "+0.2%",
    changeType: "positive",
    icon: Shield,
  },
]

export function StatsCards() {
  return (
    <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-4">
      {stats.map((stat) => (
        <Card key={stat.name} className="bg-card border-border">
          <CardContent className="p-6">
            <div className="flex items-center justify-between">
              <div className="flex items-center justify-center w-10 h-10 rounded-lg bg-emerald-500/10">
                <stat.icon className="w-5 h-5 text-emerald-500" />
              </div>
              <span
                className={`text-sm font-medium ${
                  stat.changeType === "positive" ? "text-emerald-500" : "text-destructive"
                }`}
              >
                {stat.change}
              </span>
            </div>
            <div className="mt-4">
              <p className="text-2xl font-semibold text-foreground">{stat.value}</p>
              <p className="text-sm text-muted-foreground mt-1">{stat.name}</p>
            </div>
          </CardContent>
        </Card>
      ))}
    </div>
  )
}
