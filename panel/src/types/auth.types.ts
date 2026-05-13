export interface LoginRequest {
  Email: string;
  Haslo: string;
}

export interface AuthResponse {
  accessToken: string;
  refreshToken: string;
}

export interface RefreshTokenRequest {
  RefreshToken: string;
}

export interface User {
  id: number;
  email: string;
  roles: string[];
}

export interface JwtPayload {
  sub: string;
  email: string;
  'http://schemas.microsoft.com/ws/2008/06/identity/claims/role'?: string | string[];
  role?: string | string[];
  exp: number;
  iat: number;
}
