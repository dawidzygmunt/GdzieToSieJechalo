import { create } from 'zustand';
import { persist, createJSONStorage } from 'zustand/middleware';
import { jwtDecode } from 'jwt-decode';
import type { User, JwtPayload } from '@/types/auth.types';

interface AuthState {
  user: User | null;
  accessToken: string | null;
  refreshToken: string | null;
  isAuthenticated: boolean;
}

interface AuthActions {
  setTokens: (accessToken: string, refreshToken: string) => void;
  clearAuth: () => void;
  hasRole: (role: string) => boolean;
  hasAnyRole: (roles: string[]) => boolean;
  isTokenExpired: () => boolean;
}

type AuthStore = AuthState & AuthActions;

const initialState: AuthState = {
  user: null,
  accessToken: null,
  refreshToken: null,
  isAuthenticated: false,
};

export const useAuthStore = create<AuthStore>()(
  persist(
    (set, get) => ({
      ...initialState,

      setTokens: (accessToken: string, refreshToken: string) => {
        try {
          const decoded = jwtDecode<JwtPayload>(accessToken);

          // .NET uses ClaimTypes.Role which maps to this long key
          const roleClaimKey = 'http://schemas.microsoft.com/ws/2008/06/identity/claims/role';
          const roleValue = decoded[roleClaimKey] || decoded.role;
          const roles = Array.isArray(roleValue) ? roleValue : roleValue ? [roleValue] : [];

          console.log('[AuthStore] setTokens called', { decoded, roleValue, roles });

          set({
            accessToken,
            refreshToken,
            isAuthenticated: true,
            user: {
              id: parseInt(decoded.sub, 10),
              email: decoded.email,
              roles,
            },
          });

          console.log('[AuthStore] State after setTokens:', get());
        } catch (error) {
          console.error('[AuthStore] Error in setTokens:', error);
          set(initialState);
        }
      },

      clearAuth: () => {
        set(initialState);
      },

      hasRole: (role: string) => {
        const { user } = get();
        return user?.roles.includes(role) ?? false;
      },

      hasAnyRole: (roles: string[]) => {
        const { user } = get();
        return roles.some((role) => user?.roles.includes(role)) ?? false;
      },

      isTokenExpired: () => {
        const { accessToken } = get();
        if (!accessToken) return true;

        try {
          const decoded = jwtDecode<JwtPayload>(accessToken);
          const now = Date.now() / 1000;
          return decoded.exp < now;
        } catch {
          return true;
        }
      },
    }),
    {
      name: 'auth-storage',
      storage: createJSONStorage(() => localStorage),
    }
  )
);
