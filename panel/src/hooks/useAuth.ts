import { useMutation } from '@tanstack/react-query';
import { useNavigate, useLocation } from 'react-router-dom';
import { authApi } from '@/api/auth.api';
import { useAuthStore } from '@/store/authStore';
import type { LoginRequest } from '@/types/auth.types';
import type { AxiosError } from 'axios';
import type { ApiError } from '@/types/api.types';

export function useAuth() {
  const navigate = useNavigate();
  const location = useLocation();
  const { setTokens, clearAuth, user, isAuthenticated, hasRole, hasAnyRole } = useAuthStore();

  const loginMutation = useMutation({
    mutationFn: (data: LoginRequest) => authApi.login(data),
    onSuccess: (data) => {
      console.log('[useAuth] Login successful, data:', data);
      setTokens(data.accessToken, data.refreshToken);

      // Redirect to original destination or dashboard
      const from = (location.state as { from?: Location })?.from?.pathname || '/';
      console.log('[useAuth] Navigating to:', from);
      navigate(from, { replace: true });
    },
    onError: (error) => {
      console.error('[useAuth] Login error:', error);
    },
  });

  const logout = () => {
    clearAuth();
    navigate('/login');
  };

  const getLoginError = (): string | null => {
    if (!loginMutation.error) return null;

    const error = loginMutation.error as AxiosError<ApiError>;

    if (error.response?.data?.errors) {
      const errors = error.response.data.errors;
      const firstError = Object.values(errors)[0];
      if (firstError && firstError.length > 0) {
        return firstError[0];
      }
    }

    if (error.response?.data?.title) {
      return error.response.data.title;
    }

    if (error.response?.status === 400) {
      return 'Nieprawidłowy email lub hasło';
    }

    return 'Wystąpił błąd podczas logowania';
  };

  return {
    login: loginMutation.mutate,
    logout,
    isLoggingIn: loginMutation.isPending,
    loginError: getLoginError(),
    user,
    isAuthenticated,
    hasRole,
    hasAnyRole,
  };
}
