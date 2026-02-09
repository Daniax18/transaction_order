"use client"

import type React from "react"

import { useState } from "react"
import { Eye, EyeOff, Copy, RefreshCw, CheckCircle } from "lucide-react"
import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle,
} from "@/components/ui/dialog"
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select"
import { Checkbox } from "@/components/ui/checkbox"
import { Alert, AlertDescription } from "@/components/ui/alert"

interface CreateUserDialogProps {
  open: boolean
  onOpenChange: (open: boolean) => void
}

function generateTempPassword(): string {
  const chars = "ABCDEFGHJKMNPQRSTUVWXYZabcdefghjkmnpqrstuvwxyz23456789!@#$%"
  let password = ""
  for (let i = 0; i < 16; i++) {
    password += chars.charAt(Math.floor(Math.random() * chars.length))
  }
  return password
}

export function CreateUserDialog({ open, onOpenChange }: CreateUserDialogProps) {
  const [isLoading, setIsLoading] = useState(false)
  const [success, setSuccess] = useState(false)
  const [showPassword, setShowPassword] = useState(false)
  const [copied, setCopied] = useState(false)

  const [formData, setFormData] = useState({
    name: "",
    lastName: "",
    email: "",
    role: "",
    tempPassword: generateTempPassword(),
    sendEmail: true,
    requireMfa: true,
  })

  const regeneratePassword = () => {
    setFormData({ ...formData, tempPassword: generateTempPassword() })
  }

  const copyPassword = async () => {
    await navigator.clipboard.writeText(formData.tempPassword)
    setCopied(true)
    setTimeout(() => setCopied(false), 2000)
  }

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault()
    setIsLoading(true)

    await new Promise((resolve) => setTimeout(resolve, 1500))

    setSuccess(true)
    setIsLoading(false)

    setTimeout(() => {
      onOpenChange(false)
      setSuccess(false)
      setFormData({
        name: "",
        lastName: "",
        email: "",
        role: "",
        tempPassword: generateTempPassword(),
        sendEmail: true,
        requireMfa: true,
      })
    }, 2000)
  }

  if (success) {
    return (
      <Dialog open={open} onOpenChange={onOpenChange}>
        <DialogContent className="bg-card border-border sm:max-w-md">
          <div className="flex flex-col items-center text-center py-6 space-y-4">
            <div className="w-16 h-16 rounded-full bg-primary/10 flex items-center justify-center">
              <CheckCircle className="w-8 h-8 text-primary" />
            </div>
            <div>
              <h2 className="text-xl font-semibold text-foreground">Utilisateur créé</h2>
              <p className="text-sm text-muted-foreground mt-1">
                {formData.sendEmail ? "Un e-mail d'invitation a été envoyé." : "Le compte a été créé avec succès."}
              </p>
            </div>
          </div>
        </DialogContent>
      </Dialog>
    )
  }

  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent className="bg-card border-border sm:max-w-lg">
        <DialogHeader>
          <DialogTitle>Créer un utilisateur</DialogTitle>
          <DialogDescription>
            Ajoutez un nouvel utilisateur au backoffice. Un mot de passe temporaire sera généré.
          </DialogDescription>
        </DialogHeader>

        <form onSubmit={handleSubmit} className="space-y-4">
          <div className="grid grid-cols-2 gap-4">
            <div className="space-y-2">
              <Label htmlFor="name">Name</Label>
              <Input
                id="name"
                placeholder="Jean"
                value={formData.name}
                onChange={(e) => setFormData({ ...formData, name: e.target.value })}
                required
                className="bg-input border-border"
              />
            </div>
            {/* <div className="space-y-2">
              <Label htmlFor="lastName">Nom</Label>
              <Input
                id="lastName"
                placeholder="Dupont"
                value={formData.lastName}
                onChange={(e) => setFormData({ ...formData, lastName: e.target.value })}
                required
                className="bg-input border-border"
              />
            </div> */}
          </div>

          <div className="space-y-2">
            <Label htmlFor="email">Adresse e-mail</Label>
            <Input
              id="email"
              type="email"
              placeholder="jean.dupont@barbichetz.fr"
              value={formData.email}
              onChange={(e) => setFormData({ ...formData, email: e.target.value })}
              required
              className="bg-input border-border"
            />
          </div>

          <div className="space-y-2">
            <Label htmlFor="role">Rôle</Label>
            <Select value={formData.role} onValueChange={(value) => setFormData({ ...formData, role: value })}>
              <SelectTrigger className="bg-input border-border">
                <SelectValue placeholder="Sélectionner un rôle" />
              </SelectTrigger>
              <SelectContent className="bg-card border-border">
                <SelectItem value="admin">Administrateur</SelectItem>
                <SelectItem value="operator">Opérateur</SelectItem>
                <SelectItem value="auditor">Auditeur</SelectItem>
              </SelectContent>
            </Select>
          </div>

          {/* Mot de passe temporaire */}
          <div className="space-y-2">
            <Label>Mot de passe temporaire</Label>
            <div className="flex gap-2">
              <div className="relative flex-1">
                <Input
                  type={showPassword ? "text" : "password"}
                  value={formData.tempPassword}
                  readOnly
                  className="bg-input border-border pr-10 font-mono text-sm"
                />
                <button
                  type="button"
                  onClick={() => setShowPassword(!showPassword)}
                  className="absolute right-3 top-1/2 -translate-y-1/2 text-muted-foreground hover:text-foreground"
                >
                  {showPassword ? <EyeOff className="w-4 h-4" /> : <Eye className="w-4 h-4" />}
                </button>
              </div>
              {/* <Button type="button" variant="outline" size="icon" onClick={regeneratePassword}>
                <RefreshCw className="w-4 h-4" />
              </Button>
              <Button type="button" variant="outline" size="icon" onClick={copyPassword}>
                {copied ? <CheckCircle className="w-4 h-4 text-primary" /> : <Copy className="w-4 h-4" />}
              </Button> */}
            </div>
            <p className="text-xs text-muted-foreground">
              L'utilisateur devra changer ce mot de passe lors de sa première connexion.
            </p>
          </div>

          {/* Options */}
          {/* <div className="space-y-3 p-3 rounded-lg bg-secondary/50 border border-border">
            <div className="flex items-center gap-3">
              <Checkbox
                id="sendEmail"
                checked={formData.sendEmail}
                onCheckedChange={(checked) => setFormData({ ...formData, sendEmail: checked as boolean })}
              />
              <Label htmlFor="sendEmail" className="text-sm font-normal cursor-pointer">
                Envoyer un e-mail d'invitation avec les identifiants
              </Label>
            </div>
            <div className="flex items-center gap-3">
              <Checkbox
                id="requireMfa"
                checked={formData.requireMfa}
                onCheckedChange={(checked) => setFormData({ ...formData, requireMfa: checked as boolean })}
              />
              <Label htmlFor="requireMfa" className="text-sm font-normal cursor-pointer">
                Exiger l'activation de l'authentification à deux facteurs (MFA)
              </Label>
            </div>
          </div> */}

          {/* <Alert className="bg-primary/5 border-primary/20">
            <AlertDescription className="text-sm text-muted-foreground">
              L'utilisateur recevra un lien de connexion valide 24 heures. Il devra définir un nouveau mot de passe et
              configurer la MFA.
            </AlertDescription>
          </Alert> */}

          <DialogFooter>
            <Button type="button" variant="outline" onClick={() => onOpenChange(false)}>
              Annuler
            </Button>
            <Button type="submit" disabled={isLoading || !formData.role}>
              {isLoading ? "Création en cours..." : "Créer l'utilisateur"}
            </Button>
          </DialogFooter>
        </form>
      </DialogContent>
    </Dialog>
  )
}
