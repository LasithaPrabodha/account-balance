export interface AuthResponse {
    isAuthSuccessful: boolean;
    errorMessage: string;
    accessToken: string;
    refreshToken: string;
}
