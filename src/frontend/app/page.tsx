import { DashboardHeader } from "@/components/dashboard-header"
import { DashboardSidebar } from "@/components/dashboard-sidebar"
import { StatsCards } from "@/components/stats-cards"
// import { RecentMessages } from "@/components/recent-messages"
import { AuditLogs } from "@/components/audit-logs"
import { TransactionList } from "@/components/transaction-list"
import { SecurityStatus } from "@/components/security-status"

export default function HomePage() {
  return (
    <div className="flex min-h-screen bg-background">
      <DashboardSidebar />
      <div className="flex-1 flex flex-col">
        <DashboardHeader />
        <main className="flex-1 p-6 space-y-6 overflow-auto">
          <div>
            <h1 className="text-2xl font-semibold text-foreground">Tableau de bord</h1>
            <p className="text-muted-foreground mt-1">Vue d'ensemble de votre messagerie vidéo sécurisée</p>
          </div>

          <StatsCards />

          <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
            {/* <RecentMessages /> */}
            {/* <SecurityStatus /> */}
          </div>
          <TransactionList />
            <AuditLogs />
        </main>
      </div>
    </div>
  )
}
