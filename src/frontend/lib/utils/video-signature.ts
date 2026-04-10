/**
 * video-signature.ts
 * Utilitaires de signature vidéo côté client (WebCrypto API – aucune dépendance externe)
 */

// ---------------------------------------------------------------------------
// Lecture de fichiers
// ---------------------------------------------------------------------------

export function readFileAsArrayBuffer(file: File): Promise<ArrayBuffer> {
  return new Promise((resolve, reject) => {
    const reader = new FileReader()
    reader.onload = () => resolve(reader.result as ArrayBuffer)
    reader.onerror = () => reject(reader.error)
    reader.readAsArrayBuffer(file)
  })
}

export function readFileAsText(file: File): Promise<string> {
  return new Promise((resolve, reject) => {
    const reader = new FileReader()
    reader.onload = () => resolve(reader.result as string)
    reader.onerror = () => reject(reader.error)
    reader.readAsText(file)
  })
}

// ---------------------------------------------------------------------------
// PEM → CryptoKey
// ---------------------------------------------------------------------------

/** Strip les headers PEM et décode le Base64 PKCS#8 en ArrayBuffer */
function pemToArrayBuffer(pem: string): ArrayBuffer {
  const b64 = pem
    .replace(/-----BEGIN PRIVATE KEY-----/, "")
    .replace(/-----END PRIVATE KEY-----/, "")
    .replace(/-----BEGIN RSA PRIVATE KEY-----/, "")
    .replace(/-----END RSA PRIVATE KEY-----/, "")
    .replace(/\s+/g, "")

  const binary = atob(b64)
  const bytes = new Uint8Array(binary.length)
  for (let i = 0; i < binary.length; i++) bytes[i] = binary.charCodeAt(i)
  return bytes.buffer
}

/** Importe une clé privée RSA-PKCS#8 depuis un PEM */
export async function importPrivateKey(pem: string): Promise<CryptoKey> {
  return crypto.subtle.importKey(
    "pkcs8",
    pemToArrayBuffer(pem),
    { name: "RSASSA-PKCS1-v1_5", hash: { name: "SHA-256" } },
    false,
    ["sign"]
  )
}

// ---------------------------------------------------------------------------
// Hash + Signature
// ---------------------------------------------------------------------------

export interface VideoSignatureResult {
  /** SHA-256 du contenu vidéo, encodé en Base64 */
  videoHash: string
  /** Signature RSA du hash, encodée en Base64 */
  videoSignature: string
}

/**
 * Hash le blob vidéo en SHA-256 puis signe ce hash avec la clé privée RSA.
 * @param videoBlob  Le blob vidéo enregistré
 * @param privateKeyPem  Contenu texte du fichier .pem
 */
export async function hashAndSignVideo(
  videoBlob: Blob,
  privateKeyPem: string
): Promise<VideoSignatureResult> {
  const videoBuffer = await videoBlob.arrayBuffer()

  const hashBuffer = await crypto.subtle.digest("SHA-256", videoBuffer)
  const videoHash = btoa(String.fromCharCode(...new Uint8Array(hashBuffer)))

  const privateKey = await importPrivateKey(privateKeyPem)
  const signatureBuffer = await crypto.subtle.sign(
    { name: "RSASSA-PKCS1-v1_5" },
    privateKey,
    hashBuffer
  )
  const videoSignature = btoa(String.fromCharCode(...new Uint8Array(signatureBuffer)))

  return { videoHash, videoSignature }
}