export interface TransactionRequestDto {
  ReceiverId: number;
  Amount: number;
  Validity: number;
  VideoHash: string;
  VideoSignature: string;
  SignatureKeyId: number;
  PublicKey: string;
  MediaFile: File;
}

export interface TransactionResponseDto {
  transactionId: number;
  date: string;
  userName: string;
  amount: number;
  objectName: string;
  status: "PENDING" | "VERIFIED";
  publicKey: string;
}
