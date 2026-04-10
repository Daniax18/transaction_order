import api from "@/lib/axios";
import { getUserId } from "@/lib/utils/jwt.utils";
import type { TransactionRequestDto, TransactionResponseDto } from "@/lib/types/transaction";

export const transactionService = {
  /**
   * Crée une nouvelle transaction
   * @param request - Données de la transaction (sans owner_id qui sera extrait du token)
   */
  async createTransaction(dto: TransactionRequestDto): Promise<string> {
    const formData = new FormData();
    const ownerId = getUserId();
    if (!ownerId) { throw new Error("User ID not found in token"); }
    formData.append("OwnerId", ownerId.toString());

    Object.entries(dto).forEach(([key, value]) => {
      formData.append(key, value as any);
    });

    const response = await api.post<string>("/transaction", formData);

    return response.data;
  },

  /**
   * Récupère les transactions créées par l'utilisateur
   */
  async getCreatedTransactions(): Promise<TransactionResponseDto[]> {
    const userId = getUserId();
    if (!userId) {
      throw new Error("User ID not found in token");
    }

    const response = await api.get<TransactionResponseDto[]>("/transaction/all", {
      params: {
        isOwner: true,
        userId: userId,
      },
    });

    return response.data;
  },

  /**
   * Récupère les transactions à vérifier par l'utilisateur
   */
  async getTransactionsToVerify(): Promise<TransactionResponseDto[]> {
    const userId = getUserId();
    if (!userId) {
      throw new Error("User ID not found in token");
    }

    const response = await api.get<TransactionResponseDto[]>("/transaction/all", {
      params: {
        isOwner: false,
        userId: userId,
      },
    });

    return response.data;
  },

  /**
   * Récupère le stream vidéo en tant que Blob
   */
  async getVideoStream(objectName: string): Promise<Blob> {
    const response = await api.get(`/transaction/videos/${objectName}`, {
      responseType: "blob",
    });

    return response.data;
  },

  /**
   * Vérifie une transaction avec la clé publique de l'expéditeur
   */
  async verifyTransaction(
    transactionId: number,
    publicKey: string
  ): Promise<boolean> {
    const response = await api.post<boolean>("/transaction/verify", {
      transactionId,
      publicKey,
    });

    return response.data;
  },
};
