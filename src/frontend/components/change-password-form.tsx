"use client"

import type React from "react"

import { useState } from "react"
import { useRouter, useSearchParams } from "next/navigation"
import { Lock, Eye, EyeOff, CheckCircle, XCircle, AlertTriangle, Shield } from "lucide-react"
import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card"
import { Alert, AlertDescription } from "@/components/ui/alert"
import { cn } from "@/lib/utils"

const passwordRules = [
  { id: "length", label: "Au moins 12 caractères", test: (pwd: string) => pwd.length >= 12 },
  { id: "uppercase", label: "Au moins une majuscule", test: (pwd: string) => /[A-Z]/.test(pwd) },
  { id: "lowercase", label: "Au moins une minuscule", test: (pwd: string) => /[a-z]/.test(pwd) },
  { id: "number", label: "Au moins un chiffre", test: (pwd: string) => /[0-9]/.test(pwd) },
  {
    id: "special",
    label: "Au moins un caractère spécial (!@#$%...)",
    test: (pwd: string) => /[!@#$%^&*()_+\-=[\]{};':"\\|,.<>/?]/.test(pwd),
  },
]

export function ChangePasswordForm() {
  const router = useRouter()
  const searchParams = useSearchParams()
  const isFirstLogin = searchParams.get("first") === "true"

  const [showCurrentPassword, setShowCurrentPassword] = useState(false)
  const [showNewPassword, setShowNewPassword] = useState(false)
  const [showConfirmPassword, setShowConfirmPassword] = useState(false)
  const [isLoading, setIsLoading] = useState(false)
  const [success, setSuccess] = useState(false)

  const [formData, setFormData] = useState({
    currentPassword: "",
    newPassword: "",
    confirmPassword: "",
  })

  const passwordChecks = passwordRules.map((rule) => ({
    ...rule,
    passed: rule.test(formData.newPassword),
  }))

  const allRulesPassed = passwordChecks.every((check) => check.passed)
  const passwordsMatch = formData.newPassword === formData.confirmPassword && formData.confirmPassword.length > 0

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault()
    if (!allRulesPassed || !passwordsMatch) return

    setIsLoading(true)
    await new Promise((resolve) => setTimeout(resolve, 1500))
    setSuccess(true)

    setTimeout(() => {
      router.push("/")
    }, 2000)
  }

  if (success) {
    return (
      <div className="min-h-screen flex items-center justify-center bg-background p-4">
        <Card className="w-full max-w-md bg-card border-border">
          <CardContent className="pt-6">
            <div className="flex flex-col items-center text-center space-y-4">
              <div className="w-16 h-16 rounded-full bg-primary/10 flex items-center justify-center">
                <CheckCircle className="w-8 h-8 text-primary" />
              </div>
              <div>
                <h2 className="text-xl font-semibold text-foreground">Mot de passe modifié</h2>
                <p className="text-sm text-muted-foreground mt-1">
                  Votre mot de passe a été mis à jour avec succès. Redirection...
                </p>
              </div>
            </div>
          </CardContent>
        </Card>
      </div>
    )
  }

  return (
    <div className="min-h-screen flex items-center justify-center bg-background p-4">
      <div className="w-full max-w-md space-y-6">
        {/* Logo et titre */}
        <div className="flex flex-col items-center space-y-4">
          <div className="flex items-center justify-center w-16 h-16 rounded-2xl bg-primary/10 border border-primary/20">
            <Lock className="w-8 h-8 text-primary" />
          </div>
          <div className="text-center">
            <h1 className="text-2xl font-semibold text-foreground">Moustass vidéo</h1>
            <p className="text-sm text-muted-foreground mt-1">Messagerie Vidéo Sécurisée</p>
          </div>
        </div>

        <Card className="bg-card border-border">
          <CardHeader className="space-y-1">
            <CardTitle className="text-xl">
              {isFirstLogin ? "Créez votre mot de passe" : "Modifier le mot de passe"}
            </CardTitle>
            <CardDescription>
              {isFirstLogin
                ? "Pour des raisons de sécurité, vous devez définir un nouveau mot de passe lors de votre première connexion."
                : "Choisissez un mot de passe fort et unique pour sécuriser votre compte."}
            </CardDescription>
          </CardHeader>
          <CardContent>
            {isFirstLogin && (
              <Alert className="mb-4 bg-yellow-500/10 border-yellow-500/30">
                <AlertTriangle className="h-4 w-4 text-yellow-500" />
                <AlertDescription className="text-yellow-200">
                  Première connexion détectée. Veuillez créer un mot de passe personnel.
                </AlertDescription>
              </Alert>
            )}

            <form onSubmit={handleSubmit} className="space-y-4">
              {!isFirstLogin && (
                <div className="space-y-2">
                  <Label htmlFor="currentPassword">Mot de passe actuel</Label>
                  <div className="relative">
                    <Input
                      id="currentPassword"
                      type={showCurrentPassword ? "text" : "password"}
                      value={formData.currentPassword}
                      onChange={(e) => setFormData({ ...formData, currentPassword: e.target.value })}
                      required
                      className="bg-input border-border pr-10"
                    />
                    <button
                      type="button"
                      onClick={() => setShowCurrentPassword(!showCurrentPassword)}
                      className="absolute right-3 top-1/2 -translate-y-1/2 text-muted-foreground hover:text-foreground"
                    >
                      {showCurrentPassword ? <EyeOff className="w-4 h-4" /> : <Eye className="w-4 h-4" />}
                    </button>
                  </div>
                </div>
              )}

              <div className="space-y-2">
                <Label htmlFor="newPassword">Nouveau mot de passe</Label>
                <div className="relative">
                  <Input
                    id="newPassword"
                    type={showNewPassword ? "text" : "password"}
                    value={formData.newPassword}
                    onChange={(e) => setFormData({ ...formData, newPassword: e.target.value })}
                    required
                    className="bg-input border-border pr-10"
                  />
                  <button
                    type="button"
                    onClick={() => setShowNewPassword(!showNewPassword)}
                    className="absolute right-3 top-1/2 -translate-y-1/2 text-muted-foreground hover:text-foreground"
                  >
                    {showNewPassword ? <EyeOff className="w-4 h-4" /> : <Eye className="w-4 h-4" />}
                  </button>
                </div>
              </div>

              {/* Règles de mot de passe */}
              <div className="space-y-2 p-3 rounded-lg bg-secondary/50 border border-border">
                <p className="text-xs font-medium text-muted-foreground mb-2">Exigences de sécurité :</p>
                <div className="space-y-1.5">
                  {passwordChecks.map((check) => (
                    <div key={check.id} className="flex items-center gap-2 text-xs">
                      {check.passed ? (
                        <CheckCircle className="w-3.5 h-3.5 text-primary" />
                      ) : (
                        <XCircle className="w-3.5 h-3.5 text-muted-foreground" />
                      )}
                      <span className={cn(check.passed ? "text-foreground" : "text-muted-foreground")}>
                        {check.label}
                      </span>
                    </div>
                  ))}
                </div>
              </div>

              <div className="space-y-2">
                <Label htmlFor="confirmPassword">Confirmer le mot de passe</Label>
                <div className="relative">
                  <Input
                    id="confirmPassword"
                    type={showConfirmPassword ? "text" : "password"}
                    value={formData.confirmPassword}
                    onChange={(e) => setFormData({ ...formData, confirmPassword: e.target.value })}
                    required
                    className={cn(
                      "bg-input border-border pr-10",
                      formData.confirmPassword.length > 0 && (passwordsMatch ? "border-primary" : "border-destructive"),
                    )}
                  />
                  <button
                    type="button"
                    onClick={() => setShowConfirmPassword(!showConfirmPassword)}
                    className="absolute right-3 top-1/2 -translate-y-1/2 text-muted-foreground hover:text-foreground"
                  >
                    {showConfirmPassword ? <EyeOff className="w-4 h-4" /> : <Eye className="w-4 h-4" />}
                  </button>
                </div>
                {formData.confirmPassword.length > 0 && !passwordsMatch && (
                  <p className="text-xs text-destructive">Les mots de passe ne correspondent pas</p>
                )}
              </div>

              <Button type="submit" className="w-full" disabled={isLoading || !allRulesPassed || !passwordsMatch}>
                {isLoading ? "Mise à jour en cours..." : "Valider le mot de passe"}
              </Button>
            </form>
          </CardContent>
        </Card>

        {/* Indicateurs de sécurité */}
        <div className="flex items-center justify-center gap-4 text-xs text-muted-foreground">
          <div className="flex items-center gap-1.5">
            <Shield className="w-3.5 h-3.5 text-primary" />
            <span>Chiffrement RSA-4096</span>
          </div>
          <div className="h-3 w-px bg-border" />
          <div className="flex items-center gap-1.5">
            <Shield className="w-3.5 h-3.5 text-primary" />
            <span>Hashage Argon2id</span>
          </div>
        </div>
      </div>
    </div>
  )
}
