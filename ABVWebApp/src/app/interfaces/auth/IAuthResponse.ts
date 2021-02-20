export interface AuthResponse {
    isAuthSuccessful: boolean;
    message: string;
    accessToken: string;
    refreshToken: string;
}
