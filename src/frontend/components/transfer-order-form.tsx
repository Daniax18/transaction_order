"use client"

import { useState } from "react"
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { Button } from "@/components/ui/button"
import { Textarea } from "@/components/ui/textarea"
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select"
import { Separator } from "@/components/ui/separator"
import { Badge } from "@/components/ui/badge"
import {
  Shield,
  Video,
  Send,
  AlertTriangle,
  CheckCircle2,
  Euro,
  Building2,
  FileText,
  Lock,
  Clock,
  Fingerprint,
} from "lucide-react"

export function TransferOrderForm() {
  const [step, setStep] = useState(1)
  const [amount, setAmount] = useState("")
  const [text, setText] = useState("")
  const [isRecording, setIsRecording] = useState(false)

  const formatAmount = (value: string) => {
    const num = value.replace(/\D/g, "")
    if (num) {
      return new Intl.NumberFormat("fr-FR").format(Number.parseInt(num))
    }
    return ""
  }

  return (
    <div className="grid grid-cols-1 xl:grid-cols-3 gap-6">
      {/* Formulaire principal */}
      <div className="xl:col-span-2 space-y-6">
        {/* Étapes */}
        <div className="flex items-center gap-4">
          {[
            { num: 1, label: "Informations" },
            { num: 2, label: "Validation vidéo" },
            { num: 3, label: "Insertion clé privé" },
          ].map((s, idx) => (
            <div key={s.num} className="flex items-center gap-3">
              <div
                className={`flex items-center justify-center w-8 h-8 rounded-full text-sm font-medium ${
                  step >= s.num ? "bg-primary text-primary-foreground" : "bg-muted text-muted-foreground"
                }`}
              >
                {step > s.num ? <CheckCircle2 className="w-4 h-4" /> : s.num}
              </div>
              <span className={`text-sm font-medium ${step >= s.num ? "text-foreground" : "text-muted-foreground"}`}>
                {s.label}
              </span>
              {idx < 2 && <div className="w-12 h-px bg-border" />}
            </div>
          ))}
        </div>

        {/* Étape 1 - Informations du virement */}
        {step === 1 && (
          <Card className="bg-card border-border">
            <CardHeader>
              <CardTitle className="flex items-center gap-2 text-card-foreground">
                Informations du virement
              </CardTitle>
              <CardDescription>Saisissez les détails de l'ordre de virement</CardDescription>
            </CardHeader>
            <CardContent className="space-y-6">
              <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
                <div className="space-y-2">
                  <Label htmlFor="amount">Montant</Label>
                  <div className="relative">
                    <Input
                      id="amount"
                      placeholder="0"
                      value={amount}
                      onChange={(e) => setAmount(formatAmount(e.target.value))}
                      className="pr-12 text-lg font-semibold bg-input border-border"
                    />
                    <span className="absolute right-3 top-1/2 -translate-y-1/2 text-muted-foreground font-medium">
                      RS
                    </span>
                  </div>
                </div>
              </div>

              <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
                <div className="space-y-2">
                  <Label htmlFor="debit-account">Compte à débiter</Label>
                  <Select>
                    <SelectTrigger className="bg-input border-border">
                      <SelectValue placeholder="Sélectionner un compte" />
                    </SelectTrigger>
                    <SelectContent>
                      <SelectItem value="main">Francis</SelectItem>
                      <SelectItem value="secondary">Alice</SelectItem>
                    </SelectContent>
                  </Select>
                </div>

              </div>

              <div className="flex justify-between">
                <Button variant="outline" onClick={() => setStep(1)} className="gap-2">
                  Retour
                </Button>
                <Button onClick={() => setStep(2)} className="gap-2">
                  Continuer vers la validation
                  <Video className="w-4 h-4" />
                </Button>
              </div>
            </CardContent>
          </Card>
        )}

        {/* Étape 2 - Validation vidéo */}
        {step === 2 && (
          <Card className="bg-card border-border">
            <CardHeader>
              <CardTitle className="flex items-center gap-2 text-card-foreground">
                <Video className="w-5 h-5 text-primary" />
                Authentification vidéo
              </CardTitle>
              <CardDescription>Enregistrez une confirmation vidéo pour valider l'ordre de virement</CardDescription>
            </CardHeader>
            <CardContent className="space-y-6">
              {/* Zone vidéo */}
              <div className="relative aspect-video bg-secondary rounded-lg overflow-hidden border border-border">
                <div className="absolute inset-0 flex flex-col items-center justify-center gap-4">
                  {!isRecording ? (
                    <>
                      <div className="w-20 h-20 rounded-full bg-primary/10 flex items-center justify-center">
                        <Video className="w-10 h-10 text-primary" />
                      </div>
                      <p className="text-muted-foreground text-center max-w-md">
                        Cliquez sur "Démarrer l'enregistrement" pour confirmer vocalement votre ordre de virement
                      </p>
                    </>
                  ) : (
                    <>
                      <div className="w-20 h-20 rounded-full bg-destructive/20 flex items-center justify-center animate-pulse">
                        <div className="w-4 h-4 rounded-full bg-destructive" />
                      </div>
                      <p className="text-foreground font-medium">Enregistrement en cours...</p>
                      <p className="text-muted-foreground text-sm">00:12 / 00:30</p>
                    </>
                  )}
                </div>

                {/* Indicateurs sécurité */}
                <div className="absolute top-4 left-4 flex items-center gap-2">
                  <Badge variant="outline" className="bg-card/80 backdrop-blur gap-1.5">
                    <Lock className="w-3 h-3" />
                    E2EE Actif
                  </Badge>
                </div>
                <div className="absolute top-4 right-4 flex items-center gap-2">
                  <Badge variant="outline" className="bg-card/80 backdrop-blur gap-1.5">
                    <Shield className="w-3 h-3 text-primary" />
                    RSA-PSS
                  </Badge>
                </div>
              </div>

              {/* Boutons */}
              <div className="flex flex-col sm:flex-row gap-3">
                <Button
                  variant={isRecording ? "destructive" : "default"}
                  className="flex-1 gap-2"
                  onClick={() => setIsRecording(!isRecording)}
                >
                  {isRecording ? (
                    <>
                      <div className="w-3 h-3 rounded-full bg-white" />
                      Arrêter l'enregistrement
                    </>
                  ) : (
                    <>
                      <Video className="w-4 h-4" />
                      Démarrer l'enregistrement
                    </>
                  )}
                </Button>
              </div>

              <Separator className="bg-border" />

              <div className="flex justify-between">
                <Button variant="outline" onClick={() => setStep(1)} className="gap-2">
                  Retour
                </Button>
                <Button className="gap-2" disabled={!isRecording} onClick={() => setStep(3)}> 
                  <Send className="w-4 h-4" />
                  Continuer vers l'insertion clé privé
                </Button>
              </div>
            </CardContent>
          </Card>
        )}

        {/* Étape 3 - Insertion clé privé */}
        {step === 3 && (
          <Card className="bg-card border-border">
            <CardHeader>
              <CardTitle className="flex items-center gap-2 text-card-foreground">
                Insertion de clé privée
              </CardTitle>
            </CardHeader>
            <CardContent className="space-y-6">
              <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
                <div className="space-y-2">
                  <Label htmlFor="amount">Clé privée</Label>
                  <div className="relative">
                    <Input
                      id="text"
                      placeholder="Entrez votre clé privée"
                      value={text}
                      onChange={(e) => setText(e.target.value)}
                      className="pr-12 text-lg font-semibold bg-input border-border"
                    />
                  </div>
                </div>
              </div> 

              <div className="flex justify-between">
                <Button variant="outline" onClick={() => setStep(2)} className="gap-2">
                  Retour
                </Button>
                <Button className="gap-2">
                 Valider 
                </Button>
              </div>
            </CardContent>
          </Card>
        )}
      </div>
    </div>
  )
}
